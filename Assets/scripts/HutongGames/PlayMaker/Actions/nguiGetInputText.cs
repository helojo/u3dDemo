namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Get current inputed Text in Ngui UIInput"), ActionCategory("Ngui Actions")]
    public class nguiGetInputText : FsmStateAction
    {
        public FsmEvent Empty;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [RequiredField, Tooltip("NGUI UIInput to Get Text")]
        public FsmOwnerDefault NguiUIinputObject;
        public FsmEvent notEmpty;
        [UIHint(UIHint.Variable), Tooltip("Store current inputed text")]
        public FsmString storeText;
        private UIInput theNguiUIinput;

        private void doGetText()
        {
            this.storeText.Value = this.theNguiUIinput.value;
            if (FsmEvent.IsNullOrEmpty(this.Empty) && FsmEvent.IsNullOrEmpty(this.notEmpty))
            {
                base.Finish();
            }
            if (this.theNguiUIinput.value == string.Empty)
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
            if (this.NguiUIinputObject != null)
            {
                this.theNguiUIinput = base.Fsm.GetOwnerDefaultTarget(this.NguiUIinputObject).GetComponent<UIInput>();
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
            this.NguiUIinputObject = null;
            this.storeText = null;
            this.everyFrame = false;
            this.Empty = null;
            this.notEmpty = null;
        }
    }
}

