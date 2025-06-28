using ChatRoomLibrary.Core;
using ChatRoomLibrary.Model;
using Newtonsoft.Json;
using System.Media;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ChatRoomClient
{
    public partial class ChatRoomClientForm : Form
    {
        /// <summary>
        /// ��̨��ʱ��ȡmessage, 500������ȡһ��
        /// </summary>
        private readonly System.Timers.Timer timer = new()
        {
            AutoReset = false,
            Interval = 500,
        };
        /// <summary>
        /// δ����Ϣ
        /// </summary>
        private readonly Dictionary<string, int> unreadMessageDict = [];
        /// <summary>
        /// �����б� key; ��������  value ����ID
        /// </summary>
        private readonly Dictionary<string, int> friendsDict = [];
        public ChatRoomClientForm()
        {
            InitializeComponent();

            this.buttonAddFriend.Click += ButtonClickEventHandler;
            this.buttonSendMessage.Click += ButtonClickEventHandler;

            // ��ʼ��ListView
            this.listViewMyFriends.View = View.Details;  // ����Ϊ��ϸ��Ϣ��ͼ
            this.listViewMyFriends.GridLines = true;     // ��ʾ������
            this.listViewMyFriends.FullRowSelect = true; // ��������ѡ��
            // �����
            this.listViewMyFriends.Columns.Add("����", 200);
            this.listViewMyFriends.Columns.Add("��Ϣ", 60);
            // �󶨵���¼�
            this.listViewMyFriends.ItemSelectionChanged += ListView_ItemActivate;

            this.timer.Elapsed += async (sender, e) =>
            {
                try
                {
                    bool displayMusic = false;
                    var messages = await ServerApiInvoker.QueryUnreadMessages();
                    foreach (var message in messages)
                    {
                        switch (message.IsSystem)
                        {
                            case true:
                                await ProcessSystemMessage(message);
                                break;
                            case false:
                                await ProcessUserMessage(message);
                                displayMusic = true;
                                break;
                            default:
                        }
                        await ServerApiInvoker.MarkMessageAsRead(message.Id);
                    }
                    if (displayMusic)
                    {
                        ClientCore.PlayMessageMusic();
                    }
                    // ����UI��������UI�߳���ִ�У�
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.toolStripStatusLabel.Text = "����";
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Message Query Thread Exception: {ex}");
                    // ����UI��������UI�߳���ִ�У�
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.toolStripStatusLabel.Text = "����������ӶϿ�";
                    });
                }
                finally
                {
                    this.timer.Start();
                }
            };
            this.timer.Start();

            // ���ڱ���
            this.Text += " [" + (ClientCore.User?.Username ?? "") + "]";
        }

        /// <summary>
        /// �����û���Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task ProcessUserMessage(ChatRoomLibrary.Model.Message message)
        {
            // �洢�û���Ϣ������
            var messageId = await ClientCore.SaveMessageToLocal(message);
            // ����UI��������UI�߳���ִ�У�
            this.Invoke((MethodInvoker)async delegate
            {
                // ��ǰѡ����û�
                var currentWindowUsername = this.labelFriendInfo.Text;
                // �������Ϣ�ǵ�ǰҳ��ѡ����û�����ֱ��չʾ�������Ϊ�Ѷ�
                if (this.friendsDict.TryGetValue(currentWindowUsername, out int sendId) && sendId == message.SenderId)
                {
                    await ClientCore.MarkLocalMessageAsRead([messageId]);
                    DisplayMessage(message.Content, message.SentAt, false);
                }
                else
                {
                    // ����δ����Ϣ���� +1
                    foreach (var entry in this.friendsDict)
                    {
                        if (entry.Value == message.SenderId)
                        {
                            this.unreadMessageDict[entry.Key] = this.unreadMessageDict.TryGetValue(entry.Key, out int unreadCount) ? unreadCount + 1 : 1;
                            break;
                        }
                    }
                    // ��ʾ���Ѽ���
                    DisplayFriendUnReadMessageCount();
                }
            });
        }

        /// <summary>
        /// ����ϵͳ��Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessSystemMessage(ChatRoomLibrary.Model.Message message)
        {
            var messageContent = message?.Content;
            try
            {
                if (messageContent == null)
                {
                    throw new Exception("ϵͳ��Ϣ����Ϊ��");
                }
                var systemMessage = JsonConvert.DeserializeObject<SystemMessage>(messageContent) ?? throw new Exception("ϵͳ��Ϣ����ʧ��");
                switch (systemMessage.Command)
                {
                    case "server_start":
                        System.Diagnostics.Debug.WriteLine("Server Start");
                        break;
                    case "add_friend":
                        var friends = await ServerApiInvoker.QueryFriends() ?? [];
                        await ClientCore.SaveConfig(Constant.FriendListConfig, JsonConvert.SerializeObject(friends));
                        // ����UI��������UI�߳���ִ�У�
                        this.Invoke((MethodInvoker)delegate
                        {
                            ChatRoomClientForm_Load(null, new());
                        });
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Process System Message({messageContent}) Failed: {ex}");
            }
        }

        private async void ButtonClickEventHandler(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                System.Diagnostics.Debug.WriteLine("δ֪�¼�Դ");
                return;
            }
            Button button = (Button)sender;

            try
            {
                button.Enabled = false;
                switch (button.Text)
                {
                    case "��Ӻ���": DoAddFriend(); break;
                    case "����": await DoSendMessage(); break;
                    default:
                        MessageBox.Show(this, "δ֪�İ�ť: " + e, "����");
                        break;
                }
            }
            finally
            {
                button.Enabled = true;
            }

        }

        private async Task DoSendMessage()
        {
            try
            {
                var messageContent = this.richTextBoxMessageEditor.Text.Trim();
                if (messageContent.Length == 0)
                {
                    return;
                }
                var friendUsername = this.labelFriendInfo.Text;
                if (messageContent.Length == 0)
                {
                    return;
                }
                var message = new ChatRoomLibrary.Model.Message()
                {
                    SenderId = ClientCore.User?.Id ?? -1,
                    ReceiverId = this.friendsDict[friendUsername],
                    Content = messageContent,
                    SentAt = DateTime.Now,
                    IsSystem = false,
                };
                var result = await ServerApiInvoker.SendMessage(message);
                if (!result)
                {
                    return;
                }
                // SystemSounds.Asterisk.Play();
                this.DisplayMessage(messageContent, DateTime.Now, true);
                this.richTextBoxMessageEditor.Text = "";
                await ClientCore.SaveMessageToLocal(message);
                System.Diagnostics.Debug.WriteLine($"������Ϣ[A -> B]: {message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��Ϣ����ʧ��: {ex.Message}");
            }
        }

        private void DoAddFriend()
        {
            using FriendAddForm friendAddForm = new();
            friendAddForm.StartPosition = FormStartPosition.CenterParent;
            friendAddForm.ShowDialog(this);
        }

        private void ToolStripMenuItemSignOut_Click(object sender, EventArgs e)
        {
            ClientCore.SignOut();
            this.Close();
        }

        /// <summary>
        /// ���ڼ�������¼� (���յ�ϵͳ��Ϣ�е���Ӻ���Ҳ��ִ�и��¼�)
        /// 
        /// ��ʾ�����б�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChatRoomClientForm_Load(object? sender, EventArgs e)
        {
            var rawUserlist = await ClientCore.LoadConfig(Constant.FriendListConfig);
            var userList = JsonConvert.DeserializeObject<List<User>>(rawUserlist ?? "[]");
            listViewMyFriends.Items.Clear();
            friendsDict.Clear();
            foreach (var user in userList ?? [])
            {
                // ����ʾϵͳ�û�
                if (user.Id == 0)
                {
                    continue;
                }
                // ��������ݣ���������ʾ�������б�
                ListViewItem item = new(user.Username);
                item.SubItems.Add("");
                this.listViewMyFriends.Items.Add(item);

                // ά�����ѵ�������
                friendsDict.Add(user.Username, user.Id);
            }

            // ���º��Ѽ���
            await UpdateMetadataOfFriendUnReadMessageCount();
            // ��ʾ���Ѽ���
            DisplayFriendUnReadMessageCount();
        }

        /// <summary>
        /// �л����ѶԻ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListView_ItemActivate(object? sender, EventArgs e)
        {
            ListViewItemSelectionChangedEventArgs changeEventArgs = (ListViewItemSelectionChangedEventArgs)e;
            if (changeEventArgs == null || changeEventArgs.IsSelected == false)
            {
                return;
            }
            // MessageBox.Show($"������: {changeEventArgs.Item?.Text}");
            var friendUsername = changeEventArgs.Item?.Text;
            this.labelFriendInfo.Text = friendUsername;
            var messages = await ClientCore.LoadMessageFromLocal(ClientCore.User?.Id ?? -1, (friendsDict[friendUsername ?? ""])) ?? [];
            this.richTextBoxMessageMonitor.Text = "";
            foreach (var message in messages)
            {
                // ��δ����Ϣ���Ϊ�Ѷ�
                if (message.ReadAt == null)
                {
                    await ClientCore.MarkLocalMessageAsRead([message.Id]);
                }
                var showText = message.Content;
                DisplayMessage(showText, message.SentAt, message.SenderId == (ClientCore.User?.Id ?? -1));
            }
            // ����δ����Ϣ
            this.unreadMessageDict[friendUsername ?? ""] = 0;
            // ��ʾ���Ѽ���
            DisplayFriendUnReadMessageCount();
        }

        /// <summary>
        /// չʾ��Ϣ
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <param name="selfMessage">true: �Լ�����Ϣ false: ���ѵ���Ϣ</param>
        private void DisplayMessage(string message, DateTime msgSendTime, bool selfMessage)
        {
            var username = selfMessage ? ClientCore.User?.Username ?? "��" : this.labelFriendInfo.Text;
            var displayMessage = $"[{username} At:{msgSendTime}]\n{message}";
            var originText = this.richTextBoxMessageMonitor.Text;
            this.richTextBoxMessageMonitor.AppendText(displayMessage + "\n");
            this.richTextBoxMessageMonitor.Select(originText.Length, displayMessage.Length);
            if (selfMessage)
            {
                this.richTextBoxMessageMonitor.SelectionColor = Color.Green;
                this.richTextBoxMessageMonitor.SelectionAlignment = HorizontalAlignment.Right;
            }
            else
            {
                this.richTextBoxMessageMonitor.SelectionColor = Color.Blue;
                this.richTextBoxMessageMonitor.SelectionAlignment = HorizontalAlignment.Left;
            }
            this.richTextBoxMessageMonitor.SelectionLength = 0;
            if (this.richTextBoxMessageMonitor.Text.Length > 0)
            {
                this.richTextBoxMessageMonitor.SelectionStart = this.richTextBoxMessageMonitor.Text.Length;
                this.richTextBoxMessageMonitor.ScrollToCaret();
            }
        }

        /// <summary>
        /// ���º����б���δ����Ϣ��Ԫ����: ��δ������δ����Ϣ��ά����unreadMessageDict��Ա��
        /// </summary>
        /// <returns></returns>
        private async Task UpdateMetadataOfFriendUnReadMessageCount()
        {
            var statisticResult = await ClientCore.GetCurrentUserMessageStatistic();
            foreach (var statItem in statisticResult)
            {
                string? friendUsername = null;
                var friendsList = this.friendsDict.ToList();
                // ��ȡ�����û���
                for (int i = 0; i < friendsList.Count; i++)
                {
                    var entry = friendsList[i];
                    if (entry.Value == statItem.SenderId)
                    {
                        friendUsername = entry.Key;
                        break;
                    }
                }
                // ���Ѵ��ڣ���ά������δ����Ϣ
                if (friendUsername != null)
                {
                    this.unreadMessageDict[friendUsername] = statItem.UnReadMessage;
                }
            }
        }

        /// <summary>
        /// �ڴ�����չʾδ����Ϣ����
        /// </summary>
        /// <returns></returns>
        private void DisplayFriendUnReadMessageCount()
        {
            foreach (ListViewItem item in listViewMyFriends.Items)
            {
                var friendUsername = item.SubItems[0].Text;
                var showText = unreadMessageDict.TryGetValue(friendUsername, out int value) && value > 0 ? value.ToString() : "";
                item.SubItems[1].Text = showText;
            }
        }
    }
}
