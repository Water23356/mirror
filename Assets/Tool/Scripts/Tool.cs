using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class Tool
    {
        /// <summary>
        /// 判断指定字符串是否属于此字典的键
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public static bool isInside(string aim, Dictionary<string, object> keyValuePairs)
        {
            foreach (string s in keyValuePairs.Keys)
            {
                if (s == aim) { return true; }
            }
            return false;
        }
        /// <summary>
        /// 判断指定字符创是否属于此列表
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool isInside(string aim, List<string> list)
        {
            foreach (string s in list)
            {
                if (s == aim)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
