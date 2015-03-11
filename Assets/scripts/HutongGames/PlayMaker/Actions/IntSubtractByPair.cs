namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Subtract a value to an Integer Variable in pair."), ActionCategory("Ngui Actions")]
    public class IntSubtractByPair : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField, CompoundArray("How Many", "INT Variable", "Subtract"), UIHint(UIHint.Variable)]
        public FsmInt[] intVariable;
        [RequiredField]
        public FsmInt[] Subtract;

        private void doSubtractInt()
        {
            for (int i = 0; i < this.intVariable.Length; i++)
            {
                FsmInt num1 = this.intVariable[i];
                num1.Value -= this.Subtract[i].Value;
            }
        }

        public override void OnEnter()
        {
            this.doSubtractInt();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.doSubtractInt();
        }

        public override void Reset()
        {
            this.intVariable = null;
            this.Subtract = null;
            this.everyFrame = false;
        }
    }
}

