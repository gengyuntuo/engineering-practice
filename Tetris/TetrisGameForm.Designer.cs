namespace Tetris
{
    partial class TetrisGameForm
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
            panelGameStage = new Panel();
            labelScore = new Label();
            labelLevel = new Label();
            richTextBoxGameIntro = new RichTextBox();
            label3 = new Label();
            numericUpDownGameLevel = new NumericUpDown();
            panelNextBlock = new Panel();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGameLevel).BeginInit();
            SuspendLayout();
            // 
            // panelGameStage
            // 
            panelGameStage.BorderStyle = BorderStyle.FixedSingle;
            panelGameStage.Location = new Point(23, 12);
            panelGameStage.Name = "panelGameStage";
            panelGameStage.Size = new Size(400, 800);
            panelGameStage.TabIndex = 0;
            // 
            // labelScore
            // 
            labelScore.AutoSize = true;
            labelScore.Font = new Font("隶书", 13.875F, FontStyle.Regular, GraphicsUnit.Point, 134);
            labelScore.Location = new Point(448, 12);
            labelScore.Name = "labelScore";
            labelScore.Size = new Size(150, 37);
            labelScore.TabIndex = 1;
            labelScore.Text = "得分: 0";
            // 
            // labelLevel
            // 
            labelLevel.AutoSize = true;
            labelLevel.Location = new Point(719, 69);
            labelLevel.Name = "labelLevel";
            labelLevel.Size = new Size(116, 31);
            labelLevel.TabIndex = 1;
            labelLevel.Text = "游戏难度:";
            // 
            // richTextBoxGameIntro
            // 
            richTextBoxGameIntro.Location = new Point(448, 369);
            richTextBoxGameIntro.Name = "richTextBoxGameIntro";
            richTextBoxGameIntro.ReadOnly = true;
            richTextBoxGameIntro.Size = new Size(500, 443);
            richTextBoxGameIntro.TabIndex = 2;
            richTextBoxGameIntro.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(448, 335);
            label3.Name = "label3";
            label3.Size = new Size(116, 31);
            label3.TabIndex = 3;
            label3.Text = "游戏说明:";
            // 
            // numericUpDownGameLevel
            // 
            numericUpDownGameLevel.Location = new Point(841, 67);
            numericUpDownGameLevel.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            numericUpDownGameLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownGameLevel.Name = "numericUpDownGameLevel";
            numericUpDownGameLevel.Size = new Size(82, 38);
            numericUpDownGameLevel.TabIndex = 4;
            numericUpDownGameLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // panelNextBlock
            // 
            panelNextBlock.BorderStyle = BorderStyle.FixedSingle;
            panelNextBlock.Location = new Point(448, 69);
            panelNextBlock.Name = "panelNextBlock";
            panelNextBlock.Size = new Size(160, 160);
            panelNextBlock.TabIndex = 5;
            // 
            // TetrisGameForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 828);
            Controls.Add(panelNextBlock);
            Controls.Add(numericUpDownGameLevel);
            Controls.Add(label3);
            Controls.Add(richTextBoxGameIntro);
            Controls.Add(labelLevel);
            Controls.Add(labelScore);
            Controls.Add(panelGameStage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "TetrisGameForm";
            Text = "俄罗斯方块";
            ((System.ComponentModel.ISupportInitialize)numericUpDownGameLevel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelGameStage;
        private Label labelScore;
        private Label labelLevel;
        private RichTextBox richTextBoxGameIntro;
        private Label label3;
        private NumericUpDown numericUpDownGameLevel;
        private Panel panelNextBlock;
    }
}
