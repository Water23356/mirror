using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mod_Entity
{
    /// <summary>
    /// 实体管理类：
    /// 用于对实体所拥有的属性脚本进行管理，该实体下的属性脚本可通过此对象间接 获取 其他属性脚本；
    /// 在Start时，该脚本会自动将 所挂载的游戏物体 身上的所有 IAttribute 对象收纳管理列表当中
    /// </summary>
    public class Entity : MonoBehaviour
    {
        #region 属性
        private string damageTag = "Test";
        private List<IAttribute> attributes= new List<IAttribute>();
        /// <summary>
        /// 伤害标签
        /// </summary>
        public string DamageTag
        {
            get =>damageTag; set =>damageTag = value;
        }
        #endregion

        #region 公开函数
        /// <summary>
        /// 添加新的属性对象
        /// </summary>
        public void AddAttribute(IAttribute attribute)
        {
            attributes.Add(attribute);
            attribute.Owner = this;
        }
        /// <summary>
        /// 获取单个属性组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns></returns>
        public T GetAttribute<T>() where T :class,IAttribute
        {
            foreach(IAttribute attribute in attributes)
            {
                if (attribute is T) { return attribute as T; }
            }
            return null;
        }
        /// <summary>
        /// 获取属性组件组
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns></returns>
        public T[] GetAttributes<T>() where T : class, IAttribute
        {
            List<T> list = new List<T>();
            foreach (IAttribute attribute in attributes)
            {
                if (attribute is T) { list.Add(attribute as T); }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 根据名称获取指定属性组件
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public IAttribute GetAttribute(string name)
        {
            foreach(IAttribute attribute in attributes)
            {
                if (attribute.Name == name) { return attribute; }
            }
            return null;
        }    
        #endregion

        void Start()
        {
            IAttribute[] attributes = GetComponents<IAttribute>();
            foreach(IAttribute attribute in attributes)
            {
                AddAttribute(attribute);
            }
        }
        void Update()
        {

        }
    }
}