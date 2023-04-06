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
    public class BHJump : BHSenior
    {
        #region 组件
        /// <summary>
        /// 上判定器
        /// </summary>
        public TouchLand upToucher;
        public Rigidbody2D rigidBody;
        #endregion

        #region 属性
        /// <summary>
        /// 上判定器
        /// </summary>
        public TouchLand UpToucher
        {
            get => upToucher;
            set
            {
                upToucher= value;
                upToucher.TouchEvent += TouchLandAction;
            }
        }
        /// <summary>
        /// 跳跃力度
        /// </summary>
        public float force = 20;
        /// <summary>
        /// 最大跳跃次数
        /// </summary>
        public int MultiJump = 2;
        /// <summary>
        /// 当前跳跃次数
        /// </summary>
        public int jumptimes = 0;

        private static string[] breakers = new string[] { "移动" };
        #endregion

        #region 公开属性
        public override string[] Beakers => breakers;

        #endregion

        public BHJump(ExciteEntity _owner) : base(_owner)
        {
            attributeName = "跳跃";
        }
        #region 内部函数
        public override void Initialization()
        {
            rigidBody = owner.SelfRigidbody;
            owner.Spaces["地面"].EnterEvent += TouchLandEvent;
        }
        /// <summary>
        /// 触地清空跳跃计数
        /// </summary>
        private void TouchLandEvent()
        {
            jumptimes = 0;
        }
        public void TouchLandAction()//接触顶面时触发的事件
        {
            active = false;
        }
      
        #endregion

        #region Unity

        public override bool Prerequisite()
        {
            return true;
        }

        protected override void BehaviourStart()
        {
            active = true;
        }

        protected override bool BehaviourContent()
        {
            if (jumptimes < MultiJump)
            {
                jumptimes++;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, force);
            }
            return true;
        }

        protected override void BehaviourEnd()
        {
            
        }

        #endregion
    }
}
