namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Math), Tooltip("Sets a Int variable to its absolute value.")]
    public class IntAbs : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the Float variable is changing.")]
        public bool everyFrame;
        [UIHint(UIHint.Variable), RequiredField, Tooltip("The Int variable.")]
        public FsmInt intVariable;

        private void DoFloatAbs()
        {
            this.intVariable.Value = Mathf.Abs(this.intVariable.Value);
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
            this.intVariable = null;
            this.everyFrame = false;
        }
    }
}

