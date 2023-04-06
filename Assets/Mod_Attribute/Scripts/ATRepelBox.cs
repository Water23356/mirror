using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Attribute
{
    /// <summary>
    /// 击退箱
    /// </summary>
    public class ATRepelBox : StaticAttribute
    {
        /// <summary>
        /// 击退倍率
        /// </summary>
        public float multiplier = 1;
        /// <summary>
        /// 实体刚体
        /// </summary>
        public Rigidbody2D rigidbody;

        public ATRepelBox(Entity _owner) : base(_owner)
        {

        }

        private void GetRepel(DamageEventInfo info)
        {
            rigidbody.velocity = info.repel * multiplier;
        }

        public override void Initialization()
        {
            owner.DamangeEvent += GetRepel;
            rigidbody = owner.SelfRigidbody;
        }
    }
}
