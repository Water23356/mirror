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
    public class BHMovePlayer : BHBase
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
        private new readonly string attributeName = "玩家移动";
        private Direction direction;//运动方向
        public bool touchleft = false;//左侧是否接触
        public bool touchright = false;//右侧是否接触
        public float speed;//移动速度
        public float maxSpeed;//最大移动速度

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
        /// 运动方向
        /// </summary>
        public Direction Direction { get => direction; set => direction = value; }
        /// <summary>
        /// 移动速度
        /// </summary>
        public float Speed { get => speed; set => speed = value; }
        /// <summary>
        /// 最大移动速度
        /// </summary>
        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }

        public override string[] StartFront { get => null; }

        public override string[] StartSpace { get => null; }

        protected override string[] BreakFront { get => breakFront; }

        protected override string[] BreakRunning { get => breakRunning; }

        protected override string[] BreakEnd { get => breakEnd; }
        public override string Name { get => attributeName; }
        #endregion

        #region 内部函数
        private void Running()
        {
            Debug.Log("移动！！！！");
            switch (direction)
            {
                case Direction.left:
                    Debug.Log("左接触：" + touchleft);
                    if (!touchleft)
                    {
                        rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);
                    }
                    break;
                case Direction.right:
                    Debug.Log("右接触：" + touchright);
                    if (!touchright)
                    {
                        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
                    }
                    break;
            }
        }
        protected override void StartContent()
        {
        }

        protected override void LoopContent()
        {
            Running();
        }

        protected override void StopContent()
        {
        }
        #endregion

        #region 事件委托
        private void TouchLeft()//左侧接触
        {
            Debug.Log("左接触");
            touchleft = true;
        }
        private void LeaveLeft()//左侧未接触
        {
            touchleft = false;
        }
        private void TouchRight()//右侧接触
        {
            Debug.Log("右接触");
            touchright = true;
        }
        private void LeaveRight()//右侧未接触
        {
            touchright = false;
        }
        #endregion

        #region Unity
        private void Awake()
        {
            if (leftToucher != null)
            {
                leftToucher.AddTouchAction(TouchLeft);
                leftToucher.AddUntouchAction(LeaveLeft);
            }
            if (rightToucher != null)
            {
                rightToucher.AddTouchAction(TouchRight);
                rightToucher.AddUntouchAction(LeaveRight);
            }
            timeFront = 0;
            timeRunning = -1;
            timeEnd = 0;
        }
        #endregion
    }
}
