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
            if (input.jump)//跳跃
            {
                player.ChangeStateBH("玩家跳跃");
                return;
            }
            if (input.Horizontal != 0)//移动
            {
                BHMovePlayer BHmove = player.BHs["玩家移动"] as BHMovePlayer;
                BHmove.speed = Mathf.Abs(input.Horizontal) * BHmove.MaxSpeed;
                if (input.Horizontal < 0) { BHmove.Direction = Direction.left; }
                else { BHmove.Direction = Direction.right; }
                player.ChangeStateBH("玩家移动");
            }
            else//移动停止
            {
                BHMovePlayer BHmove = player.BHs["玩家移动"] as BHMovePlayer;
                BHmove.StopBH();
            }
            /*
            if (player.NowSpaceStatus.Name == "地面空间状态")
            {
                
            }
            else if(player.NowSpaceStatus.Name == "空中空间状态")
            {

            }*/
        }
        #endregion

        private void FixedUpdate()//每个时间刻对指令进行解析(结合输入指令和当前玩家状态)
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
