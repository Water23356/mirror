

namespace Mod_Entity.Mod_SMSpace
{
    /// <summary>
    /// 地面空间状态
    /// </summary>
    public class SMLandSpace : SMSpace
    {
        #region 属性
        protected new readonly string attributeName = "地面空间状态";//属性名称
        public override string Name { get => attributeName;}
        /// <summary>
        /// 接触器
        /// </summary>
        public TouchLand touchLand;
        private bool leave = false;//是否离开地面
        #endregion

        #region 功能函数
        public override void Enter()
        {
            base.Enter();
            leave = false;
        }
        #endregion

        #region 内部函数
        private void Leave()
        {
            leave = true;
        }
        #endregion

        protected override void EnvironCheck()
        {
            if(leave)//离开地面
            {
                owner.ChangeStateSP("空中空间状态");
            }
        }

        #region Unity
        private void Awake()
        {
            touchLand.AddUntouchAction(Leave);
        }
        #endregion
    }
}
