using System.Text;
using GameComponent;

namespace SnakeGame
{
    public partial class SnakeGameForm : Form
    {
        private readonly GamePlace<Panel> gamePlace;
        private readonly ISnake snake;
        private readonly System.Windows.Forms.Timer timer = new();
        public SnakeGameForm()
        {
            InitializeComponent();

            this.gamePlace = new(this.panel);
            this.snake = new SimpleSnake(this.gamePlace);
            this.snake.Init();

            this.KeyDown += KeyDownEvent;

            this.timer.Interval = 200;
            // 设置定时器操作，如果发生异常，则停止定时器
            this.timer.Tick += (sender, e) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Tick");
                    this.snake.Move();
                    // 得分: 吃一个得一分, 原始长度3
                    this.labelScore.Text = $"分数: {this.snake.SnakeBody.Count - 3}";
                }
                catch (Exception ex)
                {
                    this.timer.Enabled = false;
                    System.Diagnostics.Debug.WriteLine("游戏结束: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine("游戏结束: " + ex);
                    MessageBox.Show("游戏结束");
                }
            };

            StringBuilder sb = new();
            sb.Append("操作：请使用键盘上的上下左右(或者是键盘上的WASD)来控制方向移动\n");
            sb.Append('\n');
            sb.Append("开始游戏: 空格键(或回车键)暂停/开始游戏游戏\n");
            sb.Append('\n');
            sb.Append("游戏规则: \n");
            sb.Append("1. 蛇不能撞到墙壁; \n");
            sb.Append("2. 蛇不能撞到自己的身体; \n");
            sb.Append("3. 吃到食物分数+1; \n");
            richTextBoxDescription.Text = sb.ToString();
        }

        /// <summary>
        /// 键盘按键事件
        /// </summary>
        /// <param name="sender">obj</param>
        /// <param name="e">按键事件</param>
        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            // 抑制按键声音
            e.SuppressKeyPress = true;
            System.Diagnostics.Debug.WriteLine("Key event: " + e.KeyCode);

            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    // 不能直接向相反方向转向，只能向左向右或者向前
                    if (this.snake.HeadOrientation != SnakeOrientation.Down)
                    {
                        this.snake.HeadOrientation = SnakeOrientation.Up;
                    }
                    break;
                case Keys.Down:
                case Keys.S:
                    if (this.snake.HeadOrientation != SnakeOrientation.Up)
                    {
                        this.snake.HeadOrientation = SnakeOrientation.Down;
                    }
                    break;
                case Keys.Left:
                case Keys.A:
                    if (this.snake.HeadOrientation != SnakeOrientation.Right)
                    {
                        this.snake.HeadOrientation = SnakeOrientation.Left;
                    }
                    break;
                case Keys.Right:
                case Keys.D:
                    if (this.snake.HeadOrientation != SnakeOrientation.Left)
                    {
                        this.snake.HeadOrientation = SnakeOrientation.Right;
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
                        // 如果蛇已经死了，则让蛇重新复活
                        System.Diagnostics.Debug.WriteLine("游戏开始");
                        if (!this.snake.IsAlive)
                        {
                            System.Diagnostics.Debug.WriteLine("复活贪吃蛇");
                            this.snake.Init();
                        }
                        this.timer.Enabled = true;
                    }
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Useless Key: " + e.KeyCode);
                    break;
            }
        }
    }
}
