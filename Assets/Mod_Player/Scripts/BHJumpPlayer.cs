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
    public class BHJumpPlayer : BHBase
    {
        #region 组件
        /// <summary>
        /// 上判定器
        /// </summary>
        public TouchLand LandToucher;
        public Rigidbody2D rigidBody;
        #endregion

        #region 属性
        private new readonly string attributeName = "玩家跳跃";
        public float force;//跳跃力度
        /// <summary>
        /// 最大跳跃次数
        /// </summary>
        public int MultiJump = 2;
        /// <summary>
        /// 当前跳跃次数
        /// </summary>
        public int jumptimes = 0;

        /// <summary>
        /// 前摇时间
        /// </summary>
        private new float timeFront = 1;
        /// <summary>
        /// 行为持续时间（小于0表示无限定）
        /// </summary>
        private new float timeRunning = 0;
        /// <summary>
        /// 后摇时间
        /// </summary>
        private new float timeEnd = 0;

        private static string[] breakFront = new string[]
        {

        };
        private static string[] breakRunning = new string[]
        {
        };
        private static string[] breakEnd = new string[]
        {

        };
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
        public override string[] StartFront { get => null; }

        public override string[] StartSpace { get => null; }

        protected override string[] BreakFront { get => breakFront; }

        protected override string[] BreakRunning { get => breakRunning; }

        protected override string[] BreakEnd { get => breakEnd; }

        public override string Name { get => attributeName; }
        #endregion

        #region 内部函数
        private void TouchLandAction()//接触顶面时触发的事件
        {
            StopBH();
        }
        protected override void StartContent()
        {
            if (jumptimes < MultiJump)
            {
                if (owner.NowSpaceStatus.Name == "空中空间状态") { jumptimes++; }
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, force);
            }
            StopBH();
        }

        protected override void LoopContent()
        {

        }

        protected override void StopContent()
        {

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
        #endregion
    }
}
