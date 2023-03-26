using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 控制面板，需要接受输入的脚本需要实现这个抽象类
    /// </summary>
    public abstract class ControlPanel:MonoBehaviour
    {
        #region 属性
        /// <summary>
        /// 是否拥有控制权
        /// </summary>
        private bool contorl = false;
        #endregion

        #region 功能函数
        /// <summary>
        /// 开启控制权（非控制分配器不要调用这个函数）
        /// </summary>
        public void PowerOn()
        {
            contorl = true;
        }
        /// <summary>
        /// 关闭控制权（非控制分配器不要调用这个函数）
        /// </summary>
        public void PowerOff()
        {
            contorl = false;
        }
        /// <summary>
        /// 输入监听（仅在拥有控制权时调用）
        /// </summary>
        public abstract void InputMonitor();
        /// <summary>
        /// 在update后于 输入监听 调用的函数
        /// </summary>
        public abstract void UpdateFunction();
        /// <summary>
        /// 初始化调用的函数
        /// </summary>
        public abstract void initialization();
        #endregion
        public void Start()
        {
            if(ControllerAlloter.instance!=null)
            {

            }
        }
        public void Update()
        {
            if(contorl)
            {
                InputMonitor();
            }
            UpdateFunction();
        }
    }
}
