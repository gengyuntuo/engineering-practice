using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.Model
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// JSON 响应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonResponse<T>
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public T? Data { get; set; }
    }

    /// <summary>
    /// 系统消息
    /// </summary>
    public class SystemMessage
    {
        /// <summary>
        /// 系统消息命令
        /// server_start  : 服务器启动
        /// friend_add    : 添加好友
        /// friend_del    : 删除好友
        /// friend_online : 好友上线
        /// friend_offline: 好友下线
        /// </summary>
        public required string Command { get; set; }
        /// <summary>
        /// 命令附加参数
        /// </summary>
        public Dictionary<string, string>? ExtraParameters { get; set; }
    }
}
