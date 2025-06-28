using ChatRoomLibrary.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.Util
{
    public class ServerRequestUtil
    {

        /// <summary>
        /// 从HTTP请求中获取其JSON对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<T?> ReadJsonFromRequest<T>(HttpListenerRequest request)
        {
            try
            {
                // 读取请求体
                using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                var requestBody = await reader.ReadToEndAsync();
                var requestModel = JsonConvert.DeserializeObject<T>(requestBody);
                return requestModel;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("将请求Body转Model时失败: " + e);
                return default;
            }
        }
    }
}
