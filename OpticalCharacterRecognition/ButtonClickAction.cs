using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OpticalCharacterRecognition
{
    /// <summary>
    /// 处理页面上所有的按钮点击事件
    /// </summary>
    class ButtonClickAction
    {
        /// <summary>
        /// 卡证识别
        /// </summary>
        /// <param name="pictureLocation">图片位置</param>
        /// <param name="cardType">卡证类型</param>
        /// <returns>识别结果</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static async Task<string> DoCardPictureRecognize(string pictureLocation, string? cardType)
        {
            var result = cardType switch
            {
                "身份证正面" => FormatCardInfo(await Task.Run(() => BaiduAIApi.INSTANCE.IdCardPictureRecognize(pictureLocation, "front"))).ToString(),
                "身份证背面" => FormatCardInfo(await Task.Run(() => BaiduAIApi.INSTANCE.IdCardPictureRecognize(pictureLocation, "back"))).ToString(),
                "银行卡" => FormatBankCard(await Task.Run(() => BaiduAIApi.INSTANCE.BankCardRecognize(pictureLocation))).ToString(),
                "营业执照" => FormatCardInfo(await Task.Run(() => BaiduAIApi.INSTANCE.BusinessLicenseRecognize(pictureLocation))).ToString(),
                _ => throw new Exception("未知的卡证类型")
            };
            return result ?? "空";
        }

        private static string FormatBankCard(BankCardInfoResponse resposne)
        {
            return ("银行: " + resposne.Result?.BankName ?? "未知银行") + "\r\n"
                + ("卡号: " + resposne.Result?.BankCardNumber ?? "未知卡号");
        }
        private static string FormatCardInfo(CardInfoResponse resposne)
        {
            var lines = resposne.WordsResult?.Select(kv =>
            {
                return "* " + kv.Key + ": " + kv.Value.Words ?? "";
            });
            return string.Join("\r\n", lines ?? []);
        }

        /// <summary>
        /// 通用文字识别
        /// </summary>
        /// <param name="pictureLocation">图片路径</param>
        /// <param name="precision">精度,取值: 普通精度, 高精度</param>
        /// <param name="containLocation">包含位置信息</param>
        /// <param name="detectDirection">检测方向</param>
        /// <returns>文字</returns>
        internal static async Task<string> DoPlainPictureRecognize(string pictureLocation, string precision, bool containLocation, bool detectDirection)
        {
            var response = await Task.Run(() => BaiduAIApi.INSTANCE.PlainRecognize(pictureLocation, precision));
            var direction = detectDirection ? MapImageDirection(response.Direction) + "\r\n" : "";
            var lines = response.WordsResult?.Select(wordResult =>
            {
                string result;
                if (containLocation)
                {
                    result = JObject.FromObject(wordResult.Location ?? new()).ToString(Newtonsoft.Json.Formatting.None) + ": " + wordResult.Words;
                }
                else
                {
                    result = wordResult.Words ?? "";
                }
                return result;
            });
            return direction + string.Join("\r\n", lines ?? []);
        }

        /// <summary>
        /// 按钮响应: 选择图片位置
        /// </summary>
        /// <param name="textBoxPictureLocation">文本框，记录图片路径</param>
        /// <param name="openFileDialogPictureSelector">打开文件对话框</param>
        internal static void DoSelectPicture(TextBox textBoxPictureLocation, OpenFileDialog openFileDialogPictureSelector)
        {
            string initialDirectory;
            // 首先判断当前图片路径文本框中是否包含合法图片，如果存在，则作为打开文件窗口的路径
            var currentFilePath = textBoxPictureLocation.Text;
            if (Directory.Exists(currentFilePath))
            {
                initialDirectory = currentFilePath;
            }
            else if (File.Exists(currentFilePath))
            {
                initialDirectory = Path.GetDirectoryName(currentFilePath) ?? AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                initialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            // 未选择或者不含有效图片路径，选用程序所在目录作为打开文件窗口的路径
            openFileDialogPictureSelector.InitialDirectory = initialDirectory;
            var dialogResult = openFileDialogPictureSelector.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textBoxPictureLocation.Text = openFileDialogPictureSelector.FileName;
            }
        }

        /// <summary>
        /// 表格文字识别
        /// </summary>
        /// <param name="pictureLocation">图片位置</param>
        /// <returns>识别结果</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static async Task<string> DoTablePictureRecognize(string pictureLocation)
        {
            return await Task.Run(() => BaiduAIApi.INSTANCE.TableRecognize(pictureLocation));
        }

        /// <summary>
        /// 网络图片识别
        /// </summary>
        /// <param name="pictureLocation">图片路径</param>
        /// <returns>识别内容</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static async Task<string> DoWebPictureRecognize(string pictureLocation)
        {
            var response = await Task.Run(() => BaiduAIApi.INSTANCE.WebPictureRecognize(pictureLocation));
            var lines = response.WordsResult?.Select(wordResult =>
            {
                return wordResult.Words ?? "";
            });
            return string.Join("\r\n", lines ?? []);
        }

        /// <summary>
        /// 图像方向，当detect_direction=true时存在。
        /// - -1:未定义，
        /// - 0:正向，
        /// - 1:逆时针90度，
        /// - 2:逆时针180度，
        /// - 3:逆时针270度
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string MapImageDirection(int code)
        {
            return code switch
            {
                0 => "正向",
                1 => "逆时针90度",
                2 => "逆时针180度",
                3 => "逆时针270度",
                _ => "未定义"
            };
        }
    }
}
