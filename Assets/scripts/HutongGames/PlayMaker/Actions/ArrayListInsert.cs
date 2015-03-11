namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Insert item at a specified index of a PlayMaker ArrayList Proxy component"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListInsert : ArrayListActions
    {
        [UIHint(UIHint.FsmEvent), Tooltip("The event to trigger if the removeAt throw errors"), ActionSection("Result")]
        public FsmEvent failureEvent;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy)), ActionSection("Set up"), RequiredField]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmInt), Tooltip("The index to remove at")]
        public FsmInt index;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The variable to add."), RequiredField, ActionSection("Data")]
        public FsmVar variable;

        public void doArrayListInsert()
        {
            if (base.isProxyValid())
            {
                try
                {
                    base.proxy.arrayList.Insert(this.index.Value, PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.variable));
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    base.Fsm.Event(this.failureEvent);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.doArrayListInsert();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.variable = null;
            this.failureEvent = null;
            this.index = null;
        }
    }
}

