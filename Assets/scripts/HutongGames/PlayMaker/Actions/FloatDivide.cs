namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Math), Tooltip("Divides one Float by another.")]
    public class FloatDivide : FsmStateAction
    {
        [RequiredField, Tooltip("Divide the float variable by this value.")]
        public FsmFloat divideBy;
        [Tooltip("Repeate every frame. Useful if the variables are changing.")]
        public bool everyFrame;
        [Tooltip("The float variable to divide."), UIHint(UIHint.Variable), RequiredField]
        public FsmFloat floatVariable;

        public override void OnEnter()
        {
            this.floatVariable.Value /= this.divideBy.Value;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.floatVariable.Value /= this.divideBy.Value;
        }

        public override void Reset()
        {
            this.floatVariable = null;
            this.divideBy = null;
            this.everyFrame = false;
        }
    }
}

