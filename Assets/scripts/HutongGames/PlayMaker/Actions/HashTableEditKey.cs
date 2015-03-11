namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Edit a key from a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)"), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableEditKey : HashTableActions
    {
        [Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up"), RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [RequiredField, Tooltip("The Key value to edit"), UIHint(UIHint.FsmString)]
        public FsmString key;
        [Tooltip("Event sent if this HashTable key does not exists"), UIHint(UIHint.FsmEvent), ActionSection("Result")]
        public FsmEvent keyNotFoundEvent;
        [UIHint(UIHint.FsmString), RequiredField, Tooltip("The Key value to edit")]
        public FsmString newKey;
        [Tooltip("Event sent if this HashTable already contains the new key"), UIHint(UIHint.FsmEvent)]
        public FsmEvent newKeyExistsAlreadyEvent;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void EditHashTableKey()
        {
            if (base.isProxyValid())
            {
                if (!base.proxy.hashTable.ContainsKey(this.key.Value))
                {
                    base.Fsm.Event(this.keyNotFoundEvent);
                }
                else if (base.proxy.hashTable.ContainsKey(this.newKey.Value))
                {
                    base.Fsm.Event(this.newKeyExistsAlreadyEvent);
                }
                else
                {
                    object obj2 = base.proxy.hashTable[this.key.Value];
                    base.proxy.hashTable[this.newKey.Value] = obj2;
                    base.proxy.hashTable.Remove(this.key.Value);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.EditHashTableKey();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.key = null;
            this.newKey = null;
            this.keyNotFoundEvent = null;
            this.newKeyExistsAlreadyEvent = null;
        }
    }
}

