namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/HashTable"), Tooltip("Revert a PlayMaker HashTable Proxy component to the prefill data, either defined at runtime or when the action HashTableTakeSnapShot was used.")]
    public class HashTableRevertSnapShot : HashTableActions
    {
        [RequiredField, ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void DoHashTableRevertToSnapShot()
        {
            if (base.isProxyValid())
            {
                base.proxy.RevertToSnapShot();
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoHashTableRevertToSnapShot();
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

