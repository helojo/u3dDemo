namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Change Alpha in Ngui Label. This will override Alpha Color"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelAlpha : FsmStateAction
    {
        [Tooltip("Input your input Alpha"), HasFloatSlider(0f, 1f)]
        public FsmFloat AlphaValue;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [RequiredField, Tooltip("NGUI Label to change Alpha")]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeAlPha()
        {
            this.theNguiLabel.alpha = this.AlphaValue.Value;
        }

        public override void OnEnter()
        {
            if ((this.NguiLabelObject == null) || (this.AlphaValue == null))
            {
                base.Finish();
            }
            this.theNguiLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabelObject).GetComponent<UILabel>();
            this.doChangeAlPha();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.doChangeAlPha();
        }

        public override void Reset()
        {
            this.NguiLabelObject = null;
            this.AlphaValue = null;
            this.everyFrame = false;
        }
    }
}

