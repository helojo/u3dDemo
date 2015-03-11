namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Gets an item from a PlayMaker ArrayList Proxy component")]
    public class ArrayListGet : ArrayListActions
    {
        [Tooltip("The index to retrieve the item from"), UIHint(UIHint.FsmInt)]
        public FsmInt atIndex;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
        public FsmEvent failureEvent;
        [RequiredField, ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [ActionSection("Result"), UIHint(UIHint.Variable)]
        public FsmVar result;

        public void GetItemAtIndex()
        {
            if (base.isProxyValid())
            {
                object obj2 = null;
                try
                {
                    obj2 = base.proxy.arrayList[this.atIndex.Value];
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message);
                    base.Fsm.Event(this.failureEvent);
                    return;
                }
                PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, this.result, obj2);
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.GetItemAtIndex();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.atIndex = null;
            this.gameObject = null;
            this.failureEvent = null;
            this.result = null;
        }
    }
}

