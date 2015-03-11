namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("NGUI HUD Follow target"), ActionCategory("Ngui Actions")]
    public class nguiSetFollowTarget : FsmStateAction
    {
        [UIHint(UIHint.Description)]
        public string notice = "If you have NGUI HUD TEXT installed.\nJust install file (nguiHUDaction) in folder (AT1 Ngui Actions)\nto activate this action";

        public override void OnEnter()
        {
            base.Finish();
        }
    }
}

