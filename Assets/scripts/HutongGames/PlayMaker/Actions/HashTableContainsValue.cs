namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Check if a value exists in a PlayMaker HashTable Proxy component (PlayMakerHashTablePRoxy)"), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableContainsValue : HashTableActions
    {
        [UIHint(UIHint.Variable), ActionSection("Result"), Tooltip("Store the result of the test")]
        public FsmBool containsValue;
        [RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy)), Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component (necessary if several component coexists on the same GameObject)")]
        public FsmString reference;
        [Tooltip("The event to trigger when value is found"), UIHint(UIHint.FsmEvent)]
        public FsmEvent valueFoundEvent;
        [Tooltip("The event to trigger when value is not found"), UIHint(UIHint.FsmEvent)]
        public FsmEvent valueNotFoundEvent;
        [Tooltip("The variable to check for.")]
        public FsmVar variable;

        public void doContainsValue()
        {
            if (base.isProxyValid())
            {
                this.containsValue.Value = base.proxy.hashTable.ContainsValue(PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable));
                if (this.containsValue.Value)
                {
                    base.Fsm.Event(this.valueFoundEvent);
                }
                else
                {
                    base.Fsm.Event(this.valueNotFoundEvent);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.doContainsValue();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.containsValue = null;
            this.valueFoundEvent = null;
            this.valueNotFoundEvent = null;
            this.variable = null;
        }
    }
}

