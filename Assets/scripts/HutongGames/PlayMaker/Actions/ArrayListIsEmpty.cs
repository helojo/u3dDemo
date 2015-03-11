namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Check if an ArrayList Proxy component is empty.")]
    public class ArrayListIsEmpty : ArrayListActions
    {
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        public FsmOwnerDefault gameObject;
        [ActionSection("Result"), Tooltip("Store in a bool wether it is empty or not"), UIHint(UIHint.Variable)]
        public FsmBool isEmpty;
        [Tooltip("Event sent if this arrayList is empty "), UIHint(UIHint.FsmEvent)]
        public FsmEvent isEmptyEvent;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this arrayList is not empty")]
        public FsmEvent isNotEmptyEvent;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)"), UIHint(UIHint.FsmString)]
        public FsmString reference;

        public override void OnEnter()
        {
            bool flag = base.GetArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value, true).arrayList.Count == 0;
            this.isEmpty.Value = flag;
            if (flag)
            {
                base.Fsm.Event(this.isEmptyEvent);
            }
            else
            {
                base.Fsm.Event(this.isNotEmptyEvent);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.isEmpty = null;
            this.isNotEmptyEvent = null;
            this.isEmptyEvent = null;
        }
    }
}

