using ChatRoomLibrary.Core;
using ChatRoomLibrary.Model;
using ChatRoomLibrary.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatRoomClient
{
    public partial class UserRegisterForm : Form
    {
        public UserRegisterForm()
        {
            InitializeComponent();

            this.buttonRegister.Click += ButtonClickEventHandler;

            // For Test
            this.textBoxUsername.Text = "superuser";
            this.textBoxPassword.Text = "Abcd1234.";
            this.textBoxRepeatPassword.Text = "Abcd1234.";
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
                    case "注册": await DoRegister(); break;
                    default:
                        MessageBox.Show(this, "未知的按钮: " + e, "错误");
                        break;
                }
            }
            finally
            {
                button.Enabled = true;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        private async Task DoRegister()
        {
            var username = this.textBoxUsername.Text;
            var password = this.textBoxPassword.Text;

            try
            {
                if (username.Trim().Length != username.Length || username.Length < 6 || username.Length > 20)
                {
                    throw new Exception("用户名长度为 6 - 20");
                }
                // 正则表达式：至少一个数字、一个大写字母、一个小写字母、一个特殊字符，长度8-32位
                string pattern = @"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,32}$";
                if (!Regex.IsMatch(password, pattern))
                {
                    throw new Exception("密码至少一个数字、一个大写字母、一个小写字母、一个特殊字符，长度8-32位");
                }
                if (password != this.textBoxRepeatPassword.Text)
                {
                    throw new Exception("两次输入的密码不一致");
                }

                // register URL
                var registerUrl = $"{Constant.ServerHost}{GlobalConstant.ApiNameRegister}";
                var result = await HttpClientUtil.PostAsync<RegisterModel, JsonResponse<Object>>(registerUrl, new RegisterModel() { Username = username, Password = password }, null);
                if (result?.Code != 0)
                {
                    throw new Exception(result?.Message);
                }

                // 注册成功
                MessageBox.Show(this, "注册成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"注册失败: {ex}");
                MessageBox.Show(this, $"注册失败: {ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
