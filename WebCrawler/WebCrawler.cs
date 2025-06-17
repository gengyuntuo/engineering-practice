using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace WebCrawler
{
    internal class WebCrawler
    {
        private readonly HttpClient _httpClient;

        public WebCrawler()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        }

        public async Task<ProjectInfo> ScrapeProjectInfoAsync(string url)
        {
            try
            {
                // 发送HTTP请求获取网页内容
                string html = await _httpClient.GetStringAsync(url);

                // 解析HTML
                HtmlAgilityPack.HtmlDocument htmlDoc = new();
                htmlDoc.LoadHtml(html);
                // using (StreamWriter writer = new("page.txt", false, System.Text.Encoding.UTF8))
                // {
                //     writer.Write(html);
                // }

                // 提取信息
                var projectInfo = new ProjectInfo
                {
                    // 根据实际网页结构调整XPath选择器
                    ProjectName = ExtractSingleNodeText(htmlDoc, "//p[contains(@class, 'title-label')]"),
                    Industry = ExtractSingleNodeText(htmlDoc, "//div[@class='detail-con']/div[3]/p[2]"),
                    Background = ExtractMultiNodeText(htmlDoc, "//div[@class='detail-con']/div[4]/p|span"),
                    TechnicalProblems = ExtractMultiNodeText(htmlDoc, "//div[@class='detail-con']/div[5]/p|span"),
                    TechnicalGoals = ExtractMultiNodeText(htmlDoc, "//div[@class='detail-con']/div[6]/p|span")
                };

                return projectInfo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"爬取失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        private static string ExtractSingleNodeText(HtmlAgilityPack.HtmlDocument doc, string xpath)
        {
            var node = doc.DocumentNode.SelectSingleNode(xpath);
            return node?.InnerText.Trim() ?? "";
        }

        private static string ExtractMultiNodeText(HtmlAgilityPack.HtmlDocument doc, string xpath)
        {
            // System.Diagnostics.Debug.WriteLine($"XPath: {xpath}");
            var nodes = doc.DocumentNode.SelectNodes(xpath);
            var multiNodeText = nodes?.Select(node => node.InnerText.Trim());
            var result = string.Join(" ", multiNodeText ?? []);
            return result;
        }
    }

    public class ProjectInfo
    {
        public string ProjectName { get; set; }
        public string Industry { get; set; }
        public string Background { get; set; }
        public string TechnicalProblems { get; set; }
        public string TechnicalGoals { get; set; }

        public override string ToString()
        {
            return $"项目名称: {ProjectName}\n" +
                   $"行业领域: {Industry}\n" +
                   $"需求背景: {Background}\n" +
                   $"需解决的主要技术难题: {TechnicalProblems}\n" +
                   $"期望实现的主要技术目标: {TechnicalGoals}";
        }
        public string ToCsvString()
        {
            return $"{ProjectName},{Industry},{Background},{TechnicalProblems},{TechnicalGoals}";
        }
    }

}
