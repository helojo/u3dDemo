namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/HashTable"), Tooltip("Gets an item from a PlayMaker HashTable Proxy component")]
    public class HashTableGet : HashTableActions
    {
        [CheckForComponent(typeof(PlayMakerHashTableProxy)), Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up"), RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("The Key value for that hash set"), RequiredField, UIHint(UIHint.FsmString)]
        public FsmString key;
        [Tooltip("The event to trigger when key is found"), UIHint(UIHint.FsmEvent)]
        public FsmEvent KeyFoundEvent;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger when key is not found")]
        public FsmEvent KeyNotFoundEvent;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [ActionSection("Result"), UIHint(UIHint.Variable)]
        public FsmVar result;

        public void Get()
        {
            if (base.isProxyValid())
            {
                if (!base.proxy.hashTable.ContainsKey(this.key.Value))
                {
                    base.Fsm.Event(this.KeyNotFoundEvent);
                }
                else
                {
                    PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, this.result, base.proxy.hashTable[this.key.Value]);
                    base.Fsm.Event(this.KeyFoundEvent);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.Get();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.key = null;
            this.KeyFoundEvent = null;
            this.KeyNotFoundEvent = null;
            this.result = null;
        }
    }
}

