namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Adds a PlayMakerArrayList Component to a Game Object. Use this to create arrayList on the fly instead of during authoring.\n Optionally remove the ArrayList component on exiting the state.\n Simply point to existing if the reference exists already."), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListCreate : ArrayListActions
    {
        private PlayMakerArrayListProxy addedComponent;
        [Tooltip("The event to trigger if the arrayList exists already"), ActionSection("Result"), UIHint(UIHint.FsmEvent)]
        public FsmEvent alreadyExistsEvent;
        [Tooltip("The gameObject to add the PlayMaker ArrayList Proxy component to"), ActionSection("Set up"), RequiredField]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmString), Tooltip("Author defined Reference of the PlayMaker arrayList proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Remove the Component when this State is exited.")]
        public FsmBool removeOnExit;

        private void DoAddPlayMakerArrayList()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.GetArrayListProxyPointer(ownerDefaultTarget, this.reference.Value, true) != null)
            {
                base.Fsm.Event(this.alreadyExistsEvent);
            }
            else
            {
                this.addedComponent = (PlayMakerArrayListProxy) ownerDefaultTarget.AddComponent("PlayMakerArrayListProxy");
                if (this.addedComponent == null)
                {
                    this.LogError("Can't add PlayMakerArrayListProxy");
                }
                else
                {
                    this.addedComponent.referenceName = this.reference.Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoAddPlayMakerArrayList();
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

