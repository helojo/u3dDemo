namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [Tooltip("Set mesh vertex positions based on vector3 found in an arrayList"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListSetVertexPositions : ArrayListActions
    {
        private Mesh _mesh;
        private Vector3[] _vertices;
        public bool everyFrame;
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [CheckForComponent(typeof(MeshFilter)), ActionSection("Target"), Tooltip("The GameObject to set the mesh vertex positions to")]
        public FsmGameObject mesh;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        public override void OnEnter()
        {
            GameObject obj2 = this.mesh.Value;
            if (obj2 == null)
            {
                base.Finish();
            }
            else
            {
                MeshFilter component = obj2.GetComponent<MeshFilter>();
                if (component == null)
                {
                    base.Finish();
                }
                else
                {
                    this._mesh = component.mesh;
                    if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
                    {
                        this.SetVertexPositions();
                    }
                    if (!this.everyFrame)
                    {
                        base.Finish();
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            this.SetVertexPositions();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.mesh = null;
            this.everyFrame = false;
        }

        public void SetVertexPositions()
        {
            if (base.isProxyValid())
            {
                this._vertices = new Vector3[base.proxy.arrayList.Count];
                int index = 0;
                IEnumerator enumerator = base.proxy.arrayList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        this._vertices[index] = (Vector3) enumerator.Current;
                        index++;
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
                this._mesh.vertices = this._vertices;
            }
        }
    }
}

