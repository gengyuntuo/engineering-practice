namespace SnakeGame
{
    partial class SnakeGameForm
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
            panel = new Panel();
            labelScore = new Label();
            label2 = new Label();
            richTextBoxDescription = new RichTextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // panel
            // 
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(12, 17);
            panel.Name = "panel";
            panel.Size = new Size(800, 800);
            panel.TabIndex = 0;
            // 
            // labelScore
            // 
            labelScore.AutoSize = true;
            labelScore.Font = new Font("隶书", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 134);
            labelScore.Location = new Point(831, 17);
            labelScore.Name = "labelScore";
            labelScore.Size = new Size(155, 37);
            labelScore.TabIndex = 1;
            labelScore.Text = "分数: 0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("隶书", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label2.Location = new Point(985, 52);
            label2.Name = "label2";
            label2.Size = new Size(0, 37);
            label2.TabIndex = 2;
            // 
            // richTextBoxDescription
            // 
            richTextBoxDescription.Location = new Point(831, 207);
            richTextBoxDescription.Name = "richTextBoxDescription";
            richTextBoxDescription.ReadOnly = true;
            richTextBoxDescription.Size = new Size(331, 610);
            richTextBoxDescription.TabIndex = 3;
            richTextBoxDescription.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("隶书", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label1.Location = new Point(831, 153);
            label1.Name = "label1";
            label1.Size = new Size(173, 37);
            label1.TabIndex = 4;
            label1.Text = "游戏说明";
            // 
            // SnakeGameForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1174, 829);
            Controls.Add(label1);
            Controls.Add(richTextBoxDescription);
            Controls.Add(label2);
            Controls.Add(labelScore);
            Controls.Add(panel);
            KeyPreview = true;
            Name = "SnakeGameForm";
            Text = "贪吃蛇游戏";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel;
        private Label labelScore;
        private Label label2;
        private RichTextBox richTextBoxDescription;
        private Label label1;
    }
}
