namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Set key/value pairs to a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)"), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableSetMany : HashTableActions
    {
        [CheckForComponent(typeof(PlayMakerHashTableProxy)), ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmString), ActionSection("Data"), CompoundArray("Count", "Key", "Value"), RequiredField, Tooltip("The Key values for that hash set")]
        public FsmString[] keys;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The variable to set.")]
        public FsmVar[] variables;

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.SetHashTable();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.keys = null;
            this.variables = null;
        }

        public void SetHashTable()
        {
            if (base.isProxyValid())
            {
                for (int i = 0; i < this.keys.Length; i++)
                {
                    base.proxy.hashTable[this.keys[i].Value] = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variables[i]);
                }
            }
        }
    }
}

