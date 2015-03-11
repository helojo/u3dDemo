namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Change Text in Ngui Label from inputed INT")]
    public class nguiChangeLabelTextFromINT : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Input your input text")]
        public FsmInt inputINT;
        [Tooltip("NGUI Label to change Text"), RequiredField]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeText()
        {
            this.theNguiLabel.text = this.inputINT.Value.ToString();
        }

        public override void OnEnter()
        {
            if ((this.NguiLabelObject == null) || (this.inputINT == null))
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
            this.inputINT = null;
            this.everyFrame = false;
        }
    }
}

