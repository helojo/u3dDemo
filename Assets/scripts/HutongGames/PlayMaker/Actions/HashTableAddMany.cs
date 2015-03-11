namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Add key/value pairs to a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)."), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableAddMany : HashTableActions
    {
        [RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy)), Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger when elements exists already")]
        public FsmEvent keyExistsAlreadyEvent;
        [UIHint(UIHint.FsmString), RequiredField, Tooltip("The Key"), ActionSection("Data"), CompoundArray("Count", "Key", "Value")]
        public FsmString[] keys;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The event to trigger when elements are added"), UIHint(UIHint.FsmEvent), ActionSection("Result")]
        public FsmEvent successEvent;
        [Tooltip("The value for that key"), RequiredField]
        public FsmVar[] variables;

        public void AddToHashTable()
        {
            if (base.isProxyValid())
            {
                for (int i = 0; i < this.keys.Length; i++)
                {
                    base.proxy.hashTable.Add(this.keys[i].Value, PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variables[i]));
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                if (this.keyExistsAlreadyEvent != null)
                {
                    foreach (FsmString str in this.keys)
                    {
                        if (base.proxy.hashTable.ContainsKey(str.Value))
                        {
                            base.Fsm.Event(this.keyExistsAlreadyEvent);
                            base.Finish();
                        }
                    }
                }
                this.AddToHashTable();
                base.Fsm.Event(this.successEvent);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.keys = null;
            this.variables = null;
            this.successEvent = null;
            this.keyExistsAlreadyEvent = null;
        }
    }
}

