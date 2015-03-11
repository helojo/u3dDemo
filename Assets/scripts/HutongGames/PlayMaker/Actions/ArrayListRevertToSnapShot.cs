namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Revert a PlayMaker ArrayList Proxy component to the prefill data, either defined at runtime or when the action ArrayListTakeSnapShot was used. ")]
    public class ArrayListRevertToSnapShot : ArrayListActions
    {
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void DoArrayListRevertToSnapShot()
        {
            if (base.isProxyValid())
            {
                base.proxy.RevertToSnapShot();
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoArrayListRevertToSnapShot();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
        }
    }
}

