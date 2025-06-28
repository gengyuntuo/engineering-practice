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
        /// 后台定时拉取message, 500毫秒拉取一次
        /// </summary>
        private readonly System.Timers.Timer timer = new()
        {
            AutoReset = false,
            Interval = 500,
        };
        /// <summary>
        /// 未读消息
        /// </summary>
        private readonly Dictionary<string, int> unreadMessageDict = [];
        /// <summary>
        /// 好友列表 key; 好友名称  value 好友ID
        /// </summary>
        private readonly Dictionary<string, int> friendsDict = [];
        public ChatRoomClientForm()
        {
            InitializeComponent();

            this.buttonAddFriend.Click += ButtonClickEventHandler;
            this.buttonSendMessage.Click += ButtonClickEventHandler;

            // 初始化ListView
            this.listViewMyFriends.View = View.Details;  // 设置为详细信息视图
            this.listViewMyFriends.GridLines = true;     // 显示网格线
            this.listViewMyFriends.FullRowSelect = true; // 允许整行选择
            // 添加列
            this.listViewMyFriends.Columns.Add("姓名", 200);
            this.listViewMyFriends.Columns.Add("消息", 60);
            // 绑定点击事件
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
                    // 更新UI（必须在UI线程中执行）
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.toolStripStatusLabel.Text = "就绪";
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Message Query Thread Exception: {ex}");
                    // 更新UI（必须在UI线程中执行）
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.toolStripStatusLabel.Text = "与服务器连接断开";
                    });
                }
                finally
                {
                    this.timer.Start();
                }
            };
            this.timer.Start();

            // 窗口标题
            this.Text += " [" + (ClientCore.User?.Username ?? "") + "]";
        }

        /// <summary>
        /// 处理用户消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task ProcessUserMessage(ChatRoomLibrary.Model.Message message)
        {
            // 存储用户消息到本地
            var messageId = await ClientCore.SaveMessageToLocal(message);
            // 更新UI（必须在UI线程中执行）
            this.Invoke((MethodInvoker)async delegate
            {
                // 当前选择的用户
                var currentWindowUsername = this.labelFriendInfo.Text;
                // 如果新消息是当前页面选择的用户，则直接展示，并标记为已读
                if (this.friendsDict.TryGetValue(currentWindowUsername, out int sendId) && sendId == message.SenderId)
                {
                    await ClientCore.MarkLocalMessageAsRead([messageId]);
                    DisplayMessage(message.Content, message.SentAt, false);
                }
                else
                {
                    // 更新未读消息计数 +1
                    foreach (var entry in this.friendsDict)
                    {
                        if (entry.Value == message.SenderId)
                        {
                            this.unreadMessageDict[entry.Key] = this.unreadMessageDict.TryGetValue(entry.Key, out int unreadCount) ? unreadCount + 1 : 1;
                            break;
                        }
                    }
                    // 显示好友计数
                    DisplayFriendUnReadMessageCount();
                }
            });
        }

        /// <summary>
        /// 处理系统消息
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
                    throw new Exception("系统消息内容为空");
                }
                var systemMessage = JsonConvert.DeserializeObject<SystemMessage>(messageContent) ?? throw new Exception("系统消息解析失败");
                switch (systemMessage.Command)
                {
                    case "server_start":
                        System.Diagnostics.Debug.WriteLine("Server Start");
                        break;
                    case "add_friend":
                        var friends = await ServerApiInvoker.QueryFriends() ?? [];
                        await ClientCore.SaveConfig(Constant.FriendListConfig, JsonConvert.SerializeObject(friends));
                        // 更新UI（必须在UI线程中执行）
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
                System.Diagnostics.Debug.WriteLine("未知事件源");
                return;
            }
            Button button = (Button)sender;

            try
            {
                button.Enabled = false;
                switch (button.Text)
                {
                    case "添加好友": DoAddFriend(); break;
                    case "发送": await DoSendMessage(); break;
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
                System.Diagnostics.Debug.WriteLine($"发送消息[A -> B]: {message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"消息发送失败: {ex.Message}");
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
        /// 窗口加载完成事件 (接收到系统消息中的添加好友也会执行该事件)
        /// 
        /// 显示好友列表
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
                // 不显示系统用户
                if (user.Id == 0)
                {
                    continue;
                }
                // 添加行数据，将好友显示到好友列表
                ListViewItem item = new(user.Username);
                item.SubItems.Add("");
                this.listViewMyFriends.Items.Add(item);

                // 维护好友到缓存中
                friendsDict.Add(user.Username, user.Id);
            }

            // 更新好友计数
            await UpdateMetadataOfFriendUnReadMessageCount();
            // 显示好友计数
            DisplayFriendUnReadMessageCount();
        }

        /// <summary>
        /// 切换好友对话框
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
            // MessageBox.Show($"激活项: {changeEventArgs.Item?.Text}");
            var friendUsername = changeEventArgs.Item?.Text;
            this.labelFriendInfo.Text = friendUsername;
            var messages = await ClientCore.LoadMessageFromLocal(ClientCore.User?.Id ?? -1, (friendsDict[friendUsername ?? ""])) ?? [];
            this.richTextBoxMessageMonitor.Text = "";
            foreach (var message in messages)
            {
                // 将未读消息标记为已读
                if (message.ReadAt == null)
                {
                    await ClientCore.MarkLocalMessageAsRead([message.Id]);
                }
                var showText = message.Content;
                DisplayMessage(showText, message.SentAt, message.SenderId == (ClientCore.User?.Id ?? -1));
            }
            // 更新未读消息
            this.unreadMessageDict[friendUsername ?? ""] = 0;
            // 显示好友计数
            DisplayFriendUnReadMessageCount();
        }

        /// <summary>
        /// 展示消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="selfMessage">true: 自己的消息 false: 好友的消息</param>
        private void DisplayMessage(string message, DateTime msgSendTime, bool selfMessage)
        {
            var username = selfMessage ? ClientCore.User?.Username ?? "我" : this.labelFriendInfo.Text;
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
        /// 更新好友列表中未读消息的元数据: 将未读好友未读消息数维护到unreadMessageDict成员中
        /// </summary>
        /// <returns></returns>
        private async Task UpdateMetadataOfFriendUnReadMessageCount()
        {
            var statisticResult = await ClientCore.GetCurrentUserMessageStatistic();
            foreach (var statItem in statisticResult)
            {
                string? friendUsername = null;
                var friendsList = this.friendsDict.ToList();
                // 获取好友用户名
                for (int i = 0; i < friendsList.Count; i++)
                {
                    var entry = friendsList[i];
                    if (entry.Value == statItem.SenderId)
                    {
                        friendUsername = entry.Key;
                        break;
                    }
                }
                // 好友存在，则维护好友未读消息
                if (friendUsername != null)
                {
                    this.unreadMessageDict[friendUsername] = statItem.UnReadMessage;
                }
            }
        }

        /// <summary>
        /// 在窗口中展示未读消息计数
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
