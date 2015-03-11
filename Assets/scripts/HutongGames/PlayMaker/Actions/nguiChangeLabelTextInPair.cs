namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Change Text in Ngui Label in Pairs"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelTextInPair : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Input your input text")]
        public FsmString[] newText;
        [CompoundArray("How Many", "Label Object", "New Text"), Tooltip("Change Text in Label in Pairs"), RequiredField]
        public UILabel[] theLabels;

        private void doChangeText()
        {
            for (int i = 0; i < this.theLabels.Length; i++)
            {
                if (this.theLabels[i] != null)
                {
                    this.theLabels[i].text = this.newText[i].Value;
                }
            }
        }

        public override void OnEnter()
        {
            if ((this.theLabels != null) && (this.theLabels.Length != 0))
            {
                this.doChangeText();
                if (!this.everyFrame)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.doChangeText();
        }

        public override void Reset()
        {
            this.theLabels = null;
            this.newText = null;
            this.everyFrame = false;
        }
    }
}

