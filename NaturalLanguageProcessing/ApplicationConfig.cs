using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalLanguageProcessing
{
    /// <summary>
    /// 应用配置
    /// </summary>
    class ApplicationConfig
    {
        public static string AppId
        {
            get
            {
                return "";
            }
        }
        public static string ApiKey
        {
            get
            {
                return "";
            }
        }
        public static string SecretKey
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// JSON序列化解析器
        /// </summary>
        public static JsonSerializerSettings JsonSerializerCamelCaseSetting
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                return settings;
            }
        }

        private ApplicationConfig()
        {
        }

    }
}
