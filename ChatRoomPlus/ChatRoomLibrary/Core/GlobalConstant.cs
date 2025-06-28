using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.Core
{
    public class GlobalConstant
    {
        public static string ApiNameRegister = "/api/user/register";
        public static string ApiNameLogin => "/api/user/login";
        public static string ApiNameFriendAdd => "/api/user/friend/add";
        public static string ApiNameFriendDel => "/api/user/firend/del";
        public static string ApiNameFriendList => "/api/user/firend/list";
        public static string ApiNameMessageUnread => "/api/msg/unread";
        public static string ApiNameMessageSend => "/api/msg/send";
        public static string ApiNameMessageAck => "/api/msg/ack";


        /// <summary>
        /// 系统消息：命令名称
        /// </summary>
        public static readonly string ServerStart = "server_start";
        public static readonly string FriendAdd = "friend_add";
        public static readonly string FriendDel = "friend_del";
        public static readonly string FriendOnline = "friend_online";
        public static readonly string FriendOffline = "friend_offline";
    }
}
