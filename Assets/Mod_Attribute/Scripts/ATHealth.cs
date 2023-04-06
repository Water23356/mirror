using Mod_Entity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Unity.VisualScripting;

namespace Mod_Attribute
{
    /// <summary>
    /// 生命响应事件
    /// </summary>
    /// <param name="info">生命事件信息</param>
    public delegate void HealthActionEvent(HealthEventInfo info);
    /// <summary>
    /// 生命判定事件
    /// </summary>
    /// <param name="info">生命事件信息</param>
    /// <returns>判定结果</returns>
    public delegate bool HealthJudgeEvent(HealthEventInfo info);

    /// <summary>
    /// 生命属性结构体
    /// </summary>
    public struct HealthValue
    {
        public float health;
        public float healthMax;
    }
    /// <summary>
    /// 生命组件 发生事件时的事件信息
    /// </summary>
    public struct HealthEventInfo
    {
        /// <summary>
        /// 当前生命值
        /// </summary>
        public float health;
        /// <summary>
        /// 当前最大生命值
        /// </summary>
        public float healthMax;
        /// <summary>
        /// 本次事件中 生命的变化值
        /// </summary>
        public float healthChange;
        /// <summary>
        /// 本次事件中 生命上限的变化值
        /// </summary>
        public float healthMaxChange;
        /// <summary>
        /// 触发此事件 的对象
        /// </summary>
        public object pruner;
    }

    /// <summary>
    /// 生命属性
    /// </summary>
    public class ATHealth : DynamicAttribute
    {
        #region 属性
        public float health = 100;
        public float healthMax = 100;
        public bool negative = false;
        public bool overflow = false;
        #endregion

        #region 公开属性
        /// <summary>
        /// 当前生命值
        /// </summary>
        public float Health { get => health; }
        /// <summary>
        /// 当前生命值上限
        /// </summary>
        public float HealthMax { get => healthMax; }
        /// <summary>
        /// 生命值是否可小于零
        /// </summary>
        public bool Negative { get => negative; set => negative = value; }
        /// <summary>
        /// 生命值是否可超过上限
        /// </summary>
        public bool Overflow { get => overflow; set => overflow = value; }

        #endregion

        #region 事件
        /// <summary>
        /// 在触发 生命值改变 事件，值改变前触发的事件（事件信息，当前操作是否生效）
        /// </summary>
        public event HealthJudgeEvent HealthChangeBefEvent;
        /// <summary>
        /// 在触发 生命值上限改变 事件，值改变前触发的事件（事件信息，当前操作是否生效）
        /// </summary>
        public event HealthJudgeEvent HealthMaxChangeBefEvent;
        /// <summary>
        /// 生命值变化后触发的事件(事件信息)
        /// </summary>
        public event HealthActionEvent HealthChangeEvent;
        /// <summary>
        /// 生命上限变化后触发的事件(事件信息)
        /// </summary>
        public event HealthActionEvent HealthMaxChangeEvent;
        /// <summary>
        /// 生命值归零后触发的事件
        /// </summary>
        public event HealthActionEvent DeadEvent;
        #endregion

        #region 功能函数
        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="info">伤害信息</param>
        public void GetDamage(DamageEventInfo info)
        {

        }
        /// <summary>
        /// 设置当前生命值
        /// </summary>
        /// <param name="value">修改后的生命值</param>
        /// <param name="pruner">修改者对象（触发事件的对象）</param>
        /// <returns></returns>
        public bool SetHealth(float value, object pruner)
        {
            bool next = true;
            float change = value - health;
            if (HealthChangeBefEvent != null)
            {
                next = HealthChangeBefEvent(new HealthEventInfo
                {
                    health = health,
                    healthMax = healthMax,
                    healthChange = change,
                    pruner= pruner
                }) ;
            }
            if(next)
            {
                health = value;

                #region 界线判定
                if (!negative)
                {
                    if (health < 0) { health = 0; }
                }
                if(!overflow)
                {
                    if (health > healthMax) { health = healthMax; }
                }
                #endregion

                if (HealthChangeEvent != null)
                {
                    HealthChangeEvent(new HealthEventInfo
                    {
                        health = health,
                        healthMax = healthMax,
                        healthChange = change,
                        pruner = pruner
                    });
                }

                if(health < 0 && DeadEvent !=null)
                {
                    DeadEvent(new HealthEventInfo
                    {
                        health = health,
                        healthMax = healthMax,
                        healthChange = change,
                        pruner = pruner
                    });
                }
            }
            return next;
        }
        /// <summary>
        /// 设置当前生命值上限
        /// </summary>
        /// <param name="value">修改后的生命值上限</param>
        /// <param name="pruner">修改者对象（触发事件的对象）</param>
        /// <returns></returns>
        public bool SetHealthMax(float value, object pruner)
        {
            bool next = true;
            float change = value - healthMax;
            if (HealthMaxChangeBefEvent != null)
            {
                next = HealthMaxChangeBefEvent(new HealthEventInfo
                {
                    health = health,
                    healthMax = healthMax,
                    healthMaxChange = change,
                    pruner = pruner
                });
            }
            if (next)
            {
                healthMax = value;

                #region 界线判定
                if (!negative)
                {
                    if (health < 0) { health = 0; }
                }
                if (!overflow)
                {
                    if (health > healthMax) { health = healthMax; }
                }
                #endregion

                if (HealthMaxChangeEvent != null)
                {
                    HealthMaxChangeEvent(new HealthEventInfo
                    {
                        health = health,
                        healthMax = healthMax,
                        healthMaxChange = change,
                        pruner = pruner
                    });
                }

            }
            return next;
        }
        /// <summary>
        /// 修改当前生命值
        /// </summary>
        /// <param name="value">变化值</param>
        /// <param name="pruner">修改者对象</param>
        /// <returns></returns>
        public bool ChangeHealth(float value, object pruner)
        {
            bool next = true;
            Debug.Log("生命发生改变！");
            if (HealthChangeBefEvent != null)
            {
                next = HealthChangeBefEvent(new HealthEventInfo
                {
                    health = health,
                    healthMax = healthMax,
                    healthChange = value,
                    pruner = pruner
                });
            }
            if (next)
            {
                health += value;

                #region 界线判定
                if (!negative)
                {
                    if (health < 0) { health = 0; }
                }
                if (!overflow)
                {
                    if (health > healthMax) { health = healthMax; }
                }
                #endregion

                if (HealthChangeEvent != null)
                {
                    HealthChangeEvent(new HealthEventInfo
                    {
                        health = health,
                        healthMax = healthMax,
                        healthChange = value,
                        pruner = pruner
                    });
                }
                if (health < 0 && DeadEvent != null)
                {
                    DeadEvent(new HealthEventInfo
                    {
                        health = health,
                        healthMax = healthMax,
                        healthChange = value,
                        pruner = pruner
                    });
                }
            }
            return next;
        }
        /// <summary>
        /// 修改当前生命值上限
        /// </summary>
        /// <param name="value">变化值</param>
        /// <param name="pruner">修改者对象</param>
        /// <returns></returns>
        public bool CHangeHealthMax(float value, object pruner)
        {
            bool next = true;
            if (HealthMaxChangeBefEvent != null)
            {
                next = HealthMaxChangeBefEvent(new HealthEventInfo
                {
                    health = health,
                    healthMax = healthMax,
                    healthMaxChange = value,
                    pruner = pruner
                });
            }
            if (next)
            {
                healthMax += value;

                #region 界线判定
                if (!negative)
                {
                    if (health < 0) { health = 0; }
                }
                if (!overflow)
                {
                    if (health > healthMax) { health = healthMax; }
                }
                #endregion

                if (HealthMaxChangeEvent != null)
                {
                    HealthMaxChangeEvent(new HealthEventInfo
                    {
                        health = health,
                        healthMax = healthMax,
                        healthMaxChange = value,
                        pruner = pruner
                    });
                }
            }
            return next;
        }
        public override object GetStatus()
        {
            return new HealthValue { health = health,healthMax=healthMax};
        }
        #endregion

        #region 内部函数
        private void Start()
        {
            attributeName = "Health";
        }

        public override void Initialization()
        {
            owner.DamangeEvent += GetDamage;
        }
        #endregion
    }
}
