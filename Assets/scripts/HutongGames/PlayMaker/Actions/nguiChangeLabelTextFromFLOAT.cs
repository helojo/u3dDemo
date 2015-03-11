namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Change Text in Ngui Label from inputed Float")]
    public class nguiChangeLabelTextFromFLOAT : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Input your input text")]
        public FsmFloat inputFloat;
        [RequiredField, Tooltip("NGUI Label object to change Text")]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeText()
        {
            if (this.theNguiLabel != null)
            {
                this.theNguiLabel.text = this.inputFloat.Value.ToString("F2");
            }
        }

        public override void OnEnter()
        {
            if ((this.NguiLabelObject == null) || (this.inputFloat == null))
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
            this.inputFloat = null;
            this.everyFrame = false;
        }
    }
}

