namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Change Color in Ngui Label")]
    public class nguiChangeLabelColor : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Input your input Color")]
        public FsmColor newColor;
        [Tooltip("NGUI Label to change Color"), RequiredField]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeColor()
        {
            this.theNguiLabel.color = this.newColor.Value;
        }

        public override void OnEnter()
        {
            if ((this.NguiLabelObject == null) || (this.newColor == null))
            {
                base.Finish();
            }
            this.theNguiLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabelObject).GetComponent<UILabel>();
            this.doChangeColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.doChangeColor();
        }

        public override void Reset()
        {
            this.NguiLabelObject = null;
            this.newColor = null;
            this.everyFrame = false;
        }
    }
}

