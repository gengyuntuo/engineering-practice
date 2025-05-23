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
            // ���ö�ʱ����������������쳣����ֹͣ��ʱ��
            this.timer.Tick += (sender, e) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Tick");
                    this.snake.Move();
                    // �÷�: ��һ����һ��, ԭʼ����3
                    this.labelScore.Text = $"����: {this.snake.SnakeBody.Count - 3}";
                }
                catch (Exception ex)
                {
                    this.timer.Enabled = false;
                    System.Diagnostics.Debug.WriteLine("��Ϸ����: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine("��Ϸ����: " + ex);
                    MessageBox.Show("��Ϸ����");
                }
            };

            StringBuilder sb = new();
            sb.Append("��������ʹ�ü����ϵ���������(�����Ǽ����ϵ�WASD)�����Ʒ����ƶ�\n");
            sb.Append('\n');
            sb.Append("��ʼ��Ϸ: �ո��(��س���)��ͣ/��ʼ��Ϸ��Ϸ\n");
            sb.Append('\n');
            sb.Append("��Ϸ����: \n");
            sb.Append("1. �߲���ײ��ǽ��; \n");
            sb.Append("2. �߲���ײ���Լ�������; \n");
            sb.Append("3. �Ե�ʳ�����+1; \n");
            richTextBoxDescription.Text = sb.ToString();
        }

        /// <summary>
        /// ���̰����¼�
        /// </summary>
        /// <param name="sender">obj</param>
        /// <param name="e">�����¼�</param>
        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            // ���ư�������
            e.SuppressKeyPress = true;
            System.Diagnostics.Debug.WriteLine("Key event: " + e.KeyCode);

            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    // ����ֱ�����෴����ת��ֻ���������һ�����ǰ
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
                        System.Diagnostics.Debug.WriteLine("��ͣ��Ϸ");
                        this.timer.Enabled = false;
                    }
                    else
                    {
                        // ������Ѿ����ˣ����������¸���
                        System.Diagnostics.Debug.WriteLine("��Ϸ��ʼ");
                        if (!this.snake.IsAlive)
                        {
                            System.Diagnostics.Debug.WriteLine("����̰����");
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
