using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Entity
{
    /// <summary>
    /// 状态机状态
    /// </summary>
    public enum State 
    { 
        /// <summary>
        /// 未启动
        /// </summary>
        wait,
        /// <summary>
        /// 开始阶段（前段）
        /// </summary>
        start,
        /// <summary>
        /// 运行阶段（中段）
        /// </summary>
        running,
        /// <summary>
        /// 结束阶段（后段）
        /// </summary>
        end
    }

    /// <summary>
    /// 行为状态信息结构体
    /// </summary>
    public struct StateBH
    {
        /// <summary>
        /// 行为名称
        /// </summary>
        public string name;
        /// <summary>
        /// 前段时可接受取消的行为
        /// </summary>
        public string[] breakFront;
        /// <summary>
        /// 中段时可接受取消的行为
        /// </summary>
        public string[] breakRunning;
        /// <summary>
        /// 后段时可接受取消的行为
        /// </summary>
        public string[] breakEnd;
        /// <summary>
        /// 当前行为的状态
        /// </summary>
        public State status;
    }

    /// <summary>
    /// 行为模块抽象基类
    /// </summary>
    public abstract class BHBase : DynamicAttribute
    {
        #region 启动要求
        /// <summary>
        /// 前置行为状态要求
        /// </summary>
        public abstract string[] StartFront { get; }
        /// <summary>
        /// 前置空间状态要求
        /// </summary>
        public abstract string[] StartSpace { get; }
        #endregion

        #region 字段|属性
        protected new string attributeName = "行为";//属性名称
        protected State status;//当前状态机状态
        protected new ExciteEntity owner;
        #region 前段
        /// <summary>
        /// 前段最长持续时间
        /// </summary>
        protected float timeFront;
        /// <summary>
        /// 行为开始时调用（仅一次）
        /// </summary>
        protected Action startAction;
        /// <summary>
        /// 处于前段过程允许被以下行为打断
        /// </summary>
        protected abstract string[] BreakFront { get; }
        /// <summary>
        /// 前段计时器
        /// </summary>
        protected float timerF;
        public void AddStartAction(Action action) { startAction += action; }
        public void ClearStartAction() { startAction = null; }
        
        #endregion
        #region 中段
        /// <summary>
        ///中段最长持续时间(<0表示永久)
        /// </summary>
        protected float timeRunning;
        /// <summary>
        /// 行为中段时调用（持续调用）
        /// </summary>
        protected Action runningAction;
        /// <summary>
        /// 处于中段过程允许被以下行为打断
        /// </summary>
        protected abstract string[] BreakRunning { get; }
        /// <summary>
        /// 中段计时器
        /// </summary>
        protected float timerR;
        public void AddRunningAction(Action action) { runningAction += action; }
        public void ClearRunningAction() { runningAction = null; }
        #endregion
        #region 后段
        /// <summary>
        /// 后段最长持续时间
        /// </summary>
        protected float timeEnd;
        /// <summary>
        /// 行为中段时调用(仅一次)
        /// </summary>
        protected Action endAction;
        /// <summary>
        /// 处于中段过程允许被以下行为打断
        /// </summary>
        protected abstract string[] BreakEnd { get; }
        /// <summary>
        /// 后端计时器
        /// </summary>
        protected float timerE;
        public void AddEndAction(Action action) { endAction += action; }
        public void ClearEndAction() { endAction = null; }
        #endregion
        #endregion

        #region 属性
        /// <summary>
        /// 当前属性状态
        /// </summary>
        public State Status
        {
            get => status;
            set
            {
                status = value;
                switch(status)
                {
                    case State.start:
                        timerF = timeFront;
                        break;
                    case State.running:
                        timerR= timeRunning;
                        break;
                    case State.end:
                        timerE= timeEnd;
                        break;
                    case State.wait:
                        StopBH();
                        break;
                }
            }
        }
        public override Entity Owner { get => owner; set => owner = value as ExciteEntity; }
        public override string Name { get => attributeName; set { } }
        /// <summary>
        /// 获取当前行为信息
        /// </summary>
        public StateBH Info
        {
            get
            {
                return new StateBH
                {
                    name = attributeName,
                    breakFront = BreakFront,
                    breakRunning = BreakRunning,
                    breakEnd = BreakEnd,    
                };
            }
        }
        #endregion

        #region 功能函数
        public override object GetStatus()
        {
            return Info;
        }
        /// <summary>
        /// 开始此行为
        /// </summary>
        public void StartBH()
        {
            Debug.Log("行为开始");
            if (startAction != null)
            {
                startAction();
            }
            Status = State.start;
            enabled = true;
        }
        /// <summary>
        /// 结束此行为
        /// </summary>
        public void StopBH()
        {
            if (endAction != null)
            {
                endAction();
            }
            Status = State.wait;
            enabled = false;
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 前段行为
        /// </summary>
        protected abstract void StartContent();
        /// <summary>
        /// 中段行为
        /// </summary>
        protected abstract void LoopContent();
        /// <summary>
        /// 后段行为
        /// </summary>
        protected abstract void StopContent();
        #endregion

        #region Unity
        protected virtual void Update()
        {
            /*Debug.Log($" {Name} 行为 阶段：{status}  " +
                $" timer: {timerF} , {timerR} , {timerE}");*/
            switch(status)
            {
                case State.start:
                    StartContent();
                    if(timerF <= 0)
                    {
                        Debug.Log("前段");
                        Status = State.running;
                    }
                    break;
                case State.running:
                    LoopContent();
                    if (timeRunning < 0 || timerR <= 0)
                    {
                        Debug.Log("中段");
                        Status = State.end;
                    }
                    break;
                case State.end:
                    StopContent();
                    if(timerE <= 0)
                    {
                        Debug.Log("后段");
                        Status = State.wait;
                    }
                    break;
            }
        }
        #endregion
    }
}
