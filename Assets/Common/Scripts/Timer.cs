using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 一个计时器，在一定时间后自动触发指定函数
    /// </summary>
    public class Timer:MonoBehaviour
    {
        #region 属性
        public float timeLimit;//最大计时时间
        private float time;//当前计时时间
        public float speed;//时间计时倍率
        private Action callback;//需要触发的事件
        public bool collect = false;//是否计时
        public bool autoDead = false;//是否在计时完成后自动销毁
        #endregion

        #region 公开属性
        /// <summary>
        /// 最大计时时间，改变会重置计时
        /// </summary>
        public float TimeLimit
        {
            get =>timeLimit;
            set
            {
                timeLimit = value;
                time = timeLimit;
            }
        }
        /// <summary>
        /// 当前计时时间，此值会从最大计时间开始减少，减少到0及以下时触发预设事件
        /// </summary>
        public float NowTime
        {
            get => time;set=> time = value;
        }
        /// <summary>
        /// 计时速率，此值>1计时器变快（实际时间流逝更短）
        /// </summary>
        public float TimeSpeed
        {
            get => speed;set=> speed = value;
        }
        /// <summary>
        /// 计时器是否生效，为true时才会计时
        /// </summary>
        public bool Collect { get=> collect; set => collect = value; }
        /// <summary>
        /// 是否在计时完成后自动销毁
        /// </summary>
        public bool AutoDead { get=>autoDead; set => autoDead = value; }
        #endregion

        #region 功能函数
        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="action">事件委托</param>
        public void Add(Action action)
        {
            callback += action;
        }
        /// <summary>
        /// 清空事件委托
        /// </summary>
        public void Clear()
        {
            callback= null;
        }
        /// <summary>
        /// 重置计时器
        /// </summary>
        public void Restart()
        {
            time = timeLimit;
        }
        #endregion

        #region Unity
        private void Start()
        {
            time = timeLimit;
        }
        private void Update()
        {
            if(collect)
            {
                time -= Time.deltaTime * speed;
                if(time<=0)
                {
                    if(callback!=null)
                    {
                        callback();
                    }
                    if(autoDead)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        #endregion
    }
}
