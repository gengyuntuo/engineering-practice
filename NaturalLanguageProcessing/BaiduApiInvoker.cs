using Baidu.Aip.Nlp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NaturalLanguageProcessing
{
    /// <summary>
    /// 百度API调用类
    /// </summary>
    class BaiduApiInvoker
    {
        private readonly Nlp client;
        private BaiduApiInvoker()
        {
            this.client = new Baidu.Aip.Nlp.Nlp(ApplicationConfig.ApiKey, ApplicationConfig.SecretKey);
        }

        private static readonly BaiduApiInvoker INSTANCE_ = new();

        public static BaiduApiInvoker INSTANCE
        {
            get { return INSTANCE_; }
        }

        /// <summary>
        /// 词性表
        /// </summary>
        public static readonly Dictionary<string, string> POSTable =
            new() {
                {"a", "形容词"},
                {"ad", "副形词"},
                {"ag", "形语素"},
                {"an", "名形词"},
                {"b", "区别词"},
                {"c", "连词"},
                {"d", "副词"},
                {"dg", "副语素"},
                {"e", "叹词"},
                {"f", "方位名词"},
                {"g", "语素"},
                {"h", "前接成分"},
                {"i", "成语"},
                {"j", "简称略语"},
                {"k", "后接成分"},
                {"l", "习惯语"},
                {"m", "数量词"},
                {"n", "普通名词"},
                {"ng", "名语素"},
                {"nr", "人名"},
                {"ns", "地名"},
                {"nt", "机构团体名"},
                {"nw", "作品名"},
                {"nz", "其他专名"},
                {"o", "拟声词"},
                {"p", "介词"},
                {"q", "量词"},
                {"r", "代词"},
                {"s", "处所名词"},
                {"t", "时间名词"},
                {"tg", "时语素"},
                {"u", "助词"},
                {"un","未知词"},
                {"v", "普通动词"},
                {"vd", "动副词"},
                {"vg", "动语素"},
                {"vn", "动名词"},
                {"w", "标点符号"},
                {"x", "非语素字"},
                {"xc", "其他虚词"},
                {"y", "语气词"},
                {"z", "状态词"},
                {"PER", "人名"},
                {"LOC", "地名"},
                {"ORG", "机构名"},
                {"TIME", "时间"},
                {"","/"}
            };

        /// <summary>
        /// 专名缩写表
        /// </summary>
        public static readonly Dictionary<string, string> NETable =
            new() {
                {"PER", "人名"},
                {"LOC", "地名"},
                {"ORG", "机构名"},
                {"TIME", "时间"},
                {"","/"}
            };

        /// <summary>
        /// 依存关系缩写
        /// </summary>
        public static readonly Dictionary<string, string> DEPRELTable =
            new() {
               {"SBV", "主谓关系"},
               {"VOB", "动宾关系"},
               {"POB", "介宾关系"},
               {"ADV", "状中关系"},
               {"CMP", "动补关系"},
               {"ATT", "定中关系"},
               {"F", "方位关系"},
               {"COO", "并列关系"},
               {"DBL", "兼语结构"},
               {"DOB", "双宾语结构"},
               {"VV", "连谓结构"},
               {"IC", "子句结构"},
               {"MT", "虚词成分"},
               {"HED", "核心关系"},
               {"",""}
            };


        /// <summary>
        /// 词法分析
        /// </summary>
        /// <param name="text">句子</param>
        /// <returns>词法分析结果</returns>
        /// <exception cref="ApiInvokeException"></exception>
        public LexerResultModel LexerAnalysis(string text)
        {
            var jObject = client.Lexer(text);
            LexerResultModel result = jObject.ToObject<LexerResultModel>(JsonSerializer.Create(ApplicationConfig.JsonSerializerCamelCaseSetting));
            // MessageBox.Show(jObject.ToString(), "提示");
            if (result.Text == null || result.Items == null)
            {
                throw new ApiInvokeException(message: jObject.ToString());
            }
            return result;
        }

        /// <summary>
        /// 依存句法分析
        /// </summary>
        /// <param name="text">句子</param>
        /// <returns>分析结果</returns>
        /// <exception cref="ApiInvokeException"></exception>
        public DepParserAnalysisModel DepParserAnalysis(string text)
        {
            var jObject = client.Depparser(text);
            DepParserAnalysisModel result = jObject.ToObject<DepParserAnalysisModel>(JsonSerializer.Create(ApplicationConfig.JsonSerializerCamelCaseSetting));
            if (result.Text == null || result.Items == null)
            {
                throw new ApiInvokeException(message: jObject.ToString());
            }
            return result;
        }

        /// <summary>
        /// 中文DNN语言模型分析
        /// </summary>
        /// <param name="text">句子</param>
        /// <returns>分析结果</returns>
        /// <exception cref="ApiInvokeException"></exception>
        public DnnlmCnAnalysisModel DnnlmCnAnalysis(string text)
        {
            var jObject = client.DnnlmCn(text);
            DnnlmCnAnalysisModel result = jObject.ToObject<DnnlmCnAnalysisModel>(JsonSerializer.Create(ApplicationConfig.JsonSerializerCamelCaseSetting));
            if (result.Text == null || result.Items == null)
            {
                throw new ApiInvokeException(message: jObject.ToString());
            }
            return result;
        }

        /// <summary>
        /// 短文本相似度
        /// </summary>
        /// <param name="textA">文本A</param>
        /// <param name="textB">文本B</param>
        /// <returns>分析结果</returns>
        /// <exception cref="ApiInvokeException"></exception>
        public SimnetAnalysisModel SimnetAnalysis(string textA, string textB)
        {
            var jObject = client.Simnet(textA, textB);
            SimnetAnalysisModel result = jObject.ToObject<SimnetAnalysisModel>(JsonSerializer.Create(ApplicationConfig.JsonSerializerCamelCaseSetting));
            if (result.LogId == null || result.Score == null)
            {
                throw new ApiInvokeException(message: jObject.ToString());
            }
            return result;
        }

        /// <summary>
        /// 评论观点抽取
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="type">行业</param>
        /// <returns>分析结果</returns>
        /// <exception cref="ApiInvokeException"></exception>
        public CommentTagModel CommentTagAnalysis(string text, int type)
        {
            var jObject = client.CommentTag(text, new Dictionary<string, object> { { "type", type } });
            var result = jObject.ToObject<CommentTagModel>();
            if (result.LogId == null || result.Items == null)
            {
                throw new ApiInvokeException(message: jObject.ToString());
            }
            return result;
        }

        /// <summary>
        /// 情感倾向分析
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>分析结果</returns>
        /// <exception cref="ApiInvokeException"></exception>
        public SentimentClassifyModel SentimentClassify(string text)
        {
            var jObject = client.SentimentClassify(text);
            var result = jObject.ToObject<SentimentClassifyModel>();
            if (result.LogId == null || result.Items == null)
            {
                throw new ApiInvokeException(message: jObject.ToString());
            }
            return result;
        }

        /// <summary>
        /// 文章标签
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <param name="content">文章内容</param>
        /// <returns>分析结果</returns>
        /// <exception cref="ApiInvokeException"></exception>
        public KeywordModel KeywordAnalysis(string title, string content)
        {
            var jObject = client.Keyword(title, content);
            var result = jObject.ToObject<KeywordModel>();
            if (result.LogId == null || result.Items == null)
            {
                throw new ApiInvokeException(message: jObject.ToString());
            }
            return result;
        }
        /// <summary>
        /// 文章分类
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <param name="content">文章内容</param>
        /// <returns>分析结果</returns>
        /// <exception cref="ApiInvokeException"></exception>
        public TopicModel TopicAnalysis(string title, string content)
        {
            var jObject = client.Topic(title, content);
            var result = jObject.ToObject<TopicModel>();
            if (result.LogId == null || result.Item == null)
            {
                throw new ApiInvokeException(message: jObject.ToString());
            }
            return result;
        }
    }


    /// <summary>
    /// 接口调用失败异常：Baidu API未按照预期返回。
    /// </summary>
    class ApiInvokeException : Exception
    {
        public ApiInvokeException()
        {
        }

        public ApiInvokeException(string? message) : base(message)
        {
        }

        public ApiInvokeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
