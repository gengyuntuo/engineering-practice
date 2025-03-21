namespace VoiceApplication
{
    partial class VoiceApplicationMain
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
            buttonAsrAudioRecognize = new Button();
            buttonTtsSelectOutputDir = new Button();
            groupBox1 = new GroupBox();
            comboBoxAsrAudioType = new ComboBox();
            label4 = new Label();
            richTextBoxAsrResult = new RichTextBox();
            label8 = new Label();
            label3 = new Label();
            textBoxAsrFile = new TextBox();
            buttonAsrSelectFile = new Button();
            label1 = new Label();
            groupBox2 = new GroupBox();
            labelTtsVolume = new Label();
            labelTtsSpeed = new Label();
            trackBarTtsAudioVolume = new TrackBar();
            trackBarTtsAudioSpeed = new TrackBar();
            comboBoxTtsPerson = new ComboBox();
            label6 = new Label();
            label5 = new Label();
            richTextBoxTtsContent = new RichTextBox();
            label9 = new Label();
            label2 = new Label();
            textBoxTtsOutputDir = new TextBox();
            buttonTtsSynthesis = new Button();
            openFileDialog = new OpenFileDialog();
            folderBrowserDialog = new FolderBrowserDialog();
            comboBoxTtsFileFormat = new ComboBox();
            label7 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarTtsAudioVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarTtsAudioSpeed).BeginInit();
            SuspendLayout();
            // 
            // buttonAsrAudioRecognize
            // 
            buttonAsrAudioRecognize.Location = new Point(411, 96);
            buttonAsrAudioRecognize.Name = "buttonAsrAudioRecognize";
            buttonAsrAudioRecognize.Size = new Size(150, 46);
            buttonAsrAudioRecognize.TabIndex = 0;
            buttonAsrAudioRecognize.Text = "开始识别";
            buttonAsrAudioRecognize.UseVisualStyleBackColor = true;
            buttonAsrAudioRecognize.Click += ButtonClick;
            // 
            // buttonTtsSelectOutputDir
            // 
            buttonTtsSelectOutputDir.Location = new Point(420, 258);
            buttonTtsSelectOutputDir.Name = "buttonTtsSelectOutputDir";
            buttonTtsSelectOutputDir.Size = new Size(150, 46);
            buttonTtsSelectOutputDir.TabIndex = 1;
            buttonTtsSelectOutputDir.Text = "选择位置";
            buttonTtsSelectOutputDir.UseVisualStyleBackColor = true;
            buttonTtsSelectOutputDir.Click += ButtonClick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBoxAsrAudioType);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(richTextBoxAsrResult);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(buttonAsrAudioRecognize);
            groupBox1.Controls.Add(textBoxAsrFile);
            groupBox1.Controls.Add(buttonAsrSelectFile);
            groupBox1.Location = new Point(29, 33);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(600, 660);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "语音识别";
            // 
            // comboBoxAsrAudioType
            // 
            comboBoxAsrAudioType.FormattingEnabled = true;
            comboBoxAsrAudioType.Location = new Point(122, 101);
            comboBoxAsrAudioType.Name = "comboBoxAsrAudioType";
            comboBoxAsrAudioType.Size = new Size(283, 39);
            comboBoxAsrAudioType.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(34, 284);
            label4.Name = "label4";
            label4.Size = new Size(110, 31);
            label4.TabIndex = 6;
            label4.Text = "识别结果";
            // 
            // richTextBoxAsrResult
            // 
            richTextBoxAsrResult.Location = new Point(34, 333);
            richTextBoxAsrResult.Name = "richTextBoxAsrResult";
            richTextBoxAsrResult.Size = new Size(527, 292);
            richTextBoxAsrResult.TabIndex = 5;
            richTextBoxAsrResult.Text = "";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(9, 105);
            label8.Name = "label8";
            label8.Size = new Size(110, 31);
            label8.TabIndex = 4;
            label8.Text = "语音种类";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 45);
            label3.Name = "label3";
            label3.Size = new Size(110, 31);
            label3.TabIndex = 4;
            label3.Text = "语音文件";
            // 
            // textBoxAsrFile
            // 
            textBoxAsrFile.Location = new Point(122, 42);
            textBoxAsrFile.Name = "textBoxAsrFile";
            textBoxAsrFile.Size = new Size(283, 38);
            textBoxAsrFile.TabIndex = 3;
            // 
            // buttonAsrSelectFile
            // 
            buttonAsrSelectFile.Location = new Point(411, 37);
            buttonAsrSelectFile.Name = "buttonAsrSelectFile";
            buttonAsrSelectFile.Size = new Size(150, 46);
            buttonAsrSelectFile.TabIndex = 1;
            buttonAsrSelectFile.Text = "选择文件";
            buttonAsrSelectFile.UseVisualStyleBackColor = true;
            buttonAsrSelectFile.Click += ButtonClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 429);
            label1.Name = "label1";
            label1.Size = new Size(110, 31);
            label1.TabIndex = 2;
            label1.Text = "语音速度";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(comboBoxTtsFileFormat);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(labelTtsVolume);
            groupBox2.Controls.Add(labelTtsSpeed);
            groupBox2.Controls.Add(trackBarTtsAudioVolume);
            groupBox2.Controls.Add(trackBarTtsAudioSpeed);
            groupBox2.Controls.Add(comboBoxTtsPerson);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(richTextBoxTtsContent);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(buttonTtsSelectOutputDir);
            groupBox2.Controls.Add(textBoxTtsOutputDir);
            groupBox2.Controls.Add(buttonTtsSynthesis);
            groupBox2.Location = new Point(661, 33);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(600, 660);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "语音合成";
            // 
            // labelTtsVolume
            // 
            labelTtsVolume.AutoSize = true;
            labelTtsVolume.Location = new Point(420, 522);
            labelTtsVolume.Name = "labelTtsVolume";
            labelTtsVolume.Size = new Size(21, 31);
            labelTtsVolume.TabIndex = 13;
            labelTtsVolume.Text = " ";
            // 
            // labelTtsSpeed
            // 
            labelTtsSpeed.AutoSize = true;
            labelTtsSpeed.Location = new Point(420, 429);
            labelTtsSpeed.Name = "labelTtsSpeed";
            labelTtsSpeed.Size = new Size(21, 31);
            labelTtsSpeed.TabIndex = 12;
            labelTtsSpeed.Text = " ";
            // 
            // trackBarTtsAudioVolume
            // 
            trackBarTtsAudioVolume.Location = new Point(131, 523);
            trackBarTtsAudioVolume.Maximum = 15;
            trackBarTtsAudioVolume.Name = "trackBarTtsAudioVolume";
            trackBarTtsAudioVolume.Size = new Size(283, 90);
            trackBarTtsAudioVolume.TabIndex = 5;
            trackBarTtsAudioVolume.Value = 5;
            trackBarTtsAudioVolume.ValueChanged += TrackBarChanged;
            // 
            // trackBarTtsAudioSpeed
            // 
            trackBarTtsAudioSpeed.Location = new Point(131, 429);
            trackBarTtsAudioSpeed.Maximum = 9;
            trackBarTtsAudioSpeed.Name = "trackBarTtsAudioSpeed";
            trackBarTtsAudioSpeed.Size = new Size(283, 90);
            trackBarTtsAudioSpeed.TabIndex = 10;
            trackBarTtsAudioSpeed.Value = 5;
            trackBarTtsAudioSpeed.ValueChanged += TrackBarChanged;
            // 
            // comboBoxTtsPerson
            // 
            comboBoxTtsPerson.FormattingEnabled = true;
            comboBoxTtsPerson.Location = new Point(131, 315);
            comboBoxTtsPerson.Name = "comboBoxTtsPerson";
            comboBoxTtsPerson.Size = new Size(283, 39);
            comboBoxTtsPerson.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(24, 266);
            label6.Name = "label6";
            label6.Size = new Size(110, 31);
            label6.TabIndex = 8;
            label6.Text = "生成目录";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(24, 45);
            label5.Name = "label5";
            label5.Size = new Size(110, 31);
            label5.TabIndex = 7;
            label5.Text = "文本内容";
            // 
            // richTextBoxTtsContent
            // 
            richTextBoxTtsContent.Location = new Point(37, 96);
            richTextBoxTtsContent.Name = "richTextBoxTtsContent";
            richTextBoxTtsContent.Size = new Size(527, 151);
            richTextBoxTtsContent.TabIndex = 5;
            richTextBoxTtsContent.Text = "";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(24, 522);
            label9.Name = "label9";
            label9.Size = new Size(110, 31);
            label9.TabIndex = 2;
            label9.Text = "语音音量";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 318);
            label2.Name = "label2";
            label2.Size = new Size(86, 31);
            label2.TabIndex = 3;
            label2.Text = "发音人";
            // 
            // textBoxTtsOutputDir
            // 
            textBoxTtsOutputDir.Location = new Point(131, 263);
            textBoxTtsOutputDir.Name = "textBoxTtsOutputDir";
            textBoxTtsOutputDir.Size = new Size(283, 38);
            textBoxTtsOutputDir.TabIndex = 3;
            // 
            // buttonTtsSynthesis
            // 
            buttonTtsSynthesis.Location = new Point(420, 310);
            buttonTtsSynthesis.Name = "buttonTtsSynthesis";
            buttonTtsSynthesis.Size = new Size(150, 46);
            buttonTtsSynthesis.TabIndex = 0;
            buttonTtsSynthesis.Text = "合成语音";
            buttonTtsSynthesis.UseVisualStyleBackColor = true;
            buttonTtsSynthesis.Click += ButtonClick;
            // 
            // comboBoxTtsFileFormat
            // 
            comboBoxTtsFileFormat.FormattingEnabled = true;
            comboBoxTtsFileFormat.Location = new Point(131, 370);
            comboBoxTtsFileFormat.Name = "comboBoxTtsFileFormat";
            comboBoxTtsFileFormat.Size = new Size(283, 39);
            comboBoxTtsFileFormat.TabIndex = 15;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(24, 373);
            label7.Name = "label7";
            label7.Size = new Size(110, 31);
            label7.TabIndex = 14;
            label7.Text = "文件格式";
            // 
            // VoiceApplicationMain
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1294, 727);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "VoiceApplicationMain";
            Text = "语音工具";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarTtsAudioVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarTtsAudioSpeed).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonAsrAudioRecognize;
        private Button buttonTtsSelectOutputDir;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button buttonAsrSelectFile;
        private Button buttonTtsSynthesis;
        private Label label1;
        private Label label2;
        private OpenFileDialog openFileDialog;
        private Label label4;
        private RichTextBox richTextBoxAsrResult;
        private Label label3;
        private TextBox textBoxAsrFile;
        private Label label5;
        private RichTextBox richTextBoxTtsContent;
        private Label label6;
        private TextBox textBoxTtsOutputDir;
        private ComboBox comboBoxAsrAudioType;
        private Label label8;
        private Label label9;
        private ComboBox comboBoxTtsPerson;
        private TrackBar trackBarTtsAudioSpeed;
        private TrackBar trackBarTtsAudioVolume;
        private Label labelTtsVolume;
        private Label labelTtsSpeed;
        private FolderBrowserDialog folderBrowserDialog;
        private ComboBox comboBoxTtsFileFormat;
        private Label label7;
    }
}
