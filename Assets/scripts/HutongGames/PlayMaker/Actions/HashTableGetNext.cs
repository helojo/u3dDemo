namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [Tooltip("Each time this action is called it gets the next item from a PlayMaker HashTable Proxy component. \nThis lets you quickly loop through all the children of an object to perform actions on them.\nNOTE: To get to specific item use HashTableGet instead."), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableGetNext : HashTableActions
    {
        private ArrayList _keys;
        [Tooltip("When to end iteration, leave to 0 to iterate until the end")]
        public FsmInt endIndex;
        [Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)"), UIHint(UIHint.FsmEvent)]
        public FsmEvent failureEvent;
        [Tooltip("Event to send when there are no more items.")]
        public FsmEvent finishedEvent;
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), ActionSection("Result")]
        public FsmString key;
        [Tooltip("Event to send to get the next item.")]
        public FsmEvent loopEvent;
        private int nextItemIndex;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [UIHint(UIHint.Variable), Tooltip("Set to true to force iterating from the first item. This variable will be set to false as it carries on iterating, force it back to true if you want to renter this action back to the first item.")]
        public FsmBool reset;
        [UIHint(UIHint.Variable)]
        public FsmVar result;
        [Tooltip("From where to start iteration, leave to 0 to start from the beginning")]
        public FsmInt startIndex;

        private void DoGetNextItem()
        {
            if (this.nextItemIndex >= this._keys.Count)
            {
                this.nextItemIndex = 0;
                base.Fsm.Event(this.finishedEvent);
            }
            else
            {
                this.GetItemAtIndex();
                if (this.nextItemIndex >= this._keys.Count)
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
                try
                {
                    obj2 = base.proxy.hashTable[this._keys[this.nextItemIndex]];
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    base.Fsm.Event(this.failureEvent);
                    return;
                }
                this.key.Value = (string) this._keys[this.nextItemIndex];
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
                if (!base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
                {
                    base.Fsm.Event(this.failureEvent);
                    base.Finish();
                }
                this._keys = new ArrayList(base.proxy.hashTable.Keys);
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
            this.reset = null;
            this.startIndex = null;
            this.endIndex = null;
            this.loopEvent = null;
            this.finishedEvent = null;
            this.failureEvent = null;
            this.result = null;
        }
    }
}

