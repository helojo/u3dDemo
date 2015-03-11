namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Return the index of an item from a PlayMaker Array List Proxy component. Can search within a range")]
    public class ArrayListIndexOf : ArrayListActions
    {
        [Tooltip("Optional amount of elements to search within: set to 0 to ignore"), UIHint(UIHint.FsmInt)]
        public FsmInt count;
        [Tooltip("Optional Event to trigger if the action fails ( likely an out of range exception when using wrong values for index and/or count)"), UIHint(UIHint.FsmEvent)]
        public FsmEvent failureEvent;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy)), RequiredField, ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("The index of the item described below"), UIHint(UIHint.Variable), ActionSection("Result")]
        public FsmInt indexOf;
        [UIHint(UIHint.FsmEvent), Tooltip("Optional Event sent if this arrayList contains that element ( described below)")]
        public FsmEvent itemFound;
        [Tooltip("Optional Event sent if this arrayList does not contains that element ( described below)"), UIHint(UIHint.FsmEvent)]
        public FsmEvent itemNotFound;
        [UIHint(UIHint.FsmString), Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
        public FsmString reference;
        [Tooltip("Optional start index to search from: set to 0 to ignore"), UIHint(UIHint.FsmInt)]
        public FsmInt startIndex;
        [RequiredField, Tooltip("The variable to get the index of."), ActionSection("Data")]
        public FsmVar variable;

        public void DoArrayListIndexOf()
        {
            if (base.isProxyValid())
            {
                object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable);
                int index = -1;
                try
                {
                    if (this.startIndex.IsNone)
                    {
                        Debug.Log("hello");
                        index = PlayMakerUtils_Extensions.IndexOf(base.proxy.arrayList, valueFromFsmVar);
                    }
                    else if (this.count.IsNone || (this.count.Value == 0))
                    {
                        if ((this.startIndex.Value < 0) || (this.startIndex.Value >= base.proxy.arrayList.Count))
                        {
                            this.LogError("start index out of range");
                            return;
                        }
                        index = PlayMakerUtils_Extensions.IndexOf(base.proxy.arrayList, valueFromFsmVar);
                    }
                    else
                    {
                        if ((this.startIndex.Value < 0) || (this.startIndex.Value >= (base.proxy.arrayList.Count - this.count.Value)))
                        {
                            this.LogError("start index and count out of range");
                            return;
                        }
                        index = PlayMakerUtils_Extensions.IndexOf(base.proxy.arrayList, valueFromFsmVar);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    base.Fsm.Event(this.failureEvent);
                    return;
                }
                this.indexOf.Value = index;
                if (index == -1)
                {
                    base.Fsm.Event(this.itemNotFound);
                }
                else
                {
                    base.Fsm.Event(this.itemFound);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoArrayListIndexOf();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.startIndex = null;
            this.count = null;
            this.itemFound = null;
            this.itemNotFound = null;
            this.variable = null;
        }
    }
}

