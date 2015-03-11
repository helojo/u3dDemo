namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("Ngui Actions"), Tooltip("TweenScale Ngui Object")]
    public class nguiTweenScale : FsmStateAction
    {
        public FsmFloat delay;
        public FsmFloat duration;
        public FsmVector3 fromScale;
        public FsmInt group;
        public FsmBool ignoreTimeScale;
        public UITweener.Method method;
        public FsmOwnerDefault NguiObject;
        public UITweener.Style playStyle;
        private TweenScale theTween;
        public FsmVector3 toScale;

        public override void OnEnter()
        {
            if (this.NguiObject != null)
            {
                base.Fsm.GetOwnerDefaultTarget(this.NguiObject).AddMissingComponent<TweenScale>();
                this.theTween = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenScale>();
                this.theTween.from = this.fromScale.Value;
                this.theTween.to = this.toScale.Value;
                this.theTween.style = this.playStyle;
                this.theTween.method = this.method;
                this.theTween.duration = this.duration.Value;
                this.theTween.ignoreTimeScale = this.ignoreTimeScale.Value;
                this.theTween.ResetToBeginning();
                this.theTween.PlayForward();
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.NguiObject = null;
            this.fromScale = Vector3.zero;
            this.toScale = Vector3.one;
            this.duration = 1f;
            this.delay = 0f;
            this.group = 0;
            this.ignoreTimeScale = 1;
        }
    }
}

