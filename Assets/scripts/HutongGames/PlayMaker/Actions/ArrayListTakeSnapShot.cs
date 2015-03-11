namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Takes a PlayMaker ArrayList Proxy component snapshot, use action ArrayListRevertToSnapShot was used. A Snapshot is taken by default at the beginning for the prefill data")]
    public class ArrayListTakeSnapShot : ArrayListActions
    {
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void DoArrayListTakeSnapShot()
        {
            if (base.isProxyValid())
            {
                base.proxy.TakeSnapShot();
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoArrayListTakeSnapShot();
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

