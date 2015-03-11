namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Push a custom gui entity."), ActionCategory("MTD-GUI")]
    public class PushGUIEntity : FsmStateAction
    {
        public FsmEvent onCompleted;
        [RequiredField]
        public FsmString Scene;

        public override void OnEnter()
        {
            GUIMgr.Instance.PushGUIEntity(this.Scene.Value, delegate (GUIEntity entity) {
                base.Fsm.Event(this.onCompleted);
                base.Finish();
            });
            if (this.onCompleted == null)
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.Scene = string.Empty;
            this.onCompleted = null;
        }
    }
}

