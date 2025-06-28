using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.AppException
{
    /// <summary>
    /// API 授权异常
    /// </summary>
    /// <param name="message"></param>
    public class ApiAuthException(string message) : Exception(message)
    {
    }

    /// <summary>
    /// Http Exception
    /// </summary>
    /// <param name="message"></param>
    public class HttpException(string message) : Exception(message)
    {
        public HttpException() : this("网络故障")
        {
        }
    }
}
