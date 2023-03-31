using Common;
using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Mod_Player
{
    public class BHJumpPlayer:BHBase
    {

        #region 静态属性
        /// <summary>
        /// 行为id
        /// </summary>
        public new readonly static BHIDTable BHid = BHIDTable.NormalPlayer;
        /// <summary>
        /// 进入等级
        /// </summary>
        public new readonly static int enterLevel = 2;
        /// <summary>
        /// 持续等级
        /// </summary>
        public new readonly static int runninglevel = 2;
        #endregion

        #region 组件
        /// <summary>
        /// 上判定器
        /// </summary>
        public TouchLand LandToucher;
        #endregion

        #region 属性

        private Rigidbody2D rigidBody;
        private string attributeName = "玩家跳跃上升状态";
        private ExciteEntity player;//玩家实体
        public float force;//跳跃力度
        /// <summary>
        /// 最大跳跃次数
        /// </summary>
        public int MultiJump = 2;
        /// <summary>
        /// 当前跳跃次数
        /// </summary>
        public int jumptimes = 0;
        #endregion

        #region 公开属性
        /// <summary>
        /// 跳跃力度
        /// </summary>
        public float Force
        {
            get => force;
            set => force = value;
        }
        public override string Name { get => attributeName; set => attributeName = value; }
        public override BHIDTable BHID { get => BHid; }
        public override ExciteEntity OwnerExcite
        {
            get => owner;
            set
            {
                owner = value;
                rigidBody = owner.GetComponent<Rigidbody2D>();
            }
        }
        #endregion

        #region 公开函数

        public override bool StartBehaviour()
        {
            base.StartBehaviour();
            StartState();
            return true;
        }
        public static new StateBH StatusInfo()
        {
            return new StateBH(BHid, enterLevel, runninglevel);
        }
        public override object GetStatus()
        {
            return null;
        }

        #endregion

        #region 内部函数
        private void StartState()//开始阶段
        {
            if (jumptimes < MultiJump)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, force);
                if() jumptimes++;
            }
        }
        private void TouchLandAction()//接触顶面时触发的事件
        {
            player.ChangeState(BHIDTable.AirPlayer);//跳跃撞到墙壁后结束状态
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
            base.Update();
            if (rigidBody.velocity.y <= 0)//判断y轴速度来确定是否结束状态
            {
                enabled = false;
            }
        }
        #endregion
    }
}
