namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Store all resolutions"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListGetScreenResolutions : ArrayListActions
    {
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void getResolutions()
        {
            if (base.isProxyValid())
            {
                base.proxy.arrayList.Clear();
                foreach (Resolution resolution in Screen.resolutions)
                {
                    base.proxy.arrayList.Add(new Vector3((float) resolution.width, (float) resolution.height, (float) resolution.refreshRate));
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.getResolutions();
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

