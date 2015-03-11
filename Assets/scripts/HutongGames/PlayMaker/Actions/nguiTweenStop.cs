namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Stop Tween that are currently active"), ActionCategory("Ngui Actions")]
    public class nguiTweenStop : FsmStateAction
    {
        [ActionSection("Do Everyframe")]
        public FsmBool everyframe;
        public FsmOwnerDefault NguiObject;
        public FsmBool stopAlpha;
        public FsmBool stopColor;
        public FsmBool stopPosition;
        [ActionSection("Stop Tween")]
        public FsmBool stopRotate;
        public FsmBool stopScale;
        private TweenAlpha theTweenAlpha;
        private TweenColor theTweenColor;
        private TweenPosition theTweenPos;
        private TweenRotation theTweenRot;
        private TweenScale theTweenScale;

        private void doStopTween()
        {
            if (this.stopRotate.Value)
            {
                this.theTweenRot = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenRotation>();
                if (this.theTweenRot != null)
                {
                    this.theTweenRot.enabled = false;
                }
            }
            if (this.stopPosition.Value)
            {
                this.theTweenPos = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenPosition>();
                if (this.theTweenPos != null)
                {
                    this.theTweenPos.enabled = false;
                }
            }
            if (this.stopColor.Value)
            {
                this.theTweenColor = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenColor>();
                if (this.theTweenColor != null)
                {
                    this.theTweenColor.enabled = false;
                }
            }
            if (this.stopAlpha.Value)
            {
                this.theTweenAlpha = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenAlpha>();
                if (this.theTweenAlpha != null)
                {
                    this.theTweenAlpha.enabled = false;
                }
            }
            if (this.stopScale.Value)
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
                this.doStopTween();
                if (!this.everyframe.Value)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.doStopTween();
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

