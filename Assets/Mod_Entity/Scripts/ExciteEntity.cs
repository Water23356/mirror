using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mod_Entity
{

    /// <summary>
    /// 自带状态管理器的实体基类
    /// </summary>
    public class ExciteEntity : Entity
    {
        #region 字段
        protected string damageTag = "ExciteEntity";
        protected List<IAttribute> attributes = new List<IAttribute>();//实体属性
        protected Dictionary<string, SMSpace> spaces = new Dictionary<string, SMSpace>();//空间状态（名称，对象）
        protected Dictionary<string, BHBase> bhs = new Dictionary<string, BHBase>();//行为状态（名称，对象）
        protected BHBase nowBh;//当前行为
        protected BHBase defBh;//默认行为状态
        #endregion

        #region 公开属性
        /// <summary>
        /// 当前行为状态
        /// </summary>
        public BHBase NowBehaviourStatus { get => nowBh; }
        /// <summary>
        /// 实体拥有的空间状态属性
        /// </summary>
        public Dictionary<string, SMSpace> Spaces { get => spaces; }
        /// <summary>
        /// 实体拥有的行为状态属性
        /// </summary>
        public Dictionary<string, BHBase> BHs { get => bhs; }
        #endregion

        #region 功能函数
        /// <summary>
        /// 设置默认行为状态
        /// </summary>
        public void SetDefBehaviour(BHBase bhbs)
        {
            defBh = bhbs;
        }
        /// <summary>
        /// 改变行为状态
        /// </summary>
        /// <param name="name">行为状态名称</param>
        public virtual void ChangeStateBH(string name)
        {
            Debug.Log("切换行为：目标:"+name);
            if (bhs.Keys.Contains(name))//存在指定行为
            {
                BHBase bh = bhs[name];
                if (bh is BHSenior)
                {
                    if (nowBh is BHSenior && nowBh != defBh)
                    {
                        if(((BHSenior)nowBh).Beakers != null)//若Breakers为null，则无视中断对象限定
                        {
                            if (!((BHSenior)nowBh).Beakers.Contains(name)) { return; }//判定是否满足中断条件
                        }
                    }
                    if (!((BHSenior)bh).Prerequisite()) { return; }//判定是否符合释放条件
                }
                Debug.Log("状态已切换至："+bh.Name);
                nowBh = bh;
            }
            else
            {
                Debug.LogWarning("指定行为不存在!");
            }
        }
        /// <summary>
        /// 添加新的属性对象
        /// </summary>
        public override void AddAttribute(IAttribute attribute)
        {
            if (attribute == null) return;
            if (attributes.Contains(attribute)) return;
            Debug.Log("添加属性成功");
            attributes.Add(attribute);
            BHBase bh = attribute as BHBase;
            if (bh != null)//添加行为模块
            {
                bhs.Add(bh.Name, bh);
            }
            SMSpace smsp = attribute as SMSpace;
            if (smsp != null)//添加空间状态模块
            {
                spaces.Add(smsp.Name, smsp);
            }
            attribute.Owner = this;
            Debug.Log(attributes.Count);
        }
        /// <summary>
        /// 从这个实体中移除指定属性（同时销毁组件）
        /// </summary>
        /// <param name="attribute"></param>
        public override void RemoveAttribute(IAttribute attribute)
        {
            if (attributes.Contains(attribute))
            {
                attributes.Remove(attribute);
                BHBase bh = attribute as BHBase;
                if (bh != null)
                {
                    bhs.Remove(bh.Name);
                }
                SMSpace smsp = attribute as SMSpace;
                if (smsp != null)//添加空间状态模块
                {
                    spaces.Remove(smsp.Name);
                }
                attribute.Destroy();
            }
        }
        /// <summary>
        /// 查看是否处于指定空间状态
        /// </summary>
        /// <param name="name">空间状态名称</param>
        /// <returns></returns>
        public virtual bool SpaceConform(string name)
        {
            if(spaces.Keys.Contains(name))
            {
                if (spaces[name].SpaceConform)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 内部函数

        #endregion

        #region Unity
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            Debug.Log("组件数量：" + attributes.Count);
            foreach(IAttribute attribute in attributes)
            {
                attribute.Initialization();
            }
        }
        protected void Update()
        {
            Debug.Log("当前角色状态："+ NowBehaviourStatus);
            if (nowBh != null)
            {
                if(nowBh.Run())
                {
                    nowBh = defBh;
                }
            }
            else
            {
                nowBh = defBh;
            }
        }
        #endregion
    }
}
