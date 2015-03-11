namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Gets an item from a PlayMaker ArrayList Proxy component using a base index and a relative increment. This allows you to move to next or previous items granuraly")]
    public class ArrayListGetRelative : ArrayListActions
    {
        [Tooltip("The index base to compute the item to get")]
        public FsmInt baseIndex;
        [RequiredField, ActionSection("Set up"), CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        public FsmOwnerDefault gameObject;
        [Tooltip("The incremental value from the base index to get the value from. Overshooting the range will loop back on the list.")]
        public FsmInt increment;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [ActionSection("Result"), UIHint(UIHint.Variable)]
        public FsmVar result;
        [Tooltip("The index of the result"), UIHint(UIHint.Variable)]
        public FsmInt resultIndex;

        public void GetItemAtIncrement()
        {
            if (base.isProxyValid())
            {
                object obj2 = null;
                int num = this.baseIndex.Value + this.increment.Value;
                if (num >= 0)
                {
                    this.resultIndex.Value = (this.baseIndex.Value + this.increment.Value) % base.proxy.arrayList.Count;
                }
                else
                {
                    this.resultIndex.Value = base.proxy.arrayList.Count - (Mathf.Abs((int) (this.baseIndex.Value + this.increment.Value)) % base.proxy.arrayList.Count);
                }
                obj2 = base.proxy.arrayList[this.resultIndex.Value];
                PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, this.result, obj2);
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.GetItemAtIncrement();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.baseIndex = null;
            this.increment = null;
            this.result = null;
            this.resultIndex = null;
        }
    }
}

