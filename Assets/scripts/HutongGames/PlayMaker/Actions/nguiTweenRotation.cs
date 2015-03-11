namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("Ngui Actions"), Tooltip("TweenRotation Ngui Object")]
    public class nguiTweenRotation : FsmStateAction
    {
        public FsmFloat delay;
        public FsmFloat duration;
        public FsmVector3 fromPosition;
        public FsmInt group;
        public FsmBool ignoreTimeScale;
        public UITweener.Method method;
        public FsmOwnerDefault NguiObject;
        public UITweener.Style playStyle;
        private TweenRotation theTween;
        public FsmVector3 toPosition;

        public override void OnEnter()
        {
            if (this.NguiObject != null)
            {
                base.Fsm.GetOwnerDefaultTarget(this.NguiObject).AddMissingComponent<TweenRotation>();
                this.theTween = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenRotation>();
                this.theTween.from = this.fromPosition.Value;
                this.theTween.to = this.toPosition.Value;
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
            this.fromPosition = Vector3.zero;
            this.toPosition = Vector3.one;
            this.duration = 1f;
            this.delay = 0f;
            this.group = 0;
            this.ignoreTimeScale = 1;
        }
    }
}

