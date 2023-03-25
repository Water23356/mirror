using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yukari
{
    /// <summary>
    /// 步骤信息
    /// </summary>
    public class StepInfo
    {
        #region 属性
        /// <summary>
        /// 步骤函数
        /// </summary>
        public FunctionInfo function;
        /// <summary>
        /// 跳转状态
        /// </summary>
        public List<SkipInfo> skips = new List<SkipInfo>();
        #endregion

        public void PrintInfo(string tabto)
        {
            Console.WriteLine(tabto+"【步骤】{");
            function.PrintInfo(tabto+"  ");
            foreach(SkipInfo s in skips)
            {
                s.PrintInfo(tabto + "  ");
            }
            Console.WriteLine(tabto+"}");
        }
        /// <summary>
        /// 清除空元素
        /// </summary>
        public void ClearNull()
        {
            function.ClearNull();
            int i = 0;
            while(i<skips.Count)
            {
                if (skips[i] == null)
                {
                    skips.RemoveAt(i);
                }
                else
                {
                    skips[i].ClearNull();
                    ++i;
                }
            }
        }
    }
}
