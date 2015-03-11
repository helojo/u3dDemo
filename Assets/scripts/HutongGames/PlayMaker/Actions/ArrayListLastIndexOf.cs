namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Return the Last index occurence of an item from a PlayMaker Array List Proxy component. Can search within a range")]
    public class ArrayListLastIndexOf : ArrayListActions
    {
        [UIHint(UIHint.FsmInt), Tooltip("Optional amount of elements to search within: set to 0 to ignore")]
        public FsmInt count;
        [Tooltip("Optional Event to trigger if the action fails ( likely an out of range exception when using wrong values for index and/or count)"), UIHint(UIHint.FsmEvent)]
        public FsmEvent failureEvent;
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this arraList contains that element ( described below)")]
        public FsmEvent itemFound;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this arraList does not contains that element ( described below)")]
        public FsmEvent itemNotFound;
        [ActionSection("Result"), Tooltip("The index of the last item described below"), RequiredField]
        public FsmInt lastIndexOf;
        [UIHint(UIHint.FsmString), Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
        public FsmString reference;
        [UIHint(UIHint.FsmInt), Tooltip("Optional start index to search from: set to 0 to ignore")]
        public FsmInt startIndex;
        [RequiredField, ActionSection("Data"), Tooltip("The variable to get the index of.")]
        public FsmVar variable;

        public void DoArrayListLastIndex()
        {
            if (base.isProxyValid())
            {
                object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable);
                int num = -1;
                try
                {
                    if ((this.startIndex.Value == 0) && (this.count.Value == 0))
                    {
                        num = PlayMakerUtils_Extensions.LastIndexOf(base.proxy.arrayList, valueFromFsmVar);
                    }
                    else if (this.count.Value == 0)
                    {
                        if ((this.startIndex.Value < 0) || (this.startIndex.Value >= base.proxy.arrayList.Count))
                        {
                            Debug.LogError("start index out of range");
                            return;
                        }
                        num = PlayMakerUtils_Extensions.LastIndexOf(base.proxy.arrayList, valueFromFsmVar, this.startIndex.Value);
                    }
                    else
                    {
                        if ((this.startIndex.Value < 0) || (this.startIndex.Value >= (base.proxy.arrayList.Count - this.count.Value)))
                        {
                            Debug.LogError("start index and count out of range");
                            return;
                        }
                        num = PlayMakerUtils_Extensions.LastIndexOf(base.proxy.arrayList, valueFromFsmVar, this.startIndex.Value, this.count.Value);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    base.Fsm.Event(this.failureEvent);
                    return;
                }
                this.lastIndexOf.Value = num;
                if (num == -1)
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
                this.DoArrayListLastIndex();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.startIndex = null;
            this.count = null;
            this.lastIndexOf = null;
            this.itemFound = null;
            this.itemNotFound = null;
            this.failureEvent = null;
        }
    }
}

