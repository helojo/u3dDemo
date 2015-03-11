namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Remove an element of a PlayMaker Array List Proxy component"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListRemove : ArrayListActions
    {
        [ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Event sent if this arraList does not contains that element ( described below)"), ActionSection("Result"), UIHint(UIHint.FsmEvent)]
        public FsmEvent notFoundEvent;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)"), UIHint(UIHint.FsmString)]
        public FsmString reference;
        [ActionSection("Data"), Tooltip("The type of Variable to remove.")]
        public FsmVar variable;

        public void DoRemoveFromArrayList()
        {
            if (base.isProxyValid() && !base.proxy.Remove(PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable), this.variable.Type.ToString(), false))
            {
                base.Fsm.Event(this.notFoundEvent);
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoRemoveFromArrayList();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.notFoundEvent = null;
            this.variable = null;
        }
    }
}

