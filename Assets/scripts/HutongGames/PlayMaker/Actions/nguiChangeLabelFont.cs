namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Change Label Font that you already created in your project using Atlas font maker"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelFont : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Input your new Font"), UIHint(UIHint.FsmGameObject)]
        public UIFont newAtlasFont;
        [UIHint(UIHint.Variable)]
        public Font newTrueTypeFont;
        [RequiredField, Tooltip("NGUI Label to change Font")]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;

        private void doChangeFont()
        {
            if (this.newAtlasFont != null)
            {
                this.theNguiLabel.bitmapFont = this.newAtlasFont;
            }
            if (this.newTrueTypeFont != null)
            {
                this.theNguiLabel.trueTypeFont = this.newTrueTypeFont;
            }
        }

        public override void OnEnter()
        {
            if (this.NguiLabelObject != null)
            {
                this.theNguiLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabelObject).GetComponent<UILabel>();
                if ((this.newAtlasFont == null) && (this.newTrueTypeFont == null))
                {
                    Debug.LogWarning("Font is null");
                }
                else
                {
                    this.doChangeFont();
                    if (!this.everyFrame)
                    {
                        base.Finish();
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            this.doChangeFont();
        }

        public override void Reset()
        {
            this.NguiLabelObject = null;
            this.newAtlasFont = null;
            this.everyFrame = false;
        }
    }
}

