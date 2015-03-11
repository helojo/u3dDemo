namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Change Text in Ngui Label"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelText : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Input your input text")]
        public FsmString newText;
        [RequiredField, Tooltip("NGUI Label to change Text")]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeText()
        {
            this.theNguiLabel.text = this.newText.Value;
        }

        public override void OnEnter()
        {
            if ((this.NguiLabelObject == null) || (this.newText == null))
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
            this.newText = null;
            this.everyFrame = false;
        }
    }
}

