using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameComponent
{
    /// <summary>
    /// 蛇头朝向
    /// </summary>
    enum SnakeOrientation
    {
        Up, Down, Left, Right
    }

    /// <summary>
    /// 蛇
    /// </summary>
    interface ISnake
    {
        /// <summary>
        /// 蛇的生命状态
        /// </summary>
        public Boolean IsAlive { get; set; }
        /// <summary>
        /// 蛇头朝向
        /// </summary>
        public SnakeOrientation HeadOrientation { get; set; }

        /// <summary>
        /// 蛇的身体，List的第一个节点是蛇头，最后一个节点是蛇尾
        /// </summary>
        public List<Tuple<int, int>> SnakeBody { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 向前移动
        /// </summary>
        void Move();

        /// <summary>
        /// 生成食物
        /// </summary>
        void GenerateEgg();
    }

    /// <summary>
    /// 简单蛇
    /// </summary>
    class SimpleSnake(GamePlace<Panel> gamePlace) : ISnake
    {
        private SnakeOrientation HeadOrientation = SnakeOrientation.Right;
        private readonly List<Tuple<int, int>> snakeBody = [];
        private Boolean isAlive = false;
        private readonly GamePlace<Panel> gamePlace = gamePlace;

        /// <summary>
        /// 蛇头朝向
        /// </summary>
        SnakeOrientation ISnake.HeadOrientation
        {
            get => this.HeadOrientation;
            set => this.HeadOrientation = value;
        }

        bool ISnake.IsAlive { get => this.isAlive; set => this.isAlive = value; }

        public List<Tuple<int, int>> SnakeBody => this.snakeBody;

        void ISnake.Move()
        {
            if (!this.isAlive)
            {
                throw new Exception("Snake is dead");
            }

            try
            {
                // 蛇头的当前坐标
                var head = this.SnakeBody.First();

                // 计算新坐标
                var newHead = this.HeadOrientation switch
                {
                    SnakeOrientation.Left => Tuple.Create(head.Item1 - 1, head.Item2),
                    SnakeOrientation.Up => Tuple.Create(head.Item1, head.Item2 - 1),
                    SnakeOrientation.Down => Tuple.Create(head.Item1, head.Item2 + 1),
                    SnakeOrientation.Right => Tuple.Create(head.Item1 + 1, head.Item2),
                    _ => throw new NotImplementedException(),
                };

                // 获取新位置的场地类型，如果越界此处会报错
                Color locationType = this.gamePlace.GetLocationType(newHead);

                // 根据场地类型做出对应的操作
                if (Color.White.Equals(locationType))
                {
                    // 空白处，向前移动：删除蛇尾，将新坐标添加到蛇头的位置
                    var tail = this.snakeBody.Last();
                    this.snakeBody.RemoveAt(this.snakeBody.Count - 1);
                    this.snakeBody.Insert(0, newHead);
                    // 渲染蛇
                    this.gamePlace.RenderSnake(newHead);
                    this.gamePlace.ClearGridCell(tail);
                }
                else if (Color.Red.Equals(locationType))
                {
                    // 吃到了蛋
                    this.snakeBody.Insert(0, newHead);
                    this.gamePlace.RenderSnake(newHead);

                    // 创建新蛋
                    GenerateEgg();
                }
                else if (Color.Green.Equals(locationType))
                {
                    // 撞到了自己
                    throw new Exception($"Crash snake's body: from {head}to {newHead}");
                }
                else
                {
                    throw new NotImplementedException("Color is :" + locationType);
                }
            }
            catch (Exception)
            {
                this.isAlive = false;
                throw;
            }

        }

        /// <summary>
        /// 初始化贪吃蛇
        /// 身体长度3，方向朝右
        /// </summary>
        void ISnake.Init()
        {
            this.isAlive = true;
            this.snakeBody.Clear();
            this.snakeBody.Insert(0, Tuple.Create(1, 1));
            this.snakeBody.Insert(0, Tuple.Create(2, 1));
            this.snakeBody.Insert(0, Tuple.Create(3, 1));
            this.HeadOrientation = SnakeOrientation.Right;

            // 清空场地
            this.gamePlace.FlushPlaceGrid();
            // 渲染蛇
            for (int i = 0; i < snakeBody.Count; i++)
            {
                this.gamePlace.RenderSnake(snakeBody[i]);
            }
            // 生成食物
            GenerateEgg();
        }

        public void GenerateEgg()
        {
            for (; ; )
            {
                var eggX = Random.Shared.Next(this.gamePlace.PlaceWidth);
                var eggY = Random.Shared.Next(this.gamePlace.PlaceHeight);
                var eggNewLocation = Tuple.Create(eggX, eggY);
                // 这里简单实现，更合适的做法是列出所有未占用的位置，然后再随机选取
                if (!gamePlace.LocationIsOccupied(eggNewLocation))
                {
                    this.gamePlace.RenderEgg(eggNewLocation);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 食物
    /// </summary>
    class Egg
    {
        /// <summary>
        /// 食物位置
        /// </summary>
        private readonly Tuple<int, int> location;

        public Egg(int x, int y)
        {
            this.location = Tuple.Create(x, y);
        }

        public Tuple<int, int> Location { get { return location; } }
    }

    /// <summary>
    /// 游戏场地控件
    /// 场地的左上角坐标为(0,0), 横向为x轴 纵向为y轴
    /// </summary>
    /// <typeparam name="T">控件</typeparam>
    class GamePlace<T> where T : Control
    {
        /// <summary>
        /// 游戏场景控件
        /// </summary>
        private readonly T control;
        /// <summary>
        /// 游戏场地网格
        /// </summary>
        private readonly Label[][] placeGrid;

        /// <summary>
        /// 场地的宽度
        /// </summary>
        public int PlaceWidth { get { return placeGrid.Length; } }
        /// <summary>
        /// 场地的高度
        /// </summary>
        public int PlaceHeight { get { return placeGrid[0].Length; } }
        public GamePlace(T control)
        {
            this.control = control;
            var gridWidth = this.control.Width / 20;
            var gridHeight = this.control.Height / 20;
            this.placeGrid = new Label[gridWidth][];
            for (var i = 0; i < gridHeight; i++)
            {
                this.placeGrid[i] = new Label[gridHeight];
                for (var j = 0; j < gridWidth; j++)
                {
                    Label label = new()
                    {
                        BackColor = Color.White,
                        Width = 20,
                        Height = 20,
                        Location = new Point(i * 20, j * 20),
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    this.control.Controls.Add(label);
                    this.placeGrid[i][j] = label;
                }
            }
        }

        /// <summary>
        /// 清空场地
        /// </summary>
        public void FlushPlaceGrid()
        {
            for (int i = 0; i < placeGrid.Length; i++)
            {
                for (int j = 0; j < placeGrid[i].Length; j++)
                {
                    placeGrid[i][j].BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// 渲染蛇, 绿色
        /// </summary>
        /// <param name="location">位置</param>
        public void RenderSnake(Tuple<int, int> location)
        {
            placeGrid[location.Item1][location.Item2].BackColor = Color.Green;
        }
        /// <summary>
        /// 渲染蛋，黄色
        /// </summary>
        /// <param name="location">位置</param>
        public void RenderEgg(Tuple<int, int> location)
        {
            placeGrid[location.Item1][location.Item2].BackColor = Color.Red;
        }
        /// <summary>
        /// 清空指定位置
        /// </summary>
        /// <param name="location">位置</param>
        public void ClearGridCell(Tuple<int, int> location)
        {
            placeGrid[location.Item1][location.Item2].BackColor = Color.White;
        }

        /// <summary>
        /// 检验当前场地是否被占用
        /// </summary>
        /// <param name="location">在场地中的位置</param>
        /// <returns></returns>
        public bool LocationIsOccupied(Tuple<int, int> location)
        {
            return placeGrid[location.Item1][location.Item2].BackColor != Color.White;
        }

        /// <summary>
        /// 从场地中获取指定位置的类型，目前采用三种颜色分别代表三种类型：
        /// Color.White 空白
        /// Color.Red   食物
        /// Color.Green 蛇身
        /// </summary>
        /// <param name="location">坐标</param>
        /// <returns>类型</returns>
        public Color GetLocationType(Tuple<int, int> location)
        {
            return placeGrid[location.Item1][location.Item2].BackColor;
        }
    }
}
