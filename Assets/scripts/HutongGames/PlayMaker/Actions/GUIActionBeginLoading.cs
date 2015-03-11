namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("begin loading"), ActionCategory("MTD-GUI")]
    public class GUIActionBeginLoading : FsmStateAction
    {
        public override void OnEnter()
        {
            base.OnEnter();
            LoadingPerfab.BeginTransition();
            base.Finish();
        }
    }
}

