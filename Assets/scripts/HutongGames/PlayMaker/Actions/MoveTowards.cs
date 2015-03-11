namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Moves a Game Object towards a Target. Optionally sends an event when successful. The Target can be specified as a Game Object or a world Position. If you specify both, then the Position is used as a local offset from the Object's Position."), ActionCategory(ActionCategory.Transform)]
    public class MoveTowards : FsmStateAction
    {
        [Tooltip("Distance at which the move is considered finished, and the Finish Event is sent."), HasFloatSlider(0f, 5f)]
        public FsmFloat finishDistance;
        [Tooltip("Event to send when the Finish Distance is reached.")]
        public FsmEvent finishEvent;
        [RequiredField, Tooltip("The GameObject to Move")]
        public FsmOwnerDefault gameObject;
        private GameObject go;
        private GameObject goTarget;
        [Tooltip("Ignore any height difference in the target.")]
        public FsmBool ignoreVertical;
        [Tooltip("The maximum movement speed. HINT: You can make this a variable to change it over time."), HasFloatSlider(0f, 20f)]
        public FsmFloat maxSpeed;
        [Tooltip("A target GameObject to move towards. Or use a world Target Position below.")]
        public FsmGameObject targetObject;
        private Vector3 targetPos;
        [Tooltip("A world position if no Target Object. Otherwise used as a local offset from the Target Object.")]
        public FsmVector3 targetPosition;
        private Vector3 targetPosWithVertical;

        private void DoMoveTowards()
        {
            if (this.UpdateTargetPos())
            {
                this.go.transform.position = Vector3.MoveTowards(this.go.transform.position, this.targetPos, this.maxSpeed.Value * Time.deltaTime);
                Vector3 vector = this.go.transform.position - this.targetPos;
                if (vector.magnitude < this.finishDistance.Value)
                {
                    base.Fsm.Event(this.finishEvent);
                    base.Finish();
                }
            }
        }

        public Vector3 GetTargetPos()
        {
            return this.targetPos;
        }

        public Vector3 GetTargetPosWithVertical()
        {
            return this.targetPosWithVertical;
        }

        public override void OnUpdate()
        {
            this.DoMoveTowards();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.targetObject = null;
            this.maxSpeed = 10f;
            this.finishDistance = 1f;
            this.finishEvent = null;
        }

        public bool UpdateTargetPos()
        {
            this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (this.go == null)
            {
                return false;
            }
            this.goTarget = this.targetObject.Value;
            if ((this.goTarget == null) && this.targetPosition.IsNone)
            {
                return false;
            }
            if (this.goTarget != null)
            {
                this.targetPos = this.targetPosition.IsNone ? this.goTarget.transform.position : this.goTarget.transform.TransformPoint(this.targetPosition.Value);
            }
            else
            {
                this.targetPos = this.targetPosition.Value;
            }
            this.targetPosWithVertical = this.targetPos;
            if (this.ignoreVertical.Value)
            {
                this.targetPos.y = this.go.transform.position.y;
            }
            return true;
        }
    }
}

