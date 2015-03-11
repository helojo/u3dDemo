namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Store mesh vertex positions into an arrayList")]
    public class ArrayListGetVertexPositions : ArrayListActions
    {
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField, ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [CheckForComponent(typeof(MeshFilter)), Tooltip("the GameObject to get the mesh from"), ActionSection("Source")]
        public FsmGameObject mesh;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public void getVertexPositions()
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
                        base.proxy.arrayList.InsertRange(0, component.mesh.vertices);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.getVertexPositions();
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

