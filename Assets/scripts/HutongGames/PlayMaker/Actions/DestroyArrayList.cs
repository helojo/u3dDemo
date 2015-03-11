namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Destroys a PlayMakerArrayListProxy Component of a Game Object.")]
    public class DestroyArrayList : ArrayListActions
    {
        [RequiredField, ActionSection("Set up"), CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        public FsmOwnerDefault gameObject;
        [Tooltip("The event to trigger if the ArrayList proxy component was not found"), UIHint(UIHint.FsmEvent)]
        public FsmEvent notFoundEvent;
        [UIHint(UIHint.FsmString), Tooltip("Author defined Reference of the PlayMaker ArrayList proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The event to trigger if the ArrayList proxy component is destroyed"), UIHint(UIHint.FsmEvent), ActionSection("Result")]
        public FsmEvent successEvent;

        private void DoDestroyArrayList()
        {
            foreach (PlayMakerArrayListProxy proxy in base.proxy.GetComponents<PlayMakerArrayListProxy>())
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
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoDestroyArrayList();
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

