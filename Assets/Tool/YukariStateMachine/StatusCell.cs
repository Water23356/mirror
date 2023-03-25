using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Yukari
{
    /// <summary>
    /// 状态单元
    /// </summary>
    public class StatusCell
    {
        #region 属性
        /// <summary>
        /// 状态名称
        /// </summary>
        public string name;
        /// <summary>
        /// 状态步骤
        /// </summary>
        private List<StepInfo> steps = new List<StepInfo>();
        /// <summary>
        /// 当前步骤索引
        /// </summary>
        public int Index { get; set; }
        #endregion

        #region 构造函数
        public StatusCell() { steps = new List<StepInfo>(); }
        public StatusCell(string name_) { name = name_; steps = new List<StepInfo>(); }
        #endregion

        #region 响应函数
        public void AddStep(StepInfo step) { steps.Add(step); }
        /// <summary>
        /// 获取该状态指定步骤的信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StepInfo this[int index]
        {
            get 
            {
                if (index > -1 && index < steps.Count) { return steps[index]; }
                return null;
            }
        }
        /// <summary>
        /// 获取当前步骤的信息，并将索引向后移动一位
        /// </summary>
        /// <returns></returns>
        public StepInfo Next()
        {
            if (Index > -1 && Index < steps.Count) { return steps[Index++]; }
            return null;
        }
        public StatusCell Copy(int index)
        {
            return new StatusCell()
            {
                name = name,
                steps = steps,
                Index = index
            };
        }
        /// <summary>
        /// 打印状态信息
        /// </summary>
        public void PrintInfo()
        {
            Console.WriteLine($"【状态】:{name}" +"{");
            int i = 0;
            while(i<steps.Count)
            {
                steps[i].PrintInfo("  ");
                ++i;
            }
            Console.WriteLine("}");
        }
        /// <summary>
        /// 清除空元素
        /// </summary>
        public void ClearNull()
        {
            int i = 0;
            while(i<steps.Count)
            {
                if (steps[i] == null)
                {
                    steps.RemoveAt(i);
                }
                else
                {
                    steps[i].ClearNull();
                    ++i;
                }
            }
        }
        #endregion
    }
}