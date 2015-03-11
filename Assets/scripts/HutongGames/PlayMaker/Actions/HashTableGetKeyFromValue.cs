namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;

    [Tooltip("Return the key for a value ofna PlayMaker hashtable Proxy component. It will return the first entry found."), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableGetKeyFromValue : HashTableActions
    {
        [ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger when value is found")]
        public FsmEvent KeyFoundEvent;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger when value is not found")]
        public FsmEvent KeyNotFoundEvent;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [UIHint(UIHint.Variable), Tooltip("The key of that value"), ActionSection("Result")]
        public FsmString result;
        [RequiredField, ActionSection("Value"), Tooltip("The value to search")]
        public FsmVar theValue;

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.SortHashTableByValues();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
        }

        public void SortHashTableByValues()
        {
            if (base.isProxyValid())
            {
                object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.theValue);
                IDictionaryEnumerator enumerator = base.proxy.hashTable.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                        if (current.Value.Equals(valueFromFsmVar))
                        {
                            this.result.Value = (string) current.Key;
                            base.Fsm.Event(this.KeyFoundEvent);
                            return;
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
                base.Fsm.Event(this.KeyNotFoundEvent);
            }
        }
    }
}

