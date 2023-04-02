using Mod_Entity;
using System;
using UnityEngine;
using Common;

namespace Mod_Player
{
    public class BHNormalPlayer : BHBase
    {
        #region 属性
        private new string attributeName = "玩家通常";
        /// <summary>
        /// 前摇时间
        /// </summary>
        private new float timeFront = 0;
        /// <summary>
        /// 行为持续时间（小于0表示无限定）
        /// </summary>
        private new float timeRunning = -1;
        /// <summary>
        /// 后摇时间
        /// </summary>
        private new float timeEnd = 0;

        private static string[] breaks = new string[]
        {
           "玩家移动",
           "玩家跳跃"
        };
        #endregion

        #region 实现
        public override string[] StartFront { get => null; }

        public override string[] StartSpace { get => null; }

        protected override string[] BreakFront { get => breaks; }

        protected override string[] BreakRunning { get => breaks; }

        protected override string[] BreakEnd { get => breaks; }
        public override string Name { get => attributeName; }
        #endregion

        #region 内部函数
        protected override void StartContent()
        {
        }

        protected override void LoopContent()
        {

        }

        protected override void StopContent()
        {

        }
        #endregion
    }
}
