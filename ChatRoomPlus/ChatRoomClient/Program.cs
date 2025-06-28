namespace ChatRoomClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                // 自动登录
                ClientCore.LoadTokenFromLocal().Wait();
                Application.Run(new ChatRoomClientForm());
            }
            catch (Exception ex)
            {
                // 自动登录失败 调用登录窗口登录
                System.Diagnostics.Debug.WriteLine("自动登录失败！" + ex.Message);
                using UserLoginForm userLoginForm = new();
                userLoginForm.StartPosition = FormStartPosition.CenterParent;
                if (userLoginForm.ShowDialog() == DialogResult.OK)
                {
                    // 登录成功
                    Application.Run(new ChatRoomClientForm());
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("取消登录");
                }
            }
        }
    }
}