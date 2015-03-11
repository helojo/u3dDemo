namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Math), Tooltip("Sets the value of an integer variable using a float value.")]
    public class SetIntFromFloat : FsmStateAction
    {
        public bool everyFrame;
        public FsmFloat floatValue;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmInt intVariable;

        public override void OnEnter()
        {
            this.intVariable.Value = (int) this.floatValue.Value;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.intVariable.Value = (int) this.floatValue.Value;
        }

        public override void Reset()
        {
            this.intVariable = null;
            this.floatValue = null;
            this.everyFrame = false;
        }
    }
}

