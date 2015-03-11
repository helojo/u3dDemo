namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("Ngui Actions"), Tooltip("Set the value of multi in Pair Integer Variable in another FSM.")]
    public class SetFsmIntMulti : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;
        private PlayMakerFSM fsm;
        [UIHint(UIHint.FsmName), Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;
        [RequiredField, Tooltip("The GameObject that owns the FSM.")]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [RequiredField, Tooltip("Set the value of the variable.")]
        public FsmInt[] setValue;
        [Tooltip("The name of the FSM variable."), CompoundArray("How Many INT", "variable Name", "Set INT"), RequiredField, UIHint(UIHint.FsmInt)]
        public FsmString[] variableName;

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
                    for (int i = 0; i < this.variableName.Length; i++)
                    {
                        if (this.fsm == null)
                        {
                            this.LogWarning("Could not find FSM: " + this.fsmName.Value);
                            return;
                        }
                        FsmInt fsmInt = this.fsm.FsmVariables.GetFsmInt(this.variableName[i].Value);
                        if (fsmInt != null)
                        {
                            fsmInt.Value = this.setValue[i].Value;
                        }
                        else
                        {
                            this.LogWarning("Could not find variable: " + this.variableName[i].Value);
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
            this.fsmName = null;
            this.setValue = null;
        }
    }
}

