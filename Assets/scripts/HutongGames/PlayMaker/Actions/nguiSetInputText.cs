namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Set inputed Text in current Ngui UIInput"), ActionCategory("Ngui Actions")]
    public class nguiSetInputText : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("NGUI UIInput to Set Text"), RequiredField]
        public FsmOwnerDefault NguiUIinputObject;
        [Tooltip("Store current inputed text"), UIHint(UIHint.FsmString)]
        public FsmString setText;
        private UIInput theNguiUIinput;

        private void doSetText()
        {
            this.theNguiUIinput.value = this.setText.Value;
        }

        public override void OnEnter()
        {
            if (this.NguiUIinputObject != null)
            {
                this.theNguiUIinput = base.Fsm.GetOwnerDefaultTarget(this.NguiUIinputObject).GetComponent<UIInput>();
                if (this.theNguiUIinput != null)
                {
                    this.doSetText();
                }
                else
                {
                    base.Finish();
                }
                if (!this.everyFrame)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.doSetText();
        }

        public override void Reset()
        {
            this.NguiUIinputObject = null;
            this.setText = null;
            this.everyFrame = false;
        }
    }
}

