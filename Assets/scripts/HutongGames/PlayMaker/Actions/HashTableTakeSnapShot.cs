namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/HashTable"), Tooltip("Takes a PlayMaker HashTable Proxy component snapshot, use action HashTableRevertToSnapShot was used. A Snapshot is taken by default at the beginning for the prefill data")]
    public class HashTableTakeSnapShot : HashTableActions
    {
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void DoHashTableTakeSnapShot()
        {
            if (base.isProxyValid())
            {
                base.proxy.TakeSnapShot();
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoHashTableTakeSnapShot();
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

