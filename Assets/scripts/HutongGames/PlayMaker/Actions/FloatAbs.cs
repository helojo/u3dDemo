namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Sets a Float variable to its absolute value."), ActionCategory(ActionCategory.Math)]
    public class FloatAbs : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the Float variable is changing.")]
        public bool everyFrame;
        [Tooltip("The Float variable."), RequiredField, UIHint(UIHint.Variable)]
        public FsmFloat floatVariable;

        private void DoFloatAbs()
        {
            this.floatVariable.Value = Mathf.Abs(this.floatVariable.Value);
        }

        public override void OnEnter()
        {
            this.DoFloatAbs();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoFloatAbs();
        }

        public override void Reset()
        {
            this.floatVariable = null;
            this.everyFrame = false;
        }
    }
}

