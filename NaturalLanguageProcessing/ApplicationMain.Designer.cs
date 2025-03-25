
namespace NaturalLanguageProcessing
{
    partial class ApplicationMain
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
            groupBoxPart1 = new GroupBox();
            textBoxPartOneDnnAnalysis = new TextBox();
            label8 = new Label();
            label2 = new Label();
            label1 = new Label();
            dataGridViewPartOneLexerAnalysis = new DataGridView();
            buttonPartOneDnnAnalysis = new Button();
            buttonPartOneDepParserAnalysis = new Button();
            dataGridViewPartOneDNNAnalysis = new DataGridView();
            treeViewPartOneDepParserAnalysis = new TreeView();
            buttonPartOneLexicalAnalysis = new Button();
            richTextBoxPartOne = new RichTextBox();
            groupBox1 = new GroupBox();
            textBoxPartTwoSimilar = new TextBox();
            label4 = new Label();
            buttonPartTwoSimilar = new Button();
            richTextBoxPartTwoSimilarB = new RichTextBox();
            richTextBoxPartTwoSimilarA = new RichTextBox();
            groupBox2 = new GroupBox();
            richTextBoxPartThreeOutput = new RichTextBox();
            buttonPartThreeSentimentClassify = new Button();
            buttonPartThreeCommentTag = new Button();
            comboBoxPartThreeType = new ComboBox();
            label3 = new Label();
            richTextBoxPartThreeInput = new RichTextBox();
            groupBox3 = new GroupBox();
            label7 = new Label();
            buttonPartFourTopic = new Button();
            buttonPartFourKeyword = new Button();
            richTextBoxPartFourResult = new RichTextBox();
            richTextBoxPartFourContent = new RichTextBox();
            label6 = new Label();
            textBoxPartFourTitle = new TextBox();
            label5 = new Label();
            groupBoxPart1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPartOneLexerAnalysis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPartOneDNNAnalysis).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxPart1
            // 
            groupBoxPart1.Controls.Add(textBoxPartOneDnnAnalysis);
            groupBoxPart1.Controls.Add(label8);
            groupBoxPart1.Controls.Add(label2);
            groupBoxPart1.Controls.Add(label1);
            groupBoxPart1.Controls.Add(dataGridViewPartOneLexerAnalysis);
            groupBoxPart1.Controls.Add(buttonPartOneDnnAnalysis);
            groupBoxPart1.Controls.Add(buttonPartOneDepParserAnalysis);
            groupBoxPart1.Controls.Add(dataGridViewPartOneDNNAnalysis);
            groupBoxPart1.Controls.Add(treeViewPartOneDepParserAnalysis);
            groupBoxPart1.Controls.Add(buttonPartOneLexicalAnalysis);
            groupBoxPart1.Controls.Add(richTextBoxPartOne);
            groupBoxPart1.Location = new Point(28, 31);
            groupBoxPart1.Name = "groupBoxPart1";
            groupBoxPart1.Size = new Size(550, 1000);
            groupBoxPart1.TabIndex = 0;
            groupBoxPart1.TabStop = false;
            groupBoxPart1.Text = "词法/依存句法分析/DNN语言模型";
            // 
            // textBoxPartOneDnnAnalysis
            // 
            textBoxPartOneDnnAnalysis.Location = new Point(170, 714);
            textBoxPartOneDnnAnalysis.Name = "textBoxPartOneDnnAnalysis";
            textBoxPartOneDnnAnalysis.ReadOnly = true;
            textBoxPartOneDnnAnalysis.Size = new Size(124, 38);
            textBoxPartOneDnnAnalysis.TabIndex = 14;
            textBoxPartOneDnnAnalysis.WordWrap = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(27, 717);
            label8.Name = "label8";
            label8.Size = new Size(158, 31);
            label8.TabIndex = 9;
            label8.Text = "句子通顺度：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 459);
            label2.Name = "label2";
            label2.Size = new Size(158, 31);
            label2.TabIndex = 8;
            label2.Text = "依存句法分析";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 201);
            label1.Name = "label1";
            label1.Size = new Size(110, 31);
            label1.TabIndex = 7;
            label1.Text = "词法分析";
            // 
            // dataGridViewPartOneLexerAnalysis
            // 
            dataGridViewPartOneLexerAnalysis.AllowUserToAddRows = false;
            dataGridViewPartOneLexerAnalysis.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPartOneLexerAnalysis.Location = new Point(27, 245);
            dataGridViewPartOneLexerAnalysis.Name = "dataGridViewPartOneLexerAnalysis";
            dataGridViewPartOneLexerAnalysis.ReadOnly = true;
            dataGridViewPartOneLexerAnalysis.RowHeadersVisible = false;
            dataGridViewPartOneLexerAnalysis.RowHeadersWidth = 82;
            dataGridViewPartOneLexerAnalysis.Size = new Size(500, 200);
            dataGridViewPartOneLexerAnalysis.TabIndex = 6;
            // 
            // buttonPartOneDnnAnalysis
            // 
            buttonPartOneDnnAnalysis.Location = new Point(327, 709);
            buttonPartOneDnnAnalysis.Name = "buttonPartOneDnnAnalysis";
            buttonPartOneDnnAnalysis.Size = new Size(200, 46);
            buttonPartOneDnnAnalysis.TabIndex = 5;
            buttonPartOneDnnAnalysis.Text = "DNN语言模型";
            buttonPartOneDnnAnalysis.UseVisualStyleBackColor = true;
            buttonPartOneDnnAnalysis.Click += OnButtonClick;
            // 
            // buttonPartOneDepParserAnalysis
            // 
            buttonPartOneDepParserAnalysis.Location = new Point(327, 451);
            buttonPartOneDepParserAnalysis.Name = "buttonPartOneDepParserAnalysis";
            buttonPartOneDepParserAnalysis.Size = new Size(200, 46);
            buttonPartOneDepParserAnalysis.TabIndex = 4;
            buttonPartOneDepParserAnalysis.Text = "依存句法分析";
            buttonPartOneDepParserAnalysis.UseVisualStyleBackColor = true;
            buttonPartOneDepParserAnalysis.Click += OnButtonClick;
            // 
            // dataGridViewPartOneDNNAnalysis
            // 
            dataGridViewPartOneDNNAnalysis.AllowUserToAddRows = false;
            dataGridViewPartOneDNNAnalysis.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPartOneDNNAnalysis.Location = new Point(27, 761);
            dataGridViewPartOneDNNAnalysis.Name = "dataGridViewPartOneDNNAnalysis";
            dataGridViewPartOneDNNAnalysis.ReadOnly = true;
            dataGridViewPartOneDNNAnalysis.RowHeadersVisible = false;
            dataGridViewPartOneDNNAnalysis.RowHeadersWidth = 82;
            dataGridViewPartOneDNNAnalysis.Size = new Size(500, 200);
            dataGridViewPartOneDNNAnalysis.TabIndex = 3;
            // 
            // treeViewPartOneDepParserAnalysis
            // 
            treeViewPartOneDepParserAnalysis.Location = new Point(27, 503);
            treeViewPartOneDepParserAnalysis.Name = "treeViewPartOneDepParserAnalysis";
            treeViewPartOneDepParserAnalysis.Size = new Size(500, 200);
            treeViewPartOneDepParserAnalysis.TabIndex = 2;
            // 
            // buttonPartOneLexicalAnalysis
            // 
            buttonPartOneLexicalAnalysis.Location = new Point(377, 193);
            buttonPartOneLexicalAnalysis.Name = "buttonPartOneLexicalAnalysis";
            buttonPartOneLexicalAnalysis.Size = new Size(150, 46);
            buttonPartOneLexicalAnalysis.TabIndex = 1;
            buttonPartOneLexicalAnalysis.Text = "词法分析";
            buttonPartOneLexicalAnalysis.UseVisualStyleBackColor = true;
            buttonPartOneLexicalAnalysis.Click += OnButtonClick;
            // 
            // richTextBoxPartOne
            // 
            richTextBoxPartOne.Location = new Point(27, 37);
            richTextBoxPartOne.Name = "richTextBoxPartOne";
            richTextBoxPartOne.Size = new Size(500, 150);
            richTextBoxPartOne.TabIndex = 0;
            richTextBoxPartOne.Text = "";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBoxPartTwoSimilar);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(buttonPartTwoSimilar);
            groupBox1.Controls.Add(richTextBoxPartTwoSimilarB);
            groupBox1.Controls.Add(richTextBoxPartTwoSimilarA);
            groupBox1.Location = new Point(584, 31);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(800, 252);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "短文本相似度";
            // 
            // textBoxPartTwoSimilar
            // 
            textBoxPartTwoSimilar.Location = new Point(116, 201);
            textBoxPartTwoSimilar.Name = "textBoxPartTwoSimilar";
            textBoxPartTwoSimilar.ReadOnly = true;
            textBoxPartTwoSimilar.Size = new Size(200, 38);
            textBoxPartTwoSimilar.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 204);
            label4.Name = "label4";
            label4.Size = new Size(110, 31);
            label4.TabIndex = 12;
            label4.Text = "相似度：";
            // 
            // buttonPartTwoSimilar
            // 
            buttonPartTwoSimilar.Location = new Point(633, 196);
            buttonPartTwoSimilar.Name = "buttonPartTwoSimilar";
            buttonPartTwoSimilar.Size = new Size(150, 46);
            buttonPartTwoSimilar.TabIndex = 10;
            buttonPartTwoSimilar.Text = "短文本相似度";
            buttonPartTwoSimilar.UseVisualStyleBackColor = true;
            buttonPartTwoSimilar.Click += OnButtonClick;
            // 
            // richTextBoxPartTwoSimilarB
            // 
            richTextBoxPartTwoSimilarB.Location = new Point(403, 37);
            richTextBoxPartTwoSimilarB.Name = "richTextBoxPartTwoSimilarB";
            richTextBoxPartTwoSimilarB.Size = new Size(380, 150);
            richTextBoxPartTwoSimilarB.TabIndex = 11;
            richTextBoxPartTwoSimilarB.Text = "";
            // 
            // richTextBoxPartTwoSimilarA
            // 
            richTextBoxPartTwoSimilarA.Location = new Point(17, 37);
            richTextBoxPartTwoSimilarA.Name = "richTextBoxPartTwoSimilarA";
            richTextBoxPartTwoSimilarA.Size = new Size(380, 150);
            richTextBoxPartTwoSimilarA.TabIndex = 10;
            richTextBoxPartTwoSimilarA.Text = "";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(richTextBoxPartThreeOutput);
            groupBox2.Controls.Add(buttonPartThreeSentimentClassify);
            groupBox2.Controls.Add(buttonPartThreeCommentTag);
            groupBox2.Controls.Add(comboBoxPartThreeType);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(richTextBoxPartThreeInput);
            groupBox2.Location = new Point(584, 306);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(800, 725);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "评论观点抽取/情感倾向分析";
            // 
            // richTextBoxPartThreeOutput
            // 
            richTextBoxPartThreeOutput.DetectUrls = false;
            richTextBoxPartThreeOutput.Location = new Point(17, 262);
            richTextBoxPartThreeOutput.Name = "richTextBoxPartThreeOutput";
            richTextBoxPartThreeOutput.ReadOnly = true;
            richTextBoxPartThreeOutput.Size = new Size(766, 420);
            richTextBoxPartThreeOutput.TabIndex = 13;
            richTextBoxPartThreeOutput.Text = "";
            // 
            // buttonPartThreeSentimentClassify
            // 
            buttonPartThreeSentimentClassify.Location = new Point(607, 199);
            buttonPartThreeSentimentClassify.Name = "buttonPartThreeSentimentClassify";
            buttonPartThreeSentimentClassify.Size = new Size(176, 46);
            buttonPartThreeSentimentClassify.TabIndex = 12;
            buttonPartThreeSentimentClassify.Text = "情感倾向分析";
            buttonPartThreeSentimentClassify.UseVisualStyleBackColor = true;
            buttonPartThreeSentimentClassify.Click += OnButtonClick;
            // 
            // buttonPartThreeCommentTag
            // 
            buttonPartThreeCommentTag.Location = new Point(403, 199);
            buttonPartThreeCommentTag.Name = "buttonPartThreeCommentTag";
            buttonPartThreeCommentTag.Size = new Size(176, 46);
            buttonPartThreeCommentTag.TabIndex = 9;
            buttonPartThreeCommentTag.Text = "评论观点抽取";
            buttonPartThreeCommentTag.UseVisualStyleBackColor = true;
            buttonPartThreeCommentTag.Click += OnButtonClick;
            // 
            // comboBoxPartThreeType
            // 
            comboBoxPartThreeType.FormattingEnabled = true;
            comboBoxPartThreeType.Location = new Point(105, 204);
            comboBoxPartThreeType.Name = "comboBoxPartThreeType";
            comboBoxPartThreeType.Size = new Size(191, 39);
            comboBoxPartThreeType.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 207);
            label3.Name = "label3";
            label3.Size = new Size(86, 31);
            label3.TabIndex = 9;
            label3.Text = "行业：";
            // 
            // richTextBoxPartThreeInput
            // 
            richTextBoxPartThreeInput.Location = new Point(17, 37);
            richTextBoxPartThreeInput.Name = "richTextBoxPartThreeInput";
            richTextBoxPartThreeInput.Size = new Size(766, 150);
            richTextBoxPartThreeInput.TabIndex = 10;
            richTextBoxPartThreeInput.Text = "";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(buttonPartFourTopic);
            groupBox3.Controls.Add(buttonPartFourKeyword);
            groupBox3.Controls.Add(richTextBoxPartFourResult);
            groupBox3.Controls.Add(richTextBoxPartFourContent);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(textBoxPartFourTitle);
            groupBox3.Controls.Add(label5);
            groupBox3.Location = new Point(1390, 31);
            groupBox3.MaximumSize = new Size(550, 1000);
            groupBox3.MinimumSize = new Size(550, 1000);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(550, 1000);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "文章标签/文章分类";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 691);
            label7.Name = "label7";
            label7.Size = new Size(168, 31);
            label7.TabIndex = 17;
            label7.Text = "标签/分类结果";
            // 
            // buttonPartFourTopic
            // 
            buttonPartFourTopic.Location = new Point(384, 635);
            buttonPartFourTopic.Name = "buttonPartFourTopic";
            buttonPartFourTopic.Size = new Size(150, 46);
            buttonPartFourTopic.TabIndex = 16;
            buttonPartFourTopic.Text = "文章分类";
            buttonPartFourTopic.UseVisualStyleBackColor = true;
            buttonPartFourTopic.Click += OnButtonClick;
            // 
            // buttonPartFourKeyword
            // 
            buttonPartFourKeyword.Location = new Point(14, 635);
            buttonPartFourKeyword.Name = "buttonPartFourKeyword";
            buttonPartFourKeyword.Size = new Size(150, 46);
            buttonPartFourKeyword.TabIndex = 14;
            buttonPartFourKeyword.Text = "文章标签";
            buttonPartFourKeyword.UseVisualStyleBackColor = true;
            buttonPartFourKeyword.Click += OnButtonClick;
            // 
            // richTextBoxPartFourResult
            // 
            richTextBoxPartFourResult.Location = new Point(14, 738);
            richTextBoxPartFourResult.Name = "richTextBoxPartFourResult";
            richTextBoxPartFourResult.ReadOnly = true;
            richTextBoxPartFourResult.Size = new Size(520, 219);
            richTextBoxPartFourResult.TabIndex = 15;
            richTextBoxPartFourResult.Text = "";
            // 
            // richTextBoxPartFourContent
            // 
            richTextBoxPartFourContent.Location = new Point(14, 178);
            richTextBoxPartFourContent.Name = "richTextBoxPartFourContent";
            richTextBoxPartFourContent.Size = new Size(520, 450);
            richTextBoxPartFourContent.TabIndex = 14;
            richTextBoxPartFourContent.Text = "";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 134);
            label6.Name = "label6";
            label6.Size = new Size(110, 31);
            label6.TabIndex = 2;
            label6.Text = "文章内容";
            // 
            // textBoxPartFourTitle
            // 
            textBoxPartFourTitle.Location = new Point(14, 83);
            textBoxPartFourTitle.Name = "textBoxPartFourTitle";
            textBoxPartFourTitle.Size = new Size(520, 38);
            textBoxPartFourTitle.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 45);
            label5.Name = "label5";
            label5.Size = new Size(110, 31);
            label5.TabIndex = 0;
            label5.Text = "文章标题";
            // 
            // ApplicationMain
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1964, 1059);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(groupBoxPart1);
            Name = "ApplicationMain";
            Text = "百度云NLP客户端";
            groupBoxPart1.ResumeLayout(false);
            groupBoxPart1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPartOneLexerAnalysis).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPartOneDNNAnalysis).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxPart1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button buttonPartOneDnnAnalysis;
        private Button buttonPartOneDepParserAnalysis;
        private DataGridView dataGridViewPartOneDNNAnalysis;
        private TreeView treeViewPartOneDepParserAnalysis;
        private Button buttonPartOneLexicalAnalysis;
        private RichTextBox richTextBoxPartOne;
        private DataGridView dataGridViewPartOneLexerAnalysis;
        private Label label3;
        private Label label2;
        private Label label1;
        private RichTextBox richTextBoxPartTwoSimilarB;
        private RichTextBox richTextBoxPartTwoSimilarA;
        private TextBox textBoxPartTwoSimilar;
        private Label label4;
        private Button buttonPartTwoSimilar;
        private RichTextBox richTextBoxPartThreeInput;
        private ComboBox comboBoxPartThreeType;
        private Button buttonPartThreeSentimentClassify;
        private Button buttonPartThreeCommentTag;
        private RichTextBox richTextBoxPartThreeOutput;
        private TextBox textBoxPartFourTitle;
        private Label label5;
        private RichTextBox richTextBoxPartFourResult;
        private RichTextBox richTextBoxPartFourContent;
        private Label label6;
        private Button buttonPartFourTopic;
        private Button buttonPartFourKeyword;
        private Label label7;
        private TextBox textBoxPartOneDnnAnalysis;
        private Label label8;
    }
}
