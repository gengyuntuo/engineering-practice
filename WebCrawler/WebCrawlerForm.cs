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

            // 控件内容初始化
            toolStripStatusLabel.Text = "已就绪";
            this.textBoxPageURL.Text = "https://www.kczg.org.cn/needs/detailThe?type=1&id=8412688";
            this.textBoxBeginIndex.Text = "0";
            this.textBoxEndIndex.Text = "0";
            this.labelProgressBar.Text = "0/0";

            if (webView == null)
            {
                throw new Exception("创建WebView2失败");
            }

            // 初始化页面按钮控件
            this.buttonPageBackward.Enabled = webView.CanGoBack;
            this.buttonPageForward.Enabled = webView.CanGoForward;
            // 订阅按钮点击事件
            this.buttonPageBackward.Click += ButtonClickHandler;
            this.buttonPageForward.Click += ButtonClickHandler;
            this.buttonAccessPage.Click += ButtonClickHandler;
            this.buttonStartCrawler.Click += ButtonClickHandler;

            // 初始化后台进程
            backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,       // 启用进度报告
                WorkerSupportsCancellation = true   // 启用取消功能
            };
            // 绑定事件处理程序
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }
        /// <summary>
        /// 后台任务: 爬虫任务运行完毕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    this.toolStripStatusLabel.Text = "爬虫任务已取消";
                }
                else if (e.Error != null)
                {
                    this.toolStripStatusLabel.Text = "爬虫任务失败";
                }
                else
                {
                    this.toolStripStatusLabel.Text = "爬虫任务已完成";
                }
            }
            finally
            {
                this.buttonStartCrawler.Enabled = true;
                this.textBoxBeginIndex.Enabled = true;
                this.textBoxEndIndex.Enabled = true;
                this.buttonStartCrawler.Text = "开始";
            }
        }

        /// <summary>
        /// 后台任务：更新UI进度条
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
        /// 后台任务：爬虫任务
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

            // 爬虫工具类
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
                // 写出文件头
                writer.WriteLine("项目名称,行业领域,需求背景,需解决的主要技术难题,期望实现的主要技术目标");

                // 运行任务
                for (int i = beginIndex; i <= endIndex; i++)
                {
                    // 检查取消请求（关键步骤）
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;  // 标记任务已取消
                        return;
                    }

                    // 本次爬取的路径
                    var tempURL = $"https://www.kczg.org.cn/needs/detailThe?type={type}&id={i}";
                    var result = webCrawler.ScrapeProjectInfoAsync(tempURL).GetAwaiter().GetResult();
                    System.Diagnostics.Debug.WriteLine(result);
                    // 写出到文件
                    writer.WriteLine(result?.ToCsvString());

                    // 报告进度
                    worker.ReportProgress(i - beginIndex + 1);
                }
            }
        }

        /// <summary>
        /// 初始化WebView2控件
        /// </summary>
        private async void InitializeWebView2()
        {
            // 创建WebView2实例并设置停靠方式
            this.webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.panel.Controls.Add(webView);

            // 异步初始化WebView2核心
            await webView.EnsureCoreWebView2Async(null);

            // 配置WebView2设置
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;  // 启用右键菜单
            webView.CoreWebView2.Settings.AreDevToolsEnabled = true;           // 启用开发者工具
            webView.CoreWebView2.Settings.IsScriptEnabled = true;          // 启用JavaScript

            // 绑定WebView业面加载完毕事件
            webView.NavigationCompleted += (sender, e) =>
            {
                // 如果是可以爬取的网页，则自动根据业面更新编号
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
                // 检查导航按钮使能状态
                CheckWebViewButton();
                // 使用WebView2当前业面链接更新地址栏内容
                this.textBoxPageURL.Text = this.webView.CoreWebView2.Source.ToString();
                this.toolStripStatusLabel.Text = "已就绪";
            };
        }

        /// <summary>
        /// 更新前进后退按钮的使能属性
        /// </summary>
        private void CheckWebViewButton()
        {
            this.buttonPageBackward.Enabled = this.webView.CanGoBack;
            this.buttonPageForward.Enabled = this.webView.CanGoForward;
        }

        /// <summary>
        /// 按钮处理事件
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
                // 禁用按钮，防止重复点击
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
                    case "转到":
                        this.webView.Source = new(this.textBoxPageURL.Text);
                        this.toolStripStatusLabel.Text = "加载中...";
                        break;
                    case "开始":
                        StartCrawler();
                        break;
                    case "停止":
                        StopCrawler();
                        break;
                    default:
                        MessageBox.Show("未知按钮！", "提示");
                        break;
                }
            }
            finally
            {
                if (button == this.buttonStartCrawler && button.Text == "停止" && this.backgroundWorker.CancellationPending)
                {
                    // 取消任务中, 仍然禁用按钮，其他情况下启用按钮
                }
                else
                {
                    button.Enabled = true;
                }
            }

            // 前进后退按钮使能设置
            CheckWebViewButton();
        }

        /// <summary>
        /// 停止爬虫程序
        /// </summary>
        private void StopCrawler()
        {
            if (backgroundWorker.WorkerSupportsCancellation)
            {
                backgroundWorker.CancelAsync(); // 发送取消请求
                toolStripStatusLabel.Text = "正在取消任务...";
            }
        }

        /// <summary>
        /// 启动爬虫程序
        /// </summary>
        private void StartCrawler()
        {
            try
            {
                var supportURLPrefix = "https://www.kczg.org.cn/needs";
                var url = this.webView.CoreWebView2.Source.ToString();
                if (!url.StartsWith(supportURLPrefix))
                {
                    MessageBox.Show($"不支持的URL，合法的URL前缀必须为：{supportURLPrefix}", "提示");
                    return;
                }
                var beginIndex = Int32.Parse(this.textBoxBeginIndex.Text);
                var endIndex = Int32.Parse(this.textBoxEndIndex.Text);

                if (beginIndex > endIndex)
                {
                    MessageBox.Show("起始编号不能大于结束编号", "提示");
                    return;
                }

                // 启动后台任务
                var parameters = new Dictionary<string, object>
                {
                    { "url", url },
                    { "beginIndex", beginIndex },
                    { "endIndex", endIndex },
                };
                var totalCount = endIndex - beginIndex + 1;
                backgroundWorker.RunWorkerAsync(parameters);

                // 成功UI更新
                this.textBoxBeginIndex.Enabled = false;
                this.textBoxEndIndex.Enabled = false;
                this.buttonStartCrawler.Text = "停止";
                this.toolStripStatusLabel.Text = "正在爬取数据";
                this.labelProgressBar.Text = $"0/{totalCount}";
                this.progressBar.Maximum = totalCount;
            }
            catch (Exception e)
            {
                MessageBox.Show("异常:" + e.Message, "提示");
            }
        }
    }
}
