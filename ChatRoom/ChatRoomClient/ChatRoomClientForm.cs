using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ChatRoomClient
{
    public partial class ChatRoomClientForm : Form
    {
        /// <summary>
        /// ״̬�����ı�
        /// </summary>
        private readonly ToolStripStatusLabel toolStripStatusLabel = new();
        /// <summary>
        /// ״̬����������
        /// </summary>
        private readonly ToolStripProgressBar toolStripProgressBar = new();

        private TcpClient? tcpClient;
        private bool connectStatus = false;

        public ChatRoomClientForm()
        {
            InitializeComponent();

            this.textBoxServerAddress.Text = "127.0.0.1";

            // ��ʼ��״̬��
            toolStripStatusLabel.Text = "δ���ӷ�����";
            toolStripProgressBar.Visible = false;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Maximum = 100;
            this.toolStripProgressBar.Size = new Size(200, this.toolStripProgressBar.Size.Height);
            this.statusStrip.Items.Add(toolStripStatusLabel);
            this.statusStrip.Items.Add(toolStripProgressBar);

            // �󶨰�ť�¼�
            this.buttonClearContent.Click += ClickButtonHandler;
            this.buttonConnectServer.Click += ClickButtonHandler;
            this.buttonSendMessage.Click += ClickButtonHandler;
        }

        /// <summary>
        /// ��ť����¼�������
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
                    case "���ӷ�����":
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
                            MessageBox.Show("����������ʧ�ܣ�", "��ʾ");
                        }
                        finally
                        {
                            this.toolStripProgressBar.Visible = false;
                        }
                        break;
                    case "�Ͽ�����":
                        try
                        {
                            this.tcpClient?.Close();
                            this.tcpClient = null;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("�رշ�����ʱ�����쳣: " + ex.Message);
                        }
                        finally
                        {
                            UpdateUIByConnectionStatus(false);
                        }
                        this.connectStatus = false;
                        button.Text = "���ӷ�����";
                        break;
                    case "���":
                        this.richTextBoxChatContent.Clear();
                        break;
                    case "����":
                        SendMessage();
                        break;
                    default:
                        throw new Exception("δ֪��ť");
                }
            }
            finally
            {
                button.Enabled = true;
            }
        }

        /// <summary>
        /// ��������״̬����UI
        /// </summary>
        /// <param name="connectionSatus">����״̬��true: ������</param>
        private void UpdateUIByConnectionStatus(bool connectionSatus)
        {
            if (connectionSatus)
            {
                this.connectStatus = true;
                this.buttonConnectServer.Text = "�Ͽ�����";
                this.toolStripStatusLabel.Text = "������";
            }
            else
            {
                this.connectStatus = false;
                this.buttonConnectServer.Text = "���ӷ�����";
                this.toolStripStatusLabel.Text = "�ѶϿ�����";
            }
            this.toolStripProgressBar.Visible = false;
        }

        /// <summary>
        /// ���ӷ�����
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
                System.Diagnostics.Debug.WriteLine("���ӷ�����ʧ��:" + e.Message);
                throw;
            }
        }

        /// <summary>
        /// �����������Ϣ
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
                        // չʾ���յ�����Ϣ
                        this.Invoke((MethodInvoker)delegate
                        {
                            DisplayMessage(receivedMessage, false);
                        });
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"������Ϣ�Ĺ����з����쳣: {e.Message}");
            }

            // �Ͽ�����
            // չʾ���յ�����Ϣ
            this.Invoke((MethodInvoker)delegate
            {
                // MessageBox.Show("�����쳣�ж�", "��ʾ");
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
        /// չʾ��Ϣ
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <param name="selfMessage">true: չʾ���͵���Ϣ��false: չʾ���յ���Ϣ</param>
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
                // ����궨λ���ı�ĩβ
                this.richTextBoxChatContent.SelectionStart = this.richTextBoxChatContent.Text.Length;
                // ���������λ��
                this.richTextBoxChatContent.ScrollToCaret();
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private void SendMessage()
        {
            try
            {
                string message = richTextBoxMessageEditer.Text;

                if (message.Trim().Length == 0)
                {
                    MessageBox.Show("��Ϣ���ݲ�����Ϊ��!", "��ʾ");
                    return;
                }

                // �Ѿ����ӣ����ܷ�����Ϣ
                if (connectStatus)
                {
                    var stream = this.tcpClient?.GetStream();
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    stream?.Write(messageBytes, 0, messageBytes.Length);
                }
                else
                {
                    MessageBox.Show("�����ӷ�������");
                    return;
                }
                // չʾ��Ϣ
                DisplayMessage(message, true);
                // ���ͳɹ��������
                richTextBoxMessageEditer.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show("����ʧ��: " + e.Message, "��ʾ");
            }
        }
    }
}
