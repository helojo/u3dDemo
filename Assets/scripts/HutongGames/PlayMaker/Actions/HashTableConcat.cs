namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;

    [Tooltip("Concat joins two or more hashTable proxy components. if a target is specified, the method use the target store the concatenation, else the "), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableConcat : HashTableActions
    {
        [ActionSection("Storage"), Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [RequiredField, CompoundArray("HashTables", "HashTable GameObject", "Reference"), Tooltip("The GameObject with the PlayMaker HashTable Proxy component to copy to"), ActionSection("HashTables to concatenate"), ObjectType(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault[] hashTableGameObjectTargets;
        [UIHint(UIHint.FsmBool), Tooltip("Overwrite existing key with new values")]
        public FsmBool overwriteExistingKey;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component to store the concatenation ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy to ( necessary if several component coexists on the same GameObject")]
        public FsmString[] referenceTargets;

        public void DoHashTableConcat(Hashtable source)
        {
            if (base.isProxyValid())
            {
                for (int i = 0; i < this.hashTableGameObjectTargets.Length; i++)
                {
                    if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.hashTableGameObjectTargets[i]), this.referenceTargets[i].Value) && base.isProxyValid())
                    {
                        IEnumerator enumerator = base.proxy.hashTable.Keys.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                object current = enumerator.Current;
                                if (source.ContainsKey(current))
                                {
                                    if (this.overwriteExistingKey.Value)
                                    {
                                        source[current] = base.proxy.hashTable[current];
                                    }
                                }
                                else
                                {
                                    source[current] = base.proxy.hashTable[current];
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator as IDisposable;
                            if (disposable == null)
                            {
                            }
                            disposable.Dispose();
                        }
                    }
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoHashTableConcat(base.proxy.hashTable);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.hashTableGameObjectTargets = null;
            this.referenceTargets = null;
            this.overwriteExistingKey = null;
        }
    }
}

