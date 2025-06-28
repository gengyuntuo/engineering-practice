using ChatRoomLibrary.Service;
using System.Data.Common;
using System.Data;
using System.Windows.Forms;

namespace ChatRoomServer
{
    public partial class ChatRoomServerForm : Form
    {
        private readonly UserService userService;
        public ChatRoomServerForm()
        {
            userService = new UserService();
            InitializeComponent();

            this.buttonUserSearch.Click += SearchButtonClickHandler;

            // 自定义列标题
            this.dataGridView.Columns.Add("Id", "用户ID");
            this.dataGridView.Columns.Add("Username", "用户名称");
            this.dataGridView.Columns.Add("Online", "是否在线");
            // 禁用显示空白列、行
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.AllowUserToAddRows = false;
            // 设置填充模式
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // 设置各列填充比例（可选）
            this.dataGridView.Columns["Id"].FillWeight = 20;        // 20% 宽度
            this.dataGridView.Columns["Username"].FillWeight = 40;  // 40% 宽度
            this.dataGridView.Columns["Online"].FillWeight = 40;    // 40% 宽度
        }

        private void ChatRoomServerForm_Load(object sender, EventArgs e)
        {
            // 加载控件值
            if (Program.ApiServerStatus)
            {
                this.toolStripStatusLabel.Text = "服务器已就绪";
            }
            else
            {
                this.toolStripStatusLabel.Text = "服务器启动失败";
            }
        }

        private void SearchButtonClickHandler(object? sender, EventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }
            try
            {
                button.Enabled = false;
                RenderUserList();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Dialog");
            }
            finally
            {
                button.Enabled = true;
            }
        }

        /// <summary>
        /// 渲染用户清单
        /// </summary>
        private async void RenderUserList()
        {
            string queryUsername = this.textBoxUserSearch.Text.Trim();
            bool onlyOnlineUser = this.checkBoxOnlyOnlinUser.Checked;
            // 加载用户数据
            var users = await userService.GetUserListByUsernameAsync(queryUsername);

            // 过滤数据
            var onlineUsers = Program.OnlineUsers;
            if (onlyOnlineUser)
            {
                users = [.. users.Where(user => onlineUsers.Contains(user.Id))];
            }

            // 渲染数据
            dataGridView.Rows.Clear();
            foreach (var user in users)
            {
                dataGridView.Rows.Add(user.Id, user.Username, onlineUsers.Contains(user.Id) ? "在线" : "");
            }
        }
    }
}
