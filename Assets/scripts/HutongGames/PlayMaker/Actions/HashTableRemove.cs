namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/HashTable"), Tooltip("Remove an item by key ( key/value pairs) in a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy).")]
    public class HashTableRemove : HashTableActions
    {
        [Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), RequiredField, ActionSection("Set up"), CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("The item key in that hashTable"), RequiredField]
        public FsmString key;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void doHashTableRemove()
        {
            if (base.isProxyValid())
            {
                base.proxy.hashTable.Remove(this.key.Value);
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.doHashTableRemove();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.key = null;
        }
    }
}

