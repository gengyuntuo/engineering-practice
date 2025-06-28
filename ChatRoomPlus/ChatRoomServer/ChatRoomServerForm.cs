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

            // �Զ����б���
            this.dataGridView.Columns.Add("Id", "�û�ID");
            this.dataGridView.Columns.Add("Username", "�û�����");
            this.dataGridView.Columns.Add("Online", "�Ƿ�����");
            // ������ʾ�հ��С���
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.AllowUserToAddRows = false;
            // �������ģʽ
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // ���ø�������������ѡ��
            this.dataGridView.Columns["Id"].FillWeight = 20;        // 20% ���
            this.dataGridView.Columns["Username"].FillWeight = 40;  // 40% ���
            this.dataGridView.Columns["Online"].FillWeight = 40;    // 40% ���
        }

        private void ChatRoomServerForm_Load(object sender, EventArgs e)
        {
            // ���ؿؼ�ֵ
            if (Program.ApiServerStatus)
            {
                this.toolStripStatusLabel.Text = "�������Ѿ���";
            }
            else
            {
                this.toolStripStatusLabel.Text = "����������ʧ��";
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
        /// ��Ⱦ�û��嵥
        /// </summary>
        private async void RenderUserList()
        {
            string queryUsername = this.textBoxUserSearch.Text.Trim();
            bool onlyOnlineUser = this.checkBoxOnlyOnlinUser.Checked;
            // �����û�����
            var users = await userService.GetUserListByUsernameAsync(queryUsername);

            // ��������
            var onlineUsers = Program.OnlineUsers;
            if (onlyOnlineUser)
            {
                users = [.. users.Where(user => onlineUsers.Contains(user.Id))];
            }

            // ��Ⱦ����
            dataGridView.Rows.Clear();
            foreach (var user in users)
            {
                dataGridView.Rows.Add(user.Id, user.Username, onlineUsers.Contains(user.Id) ? "����" : "");
            }
        }
    }
}
