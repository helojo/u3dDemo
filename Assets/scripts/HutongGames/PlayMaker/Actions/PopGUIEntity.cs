namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD-GUI"), Tooltip("Pop the topest gui entity.")]
    public class PopGUIEntity : FsmStateAction
    {
        public override void OnEnter()
        {
            GUIMgr.Instance.PopGUIEntity();
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

