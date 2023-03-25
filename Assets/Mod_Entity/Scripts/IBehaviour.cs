using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Mod_Entity
{
    /// <summary>
    /// 行为接口
    /// </summary>
    public interface IBehaviour:IAttribute
    {
        /// <summary>
        /// 执行此行动
        /// </summary>
        /// <returns>返回此执行是否成功</returns>
        public bool StartBehaviour();
        /// <summary>
        /// 终止此行动
        /// </summary>
        /// <returns>返回此执行是否成功</returns>
        public bool StopBehaviour();
    }
}
