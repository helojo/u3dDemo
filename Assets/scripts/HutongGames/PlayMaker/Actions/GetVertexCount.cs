namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Gets the number of vertices in a GameObject's mesh. Useful in conjunction with GetVertexPosition."), ActionCategory("Mesh")]
    public class GetVertexCount : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField, CheckForComponent(typeof(MeshFilter)), Tooltip("The GameObject to check.")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.Variable), RequiredField, Tooltip("Store the vertex count in a variable.")]
        public FsmInt storeCount;

        private void DoGetVertexCount()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                MeshFilter component = ownerDefaultTarget.GetComponent<MeshFilter>();
                if (component == null)
                {
                    this.LogError("Missing MeshFilter!");
                }
                else
                {
                    this.storeCount.Value = component.mesh.vertexCount;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetVertexCount();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetVertexCount();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.storeCount = null;
            this.everyFrame = false;
        }
    }
}

