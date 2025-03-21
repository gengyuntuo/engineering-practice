using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    /// <summary>
    /// 历史记录接口
    /// </summary>
    interface IHistoryRecorder
    {
        /// <summary>
        /// 存储历史记录
        /// </summary>
        /// <param name="expr">表达式</param>
        /// <param name="result">结果</param>
        void SaveExpressionAndResult(string expr, string result);
        /// <summary>
        /// 查询所有历史记录
        /// </summary>
        /// <param name="max">最大记录数, 默认10条</param>
        /// <returns>历史记录</returns>
        List<string> listHistory(int max = 10);
    }

    class MemoryHistoryRecorder : IHistoryRecorder
    {
        private List<string> resultList = new List<string>();
        public List<string> listHistory(int max = 10)
        {
            return resultList.GetRange(0, Math.Min(max, resultList.Count));
        }

        public void SaveExpressionAndResult(string expr, string result)
        {
            resultList.Insert(0, expr + " = " + result);
        }
    }
}
