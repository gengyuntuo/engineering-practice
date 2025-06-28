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
    public partial class FriendAddForm : Form
    {
        public FriendAddForm()
        {
            InitializeComponent();

            this.buttonAddFriend.Click += ButtonClickEventHandler;
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
                    case "添加好友": await DoAddFriend(); break;
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

        private async Task DoAddFriend()
        {
            var firendUsername = this.textBoxFriendUsername.Text;

            var result = await ServerApiInvoker.AddFriend(firendUsername);

            if (result)
            {
                // 添加成功
                MessageBox.Show(
                    this,
                    $"成功添加用户: {firendUsername}", "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
            }
            else
            {
                // 添加失败
                MessageBox.Show(
                    this,
                    $"添加用户{firendUsername}失败", "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );
            }
        }
    }
}