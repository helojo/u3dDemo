namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Change Text in Ngui Label from inputed variable that can be type of variable"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelTextFromVar : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Type of Var will be automatically convert to string"), UIHint(UIHint.Variable)]
        public FsmVar inputVar;
        [Tooltip("NGUI Label to change Text"), RequiredField]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeText()
        {
            this.theNguiLabel.text = this.inputVar.ToString();
        }

        public override void OnEnter()
        {
            if ((this.NguiLabelObject == null) || (this.inputVar == null))
            {
                base.Finish();
            }
            this.theNguiLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabelObject).GetComponent<UILabel>();
            this.doChangeText();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.doChangeText();
        }

        public override void Reset()
        {
            this.NguiLabelObject = null;
            this.inputVar = null;
            this.everyFrame = false;
        }
    }
}

