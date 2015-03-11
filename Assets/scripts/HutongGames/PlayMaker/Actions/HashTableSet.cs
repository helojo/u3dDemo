namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/HashTable"), Tooltip("Set an key/value pair to a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)")]
    public class HashTableSet : HashTableActions
    {
        [CheckForComponent(typeof(PlayMakerHashTableProxy)), ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmString), Tooltip("The Key value for that hash set"), RequiredField]
        public FsmString key;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The variable to set."), ActionSection("Result")]
        public FsmVar variable;

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
            this.key = null;
            this.variable = null;
        }

        public void SetHashTable()
        {
            if (base.isProxyValid())
            {
                base.proxy.hashTable[this.key.Value] = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable);
            }
        }
    }
}

