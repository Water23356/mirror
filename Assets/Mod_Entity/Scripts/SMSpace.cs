using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod_Entity
{
    /// <summary>
    /// 空间状态机基类（不负责动画）
    /// </summary>
    public abstract class SMSpace:DynamicAttribute
    {
        #region 字段
        protected new readonly string attributeName = "空间状态";//属性名称
        protected new ExciteEntity owner;
        protected Action enterAction;//进入此状态时调用的事件
        protected Action exitAction;//离开此状态时调用的事件
        protected Action sustainAction;//此状态持续时调用的事件（持续调用）
        #endregion

        #region 属性
        public override string Name { get => attributeName; set { } }
        public override Entity Owner { get => owner; set => owner = value as ExciteEntity; }
        #endregion

        #region 委托管理
        /// <summary>
        /// 添加事件委托（开始事件）
        /// </summary>
        /// <param name="action"></param>
        public void AddEnterAction(Action action) { enterAction += action; }
        /// <summary>
        /// 清空事件委托（开始事件）
        /// </summary>
        public void ClearEnterAction() { enterAction = null; }
        /// <summary>
        /// 添加事件委托（结束事件）
        /// </summary>
        /// <param name="action"></param>
        public void AddExitAction(Action action) { exitAction += action; }
        /// <summary>
        /// 清空事件委托（结束事件）
        /// </summary>
        public void ClearExitAction() { exitAction = null; }
        /// <summary>
        /// 添加事件委托（持续事件）
        /// </summary>
        /// <param name="action"></param>
        public void AddSustainAction(Action action) { sustainAction += action; }
        /// <summary>
        /// 清空事件委托（持续事件）
        /// </summary>
        public void ClearSustainAction() { sustainAction = null; }
        #endregion

        #region 功能函数
        /// <summary>
        /// 进入此状态
        /// </summary>
        public virtual void Enter()
        {
            if (enterAction != null) { enterAction(); }
            enabled = true;
        }
        /// <summary>
        /// 离开此状态
        /// </summary>
        public virtual void Exit()
        {
            if (exitAction != null) { exitAction(); }
            enabled = false;
        }
        #endregion
        /// <summary>
        /// 环境监测
        /// </summary>
        protected abstract void EnvironCheck();
        #region Unity
        private void Update()
        {
            if(sustainAction != null) { sustainAction(); }
            EnvironCheck();
        }
        #endregion


    }
}
