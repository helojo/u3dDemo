namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Set the value of a Float Variable in another FSM."), ActionCategory(ActionCategory.StateMachine)]
    public class SetFsmFloat : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;
        private PlayMakerFSM fsm;
        [Tooltip("Optional name of FSM on Game Object"), UIHint(UIHint.FsmName)]
        public FsmString fsmName;
        [Tooltip("The GameObject that owns the FSM."), RequiredField]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [Tooltip("Set the value of the variable."), RequiredField]
        public FsmFloat setValue;
        [RequiredField, UIHint(UIHint.FsmFloat), Tooltip("The name of the FSM variable.")]
        public FsmString variableName;

        private void DoSetFsmFloat()
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
                        FsmFloat fsmFloat = this.fsm.FsmVariables.GetFsmFloat(this.variableName.Value);
                        if (fsmFloat != null)
                        {
                            fsmFloat.Value = this.setValue.Value;
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
            this.DoSetFsmFloat();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetFsmFloat();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fsmName = string.Empty;
            this.setValue = null;
        }
    }
}

