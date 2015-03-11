namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Set the value of an Integer Variable in another FSM."), ActionCategory(ActionCategory.StateMachine)]
    public class SetFsmInt : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;
        private PlayMakerFSM fsm;
        [UIHint(UIHint.FsmName), Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;
        [Tooltip("The GameObject that owns the FSM."), RequiredField]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [RequiredField, Tooltip("Set the value of the variable.")]
        public FsmInt setValue;
        [UIHint(UIHint.FsmInt), RequiredField, Tooltip("The name of the FSM variable.")]
        public FsmString variableName;

        private void DoSetFsmInt()
        {
            if (this.setValue != null)
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                if (ownerDefaultTarget != null)
                {
                    if (ownerDefaultTarget != this.goLastFrame)
                    {
                        this.goLastFrame = ownerDefaultTarget;
                        this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
                    }
                    if (this.fsm == null)
                    {
                        this.LogWarning("Could not find FSM: " + this.fsmName.Value);
                    }
                    else
                    {
                        FsmInt fsmInt = this.fsm.FsmVariables.GetFsmInt(this.variableName.Value);
                        if (fsmInt != null)
                        {
                            fsmInt.Value = this.setValue.Value;
                        }
                        else
                        {
                            this.LogWarning("Could not find variable: " + this.variableName.Value);
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetFsmInt();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetFsmInt();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fsmName = string.Empty;
            this.setValue = null;
        }
    }
}

