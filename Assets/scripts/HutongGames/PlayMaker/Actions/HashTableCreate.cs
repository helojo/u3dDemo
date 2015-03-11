namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Adds a PlayMakerHashTableProxy Component to a Game Object. Use this to create arrayList on the fly instead of during authoring.\n Optionally remove the HashTable component on exiting the state.\n Simply point to existing if the reference exists already."), ActionCategory("ArrayMaker/HashTable")]
    public class HashTableCreate : HashTableActions
    {
        private PlayMakerHashTableProxy addedComponent;
        [UIHint(UIHint.FsmEvent), ActionSection("Result"), Tooltip("The event to trigger if the hashtable exists already")]
        public FsmEvent alreadyExistsEvent;
        [RequiredField, Tooltip("The Game Object to add the Hashtable Component to."), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmString), Tooltip("Author defined Reference of the PlayMaker arrayList proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Remove the Component when this State is exited.")]
        public FsmBool removeOnExit;

        private void DoAddPlayMakerHashTable()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.GetHashTableProxyPointer(ownerDefaultTarget, this.reference.Value, true) != null)
            {
                base.Fsm.Event(this.alreadyExistsEvent);
            }
            else
            {
                this.addedComponent = (PlayMakerHashTableProxy) ownerDefaultTarget.AddComponent("PlayMakerHashTableProxy");
                if (this.addedComponent == null)
                {
                    Debug.LogError("Can't add PlayMakerHashTableProxy");
                }
                else
                {
                    this.addedComponent.referenceName = this.reference.Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoAddPlayMakerHashTable();
            base.Finish();
        }

        public override void OnExit()
        {
            if (this.removeOnExit.Value && (this.addedComponent != null))
            {
                UnityEngine.Object.Destroy(this.addedComponent);
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.alreadyExistsEvent = null;
        }
    }
}

