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
    /// 带有虚拟预判定的状态机
    /// </summary>
    public abstract class SMLand : MonoBehaviour,IStateMachine
    {
        #region 属性
        /// <summary>
        /// 着地判定器
        /// </summary>
        public TouchLand LandToucher;
        #endregion

        #region 公开属性
        public abstract string Name { get; set; }
        public abstract Entity Owner { get; set; }
        #endregion

        #region 功能函数
        /// <summary>
        /// 接触地面时调用的事件
        /// </summary>
        public abstract void TouchLand();
        /// <summary>
        /// 在离开地面时调用的事件
        /// </summary>
        public abstract void LeaveLand();
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

        #region Unity
        public void Start()
        {
            if(LandToucher!=null)
            {
                Debug.Log("委托已添加");
                LandToucher.AddTouchAction(TouchLand);
                LandToucher.AddUntouchAction(LeaveLand);
            }
        }

        #endregion
    }
}
