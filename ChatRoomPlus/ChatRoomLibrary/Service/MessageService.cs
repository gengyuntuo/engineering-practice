using ChatRoomLibrary.Data;
using ChatRoomLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomLibrary.Service
{
    public class MessageService
    {
        private readonly IServerDatabase _dataAccess = IServerDatabaseFactory.CreateSqliteServerDatabase();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="fromUserId">发送者ID</param>
        /// <param name="toUserId">接收者ID</param>
        /// <param name="messageContent">消息内容</param>
        /// <param name="isSystem">是否为系统消息</param>
        /// <returns>发送状态，true表示成功</returns>
        public async Task<bool> SendMessage(int fromUserId, int toUserId, string messageContent, bool isSystem, DateTime? sendTime)
        {
            var message = new Message()
            {
                SenderId = fromUserId,
                ReceiverId = toUserId,
                Content = messageContent,
                IsSystem = isSystem,
                SentAt = sendTime ?? DateTime.Now,
            };
            var affectRows = await _dataAccess.CreateMessageAsync(message);
            return affectRows > 0;
        }

        /// <summary>
        /// 获取用户未读消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Message>> QueryUnreadMessage(int userId)
        {
            return await _dataAccess.GetUnreadMessageByUserIdAsync(userId);
        }

        /// <summary>
        /// 标记未读消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="messageIds"></param>
        /// <returns></returns>
        public async Task<int> AckMessage(int userId, List<int> messageIds)
        {
            return await _dataAccess.MarkMessageAsReadAsync(userId, messageIds);
        }
    }
}
