namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("MTD-TEST"), Tooltip("UnitTestCreateChar")]
    public class UnitTestCreateChar : FsmStateAction
    {
        public override void OnEnter()
        {
            Debug.Log("CAREATE CHAR");
            SocketMgr.Instance.RequestRegister(GameDefine.getInstance().lastAccountName, GameDefine.getInstance().lastAcctId, GameDefine.getInstance().clientPlatformType, string.Empty, this.RamdomCreateName(), 1, 2, 0, false);
        }

        private string RamdomCreateName()
        {
            return ("Test" + UnityEngine.Random.Range(0, 0x3e8));
        }
    }
}

