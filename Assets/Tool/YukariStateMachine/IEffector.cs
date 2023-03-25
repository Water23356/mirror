using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yukari
{
    /// <summary>
    /// 效果器
    /// </summary>
    public interface IEffector
    {
        /// <summary>
        /// 统一回调函数
        /// </summary>
        public Action<string> BackValue { get; set; }

        /// <summary>
        /// 实现效果，调用具体的函数
        /// </summary>
        /// <param name="name">函数标记符</param>
        /// <param name="parameters">函数参数</param>
        /// <return>返回一个字符串，用于判断是否跳转状态</return>
        public void Effect(string name,Dictionary<string,object> parameters);
    }
}
