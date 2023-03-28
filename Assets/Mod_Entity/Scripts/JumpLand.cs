using Mod_Entity;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Entity
{
    public class JumpLand:MonoBehaviour
    {
        #region 属性
        private bool runing = true;
        public SMLand smp;//关联状态机
        private List<object> lands = new List<object>();
        #endregion

        #region 功能函数
        /// <summary>
        /// 启用着地检测
        /// </summary>
        public void Usecheck()
        {
            runing = true;
        }
        /// <summary>
        /// 关闭着地检测
        /// </summary>
        public void Unusecheck()
        {
            runing= false;
        }
        #endregion

        #region 触发监听
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(runing)
            {
                if (collision.tag == "Barrier")//检测物体为障碍物
                {
                    if(!lands.Contains(collision.gameObject))
                    {
                        lands.Add(collision.gameObject);
                    }
                    smp.StopDrop();
                    smp.isAir = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (runing)
            {
                if (collision.tag == "Barrier")//检测物体为障碍物
                {
                    if (!lands.Contains(collision.gameObject))
                    {
                        lands.Remove(collision.gameObject);
                    }
                    if(lands.Count == 0)
                    {
                        smp.isAir = true;
                    }
                }
            }
        }
        #endregion

        #region Unity
        private void Start()
        {
            
        }
        private void Update()
        {
            
        }
        #endregion
    }
}
