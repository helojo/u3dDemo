﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Enables/Disables an Animation on a GameObject.\nAnimation time is paused while disabled. Animation must also have a non zero weight to play."), ActionCategory(ActionCategory.Animation)]
    public class EnableAnimation : FsmStateAction
    {
        private AnimationState anim;
        [UIHint(UIHint.Animation), RequiredField, Tooltip("The name of the animation to enable/disable.")]
        public FsmString animName;
        [Tooltip("Set to True to enable, False to disable."), RequiredField]
        public FsmBool enable;
        [RequiredField, CheckForComponent(typeof(Animation)), Tooltip("The GameObject playing the animation.")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Reset the initial enabled state when exiting the state.")]
        public FsmBool resetOnExit;

        private void DoEnableAnimation(GameObject go)
        {
            if (go != null)
            {
                if (go.animation == null)
                {
                    this.LogError("Missing animation component!");
                }
                else
                {
                    this.anim = go.animation[this.animName.Value];
                    if (this.anim != null)
                    {
                        this.anim.enabled = this.enable.Value;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoEnableAnimation(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
            base.Finish();
        }

        public override void OnExit()
        {
            if (this.resetOnExit.Value && (this.anim != null))
            {
                this.anim.enabled = !this.enable.Value;
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.animName = null;
            this.enable = 1;
            this.resetOnExit = 0;
        }
    }
}

