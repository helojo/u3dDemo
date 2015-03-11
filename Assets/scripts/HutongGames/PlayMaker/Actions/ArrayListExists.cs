namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Check if an ArrayList Proxy component exists.")]
    public class ArrayListExists : ArrayListActions
    {
        [UIHint(UIHint.Variable), ActionSection("Result"), Tooltip("Store in a bool wether it exists or not")]
        public FsmBool doesExists;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this arrayList exists ")]
        public FsmEvent doesExistsEvent;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this arrayList does not exists")]
        public FsmEvent doesNotExistsEvent;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField, ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)"), UIHint(UIHint.FsmString)]
        public FsmString reference;

        public override void OnEnter()
        {
            bool flag = base.GetArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value, true) != null;
            this.doesExists.Value = flag;
            if (flag)
            {
                base.Fsm.Event(this.doesExistsEvent);
            }
            else
            {
                base.Fsm.Event(this.doesNotExistsEvent);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.doesExists = null;
            this.doesExistsEvent = null;
            this.doesNotExistsEvent = null;
        }
    }
}

