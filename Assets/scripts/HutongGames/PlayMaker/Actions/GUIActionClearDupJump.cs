namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("GUIActionClearDupJump."), ActionCategory("MTD-GUI")]
    public class GUIActionClearDupJump : FsmStateAction
    {
        public override void OnEnter()
        {
            ActorData.getInstance().mCurrDupReturnPrePara = null;
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

