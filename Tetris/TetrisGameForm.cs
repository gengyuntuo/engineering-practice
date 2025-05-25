using System.Text;

namespace Tetris
{
    public partial class TetrisGameForm : Form
    {
        private readonly TetrisStage tetrisStage;
        private readonly System.Windows.Forms.Timer timer = new();
        private readonly int[] gameLevel = [0, 500, 300, 100];
        public TetrisGameForm()
        {
            InitializeComponent();
            // ������Ϸ��̨
            tetrisStage = new TetrisStage(this.panelGameStage, this.panelNextBlock);
            tetrisStage.Init();

            this.KeyDown += KeyDownEventHandler;

            this.timer.Interval = gameLevel[Decimal.ToInt32(this.numericUpDownGameLevel.Value)];
            // ���ö�ʱ����������������쳣����ֹͣ��ʱ��
            this.timer.Tick += DownwaroEventHandler;

            // �޸���Ϸ�Ѷ�
            this.numericUpDownGameLevel.ValueChanged += (sender, e) =>
            {
                this.timer.Interval = gameLevel[Decimal.ToInt32(this.numericUpDownGameLevel.Value)];
                System.Diagnostics.Debug.WriteLine("Game Level switch to: " + this.timer.Interval);
            };

            StringBuilder sb = new();
            sb.AppendLine("����˵��:");
            sb.AppendLine("  �ո�|�س�: ��ʼ/��ͣ/���¿�ʼ��Ϸ");
            sb.AppendLine("  ��|��    : �����ƶ�����");
            sb.AppendLine("  ��       : �任������̬");
            sb.AppendLine("  ��       : ��������");
            sb.AppendLine("�÷�˵��:");
            sb.AppendLine("  Level 1: ����һ�е÷�+1");
            sb.AppendLine("  Level 2: ����һ�е÷�+2");
            sb.AppendLine("  Level 3: ����һ�е÷�+3");
            richTextBoxGameIntro.Text = sb.ToString();
        }

        /// <summary>
        /// ���̰����¼�
        /// </summary>
        /// <param name="sender">obj</param>
        /// <param name="e">�����¼�</param>
        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            // ���ư�������
            e.SuppressKeyPress = true;
            System.Diagnostics.Debug.WriteLine("Key event: " + e.KeyCode);

            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    if (this.timer.Enabled)
                    {
                        this.tetrisStage.RotateBlock();
                    }
                    break;
                case Keys.Down:
                case Keys.S:
                    if (this.timer.Enabled)
                    {
                        DownwaroEventHandler(sender, e);
                    }
                    break;
                case Keys.Left:
                case Keys.A:
                    if (this.timer.Enabled)
                    {
                        this.tetrisStage.LeftOrRightMove(-1);
                    }
                    break;
                case Keys.Right:
                case Keys.D:
                    if (this.timer.Enabled)
                    {
                        this.tetrisStage.LeftOrRightMove(1);
                    }
                    break;
                case Keys.Space:
                case Keys.Enter:
                    if (this.timer.Enabled)
                    {
                        System.Diagnostics.Debug.WriteLine("��ͣ��Ϸ");
                        this.timer.Enabled = false;
                    }
                    else
                    {
                        // ���¸���
                        if (!this.tetrisStage.IsAlive)
                        {
                            this.tetrisStage.Init();
                        }
                        System.Diagnostics.Debug.WriteLine("��Ϸ��ʼ");
                        this.timer.Enabled = true;
                    }
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Useless Key: " + e.KeyCode);
                    break;
            }
        }

        /// <summary>
        /// �����ƶ������¼�������
        /// 
        /// �����������߶�ʱ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownwaroEventHandler(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Tick");
                this.tetrisStage.DownwardBlock();
                // �÷�: ����һ�е�һ��
                this.labelScore.Text = $"�÷�: {this.tetrisStage.Score}";
            }
            catch (Exception ex)
            {
                this.timer.Enabled = false;
                System.Diagnostics.Debug.WriteLine("��Ϸ����: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("��Ϸ����: " + ex);
                MessageBox.Show(this, "��Ϸ����");
            }
        }
    }
}
