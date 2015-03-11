namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Send event to all the GameObjects within an arrayList.")]
    public class ArrayListSendEventToGameObjects : ArrayListActions
    {
        public FsmBool excludeSelf;
        [ActionSection("Set up"), CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs."), RequiredField]
        public FsmEvent sendEvent;
        public FsmBool sendToChildren;

        private void DoSendEvent()
        {
            if (base.isProxyValid())
            {
                IEnumerator enumerator = base.proxy.arrayList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        GameObject current = (GameObject) enumerator.Current;
                        this.sendEventToGO(current);
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
            }
        }

        public override void OnEnter()
        {
            if (!base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                base.Finish();
            }
            this.DoSendEvent();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.sendEvent = null;
            this.excludeSelf = 0;
            this.sendToChildren = 0;
        }

        private void sendEventToGO(GameObject _go)
        {
            FsmEventTarget eventTarget = new FsmEventTarget {
                excludeSelf = this.excludeSelf.Value
            };
            FsmOwnerDefault default2 = new FsmOwnerDefault {
                OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                GameObject = new FsmGameObject()
            };
            default2.GameObject.Value = base.Owner;
            eventTarget.gameObject = default2;
            eventTarget.target = FsmEventTarget.EventTarget.GameObject;
            eventTarget.sendToChildren = this.sendToChildren.Value;
            base.Fsm.Event(eventTarget, this.sendEvent);
        }
    }
}

