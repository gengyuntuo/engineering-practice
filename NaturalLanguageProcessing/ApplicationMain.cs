using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NaturalLanguageProcessing
{
    public partial class ApplicationMain : Form
    {
        public ApplicationMain()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            InitializeComponent();

            // ��ʼ���ʷ������ؼ�
            dataGridViewPartOneLexerAnalysis.Columns.Add("Index", "���");
            dataGridViewPartOneLexerAnalysis.Columns.Add("Item", "�ִ�");
            dataGridViewPartOneLexerAnalysis.Columns.Add("Pos", "����");
            dataGridViewPartOneLexerAnalysis.Columns.Add("BasicWords", "������");
            dataGridViewPartOneLexerAnalysis.Columns["Index"].Width = 80;
            dataGridViewPartOneLexerAnalysis.Columns["Item"].Width = 120;
            dataGridViewPartOneLexerAnalysis.Columns["Pos"].Width = 120;
            // dataGridViewPartOneLexerAnalysis.Columns["BasicWords"].Width = 180;

            // ��ʼ���ʷ������ؼ�
            dataGridViewPartOneDNNAnalysis.Columns.Add("Index", "���");
            dataGridViewPartOneDNNAnalysis.Columns.Add("Item", "�ִ�");
            dataGridViewPartOneDNNAnalysis.Columns.Add("Prob", "����ֵ");
            dataGridViewPartOneDNNAnalysis.Columns["Index"].Width = 80;
            dataGridViewPartOneDNNAnalysis.Columns["Item"].Width = 240;
            dataGridViewPartOneDNNAnalysis.Columns["Prob"].Width = 180;

            // ��ҵ
            var typeItems = new Dictionary<string, string>
            {
                { "1", "�Ƶ�" },
                { "2", "KTV" },
                { "3", "����" },
                { "4", "��ʳ����" },
                { "5", "����" },
                { "6", "����" },
                { "7", "����" },
                { "8", "��ҵ" },
                { "9", "����" },
                { "10", "����" },
                { "11", "����" },
                { "12", "����" }
            };
            var comboBoxPartThreeTypeItems = new List<KeyValuePair<string, string>>(typeItems);
            comboBoxPartThreeType.DataSource = comboBoxPartThreeTypeItems;
            comboBoxPartThreeType.DisplayMember = "Value";
            comboBoxPartThreeType.ValueMember = "Key";

            // For test, remove it after test
            richTextBoxPartOne.Text = "�����ڱ���ѧϰ�������";
            richTextBoxPartTwoSimilarA.Text = "�������˺ź�����ʽ���� AI ����֮ǰ����Ҫ�ȴ���һ��Ӧ�ã�����ǵ��÷���Ļ���������Ԫ�����ɻ�ȱŶ";
            richTextBoxPartTwoSimilarB.Text = "�����ʻ�����Ҫ�ڵ��� AI ֮ǰ����Ӧ�ó������Ǻ��з���Ļ���������λ�����ǲ���ȱ�ٵ�";
            richTextBoxPartThreeInput.Text = "���˾���������������Ư�����ᣬ�����Ͳٿ��Զ��ܺá�";
            textBoxPartFourTitle.Text = "ŷ�޹ھ���������";
            richTextBoxPartFourContent.Text = "ŷ�޹ھ�������ŷ������Э���������������������������ŷ�޾��ֲ��������������ˮƽ������Ϊ��ȫ����������ʡ����Ӱ�����Լ����ˮƽ�ľ��ֲ����£����������Ͻ�����ߵ��������º���������֮һ��";
        }

        /// <summary>
        /// Button�ؼ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnButtonClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            try
            {
                button.Enabled = false;
                switch (button.Name)
                {
                    case "buttonPartOneLexicalAnalysis":
                        DoPartOneLexicalAnalysis();
                        break;
                    case "buttonPartOneDepParserAnalysis":
                        DoDepParserAnalysis();
                        break;
                    case "buttonPartOneDnnAnalysis":
                        DoDnnAnalysis();
                        break;
                    case "buttonPartTwoSimilar":
                        DoSimnetAnalysis();
                        break;
                    case "buttonPartThreeCommentTag":
                        DoCommentTagAnalysis();
                        break;
                    case "buttonPartThreeSentimentClassify":
                        DoSentimentClassify();
                        break;
                    case "buttonPartFourKeyword":
                        DoKeywordAnalysis();
                        break;
                    case "buttonPartFourTopic":
                        DoTopicAnalysis();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������з����쳣�������������Ϣ�Ų飺\n" + ex.Message, "����");
            }
            finally
            {
                button.Enabled = true;
            }
        }

        /// <summary>
        /// ��һ���֣��ʷ�����
        /// </summary>
        private void DoPartOneLexicalAnalysis()
        {
            var inputText = richTextBoxPartOne.Text.Trim();
            var result = BaiduApiInvoker.INSTANCE.LexerAnalysis(inputText);
            dataGridViewPartOneLexerAnalysis.Rows.Clear();
            // richTextBoxPartThreeOutput.Text = JsonConvert.SerializeObject(result).ToString();
            if (result.Items != null)
            {
                for (int i = 0; i < result.Items.Count; i++)
                {
                    dataGridViewPartOneLexerAnalysis.Rows.Add(
                        i + 1, // ���
                        result.Items[i].Item, // �ִ�
                        BaiduApiInvoker.POSTable[result.Items[i].Pos ?? ""], // ����
                        String.Join("��", result.Items[i].BasicWords ?? []) // ������
                    );
                }
            }
        }

        /// <summary>
        /// ��һ���֣�����䷨����
        /// </summary>
        private void DoDepParserAnalysis()
        {
            var inputText = richTextBoxPartOne.Text.Trim();
            var result = BaiduApiInvoker.INSTANCE.DepParserAnalysis(inputText);
            treeViewPartOneDepParserAnalysis.Nodes.Clear();

            var rootNode = ApplicationMainHelpers.GenerateTree(result.Items ?? []);
            treeViewPartOneDepParserAnalysis.Nodes.Add(node: rootNode ?? new("�հ�"));
            treeViewPartOneDepParserAnalysis.ExpandAll();
            // richTextBoxPartThreeOutput.Text = result;
        }

        /// <summary>
        /// ��һ���֣�����DNN����ģ��
        /// </summary>
        private void DoDnnAnalysis()
        {
            var inputText = richTextBoxPartOne.Text.Trim();
            var result = BaiduApiInvoker.INSTANCE.DnnlmCnAnalysis(inputText);
            dataGridViewPartOneDNNAnalysis.Rows.Clear();

            if (result.Items != null)
            {
                for (int i = 0; i < result.Items.Count; i++)
                {
                    dataGridViewPartOneDNNAnalysis.Rows.Add(
                        i + 1, // ���
                        result.Items[i].Word, // �ִ�
                        Math.Round(result.Items[i].Prob, 3) // ����ֵ
                    );
                }
            }
            textBoxPartOneDnnAnalysis.Text = result.Ppl.ToString();
        }

        /// <summary>
        /// �ڶ����֣����ı����ƶȷ���
        /// </summary>
        private void DoSimnetAnalysis()
        {
            var result = BaiduApiInvoker.INSTANCE.SimnetAnalysis(richTextBoxPartTwoSimilarA.Text, richTextBoxPartTwoSimilarB.Text);
            textBoxPartTwoSimilar.Text = Math.Round(result.Score ?? Double.NaN, 3).ToString();
        }

        /// <summary>
        /// �������֣����۹۵����
        /// </summary>
        private void DoCommentTagAnalysis()
        {
            var text = richTextBoxPartThreeInput.Text;
            var type = Convert.ToInt32(comboBoxPartThreeType.SelectedValue);
            var result = BaiduApiInvoker.INSTANCE.CommentTagAnalysis(text, type);
            richTextBoxPartThreeOutput.Text = string.Empty;

            // �۵�����0��ʾ������1��ʾ���ԣ�2��ʾ����
            List<string> standpointEnum = ["����", "����", "����"];

            if (result.Items != null)
            {
                var selected = result.Items.Select(item => $"""
                �۵�����{standpointEnum[item.Sentiment ?? -1]}
                �̾�ժҪ��{(item.Abstract ?? "").Replace("<span>", "[").Replace("</span>", "]").Replace("[]", "")}
                ƥ�����Դ�: {item.Prop}
                ƥ�������ʣ�{item.Adj}
                """).ToList();
                var analysisConent = string.Join("\n---------------------------------------\n", selected);
                richTextBoxPartThreeOutput.Text = analysisConent;
            }
        }

        /// <summary>
        /// �������֣���з���
        /// </summary>
        private void DoSentimentClassify()
        {
            var text = richTextBoxPartThreeInput.Text;
            var result = BaiduApiInvoker.INSTANCE.SentimentClassify(text);
            richTextBoxPartThreeOutput.Text = string.Empty;

            // ��м��Է�����, 0:����1:���ԣ�2:����
            List<string> sentimentEnum = ["����", "����", "����"];

            if (result.Items != null && result.Items.Count > 0)
            {
                var selected = result.Items.Select(item => $"""
                �۵�����{sentimentEnum[item.Sentiment ?? -1]}
                �������Ŷȣ�{item.Confidence}
                ����������: {item.PositiveProb}
                ���������ʣ�{item.NegativeProb}
                """).ToList();
                var analysisConent = string.Join("\n---------------------------------------\n", selected);
                richTextBoxPartThreeOutput.Text = analysisConent;
            }
        }

        /// <summary>
        /// ���±�ǩ����
        /// </summary>
        private void DoKeywordAnalysis()
        {
            var title = textBoxPartFourTitle.Text;
            var content = richTextBoxPartFourContent.Text;
            var result = BaiduApiInvoker.INSTANCE.KeywordAnalysis(title, content);
            richTextBoxPartFourResult.Text = string.Empty;

            if (result.Items != null)
            {
                var selected = result.Items.Select(item => $"{item.Tag}: {Math.Round(item.Score, 3)}").ToList();
                var analysisConent = string.Join("\n", selected);
                richTextBoxPartFourResult.Text = analysisConent;
            }

        }

        /// <summary>
        /// ���·������
        /// </summary>
        private void DoTopicAnalysis()
        {
            var title = textBoxPartFourTitle.Text;
            var content = richTextBoxPartFourContent.Text;
            var result = BaiduApiInvoker.INSTANCE.TopicAnalysis(title, content);
            richTextBoxPartFourResult.Text = string.Empty;

            if (result.Item != null)
            {
                var selectedLevel1 = result.Item.Lv1TagList.Select(item => $"{item.Tag}: {Math.Round(item.Score, 3)}").ToList();
                var selectedLevel2 = result.Item.Lv2TagList.Select(item => $"{item.Tag}: {Math.Round(item.Score, 3)}").ToList();
                var analysisConent1 = string.Join("\n", selectedLevel1);
                var analysisConent2 = string.Join("\n", selectedLevel2);
                richTextBoxPartFourResult.Text = $"һ�����ࣺ\n{analysisConent1}\n\n�������ࣺ\n{analysisConent2}";
            }
        }
    }
}
