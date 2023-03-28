using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Player
{
    internal class BHJumpPlayer : MonoBehaviour, IBehaviour
    {
        #region 属性
        private string attributeName = "玩家跳跃行为";
        private Entity owner;
        private State state = State.Waiting;
        private Rigidbody2D rigidBody;
        public float force;//跳跃力度
        #endregion

        #region 公开属性
        public GameObject GameObject { get => gameObject; }
        public string Name { get => attributeName; set => attributeName = value; }
        public Entity Owner
        { 
            get => owner;
            set
            {
                owner = value;
                rigidBody = owner.GetComponent<Rigidbody2D>();
            }
        }
        /// <summary>
        /// 跳跃力度
        /// </summary>
        public float Force
        {
            get => force;
            set => force = value;
        }
        #endregion

        #region 功能函数
        public object GetStatus()
        {
            return state;
        }

        public bool StartBehaviour()
        {
            state = State.Running;
            rigidBody.velocity = Vector2.up * force;
            return true;
        }

        public bool StopBehaviour()
        {
            state = State.Waiting;
            return true;
        }
        public void Destroy()
        {
            Destroy(this);
        }
        #endregion

        #region 内部函数
        #region 内部函数
        private void Running()
        {
            Debug.Log("跳跃！！！！");
            
        }
        #endregion
        #endregion

        #region Unity
        private void Start()
        {

        }
        private void Update()
        {
            switch (state)
            {
                case State.Running:
                    Running();
                    break;
                case State.Waiting:
                    break;
            }
        }
        #endregion
    }
}
