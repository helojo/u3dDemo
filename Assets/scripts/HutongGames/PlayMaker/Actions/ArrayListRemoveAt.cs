namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Remove item at a specified index of a PlayMaker ArrayList Proxy component")]
    public class ArrayListRemoveAt : ArrayListActions
    {
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger if the removeAt throw errors"), ActionSection("Result")]
        public FsmEvent failureEvent;
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), RequiredField, ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmInt), Tooltip("The index to remove at")]
        public FsmInt index;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void doArrayListRemoveAt()
        {
            if (base.isProxyValid())
            {
                try
                {
                    base.proxy.arrayList.RemoveAt(this.index.Value);
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    base.Fsm.Event(this.failureEvent);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.doArrayListRemoveAt();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.failureEvent = null;
            this.reference = null;
            this.index = null;
        }
    }
}

