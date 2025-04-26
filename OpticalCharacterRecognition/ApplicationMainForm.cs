namespace OpticalCharacterRecognition
{
    public partial class ApplicationMainForm : Form
    {
        public ApplicationMainForm()
        {
            InitializeComponent();

            // 绑定按钮点击事件
            buttonSelectPicture.Click += buttonClick;
            buttonPlainRecognize.Click += buttonClick;
            buttonWebRecognize.Click += buttonClick;
            buttonCardRecognize.Click += buttonClick;
            buttonTableRecognize.Click += buttonClick;

            // 初始化单选按钮
            radioButtonPlainPrecision.Checked = true;
            radioButtonIdCardFront.Checked = true;
        }

        /// <summary>
        /// 页面中所有的按钮的点击事件
        /// </summary>
        /// <param name="sender">点击的按钮</param>
        /// <param name="e">事件参数</param>
        private async void buttonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            try
            {
                // 点击后禁用按钮，防止重复点击。
                clickedButton.Enabled = false;
                switch (clickedButton.Name)
                {
                    case "buttonSelectPicture":
                        ButtonClickAction.DoSelectPicture(textBoxPictureLocation, openFileDialogPictureSelector);
                        break;
                    case "buttonPlainRecognize":
                        var precision = getCheckedValue(groupBox1) ?? "普通精度";
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
                    default: throw new Exception("您点击了不支持的Button");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("错误: " + exp.Message + "!", "错误");
            }
            finally
            {
                // 启用按钮
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
