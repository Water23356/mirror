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
        /// <summary>
        /// 接触器
        /// </summary>
        public TouchLand touchLand;
        public override bool SpaceConform
        {
            get => spaceConform;
            protected set
            {
                spaceConform = value;
                if (spaceConform)
                {
                    Enter();
                }
                else
                {
                    Exit();
                }
            }
        }
        #endregion

        public SMAirSpace()
        {
            attributeName = "空中";
        }

        #region 内部函数
        private void Touch()
        {
            SpaceConform = false;
        }
        private void Leave()
        {
            SpaceConform = true;
        }
        #endregion

        protected override void EnvironCheck()
        {
            
        }

        #region Unity
        private void Awake()
        {
            touchLand.TouchEvent+=Touch;
            touchLand.UntouchLandEvent+=Leave;
        }

        public override void Initialization()
        {
        }
        #endregion
    }
}
