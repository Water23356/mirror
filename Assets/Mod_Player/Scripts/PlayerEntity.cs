
using Mod_Entity;
using Mod_Entity.Mod_SMSpace;

namespace Mod_Player
{
    /// <summary>
    /// 玩家实体
    /// </summary>
    public class PlayerEntity:ExciteEntity
    {

        #region 组件
        public TouchLand leftTouch;
        public TouchLand upTouch;
        public TouchLand downTouch;
        public TouchLand rightTouch;
        #endregion

        #region 公开属性
        public SMAirSpace airSpace;
        public SMLandSpace landSpace;
        public InputInterpreter inputInterpreter;
        #endregion


        #region Unity
        protected override void Awake()
        {
            base.Awake();
            AddAttribute(airSpace);

            AddAttribute(landSpace);
            BHMove move = new BHMove(this);
            BHJump jump = new BHJump(this);
            AddAttribute(new BHNormal(this));
            AddAttribute(new BHAttack(this));
            AddAttribute(move);
            AddAttribute(jump);
            move.LeftToucher = leftTouch;
            move.RightToucher = rightTouch;
            jump.UpToucher = upTouch;
            defBh = bhs["通常"];
            inputInterpreter.enabled = true;
        }
        #endregion
    }
}
