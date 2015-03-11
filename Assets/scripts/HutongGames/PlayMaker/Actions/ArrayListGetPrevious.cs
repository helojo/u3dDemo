namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Each time this action is called it gets the previous item from a PlayMaker ArrayList Proxy component. \nThis lets you quickly loop backward through all the children of an object to perform actions on them.\nNOTE: To get to specific item use ArrayListGet instead.")]
    public class ArrayListGetPrevious : ArrayListActions
    {
        private int countBase;
        [UIHint(UIHint.Variable), ActionSection("Result")]
        public FsmInt currentIndex;
        [Tooltip("When to end iteration, leave to 0 to iterate until the beginning, index is relative to the last item.")]
        public FsmInt endIndex;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
        public FsmEvent failureEvent;
        [Tooltip("Event to send when there are no more items.")]
        public FsmEvent finishedEvent;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Event to send to get the previous item.")]
        public FsmEvent loopEvent;
        private int nextItemIndex;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Set to true to force iterating from the last item. This variable will be set to false as it carries on iterating, force it back to true if you want to renter this action back to the last item."), UIHint(UIHint.Variable)]
        public FsmBool reset;
        [UIHint(UIHint.Variable)]
        public FsmVar result;
        [Tooltip("From where to start iteration, leave to 0 to start from the end. index is relative to the last item, so if the start index is 2, this will start 2 items before the last item.")]
        public FsmInt startIndex;

        private void DoGetPreviousItem()
        {
            if (this.nextItemIndex > this.countBase)
            {
                this.nextItemIndex = 0;
                base.Fsm.Event(this.finishedEvent);
            }
            else
            {
                this.GetItemAtIndex();
                if (this.nextItemIndex >= this.countBase)
                {
                    this.nextItemIndex = 0;
                    base.Fsm.Event(this.finishedEvent);
                }
                else if ((this.endIndex.Value > 0) && (this.nextItemIndex > (this.countBase - this.endIndex.Value)))
                {
                    this.nextItemIndex = 0;
                    base.Fsm.Event(this.finishedEvent);
                }
                else
                {
                    this.nextItemIndex++;
                    if (this.loopEvent != null)
                    {
                        base.Fsm.Event(this.loopEvent);
                    }
                }
            }
        }

        public void GetItemAtIndex()
        {
            if (base.isProxyValid())
            {
                this.currentIndex.Value = this.countBase - this.nextItemIndex;
                object obj2 = null;
                try
                {
                    obj2 = base.proxy.arrayList[this.currentIndex.Value];
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    base.Fsm.Event(this.failureEvent);
                    return;
                }
                PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, this.result, obj2);
            }
        }

        public override void OnEnter()
        {
            if (this.reset.Value)
            {
                this.reset.Value = false;
                this.nextItemIndex = 0;
            }
            if (this.nextItemIndex == 0)
            {
                if (!base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
                {
                    base.Fsm.Event(this.failureEvent);
                    base.Finish();
                }
                this.countBase = base.proxy.arrayList.Count - 1;
                if (this.startIndex.Value > 0)
                {
                    this.nextItemIndex = this.startIndex.Value;
                }
            }
            this.DoGetPreviousItem();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.reset = null;
            this.startIndex = null;
            this.endIndex = null;
            this.loopEvent = null;
            this.finishedEvent = null;
            this.failureEvent = null;
            this.currentIndex = null;
            this.result = null;
        }
    }
}

