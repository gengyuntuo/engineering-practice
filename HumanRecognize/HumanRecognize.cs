using BaiduAIClientResponse;
using System.Text;

namespace HumanRecognize
{
    public partial class FormHumanRecognize : Form
    {
        private string LeftPictureLocation = string.Empty;
        private string RightPictureLocation = string.Empty;
        public FormHumanRecognize()
        {
            InitializeComponent();

            // �󶨰�������¼�
            buttonCompare.Click += ButtonClick;
            buttonLoadLeftPicture.Click += ButtonClick;
            buttonLoadRightPicture.Click += ButtonClick;

            // ͼƬ�ؼ�ͼƬ��ʾģʽ������Ӧ
            pictureBoxLeft.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxRight.SizeMode = PictureBoxSizeMode.Zoom;
        }

        /// <summary>
        /// ��������¼�
        /// </summary>
        /// <param name="sender">�¼�Դ</param>
        /// <param name="e">�¼�����</param>
        private async void ButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            try
            {
                // ���ð�ť
                clickedButton.Enabled = false;
                switch (clickedButton.Name)
                {
                    case "buttonCompare":
                        await ComparePicture();
                        break;
                    case "buttonLoadLeftPicture":
                        LeftPictureLocation = LoadPicture(LeftPictureLocation);
                        if (File.Exists(LeftPictureLocation))
                        {
                            await RenderPicture(pictureBoxLeft, richTextBoxLeft, LeftPictureLocation);
                        }
                        break;
                    case "buttonLoadRightPicture":
                        RightPictureLocation = LoadPicture(RightPictureLocation);
                        if (File.Exists(RightPictureLocation))
                        {
                            await RenderPicture(pictureBoxRight, richTextBoxRight, RightPictureLocation);
                        }
                        break;
                    default: throw new Exception("δ֪����������ϵ������Ա");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("ԭ��: " + exp.Message + "!", "����");
            }
            finally
            {
                // ��ť��Ӧ��ɣ����ð���
                clickedButton.Enabled = true;
            }
        }

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="refPictureLocation">������ͼƬ�ؼ���ԭʼͼƬλ��</param>
        /// <returns>��ͼƬ·������ԭ·��</returns>
        public string LoadPicture(string refPictureLocation)
        {
            string initialDirectory;
            // �����жϵ�ǰͼƬ·���ı������Ƿ�����Ϸ�ͼƬ��������ڣ�����Ϊ���ļ����ڵ�·��
            var currentFilePath = refPictureLocation;
            if (Directory.Exists(currentFilePath))
            {
                initialDirectory = currentFilePath;
            }
            else if (File.Exists(currentFilePath))
            {
                initialDirectory = Path.GetDirectoryName(currentFilePath) ?? AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                initialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            // δѡ����߲�����ЧͼƬ·����ѡ�ó�������Ŀ¼��Ϊ���ļ����ڵ�·��
            openFileDialog.InitialDirectory = initialDirectory;
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return refPictureLocation;
        }

        /// <summary>
        /// �Ա�����ͼƬ�����������ƶ�
        /// </summary>
        /// <returns>None</returns>
        /// <exception cref="Exception">�쳣</exception>
        public async Task ComparePicture()
        {
            if (!File.Exists(LeftPictureLocation))
            {
                throw new Exception("��ߵ�ͼƬ������");
            }
            if (!File.Exists(RightPictureLocation))
            {
                throw new Exception("�ұߵ�ͼƬ������");
            }

            // ����Baidu API�Դ�ͼƬ
            var response = await Task.Run(() =>
            {
                var response = BaiduAIClient.Instance.HumanFaceCompare(LeftPictureLocation, RightPictureLocation);
                return response;
            });

            if (response.errorCode != 0)
            {
                throw new Exception("Baidu API ��Ӧ�쳣");
            }

            // ��Ⱦ����ؼ�
            richTextBoxResult.Text = $"���ƶ�: {response.result.score}";
        }

        /// <summary>
        /// ��ȾͼƬ
        /// </summary>
        /// <param name="pictureBox">ͼƬչʾ�ؼ�</param>
        /// <param name="richTextBoxPictureInfo">ͼƬ����չʾ�ؼ�</param>
        /// <param name="pictureLocation">ͼƬλ��</param>
        /// <returns>None</returns>
        /// <exception cref="Exception">�쳣</exception>
        public async Task RenderPicture(PictureBox pictureBox, RichTextBox richTextBoxPictureInfo, string pictureLocation)
        {
            if (string.Empty.Equals(pictureLocation))
            {
                return;
            }
            if (!File.Exists(pictureLocation))
            {
                throw new Exception("ͼƬ(" + pictureBox + ")������");
            }
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }
            // ����ԭʼͼƬ
            pictureBox.Image = Image.FromFile(pictureLocation);
            // ����Baidu API���ͼƬ
            var response = await Task.Run(() =>
            {
                var response = BaiduAIClient.Instance.HumanFaceDetect(pictureLocation);
                return response;
            });
            if (response.errorCode != 0)
            {
                throw new Exception("Baidu API ��Ӧ�쳣");
            }
            if (response.result.faceNum == 0)
            {
                richTextBoxPictureInfo.Text = "û�м�⵽����";
                return;
            }
            var face = response.result.faceList[0];
            StringBuilder sb = new();
            sb.Append($"�������䣺{face.age}\n");
            sb.Append($"��ò���֣�{Math.Round(face.faceProbability, 2)}\n");
            sb.Append($"���飺{face.emotion.type} ���Ŷȣ�{Math.Round(face.emotion.probability, 2)}\n");
            sb.Append($"���ͣ�{face.faceShape.type} ���Ŷȣ�{Math.Round(face.faceShape.probability, 2)}\n");
            sb.Append($"�Ա�{face.gender.type} ���Ŷȣ�{Math.Round(face.gender.probability, 2)}\n");
            sb.Append($"���֣�{face.faceType.type} ���Ŷȣ�{Math.Round(face.faceType.probability, 2)}\n");
            richTextBoxPictureInfo.Text = sb.ToString();

            // ������ȾͼƬ, ��ʱ�������첽ʵ��
            Bitmap editableImage = new(pictureBox.Image);
            await Task.Run(() =>
            {
                // ����Graphics�������ڻ���
                using (Graphics g = Graphics.FromImage(editableImage))
                {
                    // ���û��ʣ���ɫ�����Ϊ6���أ�
                    using (Pen pen = new(Color.Red, 6))
                    {
                        // ��������
                        g.DrawRectangle(
                            pen,
                            new Rectangle(
                                new Point(((int)face.location.left), ((int)face.location.top)),
                                new Size(face.location.width, face.location.height)
                                )
                            );
                    }

                    // ���û��ʣ���ɫ�����Ϊ15���أ�
                    using (Pen pen = new(Color.Blue, 15))
                    {
                        // ����4���ؼ���λ�ã��������ġ��������ġ��Ǽ⡢�����ġ�
                        var point0 = new Point((int)face.landmark[0].x, (int)face.landmark[0].y);
                        var point1 = new Point((int)face.landmark[1].x, (int)face.landmark[1].y);
                        var point2 = new Point((int)face.landmark[2].x, (int)face.landmark[2].y);
                        var point3 = new Point((int)face.landmark[3].x, (int)face.landmark[3].y);
                        g.DrawEllipse(pen, new(point0, new Size(6, 6)));
                        g.DrawEllipse(pen, new(point1, new Size(6, 6)));
                        g.DrawEllipse(pen, new(point2, new Size(6, 6)));
                        g.DrawEllipse(pen, new(point3, new Size(6, 6)));
                    }
                }
            });
            pictureBox.Image.Dispose();
            pictureBox.Image = editableImage;
        }

    }

}
