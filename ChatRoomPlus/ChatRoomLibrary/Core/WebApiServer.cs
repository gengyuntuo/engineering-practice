using ChatRoomLibrary.AppException;
using ChatRoomLibrary.Core;
using ChatRoomLibrary.Model;
using ChatRoomLibrary.Service;
using ChatRoomLibrary.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace ChatRoomLibrary.Core
{
    /// <summary>
    /// WebApiServer工厂类
    /// </summary>
    public class WebApiServerFactory
    {
        public static IWebApiServer CreateWebApiServer()
        {
            return new HttpListenerWebApiServer();
        }

    }

    /// <summary>
    /// Web服务器接口
    /// </summary>
    public interface IWebApiServer
    {
        /// <summary>
        /// 启动服务器
        /// </summary>
        void StartServer();

        /// <summary>
        /// 关闭服务器
        /// </summary>
        void StopServer();

        /// <summary>
        /// 服务器状态
        /// </summary>
        /// <returns></returns>
        bool ServerStatus();

        /// <summary>
        /// 获取用户最后一次上报HeartBeat的时间
        /// 
        /// 最近10秒没有心跳数据的用户被视为下线
        /// </summary>
        /// <returns>用户id</returns>
        List<int> GetOnlineUserJudgeByHeartbeat();

    }

    /// <summary>
    /// 基于 HttpListener 实现的Web服务器接口
    /// </summary>
    class HttpListenerWebApiServer : IWebApiServer
    {
        private readonly string defaultHostAddress = "http://localhost:8080/";
        private readonly HttpListener _listener = new();
        /// <summary>
        /// 记录用户心跳上报的时间
        /// </summary>
        private readonly ConcurrentDictionary<int, long> _userHeatbeat = new();

        /// <summary>
        /// 服务器启动状态
        /// </summary>
        private bool _isRunning;

        private readonly UserService _userService = new();
        private readonly JwtService _jwtService = new();
        private readonly MessageService _messageService = new();

        public HttpListenerWebApiServer()
        {
            _listener.Prefixes.Add(defaultHostAddress);
        }

        /// <summary>
        /// Server启动后，向所有用户发送启动消息
        /// </summary>
        private async Task NotifyAllUsers()
        {
            // 获取系统用户的好友，所有的个人用户在注册的时候都会添加系统用户为好友，系统用户的Id为0，用户名为system
            var users = await _userService.GetFriendsByUserIdAsync(0);
            var message = JsonConvert.SerializeObject(new SystemMessage() { Command = GlobalConstant.ServerStart, ExtraParameters = { } });
            foreach (var user in users)
            {
                await _messageService.SendMessage(0, user.Id, message, true, null);
            }
        }

        public void StartServer()
        {
            try
            {
                NotifyAllUsers().Wait();
                _listener.Start();
                _isRunning = true;

                while (_isRunning)
                {
                    var context = _listener.GetContext();
                    _ = HandleRequestAsync(context);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"服务器错误: {ex.Message}");
            }
            finally
            {
                _isRunning = false;
                _listener.Close();
            }
        }

        public void StopServer()
        {
            _isRunning = false;
            _listener.Close();
        }

        /// <summary>
        /// 异步处理接口响应
        /// 
        /// 接口列表：
        /// 1. 用户相关
        ///    1. 用户注册 POST /api/user/register
        ///    2. 用户登录 POST /api/user/login
        ///    3. 建立好友 POST /api/user/friend/add
        ///    3. 删除好友 POST /api/user/firend/del
        ///    4. 好友列表 POST /api/user/firend/list
        /// 2. 消息相关
        ///    1. 查询未读消息 POST /api/msg/unread
        ///    2. 标记已读消息 POST /api/msg/ack
        ///    3. 发送消息     POST /api/msg/send
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task HandleRequestAsync(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                var response = context.Response;

                System.Diagnostics.Debug.WriteLine($"收到 {request.HttpMethod} 请求: {request.Url}");

                var apiName = request?.Url?.AbsolutePath.ToLower();
                User? user = null;
                if ((apiName ?? "").StartsWith("/api/user/register")
                    || (apiName ?? "").StartsWith("/api/user/login")
                    || (apiName ?? "").StartsWith("/api/check/health")
                    )
                {
                    // 注册 登录 健康检查 接口不检验 TOKEN
                }
                else
                {
                    // 其余情况均校验
                    try
                    {
                        user = CheckAuth(request);
                        // 记录用户心跳数据
                        if (user != null)
                        {
                            var currentTimestamap = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            this._userHeatbeat.AddOrUpdate(user.Id, currentTimestamap, (key, oldValue) => currentTimestamap);
                        }
                    }
                    catch (ApiAuthException e)
                    {
                        SendJsonResponse(
                            response,
                            HttpStatusCode.Unauthorized,
                            new JsonResponse<Object> { Code = 10, Message = e.Message }
                            );
                        return;
                    }
                }

                // 路由处理
                switch (request?.Url?.AbsolutePath.ToLower())
                {
                    case "/api/user/register":
                        await HandleRegisterAsync(request, response);
                        break;
                    case "/api/user/login":
                        await HandleLoginAsync(request, response);
                        break;
                    case "/api/user/friend/add":
                        await HandleFriendAdd(request, response, user);
                        break;
                    case "/api/user/firend/del":
                        await HandleFriendDel(request, response, user);
                        break;
                    case "/api/user/firend/list":
                        await HandleFriendList(request, response, user);
                        break;
                    case "/api/msg/unread":
                        await HandleUnReadMessageAsync(request, response, user);
                        break;
                    case "/api/msg/ack":
                        await HandleMessageAckAsync(request, response, user);
                        break;
                    case "/api/msg/send":
                        await HandleMessageSendAsync(request, response, user);
                        break;
                    case "/api/check/health":
                        HandleTestAuthAsync(request, response);
                        break;
                    default:
                        SendJsonResponse(
                            response,
                            HttpStatusCode.NotFound,
                            new JsonResponse<Object> { Code = 404, Message = "未找到资源" }
                            );
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理请求时出错: {ex.Message}");
                SendJsonResponse(
                    context.Response,
                    HttpStatusCode.InternalServerError,
                    new JsonResponse<Object> { Code = 500, Message = "服务器内部错误" }
                    );
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task HandleMessageSendAsync(HttpListenerRequest request, HttpListenerResponse response, User? user)
        {
            try
            {
                if (request.HttpMethod != "POST")
                {
                    SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<Object> { Code = 20, Message = "方法不允许" });
                    return;
                }
                ArgumentNullException.ThrowIfNull(user);
                // 读取请求体
                var message = await ServerRequestUtil.ReadJsonFromRequest<Message>(request) ?? throw new Exception("请求中没有消息");
                var result = await _messageService.SendMessage(user.Id, message.ReceiverId, message.Content, false, null);
                SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<bool> { Code = 0, Message = "Success", Data = result });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"发送消息({user})失败: {e}");
                SendJsonResponse(response, HttpStatusCode.OK, new JsonResponse<object> { Code = 80, Message = e.Message });
            }
        }

        private async Task HandleFriendList(HttpListenerRequest request, HttpListenerResponse response, User? user)
        {
            try
            {
                if (request.HttpMethod != "POST")
                {
                    SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<Object> { Code = 20, Message = "方法不允许" });
                    return;
                }
                ArgumentNullException.ThrowIfNull(user);
                // 读取请求体
                var users = await _userService.GetFriendsByUserIdAsync(user.Id);
                SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<List<User>> { Code = 0, Message = "Success", Data = users });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"查询好友列表({user})失败: {e}");
                SendJsonResponse(response, HttpStatusCode.OK, new JsonResponse<object> { Code = 30, Message = e.Message });
            }
        }

        private async Task HandleFriendDel(HttpListenerRequest request, HttpListenerResponse response, User? user)
        {
            try
            {
                if (request.HttpMethod != "POST")
                {
                    SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<Object> { Code = 20, Message = "方法不允许" });
                    return;
                }
                ArgumentNullException.ThrowIfNull(user);
                // 读取请求体
                var friendUsername = await ServerRequestUtil.ReadJsonFromRequest<string>(request) ?? throw new Exception("请输入好友姓名");
                var friendUser = await _userService.GetUserByUsernameAsycn(friendUsername) ?? throw new Exception("好友不存在");
                await _userService.CancelFriendshipAsync(user.Id, friendUser.Id);
                SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<bool> { Code = 0, Message = "Success", Data = true });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"移除好友失败: {e}");
                SendJsonResponse(response, HttpStatusCode.OK, new JsonResponse<object> { Code = 60, Message = e.Message });
            }
        }

        private async Task HandleFriendAdd(HttpListenerRequest request, HttpListenerResponse response, User? user)
        {
            try
            {
                if (request.HttpMethod != "POST")
                {
                    SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<Object> { Code = 20, Message = "方法不允许" });
                    return;
                }
                ArgumentNullException.ThrowIfNull(user);
                // 读取请求体
                var friendUsername = await ServerRequestUtil.ReadJsonFromRequest<string>(request) ?? throw new Exception("请输入好友姓名");
                
                // 添加用户
                var friendUser = await _userService.GetUserByUsernameAsycn(friendUsername) ?? throw new Exception("好友不存在");
                await _userService.CreateFriendshipAsync(user.Id, friendUser.Id);
                
                // 向两个用户添加系统消息，通知用户新增事件
                var systemMessage = JsonConvert.SerializeObject(new SystemMessage()
                {
                    Command = "add_friend",
                    ExtraParameters = [],
                });
                await _messageService.SendMessage(0, friendUser.Id, systemMessage, true, null);
                await _messageService.SendMessage(0, user.Id, systemMessage, true, null);

                SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<bool> { Code = 0, Message = "Success", Data = true });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"添加好友失败: {e}");
                SendJsonResponse(response, HttpStatusCode.OK, new JsonResponse<object> { Code = 50, Message = e.Message });
            }
        }

        private async Task HandleMessageAckAsync(HttpListenerRequest request, HttpListenerResponse response, User? user)
        {
            try
            {
                if (request.HttpMethod != "POST")
                {
                    SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<Object> { Code = 20, Message = "方法不允许" });
                    return;
                }
                ArgumentNullException.ThrowIfNull(user);
                var messageIds = await ServerRequestUtil.ReadJsonFromRequest<List<int>>(request);
                var affectRows = await _messageService.AckMessage(user.Id, messageIds ?? []);
                SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<int> { Code = 0, Message = "Success", Data = affectRows });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"查询未读消息({user})失败: {e}");
                SendJsonResponse(response, HttpStatusCode.OK, new JsonResponse<object> { Code = 20, Message = e.Message });
            }
        }

        private async Task HandleUnReadMessageAsync(HttpListenerRequest request, HttpListenerResponse response, User? user)
        {
            try
            {
                if (request.HttpMethod != "POST")
                {
                    SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<Object> { Code = 20, Message = "方法不允许" });
                    return;
                }
                ArgumentNullException.ThrowIfNull(user);
                // 读取请求体
                var messages = await _messageService.QueryUnreadMessage(user.Id);
                SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<List<Message>> { Code = 0, Message = "Success", Data = messages });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"查询未读消息({user})失败: {e}");
                SendJsonResponse(response, HttpStatusCode.OK, new JsonResponse<object> { Code = 20, Message = e.Message });
            }
        }

        /// <summary>
        /// 处理注册请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task HandleRegisterAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                if (request.HttpMethod != "POST")
                {
                    SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<Object> { Code = 20, Message = "方法不允许" });
                    return;
                }
                // 读取请求体
                var registerModel = await ServerRequestUtil.ReadJsonFromRequest<RegisterModel>(request);
                var user = await _userService.RegisterUserAsync(registerModel ?? new());
                // 创建与系统用户的好友关系
                await _userService.CreateFriendshipAsync(0, user.Id);
                SendJsonResponse(response, HttpStatusCode.MethodNotAllowed, new JsonResponse<Object> { Code = 0, Message = "Success", });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("注册失败: " + e.Message);
                SendJsonResponse(response, HttpStatusCode.OK, new JsonResponse<Object> { Code = 20, Message = e.Message });
            }
        }

        /// <summary>
        /// 处理登录请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task HandleLoginAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                // 读取请求体
                using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                var requestBody = await reader.ReadToEndAsync();
                var loginModel = JsonConvert.DeserializeObject<LoginModel>(requestBody);

                // 验证用户
                var user = await _userService.AuthenticateUserAsync(loginModel.Username, loginModel.Password);

                if (user == null)
                {
                    SendJsonResponse(response, HttpStatusCode.Unauthorized, new JsonResponse<object>() { Code = 1001, Message = "用户名或密码错误", Data = null });
                    return;
                }

                // 生成JWT令牌
                var token = _jwtService.GenerateToken(user);

                // 返回令牌
                SendJsonResponse(response, HttpStatusCode.OK, new JsonResponse<string>() { Code = 0, Message = "Success", Data = token });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"登录失败: {ex.Message}");
                SendJsonResponse(response, HttpStatusCode.InternalServerError, new JsonResponse<object>() { Code = 0, Message = "登录失败", Data = null });
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private void HandleTestAuthAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            SendResponse(response, HttpStatusCode.OK, "ChatRoomPlus status is OK.");
        }

        /// <summary>
        /// 响应HTTP请求：文本格式
        /// </summary>
        /// <param name="response"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        private void SendResponse(HttpListenerResponse response, HttpStatusCode statusCode, string message)
        {
            response.StatusCode = (int)statusCode;
            response.ContentType = "text/plain";
            response.ContentEncoding = Encoding.UTF8;

            using var writer = new StreamWriter(response.OutputStream);
            writer.Write(message);
            writer.Flush();
        }

        /// <summary>
        /// 响应HTTP请求：JSON格式
        /// </summary>
        /// <param name="response"></param>
        /// <param name="statusCode"></param>
        /// <param name="data"></param>
        private void SendJsonResponse(HttpListenerResponse response, HttpStatusCode statusCode, object data)
        {
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;

            var json = JsonConvert.SerializeObject(data);
            using var writer = new StreamWriter(response.OutputStream);
            writer.Write(json);
            writer.Flush();
        }

        /// <summary>
        /// JWT校验
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ApiAuthException"></exception>
        private User CheckAuth(HttpListenerRequest? request)
        {
            // 验证授权头
            if (request == null || !request.Headers.AllKeys.Contains("Authorization"))
            {
                throw new ApiAuthException("未提供授权头");
            }

            var authHeader = request.Headers["Authorization"];
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                throw new ApiAuthException("无效的授权头格式");
            }

            var token = authHeader["Bearer ".Length..].Trim();
            try
            {
                // 验证JWT令牌
                var user = _jwtService.ValidateToken(token);
                if (string.IsNullOrEmpty(user?.Username))
                {
                    throw new ApiAuthException("无效的令牌");
                }

                // 返回受保护的资源
                return user;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"验证令牌失败: {ex.Message}");
                throw new ApiAuthException("令牌验证失败");
            }
        }

        public bool ServerStatus()
        {
            return _isRunning;
        }

        public List<int> GetOnlineUserJudgeByHeartbeat()
        {
            return [.. _userHeatbeat.
                Where(entry => entry.Value + 1000 * 10 > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                .Select(entry => entry.Key)];
        }
    }

}

