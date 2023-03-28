using Mod_Player;
using UnityEngine;

namespace Common
{
    public class TESTCMD : MonoBehaviour
    {
        public PlayerController controller;
        public SMPlayer psm;

        public void GetPower()
        {
            Debug.Log("按钮按下！");
            bool f = ControllerAlloter.instance.GetPower(controller);
            psm.StartMachine();
            Debug.Log($"权限获取成功：{f}");
        }

        public void OffPower()
        {
            ControllerAlloter.instance.OffPower(controller);
        }
    }
}
