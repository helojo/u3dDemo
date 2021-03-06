﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.AnimateVariables), Tooltip("Animates the value of a Float Variable using an Animation Curve.")]
    public class AnimateFloat : FsmStateAction
    {
        [RequiredField, Tooltip("The animation curve to use.")]
        public FsmAnimationCurve animCurve;
        private float currentTime;
        private float endTime;
        [Tooltip("Optionally send an Event when the animation finishes.")]
        public FsmEvent finishEvent;
        [Tooltip("The float variable to set."), UIHint(UIHint.Variable), RequiredField]
        public FsmFloat floatVariable;
        private bool looping;
        [Tooltip("Ignore TimeScale. Useful if the game is paused.")]
        public bool realTime;
        private float startTime;

        public override void OnEnter()
        {
            this.startTime = FsmTime.RealtimeSinceStartup;
            this.currentTime = 0f;
            if (((this.animCurve != null) && (this.animCurve.curve != null)) && (this.animCurve.curve.keys.Length > 0))
            {
                this.endTime = this.animCurve.curve.keys[this.animCurve.curve.length - 1].time;
                this.looping = ActionHelpers.IsLoopingWrapMode(this.animCurve.curve.postWrapMode);
            }
            else
            {
                base.Finish();
                return;
            }
            this.floatVariable.Value = this.animCurve.curve.Evaluate(0f);
        }

        public override void OnUpdate()
        {
            if (this.realTime)
            {
                this.currentTime = FsmTime.RealtimeSinceStartup - this.startTime;
            }
            else
            {
                this.currentTime += Time.deltaTime;
            }
            if (((this.animCurve != null) && (this.animCurve.curve != null)) && (this.floatVariable != null))
            {
                this.floatVariable.Value = this.animCurve.curve.Evaluate(this.currentTime);
            }
            if (this.currentTime >= this.endTime)
            {
                if (!this.looping)
                {
                    base.Finish();
                }
                if (this.finishEvent != null)
                {
                    base.Fsm.Event(this.finishEvent);
                }
            }
        }

        public override void Reset()
        {
            this.animCurve = null;
            this.floatVariable = null;
            this.finishEvent = null;
            this.realTime = false;
        }
    }
}

