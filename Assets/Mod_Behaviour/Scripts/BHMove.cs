using System.Collections.Generic;
using UnityEngine;
using Tools;
using Mod_Attribute;
using Mod_Entity;

namespace Mod_Behaviour
{
    enum State { Waiting,Running}

    /// <summary>
    /// 移动行为
    /// </summary>
    public class BHMove : MonoBehaviour, IBehaviour
    {
        #region 属性
        private string attributeName;
        private Entity owner;
        private State state = State.Waiting;//状态机状态
        private Animator animator;//动画机
        private Vector2 direction;//运动方向
        #endregion

        #region 公开属性
        public string Name { get=>attributeName; set=>attributeName=value; }
        public Entity Owner { get; set; }

        public Vector2 Direction 
        { 
            get => direction;
            set
            {
                direction = value.normalized;
            }
                }

        #endregion
        #region 功能属性
        public object GetStatus()
        {
            return state;
        }

        public bool StartBehaviour()
        {
            return true;
        }

        public bool StopBehaviour()
        {

            return true;
        }
        #endregion

        #region Unity
        private void Start()
        {
            
        }
        private void Update()
        {
            
        }
        #endregion
    }
}
