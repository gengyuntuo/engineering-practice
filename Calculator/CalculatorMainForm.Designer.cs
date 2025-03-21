namespace Calculator
{
    partial class CalculatorMainForm
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
            inputTextBox = new RichTextBox();
            resultTextBox = new TextBox();
            menuStrip1 = new MenuStrip();
            文件ToolStripMenuItem = new ToolStripMenuItem();
            历史记录ToolStripMenuItem = new ToolStripMenuItem();
            退出ToolStripMenuItem = new ToolStripMenuItem();
            帮助ToolStripMenuItem = new ToolStripMenuItem();
            关于ToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // inputTextBox
            // 
            inputTextBox.DetectUrls = false;
            inputTextBox.Font = new Font("Microsoft YaHei UI", 13.875F, FontStyle.Regular, GraphicsUnit.Point, 134);
            inputTextBox.Location = new Point(102, 42);
            inputTextBox.MaxLength = 1024;
            inputTextBox.Name = "inputTextBox";
            inputTextBox.ShortcutsEnabled = false;
            inputTextBox.Size = new Size(751, 123);
            inputTextBox.TabIndex = 0;
            inputTextBox.Text = "";
            inputTextBox.TextChanged += textBoxChanged;
            // 
            // resultTextBox
            // 
            resultTextBox.Font = new Font("Microsoft YaHei UI", 13.875F, FontStyle.Regular, GraphicsUnit.Point, 134);
            resultTextBox.Location = new Point(102, 171);
            resultTextBox.Name = "resultTextBox";
            resultTextBox.ReadOnly = true;
            resultTextBox.Size = new Size(751, 54);
            resultTextBox.TabIndex = 21;
            resultTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { 文件ToolStripMenuItem, 帮助ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(974, 39);
            menuStrip1.TabIndex = 22;
            menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            文件ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 历史记录ToolStripMenuItem, 退出ToolStripMenuItem });
            文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            文件ToolStripMenuItem.Size = new Size(82, 35);
            文件ToolStripMenuItem.Text = "文件";
            // 
            // 历史记录ToolStripMenuItem
            // 
            历史记录ToolStripMenuItem.Name = "历史记录ToolStripMenuItem";
            历史记录ToolStripMenuItem.Size = new Size(243, 44);
            历史记录ToolStripMenuItem.Text = "历史记录";
            历史记录ToolStripMenuItem.Click += historyClick;
            // 
            // 退出ToolStripMenuItem
            // 
            退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            退出ToolStripMenuItem.Size = new Size(243, 44);
            退出ToolStripMenuItem.Text = "退出";
            退出ToolStripMenuItem.Click += exitClick;
            // 
            // 帮助ToolStripMenuItem
            // 
            帮助ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 关于ToolStripMenuItem });
            帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            帮助ToolStripMenuItem.Size = new Size(82, 35);
            帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 关于ToolStripMenuItem
            // 
            关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            关于ToolStripMenuItem.Size = new Size(195, 44);
            关于ToolStripMenuItem.Text = "关于";
            关于ToolStripMenuItem.Click += aboutClick;
            // 
            // CalculatorMainForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 829);
            Controls.Add(resultTextBox);
            Controls.Add(inputTextBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(1000, 900);
            Name = "CalculatorMainForm";
            Text = "计算器";
            KeyDown += keyDownEvent;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox inputTextBox;
        private TextBox resultTextBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 文件ToolStripMenuItem;
        private ToolStripMenuItem 历史记录ToolStripMenuItem;
        private ToolStripMenuItem 退出ToolStripMenuItem;
        private ToolStripMenuItem 帮助ToolStripMenuItem;
        private ToolStripMenuItem 关于ToolStripMenuItem;
    }
}
