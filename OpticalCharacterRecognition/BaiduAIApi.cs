using Baidu.Aip.Ocr;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OpticalCharacterRecognition
{
    /// <summary>
    /// Baidu API Invoker
    /// </summary>
    class BaiduAIApi
    {
        private static readonly BaiduAIApi _INSTANCE = new();
        public static BaiduAIApi INSTANCE
        {
            get { return _INSTANCE; }
        }
        private readonly Ocr ocrClient;
        private string? AccessToken = null;
        private long AccessTokenGenTimestamp = 0;
        private BaiduAIApi()
        {
            ocrClient = new Baidu.Aip.Ocr.Ocr(BaiduAIConfig.AccessKey, BaiduAIConfig.SecretKey);
            ocrClient.Timeout = 60000;
        }

        /// <summary>
        /// 通用文字识别
        /// </summary>
        /// <param name="pictureLocation">图片路径</param>
        /// <param name="precision">精度,取值: 普通精度, 高精度</param>
        /// <returns>文字</returns>
        public PlainRecognizeResponse PlainRecognize(string pictureLocation, string precision)
        {
            var pictureData = LoadPicture(pictureLocation);
            var options = new Dictionary<string, object>{
                {"recognize_granularity", "big"},
                {"language_type", "CHN_ENG"},
                {"detect_direction", "true"},
                {"detect_language", "true"},
                // {"vertexes_location", "true"},
                {"probability", "true"}
            };
            try
            {
                var result = "高精度".Equals(precision) ? ocrClient.AccurateBasic(pictureData, options) : ocrClient.General(pictureData, options);
                return result.ToObject<PlainRecognizeResponse>();
            }
            catch (Exception e)
            {
                throw new Exception("请求失败!" + e.Message);
            }
        }

        /// <summary>
        /// 网络图片识别
        /// </summary>
        /// <param name="pictureLocation">图片位置</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public PlainRecognizeResponse WebPictureRecognize(string pictureLocation)
        {
            var pictureData = LoadPicture(pictureLocation);
            var options = new Dictionary<string, object>{
                {"detect_direction", "true"},
                {"detect_language", "true"}
            };
            try
            {
                var result = ocrClient.WebImage(pictureData, options);
                return result.ToObject<PlainRecognizeResponse>();
            }
            catch (Exception e)
            {
                throw new Exception("请求失败!" + e.Message);
            }
        }

        /// <summary>
        /// 身份证识别
        /// </summary>
        /// <param name="pictureLocation">图片位置</param>
        /// <param name="idCardSide">front - 身份证含照片的一面 back - 身份证带国徽的一面</param>
        /// <exception cref="Exception"></exception>
        public CardInfoResponse IdCardPictureRecognize(string pictureLocation, string idCardSide)
        {
            var pictureData = LoadPicture(pictureLocation);
            var options = new Dictionary<string, object>{
                {"detect_direction", "true"},
                {"detect_risk", "false"}
            };
            try
            {
                var result = ocrClient.Idcard(pictureData, idCardSide, options);
                return result.ToObject<CardInfoResponse>();
            }
            catch (Exception e)
            {
                throw new Exception("请求失败!" + e.Message);
            }
        }

        /// <summary>
        /// 银行卡识别
        /// </summary>
        /// <param name="pictureLocation">图片位置</param>
        /// <exception cref="Exception"></exception>
        public BankCardInfoResponse BankCardRecognize(string pictureLocation)
        {
            var pictureData = LoadPicture(pictureLocation);
            try
            {
                var result = ocrClient.Bankcard(pictureData);
                return result.ToObject<BankCardInfoResponse>();
            }
            catch (Exception e)
            {
                throw new Exception("请求失败!" + e.Message);
            }
        }

        /// <summary>
        /// 营业执照识别
        /// </summary>
        /// <param name="pictureLocation">图片位置</param>
        /// <exception cref="Exception"></exception>
        public CardInfoResponse BusinessLicenseRecognize(string pictureLocation)
        {
            var pictureData = LoadPicture(pictureLocation);
            try
            {
                var result = ocrClient.BusinessLicense(pictureData);
                return result.ToObject<CardInfoResponse>();
            }
            catch (Exception e)
            {
                throw new Exception("请求失败!" + e.Message);
            }
        }

        /// <summary>
        /// 表格识别
        /// </summary>
        /// <param name="pictureLocation">图片位置</param>
        /// <exception cref="Exception"></exception>
        public void TablePictureRecognize(string pictureLocation)
        {
            var pictureData = LoadPicture(pictureLocation);
            var options = new Dictionary<string, object>{
                {"detect_direction", "true"},
                {"detect_language", "true"}
            };
            try
            {
                var result = ocrClient.WebImage(pictureData, options);
            }
            catch (Exception e)
            {
                throw new Exception("请求失败!" + e.Message);
            }
        }



        private static byte[] LoadPicture(string pictureLocation)
        {
            if (!File.Exists(pictureLocation))
            {
                throw new Exception("图片不存在，请重新指定图片路径");
            }
            return File.ReadAllBytes(pictureLocation);
        }

        private string GenerateToken()
        {
            // 垃圾百度文档，误导我，AKSK参数位置互换
            string host = $"https://aip.baidubce.com/oauth/2.0/token?client_id={BaiduAIConfig.AccessKey}&client_secret={BaiduAIConfig.SecretKey}&grant_type=client_credentials";
            using (HttpClient httpClient = new HttpClient())
            {
                var content = new StringContent(@"", Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync(host, content).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                return JObject.Parse(response).Value<string>("access_token");
            }
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string GetAccessToken()
        {
            try
            {
                // access token超过10天或者不存在时获取Token
                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - AccessTokenGenTimestamp > 3600 * 24 * 10 || AccessToken == null)
                {
                    AccessToken = GenerateToken();
                    AccessTokenGenTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }
                return AccessToken;
            }
            catch (Exception e)
            {
                throw new Exception("获取Token失败! " + e.Message);
            }
        }

        /// <summary>
        /// 表格识别
        /// </summary>
        /// <param name="pictureLocation"></param>
        /// <returns></returns>
        public string TableRecognize(string pictureLocation)
        {
            try
            {
                // string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/table?access_token=" + getAccessToken();
                // using (HttpClient httpClient = new())
                // {
                //     httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                //     // 图片的base64编码
                //     string base64Image = Convert.ToBase64String(LoadPicture(pictureLocation));
                //     string encodeImage = HttpUtility.UrlEncode(base64Image);
                //     var formData = new Dictionary<string, string>
                //     {
                //         { "image", base64Image }
                //     };
                //     // 创建 FormUrlEncodedContent 对象
                //     var content = new FormUrlEncodedContent(formData);
                //     // 设置 Content-Type 请求头
                //     content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                //     var result = httpClient.PostAsync(host, content).Result; 
                //     return result.Content.ReadAsStringAsync().Result;
                // }
                // 图片的base64编码
                string base64Image = Convert.ToBase64String(LoadPicture(pictureLocation));
                var client = new RestClient($"https://aip.baidubce.com/rest/2.0/ocr/v1/table?access_token={GetAccessToken()}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Accept", "application/json");
                request.AddParameter("image", base64Image);
                request.AddParameter("cell_contents", "false");
                request.AddParameter("return_excel", "false");
                IRestResponse response = client.Execute(request);
                return JObject.Parse(response.Content).ToString();
            }
            catch (Exception e)
            {
                throw new Exception("请求失败! " + e.Message);
            }
        }
    }
}
