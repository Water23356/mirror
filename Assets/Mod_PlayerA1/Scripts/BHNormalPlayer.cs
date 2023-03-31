using Mod_Entity;
using System;
using UnityEngine;
using Common;

namespace Mod_Player
{
    public class BHNormalPlayer : BHBase
    {
        #region 静态属性
        /// <summary>
        /// 行为id
        /// </summary>
        public new readonly static BHIDTable BHid = BHIDTable.NormalPlayer;
        /// <summary>
        /// 进入等级
        /// </summary>
        public new readonly static int enterLevel = 1;
        /// <summary>
        /// 持续等级
        /// </summary>
        public new readonly static int runninglevel = 0;
        #endregion

        #region 组件
        /// <summary>
        /// 着地判定器
        /// </summary>
        public TouchLand LandToucher;
        #endregion

        #region 属性
        private string attributeName = "玩家一般状态";
        private ExciteEntity player;//玩家实体
        #endregion

        #region 公开属性
        public override string Name { get => attributeName; set => attributeName = value; }
        public override BHIDTable BHID { get => BHid; }
        #endregion

        #region 功能函数
        public static new StateBH StatusInfo()
        {
            return new StateBH(BHid, enterLevel, runninglevel);
        }
        public void LeaveLandAction()//离开地面时触发的事件
        {
            player.ChangeState(BHIDTable.AirPlayer);
        }

        public override object GetStatus()
        {
            return null;
        }

        #endregion

        #region Unity
        private void Awake()
        {
            if (LandToucher != null)
            {
                Debug.Log("委托已添加");
                LandToucher.AddUntouchAction(LeaveLandAction);
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void Update()
        {

        }
        #endregion
    }
}
