
using Mod_Entity;
using Mod_Entity.Mod_SMSpace;

namespace Mod_Player
{
    public class PlayerEntity:ExciteEntity
    {
        #region 公开属性
        public BHNormalPlayer normalPlayer;
        public BHMovePlayer movePlayer;
        public BHJumpPlayer jumpPlayer;
        public SMAirSpace airSpace;
        public SMLandSpace landSpace;
        public InputInterpreter inputInterpreter;
        #endregion


        #region Unity
        private void Awake()
        {
            AddAttribute(normalPlayer);
            AddAttribute(movePlayer);
            AddAttribute(jumpPlayer);
            AddAttribute(airSpace);
            AddAttribute(landSpace);
            defSp = spaces["地面空间状态"];
            defBh = bhs["玩家通常"];

            inputInterpreter.enabled = true;
        }
        #endregion
    }
}
