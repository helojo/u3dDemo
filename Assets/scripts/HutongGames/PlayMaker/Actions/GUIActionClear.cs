namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("close all gui panel."), ActionCategory("MTD-GUI")]
    public class GUIActionClear : FsmStateAction
    {
        public override void OnEnter()
        {
            GUIMgr.Instance.ClearAll();
            base.Finish();
        }
    }
}

