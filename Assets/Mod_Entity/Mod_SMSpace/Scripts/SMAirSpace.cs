using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod_Entity.Mod_SMSpace
{
    /// <summary>
    /// 空中空间状态
    /// </summary>
    public class SMAirSpace : SMSpace
    {
        #region 属性
        protected new readonly string attributeName = "空中空间状态";//属性名称
        public override string Name { get => attributeName;}
        /// <summary>
        /// 接触器
        /// </summary>
        public TouchLand touchLand;
        private bool touch = false;//是否接触地面
        #endregion

        #region 功能函数
        public override void Enter()
        {
            base.Enter();
            touch = false;
        }
        #endregion

        #region 内部函数
        private void Touch()
        {
            touch = true;
        }
        #endregion

        protected override void EnvironCheck()
        {
            if (touch)//接触地面
            {
                owner.ChangeStateSP("地面空间状态");
            }
        }

        #region Unity
        private void Awake()
        {
            touchLand.AddTouchAction(Touch);
        }
        #endregion
    }
}
