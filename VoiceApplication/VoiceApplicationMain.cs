using NAudio.Wave;
using System.ComponentModel;
using System.Text;
using System.Threading.Channels;

namespace VoiceApplication
{
    public partial class VoiceApplicationMain : Form
    {
        private readonly WaveOutEvent player;
        public VoiceApplicationMain()
        {
            InitializeComponent();

            // 语音识别类型
            var audioTypeItems = new Dictionary<string, string>
            {
                { "1537", "普通话" },
                { "1737", "英语" },
                { "1637", "粤语" },
                { "1837", "四川话" }
            };
            var comboBoxAsrAudioTypeItems = new List<KeyValuePair<string, string>>(audioTypeItems);
            comboBoxAsrAudioType.DataSource = comboBoxAsrAudioTypeItems;
            comboBoxAsrAudioType.DisplayMember = "Value";
            comboBoxAsrAudioType.ValueMember = "Key";

            // 语音合成：发言人
            var ttsPersonItems = new Dictionary<string, string>();
            // 度小美=0(默认)，度小宇=1，，度逍遥（基础）=3，度丫丫=4
            ttsPersonItems.Add("0", "度小美");
            ttsPersonItems.Add("1", "度小宇");
            ttsPersonItems.Add("3", "度逍遥（基础）");
            ttsPersonItems.Add("4", "度丫丫");
            // 度逍遥（精品）= 5003，度小鹿 = 5118，度博文 = 106，度小童 = 110，度小萌 = 111，度米朵 = 103，度小娇 = 5
            ttsPersonItems.Add("5003", "度逍遥（精品）");
            ttsPersonItems.Add("5118", "度小鹿");
            ttsPersonItems.Add("106", "度博文");
            ttsPersonItems.Add("110", "度小童");
            ttsPersonItems.Add("111", "度小萌");
            ttsPersonItems.Add("103", "度米朵");
            ttsPersonItems.Add("5", "度小娇");
            var comboBoxTtsPersonItems = new List<KeyValuePair<string, string>>(ttsPersonItems);
            comboBoxTtsPerson.DataSource = comboBoxTtsPersonItems;
            comboBoxTtsPerson.DisplayMember = "Value";
            comboBoxTtsPerson.ValueMember = "Key";

            // 语音合成：发言人
            var ttsFileFormatItems = new Dictionary<string, string>();
            ttsFileFormatItems.Add("mp3", "mp3");
            ttsFileFormatItems.Add("pcm", "pcm");
            ttsFileFormatItems.Add("wav", "wav");
            var comboBoxTtsFileFormatItems = new List<KeyValuePair<string, string>>(ttsFileFormatItems);
            comboBoxTtsFileFormat.DataSource = comboBoxTtsFileFormatItems;
            comboBoxTtsPerson.DisplayMember = "Value";
            comboBoxTtsFileFormat.ValueMember = "Key";

            // 展示语音速度、语音音量
            labelTtsSpeed.Text = trackBarTtsAudioSpeed.Value.ToString();
            labelTtsVolume.Text = trackBarTtsAudioVolume.Value.ToString();

            // 获取当前程序路径
            textBoxTtsOutputDir.Text = System.AppDomain.CurrentDomain.BaseDirectory;

            // 设置播放器
            player = new WaveOutEvent();
        }

        private void TrackBarChanged(object sender, EventArgs e)
        {
            var trackBar = (TrackBar)sender;
            switch(trackBar.Name)
            {
                case "trackBarTtsAudioSpeed":
                    labelTtsSpeed.Text = trackBarTtsAudioSpeed.Value.ToString();
                    break;
                case "trackBarTtsAudioVolume":
                    labelTtsVolume.Text = trackBarTtsAudioVolume.Value.ToString();
                    break;
                default:
                    MessageBox.Show("未添加响应事件的控件", "警告");
                    break;
            }
        }

        private async void ButtonClick(object sender, EventArgs e)
        {
            Button clickedBtn = (Button)sender;
            switch(clickedBtn.Name)
            {
                // 语音识别：选择语音文件
                case "buttonAsrSelectFile":
                    var asrDialogResult = openFileDialog.ShowDialog();
                    if(asrDialogResult == DialogResult.OK)
                    {
                        textBoxAsrFile.Text = openFileDialog.FileName;
                    }
                    break;
                // 语音识别：开始语音识别
                case "buttonAsrAudioRecognize":
                    await DoAsrAudioRecognize();
                    break;
                // 语音合成: 选择语音文件生成位置
                case "buttonTtsSelectOutputDir":
                    var ttsDialogResult = folderBrowserDialog.ShowDialog();
                    if (ttsDialogResult == DialogResult.OK)
                    {
                        textBoxTtsOutputDir.Text = folderBrowserDialog.SelectedPath;
                    }
                    break;
                // 语音合成：合成语音
                case "buttonTtsSynthesis":
                    await DoTtsSynthesis();
                    break;
                default:
                    MessageBox.Show("您点击的按键未添加响应程序！", "警告");
                    break;

            }
        }

        private async Task DoTtsSynthesis()
        {
            // 语音合成文本
            var textContent = richTextBoxTtsContent.Text.Trim();
            // 输出目录
            var outputDir = textBoxTtsOutputDir.Text;
            // 发言人
            var voicePersion = comboBoxTtsPerson.SelectedValue?.ToString()??"0";
            // 语音速率
            var audioSpeed = trackBarTtsAudioSpeed.Value.ToString();
            // 语音音量
            var audioVolume = trackBarTtsAudioVolume.Value.ToString();
            // 检查当前播放状态
            if (player.PlaybackState == PlaybackState.Playing)
            {
                // 正在播放，不允许合成语音
                DialogResult result = MessageBox.Show("播放中声音中，如果继续执行，合成的声音可能不会被播放！", "温馨提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // 根据用户的选择进行相应处理
                // if (result == DialogResult.Yes)
                if (result == DialogResult.No)
                {
                    // 用户选择否，退出执行
                    return;
                }
            }
            if (textContent.Length == 0)
            {
                MessageBox.Show("内容不可为空！", "警告");
                return;
            }
            if (Encoding.UTF8.GetBytes(textContent).Length > 1024)
            {
                MessageBox.Show("内容长度不可超过接口允许最大长度(1024GBK)！", "警告");
                return;
            }
            var outputFileFormat = comboBoxTtsFileFormat.SelectedValue?.ToString() ?? "mp3";
            try
            {
                // 禁用按钮
                buttonTtsSynthesis.Enabled = false;
                var result = await Task.Run(() =>
                {
                    var tempResult = BaiduAudioCore.INSTANCE.TtsSynthesis(textContent, outputDir, outputFileFormat, voicePersion, audioSpeed, audioVolume);
                    if (tempResult.Length != 0)
                    {
                        PlayMusic(tempResult);
                    }
                    return tempResult;
                });
                if (result.Length == 0)
                {
                    MessageBox.Show("语音合成失败！", "警告");
                }
            }
            finally
            {
                // 启用按钮
                buttonTtsSynthesis.Enabled = true;
            }

        }

        /// <summary>
        /// 识别语音
        /// </summary>
        private async Task DoAsrAudioRecognize()
        {
            // 语音文件路径
            var asrIpnutFile = textBoxAsrFile.Text;
            // 语音种类
            var audioType = comboBoxAsrAudioType;

            if (!File.Exists(asrIpnutFile))
            {
                MessageBox.Show("请选择语音文件！", "警告");
                return;
            }
            try
            {
                // 禁用按钮
                buttonAsrAudioRecognize.Enabled = false;
                var result = await Task.Run(() =>
                {
                    return BaiduAudioCore.INSTANCE.RecognizeAudio(asrIpnutFile, Convert.ToInt32(comboBoxAsrAudioType.SelectedValue));
                });
                richTextBoxAsrResult.Text = result;
            }
            finally
            {
                // 启用按钮
                buttonAsrAudioRecognize.Enabled = true;
            }
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="audioFilePath"></param>
        private void PlayMusic(string audioFilePath)
        {
            if (player.PlaybackState == PlaybackState.Playing)
            {
                // 正在播放，则不再播放
                return;
            }
            var fileFormat = audioFilePath.Split(".").LastOrDefault("mp3");
            if ("mp3" == fileFormat)
            {
                using var audioFileReader = new AudioFileReader(audioFilePath);
                player.Init(audioFileReader);
            }
            else if ("wav" == fileFormat)
            {
                var waveFileReader = new WaveFileReader(audioFilePath);
                player.Init(waveFileReader);
            }
            else if ("pcm" == fileFormat)
            {
                var waveFormat = new WaveFormat(16000, 16, 1);
                var fileStream = new System.IO.FileStream(audioFilePath, System.IO.FileMode.Open);
                var rawSourceStream = new RawSourceWaveStream(fileStream, waveFormat);
                player.Init(rawSourceStream);
            }
            else
            {
                throw new Exception("Unsupported format: " + fileFormat);
            }
            player.Play();
        }
    }
}
