using Mod_Player;

namespace Common
{
    /// <summary>
    /// 空间状态
    /// </summary>
    public enum SpaceSatus
    {
        /// <summary>
        /// 陆地
        /// </summary>
        land,
        /// <summary>
        /// 空中
        /// </summary>
        air,
        /// <summary>
        /// 水中
        /// </summary>
        water
    }
    /// <summary>
    /// 方向
    /// </summary>
    public enum Direction
    {
        left,
        right,
        up,
        down
    }
    /// <summary>
    /// 行为状态信息结构体
    /// </summary>
    public struct StateBH
    {
        /// <summary>
        /// 行为id
        /// </summary>
        public BHIDTable BHid;
        /// <summary>
        /// 进入等级
        /// </summary>
        public int enterLevel;
        /// <summary>
        /// 持续等级
        /// </summary>
        public int runninglevel;
        /// <summary>
        /// 行为状态信息结构体
        /// </summary>
        /// <param name="_enterLevel">进入等级</param>
        /// <param name="_runninglevel">持续等级</param>
        public StateBH(BHIDTable _BHid, int _enterLevel, int _runninglevel)
        {
            BHid = _BHid;
            enterLevel = _enterLevel;
            runninglevel = _runninglevel;
        }
    }
    /// <summary>
    /// 行为id表
    /// </summary>
    public enum BHIDTable
    {
        /// <summary>
        /// 玩家一般状态
        /// </summary>
        NormalPlayer,
        /// <summary>
        /// 玩家空中状态
        /// </summary>
        AirPlayer,
        /// <summary>
        /// 玩家跑动状态
        /// </summary>
        RunningPlayer,
        /// <summary>
        /// 玩家跳跃上升阶段
        /// </summary>
        JumpingPlayer,
    }
    /// <summary>
    /// 行为状态信息表
    /// </summary>
    public static class BHInfoTable
    {
        /// <summary>
        /// 获取指定行为状态的具体信息
        /// </summary>
        /// <param name="id">行为状态id</param>
        /// <returns></returns>
        public static StateBH GetInfo(BHIDTable id)
        {
            switch(id)
            {
                case BHIDTable.NormalPlayer:
                    return BHNormalPlayer.StatusInfo();
                case BHIDTable.AirPlayer:
                    return BHAirPlayer.StatusInfo();
                case BHIDTable.RunningPlayer:
                    return new StateBH();
                case BHIDTable.JumpingPlayer:
                    return BHJumpPlayer.StatusInfo();
            }
            return new StateBH();
        }
    }
}
