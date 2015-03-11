namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("tween alpha"), ActionCategory("MTD")]
    public class TweenUIAlpha : FsmStateAction
    {
        public FsmEvent finishEvent;
        public FsmFloat from;
        public FsmGameObject obj;
        public FsmFloat time;
        public FsmFloat to;

        public override void OnEnter()
        {
            if (null == this.obj.Value)
            {
                base.Finish();
            }
            TweenAlpha alpha = TweenAlpha.Begin(this.obj.Value, this.time.Value, 0f);
            alpha.from = this.from.Value;
            alpha.to = this.to.Value;
            alpha.duration = this.time.Value;
            alpha.method = UITweener.Method.EaseOut;
            alpha.style = UITweener.Style.Once;
            alpha.SetOnFinished((EventDelegate.Callback) null);
            alpha.SetOnFinished(delegate {
                base.Finish();
                if (this.finishEvent != null)
                {
                    base.Fsm.Event(this.finishEvent);
                }
            });
        }
    }
}

