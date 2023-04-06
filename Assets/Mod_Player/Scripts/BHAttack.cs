using Common;
using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Player
{
    public class BHAttack : BHSenior
    {
        private string[] breakers = new string[] { };
        private float timer = 0.5f;
        private int state = 0;

        public BHAttack(ExciteEntity _owner) : base(_owner)
        {
            attributeName = "攻击";
        }

        public override string[] Beakers => breakers;

        public override void Initialization()
        {

        }

        public override bool Prerequisite()
        {
            return true;
        }

        private void Creat()
        {
            GameObject obj = PrefabManager.instance.Instantiate("基础伤害体");
            DamageEntity damage = obj.GetComponent<DamageEntity>();
            damage.AutoDead = true;
            damage.MaxLiveTime = 1;
            damage.RepMode = DamageEntity.RepelMode.Auto;
            damage.RepPower = 2;
            damage.Damage = 10;
            damage.addDamageTag("Test");
            damage.Owner = owner;
            obj.transform.position = owner.transform.position;
        }

        protected override bool BehaviourContent()
        {
            bool next = true;
            switch(state)
            {
                case 0:
                    timer -= Time.deltaTime;
                    if(timer<0)
                    {
                        state = 1;
                        timer = 1;
                    }
                    break;
                case 1:
                    Creat();
                    state = 2;
                    break;
                case 2:
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        state = 3;
                        timer = 0.5f;
                    }
                    break;
                case 3:
                    timer -= Time.deltaTime;
                    if(timer<0)
                    {
                        next = true;
                    }
                    break;

            }
            return next;
        }

        protected override void BehaviourEnd()
        {
        }

        protected override void BehaviourStart()
        {
            timer = 0.5f;
        }
    }
}
