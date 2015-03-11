namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Add an item to a PlayMaker Array List Proxy component"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListAdd : ArrayListActions
    {
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)"), UIHint(UIHint.FsmString)]
        public FsmString reference;
        [RequiredField, Tooltip("The variable to add."), ActionSection("Data")]
        public FsmVar variable;

        public void AddToArrayList()
        {
            if (base.isProxyValid())
            {
                base.proxy.Add(PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable), this.variable.Type.ToString(), false);
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.AddToArrayList();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.variable = null;
        }
    }
}

