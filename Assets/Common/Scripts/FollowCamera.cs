using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 玩家跟随镜头
    /// </summary>
    public class FollowCamera : MonoBehaviour
    {
        #region 属性
        /// <summary>
        /// 跟随对象
        /// </summary>
        public Rigidbody2D aim;
        public Rigidbody2D self;
        public bool Xlock = false;//X轴锁定
        public bool Ylock = false;//Y轴锁定
        public Vector2 limitMin;//限制（最小）
        public Vector2 limitMax;//限制（最大）
        public float speedMax = 50;//镜头最大移动速度
        public bool transition = true;//是否启用过渡
        public float height = 30;//镜头高度
        public float speed = 0;//当前镜头速度
        public bool lockMaxSpeed = true;//锁定最大镜头移动速度
        private float timer = 0;
        public float minFollow = 0.5f;//最小过渡距离
        #endregion

        #region 内部函数
        private void Follow()
        {
            if (transition)
            {
                if (aim.velocity.magnitude > 0)
                {
                    if (timer < 3.14f) { timer += Time.deltaTime; }
                    self.velocity = aim.velocity * Mathf.Sin(timer);
                }
                else
                {
                    if (self.velocity.magnitude > 0.1f)
                    {
                        self.velocity *= 0.5f;
                    }
                    else
                    {
                        self.velocity = Vector2.zero;
                        transform.position = aim.transform.position + Vector3.back * height;
                    }
                }
            }
            else
            {
                transform.position = aim.transform.position + Vector3.back * height;
            }
        }
        #endregion

        #region Unity
        private void Update()
        {
            if (aim != null)
            {
                Follow();
            }
        }
        #endregion
    }
}
