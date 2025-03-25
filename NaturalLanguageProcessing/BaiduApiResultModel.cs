using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalLanguageProcessing
{
    /// <summary>
    /// 词法分析接口返回结果
    /// </summary>
    public class LexerResultModel
    {
        public string? Text { get; set; }
        public List<ItemModel>? Items { get; set; }
        public long LogId { get; set; }
    }

    /// <summary>
    /// 词法分析Item
    /// </summary>
    public class ItemModel
    {
        public string? Uri { get; set; }
        public string? Formal { get; set; }
        public string? Ne { get; set; }
        public string? Item { get; set; }
        public List<object>? LocDetails { get; set; }
        [JsonProperty("basic_words")]
        public List<string>? BasicWords { get; set; }
        public int? ByteOffset { get; set; }
        public int? ByteLength { get; set; }
        public string? Pos { get; set; }
    }

    /// <summary>
    /// 依存句法分析
    /// </summary>
    public class DepParserAnalysisModel
    {
        public List<DepParserAnalysisItemModel>? Items { get; set; }
        public string? Text { get; set; }
        public long? LogId { get; set; }
    }

    /// <summary>
    /// 依存句法分析 Item 的模型类
    /// </summary>
    public class DepParserAnalysisItemModel
    {
        public required int Id { get; set; }
        public string? Postag { get; set; }
        public required int Head { get; set; }
        public required string Word { get; set; }
        public string? Deprel { get; set; }
    }

    /// <summary>
    /// 中文DNN语言模型
    /// </summary>
    public class DnnlmCnAnalysisModel
    {
        public string? Text { get; set; }
        public List<DnnlmCnAnalysisItemModel>? Items { get; set; }
        public double? Ppl { get; set; }
    }

    /// <summary>
    /// 中文DNN语言模型
    /// </summary>
    public class DnnlmCnAnalysisItemModel
    {
        public required string Word { get; set; }
        public required double Prob { get; set; }
    }

    /// <summary>
    /// 短文本相似度
    /// </summary>
    public class SimnetAnalysisModel
    {
        [JsonProperty("log_id")]
        public long? LogId { get; set; }
        public SimnetAnalysisTextsModel? Texts { get; set; }
        public double? Score { get; set; }
    }

    /// <summary>
    /// 短文本相似度
    /// </summary>
    public class SimnetAnalysisTextsModel
    {
        public required string Text1 { get; set; }
        public required string Text2 { get; set; }
    }

    /// <summary>
    /// 评论观点分析
    /// </summary>
    public class CommentTagModel
    {
        [JsonProperty("log_id")]
        public long? LogId { get; set; }

        [JsonProperty("items")]
        public List<CommentTagItemModel>? Items { get; set; }
    }

    /// <summary>
    /// 评论观点分析
    /// </summary>
    public class CommentTagItemModel
    {
        [JsonProperty("sentiment")]
        public int? Sentiment { get; set; }

        [JsonProperty("abstract")]
        public string? Abstract { get; set; }

        [JsonProperty("prop")]
        public string? Prop { get; set; }

        [JsonProperty("begin_pos")]
        public int? BeginPos { get; set; }

        [JsonProperty("end_pos")]
        public int? EndPos { get; set; }

        [JsonProperty("adj")]
        public string? Adj { get; set; }
    }

    /// <summary>
    /// 情感倾向分析
    /// </summary>
    public class SentimentClassifyModel
    {
        [JsonProperty("text")]
        public string? Text { get; set; }

        [JsonProperty("items")]
        public List<SentimentClassifyItem>? Items { get; set; }

        [JsonProperty("log_id")]
        public long? LogId { get; set; }
    }

    /// <summary>
    /// 情感倾向分析
    /// </summary>
    public class SentimentClassifyItem
    {
        [JsonProperty("confidence")]
        public double? Confidence { get; set; }

        [JsonProperty("negative_prob")]
        public double? NegativeProb { get; set; }

        [JsonProperty("positive_prob")]
        public double? PositiveProb { get; set; }

        [JsonProperty("sentiment")]
        public int? Sentiment { get; set; }
    }

    /// <summary>
    /// 文章标签分析
    /// </summary>
    public class KeywordModel
    {
        [JsonProperty("log_id")]
        public long? LogId { get; set; }

        [JsonProperty("items")]
        public List<KeywordModelItem>? Items { get; set; }
    }

    /// <summary>
    /// 文章标签分析
    /// </summary>
    public class KeywordModelItem
    {
        [JsonProperty("score")]
        public required double Score { get; set; }

        [JsonProperty("tag")]
        public required string Tag { get; set; }
    }

    /// <summary>
    /// 文章分类
    /// </summary>
    public class TopicModel
    {
        [JsonProperty("log_id")]
        public long? LogId { get; set; }

        [JsonProperty("item")]
        public TopicModelItemModel? Item { get; set; }
    }

    /// <summary>
    /// 文章分类
    /// （复用文章标签Item类）
    /// </summary>
    public class TopicModelItemModel
    {
        [JsonProperty("lv2_tag_list")]
        public required List<KeywordModelItem> Lv2TagList { get; set; }

        [JsonProperty("lv1_tag_list")]
        public required List<KeywordModelItem> Lv1TagList { get; set; }
    }

}
