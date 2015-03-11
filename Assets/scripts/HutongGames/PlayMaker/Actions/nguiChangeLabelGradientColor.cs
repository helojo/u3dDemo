namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Change Color in Ngui Label"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelGradientColor : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        public FsmEvent finishEvent;
        [Tooltip("Input your gradientBottomColor Color")]
        public FsmColor gradientBottomColor;
        [Tooltip("Input your gradientTopColor Color")]
        public FsmColor gradientTopColor;
        [RequiredField, Tooltip("NGUI Label to change Color")]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeColor()
        {
            this.theNguiLabel.gradientTop = this.gradientTopColor.Value;
            this.theNguiLabel.gradientBottom = this.gradientBottomColor.Value;
        }

        public override void OnEnter()
        {
            if ((this.NguiLabelObject == null) || (this.gradientBottomColor == null))
            {
                base.Finish();
            }
            this.theNguiLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabelObject).GetComponent<UILabel>();
            this.doChangeColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
            if (this.finishEvent != null)
            {
                base.Fsm.Event(this.finishEvent);
            }
        }

        public override void OnUpdate()
        {
            this.doChangeColor();
        }

        public override void Reset()
        {
            this.NguiLabelObject = null;
            this.gradientBottomColor = null;
            this.gradientTopColor = null;
            this.finishEvent = null;
        }
    }
}

