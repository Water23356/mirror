using Common;
using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Mod_Player
{
    public enum Direction { left,up,down,right}
    public class BHMove : BHSenior
    {

        #region 组件
        public Rigidbody2D rigidBody;
        /// <summary>
        /// 左判定器
        /// </summary>
        public TouchLand leftToucher;
        /// <summary>
        /// 右判定器
        /// </summary>
        public TouchLand rightToucher;
        #endregion

        #region 属性
        /// <summary>
        /// 左判定器
        /// </summary>
        public TouchLand LeftToucher
        {
            get => leftToucher;
            set
            {
                leftToucher = value;
                leftToucher.TouchEvent += TouchLeft;
                leftToucher.UntouchLandEvent += LeaveLeft;
            }
        }
        /// <summary>
        /// 右判定器
        /// </summary>
        public TouchLand RightToucher
        {
            get=> rightToucher;
            set
            {
                rightToucher = value;
                rightToucher.TouchEvent += TouchRight;
                rightToucher.UntouchLandEvent += LeaveRight;
            }
        }
        /// <summary>
        /// 运动方向
        /// </summary>
        public Direction direction;
        /// <summary>
        /// 左侧是否接触
        /// </summary>
        public bool touchleft = false;
        /// <summary>
        /// 右侧是否接触
        /// </summary>
        public bool touchright = false;
        /// <summary>
        /// 当前移动速度
        /// </summary>
        public float speed;
        /// <summary>
        /// 最大移动速度
        /// </summary>
        public float maxSpeed = 10;

        private string[] breakers = new string[] { "跳跃"};
        #endregion

        public BHMove(ExciteEntity _owner):base(_owner)
        {
            attributeName="移动";
        }

        #region 公开属性
        public override string[] Beakers { get => breakers; }
        #endregion

        #region 内部函数
        public override void Initialization()
        {
            Debug.Log("玩家移动组件初始化");
            rigidBody = owner.SelfRigidbody;
        }
        private void Running()
        {
            //Debug.Log("移动！！！！speed:"+ speed);
            switch (direction)
            {
                case Direction.left:
                    //Debug.Log("左接触：" + touchleft);
                    if (!touchleft)
                    {
                        rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);
                    }
                    break;
                case Direction.right:
                    //Debug.Log("右接触：" + touchright);
                    if (!touchright)
                    {
                        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
                    }
                    break;
            }
        }
        #endregion

        #region 事件委托
        private void TouchLeft()//左侧接触
        {
            //Debug.Log("左接触");
            touchleft = true;
        }
        private void LeaveLeft()//左侧未接触
        {
            touchleft = false;
        }
        private void TouchRight()//右侧接触
        {
            //Debug.Log("右接触");
            touchright = true;
        }
        private void LeaveRight()//右侧未接触
        {
            touchright = false;
        }
        #endregion

        #region 实现

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
            Running();
            return !active;
        }

        protected override void BehaviourEnd()
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

        #endregion
    }
}
