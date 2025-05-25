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
            // 创建游戏舞台
            tetrisStage = new TetrisStage(this.panelGameStage, this.panelNextBlock);
            tetrisStage.Init();

            this.KeyDown += KeyDownEventHandler;

            this.timer.Interval = gameLevel[Decimal.ToInt32(this.numericUpDownGameLevel.Value)];
            // 设置定时器操作，如果发生异常，则停止定时器
            this.timer.Tick += DownwaroEventHandler;

            // 修改游戏难度
            this.numericUpDownGameLevel.ValueChanged += (sender, e) =>
            {
                this.timer.Interval = gameLevel[Decimal.ToInt32(this.numericUpDownGameLevel.Value)];
                System.Diagnostics.Debug.WriteLine("Game Level switch to: " + this.timer.Interval);
            };

            StringBuilder sb = new();
            sb.AppendLine("操作说明:");
            sb.AppendLine("  空格|回车: 开始/暂停/重新开始游戏");
            sb.AppendLine("  ←|→    : 左右移动方块");
            sb.AppendLine("  ↑       : 变换方块形态");
            sb.AppendLine("  ↓       : 加速下落");
            sb.AppendLine("得分说明:");
            sb.AppendLine("  Level 1: 消除一行得分+1");
            sb.AppendLine("  Level 2: 消除一行得分+2");
            sb.AppendLine("  Level 3: 消除一行得分+3");
            richTextBoxGameIntro.Text = sb.ToString();
        }

        /// <summary>
        /// 键盘按键事件
        /// </summary>
        /// <param name="sender">obj</param>
        /// <param name="e">按键事件</param>
        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            // 抑制按键声音
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
                        System.Diagnostics.Debug.WriteLine("暂停游戏");
                        this.timer.Enabled = false;
                    }
                    else
                    {
                        // 重新复活
                        if (!this.tetrisStage.IsAlive)
                        {
                            this.tetrisStage.Init();
                        }
                        System.Diagnostics.Debug.WriteLine("游戏开始");
                        this.timer.Enabled = true;
                    }
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Useless Key: " + e.KeyCode);
                    break;
            }
        }

        /// <summary>
        /// 向下移动方块事件处理器
        /// 
        /// 按键触发或者定时器触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownwaroEventHandler(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Tick");
                this.tetrisStage.DownwardBlock();
                // 得分: 消除一行得一分
                this.labelScore.Text = $"得分: {this.tetrisStage.Score}";
            }
            catch (Exception ex)
            {
                this.timer.Enabled = false;
                System.Diagnostics.Debug.WriteLine("游戏结束: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("游戏结束: " + ex);
                MessageBox.Show(this, "游戏结束");
            }
        }
    }
}
