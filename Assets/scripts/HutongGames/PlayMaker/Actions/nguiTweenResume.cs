namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Resume Tween that has been stop from previous state")]
    public class nguiTweenResume : FsmStateAction
    {
        [ActionSection("Do Everyframe")]
        public FsmBool everyframe;
        public FsmOwnerDefault NguiObject;
        public FsmBool resumeAlpha;
        public FsmBool resumeColor;
        public FsmBool resumePosition;
        [ActionSection("Resume Tween")]
        public FsmBool resumeRotate;
        public FsmBool resumeScale;
        private TweenAlpha theTweenAlpha;
        private TweenColor theTweenColor;
        private TweenPosition theTweenPos;
        private TweenRotation theTweenRot;
        private TweenScale theTweenScale;

        private void doResumeTween()
        {
            if (this.resumeRotate.Value)
            {
                this.theTweenRot = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenRotation>();
                if (this.theTweenRot != null)
                {
                    this.theTweenRot.enabled = true;
                }
            }
            if (this.resumePosition.Value)
            {
                this.theTweenPos = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenPosition>();
                if (this.theTweenPos != null)
                {
                    this.theTweenPos.enabled = true;
                }
            }
            if (this.resumeColor.Value)
            {
                this.theTweenColor = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenColor>();
                if (this.theTweenColor != null)
                {
                    this.theTweenColor.enabled = true;
                }
            }
            if (this.resumeAlpha.Value)
            {
                this.theTweenAlpha = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenAlpha>();
                if (this.theTweenAlpha != null)
                {
                    this.theTweenAlpha.enabled = true;
                }
            }
            if (this.resumeScale.Value)
            {
                this.theTweenScale = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenScale>();
                if (this.theTweenScale != null)
                {
                    this.theTweenScale.enabled = true;
                }
            }
        }

        public override void OnEnter()
        {
            if (this.NguiObject != null)
            {
                this.doResumeTween();
                if (!this.everyframe.Value)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.doResumeTween();
        }

        public override void Reset()
        {
            this.NguiObject = null;
            this.theTweenPos = null;
            this.theTweenRot = null;
            this.theTweenColor = null;
            this.theTweenAlpha = null;
            this.theTweenScale = null;
        }
    }
}

