namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Check if a key exists in a PlayMaker HashTable Proxy component (PlayMakerHashTablePRoxy)"), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableContains : HashTableActions
    {
        [Tooltip("Store the result of the test"), UIHint(UIHint.FsmBool)]
        public FsmBool containsKey;
        [RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy)), Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [RequiredField, Tooltip("The Key value to check for"), UIHint(UIHint.FsmString)]
        public FsmString key;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger when key is found"), ActionSection("Result")]
        public FsmEvent keyFoundEvent;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger when key is not found")]
        public FsmEvent keyNotFoundEvent;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void checkIfContainsKey()
        {
            if (base.isProxyValid())
            {
                this.containsKey.Value = base.proxy.hashTable.Contains(this.key.Value);
                if (this.containsKey.Value)
                {
                    base.Fsm.Event(this.keyFoundEvent);
                }
                else
                {
                    base.Fsm.Event(this.keyNotFoundEvent);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.checkIfContainsKey();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.key = null;
            this.containsKey = null;
            this.keyFoundEvent = null;
            this.keyNotFoundEvent = null;
        }
    }
}

