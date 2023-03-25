using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yukari
{
    /// <summary>
    /// 跳转信息
    /// </summary>
    public class SkipInfo
    {
        #region 属性
        /// <summary>
        /// 目标单元
        /// </summary>
        public string name;
        /// <summary>
        /// 条件
        /// </summary>
        public string condition;
        /// <summary>
        /// 起始步骤索引（默认为0）
        /// </summary>
        public int index;
        /// <summary>
        /// 过渡函数
        /// </summary>
        public List<FunctionInfo> functions = new List<FunctionInfo>();
        #endregion

        public void PrintInfo(string tabto)
        {
            Console.WriteLine(tabto + $"【出口】:{name}:{condition}"+"{");
            foreach(FunctionInfo f in functions)
            {
                f.PrintInfo(tabto+"  ");
            }
            Console.WriteLine(tabto+"}");
        }
        /// <summary>
        /// 清除空元素
        /// </summary>
        public void ClearNull()
        {
            int i = 0;
            while(i<functions.Count)
            {
                if (functions[i] == null)
                {
                    functions.RemoveAt(i);
                }
                else
                {
                    functions[i].ClearNull();
                    ++i;
                }
            }
        }
    }

}
