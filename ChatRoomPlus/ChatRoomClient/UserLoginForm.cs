using ChatRoomLibrary.Core;
using ChatRoomLibrary.Model;
using ChatRoomLibrary.Service;
using ChatRoomLibrary.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatRoomClient
{
    public partial class UserLoginForm : Form
    {
        public UserLoginForm()
        {
            InitializeComponent();

            this.buttonLogin.Click += ButtonClickEventHandler;
            this.buttonRegister.Click += ButtonClickEventHandler;

            // For Test
            this.textBoxUserName.Text = "superuser";
            this.textBoxPassword.Text = "Abcd1234.";
        }

        private async void ButtonClickEventHandler(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                System.Diagnostics.Debug.WriteLine("未知事件源");
                return;
            }
            Button button = (Button)sender;

            try
            {
                button.Enabled = false;
                switch (button.Text)
                {
                    case "登录": await DoLogin(); break;
                    case "注册": DoRegister(); break;
                    default:
                        MessageBox.Show("未知的按钮: " + e, "错误");
                        break;
                }
            }
            finally
            {
                button.Enabled = true;
            }

        }

        /// <summary>
        /// 登录
        /// </summary>
        private async Task DoLogin()
        {
            try
            {
                var username = this.textBoxUserName.Text;
                var password = this.textBoxPassword.Text;

                var registerUrl = $"{Constant.ServerHost}{GlobalConstant.ApiNameLogin}";
                var result = await HttpClientUtil.PostAsync<LoginModel, JsonResponse<string>>(registerUrl, new LoginModel() { Username = username, Password = password }, null);

                if (result?.Code != 0)
                {
                    throw new Exception(result?.Message ?? "未知异常");
                }

                // 登录成功，关闭登录窗口
                // MessageBox.Show(this, "登录成功！" + result?.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var token = result?.Data;
                ClientCore.SaveToken(token);
                var friends = await ServerApiInvoker.QueryFriends() ?? [];
                await ClientCore.SaveConfig(Constant.FriendListConfig, JsonConvert.SerializeObject(friends));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // 登录失败
                System.Diagnostics.Debug.WriteLine($"登录失败: {ex}");
                MessageBox.Show(this, "登录失败: " + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        private void DoRegister()
        {
            using (UserRegisterForm userRegisterForm = new())
            {
                userRegisterForm.StartPosition = FormStartPosition.CenterParent;
                userRegisterForm.ShowDialog(this);
            }
        }
    }

}
