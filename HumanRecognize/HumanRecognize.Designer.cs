namespace HumanRecognize
{
    partial class FormHumanRecognize
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
            pictureBoxLeft = new PictureBox();
            pictureBoxRight = new PictureBox();
            buttonCompare = new Button();
            buttonLoadLeftPicture = new Button();
            buttonLoadRightPicture = new Button();
            label1 = new Label();
            richTextBoxLeft = new RichTextBox();
            richTextBoxRight = new RichTextBox();
            label2 = new Label();
            label3 = new Label();
            richTextBoxResult = new RichTextBox();
            openFileDialog = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLeft).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRight).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxLeft
            // 
            pictureBoxLeft.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxLeft.Location = new Point(37, 32);
            pictureBoxLeft.Name = "pictureBoxLeft";
            pictureBoxLeft.Size = new Size(400, 300);
            pictureBoxLeft.TabIndex = 0;
            pictureBoxLeft.TabStop = false;
            // 
            // pictureBoxRight
            // 
            pictureBoxRight.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxRight.Location = new Point(683, 32);
            pictureBoxRight.Name = "pictureBoxRight";
            pictureBoxRight.Size = new Size(400, 300);
            pictureBoxRight.TabIndex = 1;
            pictureBoxRight.TabStop = false;
            // 
            // buttonCompare
            // 
            buttonCompare.Location = new Point(464, 32);
            buttonCompare.Name = "buttonCompare";
            buttonCompare.Size = new Size(150, 46);
            buttonCompare.TabIndex = 2;
            buttonCompare.Text = "人脸比对";
            buttonCompare.UseVisualStyleBackColor = true;
            // 
            // buttonLoadLeftPicture
            // 
            buttonLoadLeftPicture.Location = new Point(287, 338);
            buttonLoadLeftPicture.Name = "buttonLoadLeftPicture";
            buttonLoadLeftPicture.Size = new Size(150, 46);
            buttonLoadLeftPicture.TabIndex = 3;
            buttonLoadLeftPicture.Text = "载入图像";
            buttonLoadLeftPicture.UseVisualStyleBackColor = true;
            // 
            // buttonLoadRightPicture
            // 
            buttonLoadRightPicture.Location = new Point(933, 338);
            buttonLoadRightPicture.Name = "buttonLoadRightPicture";
            buttonLoadRightPicture.Size = new Size(150, 46);
            buttonLoadRightPicture.TabIndex = 4;
            buttonLoadRightPicture.Text = "载入图像";
            buttonLoadRightPicture.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(464, 99);
            label1.Name = "label1";
            label1.Size = new Size(110, 31);
            label1.TabIndex = 5;
            label1.Text = "比对结果";
            // 
            // richTextBoxLeft
            // 
            richTextBoxLeft.BorderStyle = BorderStyle.FixedSingle;
            richTextBoxLeft.Location = new Point(37, 437);
            richTextBoxLeft.Name = "richTextBoxLeft";
            richTextBoxLeft.ReadOnly = true;
            richTextBoxLeft.Size = new Size(400, 300);
            richTextBoxLeft.TabIndex = 6;
            richTextBoxLeft.Text = "";
            // 
            // richTextBoxRight
            // 
            richTextBoxRight.BorderStyle = BorderStyle.FixedSingle;
            richTextBoxRight.Location = new Point(683, 437);
            richTextBoxRight.Name = "richTextBoxRight";
            richTextBoxRight.ReadOnly = true;
            richTextBoxRight.Size = new Size(400, 300);
            richTextBoxRight.TabIndex = 7;
            richTextBoxRight.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(37, 403);
            label2.Name = "label2";
            label2.Size = new Size(158, 31);
            label2.TabIndex = 8;
            label2.Text = "人脸检测结果";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(683, 403);
            label3.Name = "label3";
            label3.Size = new Size(158, 31);
            label3.TabIndex = 9;
            label3.Text = "人脸检测结果";
            // 
            // richTextBoxResult
            // 
            richTextBoxResult.Location = new Point(464, 133);
            richTextBoxResult.Name = "richTextBoxResult";
            richTextBoxResult.ReadOnly = true;
            richTextBoxResult.Size = new Size(200, 150);
            richTextBoxResult.TabIndex = 10;
            richTextBoxResult.Text = "";
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // FormHumanRecognize
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1134, 749);
            Controls.Add(richTextBoxResult);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(richTextBoxRight);
            Controls.Add(richTextBoxLeft);
            Controls.Add(label1);
            Controls.Add(buttonLoadRightPicture);
            Controls.Add(buttonLoadLeftPicture);
            Controls.Add(buttonCompare);
            Controls.Add(pictureBoxRight);
            Controls.Add(pictureBoxLeft);
            Name = "FormHumanRecognize";
            Text = "人脸识别程序";
            ((System.ComponentModel.ISupportInitialize)pictureBoxLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRight).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxLeft;
        private PictureBox pictureBoxRight;
        private Button buttonCompare;
        private Button buttonLoadLeftPicture;
        private Button buttonLoadRightPicture;
        private Label label1;
        private RichTextBox richTextBoxLeft;
        private RichTextBox richTextBoxRight;
        private Label label2;
        private Label label3;
        private RichTextBox richTextBoxResult;
        private OpenFileDialog openFileDialog;
    }
}
