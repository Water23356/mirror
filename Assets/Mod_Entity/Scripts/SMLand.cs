using Mod_Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Entity
{
    /// <summary>
    /// 带有落地判定的状态机
    /// </summary>
    public abstract class SMLand : MonoBehaviour,IStateMachine
    {
        #region 属性
        /// <summary>
        /// 着地判定器
        /// </summary>
        public JumpLand LandToucher;
        /// <summary>
        /// 是否处于空中
        /// </summary>
        public bool isAir = false;
        #endregion

        #region 公开属性
        public abstract string Name { get; set; }
        public abstract Entity Owner { get; set; }
        #endregion

        #region 功能函数
        /// <summary>
        /// 触地事件（在实体接触到地面时触发）
        /// </summary>
        public abstract void StopDrop();
        public virtual void Destroy()
        {
            Destroy(this);
        }
        public virtual object GetStatus()
        {
            return null;
        }

        public virtual bool StartMachine()
        {
            return false;
        }

        public virtual bool StopMachine()
        {
            return false;
        }
        #endregion
    }
}
