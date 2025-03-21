namespace Calculator
{

    public partial class CalculatorMainForm : Form
    {
        private readonly IHistoryRecorder historyRecorder = new MemoryHistoryRecorder();
        /// <summary>
        /// ���м��̰�������
        /// </summary>
        private readonly Button[] keyBoardArray = new Button[24];
        /// <summary>
        /// �������Զ���
        /// </summary>
        private ButtonAttribute[] simpleKeyBoardButtonAttributeArray = new ButtonAttribute[] {
            new ButtonAttribute(){ ButtonText = "(", ButtonValue = "("},
            new ButtonAttribute(){ ButtonText = ")", ButtonValue = ")"},
            new ButtonAttribute(){ ButtonText = "e", ButtonValue = "e"},
            new ButtonAttribute(){ ButtonText = "PI", ButtonValue = "PI"},
            new ButtonAttribute(){ ButtonText = "%", ButtonValue = "%"},
            new ButtonAttribute(){ ButtonText = "/", ButtonValue = "/"},
            new ButtonAttribute(){ ButtonText = "C", ButtonValue = "C"},
            new ButtonAttribute(){ ButtonText = "DEL", ButtonValue = "DEL"},
            new ButtonAttribute(){ ButtonText = "7", ButtonValue = "7"},
            new ButtonAttribute(){ ButtonText = "8", ButtonValue = "8"},
            new ButtonAttribute(){ ButtonText = "9", ButtonValue = "9"},
            new ButtonAttribute(){ ButtonText = "*", ButtonValue = "*"},
            new ButtonAttribute(){ ButtonText = "4", ButtonValue = "4"},
            new ButtonAttribute(){ ButtonText = "5", ButtonValue = "5"},
            new ButtonAttribute(){ ButtonText = "6", ButtonValue = "6"},
            new ButtonAttribute(){ ButtonText = "+", ButtonValue = "+"},
            new ButtonAttribute(){ ButtonText = "1", ButtonValue = "1"},
            new ButtonAttribute(){ ButtonText = "2", ButtonValue = "2"},
            new ButtonAttribute(){ ButtonText = "3", ButtonValue = "3"},
            new ButtonAttribute(){ ButtonText = "-", ButtonValue = "-"},
            new ButtonAttribute(){ ButtonText = "00", ButtonValue = "00"},
            new ButtonAttribute(){ ButtonText = "0", ButtonValue = "0"},
            new ButtonAttribute(){ ButtonText = ".", ButtonValue = "."},
            new ButtonAttribute(){ ButtonText = "=", ButtonValue = "="},
        };
        public CalculatorMainForm()
        {
            InitializeComponent();
            InitKeyBoard();
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        private void InitKeyBoard()
        {
            int rowNum = 6; // 6��
            int columnNum = 4; // 4��
            // ÿ��Button֮������ھ�
            int innerGap = (this.Width - 168 * columnNum - 100 * 2) / (columnNum - 1);
            // Button֮����м��
            int rowGap = (this.Height - 64 * rowNum - 300 - 120) / (rowNum - 1);
            for (int i = 0; i < keyBoardArray.Length; i++)
            {
                int rowIndex = i / 4;
                int ceilIndex = i % 4;
                keyBoardArray[i] = new Button();
                keyBoardArray[i].Text = simpleKeyBoardButtonAttributeArray[i].ButtonText;
                keyBoardArray[i].Click += this.buttonClick;
                keyBoardArray[i].Location = new Point(100 + (168 + innerGap) * ceilIndex, 300 + (64 + rowGap) * rowIndex);
                keyBoardArray[i].Name = simpleKeyBoardButtonAttributeArray[i].ButtonValue;
                keyBoardArray[i].Size = new Size(168, 64);
                keyBoardArray[i].TabIndex = 6;
                keyBoardArray[i].UseVisualStyleBackColor = true;
                this.Controls.Add(keyBoardArray[i]);
            }

        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text)
            {
                case "C": inputTextBox.Text = String.Empty; break;
                case "DEL":
                    string content = inputTextBox.Text;
                    inputTextBox.Text = content.Length > 0 ? content.Substring(0, content.Length - 1) : String.Empty;
                    break;
                case "=": historyRecorder.SaveExpressionAndResult(inputTextBox.Text, resultTextBox.Text); break;
                default:
                    // ����й�����ڹ�����룬����׷��
                    if (inputTextBox.Focused)
                    {
                        inputTextBox.Text = inputTextBox.Text.Insert(inputTextBox.SelectionStart > 0 ? inputTextBox.SelectionStart + 1 : 0, btn.Name);
                    }
                    else
                    {
                        inputTextBox.Text += btn.Name;
                    }
                    break;
            }

        }
        /// <summary>
        /// ��������ݸı�����½��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxChanged(object sender, EventArgs e)
        {
            try
            {
                if (inputTextBox.Text.Length == 0)
                {
                    resultTextBox.Text = String.Empty;
                    return;
                }
                double? value = ExpressionUtil.CalculateEnhancedExpression(inputTextBox.Text);
                resultTextBox.Text = String.Empty + value;
            }
            catch (Exception)
            {
                resultTextBox.Text = "ERROR";
            }

        }
        private void aboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("������: ZF2421107(�)\n��Ȩ���� �������պ����ѧ", "����");
        }
        private void historyClick(object sender, EventArgs e)
        {
            string content = String.Join("\n", historyRecorder.listHistory());
            MessageBox.Show(content.Length != 0 ? content: "<��>", "��ʷ��¼(���10��)");
        }

        private void exitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void keyDownEvent(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter: break;
                default: break;
            }
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    class ButtonAttribute
    {
        /// <summary>
        /// �����ı�
        /// </summary>
        public string? ButtonText { set; get; }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public string? ButtonValue { set; get; }
    }
}
