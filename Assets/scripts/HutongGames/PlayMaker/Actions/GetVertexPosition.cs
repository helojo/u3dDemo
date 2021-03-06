﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("Mesh"), Tooltip("Gets the position of a vertex in a GameObject's mesh. Hint: Use GetVertexCount to get the number of vertices in a mesh.")]
    public class GetVertexPosition : FsmStateAction
    {
        [Tooltip("Repeat every frame. Useful if the mesh is animated.")]
        public bool everyFrame;
        [CheckForComponent(typeof(MeshFilter)), RequiredField, Tooltip("The GameObject to check.")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Coordinate system to use.")]
        public Space space;
        [UIHint(UIHint.Variable), RequiredField, Tooltip("Store the vertex position in a variable.")]
        public FsmVector3 storePosition;
        [RequiredField, Tooltip("The index of the vertex.")]
        public FsmInt vertexIndex;

        private void DoGetVertexPosition()
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
                    switch (this.space)
                    {
                        case Space.World:
                        {
                            Vector3 position = component.mesh.vertices[this.vertexIndex.Value];
                            this.storePosition.Value = ownerDefaultTarget.transform.TransformPoint(position);
                            break;
                        }
                        case Space.Self:
                            this.storePosition.Value = component.mesh.vertices[this.vertexIndex.Value];
                            break;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetVertexPosition();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetVertexPosition();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.space = Space.World;
            this.storePosition = null;
            this.everyFrame = false;
        }
    }
}

