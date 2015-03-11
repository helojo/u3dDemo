namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Check if an HashTable Proxy component is empty."), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableIsEmpty : ArrayListActions
    {
        [RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), ActionSection("Result"), Tooltip("Store in a bool wether it is empty or not")]
        public FsmBool isEmpty;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this HashTable is empty ")]
        public FsmEvent isEmptyEvent;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this HashTable is not empty")]
        public FsmEvent isNotEmptyEvent;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public override void OnEnter()
        {
            bool flag = base.GetHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value, true).hashTable.Count == 0;
            this.isEmpty.Value = flag;
            if (flag)
            {
                base.Fsm.Event(this.isEmptyEvent);
            }
            else
            {
                base.Fsm.Event(this.isNotEmptyEvent);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.isEmpty = null;
            this.isEmptyEvent = null;
            this.isNotEmptyEvent = null;
        }
    }
}

