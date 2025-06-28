using ChatRoomLibrary.Core;
using ChatRoomLibrary.Data;
using ChatRoomLibrary.Model;
using ChatRoomLibrary.Service;
using ChatRoomLibrary.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatRoomClient
{
    public class Constant
    {
        public static string ServerHost => "http://localhost:8080";
        public static string FriendListConfig => "application.friend.list";
    }
    public class ClientCore
    {
        private static readonly JwtService jwtService = new();
        private static readonly IClientDatabase dataAccess = IClientDatabaseFactory.CreateSqliteDatabase();
        /// <summary>
        /// Token
        /// </summary>
        public static string? Token { get; set; }
        /// <summary>
        /// 当前用户
        /// </summary>
        public static User? User { get; internal set; }

        public async static void SaveToken(string? token)
        {
            User? user = jwtService.ValidateToken(token) ?? throw new Exception("无效的Token");
            var config = new Config()
            {
                Key = "application.token",
                Value = token ?? "",
                Description = "User Token",
                Type = "string",
                IsSystem = true,
            };
            await dataAccess.UpsertConfigAsync(config);

            System.Diagnostics.Debug.WriteLine($"登录成功, 用户: {JsonConvert.SerializeObject(user)}");
            ClientCore.Token = token;
            ClientCore.User = user;
        }

        /// <summary>
        /// 存储配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        /// <param name="isSystem"></param>
        /// <returns></returns>
        public async static Task<bool> SaveConfig(string key, string? value, string description = "No Description.", bool isSystem = false)
        {
            var config = new Config()
            {
                Key = key,
                Value = value ?? "",
                Description = description,
                Type = "string",
                IsSystem = true,
            };
            return await dataAccess.UpsertConfigAsync(config) > 0;
        }
        public async static Task<string?> LoadConfig(string key)
        {
            var config = await dataAccess.GetConfigByKeyAsync(key);
            return config?.Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        public static async Task LoadTokenFromLocal()
        {
            var config = await dataAccess.GetConfigByKeyAsync("application.token") ?? throw new Exception("当前Token为空");
            User? user = jwtService.ValidateToken(config.Value) ?? throw new Exception("无效的Token");
            System.Diagnostics.Debug.WriteLine($"自动登录成功, 用户: {JsonConvert.SerializeObject(user)}");
            ClientCore.Token = config.Value;
            ClientCore.User = user;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        public static void SignOut()
        {
            var config = new Config()
            {
                Key = "application.token",
                Value = "",
                Description = "User Token",
                Type = "string",
                IsSystem = true,
            };
            dataAccess.UpsertConfigAsync(config).Wait();
        }

        public static async Task<List<ChatRoomLibrary.Model.Message>> LoadMessageFromLocal(int userId, int friendUserId)
        {
            return await dataAccess.GetLocalMessageByFriendUserIdAsync(userId, friendUserId);
        }

        public static async Task<int> SaveMessageToLocal(ChatRoomLibrary.Model.Message message)
        {
            return await dataAccess.CreateMessageAsync(message);
        }

        /// <summary>
        /// 将本地的消息标记为已读
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static async Task<int> MarkLocalMessageAsRead(List<int> messageIds)
        {
            return await dataAccess.MarkMessageAsReadAsync(ClientCore.User?.Id ?? -1, messageIds);
        }

        public static async Task<List<MessageStatisticItem>> GetCurrentUserMessageStatistic()
        {
            return await dataAccess.GetMessageStatisticByUserIdAsync(ClientCore.User?.Id ?? -1);
        }

        public static void PlayMessageMusic()
        {
            try
            {
                // 获取应用程序目录
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                // 加载相对路径下的音频文件
                string audioPath = Path.Combine(appDir, "Sounds", "message.wav");
                if (!File.Exists(audioPath))
                {
                    throw new Exception("未找到文件: " + audioPath);
                }
                // 使用 SoundPlayer 播放
                using var player = new SoundPlayer(audioPath);
                player.Play();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"播放声音失败: {e}");
            }
        }

    }

    /// <summary>
    /// 服务端接口调用工具类
    /// </summary>
    public class ServerApiInvoker
    {
        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <returns></returns>
        public static async Task<List<ChatRoomLibrary.Model.Message>> QueryUnreadMessages()
        {
            try
            {
                var registerUrl = $"{Constant.ServerHost}{GlobalConstant.ApiNameMessageUnread}";
                var result = await HttpClientUtil.PostAsync<Dictionary<string, string>, JsonResponse<List<ChatRoomLibrary.Model.Message>>>(
                    registerUrl,
                    [],
                    new Dictionary<string, string>() { { "Authorization", $"Bearer {ClientCore.Token}" }, }
                    );

                if (result?.Code != 0)
                {
                    throw new Exception(result?.Message ?? "未知异常");
                }

                // 登录成功，关闭登录窗口
                // MessageBox.Show(this, "登录成功！" + result?.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return result?.Data ?? [];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从服务器拉取消息失败: {ex}");
                throw;
            }
        }

        public static async Task<bool> SendMessage(ChatRoomLibrary.Model.Message message)
        {
            try
            {
                var registerUrl = $"{Constant.ServerHost}{GlobalConstant.ApiNameMessageSend}";
                var result = await HttpClientUtil.PostAsync<ChatRoomLibrary.Model.Message, JsonResponse<bool>>(
                    registerUrl,
                    message,
                    new Dictionary<string, string>() { { "Authorization", $"Bearer {ClientCore.Token}" }, }
                    );
                if (result?.Code != 0)
                {
                    throw new Exception(result?.Message ?? "未知异常");
                }
                return result?.Data ?? false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从服务器拉取消息失败: {ex}");
                throw;
            }
        }

        /// <summary>
        /// 标记服务端消息为已读状态
        /// </summary>
        /// <param name="messageIds"></param>
        /// <returns></returns>
        public static async Task MarkMessageAsRead(params int[] messageIds)
        {
            try
            {
                var registerUrl = $"{Constant.ServerHost}{GlobalConstant.ApiNameMessageAck}";
                var result = await HttpClientUtil.PostAsync<List<int>, JsonResponse<int>>(
                    registerUrl,
                    [.. messageIds],
                    new Dictionary<string, string>() { { "Authorization", $"Bearer {ClientCore.Token}" }, }
                    );

                if (result?.Code != 0)
                {
                    throw new Exception(result?.Message ?? "未知异常");
                }
            }
            catch (Exception ex)
            {
                // 登录失败
                System.Diagnostics.Debug.WriteLine($"标记Message为已读状态失败: {ex}");
                return;
            }
        }

        /// <summary>
        /// 查询好友列表
        /// </summary>
        /// <returns></returns>
        public static async Task<List<User>> QueryFriends()
        {
            try
            {
                var registerUrl = $"{Constant.ServerHost}{GlobalConstant.ApiNameFriendList}";
                var result = await HttpClientUtil.PostAsync<int, JsonResponse<List<User>>>(
                    registerUrl,
                    ClientCore.User?.Id ?? -1,
                    new Dictionary<string, string>() { { "Authorization", $"Bearer {ClientCore.Token}" }, });
                if (result?.Code != 0)
                {
                    throw new Exception(result?.Message ?? "未知异常");
                }
                return result?.Data ?? [];
            }
            catch (Exception ex)
            {
                // 登录失败
                System.Diagnostics.Debug.WriteLine($"登录失败: {ex}");
                throw;
            }
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="friendUsername"></param>
        /// <returns></returns>
        public static async Task<bool> AddFriend(string friendUsername)
        {
            try
            {
                var registerUrl = $"{Constant.ServerHost}{GlobalConstant.ApiNameFriendAdd}";
                var result = await HttpClientUtil.PostAsync<string, JsonResponse<bool>>(
                    registerUrl,
                    friendUsername,
                    new Dictionary<string, string>() { { "Authorization", $"Bearer {ClientCore.Token}" }, }
                    );
                if (result?.Code != 0)
                {
                    throw new Exception(result?.Message ?? "未知异常");
                }
                return result?.Data ?? false;
            }
            catch (Exception ex)
            {
                // 登录失败
                System.Diagnostics.Debug.WriteLine($"登录失败: {ex}");
                return false;
            }
        }

    }
}
