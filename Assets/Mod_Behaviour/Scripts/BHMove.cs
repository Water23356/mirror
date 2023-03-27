using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Behaviour
{
    public class BHMove : BehaviourNormal
    {
        #region 属性
        private string attributeName;
        private Entity owner;
        #endregion

        #region 公开属性
        public override string Name { get => attributeName; set => attributeName = value; }
        public override Entity Owner { get => owner; set => owner=value; }
        #endregion

        #region 功能函数
        public override object GetStatus()
        {
            return null;
        }

        public override bool StartBehaviour()
        {
            return false;
        }

        public override bool StopBehaviour()
        {
            return false;
        }
        #endregion
    }
}
