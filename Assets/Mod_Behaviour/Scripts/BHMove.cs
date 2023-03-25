using System.Collections.Generic;
using UnityEngine;
using Tools;
using Mod_Attribute;
using Mod_Entity;

namespace Mod_Behaviour
{
    public enum State { Waiting, Running }
    public enum Dir {left,right,up,down }

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
        private Dir direction;//运动方向
        private float speed;//移动速度
        #endregion

        #region 公开属性
        public string Name { get => attributeName; set => attributeName = value; }
        public Entity Owner { get; set; }
        /// <summary>
        /// 运动方向
        /// </summary>
        public Dir Direction { get; set; }
        /// <summary>
        /// 移动速度
        /// </summary>
        public float Speed { get; set; }
        #endregion

        #region 功能属性
        public object GetStatus()
        {
            return state;
        }
        public bool StartBehaviour()
        {
            state = State.Running;
            return true;
        }
        public bool StopBehaviour()
        {
            state = State.Waiting;
            return true;
        }
        #endregion

        #region 内部函数
        private void Running()
        {
            switch(direction)
            {
                case Dir.left:
                    owner.transform.position -= new Vector3(speed,0,0);
                    break;
                case Dir.right:
                    owner.transform.position += new Vector3(speed,0,0);
                    break;
            }
        }
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
