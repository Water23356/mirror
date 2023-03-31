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
    public class BHMoveLandPlayer : BHBase
    {

        #region 静态属性
        /// <summary>
        /// 行为id
        /// </summary>
        public new readonly static BHIDTable BHid = BHIDTable.RunningPlayer;
        /// <summary>
        /// 进入等级
        /// </summary>
        public new readonly static int enterLevel = 1;
        /// <summary>
        /// 持续等级
        /// </summary>
        public new readonly static int runninglevel = 1;
        #endregion

        #region 组件

        private Rigidbody2D rigidBody;
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
        private string attributeName = "玩家地面移动状态";
        private ExciteEntity player;//玩家实体
        private Direction direction;//运动方向
        public bool touchleft = false;//左侧是否接触
        public bool touchright = false;//右侧是否接触
        public float speed;//移动速度
        public float maxSpeed;//最大移动速度
        #endregion

        #region 公开属性
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
            Running();
        }
        #endregion
    }
}
