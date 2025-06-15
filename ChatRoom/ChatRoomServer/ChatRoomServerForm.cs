using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatRoomServer
{
    public partial class ChatRoomServerForm : Form
    {
        /// <summary>
        /// 状态栏：文本
        /// </summary>
        private readonly ToolStripStatusLabel toolStripStatusLabel = new();
        /// <summary>
        /// 状态栏：进度条
        /// </summary>
        private readonly ToolStripProgressBar toolStripProgressBar = new();

        /// <summary>
        /// 服务器相关
        /// </summary>
        private TcpListener? tcpListener;
        private readonly List<TcpClient> tcpClientList = [];
        private bool serverStatus = false;

        public ChatRoomServerForm()
        {
            InitializeComponent();

            this.textBoxServerAddress.Text = "127.0.0.1";

            // 初始化状态栏
            toolStripStatusLabel.Text = "服务未启动";
            toolStripProgressBar.Visible = false;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Maximum = 100;
            this.toolStripProgressBar.Size = new Size(200, this.toolStripProgressBar.Size.Height);
            this.statusStrip.Items.Add(toolStripStatusLabel);
            this.statusStrip.Items.Add(toolStripProgressBar);

            // 初始化客户端列表: 清空
            this.listBoxOnlineClient.Items.Clear();

            // 绑定按钮事件
            this.buttonStartServer.Click += ClickStartServerButton;
        }

        /// <summary>
        /// 启动/关闭服务器按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickStartServerButton(object? sender, EventArgs e)
        {
            Button? button = sender as Button;
            switch (button?.Text)
            {
                case "启动服务":
                    this.buttonStartServer.Text = "关闭服务";
                    this.listBoxOnlineClient.Items.Clear();
                    this.StartChatServer();
                    this.toolStripStatusLabel.Text = "服务已就绪";
                    break;
                case "关闭服务":
                    this.StopChatServer();
                    this.buttonStartServer.Text = "启动服务";
                    this.toolStripStatusLabel.Text = "服务已关闭";
                    break;
                default:
                    throw new Exception("未知按钮");
            }
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        private void StartChatServer()
        {
            try
            {
                IPAddress localAddr = IPAddress.Parse(this.textBoxServerAddress.Text);
                this.tcpListener = new TcpListener(localAddr, 8088);
                this.tcpListener.Start();
                this.serverStatus = true;

                var processThread = new Thread(() =>
                {
                    HandleTcpClientAccept();
                });
                processThread.IsBackground = true;
                processThread.Start();
            }
            catch (Exception)
            {
                this.serverStatus = false;
                this.toolStripStatusLabel.Text = "服务启动失败";
            }
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        private void StopChatServer()
        {
            this.serverStatus = false;
            this.tcpListener?.Stop();
            this.tcpClientList.ForEach(x => x.Close());

            this.tcpListener = null;
            this.tcpClientList.Clear();
        }

        /// <summary>
        /// 处理连接通讯
        /// </summary>
        /// <param name="hostPort">远端地址</param>
        /// <param name="currentTcpClient">远端TCP客户端</param>
        private void HandleTcpClientMessage(String hostPort, TcpClient currentTcpClient)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            var networkStream = currentTcpClient.GetStream();

            // 循环读取客户端发送的数据
            try
            {
                while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string dataReceived = $"[{hostPort}]: " + Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    byte[] messageBytes = Encoding.UTF8.GetBytes(dataReceived);
                    System.Diagnostics.Debug.WriteLine($"Receive Message From {hostPort}: {dataReceived}");
                    System.Diagnostics.Debug.WriteLine($"Current Client number: " + this.tcpClientList.Count);

                    // 转发消息给其他客户端
                    foreach (var tcpClient in this.tcpClientList)
                    {
                        if (tcpClient == currentTcpClient)
                        {
                            continue;
                        }
                        try
                        {
                            var stream = tcpClient.GetStream();
                            stream.Write(messageBytes, 0, messageBytes.Length);
                            stream.Flush();
                            System.Diagnostics.Debug.WriteLine($"Send message to {tcpClient.Client.RemoteEndPoint?.ToString()}: {dataReceived}");
                        }
                        catch
                        {
                            System.Diagnostics.Debug.WriteLine("Message send failed.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Message read failed: " + e.Message);
            }
            // 客户端关闭，在UI线程更新结果
            this.Invoke((MethodInvoker)delegate
            {
                this.listBoxOnlineClient.Items.Remove(hostPort);
                this.tcpClientList.Remove(currentTcpClient);
            });
        }

        /// <summary>
        /// 初始服务器连接操作
        /// </summary>
        private void HandleTcpClientAccept()
        {
            try
            {
                while (serverStatus)
                {
                    if (this.tcpListener == null)
                    {
                        return;
                    }
                    var tcpClient = this.tcpListener.AcceptTcpClient();
                    var hostPort = tcpClient.Client.RemoteEndPoint?.ToString();
                    if (hostPort == null || hostPort.Length == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Cant't obtain remote server information.");
                        continue;
                    }
                    var networkStream = tcpClient.GetStream();
                    var processThread = new Thread(() =>
                    {
                        HandleTcpClientMessage(hostPort, tcpClient);
                    });
                    processThread.IsBackground = true;
                    processThread.Start();

                    // 保存显示客户端
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.listBoxOnlineClient.Items.Add(hostPort);
                        this.tcpClientList.Add(tcpClient);
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"等待客户端连接时发生异常: {ex.Message}");
                // 关闭当前Server
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("服务器异常关闭:" + ex.Message, "提示");
                    try
                    {
                        StopChatServer();
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
    }
}