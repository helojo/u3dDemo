namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/HashTable"), Tooltip("Check if an HashTable Proxy component exists.")]
    public class HashTableExists : ArrayListActions
    {
        [UIHint(UIHint.Variable), Tooltip("Store in a bool wether it exists or not"), ActionSection("Result")]
        public FsmBool doesExists;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this HashTable exists ")]
        public FsmEvent doesExistsEvent;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this HashTable does not exists")]
        public FsmEvent doesNotExistsEvent;
        [RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public override void OnEnter()
        {
            bool flag = base.GetHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value, true) != null;
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

