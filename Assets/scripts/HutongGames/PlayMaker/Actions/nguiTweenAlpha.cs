namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("TweenAlpha NGUI object")]
    public class nguiTweenAlpha : FsmStateAction
    {
        public FsmFloat delay;
        public FsmFloat duration;
        public FsmFloat fromAlpha;
        public FsmInt group;
        public FsmBool ignoreTimeScale;
        public UITweener.Method method;
        public FsmOwnerDefault NguiObject;
        public UITweener.Style playStyle;
        private TweenAlpha theTween;
        public FsmFloat toAlpha;

        public override void OnEnter()
        {
            if (this.NguiObject != null)
            {
                base.Fsm.GetOwnerDefaultTarget(this.NguiObject).AddMissingComponent<TweenAlpha>();
                this.theTween = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenAlpha>();
                this.theTween.from = this.fromAlpha.Value;
                this.theTween.to = this.toAlpha.Value;
                this.theTween.style = this.playStyle;
                this.theTween.method = this.method;
                this.theTween.duration = this.duration.Value;
                this.theTween.ignoreTimeScale = this.ignoreTimeScale.Value;
                this.theTween.ResetToBeginning();
                this.theTween.PlayForward();
                base.Finish();
            }
        }

        [Tooltip("When true, runs on every frame")]
        public override void Reset()
        {
            this.NguiObject = null;
            this.fromAlpha = 0f;
            this.toAlpha = 1f;
            this.duration = 1f;
            this.delay = 0f;
            this.group = 0;
            this.ignoreTimeScale = 1;
        }
    }
}

