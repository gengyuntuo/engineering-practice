namespace WebCrawler
{
    partial class WebCrawlerForm
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
            buttonPageBackward = new Button();
            buttonPageForward = new Button();
            label1 = new Label();
            textBoxPageURL = new TextBox();
            buttonAccessPage = new Button();
            label2 = new Label();
            progressBar = new ProgressBar();
            labelProgressBar = new Label();
            groupBox = new GroupBox();
            label5 = new Label();
            textBoxEndIndex = new TextBox();
            textBoxBeginIndex = new TextBox();
            label4 = new Label();
            buttonStartCrawler = new Button();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            panel = new Panel();
            groupBox.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // buttonPageBackward
            // 
            buttonPageBackward.Location = new Point(21, 12);
            buttonPageBackward.Name = "buttonPageBackward";
            buttonPageBackward.Size = new Size(80, 46);
            buttonPageBackward.TabIndex = 0;
            buttonPageBackward.Text = "<-";
            buttonPageBackward.UseVisualStyleBackColor = true;
            // 
            // buttonPageForward
            // 
            buttonPageForward.Location = new Point(107, 12);
            buttonPageForward.Name = "buttonPageForward";
            buttonPageForward.Size = new Size(80, 46);
            buttonPageForward.TabIndex = 1;
            buttonPageForward.Text = "->";
            buttonPageForward.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(193, 20);
            label1.Name = "label1";
            label1.Size = new Size(62, 31);
            label1.TabIndex = 2;
            label1.Text = "网址";
            // 
            // textBoxPageURL
            // 
            textBoxPageURL.Location = new Point(256, 17);
            textBoxPageURL.Name = "textBoxPageURL";
            textBoxPageURL.Size = new Size(1036, 38);
            textBoxPageURL.TabIndex = 3;
            // 
            // buttonAccessPage
            // 
            buttonAccessPage.Location = new Point(1298, 12);
            buttonAccessPage.Name = "buttonAccessPage";
            buttonAccessPage.Size = new Size(150, 46);
            buttonAccessPage.TabIndex = 4;
            buttonAccessPage.Text = "转到";
            buttonAccessPage.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 118);
            label2.Name = "label2";
            label2.Size = new Size(134, 31);
            label2.TabIndex = 5;
            label2.Text = "页扫描计数";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(169, 115);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(388, 36);
            progressBar.TabIndex = 6;
            // 
            // labelProgressBar
            // 
            labelProgressBar.AutoSize = true;
            labelProgressBar.Location = new Point(323, 81);
            labelProgressBar.Name = "labelProgressBar";
            labelProgressBar.Size = new Size(52, 31);
            labelProgressBar.TabIndex = 7;
            labelProgressBar.Text = "0/0";
            // 
            // groupBox
            // 
            groupBox.Controls.Add(label5);
            groupBox.Controls.Add(textBoxEndIndex);
            groupBox.Controls.Add(textBoxBeginIndex);
            groupBox.Controls.Add(label4);
            groupBox.Location = new Point(563, 72);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(421, 100);
            groupBox.TabIndex = 8;
            groupBox.TabStop = false;
            groupBox.Text = "搜索起止编号";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(228, 46);
            label5.Name = "label5";
            label5.Size = new Size(24, 31);
            label5.TabIndex = 12;
            label5.Text = "-";
            // 
            // textBoxEndIndex
            // 
            textBoxEndIndex.Location = new Point(258, 43);
            textBoxEndIndex.MaxLength = 10;
            textBoxEndIndex.Name = "textBoxEndIndex";
            textBoxEndIndex.Size = new Size(136, 38);
            textBoxEndIndex.TabIndex = 11;
            // 
            // textBoxBeginIndex
            // 
            textBoxBeginIndex.Location = new Point(86, 43);
            textBoxBeginIndex.MaxLength = 10;
            textBoxBeginIndex.Name = "textBoxBeginIndex";
            textBoxBeginIndex.Size = new Size(136, 38);
            textBoxBeginIndex.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(18, 46);
            label4.Name = "label4";
            label4.Size = new Size(62, 31);
            label4.TabIndex = 9;
            label4.Text = "编号";
            // 
            // buttonStartCrawler
            // 
            buttonStartCrawler.Location = new Point(1025, 105);
            buttonStartCrawler.Name = "buttonStartCrawler";
            buttonStartCrawler.Size = new Size(150, 46);
            buttonStartCrawler.TabIndex = 9;
            buttonStartCrawler.Text = "开始";
            buttonStartCrawler.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(32, 32);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 807);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1474, 22);
            statusStrip.TabIndex = 10;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0, 12);
            // 
            // panel
            // 
            panel.Location = new Point(12, 178);
            panel.Name = "panel";
            panel.Size = new Size(1450, 600);
            panel.TabIndex = 11;
            // 
            // WebCrawlerForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1474, 829);
            Controls.Add(panel);
            Controls.Add(statusStrip);
            Controls.Add(buttonStartCrawler);
            Controls.Add(groupBox);
            Controls.Add(labelProgressBar);
            Controls.Add(progressBar);
            Controls.Add(label2);
            Controls.Add(buttonAccessPage);
            Controls.Add(textBoxPageURL);
            Controls.Add(label1);
            Controls.Add(buttonPageForward);
            Controls.Add(buttonPageBackward);
            Name = "WebCrawlerForm";
            Text = "网络爬虫程序";
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonPageBackward;
        private Button buttonPageForward;
        private Label label1;
        private TextBox textBoxPageURL;
        private Button buttonAccessPage;
        private Label label2;
        private ProgressBar progressBar;
        private Label labelProgressBar;
        private GroupBox groupBox;
        private Label label5;
        private TextBox textBoxEndIndex;
        private TextBox textBoxBeginIndex;
        private Label label4;
        private Button buttonStartCrawler;
        private StatusStrip statusStrip;
        private Panel panel;
        private ToolStripStatusLabel toolStripStatusLabel;
    }
}
