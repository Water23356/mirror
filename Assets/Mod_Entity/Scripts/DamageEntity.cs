using System.Collections.Generic;
using UnityEngine;
using Tools;
using Mod_Attribute;

namespace Mod_Entity
{
    /// <summary>
    /// 对象计时器
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// 所属对象
        /// </summary>
        public object owner;
        /// <summary>
        /// 计时器
        /// </summary>
        public float timer;
        /// <summary>
        /// 附加计数器
        /// </summary>
        public int times;
    }
    /// <summary>
    /// 伤害体脚本
    /// </summary>
    public class DamageEntity : MonoBehaviour
    {
        #region 关联组件
        /// <summary>
        /// 所挂载物体身上的碰撞器
        /// </summary>
        public Collider2D Collider;
        #endregion

        #region 属性
        private object owner;
        private Dictionary<object, Timer> timers = new Dictionary<object, Timer>();//计时器
        private List<string> damageTag;//伤害标签

        private float damage;//伤害值
        private int damageTimes = 1;//对同一对象的最大允许伤害次数
        private float cd = 0.05f;//多段伤害触发间隔
        private float power = 0;//击退力量

        private bool effective = true;//伤害判定是否有效
        private bool autoDead = false;//自动死亡
        private bool hitsLimit = false;//是否限定造成伤害的总次数
        private int hits = 0;//当前伤害判定生效次数
        private int maxHits = 1;//最大伤害生效次数
        private float maxLiveTime = 1;//存活时间
        
        #endregion

        #region 公开属性
        /// <summary>
        /// 伤害体所属者
        /// </summary>
        public object Owner
        {
            get=>owner; set => owner = value;
        }
        /// <summary>
        /// 伤害值
        /// </summary>
        public float Damage
        {
            get => damage; set => damage = value;
        }
        /// <summary>
        /// 伤害次数
        /// </summary>
        public int DamageTimes
        {
            get => damageTimes; set => damageTimes = value;
        }
        /// <summary>
        /// 伤害间隔
        /// </summary>
        public float CD
        {
            get => cd; set => cd = value;
        }
        /// <summary>
        /// 击退力度
        /// </summary>
        public float Power
        {
            get => power;set => power = value;
        }
        /// <summary>
        /// 伤害体存活时间
        /// </summary>
        public float LiveTime
        {
            get => maxLiveTime; set => maxLiveTime = value;
        }

        /// <summary>
        /// 判定是否启用
        /// </summary>
        public bool Effective
        {
            get => effective; set => effective = value;
        }
        /// <summary>
        /// 是否在寿命尽头自动销毁此物体
        /// </summary>
        public bool AutoDead
        { 
            get => autoDead; set => autoDead = value;
        }
        /// <summary>
        /// 是否限制最大伤害次数，达到最大伤害次数时自动销毁
        /// </summary>
        public bool HitsLimit
        {
            get => hitsLimit;set => hitsLimit = value;
        }
        /// <summary>
        /// 最大伤害次数
        /// </summary>
        public int MaxHits { get => maxHits; set => maxHits = value; }
        /// <summary>
        /// 最大存活时间
        /// </summary>
        public float MaxLiveTime { get => maxLiveTime;set=> maxLiveTime = value; }
        #endregion

        #region 公开函数
        /// <summary>
        /// 添加伤害标签
        /// </summary>
        /// <param name="tag">伤害标签</param>
        public void addDamageTag(string tag)
        {
            damageTag.Add(tag);
        }
        /// <summary>
        /// 清除伤害标签
        /// </summary>
        public void clearDamageTag()
        {
            damageTag.Clear();
        }
        /// <summary>
        /// 获取此伤害体的标签组
        /// </summary>
        /// <returns></returns>
        public List<string> DamageTag()
        {
            return damageTag;
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 对目标对象做伤害判定
        /// </summary>
        /// <param name="aim">目标对象</param>
        private void DamageJudge(object aim)
        {
            Entity entity = aim as Entity;
            if(entity != null)
            {
                if(Tool.isInside(entity.DamageTag,damageTag))//判定是否在判定标签内
                {
                    if(isInside(entity))//判断是否已拥有计时器
                    {
                        if(Damagable(entity))//判断伤害cd是否冷却
                        {
                            ATHealth health = entity.GetAttribute<ATHealth>();
                            health.ChangeHealth(damage, Owner);
                            RestartCD(health);//重置计时器
                        }
                    }
                    else
                    {
                        ATHealth health = entity.GetAttribute<ATHealth>();
                        health.ChangeHealth(damage, Owner);
                        AddCdLabel(health);//添加计时器
                    }
                    timers[entity].times++;
                    hits++;
                    if(hitsLimit)//对总伤害次数有限制
                    {
                        if(hits >= maxHits)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 给指定对象 创建计时器
        /// </summary>
        /// <param name="aim">需要创建计时器的对象</param>
        private void AddCdLabel(object aim)
        {
            timers.Add(aim, new Timer()
            {
                owner = aim,
                timer = cd
            });
        }
        /// <summary>
        /// 判断指定判定对象 是否在计时器列表内
        /// </summary>
        /// <param name="aim">需要判定的对象</param>
        /// <returns></returns>
        private bool isInside(object aim)
        {
            foreach(object obj in timers.Keys)
            {
                if(obj == aim) { return true; }
            }
            return false;
        }
        /// <summary>
        /// 判断指定判定对象 是否可进行伤害判定（主要判定cd是否冷却，是否达到最大伤害次数）
        /// </summary>
        /// <param name="aim">需要判定的对象</param>
        /// <returns></returns>
        private bool Damagable(object aim)
        {
            if (timers[aim].timer <= 0 && timers[aim].times<damageTimes) { return true; }
            return false;
        }
        /// <summary>
        /// 使指定判定对象 的计时器重置
        /// </summary>
        /// <param name="aim"></param>
        private void RestartCD(object aim)
        {
            timers[aim].timer = cd;
        }
        #endregion

        #region 碰撞检测
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!effective) return;

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!effective) return;
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!effective) return;
            DamageJudge(collision.GetComponent<Entity>());
        }
        #endregion

        #region Unity
        private void Start()
        {
            
        }
        private void Update()
        {
            if (!effective) return;
            for(int i=0;i<timers.Count;i++)
            {
                timers[i].timer -= Time.deltaTime;
            }

            if(autoDead)
            {
                maxLiveTime -= Time.deltaTime;
                if (maxLiveTime <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        #endregion
    }
}
