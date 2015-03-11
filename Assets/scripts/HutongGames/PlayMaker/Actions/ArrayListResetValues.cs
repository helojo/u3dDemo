namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Sets all element to to a given value of a PlayMaker ArrayList Proxy component")]
    public class ArrayListResetValues : ArrayListActions
    {
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField, ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The value to reset all the arrayList with")]
        public FsmVar resetValue;

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.ResetArrayList();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.resetValue = null;
        }

        public void ResetArrayList()
        {
            if (base.isProxyValid())
            {
                object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.resetValue);
                for (int i = 0; i < base.proxy.arrayList.Count; i++)
                {
                    base.proxy.arrayList[i] = valueFromFsmVar;
                }
            }
        }
    }
}

