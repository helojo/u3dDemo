namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("lock gui system."), ActionCategory("MTD-GUI")]
    public class LockGUIAction : FsmStateAction
    {
        public override void OnEnter()
        {
            GUIMgr.Instance.Lock();
            base.Finish();
        }
    }
}

