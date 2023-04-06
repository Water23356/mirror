using Common;
using Mod_Entity;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Player
{
    /// <summary>
    /// 输入解释器
    /// </summary>
    public class InputInterpreter:MonoBehaviour
    {
        #region 属性
        /// <summary>
        /// 玩家实体
        /// </summary>
        public ExciteEntity player;
        /// <summary>
        /// 历史记录最大数量
        /// </summary>
        public int maxCount = 30;
        /// <summary>
        /// 输入指令队列
        /// </summary>
        private Queue<PlayerInputInfo> inputs = new Queue<PlayerInputInfo>();
        /// <summary>
        /// 输入历史记录
        /// </summary>
        private List<PlayerInputInfo> old = new List<PlayerInputInfo>();
        #endregion

        #region 功能函数
        public void AddInput(PlayerInputInfo input)
        {
            inputs.Enqueue(input);
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 根据当前玩家状态 和 玩家输入 解释出相应的动作行为
        /// </summary>
        /// <param name="input"></param>
        private void Parse(PlayerInputInfo input)
        {
            Debug.Log("解释轴："+(input.Horizontal!=0));
            Debug.Log("解释跳跃:"+input.jump);
            if (input.jump)//跳跃
            {
                player.ChangeStateBH("跳跃");
                return;
            }
            if (input.Horizontal != 0)//移动
            {
                BHMove BHmove = player.BHs["移动"] as BHMove;
                BHmove.speed = Mathf.Abs(input.Horizontal) * BHmove.maxSpeed;
                if (input.Horizontal < 0) { BHmove.direction = Direction.left; }
                else { BHmove.direction = Direction.right; }
                player.ChangeStateBH("移动");
            }
            else//移动停止
            {
                Debug.Log("移动停止");
                BHMove BHmove = player.BHs["移动"] as BHMove;
                BHmove.active = false;
            }
        }
        #endregion

        private void Update()//每帧对指令进行解析(结合输入指令和当前玩家状态)
        {
            while(inputs.Count > 0)
            {
                PlayerInputInfo input = inputs.Dequeue();
                Parse(input);
                old.Add(input);
                if(old.Count > maxCount)
                {
                    old.RemoveAt(0);
                }
            }
        }
    }
}
