using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// 俄罗斯方块游戏舞台
    /// </summary>
    class TetrisStage
    {
        /// <summary>
        /// 背景色
        /// </summary>
        private readonly Color backgroundColor = Color.White;
        /// <summary>
        /// 已经固定的方块颜色
        /// </summary>
        private readonly Color foregroundColor = Color.DarkGray;
        /// <summary>
        /// Panel 游戏舞台
        /// </summary>
        private readonly Panel mainPanel;
        /// <summary>
        /// Panel 下一个方块展示区
        /// </summary>
        private readonly Panel nextBlockPanel;
        /// <summary>
        /// 主舞台网格
        /// </summary>
        private readonly Label[,] stageGrid;
        /// <summary>
        /// 下一个方框展台网格
        /// </summary>
        private readonly Label[,] nextBlockStageGrid;

        private Shape currentBlock;
        private Shape nextBlock;

        /// <summary>
        /// 得分
        /// </summary>
        private int score = 0;
        /// <summary>
        /// 游戏是否结束, true 没有结束 false结束
        /// </summary>
        private bool isAlive = false;

        public int Score { get { return score; } }

        public bool IsAlive { get { return isAlive; } }

        public TetrisStage(Panel mainPanel, Panel nextBlockPanel)
        {
            this.mainPanel = mainPanel;
            this.nextBlockPanel = nextBlockPanel;
            // 多3行，在最顶行，防止用户，这3行不展示给用户
            this.stageGrid = new Label[mainPanel.Width / 40, mainPanel.Height / 40 + 3];
            this.nextBlockStageGrid = new Label[4, 4];

            for (int x = 0; x < stageGrid.GetLength(0); x++)
            {
                for (int y = 0; y < stageGrid.GetLength(1); y++)
                {
                    Label label = new()
                    {
                        Width = 40,
                        Height = 40,
                        Location = new(x * 40, (y - 3) * 40), // 顶部的3行不展示
                        BackColor = backgroundColor,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    this.mainPanel.Controls.Add(label);
                    this.stageGrid[x, y] = label;
                }
            }

            for (int i = 0; i < nextBlockStageGrid.GetLength(0); i++)
            {
                for (int j = 0; j < nextBlockStageGrid.GetLength(1); j++)
                {
                    Label label = new()
                    {
                        Width = 40,
                        Height = 40,
                        Location = new(i * 40, j * 40),
                        BackColor = backgroundColor,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    this.nextBlockPanel.Controls.Add(label);
                    this.nextBlockStageGrid[i, j] = label;
                }
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            currentBlock = GenerateBlock();
            nextBlock = GenerateBlock();
            this.score = 0;
            this.isAlive = true;
            FlushMainPanel();
            FlushNextBlockPanel();
            RenderMainStage();
            RenderNextBlock();
        }
        /// <summary>
        /// 生成新的方块
        /// </summary>
        /// <returns></returns>
        private Shape GenerateBlock()
        {
            // 随机获取方块
            var block = Block.Blocks[Random.Shared.Next(Block.Blocks.Length)];
            var shapeArray = block.ShapeArray;
            // 基于方块的原始形状，随机旋转
            for (var i = 0; i < Random.Shared.Next(4); i++)
            {
                shapeArray = Shape.Rotate(shapeArray);
            }

            // 计算方块在舞台中的坐标
            var location = Tuple.Create(
                // 横坐标使图形居中显示
                (stageGrid.GetLength(0) - shapeArray.GetLength(1)) / 2,
                // 总坐标, 展示图形的最底部, （注意：顶部3行不展示给用户，方块最开始只在顶部展示方块最下面的一行）
                3 - shapeArray.GetLength(0) + 1
                );
            return new Shape(block)
            {
                Location = location,
                ShapeArray = shapeArray
            };
        }

        /// <summary>
        /// 在舞台中展示当前方块
        /// </summary>
        private void RenderMainStage()
        {
            // 判断当前方块是否可以被渲染，如果不可以被渲染，则游戏失败。
            var isValid = DetectMargin(this.currentBlock.ShapeArray, this.currentBlock.Location);
            if (!isValid || !this.isAlive)
            {
                this.isAlive = false;
                throw new Exception("Game Over");
            }
            // 清空当前的图形
            for (int x = 0; x < stageGrid.GetLength(0); x++)
            {
                for (int y = 0; y < stageGrid.GetLength(1); y++)
                {
                    // 不是背景色，也不是已经固定的方块儿则将其设置为背景色，即去掉当前的方块儿
                    if (this.stageGrid[x, y].BackColor != backgroundColor
                        && this.stageGrid[x, y].BackColor != foregroundColor)
                    {
                        this.stageGrid[x, y].BackColor = backgroundColor;
                    }
                }
            }
            // 重新渲染新的方块儿位置
            var blockCoordinates = this.currentBlock.CalculateCoordinate();
            System.Diagnostics.Debug.WriteLine("Main Stage Coordinates: " + string.Join(", ", blockCoordinates));
            for (int i = 0; blockCoordinates.Count > i; i++)
            {
                this.stageGrid[blockCoordinates[i].Item1, blockCoordinates[i].Item2].BackColor = this.currentBlock.RawBlock.BlockColor;
            }
        }

        /// <summary>
        /// 在下一个方块儿面板中展示方块儿
        /// </summary>
        private void RenderNextBlock()
        {
            if (this.nextBlock != null)
            {
                var x = (this.nextBlockStageGrid.GetLength(0) - this.nextBlock.ShapeArray.GetLength(1)) / 2;
                var y = (this.nextBlockStageGrid.GetLength(1) - this.nextBlock.ShapeArray.GetLength(0)) / 2;
                var coordinates = Shape.CalculateCoordinate(this.nextBlock.ShapeArray, Tuple.Create(x, y));
                FlushNextBlockPanel();
                foreach (var item in coordinates)
                {
                    this.nextBlockStageGrid[item.Item1, item.Item2].BackColor = this.nextBlock.RawBlock.BlockColor;
                }
            }
        }

        /// <summary>
        /// 旋转方块
        /// </summary>
        public void RotateBlock()
        {
            var newShapeArray = Shape.Rotate(this.currentBlock.ShapeArray);
            var isValid = DetectMargin(newShapeArray, this.currentBlock.Location);
            // 旋转成功
            if (isValid)
            {
                this.currentBlock.ShapeArray = newShapeArray;
                RenderMainStage();
            }
        }

        /// <summary>
        /// 横向移动方块
        /// </summary>
        /// <param name="step">横向看移动的步数, -1 向左移动一步, 1 向右移动一步</param>
        public void LeftOrRightMove(int step)
        {
            var newLocation = Tuple.Create(this.currentBlock.Location.Item1 + step, this.currentBlock.Location.Item2);
            var isValid = DetectMargin(this.currentBlock.ShapeArray, newLocation);
            if (isValid)
            {
                this.currentBlock.Location = newLocation;
                RenderMainStage();
            }
        }

        /// <summary>
        /// 下落方块
        /// </summary>
        public void DownwardBlock()
        {
            var newLocation = Tuple.Create(this.currentBlock.Location.Item1, this.currentBlock.Location.Item2 + 1);
            var isValid = DetectMargin(this.currentBlock.ShapeArray, newLocation);
            if (isValid)
            {
                // 正常下落
                this.currentBlock.Location = newLocation;
                RenderMainStage();
            }
            else
            {
                // 触底固定方块
                var coordinates = this.currentBlock.CalculateCoordinate();
                // 修改方块儿为前景色
                foreach (var coord in coordinates)
                {
                    this.stageGrid[coord.Item1, coord.Item2].BackColor = foregroundColor;
                }
                // 检测消除行
                // 可消除的行
                List<int> dismissedRow = [];
                // 保留的行
                List<int> reservedRow = [];
                for (var rowIndex = this.stageGrid.GetLength(1) - 1; rowIndex >= 0; rowIndex--)
                {
                    // count 舞台中rowIndex行的方块数
                    int count = 0;
                    for (var colIndex = 0; this.stageGrid.GetLength(0) > colIndex; colIndex++)
                    {
                        if (this.stageGrid[colIndex, rowIndex].BackColor == foregroundColor)
                        {
                            count++;
                        }
                    }
                    // 不完整的行，保留
                    if (count > 0 && count < this.stageGrid.GetLength(0))
                    {
                        reservedRow.Add(rowIndex);
                    }
                    // 完整的行，可以消除
                    else if (count == this.stageGrid.GetLength(0))
                    {
                        dismissedRow.Add(rowIndex);
                    }
                    // 全空的行，无需检验其他的行
                    else
                    {
                        break;
                    }
                }
                // 得分&消除行
                if (dismissedRow.Count > 0)
                {
                    this.score += dismissedRow.Count;
                    // 消除后仍需保留的行数
                    var reservedRows = reservedRow.Count;
                    // 行下标，从最底行开始
                    var rowIndex = this.stageGrid.GetLength(1) - 1;
                    foreach (var row in reservedRow)
                    {
                        for (var x = 0; x < stageGrid.GetLength(0); x++)
                        {
                            // 复制颜色到新的行
                            stageGrid[x, rowIndex].BackColor = stageGrid[x, row].BackColor;
                        }
                        rowIndex--;
                    }
                    // 剩下的行全部置空
                    for (var y = rowIndex; y >= 0; y--)
                    {
                        for (var x = 0; x < stageGrid.GetLength(0); x++)
                        {
                            // 删除原来的行
                            stageGrid[x, y].BackColor = backgroundColor;
                        }
                    }
                }

                // 创建渲染新的方块
                this.currentBlock = nextBlock;
                this.nextBlock = GenerateBlock();
                RenderNextBlock();
                RenderMainStage();
            }
        }
        /// <summary>
        /// 边界检查, 是否越界，是否触及固定的方块儿
        /// </summary>
        /// <param name="shape">图像数组</param>
        /// <param name="location">方块儿坐标</param>
        /// <returns>true 表示未越界，false表示越界</returns>
        private bool DetectMargin(bool[,] shapeArray, Tuple<int, int> location)
        {
            var shapeCoordinates = Shape.CalculateCoordinate(shapeArray, location);
            foreach (var coord in shapeCoordinates)
            {
                if (coord.Item1 < 0 || coord.Item1 >= stageGrid.GetLength(0)
                    || coord.Item2 < 0 || coord.Item2 >= stageGrid.GetLength(1)
                    || stageGrid[coord.Item1, coord.Item2].BackColor == foregroundColor)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 刷新游戏舞台
        /// </summary>
        private void FlushMainPanel()
        {
            for (int i = 0; i < stageGrid.GetLength(0); i++)
            {
                for (int j = 0; j < stageGrid.GetLength(1); j++)
                {
                    stageGrid[i, j].BackColor = backgroundColor;
                }
            }
        }

        /// <summary>
        /// 刷新下个图形舞台
        /// </summary>
        private void FlushNextBlockPanel()
        {
            for (int i = 0; i < nextBlockStageGrid.GetLength(0); i++)
            {
                for (int j = 0; j < nextBlockStageGrid.GetLength(1); j++)
                {
                    nextBlockStageGrid[i, j].BackColor = backgroundColor;
                }
            }
        }
    }

    /// <summary>
    /// 方块
    /// </summary>
    class Block(Color blockColor, bool[,] shapeArray)
    {
        /// <summary>
        /// 方块颜色
        /// </summary>
        public Color BlockColor { get { return blockColor; } }
        /// <summary>
        /// 方块形状数组
        /// </summary>
        public bool[,] ShapeArray { get { return shapeArray; } }

        /// <summary>
        /// 所有的方块
        /// </summary>
        public static Block[] Blocks = [
        // F
        new Block(Color.Red, new bool[3,2]{{true, true,},{true, false,},{true, false,},}),
        // F (mirror)
        new Block(Color.Orange, new bool[3,2]{{true, true,},{false, true,},{false, true,},}),
        // Z
        new Block(Color.Yellow, new bool[2,3]{{false, true, true,},{true, true, false,},}),
        // Z (mirror)
        new Block(Color.Green, new bool[2,3]{{true, true, false,},{false, true, true,},}),
        // ┋
        new Block(Color.Blue, new bool[1,4]{{ true, true, true, true,},}),
        // ┻
        new Block(Color.Indigo, new bool[2,3]{{false, true, false,},{true, true, true,},}),
        // ■
        new Block(Color.Purple, new bool[2,2]{{true,true,},{true,true,},}),
        ];
    }

    /// <summary>
    /// 形状
    /// </summary>
    /// <param name="block"></param>
    class Shape(Block block)
    {
        private readonly Block block = block;
        /// <summary>
        /// 图形
        /// </summary>
        private bool[,] shape = block.ShapeArray;

        /// <summary>
        /// 坐标
        /// </summary>
        public required Tuple<int, int> Location { get; set; }
        /// <summary>
        /// 原始的Block
        /// </summary>
        public Block RawBlock { get => block; }

        /// <summary>
        /// 图形
        /// </summary>
        public bool[,] ShapeArray { get => shape; set => shape = value; }

        /// <summary>
        /// 将图形顺时针旋转90°
        /// </summary>
        /// <param name="shapeArray">图形数组</param>
        /// <returns>新的图形</returns>
        public static bool[,] Rotate(bool[,] shapeArray)
        {
            int rows = shapeArray.GetLength(0);
            int cols = shapeArray.GetLength(1);
            bool[,] result = new bool[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // 关键转换：[i,j] -> [j, rows-1-i]
                    result[j, rows - 1 - i] = shapeArray[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 基于指定位置计算当前图形各个点的坐标
        /// </summary>
        /// <param name="shapeArray">图形</param>
        /// <param name="location">基于此坐标计算该图形的各点坐标</param>
        /// <returns></returns>
        public static List<Tuple<int, int>> CalculateCoordinate(bool[,] shapeArray, Tuple<int, int> location)
        {
            var result = new List<Tuple<int, int>>();
            int rows = shapeArray.GetLength(0);
            int cols = shapeArray.GetLength(1);
            var coordinate = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (shapeArray[i, j])
                    {
                        result.Add(Tuple.Create(location.Item1 + j, location.Item2 + i));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 基于当前位置计算图形各个点的坐标
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int>> CalculateCoordinate()
        {
            return CalculateCoordinate(this.ShapeArray, this.Location);
        }
    }
}
