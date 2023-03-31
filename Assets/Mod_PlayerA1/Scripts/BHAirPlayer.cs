using Common;
using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Player
{
    public class BHAirPlayer:BHBase
    {
        #region 静态属性
        /// <summary>
        /// 行为id
        /// </summary>
        public new readonly static BHIDTable BHid = BHIDTable.AirPlayer;
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
        private BHJumpPlayer jumpPlayer;//玩家跳跃组件
        /// <summary>
        /// 着地判定器
        /// </summary>
        public TouchLand LandToucher;
        #endregion

        #region 属性
        private string attributeName = "玩家空中一般状态";
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
        public void TouchLandAction()//接触地面时触发的事件
        {
            player.ChangeState(BHIDTable.NormalPlayer);
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
                LandToucher.AddTouchAction(TouchLandAction);
            }
            jumpPlayer = player.GetAttribute<BHJumpPlayer>();
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
