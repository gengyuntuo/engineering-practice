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
        /// ״̬�����ı�
        /// </summary>
        private readonly ToolStripStatusLabel toolStripStatusLabel = new();
        /// <summary>
        /// ״̬����������
        /// </summary>
        private readonly ToolStripProgressBar toolStripProgressBar = new();

        /// <summary>
        /// ���������
        /// </summary>
        private TcpListener? tcpListener;
        private readonly List<TcpClient> tcpClientList = [];
        private bool serverStatus = false;

        public ChatRoomServerForm()
        {
            InitializeComponent();

            this.textBoxServerAddress.Text = "127.0.0.1";

            // ��ʼ��״̬��
            toolStripStatusLabel.Text = "����δ����";
            toolStripProgressBar.Visible = false;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Maximum = 100;
            this.toolStripProgressBar.Size = new Size(200, this.toolStripProgressBar.Size.Height);
            this.statusStrip.Items.Add(toolStripStatusLabel);
            this.statusStrip.Items.Add(toolStripProgressBar);

            // ��ʼ���ͻ����б�: ���
            this.listBoxOnlineClient.Items.Clear();

            // �󶨰�ť�¼�
            this.buttonStartServer.Click += ClickStartServerButton;
        }

        /// <summary>
        /// ����/�رշ�������ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickStartServerButton(object? sender, EventArgs e)
        {
            Button? button = sender as Button;
            switch (button?.Text)
            {
                case "��������":
                    this.buttonStartServer.Text = "�رշ���";
                    this.listBoxOnlineClient.Items.Clear();
                    this.StartChatServer();
                    this.toolStripStatusLabel.Text = "�����Ѿ���";
                    break;
                case "�رշ���":
                    this.StopChatServer();
                    this.buttonStartServer.Text = "��������";
                    this.toolStripStatusLabel.Text = "�����ѹر�";
                    break;
                default:
                    throw new Exception("δ֪��ť");
            }
        }

        /// <summary>
        /// ����������
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
                this.toolStripStatusLabel.Text = "��������ʧ��";
            }
        }

        /// <summary>
        /// ֹͣ������
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
        /// ��������ͨѶ
        /// </summary>
        /// <param name="hostPort">Զ�˵�ַ</param>
        /// <param name="currentTcpClient">Զ��TCP�ͻ���</param>
        private void HandleTcpClientMessage(String hostPort, TcpClient currentTcpClient)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            var networkStream = currentTcpClient.GetStream();

            // ѭ����ȡ�ͻ��˷��͵�����
            try
            {
                while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string dataReceived = $"[{hostPort}]: " + Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    byte[] messageBytes = Encoding.UTF8.GetBytes(dataReceived);
                    System.Diagnostics.Debug.WriteLine($"Receive Message From {hostPort}: {dataReceived}");
                    System.Diagnostics.Debug.WriteLine($"Current Client number: " + this.tcpClientList.Count);

                    // ת����Ϣ�������ͻ���
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
            // �ͻ��˹رգ���UI�̸߳��½��
            this.Invoke((MethodInvoker)delegate
            {
                this.listBoxOnlineClient.Items.Remove(hostPort);
                this.tcpClientList.Remove(currentTcpClient);
            });
        }

        /// <summary>
        /// ��ʼ���������Ӳ���
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

                    // ������ʾ�ͻ���
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.listBoxOnlineClient.Items.Add(hostPort);
                        this.tcpClientList.Add(tcpClient);
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"�ȴ��ͻ�������ʱ�����쳣: {ex.Message}");
                // �رյ�ǰServer
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("�������쳣�ر�:" + ex.Message, "��ʾ");
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