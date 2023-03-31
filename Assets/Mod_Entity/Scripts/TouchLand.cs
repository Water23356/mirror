using Mod_Entity;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Entity
{
    /// <summary>
    /// 虚拟碰撞预测
    /// </summary>
    public class TouchLand : MonoBehaviour
    {
        #region 事件
        /// <summary>
        /// 接触事件委托
        /// </summary>
        private Action TouchAction;
        /// <summary>
        /// 未接触 事件委托
        /// </summary>
        private Action UntouchAction;
        #endregion

        #region 属性
        private bool runing = true;
        private List<string> touchTags = new List<string>();//检测标签
        private List<object> lands = new List<object>();
        /// <summary>
        /// 需检测的物体标签
        /// </summary>
        /// <returns></returns>
        public List<string> TouchTags
        {
            get => touchTags;
        }
        #endregion

        #region 功能函数

        public void AddTouchAction(Action action)
        {
            TouchAction += action;
        }
        public void ClearTouchAction() 
        {
            TouchAction = null;
        }
        public void AddUntouchAction(Action action)
        {
            UntouchAction += action; 
        }
        public void ClearUntouchAction()
        {
            UntouchAction = null;
        }

        /// <summary>
        /// 启用着地检测
        /// </summary>
        public void Usecheck()
        {
            runing = true;
        }
        /// <summary>
        /// 关闭着地检测
        /// </summary>
        public void Unusecheck()
        {
            runing = false;
        }
        #endregion

        #region 触发监听
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (runing && TouchAction != null)
            {
                if (touchTags.Contains(collision.tag))//检测物体为障碍物
                {
                    if (!lands.Contains(collision.gameObject))
                    {
                        lands.Add(collision.gameObject);
                    }
                    TouchAction();
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (runing && UntouchAction != null)
            {
                if (touchTags.Contains(collision.tag))//检测物体为障碍物
                {
                    if (lands.Contains(collision.gameObject))
                    {
                        lands.Remove(collision.gameObject);
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (runing && TouchAction != null)
            {
                if (touchTags.Contains(collision.tag))//检测物体为障碍物
                {
                    if (!lands.Contains(collision.gameObject))
                    {
                        lands.Add(collision.gameObject);
                    }
                    TouchAction();
                }
            }
        }
        #endregion

        #region Unity
        private void Start()
        {
            touchTags.Add("Barrier");
        }
        private void Update()
        {
            if (lands.Count == 0)
            {
                UntouchAction();
            }
        }
        #endregion
    }
}
