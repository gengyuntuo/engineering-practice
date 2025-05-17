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

            // 绑定按键点击事件
            buttonCompare.Click += ButtonClick;
            buttonLoadLeftPicture.Click += ButtonClick;
            buttonLoadRightPicture.Click += ButtonClick;

            // 图片控件图片显示模式：自适应
            pictureBoxLeft.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxRight.SizeMode = PictureBoxSizeMode.Zoom;
        }

        /// <summary>
        /// 按键点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private async void ButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            try
            {
                // 禁用按钮
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
                    default: throw new Exception("未知按键，请联系开发人员");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("原因: " + exp.Message + "!", "错误");
            }
            finally
            {
                // 按钮响应完成，启用按键
                clickedButton.Enabled = true;
            }
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="refPictureLocation">被加载图片控件的原始图片位置</param>
        /// <returns>新图片路径或者原路径</returns>
        public string LoadPicture(string refPictureLocation)
        {
            string initialDirectory;
            // 首先判断当前图片路径文本框中是否包含合法图片，如果存在，则作为打开文件窗口的路径
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

            // 未选择或者不含有效图片路径，选用程序所在目录作为打开文件窗口的路径
            openFileDialog.InitialDirectory = initialDirectory;
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return refPictureLocation;
        }

        /// <summary>
        /// 对比两张图片中人脸的相似度
        /// </summary>
        /// <returns>None</returns>
        /// <exception cref="Exception">异常</exception>
        public async Task ComparePicture()
        {
            if (!File.Exists(LeftPictureLocation))
            {
                throw new Exception("左边的图片不存在");
            }
            if (!File.Exists(RightPictureLocation))
            {
                throw new Exception("右边的图片不存在");
            }

            // 调用Baidu API对此图片
            var response = await Task.Run(() =>
            {
                var response = BaiduAIClient.Instance.HumanFaceCompare(LeftPictureLocation, RightPictureLocation);
                return response;
            });

            if (response.errorCode != 0)
            {
                throw new Exception("Baidu API 响应异常");
            }

            // 渲染窗体控件
            richTextBoxResult.Text = $"相似度: {response.result.score}";
        }

        /// <summary>
        /// 渲染图片
        /// </summary>
        /// <param name="pictureBox">图片展示控件</param>
        /// <param name="richTextBoxPictureInfo">图片属性展示控件</param>
        /// <param name="pictureLocation">图片位置</param>
        /// <returns>None</returns>
        /// <exception cref="Exception">异常</exception>
        public async Task RenderPicture(PictureBox pictureBox, RichTextBox richTextBoxPictureInfo, string pictureLocation)
        {
            if (string.Empty.Equals(pictureLocation))
            {
                return;
            }
            if (!File.Exists(pictureLocation))
            {
                throw new Exception("图片(" + pictureBox + ")不存在");
            }
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }
            // 加载原始图片
            pictureBox.Image = Image.FromFile(pictureLocation);
            // 调用Baidu API检测图片
            var response = await Task.Run(() =>
            {
                var response = BaiduAIClient.Instance.HumanFaceDetect(pictureLocation);
                return response;
            });
            if (response.errorCode != 0)
            {
                throw new Exception("Baidu API 响应异常");
            }
            if (response.result.faceNum == 0)
            {
                richTextBoxPictureInfo.Text = "没有检测到人脸";
                return;
            }
            var face = response.result.faceList[0];
            StringBuilder sb = new();
            sb.Append($"估算年龄：{face.age}\n");
            sb.Append($"样貌评分：{Math.Round(face.faceProbability, 2)}\n");
            sb.Append($"表情：{face.emotion.type} 置信度：{Math.Round(face.emotion.probability, 2)}\n");
            sb.Append($"脸型：{face.faceShape.type} 置信度：{Math.Round(face.faceShape.probability, 2)}\n");
            sb.Append($"性别：{face.gender.type} 置信度：{Math.Round(face.gender.probability, 2)}\n");
            sb.Append($"人种：{face.faceType.type} 置信度：{Math.Round(face.faceType.probability, 2)}\n");
            richTextBoxPictureInfo.Text = sb.ToString();

            // 重新渲染图片, 耗时操作，异步实现
            Bitmap editableImage = new(pictureBox.Image);
            await Task.Run(() =>
            {
                // 创建Graphics对象用于绘制
                using (Graphics g = Graphics.FromImage(editableImage))
                {
                    // 设置画笔（红色，宽度为6像素）
                    using (Pen pen = new(Color.Red, 6))
                    {
                        // 绘制人脸
                        g.DrawRectangle(
                            pen,
                            new Rectangle(
                                new Point(((int)face.location.left), ((int)face.location.top)),
                                new Size(face.location.width, face.location.height)
                                )
                            );
                    }

                    // 设置画笔（蓝色，宽度为15像素）
                    using (Pen pen = new(Color.Blue, 15))
                    {
                        // 绘制4个关键点位置，左眼中心、右眼中心、鼻尖、嘴中心。
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
