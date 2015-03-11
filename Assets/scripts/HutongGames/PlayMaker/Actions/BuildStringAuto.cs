namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Builds a String from other variables with color setting in each part.")]
    public class BuildStringAuto : FsmStateAction
    {
        [Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;
        private string result;
        [Tooltip("Separator to insert between each String. E.g. space character.")]
        public FsmString separator;
        [RequiredField, UIHint(UIHint.Variable), Tooltip("Store the final String in a variable.")]
        public FsmString storeResult;
        public FsmColor[] stringColor;
        [RequiredField, CompoundArray("How Many", "From Varriable", "Color"), Tooltip("Array of Strings to combine.")]
        public FsmVar[] stringParts;

        private void DoBuildString()
        {
            if (this.storeResult != null)
            {
                this.result = string.Empty;
                for (int i = 0; i < this.stringParts.Length; i++)
                {
                    string result = this.result;
                    string[] textArray1 = new string[] { result, "[", NGUIText.EncodeColor(this.stringColor[i].Value), "]", this.stringParts[i].ToString(), "[-]" };
                    this.result = string.Concat(textArray1);
                    this.result = this.result + this.separator.Value;
                }
                this.storeResult.Value = this.result;
            }
        }

        public override void OnEnter()
        {
            this.DoBuildString();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoBuildString();
        }

        public override void Reset()
        {
            this.stringParts = null;
            this.stringColor = null;
            this.separator = null;
            this.storeResult = null;
            this.everyFrame = false;
        }
    }
}

