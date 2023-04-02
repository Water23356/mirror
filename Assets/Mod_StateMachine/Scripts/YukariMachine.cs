
using Mod_Entity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Yukari;

namespace Mod_StateMachine
{
    /// <summary>
    /// 状态机信息
    /// </summary>
    public struct StateMachineInfo
    {

    }

    public class YukariMachine : MonoBehaviour
    {
        #region 属性
        private Entity owner;
        private string attributeName = "StateMachine";
        private StateController controller;//状态控制器
        private StateMachine stateMachine;//状态机
        #endregion

        #region 公开属性
        public Entity Owner { get => owner; set => owner = value; }
        public string Name { get=> attributeName; set => attributeName = value; }
        #endregion


        #region 功能函数
        public object GetStatus()
        {
            throw new System.NotImplementedException();

        }

        public bool StartMachine()
        {
            throw new NotImplementedException();
        }

        public bool StopMachine()
        {
            throw new NotImplementedException();
        }
        public void Destroy()
        {
            Destroy(this);
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
