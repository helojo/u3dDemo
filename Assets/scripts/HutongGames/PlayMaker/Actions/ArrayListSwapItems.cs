namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Swap two items at a specified indexes of a PlayMaker ArrayList Proxy component"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListSwapItems : ArrayListActions
    {
        [ActionSection("Result"), UIHint(UIHint.FsmEvent), Tooltip("The event to trigger if the removeAt throw errors")]
        public FsmEvent failureEvent;
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmInt), Tooltip("The first index to swap")]
        public FsmInt index1;
        [UIHint(UIHint.FsmInt), Tooltip("The second index to swap")]
        public FsmInt index2;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void doArrayListSwap()
        {
            if (base.isProxyValid())
            {
                try
                {
                    object obj2 = base.proxy.arrayList[this.index2.Value];
                    base.proxy.arrayList[this.index2.Value] = base.proxy.arrayList[this.index1.Value];
                    base.proxy.arrayList[this.index1.Value] = obj2;
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
                this.doArrayListSwap();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.failureEvent = null;
            this.reference = null;
            this.index1 = null;
            this.index2 = null;
        }
    }
}

