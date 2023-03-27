using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Player
{
    internal class BHAttackPlayer : MonoBehaviour,IBehaviour
    {
        #region 属性
        private string attributeName = "玩家攻击行为";
        private Entity owner;
        #endregion

        #region 公开属性
        public GameObject GameObject { get => gameObject; }
        public string Name { get => attributeName; set => attributeName = value; }
        public Entity Owner { get => owner; set => owner = value; }
        #endregion

        #region 功能函数
        public object GetStatus()
        {
            return null;
        }

        public bool StartBehaviour()
        {
            return false;
        }

        public bool StopBehaviour()
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
