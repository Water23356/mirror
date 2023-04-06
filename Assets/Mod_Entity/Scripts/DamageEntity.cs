#define debug
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Mod_Attribute;
using Unity.Burst.Intrinsics;
using System.Linq;

namespace Mod_Entity
{
    /// <summary>
    /// 伤害事件信息
    /// </summary>
    public struct DamageEventInfo
    {
        /// <summary>
        /// 伤害值
        /// </summary>
        public float damage;
        /// <summary>
        /// 伤害源对象
        /// </summary>
        public object hiter;
        /// <summary>
        /// 击退效果向量
        /// </summary>
        public Vector2 repel;
    }
    
    /// <summary>
    /// 伤害体脚本
    /// </summary>
    public class DamageEntity : MonoBehaviour
    {
        /// <summary>
        /// 击退模式
        /// </summary>
        public enum RepelMode
        {
            /// <summary>
            /// 无击退的
            /// </summary>
            Off,
            /// <summary>
            /// 自动的（根据伤害体位置与被判定实体的位置自动确定方向）
            /// </summary>
            Auto,
            /// <summary>
            /// 自定义
            /// </summary>
            Custom,
            /// <summary>
            /// 锁定X轴的（击退方向限制在X轴）
            /// </summary>
            LimX,
            /// <summary>
            /// 锁定Y轴的（击退方向限制在Y轴）
            /// </summary>
            LimY,
        }
        /// <summary>
        /// 对象计时器
        /// </summary>
        private class Timer
        {
            /// <summary>
            /// 所属对象
            /// </summary>
            public Entity owner;
            /// <summary>
            /// 计时器
            /// </summary>
            public float timer;
            /// <summary>
            /// 附加计数器
            /// </summary>
            public int times;
        }

        #region 关联组件
        /// <summary>
        /// 所挂载物体身上的碰撞器
        /// </summary>
        private Collider2D Collider;
        #endregion

        #region 属性
        /// <summary>
        /// 关联此伤害体的对象（伤害源对象）
        /// </summary>
        private object owner;
        private Dictionary<Entity, Timer> timers = new Dictionary<Entity, Timer>();//计时器
        private List<string> damageTag = new List<string>();//伤害标签
        private List<Entity> inDamageItems = new List<Entity>();//处于伤害范围内的实体对象

        private float damage;//伤害值
        private int damageTimes = 1;//对同一对象的最大允许伤害次数
        private float cd = 0.05f;//多段伤害触发间隔
        private float repPower = 0;//击退参考效果
        private RepelMode repMode = RepelMode.Auto;//击退方向
        private Vector2 customDir;//自定义击退方向

        private bool effective = true;//伤害判定是否有效
        private bool autoDead = false;//自动死亡
        private bool hitsLimit = false;//是否限定造成伤害的总次数
        private int hits = 0;//当前伤害判定生效次数
        private int maxHits = 1;//最大伤害生效次数
        private float maxLiveTime = 1;//存活时间

        #endregion

        #region 公开属性
        /// <summary>
        /// 自定义击退方向
        /// </summary>
        public Vector2 CustomDir
        {
            get => customDir;set => customDir = value;
        }
        /// <summary>
        /// 伤害体所属者
        /// </summary>
        public object Owner
        {
            get => owner; set => owner = value;
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
        public float RepPower
        {
            get => repPower; set => repPower = value;
        }
        /// <summary>
        /// 击退方向模式
        /// </summary>
        public RepelMode RepMode
        {
            get => repMode; set => repMode = value;
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
            get => hitsLimit; set => hitsLimit = value;
        }
        /// <summary>
        /// 最大伤害次数
        /// </summary>
        public int MaxHits { get => maxHits; set => maxHits = value; }
        /// <summary>
        /// 最大存活时间
        /// </summary>
        public float MaxLiveTime { get => maxLiveTime; set => maxLiveTime = value; }
        #endregion

        #region 公开函数
        /// <summary>
        /// 获取此伤害体对指定实体的模拟击退向量
        /// </summary>
        /// <param name="aim">目标实体</param>
        /// <returns></returns>
        public Vector2 GetDamageDirction(Entity aim)
        {
            switch(repMode)
            {
                case RepelMode.Off:
                    return Vector2.zero;
                case RepelMode.Auto:
                    return (aim.transform.position - transform.position).normalized * repPower;
                case RepelMode.Custom:
                    return CustomDir * repPower;
                case RepelMode.LimX:
                    if (aim.transform.position.x < transform.position.x) { return Vector2.left * repPower; }
                    return Vector2.right * repPower;
                case RepelMode.LimY:
                    if (aim.transform.position.y < transform.position.y) { return Vector2.up * repPower; }
                    return Vector2.down * repPower;
            }
            return Vector2.zero;
        }
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
        private void DamageJudge(Entity aim)
        {
            //Debug.Log("进行伤害判断");
            if (damageTag.Contains(aim.DamageTag))
            {
                if (isInside(aim))//判断是否已拥有计时器
                {
                    //Debug.Log("已拥有计时器");

                    if (Damagable(aim))//判断伤害cd是否冷却
                    {
                        Debug.Log("造成伤害");
                        aim.GetDamage(new DamageEventInfo
                        {
                            damage = damage,
                            repel = GetDamageDirction(aim),
                            hiter = owner
                        });
                        ATHealth health = aim.GetAttribute<ATHealth>();
                        health.ChangeHealth(damage, Owner);
                        RestartCD(aim);//重置计时器
                    }
                }
                else
                {
                    //Debug.Log("没有计时器");
                    ATHealth health = aim.GetAttribute<ATHealth>();
                    health.ChangeHealth(damage, Owner);
                    AddCdLabel(aim);//添加计时器
                }
                timers[aim].times++;
                hits++;
                if (hitsLimit)//对总伤害次数有限制
                {
                    if (hits >= maxHits)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        /// <summary>
        /// 给指定对象 创建计时器
        /// </summary>
        /// <param name="aim">需要创建计时器的对象</param>
        private void AddCdLabel(Entity aim)
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
        private bool isInside(Entity aim)
        {
            return timers.Keys.Contains(aim);
        }
        /// <summary>
        /// 判断指定判定对象 是否可进行伤害判定（主要判定cd是否冷却，是否达到最大伤害次数）
        /// </summary>
        /// <param name="aim">需要判定的对象</param>
        /// <returns></returns>
        private bool Damagable(Entity aim)
        {
            Debug.Log($"CDTime:{timers[aim].timer}");

            if (timers[aim].timer <= 0 && timers[aim].times < damageTimes) { return true; }
            return false;
        }
        /// <summary>
        /// 使指定判定对象 的计时器重置
        /// </summary>
        /// <param name="aim"></param>
        private void RestartCD(Entity aim)
        {
            timers[aim].timer = cd;
        }
        #endregion

        #region 
        //进入触发器
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!effective) return;
            Entity entity = collision.GetComponent<Entity>();
            if (entity != null)//如果是实体对象，则加入判定列表
            {
                if(damageTag.Contains(entity.DamageTag))
                {
                    inDamageItems.Add(entity);
                    DamageJudge(entity);
                }
            }
#if debug
            Debug.Log("进入");
#endif
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!effective) return;
            Entity entity = collision.GetComponent<Entity>();
            if (entity != null)//如果是实体对象，则移除判定列表
            {
                inDamageItems.Remove(entity);
            }
#if debug
            Debug.Log("离开");
#endif
        }
        private void OnTriggerStay2D(Collider2D collision)//弃用此方案，因为长时间在范围内不移动则不会触发此事件
        {
        }

        #endregion

        #region Unity
        private void Start()
        {
            Collider = GetComponent<Collider2D>();
            damageTag.Add("Test");
        }
        private void Update()
        {
            if (!effective) return;
            foreach (Entity entity in inDamageItems)
            {
                if (timers[entity].timer > 0) { timers[entity].timer -= Time.deltaTime; }
                DamageJudge(entity);
            }

            if (autoDead)
            {
                if (maxLiveTime > 0) { maxLiveTime -= Time.deltaTime; }
                if (maxLiveTime <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        #endregion
    }
}
