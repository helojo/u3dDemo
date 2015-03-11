namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Store a mesh vertex colors into an arrayList"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListGetVertexColors : ArrayListActions
    {
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [CheckForComponent(typeof(MeshFilter)), ActionSection("Source"), Tooltip("the GameObject to get the mesh from")]
        public FsmGameObject mesh;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void getVertexColors()
        {
            if (base.isProxyValid())
            {
                base.proxy.arrayList.Clear();
                GameObject obj2 = this.mesh.Value;
                if (obj2 != null)
                {
                    MeshFilter component = obj2.GetComponent<MeshFilter>();
                    if (component != null)
                    {
                        base.proxy.arrayList.InsertRange(0, component.mesh.colors);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.getVertexColors();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.mesh = null;
        }
    }
}

