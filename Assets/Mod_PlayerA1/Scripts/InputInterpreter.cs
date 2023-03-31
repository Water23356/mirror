using Mod_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

        private void FixedUpdate()//每个时间刻对指令进行解析(结合输入指令和当前玩家状态)
        {
            while(inputs.Count > 0)
            {
                PlayerInputInfo input = inputs.Dequeue();
                old.Add(input);
                if(old.Count > maxCount)
                {
                    old.RemoveAt(0);
                }
            }
        }
    }
}
