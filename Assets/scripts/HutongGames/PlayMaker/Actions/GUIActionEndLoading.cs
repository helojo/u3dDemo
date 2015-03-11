namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("end loading"), ActionCategory("MTD-GUI")]
    public class GUIActionEndLoading : FsmStateAction
    {
        public override void OnEnter()
        {
            base.OnEnter();
            LoadingPerfab.EndTransition();
            base.Finish();
        }
    }
}

