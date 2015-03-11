namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("TweenColor Ngui Object"), ActionCategory("Ngui Actions")]
    public class nguiTweenColor : FsmStateAction
    {
        public FsmFloat delay;
        public FsmFloat duration;
        public FsmColor fromColor;
        public FsmInt group;
        public FsmBool ignoreTimeScale;
        public UITweener.Method method;
        public FsmOwnerDefault NguiObject;
        public UITweener.Style playStyle;
        private TweenColor theTween;
        public FsmColor toColor;

        public override void OnEnter()
        {
            if (this.NguiObject != null)
            {
                base.Fsm.GetOwnerDefaultTarget(this.NguiObject).AddMissingComponent<TweenColor>();
                this.theTween = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenColor>();
                this.theTween.from = this.fromColor.Value;
                this.theTween.to = this.toColor.Value;
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
            this.fromColor = Color.black;
            this.toColor = Color.white;
            this.duration = 1f;
            this.delay = 0f;
            this.group = 0;
            this.ignoreTimeScale = 1;
        }
    }
}

