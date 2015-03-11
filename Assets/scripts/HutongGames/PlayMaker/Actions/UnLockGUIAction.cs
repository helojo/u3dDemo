namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("unlock gui system."), ActionCategory("MTD-GUI")]
    public class UnLockGUIAction : FsmStateAction
    {
        public override void OnEnter()
        {
            GUIMgr.Instance.UnLock();
            base.Finish();
        }
    }
}

