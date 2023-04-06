using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Mod_Entity
{
    /// <summary>
    /// 状态机状态
    /// </summary>
    public enum State 
    { 
        /// <summary>
        /// 未启动
        /// </summary>
        wait,
        /// <summary>
        /// 开始阶段（前段）
        /// </summary>
        start,
        /// <summary>
        /// 运行阶段（中段）
        /// </summary>
        running,
        /// <summary>
        /// 结束阶段（后段）
        /// </summary>
        end
    }

    /// <summary>
    /// 行为状态信息结构体
    /// </summary>
    public struct StateBH
    {
        /// <summary>
        /// 行为名称
        /// </summary>
        public string name;
        /// <summary>
        /// 可被一下行为取消
        /// </summary>
        public string[] breakers;
        /// <summary>
        /// 当前行为的状态
        /// </summary>
        public State status;
    }
    /// <summary>
    /// 行为事件
    /// </summary>
    public delegate void DelBehaviour();

    /// <summary>
    /// 行为模块抽象基类
    /// </summary>
    public abstract class BHBase : StaticAttribute
    {
        #region 字段|属性
        protected new ExciteEntity owner;
        /// <summary>
        /// 启用此行为时触发的事件
        /// </summary>
        public event DelBehaviour StartEvent;
        /// <summary>
        /// 结束此行为时触发的事件
        /// </summary>
        public event DelBehaviour EndEvent;
        protected bool started = false;//是否已开始此行为
        protected bool init = false;
        #endregion

        #region 属性
        /// <summary>
        /// 是否经过初始化
        /// </summary>
        public bool Init { get => init;protected set => init = value; }
        public override Entity Owner { get => owner; set => owner = value as ExciteEntity; }
        /// <summary>
        /// 获取当前行为信息
        /// </summary>
        public StateBH Info
        {
            get
            {
                return new StateBH
                {
                    name = attributeName 
                };
            }
        }
        #endregion

        #region 构造函数
        public BHBase(ExciteEntity _owner):base(_owner)
        {
            owner = _owner;
            attributeName = "基态行为";
        }
        #endregion

        #region 功能函数
        public override object GetStatus()
        {
            return Info;
        }
        /// <summary>
        /// 运行此行为
        /// </summary>
        /// <returns>行为是否结束</returns>
        public virtual bool Run()
        {
            if(!started)
            {
                started = true;
                BehaviourStart();
            }
            if(BehaviourContent())
            {
                BehaviourEnd();
                started = false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 行为开始前的内容
        /// </summary>
        protected abstract void BehaviourStart();
        /// <summary>
        ///  行为内容
        /// </summary>
        /// <returns>行为是否结束</returns>
        protected abstract bool BehaviourContent();
        /// <summary>
        /// 行为结束前的内容
        /// </summary>
        protected abstract void BehaviourEnd();
        #endregion
    }
}
