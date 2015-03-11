namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Destroys a PlayMakerHashTableProxy Component of a Game Object."), ActionCategory("ArrayMaker/HashTable")]
    public class DestroyHashTable : HashTableActions
    {
        [Tooltip("The gameObject with the PlayMaker HashTable Proxy component"), ActionSection("Set up"), RequiredField, CheckForComponent(typeof(PlayMakerHashTableProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("The event to trigger if the HashTable proxy component was not found"), UIHint(UIHint.FsmEvent)]
        public FsmEvent notFoundEvent;
        [UIHint(UIHint.FsmString), Tooltip("Author defined Reference of the PlayMaker HashTable proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The event to trigger if the HashTable proxy component is destroyed"), ActionSection("Result"), UIHint(UIHint.FsmEvent)]
        public FsmEvent successEvent;

        private void DoDestroyHashTable(GameObject go)
        {
            foreach (PlayMakerHashTableProxy proxy in base.proxy.GetComponents<PlayMakerHashTableProxy>())
            {
                if (proxy.referenceName == this.reference.Value)
                {
                    UnityEngine.Object.Destroy(proxy);
                    base.Fsm.Event(this.successEvent);
                    return;
                }
            }
            base.Fsm.Event(this.notFoundEvent);
        }

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.SetUpHashTableProxyPointer(ownerDefaultTarget, this.reference.Value))
            {
                this.DoDestroyHashTable(ownerDefaultTarget);
            }
            else
            {
                base.Fsm.Event(this.notFoundEvent);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.successEvent = null;
            this.notFoundEvent = null;
        }
    }
}

