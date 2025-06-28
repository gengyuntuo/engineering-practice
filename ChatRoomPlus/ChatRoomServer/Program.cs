using ChatRoomLibrary.Core;

namespace ChatRoomServer
{
    internal static class Program
    {
        private static IWebApiServer? _httpServer;
        private static Thread? _serverThread;

        /// <summary>
        /// 服务器状态
        /// </summary>
        public static bool ApiServerStatus { get { return _httpServer?.ServerStatus() ?? false; } }

        public static List<int> OnlineUsers { get { return _httpServer?.GetOnlineUserJudgeByHeartbeat() ?? []; } }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // 启动HTTP服务器
            StartHttpServer();

            Application.Run(new ChatRoomServerForm());

            // 停止服务器
            StopHttpServer();
        }

        private static void StartHttpServer()
        {

            try
            {
                _httpServer = WebApiServerFactory.CreateWebApiServer();
                _serverThread = new(_httpServer.StartServer)
                {
                    IsBackground = true
                };
                _serverThread.Start();
                System.Diagnostics.Debug.WriteLine("HTTP服务器已启动，监听地址: http://localhost:8080");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动服务器时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private static void StopHttpServer()
        {
            _httpServer?.StopServer();
            _serverThread?.Join(1000);
        }
    }
}