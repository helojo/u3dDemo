namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Gets a random item from a PlayMaker ArrayList Proxy component")]
    public class ArrayListGetRandom : ArrayListActions
    {
        [Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)"), UIHint(UIHint.FsmEvent)]
        public FsmEvent failureEvent;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("The random item index picked from the array"), UIHint(UIHint.Variable)]
        public FsmInt randomIndex;
        [ActionSection("Result"), UIHint(UIHint.Variable), Tooltip("The random item data picked from the array")]
        public FsmVar randomItem;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void GetRandomItem()
        {
            if (base.isProxyValid())
            {
                int num = UnityEngine.Random.Range(0, base.proxy.arrayList.Count);
                object obj2 = null;
                try
                {
                    obj2 = base.proxy.arrayList[num];
                }
                catch (Exception exception)
                {
                    Debug.LogWarning(exception.Message);
                    base.Fsm.Event(this.failureEvent);
                    return;
                }
                this.randomIndex.Value = num;
                if (!PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, this.randomItem, obj2))
                {
                    Debug.LogWarning("ApplyValueToFsmVar failed");
                    base.Fsm.Event(this.failureEvent);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.GetRandomItem();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.failureEvent = null;
            this.randomItem = null;
            this.randomIndex = null;
        }
    }
}

