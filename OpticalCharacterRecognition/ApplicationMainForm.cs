namespace OpticalCharacterRecognition
{
    public partial class ApplicationMainForm : Form
    {
        public ApplicationMainForm()
        {
            InitializeComponent();

            // �󶨰�ť����¼�
            buttonSelectPicture.Click += buttonClick;
            buttonPlainRecognize.Click += buttonClick;
            buttonWebRecognize.Click += buttonClick;
            buttonCardRecognize.Click += buttonClick;
            buttonTableRecognize.Click += buttonClick;

            // ��ʼ����ѡ��ť
            radioButtonPlainPrecision.Checked = true;
            radioButtonIdCardFront.Checked = true;
        }

        /// <summary>
        /// ҳ�������еİ�ť�ĵ���¼�
        /// </summary>
        /// <param name="sender">����İ�ť</param>
        /// <param name="e">�¼�����</param>
        private async void buttonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            try
            {
                // �������ð�ť����ֹ�ظ������
                clickedButton.Enabled = false;
                switch (clickedButton.Name)
                {
                    case "buttonSelectPicture":
                        ButtonClickAction.DoSelectPicture(textBoxPictureLocation, openFileDialogPictureSelector);
                        break;
                    case "buttonPlainRecognize":
                        var precision = getCheckedValue(groupBox1) ?? "��ͨ����";
                        var text = await ButtonClickAction.DoPlainPictureRecognize(
                            textBoxPictureLocation.Text,
                            precision, checkBoxContainLocation.Checked,
                            checkBoxDetectDirection.Checked
                            );
                        richTextBoxPlainRecognize.Text = text;
                        break;
                    case "buttonWebRecognize":
                         text = await ButtonClickAction.DoWebPictureRecognize(textBoxPictureLocation.Text);
                        richTextBoxWebRecognize.Text = text;
                        break;
                    case "buttonCardRecognize":
                        var cardType = getCheckedValue(groupBox3);
                        text = await ButtonClickAction.DoCardPictureRecognize(textBoxPictureLocation.Text, cardType);
                        richTextBoxCardRecognize.Text = text;
                        break;
                    case "buttonTableRecognize":
                        text = await ButtonClickAction.DoTablePictureRecognize(textBoxPictureLocation.Text);
                        richTextBoxTableRecognize.Text = text;
                        break;
                    default: throw new Exception("������˲�֧�ֵ�Button");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("����: " + exp.Message + "!", "����");
            }
            finally
            {
                // ���ð�ť
                clickedButton.Enabled = true;
            }
        }

        private string? getCheckedValue(GroupBox groupBox)
        {
            foreach (Control control in groupBox.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    return radioButton.Text;
                }
            }
            return null;
        }
    }
}
