using Mod_Entity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Unity.VisualScripting;

namespace Mod_Attribute
{
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
    public class ATHealth : MonoBehaviour, IAttribute
    {
        #region 属性
        private string attributeName = "Health";
        public float health = 100;
        public float healthMax = 100;
        private Entity owner;
        public bool negative = false;
        public bool overflow = false;
        #endregion

        #region 公开属性
        /// <summary>
        /// 生命值是否可小于零
        /// </summary>
        public bool Negative { get => negative; set => negative = value; }
        /// <summary>
        /// 生命值是否可超过上限
        /// </summary>
        public bool Overflow { get => overflow; set => overflow = value; }
        public string Name { get => attributeName; set => attributeName = value; }
        public Entity Owner { get => owner; set => owner = value; }

        #endregion

        #region 事件
        /// <summary>
        /// 在触发 生命值改变 事件，值改变前触发的事件（事件信息，当前操作是否生效）
        /// </summary>
        public Func<HealthEventInfo, bool> HealthChangeBefAction;
        /// <summary>
        /// 在触发 生命值上限改变 事件，值改变前触发的事件（事件信息，当前操作是否生效）
        /// </summary>
        public Func<HealthEventInfo, bool> HealthMaxChangeBefAction;
        /// <summary>
        /// 生命值变化后触发的事件(事件信息)
        /// </summary>
        public Action<HealthEventInfo> HealthChangeAction;
        /// <summary>
        /// 生命上限变化后触发的事件(事件信息)
        /// </summary>
        public Action<HealthEventInfo> HealthMaxChangeAction;
        /// <summary>
        /// 生命值归零后触发的事件
        /// </summary>
        public Action DeadAction;
        #endregion

        #region 功能函数
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
            if (HealthChangeBefAction != null)
            {
                next = HealthChangeBefAction(new HealthEventInfo
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

                if (HealthChangeAction != null)
                {
                    HealthChangeAction(new HealthEventInfo
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
            if (HealthMaxChangeBefAction != null)
            {
                next = HealthMaxChangeBefAction(new HealthEventInfo
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

                if (HealthMaxChangeAction != null)
                {
                    HealthMaxChangeAction(new HealthEventInfo
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
            if (HealthChangeBefAction != null)
            {
                next = HealthChangeBefAction(new HealthEventInfo
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

                if (HealthChangeAction != null)
                {
                    HealthChangeAction(new HealthEventInfo
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
            if (HealthMaxChangeBefAction != null)
            {
                next = HealthMaxChangeBefAction(new HealthEventInfo
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

                if (HealthMaxChangeAction != null)
                {
                    HealthMaxChangeAction(new HealthEventInfo
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
        public object GetStatus()
        {
            return new HealthValue { health = health,healthMax=healthMax};
        }
        #endregion

        #region 内部函数

        #endregion
    }
}
