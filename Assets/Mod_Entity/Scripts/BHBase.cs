using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Entity
{
    /// <summary>
    /// 行为模块抽象基类（激活时自动激活行为，行为结束时自动取消激活）
    /// </summary>
    public abstract class BHBase : MonoBehaviour,IBehaviour
    {
        #region 属性
        /// <summary>
        /// 行为id
        /// </summary>
        public readonly static BHIDTable BHid;
        /// <summary>
        /// 进入等级
        /// </summary>
        public readonly static int enterLevel;
        /// <summary>
        /// 持续等级
        /// </summary>
        public readonly static int runninglevel;
        /// <summary>
        /// 行为结束时调用
        /// </summary>
        protected Action EndAction;
        /// <summary>
        /// 行为开始时调用
        /// </summary>
        protected Action StartAction;
        /// <summary>
        /// 所属实体
        /// </summary>
        protected ExciteEntity owner;

        public abstract string Name { get; set; }
        /// <summary>
        /// 所属实体
        /// </summary>
        public virtual ExciteEntity OwnerExcite { get=> owner; set=> owner=value; }
        /// <summary>
        /// 所属实体(必须是ExciteEntity才能set)
        /// </summary>
        public virtual Entity Owner 
        { 
            get => owner;
            set 
            {
                if(value is ExciteEntity)
                {
                    OwnerExcite = value as ExciteEntity;
                }
            } 
        }
        /// <summary>
        /// 获取行为id
        /// </summary>
        public virtual BHIDTable BHID { get => BHid; }
        #endregion

        #region 功能
        /// <summary>
        /// 添加开始事件
        /// </summary>
        /// <param name="action"></param>
        public void AddStartAction(Action action)
        {
            StartAction += action;
        }
        /// <summary>
        /// 清空开始事件
        /// </summary>
        public void ClearStartAction()
        {
            StartAction = null;
        }
        /// <summary>
        /// 添加结束事件
        /// </summary>
        /// <param name="action"></param>
        public void AddEndAction(Action action)
        {
            EndAction += action;
        }
        /// <summary>
        /// 清空结束事件
        /// </summary>
        public void ClearEndAction()
        {
            EndAction = null;
        }
        /// <summary>
        /// 获取此行为的状态信息
        /// </summary>
        /// <returns></returns>
        public static StateBH StatusInfo()
        {
            return new StateBH(BHid,enterLevel,runninglevel);
        }
        public virtual void Destroy()
        {
            enabled= false;
            Destroy(this);
        }
        public abstract object GetStatus();
        public virtual bool StartBehaviour()
        {
            enabled = true;
            return true;
        }
        public virtual bool StopBehaviour()
        {
            enabled = false;
            return true;
        }
        #endregion

        #region Unity
        protected virtual void OnEnable() 
        {
            if(StartAction != null)
            {
                StartAction();
            }
        }
        protected virtual void OnDisable()
        {
            if (EndAction != null)
            {
                EndAction();
            }
        }
        protected virtual void Update()
        {
            
        }
        #endregion
    }
}
