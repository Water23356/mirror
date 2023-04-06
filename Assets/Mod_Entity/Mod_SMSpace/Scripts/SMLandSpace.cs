

using UnityEngine;

namespace Mod_Entity.Mod_SMSpace
{
    /// <summary>
    /// 地面空间状态
    /// </summary>
    public class SMLandSpace : SMSpace
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

        public SMLandSpace()
        {
            attributeName = "地面";
        }

        #region 功能函数
        #endregion

        #region 内部函数
        private void Touch()
        {
            SpaceConform = true;
        }
        private void Leave()
        {
            SpaceConform = false;
        }
        #endregion

        protected override void EnvironCheck()
        {

        }

        #region Unity
        private void Awake()
        {
            touchLand.UntouchLandEvent+=Leave;
            touchLand.TouchEvent+=Touch;
        }

        public override void Initialization()
        {
        }
        #endregion
    }
}
