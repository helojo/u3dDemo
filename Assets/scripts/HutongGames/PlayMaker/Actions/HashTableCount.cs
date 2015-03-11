namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Count the number of items ( key/value pairs) in a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)."), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableCount : HashTableActions
    {
        [Tooltip("The number of items in that hashTable"), UIHint(UIHint.Variable), RequiredField, ActionSection("Result")]
        public FsmInt count;
        [Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up"), CheckForComponent(typeof(PlayMakerHashTableProxy)), RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void doHashTableCount()
        {
            if (base.isProxyValid())
            {
                this.count.Value = base.proxy.hashTable.Count;
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.doHashTableCount();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.count = null;
        }
    }
}

