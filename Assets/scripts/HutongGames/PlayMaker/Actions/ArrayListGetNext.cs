namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Each time this action is called it gets the next item from a PlayMaker ArrayList Proxy component. \nThis lets you quickly loop through all the children of an object to perform actions on them.\nNOTE: To get to specific item use ArrayListGet instead."), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListGetNext : ArrayListActions
    {
        [Tooltip("The current index."), ActionSection("Result"), UIHint(UIHint.Variable)]
        public FsmInt currentIndex;
        [Tooltip("When to end iteration, leave to 0 to iterate until the end")]
        public FsmInt endIndex;
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
        public FsmEvent failureEvent;
        [Tooltip("Event to send when there are no more items.")]
        public FsmEvent finishedEvent;
        [RequiredField, ActionSection("Set up"), CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Event to send to get the next item.")]
        public FsmEvent loopEvent;
        private int nextItemIndex;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Set to true to force iterating from the first item. This variable will be set to false as it carries on iterating, force it back to true if you want to renter this action back to the first item."), UIHint(UIHint.Variable)]
        public FsmBool reset;
        [Tooltip("The value for the current index."), UIHint(UIHint.Variable)]
        public FsmVar result;
        [Tooltip("From where to start iteration, leave to 0 to start from the beginning")]
        public FsmInt startIndex;

        private void DoGetNextItem()
        {
            if (this.nextItemIndex >= base.proxy.arrayList.Count)
            {
                this.nextItemIndex = 0;
                base.Fsm.Event(this.finishedEvent);
            }
            else
            {
                this.GetItemAtIndex();
                if (this.nextItemIndex >= base.proxy.arrayList.Count)
                {
                    this.nextItemIndex = 0;
                    base.Fsm.Event(this.finishedEvent);
                }
                else if ((this.endIndex.Value > 0) && (this.nextItemIndex >= this.endIndex.Value))
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
                object obj2 = null;
                this.currentIndex.Value = this.nextItemIndex;
                try
                {
                    obj2 = base.proxy.arrayList[this.nextItemIndex];
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
                if (this.startIndex.Value > 0)
                {
                    this.nextItemIndex = this.startIndex.Value;
                }
            }
            this.DoGetNextItem();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.startIndex = null;
            this.endIndex = null;
            this.reset = null;
            this.loopEvent = null;
            this.finishedEvent = null;
            this.failureEvent = null;
            this.result = null;
            this.currentIndex = null;
        }
    }
}

