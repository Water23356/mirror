using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mod_Entity
{

    /// <summary>
    /// 自带状态管理器的实体基类
    /// </summary>
    public class ExciteEntity : Entity
    {
        #region 字段
        protected string damageTag = "ExciteEntity";
        protected List<IAttribute> attributes = new List<IAttribute>();//实体属性
        protected Dictionary<string, SMSpace> spaces = new Dictionary<string, SMSpace>();//空间状态（名称，对象）
        protected Dictionary<string, BHBase> bhs = new Dictionary<string, BHBase>();//行为状态（名称，对象）
        protected SMSpace nowSp;//当前空间状态
        protected SMSpace defSp;//默认空间状态
        protected BHBase nowBh;//当前行为
        protected BHBase defBh;//默认行为状态
        protected bool CancelBackDefaultSP = false;//取消重置初始状态（用于对回归初始空间状态的开关）
        protected bool CancelBackDefaultBH = false;//取消重置初始状态（用于对回归初始行为状态的开关）
        #endregion

        #region 公开属性
        /// <summary>
        /// 当前空间状态
        /// </summary>
        public SMSpace NowSpaceStatus { get => nowSp; }
        /// <summary>
        /// 当前行为状态
        /// </summary>
        public BHBase NowBehaviourStatus { get => nowBh; }
        /// <summary>
        /// 实体拥有的空间状态属性
        /// </summary>
        public Dictionary<string, SMSpace> Spaces { get => spaces; }
        /// <summary>
        /// 实体拥有的行为状态属性
        /// </summary>
        public Dictionary<string, BHBase> BHs { get => bhs; }
        #endregion

        #region 功能函数
        /// <summary>
        /// 设置默认空间状态
        /// </summary>
        public void SetDefSpace(SMSpace smsp)
        {
            defSp = smsp;
        }
        /// <summary>
        /// 设置默认行为状态
        /// </summary>
        public void SetDefBehaviour(BHBase bhbs)
        {
            defBh = bhbs;
        }

        /// <summary>
        /// 改变当前空间状态
        /// </summary>
        /// <param name="name">空间状态名称</param>
        public virtual void ChangeStateSP(string name)
        {
            if (spaces.Keys.Contains(name))//存在指定空间状态
            {
                CancelBackDefaultSP = true;
                if (nowSp != null) { nowSp.Exit(); }
                nowSp = spaces[name];
                nowSp.Enter();
            }
            else
            {
                Debug.LogWarning("指定空间状态不存在!");
            }
        }
        /// <summary>
        /// 改变行为状态
        /// </summary>
        /// <param name="name">行为状态名称</param>
        public virtual void ChangeStateBH(string name)
        {
            Debug.Log("切换行为：目标:"+name);
            if (bhs.Keys.Contains(name))//存在指定行为
            {
                Debug.Log("存在此行为");
                BHBase bhb = bhs[name];
                if(nowBh != null)
                {
                    StateBH state = nowBh.Info;
                    bool change = false;
                    Debug.Log("当前行为存在：" + nowBh.Name+"  当前行为的前置中断条件数量：" +state.breakFront.Length+ "  当前行为的状态为：" + state.status);
                    switch (state.status)
                    {
                        case State.start:
                            if (state.breakFront.Contains(name))
                            {
                                change = true;
                            }
                            break;
                        case State.running:
                            if (state.breakRunning.Contains(name))
                            {
                                change = true;
                            }
                            break;
                        case State.end:
                            if (state.breakEnd.Contains(name))
                            {
                                change = true;
                            }
                            break;
                        case State.wait:
                            change = true;
                            break;
                    }
                    if (change)//满足当前行为离开条件
                    {
                        Debug.Log("满足中断条件");
                        if (bhb.StartSpace == null || bhb.StartSpace.Contains(nowSp.name))//满足前置空间要求
                        {
                            Debug.Log("满足前置空间条件");
                            if (bhb.StartFront == null || bhb.StartFront.Contains(nowBh.name))//满足前置行为要求
                            {
                                Debug.Log("满足前置行为条件,开始切换");
                                CancelBackDefaultBH = true;
                                nowBh.StopBH();
                                nowBh = bhb;
                                nowBh.StartBH();
                            }
                        }
                    }
                }
                else
                {
                    nowBh = bhb;
                    nowBh.StartBH();
                }
            }
            else
            {
                Debug.LogWarning("指定行为不存在!");
            }
        }
        /// <summary>
        /// 添加新的属性对象
        /// </summary>
        public override void AddAttribute(IAttribute attribute)
        {
            if (attribute == null) return;
            if (attributes.Contains(attribute)) return;
            attributes.Add(attribute);
            BHBase bh = attribute as BHBase;
            if (bh != null)//添加行为模块
            {
                bhs.Add(bh.Name, bh);
                bh.AddEndAction(BHEndAction);
            }
            SMSpace smsp = attribute as SMSpace;
            if (smsp != null)//添加空间状态模块
            {
                spaces.Add(smsp.Name, smsp);
                smsp.AddExitAction(SPEndAction);
            }
            attribute.Owner = this;
        }
        /// <summary>
        /// 从这个实体中移除指定属性（同时销毁组件）
        /// </summary>
        /// <param name="attribute"></param>
        public override void RemoveAttribute(IAttribute attribute)
        {
            if (attributes.Contains(attribute))
            {
                attributes.Remove(attribute);
                BHBase bh = attribute as BHBase;
                if (bh != null)
                {
                    bhs.Remove(bh.Name);
                }
                SMSpace smsp = attribute as SMSpace;
                if (smsp != null)//添加空间状态模块
                {
                    spaces.Remove(smsp.Name);
                }
                attribute.Destroy();
            }
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 在空间结束时调用的事件；空间状态自动切换为默认空间状态
        /// </summary>
        protected void SPEndAction()
        {
            if (CancelBackDefaultSP)
            {
                CancelBackDefaultSP = false;
            }
            else
            {
                nowSp = defSp;
            }
        }
        /// <summary>
        /// 在行为结束时调用的事件;行为状态结束时自动切换到一般状态
        /// </summary>
        protected void BHEndAction()
        {
            if (CancelBackDefaultBH)
            {
                CancelBackDefaultBH = false;
            }
            else
            {
                nowBh = defBh;
            }
        }

        #endregion

        #region Unity
        #endregion
    }
}
