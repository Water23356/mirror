using UnityEngine;

namespace Common
{
    public class PrefabManager:MonoBehaviour
    {
        #region 单例封装
        /// <summary>
        /// 单例对象
        /// </summary>
        public static PrefabManager instance;
        #endregion

        #region 预制体组件
        public GameObject Entity;
        public GameObject DamagEntity;
        #endregion

        #region 功能函数
        public GameObject Instantiate(string prefabName)
        {
            GameObject obj = null;
            switch(prefabName)
            {
                case "基本实体":
                    obj = Instantiate(Entity);
                    break;
                case "基本伤害体":
                    obj = Instantiate(DamagEntity);
                    break;
            }
            

            return obj;
        }

        #endregion


        #region Unity
        private void Awake()
        {
            instance = this;
        }
        #endregion

    }
}
