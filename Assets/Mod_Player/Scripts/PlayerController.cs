using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Mod_Behaviour;
using Mod_Entity;
using UnityEngine;

namespace Mod_Player
{
    /// <summary>
    /// 输入信息
    /// </summary>
    public struct PlayerInputInfo
    {
        public float Horizontal;
        public float Vertical;
        public bool jump;
        public bool attack;
        public bool dash;
        public bool skill;
        public bool change;
    }

    public class PlayerController : ControlPanel,IAttribute
    {
        #region 属性
        private string attributeName = "玩家输入控制器";
        private PlayerInputInfo inputInfo;//输入信息缓存
        private Entity player;//玩家实体组件
        private bool active;
        #endregion

        #region 公开属性
        public GameObject GameObject { get => gameObject; }
        /// <summary>
        /// 输入监听是否有效
        /// </summary>
        public bool Active
        {
            get => active; set => active = value;  
        }
        public string Name { get => attributeName; set => attributeName=value; }
        public Entity Owner { get => player; set => player = value; }
        #endregion

        

        #region 抽象实现
        public sealed override void initialization()
        {
            player = GetComponent<Entity>();
        }
        /// <summary>
        /// 玩家输入监听
        /// </summary>
        public sealed override void InputMonitor()
        {
            inputInfo.Horizontal = Input.GetAxis("Horizontal");//水平轴
            inputInfo.Vertical = Input.GetAxis("Vertical");//垂直轴
            inputInfo.jump = Input.GetButtonDown("Jump");//跳跃
            inputInfo.attack = Input.GetButtonDown("Attack");//攻击
            inputInfo.dash = Input.GetButtonDown("Dash");//冲刺
            inputInfo.skill = Input.GetButtonDown("Skill");//技能
            inputInfo.change = Input.GetButtonDown("Change");//切换
            Debug.Log("玩家输入正在监听");
        }
        public sealed override void UpdateFunction()
        {
            player.GetAttribute<SMPlayer>().Input(inputInfo);//输入信息同步到状态机
        }
        /// <summary>
        /// 获取输入状态
        /// </summary>
        public object GetStatus()
        {
            return inputInfo;
        }
        public void Destroy()
        {
            Destroy(this);
        }
        #endregion
    }
}
