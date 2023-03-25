using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Yukari
{
    /// <summary>
    /// 状态控制器
    /// </summary>
    public class StateController
    {
        #region 属性
        private IEffector effector;
        /// <summary>
        /// 效果器
        /// </summary>
        public IEffector Effector
        {
            get => effector;
            set
            {
                effector = value;
                effector.BackValue = BackValue;
            }
        }

        /// <summary>
        /// 状态机
        /// </summary>
        public StateMachine stateMachine;
        /// <summary>
        /// 消息委托
        /// </summary>
        public Action<string> printAction;
        /// <summary>
        /// 当前状态信息
        /// </summary>
        public StatusCell statusInfo;
        /// <summary>
        /// 控制器状态
        /// </summary>
        public bool active = false;
        #endregion

        #region 构造函数
        public StateController() { }
        public StateController(StateMachine stateMachine, IEffector effector)
        {
            Effector = effector;
            this.stateMachine = stateMachine;
        }

        #endregion

        #region 内部函数
        private void Log(string txt)
        {
            if (printAction != null) { printAction(txt); }
            else { Console.Write(txt); }
        }
        #endregion
        /// <summary>
        /// 从头开始
        /// </summary>
        public void Start()
        {
            if (effector == null || stateMachine == null) { Log("缺少状态库 或 效果器\n"); return; }
            statusInfo = stateMachine.Find("Start");
            if (statusInfo == null) { Log("状态库缺少启动状态Start\n"); return; }
            active = true;
            Go();
        }

        #region 状态机
        private string nextName = "End";//下一个状态名称
        private int nextIndex = 0;//下一个状态步骤索引
        private string backvalue = "";//返回钥匙

        private int skipIndex = 0;//当前出口索引
        private int funcIndex = 0;//当前伴随函数索引

        private StepInfo nowStep = null;//当前步骤
        private SkipInfo nowSkip = null;//当前出口
        private bool exit = false;//出口确定
        private bool wait = false;//等待状态
        private enum status { step,skip,function}
        private status stm = status.step;
        private void Go()
        {
            while (active)//不用递归，防止深层递归导致爆栈
            {
                if(!wait)//若处于等待状态则直接跳过
                {
                    switch(stm)
                    {
                        case status.step:
                            Run_Step();
                            break;
                        case status.skip:
                            skipIndex = 0;
                            Run_Skip_Pre();
                            break;
                        case status.function:
                            Run_Skip_Function();
                            break;
                    }
                }
            }
        }

        public void BackValue(string key)//回调函数：获取事件返回值
        {
            backvalue = key + "";
            switch (stm)
            {
                case status.step://处于步骤函数运行后的断口
                    stm = status.skip;
                    wait = false;
                    break;
                case status.function://处于出口伴随函数的断口
                    wait = false;
                    break;
            }
        }
        public void Run_Step()//执行当前状态的所有步骤
        {
            nowStep = statusInfo.Next();
            if (nowStep == null || exit) //所有步骤执行完毕，或者已经确定了出口
            {
                if (statusInfo.name == "End") { active = false; }//为End状态不进行状态跳转
                else
                {
                    statusInfo = stateMachine.Find(nextName, nextIndex);//更新状态信息
                }
                //重置配置
                exit = false;
                nextIndex = 0;
                nextName = "End";
            }
            else { Run_Step_Function(); }
        }
        public void Run_Step_Function()//执行步骤函数，等待中断
        {
            FunctionInfo function = nowStep.function;//函数信息
            backvalue = "";//清空函数返回值
            if (function.name != "null")//为非空函数
            {
                wait = true;
                effector.Effect(function.name, function.parameters);//运行等待返回值
            }
            else//否则直接对出口进行判定运行
            {
                stm = status.skip;
                skipIndex = 0;
                Run_Skip_Pre();
            }
        }
        public void Run_Skip_Pre()//判定所有出口条件
        {
            //跳出出口遍历的条件:
            //当前状态是：End，因为End没有后继单元，无需进行出口判定
            //状态组遍历完毕
            //已经确定了出口
            if (statusInfo.name == "End" || skipIndex >= nowStep.skips.Count || exit) //若为End状态，则跳过此过程，直接跳出出口判定
            {
                nowSkip = null;
                stm = status.step;//返回上一级：对步骤的遍历
                return;
            }
            else//循环遍历出口
            {
                nowSkip = nowStep.skips[skipIndex];//获取出口
                //Console.WriteLine($"【debug】返回值：{backvalue}\t当前索引：{skipIndex}\t名称：{nowSkip.name}");
                if (backvalue == "" || nowSkip.condition == backvalue)//满足出口条件
                {
                    exit = true;
                    funcIndex = 0;
                    nextName = nowSkip.name;
                    nextIndex = nowSkip.index;
                    stm = status.function;//开始对伴随函数进行循环
                }
                ++skipIndex;
            }
        }
        public void Run_Skip_Function()
        {
            //跳出函数遍历条件：
            //出口伴随函数为空
            //出口伴随函数已全部遍历
            if(nowSkip.functions == null || funcIndex >= nowSkip.functions.Count)
            {
                
                stm = status.skip;//返回上一级：对出口的遍历
                return;
            }
            else
            {
                FunctionInfo function = nowSkip.functions[funcIndex];
                if (function.name != "null")//为非空函数
                {
                    wait = true;
                    effector.Effect(function.name, function.parameters);//运行等待返回值
                }
                //否则直接跳过此函数的执行
                ++funcIndex;
            }
        }
        #endregion

    }
}
