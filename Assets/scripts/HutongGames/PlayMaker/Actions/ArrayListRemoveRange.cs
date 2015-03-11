namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Remove the specified range of elements from a PlayMaker ArrayList Proxy component")]
    public class ArrayListRemoveRange : ArrayListActions
    {
        [UIHint(UIHint.FsmInt), Tooltip("The number of elements to remove. This value is between 0 and the difference between the array.count minus the index ( inclusive )")]
        public FsmInt count;
        [Tooltip("The event to trigger if the removeRange throw errors"), UIHint(UIHint.FsmEvent), ActionSection("Result")]
        public FsmEvent failureEvent;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("The zero-based index of the first element of the range of elements to remove. This value is between 0 and the array.count minus count (inclusive)"), UIHint(UIHint.FsmInt)]
        public FsmInt index;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void doArrayListRemoveRange()
        {
            if (base.isProxyValid())
            {
                try
                {
                    base.proxy.arrayList.RemoveRange(this.index.Value, this.count.Value);
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
                this.doArrayListRemoveRange();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.index = null;
            this.count = null;
            this.failureEvent = null;
        }
    }
}

