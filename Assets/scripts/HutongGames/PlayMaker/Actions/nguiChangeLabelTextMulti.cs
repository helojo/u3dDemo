namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Change multi Ngui Label text with an input"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelTextMulti : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Input your input text")]
        public FsmString newText;
        [Tooltip("List of NGUI objects to set"), RequiredField]
        public UILabel[] theNguiLabels;

        private void doChangeText()
        {
            if ((this.theNguiLabels != null) && (this.theNguiLabels.Length != 0))
            {
                for (int i = 0; i < this.theNguiLabels.Length; i++)
                {
                    this.theNguiLabels[i].text = this.newText.Value;
                }
            }
        }

        public override void OnEnter()
        {
            if ((this.theNguiLabels == null) || (this.newText == null))
            {
                base.Finish();
            }
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
            this.theNguiLabels = null;
            this.newText = null;
            this.everyFrame = false;
        }
    }
}

