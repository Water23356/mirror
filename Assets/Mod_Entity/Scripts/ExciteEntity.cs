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
        #region 属性
        private string damageTag = "Test";
        private List<IAttribute> attributes = new List<IAttribute>();
        /// <summary>
        /// 行为表(BHid,行为组件对象)
        /// </summary>
        private Dictionary<BHIDTable, BHBase> bhs = new Dictionary<BHIDTable, BHBase>();
        /// <summary>
        /// 当前状态等级
        /// </summary>.
        private int nowLeve;
        /// <summary>
        /// 当前行为状态id
        /// </summary>
        private BHIDTable BHid;
        /// <summary>
        /// 默认行为状态id
        /// </summary>
        private BHIDTable defaultBH;
        /// <summary>
        /// 实体空间状态
        /// </summary>
        private SpaceSatus spaceSatus = SpaceSatus.land;
        #endregion

        #region 公开属性
        /// <summary>
        /// 当前状态等级
        /// </summary>
        public int NowLeve
        {
            get => nowLeve;set=> nowLeve = value;
        }
        /// <summary>
        /// 当前行为id
        /// </summary>
        public BHIDTable BHId
        {
            get => BHid;
        }
        /// <summary>
        /// 默认行为状态id
        /// </summary>
        public BHIDTable DefaultBH
        {
            get => defaultBH; set=> defaultBH = value;
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 改变状态机状态
        /// </summary>
        /// <param name="stateBH">状态信息</param>
        public virtual void ChangeState(BHIDTable BHid)
        {
            if (bhs.Keys.Contains(BHid))//存在指定行为
            {
                StateBH stateBH = BHInfoTable.GetInfo(BHid);
                if (stateBH.enterLevel >= nowLeve)
                {
                    bhs[BHid].StopBehaviour();//取消当前行为状态机
                    nowLeve = stateBH.runninglevel;
                    bhs[BHid].StartBehaviour();//激活此行为状态机
                    this.BHid = BHid;
                }
            }
            else
            {
                Debug.Log("指定行为不存在!");
            }
        }
        /// <summary>
        /// 添加新的属性对象
        /// </summary>
        public override void AddAttribute(IAttribute attribute)
        {
            if (attribute == null) return;
            attributes.Add(attribute);
            BHBase bh = attribute as BHBase;
            if (bh != null)
            {
                bhs.Add(bh.BHID, bh);
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
                    bhs.Remove(bh.BHID);
                }
                attribute.Destroy();
            }
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 在行为结束时调用的事件
        /// </summary>
        protected void BHEndAction()
        {
            BHid = defaultBH;
            if(bhs.Keys.Contains(BHid))
            {
                ChangeState(BHid);
            }
        }
        #endregion

        #region Unity
        private void Update()
        {
            if(BHid != defaultBH)
            {
                ChangeState(defaultBH);
            }
        }
        #endregion
    }
}
