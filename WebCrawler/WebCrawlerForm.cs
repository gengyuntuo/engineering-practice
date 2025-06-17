using Microsoft.Web.WebView2.WinForms;
using System.ComponentModel;
using System.Resources;
using System.Security.Policy;
using System.Windows.Forms;

namespace WebCrawler
{
    public partial class WebCrawlerForm : Form
    {
        private WebView2 webView;
        private BackgroundWorker backgroundWorker;
        public WebCrawlerForm()
        {
            InitializeComponent();
            InitializeWebView2();

            // �ؼ����ݳ�ʼ��
            toolStripStatusLabel.Text = "�Ѿ���";
            this.textBoxPageURL.Text = "https://www.kczg.org.cn/needs/detailThe?type=1&id=8412688";
            this.textBoxBeginIndex.Text = "0";
            this.textBoxEndIndex.Text = "0";
            this.labelProgressBar.Text = "0/0";

            if (webView == null)
            {
                throw new Exception("����WebView2ʧ��");
            }

            // ��ʼ��ҳ�水ť�ؼ�
            this.buttonPageBackward.Enabled = webView.CanGoBack;
            this.buttonPageForward.Enabled = webView.CanGoForward;
            // ���İ�ť����¼�
            this.buttonPageBackward.Click += ButtonClickHandler;
            this.buttonPageForward.Click += ButtonClickHandler;
            this.buttonAccessPage.Click += ButtonClickHandler;
            this.buttonStartCrawler.Click += ButtonClickHandler;

            // ��ʼ����̨����
            backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,       // ���ý��ȱ���
                WorkerSupportsCancellation = true   // ����ȡ������
            };
            // ���¼��������
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }
        /// <summary>
        /// ��̨����: ����������������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    this.toolStripStatusLabel.Text = "����������ȡ��";
                }
                else if (e.Error != null)
                {
                    this.toolStripStatusLabel.Text = "��������ʧ��";
                }
                else
                {
                    this.toolStripStatusLabel.Text = "�������������";
                }
            }
            finally
            {
                this.buttonStartCrawler.Enabled = true;
                this.textBoxBeginIndex.Enabled = true;
                this.textBoxEndIndex.Enabled = true;
                this.buttonStartCrawler.Text = "��ʼ";
            }
        }

        /// <summary>
        /// ��̨���񣺸���UI������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            var beginIndex = Int32.Parse(this.textBoxBeginIndex.Text);
            var endIndex = Int32.Parse(this.textBoxEndIndex.Text);
            var count = endIndex - beginIndex + 1;
            this.progressBar.Value = e.ProgressPercentage;
            this.labelProgressBar.Text = $"{e.ProgressPercentage}/{count}";
        }

        /// <summary>
        /// ��̨������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender is not BackgroundWorker worker)
            {
                return;
            }

            if (e.Argument is not Dictionary<string, object> parameters)
            {
                return;
            }

            // ���湤����
            WebCrawler webCrawler = new();

            string url = (string)parameters["url"];
            int beginIndex = (int)parameters["beginIndex"];
            int endIndex = (int)parameters["endIndex"];

            var parameterPart = url.Substring(url.IndexOf("?"));
            var parameterList = parameterPart.Split("&");
            string type = "";
            for (int i = 0; i < parameterList.Length; i++)
            {
                var kvPair = parameterList[i].Split('=');
                if (kvPair[0] == "type")
                {
                    type = kvPair[1];
                }
            }

            var fileName = $"crawler-result-type-{type}-range-{beginIndex}-{endIndex}.csv";
            using (StreamWriter writer = new(fileName, false, System.Text.Encoding.UTF8))
            {
                // д���ļ�ͷ
                writer.WriteLine("��Ŀ����,��ҵ����,���󱳾�,��������Ҫ��������,����ʵ�ֵ���Ҫ����Ŀ��");

                // ��������
                for (int i = beginIndex; i <= endIndex; i++)
                {
                    // ���ȡ�����󣨹ؼ����裩
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;  // ���������ȡ��
                        return;
                    }

                    // ������ȡ��·��
                    var tempURL = $"https://www.kczg.org.cn/needs/detailThe?type={type}&id={i}";
                    var result = webCrawler.ScrapeProjectInfoAsync(tempURL).GetAwaiter().GetResult();
                    System.Diagnostics.Debug.WriteLine(result);
                    // д�����ļ�
                    writer.WriteLine(result?.ToCsvString());

                    // �������
                    worker.ReportProgress(i - beginIndex + 1);
                }
            }
        }

        /// <summary>
        /// ��ʼ��WebView2�ؼ�
        /// </summary>
        private async void InitializeWebView2()
        {
            // ����WebView2ʵ��������ͣ����ʽ
            this.webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.panel.Controls.Add(webView);

            // �첽��ʼ��WebView2����
            await webView.EnsureCoreWebView2Async(null);

            // ����WebView2����
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;  // �����Ҽ��˵�
            webView.CoreWebView2.Settings.AreDevToolsEnabled = true;           // ���ÿ����߹���
            webView.CoreWebView2.Settings.IsScriptEnabled = true;          // ����JavaScript

            // ��WebViewҵ���������¼�
            webView.NavigationCompleted += (sender, e) =>
            {
                // ����ǿ�����ȡ����ҳ�����Զ�����ҵ����±��
                try
                {
                    var supportURLPrefix = "https://www.kczg.org.cn/needs";
                    var url = this.webView.CoreWebView2.Source.ToString();
                    if (url.StartsWith(supportURLPrefix))
                    {
                        var parameterPart = url.Substring(url.IndexOf("?"));
                        var parameterList = parameterPart.Split("&");
                        string id = "";
                        for (int i = 0; i < parameterList.Length; i++)
                        {
                            var kvPair = parameterList[i].Split('=');
                            if (kvPair[0] == "id")
                            {
                                id = kvPair[1];
                            }
                        }
                        this.textBoxBeginIndex.Text = id;
                        this.textBoxEndIndex.Text = id;
                    }
                }
                catch (Exception)
                {
                    // 
                }
                // ��鵼����ťʹ��״̬
                CheckWebViewButton();
                // ʹ��WebView2��ǰҵ�����Ӹ��µ�ַ������
                this.textBoxPageURL.Text = this.webView.CoreWebView2.Source.ToString();
                this.toolStripStatusLabel.Text = "�Ѿ���";
            };
        }

        /// <summary>
        /// ����ǰ�����˰�ť��ʹ������
        /// </summary>
        private void CheckWebViewButton()
        {
            this.buttonPageBackward.Enabled = this.webView.CanGoBack;
            this.buttonPageForward.Enabled = this.webView.CanGoForward;
        }

        /// <summary>
        /// ��ť�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickHandler(object? sender, EventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }
            try
            {
                // ���ð�ť����ֹ�ظ����
                button.Enabled = false;
                switch (button?.Text)
                {
                    case "<-":
                        if (this.webView.CanGoBack)
                        {
                            this.webView.GoBack();
                        }
                        break;
                    case "->":
                        if (this.webView.CanGoForward)
                        {
                            this.webView.GoForward();
                        }
                        break;
                    case "ת��":
                        this.webView.Source = new(this.textBoxPageURL.Text);
                        this.toolStripStatusLabel.Text = "������...";
                        break;
                    case "��ʼ":
                        StartCrawler();
                        break;
                    case "ֹͣ":
                        StopCrawler();
                        break;
                    default:
                        MessageBox.Show("δ֪��ť��", "��ʾ");
                        break;
                }
            }
            finally
            {
                if (button == this.buttonStartCrawler && button.Text == "ֹͣ" && this.backgroundWorker.CancellationPending)
                {
                    // ȡ��������, ��Ȼ���ð�ť��������������ð�ť
                }
                else
                {
                    button.Enabled = true;
                }
            }

            // ǰ�����˰�ťʹ������
            CheckWebViewButton();
        }

        /// <summary>
        /// ֹͣ�������
        /// </summary>
        private void StopCrawler()
        {
            if (backgroundWorker.WorkerSupportsCancellation)
            {
                backgroundWorker.CancelAsync(); // ����ȡ������
                toolStripStatusLabel.Text = "����ȡ������...";
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        private void StartCrawler()
        {
            try
            {
                var supportURLPrefix = "https://www.kczg.org.cn/needs";
                var url = this.webView.CoreWebView2.Source.ToString();
                if (!url.StartsWith(supportURLPrefix))
                {
                    MessageBox.Show($"��֧�ֵ�URL���Ϸ���URLǰ׺����Ϊ��{supportURLPrefix}", "��ʾ");
                    return;
                }
                var beginIndex = Int32.Parse(this.textBoxBeginIndex.Text);
                var endIndex = Int32.Parse(this.textBoxEndIndex.Text);

                if (beginIndex > endIndex)
                {
                    MessageBox.Show("��ʼ��Ų��ܴ��ڽ������", "��ʾ");
                    return;
                }

                // ������̨����
                var parameters = new Dictionary<string, object>
                {
                    { "url", url },
                    { "beginIndex", beginIndex },
                    { "endIndex", endIndex },
                };
                var totalCount = endIndex - beginIndex + 1;
                backgroundWorker.RunWorkerAsync(parameters);

                // �ɹ�UI����
                this.textBoxBeginIndex.Enabled = false;
                this.textBoxEndIndex.Enabled = false;
                this.buttonStartCrawler.Text = "ֹͣ";
                this.toolStripStatusLabel.Text = "������ȡ����";
                this.labelProgressBar.Text = $"0/{totalCount}";
                this.progressBar.Maximum = totalCount;
            }
            catch (Exception e)
            {
                MessageBox.Show("�쳣:" + e.Message, "��ʾ");
            }
        }
    }
}
