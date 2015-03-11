namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("rebuild card pool"), ActionCategory("MTD")]
    public class ReBuildCardPool : FsmStateAction
    {
        public override void OnEnter()
        {
            CardPool.RebuildAll(true);
            base.Finish();
        }
    }
}

