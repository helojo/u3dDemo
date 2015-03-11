namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Get current Text in Ngui Label"), ActionCategory("Ngui Actions")]
    public class nguiGetLabelText : FsmStateAction
    {
        public FsmEvent Empty;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("NGUI Label to Get Text"), RequiredField]
        public FsmOwnerDefault NguiLabelObject;
        public FsmEvent notEmpty;
        [UIHint(UIHint.Variable), Tooltip("Store current text")]
        public FsmString storeText;
        private UILabel theNguiLabel;

        private void doGetText()
        {
            this.storeText.Value = this.theNguiLabel.text;
            if (FsmEvent.IsNullOrEmpty(this.Empty) && FsmEvent.IsNullOrEmpty(this.notEmpty))
            {
                base.Finish();
            }
            if (this.theNguiLabel.text == string.Empty)
            {
                base.Fsm.Event(this.Empty);
            }
            else
            {
                base.Fsm.Event(this.notEmpty);
            }
        }

        public override void OnEnter()
        {
            if (this.NguiLabelObject != null)
            {
                this.theNguiLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabelObject).GetComponent<UILabel>();
                this.doGetText();
                if (!this.everyFrame)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.doGetText();
        }

        public override void Reset()
        {
            this.NguiLabelObject = null;
            this.storeText = null;
            this.everyFrame = false;
            this.Empty = null;
            this.notEmpty = null;
        }
    }
}

