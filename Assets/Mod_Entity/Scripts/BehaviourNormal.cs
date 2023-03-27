using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Entity
{
    /// <summary>
    /// 行为属性模板类
    /// </summary>
    public class BehaviourNormal:MonoBehaviour,IBehaviour
    {
        #region 属性
        private string attributeName;
        private Entity owner;
        #endregion

        #region 公开属性
        public GameObject GameObject { get => gameObject; }
        public virtual string Name { get => attributeName; set => attributeName = value; }
        public virtual Entity Owner { get => owner; set => owner = value; }
        #endregion

        #region 功能函数
        public virtual object GetStatus()
        {
            return null;
        }

        public virtual bool StartBehaviour()
        {
            return false;
        }

        public virtual bool StopBehaviour()
        {
            return false;
        }
        public void Destroy()
        {
            Destroy(this);
        }
        #endregion
    }
}
