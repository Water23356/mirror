using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 控制器分配器
    /// </summary>
    public class ControllerAlloter : MonoBehaviour
    {
        #region 单例封装
        /// <summary>
        /// 单例对象
        /// </summary>
        public static ControllerAlloter instance;
        #endregion

        #region 属性
        /// <summary>
        /// 最大同时控制数量
        /// </summary>
        private int maxCount = 4;
        /// <summary>
        /// 控制权栈集（最多允许4个面板同时拥有控制权）
        /// </summary>
        private Stack<ControlPanel>[] controlStacks = new Stack<ControlPanel>[4];
        #endregion

        #region 公开属性
        /// <summary>
        /// 允许同时拥有控制权的面板 最大数量
        /// </summary>
        public int MaxCount
        {
            get => maxCount;
            set
            {
                maxCount = value;
                if (maxCount <= 0) { maxCount = 1; }
                controlStacks = new Stack<ControlPanel>[maxCount];
            }
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 获取（主栈）控制权(覆盖同一栈内的控制面板的权)
        /// </summary>
        /// <param name="obj">需要控制权的面板对象</param>
        /// <returns>获取是否成功</returns>
        public bool GetPower(ControlPanel obj)
        {
            Debug.Log($"有面板尝试获取权限：{obj.panelName}");
            ControlPanel last = null;
            if(controlStacks[0].TryPeek(out last))
            {
                last.PowerOff();
            }
            controlStacks[0].Push(obj);
            obj.PowerOn();
            return true;
        }
        /// <summary>
        /// 关闭指定面板的控制权（只有该面板位于栈顶时才能删除）
        /// </summary>
        /// <param name="obj">面板对象</param>
        /// <returns>操作是否成功</returns>
        public bool OffPower(ControlPanel obj)
        {
            bool del = false;
            foreach(var stk in controlStacks)
            {
                ControlPanel last = null;
                if (stk.TryPeek(out last))
                {
                    if(last == obj)
                    {
                        obj.PowerOff();
                        stk.Pop();
                        if(stk.TryPeek(out last))
                        {
                            last.PowerOn();
                        }
                        del = true;
                    }
                }
            }
            return del;
        }
        /// <summary>
        /// 获取并行控制权（不会覆盖原有的控制权）
        /// </summary>
        /// <param name="obj">需要控制权的面板</param>
        /// <returns>操作是否成功</returns>
        public bool GetParallelPower(ControlPanel obj) 
        {
            ControlPanel temp = null;
            foreach (var stk in controlStacks)
            {
                if(!stk.TryPeek(out temp))
                {
                    obj.PowerOn();
                    stk.Push(obj);
                    return true;
                }
            }
            return false;
        }
        #endregion


        #region Unity
        private void Awake()
        {
            instance = this;
            controlStacks[0] = new Stack<ControlPanel>();
            controlStacks[1] = new Stack<ControlPanel>();
            controlStacks[2] = new Stack<ControlPanel>();
            controlStacks[3] = new Stack<ControlPanel>();
        }
        #endregion
    }
}
