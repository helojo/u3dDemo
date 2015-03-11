namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("multi Integer Variables from another FSM."), ActionCategory("Ngui Actions")]
    public class GetFsmIntMulti : FsmStateAction
    {
        public bool everyFrame;
        private PlayMakerFSM fsm;
        private FsmInt fsmInt;
        [Tooltip("Name of FSM on Game Object, if you have more than one so this is important to get the right one"), UIHint(UIHint.FsmName)]
        public FsmString fsmName;
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmInt[] storeValue;
        [RequiredField, UIHint(UIHint.FsmInt), CompoundArray("How Many INT", "variable Name", "Store INT")]
        public FsmString[] variableName;

        private void DoGetFsmInt()
        {
            if (this.storeValue != null)
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                if (ownerDefaultTarget != null)
                {
                    this.fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, this.fsmName.Value);
                    for (int i = 0; i < this.variableName.Length; i++)
                    {
                        if (this.fsm == null)
                        {
                            Debug.LogWarning("no Fsm Name " + this.fsmName.Value);
                            return;
                        }
                        this.fsmInt = this.fsm.FsmVariables.GetFsmInt(this.variableName[i].Value);
                        if (this.fsmInt == null)
                        {
                            Debug.LogWarning("wrong FsmInt Name " + this.variableName[i].Value);
                            return;
                        }
                        this.storeValue[i].Value = this.fsmInt.Value;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetFsmInt();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetFsmInt();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.fsmName = null;
            this.storeValue = null;
        }
    }
}

