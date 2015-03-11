namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Remove all content of a PlayMaker hashtable Proxy component"), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableClear : HashTableActions
    {
        [CheckForComponent(typeof(PlayMakerHashTableProxy)), ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void ClearHashTable()
        {
            if (base.isProxyValid())
            {
                base.proxy.hashTable.Clear();
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.ClearHashTable();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
        }
    }
}

