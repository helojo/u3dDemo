﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Physics), Tooltip("Forces a Game Object's Rigid Body to Sleep at least one frame.")]
    public class Sleep : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(Rigidbody))]
        public FsmOwnerDefault gameObject;

        private void DoSleep()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if ((ownerDefaultTarget != null) && (ownerDefaultTarget.rigidbody != null))
            {
                ownerDefaultTarget.rigidbody.Sleep();
            }
        }

        public override void OnEnter()
        {
            this.DoSleep();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
        }
    }
}

