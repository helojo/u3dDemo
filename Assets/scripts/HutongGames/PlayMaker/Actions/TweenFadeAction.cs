namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD"), Tooltip("tween alpha")]
    public class TweenFadeAction : FsmStateAction
    {
        public FsmEvent finishEvent;
        public FsmGameObject obj;
        public FsmFloat time;
        public FsmFloat to;

        public override void OnEnter()
        {
            if (null == this.obj.Value)
            {
                base.Finish();
            }
            iTween.FadeTo(this.obj.Value, this.to.Value, this.time.Value);
            ScheduleMgr.Schedule(this.time.Value, delegate {
                base.Finish();
                if (this.finishEvent != null)
                {
                    base.Fsm.Event(this.finishEvent);
                }
            });
        }
    }
}

