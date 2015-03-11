namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD-GUI"), Tooltip("open an unique gui entity.")]
    public class OpenUniqueGUIAction : FsmStateAction
    {
        public FsmInt depth;
        public FsmEvent onCompleted;
        [RequiredField]
        public FsmString Scene;

        public override void OnEnter()
        {
            GUIMgr.Instance.OpenUniqueGUIEntity(this.Scene.Value, delegate (GUIEntity entity) {
                if (this.depth.Value > 0)
                {
                    entity.Depth = this.depth.Value;
                }
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
            this.depth = -1;
            this.onCompleted = null;
        }
    }
}

