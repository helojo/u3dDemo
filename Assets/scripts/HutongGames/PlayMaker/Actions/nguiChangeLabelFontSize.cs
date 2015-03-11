namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Change Text in Ngui Label"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelFontSize : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Input your input text")]
        public FsmInt newFontSize;
        [RequiredField, Tooltip("NGUI Label to change Text")]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeText()
        {
        }

        public override void OnEnter()
        {
            if (this.NguiLabelObject == null)
            {
                base.Finish();
            }
            this.theNguiLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabelObject).GetComponent<UILabel>();
            this.theNguiLabel.fontSize = this.newFontSize.Value;
            UIWidget component = this.theNguiLabel.GetComponent<UIWidget>();
            component.height = Mathf.Max(component.height, this.theNguiLabel.fontSize);
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
            this.newFontSize = 0x24;
            this.everyFrame = false;
        }
    }
}

