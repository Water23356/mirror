using Mod_Entity;
using UnityEngine;

namespace Mod_Player
{
    /// <summary>
    /// 玩家状态机，负责状态切换，协调动画机，状态内逻辑
    /// </summary>
    public class SMPlayer : SMLand
    {

        #region 属性
        private string attributeName = "玩家状态机";
        private Entity player;
        private PlayerInputInfo inputInfo;//输入信息
        #endregion

        #region 公开属性
        public override string Name { get => attributeName; set => attributeName = value; }
        public override Entity Owner { get => player; set => player = value; }
        #endregion

        #region 功能函数
        /// <summary>
        /// 获取当前状态状态
        /// </summary>
        /// <returns>一个状态枚举</returns>
        public override object GetStatus()
        {
            return status;
        }
        public override bool StartMachine()
        {
            status = State.normal;
            return true;
        }
        public override bool StopMachine()
        {
            status = State.stop;
            return false;
        }
        public void Input(PlayerInputInfo input) { inputInfo = input; }
        #endregion



        #region 状态机
        public override void StopDrop()
        {
            switch(status)
            {
                case State.air:
                    status = State.normal;
                    break;
                case State.moveAir:
                    status = State.move;
                    break;
            }
            
        }
        /// <summary>
        /// 状态机状态
        /// </summary>
        public enum State
        {
            /// <summary>
            /// 状态机停止
            /// </summary>
            stop,
            /// <summary>
            /// 通常
            /// </summary>
            normal,
            /// <summary>
            /// 移动
            /// </summary>
            move,
            /// <summary>
            /// 跳跃(空中状态)
            /// </summary>
            air,
            /// <summary>
            /// 空中移动
            /// </summary>
            moveAir,
            /// <summary>
            /// 攻击
            /// </summary>
            attack,
            /// <summary>
            /// 技能
            /// </summary>
            skill,
            /// <summary>
            /// 切换
            /// </summary>
            change,
            /// <summary>
            /// 下落
            /// </summary>
            drop,
            /// <summary>
            /// 冲刺
            /// </summary>
            dash,
            /// <summary>
            /// 受伤僵直
            /// </summary>
            injured,
        }
        private State status = State.normal;//当前状态
        /// <summary>
        /// 通常状态
        /// </summary>
        private void State_Normal()
        {
            if(isAir)//滞空判定
            {
                status = State.air;
                return;
            }
            #region 移动操作判定
            if (inputInfo.Horizontal < 0)
            {
                BHMovePlayer bHMove = player.GetAttribute<BHMovePlayer>();
                if (bHMove != null)
                {
                    bHMove.Direction = Dir.left;
                    bHMove.Speed = Mathf.Abs(bHMove.MaxSpeed*inputInfo.Horizontal);
                    bHMove.StartBehaviour();
                    status = State.move;
                }
            }
            else if (inputInfo.Horizontal > 0)
            {
                BHMovePlayer bHMove = player.GetAttribute<BHMovePlayer>();
                if (bHMove != null)
                {
                    bHMove.Direction = Dir.right;
                    bHMove.Speed = Mathf.Abs(bHMove.MaxSpeed * inputInfo.Horizontal);
                    bHMove.StartBehaviour();
                    status = State.move;
                }
            }
            #endregion
            else if (inputInfo.attack)//攻击操作判定
            {
                BHAttackPlayer bHAttack = player.GetAttribute<BHAttackPlayer>();
                if (bHAttack != null)
                {
                    status = State.attack;
                }
            }
            else if (inputInfo.jump)//跳跃操作判定
            {
                BHJumpPlayer bHJump = player.GetAttribute<BHJumpPlayer>();
                if (bHJump != null)
                {
                    bHJump.StartBehaviour();
                    status = State.air;
                }
            }
            else if (inputInfo.skill)//技能操作判定
            {

            }
            else if (inputInfo.change)//切换操作判定
            {

            }
        }
        /// <summary>
        /// 移动状态
        /// </summary>
        private void State_Move()
        {
            if(inputInfo.Horizontal == 0)//停止移动
            {
                BHMovePlayer bHMove = player.GetAttribute<BHMovePlayer>();
                bHMove.Speed = 0;
                
                bHMove.StopBehaviour();
                Debug.Log("停止：" + bHMove.GetStatus());
                status = State.normal;
            }
            else
            {
                BHMovePlayer bHMove = player.GetAttribute<BHMovePlayer>();
                bHMove.Speed = Mathf.Abs(bHMove.MaxSpeed * inputInfo.Horizontal);
            }
        }
        /// <summary>
        /// 跳跃状态
        /// </summary>
        private void State_Air()
        {
            #region 移动操作判定
            if (inputInfo.Horizontal < 0)
            {
                BHMovePlayer bHMove = player.GetAttribute<BHMovePlayer>();
                if (bHMove != null)
                {
                    bHMove.Direction = Dir.left;
                    bHMove.Speed = Mathf.Abs(bHMove.MaxSpeed * inputInfo.Horizontal);
                    bHMove.StartBehaviour();
                    status = State.move;
                }
            }
            else if (inputInfo.Horizontal > 0)
            {
                BHMovePlayer bHMove = player.GetAttribute<BHMovePlayer>();
                if (bHMove != null)
                {
                    bHMove.Direction = Dir.right;
                    bHMove.Speed = Mathf.Abs(bHMove.MaxSpeed * inputInfo.Horizontal);
                    bHMove.StartBehaviour();
                    status = State.move;
                }
            }
            #endregion
            if (inputInfo.jump)//跳跃操作判定
            {
                BHJumpPlayer bHJump = player.GetAttribute<BHJumpPlayer>();
                if (bHJump != null)
                {
                    bHJump.StartBehaviour();
                    status = State.moveAir;
                }
            }
        }
        /// <summary>
        /// 空中移动
        /// </summary>
        private void State_MoveAir()
        {
            if (inputInfo.Horizontal == 0)//停止移动
            {
                BHMovePlayer bHMove = player.GetAttribute<BHMovePlayer>();
                bHMove.Speed = 0;

                bHMove.StopBehaviour();
                Debug.Log("停止：" + bHMove.GetStatus());
                status = State.air;
            }
            else
            {
                BHMovePlayer bHMove = player.GetAttribute<BHMovePlayer>();
                bHMove.Speed = Mathf.Abs(bHMove.MaxSpeed * inputInfo.Horizontal);
            }
        }
        /// <summary>
        /// 攻击状态
        /// </summary>
        private void State_Attack()
        {

        }
        /// <summary>
        /// 冲刺状态
        /// </summary>
        private void State_Dash()
        {

        }
        /// <summary>
        /// 使用技能状态
        /// </summary>
        private void State_Skill()
        {

        }
        /// <summary>
        /// 切换宝石状态
        /// </summary>
        private void State_Change()
        {

        }

        #endregion

        #region Unity
        private void Update()
        {
            Debug.Log($"当前玩家状态：{status}");
            switch (status)
            {
                case State.normal:
                    State_Normal();
                    break;
                case State.move:
                    State_Move();
                    break;
                case State.skill:
                    State_Skill();
                    break;
                case State.change:
                    State_Change();
                    break;
                case State.attack:
                    State_Attack();
                    break;
                case State.moveAir:
                    State_MoveAir();
                    break;
                case State.air:
                    State_Air();
                    break;
                case State.dash:
                    State_Dash();
                    break;
            }
        }

        #endregion
    }
}
