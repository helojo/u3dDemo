namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD"), Tooltip("Converts an Integer value to a String value with an optional format.")]
    public class ConvertIntToTime : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the Int variable is changing.")]
        public bool everyFrame;
        [Tooltip("The Int variable to convert."), RequiredField, UIHint(UIHint.Variable)]
        public FsmInt intVariable;
        [UIHint(UIHint.Variable), Tooltip("A String variable to store the converted value."), RequiredField]
        public FsmString stringVariable;

        private void DoConvertIntToString()
        {
            int num = this.intVariable.Value / 60;
            int num2 = this.intVariable.Value % 60;
            this.stringVariable.Value = string.Format("{0:00}:{1:00}", num, num2);
        }

        public override void OnEnter()
        {
            this.DoConvertIntToString();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoConvertIntToString();
        }

        public override void Reset()
        {
            this.intVariable = null;
            this.stringVariable = null;
            this.everyFrame = false;
        }
    }
}

