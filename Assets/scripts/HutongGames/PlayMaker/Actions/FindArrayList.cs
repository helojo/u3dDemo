namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Finds an ArrayList by reference. Warning: this function can be very slow.")]
    public class FindArrayList : CollectionsActions
    {
        [UIHint(UIHint.FsmString), RequiredField, ActionSection("Set up"), Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component")]
        public FsmString ArrayListReference;
        public FsmEvent foundEvent;
        public FsmEvent notFoundEvent;
        [Tooltip("Store the GameObject hosting the PlayMaker ArrayList Proxy component here"), ActionSection("Result"), RequiredField]
        public FsmGameObject store;

        public override void OnEnter()
        {
            PlayMakerArrayListProxy[] proxyArray = UnityEngine.Object.FindObjectsOfType(typeof(PlayMakerArrayListProxy)) as PlayMakerArrayListProxy[];
            foreach (PlayMakerArrayListProxy proxy in proxyArray)
            {
                if (proxy.referenceName == this.ArrayListReference.Value)
                {
                    this.store.Value = proxy.gameObject;
                    base.Fsm.Event(this.foundEvent);
                    return;
                }
            }
            this.store.Value = null;
            base.Fsm.Event(this.notFoundEvent);
            base.Finish();
        }

        public override void Reset()
        {
            this.ArrayListReference = string.Empty;
            this.store = null;
            this.foundEvent = null;
            this.notFoundEvent = null;
        }
    }
}

