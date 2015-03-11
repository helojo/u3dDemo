namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Set a mesh vertex colors based on colors found in an arrayList")]
    public class ArrayListSetVertexColors : ArrayListActions
    {
        private Color[] _colors;
        private Mesh _mesh;
        public bool everyFrame;
        [ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("The GameObject to set the mesh colors to"), CheckForComponent(typeof(MeshFilter)), ActionSection("Target")]
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
                        this.SetVertexColors();
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
            this.SetVertexColors();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.mesh = null;
            this.everyFrame = false;
        }

        public void SetVertexColors()
        {
            if (base.isProxyValid())
            {
                this._colors = new Color[base.proxy.arrayList.Count];
                int index = 0;
                IEnumerator enumerator = base.proxy.arrayList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        this._colors[index] = (Color) enumerator.Current;
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
                this._mesh.colors = this._colors;
            }
        }
    }
}

