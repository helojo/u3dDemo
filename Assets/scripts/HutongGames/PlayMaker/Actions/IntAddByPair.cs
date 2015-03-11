namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Adds a value to an Integer Variable in pair."), ActionCategory("Ngui Actions")]
    public class IntAddByPair : FsmStateAction
    {
        [RequiredField]
        public FsmInt[] add;
        public bool everyFrame;
        [RequiredField, UIHint(UIHint.Variable), CompoundArray("How Many", "INT Variable", "Add")]
        public FsmInt[] intVariable;

        private void doAddInt()
        {
            for (int i = 0; i < this.intVariable.Length; i++)
            {
                FsmInt num1 = this.intVariable[i];
                num1.Value += this.add[i].Value;
            }
        }

        public override void OnEnter()
        {
            this.doAddInt();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.doAddInt();
        }

        public override void Reset()
        {
            this.intVariable = null;
            this.add = null;
            this.everyFrame = false;
        }
    }
}

