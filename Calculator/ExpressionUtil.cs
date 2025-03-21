using NCalc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class ExpressionUtil
    {
        [Obsolete("此方案不合适，无法用于科学计算的场景中")]
        public static double? CalculateExpression(string expr)
        {
            DataTable dt = new DataTable();
            double? result = Convert.ToDouble(dt.Compute(expr, ""));
            return result;
        }

        /// <summary>
        /// 计算增强的数学表达式
        /// </summary>
        /// <param name="expr">表达式</param>
        /// <returns>表达式结果</returns>
        public static double? CalculateEnhancedExpression(string expr)
        {
            Expression exprObj = new Expression(expr);
            exprObj.Parameters["PI"] = Math.PI;
            exprObj.Parameters["e"] = Math.E;
            double? result = Convert.ToDouble(exprObj.Evaluate());
            return result;
        }

    }

}
