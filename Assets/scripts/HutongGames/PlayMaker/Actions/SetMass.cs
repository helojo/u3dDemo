namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Physics), Tooltip("Sets the Mass of a Game Object's Rigid Body.")]
    public class SetMass : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;
        [HasFloatSlider(0.1f, 10f), RequiredField]
        public FsmFloat mass;

        private void DoSetMass()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if ((ownerDefaultTarget != null) && (ownerDefaultTarget.rigidbody != null))
            {
                ownerDefaultTarget.rigidbody.mass = this.mass.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetMass();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.mass = 1f;
        }
    }
}

