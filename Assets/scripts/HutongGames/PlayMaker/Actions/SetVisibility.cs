﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Material), Tooltip("Sets the visibility of a GameObject. Note: this action sets the GameObject Renderer's enabled state.")]
    public class SetVisibility : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(Renderer))]
        public FsmOwnerDefault gameObject;
        private bool initialVisibility;
        [Tooltip("Resets to the initial visibility when it leaves the state")]
        public bool resetOnExit;
        [Tooltip("Should the object visibility be toggled?\nHas priority over the 'visible' setting")]
        public FsmBool toggle;
        [Tooltip("Should the object be set to visible or invisible?")]
        public FsmBool visible;

        private void DoSetVisibility(GameObject go)
        {
            if (go != null)
            {
                if (go.renderer == null)
                {
                    this.LogError("Missing Renderer!");
                }
                else
                {
                    this.initialVisibility = go.renderer.enabled;
                    if (!this.toggle.Value)
                    {
                        go.renderer.enabled = this.visible.Value;
                    }
                    else
                    {
                        go.renderer.enabled = !go.renderer.enabled;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetVisibility(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
            base.Finish();
        }

        public override void OnExit()
        {
            if (this.resetOnExit)
            {
                this.ResetVisibility();
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.toggle = 0;
            this.visible = 0;
            this.resetOnExit = true;
            this.initialVisibility = false;
        }

        private void ResetVisibility()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if ((ownerDefaultTarget != null) && (ownerDefaultTarget.renderer != null))
            {
                ownerDefaultTarget.renderer.enabled = this.initialVisibility;
            }
        }
    }
}

