using Baidu.Aip.Speech;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceApplication
{
    class BaiduAudioCore
    {
        private static readonly BaiduAudioCore INSTANCE_ = new();
        public static BaiduAudioCore INSTANCE
        {
            get { return INSTANCE_; }
        }

        private readonly Asr asrClient;
        private readonly Tts ttsClient;

        private BaiduAudioCore()
        {
            // 初始话client
            this.asrClient = new Baidu.Aip.Speech.Asr(Configuration.AppId, Configuration.ApiKey, Configuration.ApiSecret);
            this.ttsClient = new Baidu.Aip.Speech.Tts(Configuration.ApiKey, Configuration.ApiSecret);

            // 设置超时时间
            this.asrClient.Timeout = 60 * 1000;
            this.ttsClient.Timeout = 60 * 1000;
        }

        /// <summary>
        /// 识别语音
        /// </summary>
        /// <param name="inputFilePath">语音文件路径</param>
        /// <param name="audioType">语音类型：普通话，粤语等, 编码，参考：https://ai.baidu.com/ai-doc/SPEECH/ilbxfvpau</param>
        /// <returns>语音内容</returns>
        public string RecognizeAudio(string inputFilePath, int audioType)
        {
            try
            {
                var data = File.ReadAllBytes(inputFilePath);
                // 默认pcm格式
                var fileType = inputFilePath.Split(".").LastOrDefault("pcm").ToLower();
                
                // pcm格式原样上传；wav格式原样上传（拓展名为wav）；mp3转pcm格式上传（修改拓展名为pcm）
                byte[] uploadBytes = fileType switch
                {
                    "pcm" => data,
                    "wav" => data,
                    "mp3" => AudioConvertor.ConvertMp3ToPcm(data),
                    _ => throw new Exception("Unsupported format: " + fileType)
                };
                // mp3格式转pcm格式，同时修改扩展名为pcm
                fileType = "mp3".Equals(fileType) ? "pcm" : fileType;
                // 可选参数
                var options = new Dictionary<string, object>();
                options.Add("dev_pid", audioType);
                var result = this.asrClient.Recognize(uploadBytes, format: fileType, rate: 16000, options);
                if (result.Value<int>("err_no") != 0)
                {
                    throw new Exception("接口响应结果: " + result.ToString());
                }
                List<string> contentList = new();
                foreach(JToken item in (JArray)result["result"])
                {
                    contentList.Add(item.Value<string>());
                }
                return contentList.First();
            } catch (Exception e)
            {
                MessageBox.Show("语音识别失败！", "警告");
                System.Diagnostics.Debug.WriteLine(e);
            }
            return string.Empty;
        }

        /// <summary>
        /// 语音合成（合成mp3格式）
        /// 参考资料：https://ai.baidu.com/ai-doc/SPEECH/Zlbxhlc9x
        /// </summary>
        /// <param name="textContent">文本（需要转换成为语音的文字）</param>
        /// <param name="outputDir">输出文件目录</param>
        /// <param name="outputFileFormat">输出文件格式</param>
        /// <param name="voicePersion">发言人</param>
        /// <param name="audioSpeed">语速</param>
        /// <param name="audioVolume">音量</param>
        /// <returns>合成语音文件路径（mp3格式）</returns>
        public string TtsSynthesis(string textContent, string outputDir, string outputFileFormat, string voicePersion, string audioSpeed, string audioVolume)
        {
            try
            {
                // 可选参数
                var options = new Dictionary<string, object>();
                options.Add("spd", audioSpeed);// 语速
                options.Add("vol", audioVolume);// 音量
                options.Add("per", voicePersion);// 发音人
                var result = ttsClient.Synthesis(textContent, options);

                if (result.Success)
                {
                    var outputFilePath = Path.Combine(outputDir, Guid.NewGuid().ToString("N") + "." + outputFileFormat);
                    byte[] outputBytes = outputFileFormat switch
                    {
                        "mp3" => result.Data,
                        "wav" => AudioConvertor.ConvertMp3ToWav(result.Data),
                        "pcm" => AudioConvertor.ConvertMp3ToPcm(result.Data),
                        _ => throw new Exception("Unsupported file format:" + outputFileFormat),
                    };
                    File.WriteAllBytes(outputFilePath, outputBytes);
                    return outputFilePath;
                }
                throw new Exception("接口响应结果：" + result.ToString()); 
            }
            catch (Exception e)
            {
                MessageBox.Show("语音合成失败！", "警告");
                System.Diagnostics.Debug.WriteLine(e);
            }
            return string.Empty;
        }
    }
}
