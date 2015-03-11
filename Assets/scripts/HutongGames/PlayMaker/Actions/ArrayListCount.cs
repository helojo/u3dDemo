namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Count items from a PlayMaker ArrayList Proxy component")]
    public class ArrayListCount : ArrayListActions
    {
        [Tooltip("Store the count value"), UIHint(UIHint.FsmInt), ActionSection("Result")]
        public FsmInt count;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy)), RequiredField, ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void getArrayListCount()
        {
            if (base.isProxyValid())
            {
                int count = base.proxy.arrayList.Count;
                this.count.Value = count;
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.getArrayListCount();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.count = null;
        }
    }
}

