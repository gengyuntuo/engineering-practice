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

            // ����ʶ������
            var audioTypeItems = new Dictionary<string, string>
            {
                { "1537", "��ͨ��" },
                { "1737", "Ӣ��" },
                { "1637", "����" },
                { "1837", "�Ĵ���" }
            };
            var comboBoxAsrAudioTypeItems = new List<KeyValuePair<string, string>>(audioTypeItems);
            comboBoxAsrAudioType.DataSource = comboBoxAsrAudioTypeItems;
            comboBoxAsrAudioType.DisplayMember = "Value";
            comboBoxAsrAudioType.ValueMember = "Key";

            // �����ϳɣ�������
            var ttsPersonItems = new Dictionary<string, string>();
            // ��С��=0(Ĭ��)����С��=1��������ң��������=3����ѾѾ=4
            ttsPersonItems.Add("0", "��С��");
            ttsPersonItems.Add("1", "��С��");
            ttsPersonItems.Add("3", "����ң��������");
            ttsPersonItems.Add("4", "��ѾѾ");
            // ����ң����Ʒ��= 5003����С¹ = 5118���Ȳ��� = 106����Сͯ = 110����С�� = 111�����׶� = 103����С�� = 5
            ttsPersonItems.Add("5003", "����ң����Ʒ��");
            ttsPersonItems.Add("5118", "��С¹");
            ttsPersonItems.Add("106", "�Ȳ���");
            ttsPersonItems.Add("110", "��Сͯ");
            ttsPersonItems.Add("111", "��С��");
            ttsPersonItems.Add("103", "���׶�");
            ttsPersonItems.Add("5", "��С��");
            var comboBoxTtsPersonItems = new List<KeyValuePair<string, string>>(ttsPersonItems);
            comboBoxTtsPerson.DataSource = comboBoxTtsPersonItems;
            comboBoxTtsPerson.DisplayMember = "Value";
            comboBoxTtsPerson.ValueMember = "Key";

            // �����ϳɣ�������
            var ttsFileFormatItems = new Dictionary<string, string>();
            ttsFileFormatItems.Add("mp3", "mp3");
            ttsFileFormatItems.Add("pcm", "pcm");
            ttsFileFormatItems.Add("wav", "wav");
            var comboBoxTtsFileFormatItems = new List<KeyValuePair<string, string>>(ttsFileFormatItems);
            comboBoxTtsFileFormat.DataSource = comboBoxTtsFileFormatItems;
            comboBoxTtsPerson.DisplayMember = "Value";
            comboBoxTtsFileFormat.ValueMember = "Key";

            // չʾ�����ٶȡ���������
            labelTtsSpeed.Text = trackBarTtsAudioSpeed.Value.ToString();
            labelTtsVolume.Text = trackBarTtsAudioVolume.Value.ToString();

            // ��ȡ��ǰ����·��
            textBoxTtsOutputDir.Text = System.AppDomain.CurrentDomain.BaseDirectory;

            // ���ò�����
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
                    MessageBox.Show("δ�����Ӧ�¼��Ŀؼ�", "����");
                    break;
            }
        }

        private async void ButtonClick(object sender, EventArgs e)
        {
            Button clickedBtn = (Button)sender;
            switch(clickedBtn.Name)
            {
                // ����ʶ��ѡ�������ļ�
                case "buttonAsrSelectFile":
                    var asrDialogResult = openFileDialog.ShowDialog();
                    if(asrDialogResult == DialogResult.OK)
                    {
                        textBoxAsrFile.Text = openFileDialog.FileName;
                    }
                    break;
                // ����ʶ�𣺿�ʼ����ʶ��
                case "buttonAsrAudioRecognize":
                    await DoAsrAudioRecognize();
                    break;
                // �����ϳ�: ѡ�������ļ�����λ��
                case "buttonTtsSelectOutputDir":
                    var ttsDialogResult = folderBrowserDialog.ShowDialog();
                    if (ttsDialogResult == DialogResult.OK)
                    {
                        textBoxTtsOutputDir.Text = folderBrowserDialog.SelectedPath;
                    }
                    break;
                // �����ϳɣ��ϳ�����
                case "buttonTtsSynthesis":
                    await DoTtsSynthesis();
                    break;
                default:
                    MessageBox.Show("������İ���δ�����Ӧ����", "����");
                    break;

            }
        }

        private async Task DoTtsSynthesis()
        {
            // �����ϳ��ı�
            var textContent = richTextBoxTtsContent.Text.Trim();
            // ���Ŀ¼
            var outputDir = textBoxTtsOutputDir.Text;
            // ������
            var voicePersion = comboBoxTtsPerson.SelectedValue?.ToString()??"0";
            // ��������
            var audioSpeed = trackBarTtsAudioSpeed.Value.ToString();
            // ��������
            var audioVolume = trackBarTtsAudioVolume.Value.ToString();
            // ��鵱ǰ����״̬
            if (player.PlaybackState == PlaybackState.Playing)
            {
                // ���ڲ��ţ�������ϳ�����
                DialogResult result = MessageBox.Show("�����������У��������ִ�У��ϳɵ��������ܲ��ᱻ���ţ�", "��ܰ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // �����û���ѡ�������Ӧ����
                // if (result == DialogResult.Yes)
                if (result == DialogResult.No)
                {
                    // �û�ѡ����˳�ִ��
                    return;
                }
            }
            if (textContent.Length == 0)
            {
                MessageBox.Show("���ݲ���Ϊ�գ�", "����");
                return;
            }
            if (Encoding.UTF8.GetBytes(textContent).Length > 1024)
            {
                MessageBox.Show("���ݳ��Ȳ��ɳ����ӿ�������󳤶�(1024GBK)��", "����");
                return;
            }
            var outputFileFormat = comboBoxTtsFileFormat.SelectedValue?.ToString() ?? "mp3";
            try
            {
                // ���ð�ť
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
                    MessageBox.Show("�����ϳ�ʧ�ܣ�", "����");
                }
            }
            finally
            {
                // ���ð�ť
                buttonTtsSynthesis.Enabled = true;
            }

        }

        /// <summary>
        /// ʶ������
        /// </summary>
        private async Task DoAsrAudioRecognize()
        {
            // �����ļ�·��
            var asrIpnutFile = textBoxAsrFile.Text;
            // ��������
            var audioType = comboBoxAsrAudioType;

            if (!File.Exists(asrIpnutFile))
            {
                MessageBox.Show("��ѡ�������ļ���", "����");
                return;
            }
            try
            {
                // ���ð�ť
                buttonAsrAudioRecognize.Enabled = false;
                var result = await Task.Run(() =>
                {
                    return BaiduAudioCore.INSTANCE.RecognizeAudio(asrIpnutFile, Convert.ToInt32(comboBoxAsrAudioType.SelectedValue));
                });
                richTextBoxAsrResult.Text = result;
            }
            finally
            {
                // ���ð�ť
                buttonAsrAudioRecognize.Enabled = true;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="audioFilePath"></param>
        private void PlayMusic(string audioFilePath)
        {
            if (player.PlaybackState == PlaybackState.Playing)
            {
                // ���ڲ��ţ����ٲ���
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
