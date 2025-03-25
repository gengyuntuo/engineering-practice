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

            // 初始化词法分析控件
            dataGridViewPartOneLexerAnalysis.Columns.Add("Index", "序号");
            dataGridViewPartOneLexerAnalysis.Columns.Add("Item", "分词");
            dataGridViewPartOneLexerAnalysis.Columns.Add("Pos", "词性");
            dataGridViewPartOneLexerAnalysis.Columns.Add("BasicWords", "基本词");
            dataGridViewPartOneLexerAnalysis.Columns["Index"].Width = 80;
            dataGridViewPartOneLexerAnalysis.Columns["Item"].Width = 120;
            dataGridViewPartOneLexerAnalysis.Columns["Pos"].Width = 120;
            // dataGridViewPartOneLexerAnalysis.Columns["BasicWords"].Width = 180;

            // 初始化词法分析控件
            dataGridViewPartOneDNNAnalysis.Columns.Add("Index", "序号");
            dataGridViewPartOneDNNAnalysis.Columns.Add("Item", "分词");
            dataGridViewPartOneDNNAnalysis.Columns.Add("Prob", "概率值");
            dataGridViewPartOneDNNAnalysis.Columns["Index"].Width = 80;
            dataGridViewPartOneDNNAnalysis.Columns["Item"].Width = 240;
            dataGridViewPartOneDNNAnalysis.Columns["Prob"].Width = 180;

            // 行业
            var typeItems = new Dictionary<string, string>
            {
                { "1", "酒店" },
                { "2", "KTV" },
                { "3", "丽人" },
                { "4", "美食餐饮" },
                { "5", "旅游" },
                { "6", "健康" },
                { "7", "教育" },
                { "8", "商业" },
                { "9", "房产" },
                { "10", "汽车" },
                { "11", "生活" },
                { "12", "购物" }
            };
            var comboBoxPartThreeTypeItems = new List<KeyValuePair<string, string>>(typeItems);
            comboBoxPartThreeType.DataSource = comboBoxPartThreeTypeItems;
            comboBoxPartThreeType.DisplayMember = "Value";
            comboBoxPartThreeType.ValueMember = "Key";

            // For test, remove it after test
            richTextBoxPartOne.Text = "我们在北航学习软件工程";
            richTextBoxPartTwoSimilarA.Text = "创建好账号后，在正式调用 AI 能力之前，需要先创建一下应用，这个是调用服务的基础能力单元，不可或缺哦";
            richTextBoxPartTwoSimilarB.Text = "创建帐户后，需要在调用 AI 之前创建应用程序。这是呼叫服务的基本能力单位。这是不可缺少的";
            richTextBoxPartThreeInput.Text = "个人觉得这个车不错，外观漂亮年轻，动力和操控性都很好。";
            textBoxPartFourTitle.Text = "欧洲冠军杯足球赛";
            richTextBoxPartFourContent.Text = "欧洲冠军联赛是欧洲足球协会联盟主办的年度足球比赛，代表欧洲俱乐部足球最高荣誉和水平，被认为是全世界最高素质、最具影响力以及最高水平的俱乐部赛事，亦是世界上奖金最高的足球赛事和体育赛事之一。";
        }

        /// <summary>
        /// Button控件点击事件
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
                MessageBox.Show("程序运行发生异常，请根据以下信息排查：\n" + ex.Message, "错误");
            }
            finally
            {
                button.Enabled = true;
            }
        }

        /// <summary>
        /// 第一部分：词法分析
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
                        i + 1, // 序号
                        result.Items[i].Item, // 分词
                        BaiduApiInvoker.POSTable[result.Items[i].Pos ?? ""], // 词性
                        String.Join("、", result.Items[i].BasicWords ?? []) // 基本词
                    );
                }
            }
        }

        /// <summary>
        /// 第一部分：依存句法分析
        /// </summary>
        private void DoDepParserAnalysis()
        {
            var inputText = richTextBoxPartOne.Text.Trim();
            var result = BaiduApiInvoker.INSTANCE.DepParserAnalysis(inputText);
            treeViewPartOneDepParserAnalysis.Nodes.Clear();

            var rootNode = ApplicationMainHelpers.GenerateTree(result.Items ?? []);
            treeViewPartOneDepParserAnalysis.Nodes.Add(node: rootNode ?? new("空白"));
            treeViewPartOneDepParserAnalysis.ExpandAll();
            // richTextBoxPartThreeOutput.Text = result;
        }

        /// <summary>
        /// 第一部分：中文DNN语言模型
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
                        i + 1, // 序号
                        result.Items[i].Word, // 分词
                        Math.Round(result.Items[i].Prob, 3) // 概率值
                    );
                }
            }
            textBoxPartOneDnnAnalysis.Text = result.Ppl.ToString();
        }

        /// <summary>
        /// 第二部分：短文本相似度分析
        /// </summary>
        private void DoSimnetAnalysis()
        {
            var result = BaiduApiInvoker.INSTANCE.SimnetAnalysis(richTextBoxPartTwoSimilarA.Text, richTextBoxPartTwoSimilarB.Text);
            textBoxPartTwoSimilar.Text = Math.Round(result.Score ?? Double.NaN, 3).ToString();
        }

        /// <summary>
        /// 第三部分：评论观点分析
        /// </summary>
        private void DoCommentTagAnalysis()
        {
            var text = richTextBoxPartThreeInput.Text;
            var type = Convert.ToInt32(comboBoxPartThreeType.SelectedValue);
            var result = BaiduApiInvoker.INSTANCE.CommentTagAnalysis(text, type);
            richTextBoxPartThreeOutput.Text = string.Empty;

            // 观点倾向：0表示消极，1表示中性，2表示积极
            List<string> standpointEnum = ["消极", "中性", "积极"];

            if (result.Items != null)
            {
                var selected = result.Items.Select(item => $"""
                观点倾向：{standpointEnum[item.Sentiment ?? -1]}
                短句摘要：{(item.Abstract ?? "").Replace("<span>", "[").Replace("</span>", "]").Replace("[]", "")}
                匹配属性词: {item.Prop}
                匹配描述词：{item.Adj}
                """).ToList();
                var analysisConent = string.Join("\n---------------------------------------\n", selected);
                richTextBoxPartThreeOutput.Text = analysisConent;
            }
        }

        /// <summary>
        /// 第三部分：情感分析
        /// </summary>
        private void DoSentimentClassify()
        {
            var text = richTextBoxPartThreeInput.Text;
            var result = BaiduApiInvoker.INSTANCE.SentimentClassify(text);
            richTextBoxPartThreeOutput.Text = string.Empty;

            // 情感极性分类结果, 0:负向，1:中性，2:正向
            List<string> sentimentEnum = ["负向", "中性", "正向"];

            if (result.Items != null && result.Items.Count > 0)
            {
                var selected = result.Items.Select(item => $"""
                观点倾向：{sentimentEnum[item.Sentiment ?? -1]}
                分类置信度：{item.Confidence}
                积极类别概率: {item.PositiveProb}
                消极类别概率：{item.NegativeProb}
                """).ToList();
                var analysisConent = string.Join("\n---------------------------------------\n", selected);
                richTextBoxPartThreeOutput.Text = analysisConent;
            }
        }

        /// <summary>
        /// 文章标签分析
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
        /// 文章分类分析
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
                richTextBoxPartFourResult.Text = $"一级分类：\n{analysisConent1}\n\n二级分类：\n{analysisConent2}";
            }
        }
    }
}
