﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Action version of Unity's Smooth Follow script."), ActionCategory(ActionCategory.Transform)]
    public class SmoothFollowAction : FsmStateAction
    {
        private GameObject cachedObect;
        [RequiredField, Tooltip("The distance in the x-z plane to the target.")]
        public FsmFloat distance;
        [Tooltip("The game object to control. E.g. The camera."), RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField, Tooltip("The height we want the camera to be above the target")]
        public FsmFloat height;
        [RequiredField, Tooltip("How much to dampen height movement.")]
        public FsmFloat heightDamping;
        private Transform myTransform;
        [RequiredField, Tooltip("How much to dampen rotation changes.")]
        public FsmFloat rotationDamping;
        [Tooltip("The GameObject to follow.")]
        public FsmGameObject targetObject;
        private Transform targetTransform;

        public override void OnLateUpdate()
        {
            if (this.targetObject.Value != null)
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                if (ownerDefaultTarget != null)
                {
                    if (this.cachedObect != ownerDefaultTarget)
                    {
                        this.cachedObect = ownerDefaultTarget;
                        this.myTransform = ownerDefaultTarget.transform;
                        this.targetTransform = this.targetObject.Value.transform;
                    }
                    float y = this.targetTransform.eulerAngles.y;
                    float to = this.targetTransform.position.y + this.height.Value;
                    float a = this.myTransform.eulerAngles.y;
                    float from = this.myTransform.position.y;
                    a = Mathf.LerpAngle(a, y, this.rotationDamping.Value * Time.deltaTime);
                    from = Mathf.Lerp(from, to, this.heightDamping.Value * Time.deltaTime);
                    Quaternion quaternion = Quaternion.Euler(0f, a, 0f);
                    this.myTransform.position = this.targetTransform.position;
                    this.myTransform.position -= (Vector3) ((quaternion * Vector3.forward) * this.distance.Value);
                    this.myTransform.position = new Vector3(this.myTransform.position.x, from, this.myTransform.position.z);
                    this.myTransform.LookAt(this.targetTransform);
                }
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.targetObject = null;
            this.distance = 10f;
            this.height = 5f;
            this.heightDamping = 2f;
            this.rotationDamping = 3f;
        }
    }
}

