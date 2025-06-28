using ChatRoomLibrary.Core;

namespace ChatRoomServer
{
    internal static class Program
    {
        private static IWebApiServer? _httpServer;
        private static Thread? _serverThread;

        /// <summary>
        /// ������״̬
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

            // ����HTTP������
            StartHttpServer();

            Application.Run(new ChatRoomServerForm());

            // ֹͣ������
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
                System.Diagnostics.Debug.WriteLine("HTTP��������������������ַ: http://localhost:8080");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����������ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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