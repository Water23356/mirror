using Mod_Entity;
using System;
using UnityEngine;
using Common;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Mod_Player
{
    public class BHNormal : BHSenior
    {
        #region 属性

        #endregion

        public BHNormal(ExciteEntity _owner) : base(_owner)
        {
            attributeName = "通常";
        }

        #region 实现
        public override string Name { get => attributeName; }

        public override string[] Beakers { get => null; }

        public override void Initialization()
        {
            
        }

        public override bool Prerequisite()
        {
            return true;
        }

        protected override bool BehaviourContent()
        {
            return false;
        }

        protected override void BehaviourEnd()
        {
            
        }

        protected override void BehaviourStart()
        {
            
        }
        #endregion

        #region 内部函数
        #endregion
    }
}
