using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceApplication
{
    /// <summary>
    /// 应用配置类
    /// </summary>
    class Configuration
    {
        private static readonly string APP_ID = "";
        private static readonly string API_KEY = "";
        private static readonly string API_SECRET = "";

        public static string AppId
        {
            get { return APP_ID; }
        }

        public static string ApiSecret
        {
            get { return API_SECRET; }
        }

        public static string ApiKey
        {
            get { return API_KEY; }
        }
    }
}
