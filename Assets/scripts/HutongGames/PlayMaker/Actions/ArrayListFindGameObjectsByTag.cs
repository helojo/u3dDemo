namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Store all active GameObjects with a specific tag. Tags must be declared in the tag manager before using them"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListFindGameObjectsByTag : ArrayListActions
    {
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField, ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("the tag")]
        public FsmString tag;

        public void FindGOByTag()
        {
            if (base.isProxyValid())
            {
                base.proxy.arrayList.Clear();
                GameObject[] c = GameObject.FindGameObjectsWithTag(this.tag.Value);
                base.proxy.arrayList.InsertRange(0, c);
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.FindGOByTag();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.tag = null;
        }
    }
}

