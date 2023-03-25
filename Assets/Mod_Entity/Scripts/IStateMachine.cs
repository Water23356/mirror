using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod_Entity
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IStateMachine : IAttribute
    {
        /// <summary>
        /// 开启状态机（重新开始）
        /// </summary>
        /// <returns>执行是否成功</returns>
        public bool StartMachine();
        /// <summary>
        /// 终止状态机
        /// </summary>
        /// <returns>执行是否成功</returns>
        public bool StopMachine();
    }
}
