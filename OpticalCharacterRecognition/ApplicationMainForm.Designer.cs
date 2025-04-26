namespace OpticalCharacterRecognition
{
    partial class ApplicationMainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBoxPictureLocation = new TextBox();
            buttonSelectPicture = new Button();
            groupBox1 = new GroupBox();
            richTextBoxPlainRecognize = new RichTextBox();
            buttonPlainRecognize = new Button();
            checkBoxDetectDirection = new CheckBox();
            checkBoxContainLocation = new CheckBox();
            radioButtonHighPrecision = new RadioButton();
            radioButtonPlainPrecision = new RadioButton();
            groupBox2 = new GroupBox();
            richTextBoxWebRecognize = new RichTextBox();
            buttonWebRecognize = new Button();
            groupBox3 = new GroupBox();
            radioButtonPayCard = new RadioButton();
            radioButtonIdCardBack = new RadioButton();
            radioButtonBusinessCard = new RadioButton();
            radioButtonIdCardFront = new RadioButton();
            buttonCardRecognize = new Button();
            richTextBoxCardRecognize = new RichTextBox();
            groupBox4 = new GroupBox();
            buttonTableRecognize = new Button();
            richTextBoxTableRecognize = new RichTextBox();
            openFileDialogPictureSelector = new OpenFileDialog();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxPictureLocation
            // 
            textBoxPictureLocation.Location = new Point(22, 17);
            textBoxPictureLocation.Name = "textBoxPictureLocation";
            textBoxPictureLocation.Size = new Size(1125, 38);
            textBoxPictureLocation.TabIndex = 0;
            // 
            // buttonSelectPicture
            // 
            buttonSelectPicture.Location = new Point(1152, 12);
            buttonSelectPicture.Name = "buttonSelectPicture";
            buttonSelectPicture.Size = new Size(150, 46);
            buttonSelectPicture.TabIndex = 1;
            buttonSelectPicture.Text = "选择图片";
            buttonSelectPicture.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(richTextBoxPlainRecognize);
            groupBox1.Controls.Add(buttonPlainRecognize);
            groupBox1.Controls.Add(checkBoxDetectDirection);
            groupBox1.Controls.Add(checkBoxContainLocation);
            groupBox1.Controls.Add(radioButtonHighPrecision);
            groupBox1.Controls.Add(radioButtonPlainPrecision);
            groupBox1.Location = new Point(25, 59);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(630, 380);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "通用文字识别";
            // 
            // richTextBoxPlainRecognize
            // 
            richTextBoxPlainRecognize.Location = new Point(20, 149);
            richTextBoxPlainRecognize.Name = "richTextBoxPlainRecognize";
            richTextBoxPlainRecognize.ReadOnly = true;
            richTextBoxPlainRecognize.Size = new Size(590, 220);
            richTextBoxPlainRecognize.TabIndex = 5;
            richTextBoxPlainRecognize.Text = "";
            // 
            // buttonPlainRecognize
            // 
            buttonPlainRecognize.Location = new Point(460, 97);
            buttonPlainRecognize.Name = "buttonPlainRecognize";
            buttonPlainRecognize.Size = new Size(150, 46);
            buttonPlainRecognize.TabIndex = 4;
            buttonPlainRecognize.Text = "识别图片";
            buttonPlainRecognize.UseVisualStyleBackColor = true;
            // 
            // checkBoxDetectDirection
            // 
            checkBoxDetectDirection.AutoSize = true;
            checkBoxDetectDirection.Location = new Point(250, 97);
            checkBoxDetectDirection.Name = "checkBoxDetectDirection";
            checkBoxDetectDirection.Size = new Size(142, 35);
            checkBoxDetectDirection.TabIndex = 3;
            checkBoxDetectDirection.Text = "检测方向";
            checkBoxDetectDirection.UseVisualStyleBackColor = true;
            // 
            // checkBoxContainLocation
            // 
            checkBoxContainLocation.AutoSize = true;
            checkBoxContainLocation.Location = new Point(24, 97);
            checkBoxContainLocation.Name = "checkBoxContainLocation";
            checkBoxContainLocation.Size = new Size(166, 35);
            checkBoxContainLocation.TabIndex = 2;
            checkBoxContainLocation.Text = "含位置信息";
            checkBoxContainLocation.UseVisualStyleBackColor = true;
            // 
            // radioButtonHighPrecision
            // 
            radioButtonHighPrecision.AutoSize = true;
            radioButtonHighPrecision.Location = new Point(250, 51);
            radioButtonHighPrecision.Name = "radioButtonHighPrecision";
            radioButtonHighPrecision.Size = new Size(117, 35);
            radioButtonHighPrecision.TabIndex = 1;
            radioButtonHighPrecision.TabStop = true;
            radioButtonHighPrecision.Text = "高精度";
            radioButtonHighPrecision.UseVisualStyleBackColor = true;
            // 
            // radioButtonPlainPrecision
            // 
            radioButtonPlainPrecision.AutoSize = true;
            radioButtonPlainPrecision.Location = new Point(24, 51);
            radioButtonPlainPrecision.Name = "radioButtonPlainPrecision";
            radioButtonPlainPrecision.Size = new Size(141, 35);
            radioButtonPlainPrecision.TabIndex = 0;
            radioButtonPlainPrecision.TabStop = true;
            radioButtonPlainPrecision.Text = "普通精度";
            radioButtonPlainPrecision.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(richTextBoxWebRecognize);
            groupBox2.Controls.Add(buttonWebRecognize);
            groupBox2.Location = new Point(25, 445);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(630, 380);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "网络图片文字识别";
            // 
            // richTextBoxWebRecognize
            // 
            richTextBoxWebRecognize.Location = new Point(20, 89);
            richTextBoxWebRecognize.Name = "richTextBoxWebRecognize";
            richTextBoxWebRecognize.ReadOnly = true;
            richTextBoxWebRecognize.Size = new Size(590, 280);
            richTextBoxWebRecognize.TabIndex = 6;
            richTextBoxWebRecognize.Text = "";
            // 
            // buttonWebRecognize
            // 
            buttonWebRecognize.Location = new Point(460, 37);
            buttonWebRecognize.Name = "buttonWebRecognize";
            buttonWebRecognize.Size = new Size(150, 46);
            buttonWebRecognize.TabIndex = 6;
            buttonWebRecognize.Text = "识别图片";
            buttonWebRecognize.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioButtonPayCard);
            groupBox3.Controls.Add(radioButtonIdCardBack);
            groupBox3.Controls.Add(radioButtonBusinessCard);
            groupBox3.Controls.Add(radioButtonIdCardFront);
            groupBox3.Controls.Add(buttonCardRecognize);
            groupBox3.Controls.Add(richTextBoxCardRecognize);
            groupBox3.Location = new Point(672, 59);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(630, 380);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "卡证识别";
            // 
            // radioButtonPayCard
            // 
            radioButtonPayCard.AutoSize = true;
            radioButtonPayCard.Location = new Point(348, 51);
            radioButtonPayCard.Name = "radioButtonPayCard";
            radioButtonPayCard.Size = new Size(117, 35);
            radioButtonPayCard.TabIndex = 10;
            radioButtonPayCard.TabStop = true;
            radioButtonPayCard.Text = "银行卡";
            radioButtonPayCard.UseVisualStyleBackColor = true;
            // 
            // radioButtonIdCardBack
            // 
            radioButtonIdCardBack.AutoSize = true;
            radioButtonIdCardBack.Location = new Point(177, 51);
            radioButtonIdCardBack.Name = "radioButtonIdCardBack";
            radioButtonIdCardBack.Size = new Size(165, 35);
            radioButtonIdCardBack.TabIndex = 9;
            radioButtonIdCardBack.TabStop = true;
            radioButtonIdCardBack.Text = "身份证背面";
            radioButtonIdCardBack.UseVisualStyleBackColor = true;
            // 
            // radioButtonBusinessCard
            // 
            radioButtonBusinessCard.AutoSize = true;
            radioButtonBusinessCard.Location = new Point(471, 51);
            radioButtonBusinessCard.Name = "radioButtonBusinessCard";
            radioButtonBusinessCard.Size = new Size(141, 35);
            radioButtonBusinessCard.TabIndex = 8;
            radioButtonBusinessCard.TabStop = true;
            radioButtonBusinessCard.Text = "营业执照";
            radioButtonBusinessCard.UseVisualStyleBackColor = true;
            // 
            // radioButtonIdCardFront
            // 
            radioButtonIdCardFront.AutoSize = true;
            radioButtonIdCardFront.Location = new Point(6, 51);
            radioButtonIdCardFront.Name = "radioButtonIdCardFront";
            radioButtonIdCardFront.Size = new Size(165, 35);
            radioButtonIdCardFront.TabIndex = 6;
            radioButtonIdCardFront.TabStop = true;
            radioButtonIdCardFront.Text = "身份证正面";
            radioButtonIdCardFront.UseVisualStyleBackColor = true;
            // 
            // buttonCardRecognize
            // 
            buttonCardRecognize.Location = new Point(462, 97);
            buttonCardRecognize.Name = "buttonCardRecognize";
            buttonCardRecognize.Size = new Size(150, 46);
            buttonCardRecognize.TabIndex = 7;
            buttonCardRecognize.Text = "识别图片";
            buttonCardRecognize.UseVisualStyleBackColor = true;
            // 
            // richTextBoxCardRecognize
            // 
            richTextBoxCardRecognize.Location = new Point(22, 149);
            richTextBoxCardRecognize.Name = "richTextBoxCardRecognize";
            richTextBoxCardRecognize.ReadOnly = true;
            richTextBoxCardRecognize.Size = new Size(590, 220);
            richTextBoxCardRecognize.TabIndex = 7;
            richTextBoxCardRecognize.Text = "";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(buttonTableRecognize);
            groupBox4.Controls.Add(richTextBoxTableRecognize);
            groupBox4.Location = new Point(672, 445);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(630, 380);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "表格文字识别";
            // 
            // buttonTableRecognize
            // 
            buttonTableRecognize.Location = new Point(462, 37);
            buttonTableRecognize.Name = "buttonTableRecognize";
            buttonTableRecognize.Size = new Size(150, 46);
            buttonTableRecognize.TabIndex = 8;
            buttonTableRecognize.Text = "识别图片";
            buttonTableRecognize.UseVisualStyleBackColor = true;
            // 
            // richTextBoxTableRecognize
            // 
            richTextBoxTableRecognize.Location = new Point(22, 89);
            richTextBoxTableRecognize.Name = "richTextBoxTableRecognize";
            richTextBoxTableRecognize.ReadOnly = true;
            richTextBoxTableRecognize.Size = new Size(590, 280);
            richTextBoxTableRecognize.TabIndex = 8;
            richTextBoxTableRecognize.Text = "";
            // 
            // ApplicationMainForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1324, 829);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(buttonSelectPicture);
            Controls.Add(textBoxPictureLocation);
            Name = "ApplicationMainForm";
            Text = "百度文字识别客户端";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxPictureLocation;
        private Button buttonSelectPicture;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private RadioButton radioButtonHighPrecision;
        private RadioButton radioButtonPlainPrecision;
        private RichTextBox richTextBoxPlainRecognize;
        private Button buttonPlainRecognize;
        private CheckBox checkBoxDetectDirection;
        private CheckBox checkBoxContainLocation;
        private RichTextBox richTextBoxWebRecognize;
        private Button buttonWebRecognize;
        private RichTextBox richTextBoxCardRecognize;
        private RichTextBox richTextBoxTableRecognize;
        private Button buttonCardRecognize;
        private Button buttonTableRecognize;
        private RadioButton radioButtonPayCard;
        private RadioButton radioButtonIdCardBack;
        private RadioButton radioButtonBusinessCard;
        private RadioButton radioButtonIdCardFront;
        private OpenFileDialog openFileDialogPictureSelector;
    }
}
