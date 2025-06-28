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
                // �Զ���¼
                ClientCore.LoadTokenFromLocal().Wait();
                Application.Run(new ChatRoomClientForm());
            }
            catch (Exception ex)
            {
                // �Զ���¼ʧ�� ���õ�¼���ڵ�¼
                System.Diagnostics.Debug.WriteLine("�Զ���¼ʧ�ܣ�" + ex.Message);
                using UserLoginForm userLoginForm = new();
                userLoginForm.StartPosition = FormStartPosition.CenterParent;
                if (userLoginForm.ShowDialog() == DialogResult.OK)
                {
                    // ��¼�ɹ�
                    Application.Run(new ChatRoomClientForm());
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ȡ����¼");
                }
            }
        }
    }
}