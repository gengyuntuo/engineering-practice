using ChatRoomLibrary.AppException;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace ChatRoomLibrary.Util
{
    /// <summary>
    /// Http Client Util
    /// </summary>
    public class HttpClientUtil
    {
        private static readonly HttpClient _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(10)  // 全局超时时间为 10 秒
        };

        public static async Task<O?> PostAsync<I, O>(string uri, I payload, Dictionary<string, string>? headers)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                // 创建自定义请求
                var request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = jsonContent
                };

                // 添加请求特定的 Header
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                // 发送请求
                var response = await _httpClient.SendAsync(request);
                // response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"POST: {uri}  Response: {responseContent}");
                var requestModel = JsonConvert.DeserializeObject<O>(responseContent);
                return requestModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"POST: {uri}  Faild: {ex}");
                throw new HttpException();
            }
        }
    }
}
