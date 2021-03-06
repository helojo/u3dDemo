﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Set an item at a specified index to a PlayMaker array List component")]
    public class ArrayListSet : ArrayListActions
    {
        [Tooltip("The index of the Data in the ArrayList"), UIHint(UIHint.FsmString)]
        public FsmInt atIndex;
        public bool everyFrame;
        [ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)"), UIHint(UIHint.FsmString)]
        public FsmString reference;
        [Tooltip("The variable to add."), ActionSection("Data")]
        public FsmVar variable;

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.SetToArrayList();
            }
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.SetToArrayList();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.variable = null;
            this.everyFrame = false;
        }

        public void SetToArrayList()
        {
            if (base.isProxyValid())
            {
                base.proxy.Set(this.atIndex.Value, PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable), this.variable.Type.ToString());
            }
        }
    }
}

