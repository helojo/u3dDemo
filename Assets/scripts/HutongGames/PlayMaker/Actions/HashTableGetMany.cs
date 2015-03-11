namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Gets items from a PlayMaker HashTable Proxy component"), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableGetMany : HashTableActions
    {
        [Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up"), RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [RequiredField, Tooltip("The Key value for that hash set"), UIHint(UIHint.FsmString), CompoundArray("Count", "Key", "Value"), ActionSection("Data")]
        public FsmString[] keys;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The value for that key"), UIHint(UIHint.Variable)]
        public FsmVar[] results;

        public void Get()
        {
            if (base.isProxyValid())
            {
                for (int i = 0; i < this.keys.Length; i++)
                {
                    if (base.proxy.hashTable.ContainsKey(this.keys[i].Value))
                    {
                        PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, this.results[i], base.proxy.hashTable[this.keys[i].Value]);
                    }
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
            this.keys = null;
            this.results = null;
        }
    }
}

