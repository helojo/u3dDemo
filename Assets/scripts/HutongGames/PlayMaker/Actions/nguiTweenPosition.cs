namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("TweenPosition Ngui Object"), ActionCategory("Ngui Actions")]
    public class nguiTweenPosition : FsmStateAction
    {
        public FsmFloat delay;
        public FsmFloat duration;
        public FsmVector3 fromPosition;
        public FsmInt group;
        public FsmBool ignoreTimeScale;
        public UITweener.Method method;
        public FsmOwnerDefault NguiObject;
        public FsmEvent onFinished;
        public UITweener.Style playStyle;
        private TweenPosition theTween;
        public FsmVector3 toPosition;

        public override void OnEnter()
        {
            if (this.NguiObject != null)
            {
                base.Fsm.GetOwnerDefaultTarget(this.NguiObject).AddMissingComponent<TweenPosition>();
                this.theTween = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenPosition>();
                this.theTween.from = this.fromPosition.Value;
                this.theTween.to = this.toPosition.Value;
                this.theTween.style = this.playStyle;
                this.theTween.method = this.method;
                this.theTween.duration = this.duration.Value;
                this.theTween.ignoreTimeScale = this.ignoreTimeScale.Value;
                this.theTween.ResetToBeginning();
                this.theTween.PlayForward();
                if (this.onFinished != null)
                {
                    this.theTween.AddOnFinished(delegate {
                        base.Fsm.Event(this.onFinished);
                        base.Finish();
                    });
                }
                else
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
        }

        public override void Reset()
        {
            this.NguiObject = null;
            this.fromPosition = Vector3.zero;
            this.toPosition = Vector3.one;
            this.duration = 1f;
            this.delay = 0f;
            this.group = 0;
            this.ignoreTimeScale = 1;
            this.onFinished = null;
        }
    }
}

