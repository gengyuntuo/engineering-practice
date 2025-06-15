using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ChatRoomClient
{
    public partial class ChatRoomClientForm : Form
    {
        /// <summary>
        /// 状态栏：文本
        /// </summary>
        private readonly ToolStripStatusLabel toolStripStatusLabel = new();
        /// <summary>
        /// 状态栏：进度条
        /// </summary>
        private readonly ToolStripProgressBar toolStripProgressBar = new();

        private TcpClient? tcpClient;
        private bool connectStatus = false;

        public ChatRoomClientForm()
        {
            InitializeComponent();

            this.textBoxServerAddress.Text = "127.0.0.1";

            // 初始化状态栏
            toolStripStatusLabel.Text = "未连接服务器";
            toolStripProgressBar.Visible = false;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Maximum = 100;
            this.toolStripProgressBar.Size = new Size(200, this.toolStripProgressBar.Size.Height);
            this.statusStrip.Items.Add(toolStripStatusLabel);
            this.statusStrip.Items.Add(toolStripProgressBar);

            // 绑定按钮事件
            this.buttonClearContent.Click += ClickButtonHandler;
            this.buttonConnectServer.Click += ClickButtonHandler;
            this.buttonSendMessage.Click += ClickButtonHandler;
        }

        /// <summary>
        /// 按钮点击事件处理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void ClickButtonHandler(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            Button button = (Button)sender;
            try
            {
                button.Enabled = false;
                switch (button.Text)
                {
                    case "连接服务器":
                        try
                        {
                            this.toolStripProgressBar.Visible = true;
                            this.toolStripProgressBar.Value = 50;
                            ConnectServer();
                            this.toolStripProgressBar.Value = 100;
                            UpdateUIByConnectionStatus(true);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("服务器连接失败！", "提示");
                        }
                        finally
                        {
                            this.toolStripProgressBar.Visible = false;
                        }
                        break;
                    case "断开连接":
                        try
                        {
                            this.tcpClient?.Close();
                            this.tcpClient = null;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("关闭服务器时发生异常: " + ex.Message);
                        }
                        finally
                        {
                            UpdateUIByConnectionStatus(false);
                        }
                        this.connectStatus = false;
                        button.Text = "连接服务器";
                        break;
                    case "清空":
                        this.richTextBoxChatContent.Clear();
                        break;
                    case "发送":
                        SendMessage();
                        break;
                    default:
                        throw new Exception("未知按钮");
                }
            }
            finally
            {
                button.Enabled = true;
            }
        }

        /// <summary>
        /// 根据连接状态更新UI
        /// </summary>
        /// <param name="connectionSatus">连接状态，true: 已连接</param>
        private void UpdateUIByConnectionStatus(bool connectionSatus)
        {
            if (connectionSatus)
            {
                this.connectStatus = true;
                this.buttonConnectServer.Text = "断开连接";
                this.toolStripStatusLabel.Text = "已连接";
            }
            else
            {
                this.connectStatus = false;
                this.buttonConnectServer.Text = "连接服务器";
                this.toolStripStatusLabel.Text = "已断开连接";
            }
            this.toolStripProgressBar.Visible = false;
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        private void ConnectServer()
        {
            try
            {
                this.tcpClient = new TcpClient();
                this.tcpClient.Connect(IPAddress.Parse(this.textBoxServerAddress.Text), 8088);

                if (!this.tcpClient.Connected)
                {
                    throw new Exception();
                }
                var thread = new Thread(() =>
                {
                    TcpClientStreamHandler(tcpClient);
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("连接服务器失败:" + e.Message);
                throw;
            }
        }

        /// <summary>
        /// 处理服务器消息
        /// </summary>
        /// <param name="tcpClient"></param>
        private void TcpClientStreamHandler(TcpClient tcpClient)
        {
            var stream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (connectStatus)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (receivedMessage.Length != 0)
                    {
                        // 展示接收到的消息
                        this.Invoke((MethodInvoker)delegate
                        {
                            DisplayMessage(receivedMessage, false);
                        });
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"接收消息的过程中发生异常: {e.Message}");
            }

            // 断开连接
            // 展示接收到的消息
            this.Invoke((MethodInvoker)delegate
            {
                // MessageBox.Show("连接异常中断", "提示");
                try
                {
                    this.tcpClient?.Close();
                }
                catch (Exception)
                {
                }
                UpdateUIByConnectionStatus(false);
            });
        }

        /// <summary>
        /// 展示消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="selfMessage">true: 展示发送的消息，false: 展示接收的消息</param>
        private void DisplayMessage(string message, bool selfMessage)
        {
            var originText = this.richTextBoxChatContent.Text;
            this.richTextBoxChatContent.AppendText(message + "\n");
            this.richTextBoxChatContent.Select(originText.Length, message.Length);
            if (selfMessage)
            {
                this.richTextBoxChatContent.SelectionColor = Color.Green;
                this.richTextBoxChatContent.SelectionAlignment = HorizontalAlignment.Right;
            }
            else
            {
                this.richTextBoxChatContent.SelectionColor = Color.Blue;
                this.richTextBoxChatContent.SelectionAlignment = HorizontalAlignment.Left;
            }
            this.richTextBoxChatContent.SelectionLength = 0;
            if (this.richTextBoxChatContent.Text.Length > 0)
            {
                // 将光标定位到文本末尾
                this.richTextBoxChatContent.SelectionStart = this.richTextBoxChatContent.Text.Length;
                // 滚动到光标位置
                this.richTextBoxChatContent.ScrollToCaret();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        private void SendMessage()
        {
            try
            {
                string message = richTextBoxMessageEditer.Text;

                if (message.Trim().Length == 0)
                {
                    MessageBox.Show("消息内容不可以为空!", "提示");
                    return;
                }

                // 已经连接，才能发送消息
                if (connectStatus)
                {
                    var stream = this.tcpClient?.GetStream();
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    stream?.Write(messageBytes, 0, messageBytes.Length);
                }
                else
                {
                    MessageBox.Show("请连接服务器！");
                    return;
                }
                // 展示消息
                DisplayMessage(message, true);
                // 发送成功清空内容
                richTextBoxMessageEditer.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show("发送失败: " + e.Message, "提示");
            }
        }
    }
}
