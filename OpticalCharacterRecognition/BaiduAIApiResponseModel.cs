using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticalCharacterRecognition
{
    /// <summary>
    /// 将JSON写成C#的值对象, 使用Newtonsoft.Json.Linq，用注解，使其符合C#命名规范
    /// </summary>
    public class PlainRecognizeResponse
    {
        [JsonProperty("words_result")]
        public List<WordsResult>? WordsResult { get; set; }

        [JsonProperty("direction")]
        public int Direction { get; set; }

        [JsonProperty("language")]
        public int Language { get; set; }

        [JsonProperty("words_result_num")]
        public int WordsResultNum { get; set; }

        [JsonProperty("log_id")]
        public long LogId { get; set; }
    }

    public class WordsResult
    {
        [JsonProperty("probability")]
        public Probability? Probability { get; set; }

        [JsonProperty("words")]
        public string? Words { get; set; }

        [JsonProperty("location")]
        public Location? Location { get; set; }
    }

    public class Probability
    {
        [JsonProperty("average")]
        public double Average { get; set; }

        [JsonProperty("min")]
        public double Min { get; set; }

        [JsonProperty("variance")]
        public double Variance { get; set; }
    }

    public class Location
    {
        [JsonProperty("top")]
        public int Top { get; set; }

        [JsonProperty("left")]
        public int Left { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class BankCardInfoResponse
    {
        [JsonProperty("log_id")]
        public long LogId { get; set; }

        [JsonProperty("result")]
        public BankCardResult? Result { get; set; }
    }

    public class BankCardResult
    {
        [JsonProperty("bank_card_number")]
        public string? BankCardNumber { get; set; }

        [JsonProperty("bank_name")]
        public string? BankName { get; set; }

        [JsonProperty("bank_card_type")]
        public int BankCardType { get; set; }
    }

    public class CardInfoResponse
    {
        [JsonProperty("log_id")]
        public long LogId { get; set; }

        [JsonProperty("words_result")]
        public Dictionary<string, WordResult>? WordsResult { get; set; }

        [JsonProperty("words_result_num")]
        public int WordsResultNum { get; set; }

        [JsonProperty("direction")]
        public int? Direction { get; set; }
    }

    public class WordResult
    {
        [JsonProperty("location")]
        public Location? Location { get; set; }

        [JsonProperty("words")]
        public string? Words { get; set; }
    }
}
