namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Add an key/value pair to a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)."), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableAdd : HashTableActions
    {
        [RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy)), Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [RequiredField, UIHint(UIHint.FsmString), ActionSection("Data"), Tooltip("The Key value for that hash set")]
        public FsmString key;
        [Tooltip("The event to trigger when element exists already"), UIHint(UIHint.FsmEvent)]
        public FsmEvent keyExistsAlreadyEvent;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [UIHint(UIHint.FsmEvent), ActionSection("Result"), Tooltip("The event to trigger when element is added")]
        public FsmEvent successEvent;
        [Tooltip("The variable to add."), RequiredField]
        public FsmVar variable;

        public void AddToHashTable()
        {
            if (base.isProxyValid())
            {
                base.proxy.hashTable.Add(this.key.Value, PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable));
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                if (base.proxy.hashTable.ContainsKey(this.key.Value))
                {
                    base.Fsm.Event(this.keyExistsAlreadyEvent);
                }
                else
                {
                    this.AddToHashTable();
                    base.Fsm.Event(this.successEvent);
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.key = null;
            this.variable = null;
            this.successEvent = null;
            this.keyExistsAlreadyEvent = null;
        }
    }
}

