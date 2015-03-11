﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Sets the Color of a Light."), ActionCategory(ActionCategory.Lights)]
    public class SetLightColor : FsmStateAction
    {
        public bool everyFrame;
        [CheckForComponent(typeof(Light)), RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField]
        public FsmColor lightColor;

        private void DoSetLightColor()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                Light light = ownerDefaultTarget.light;
                if (light == null)
                {
                    this.LogError("Missing Light Component!");
                }
                else
                {
                    light.color = this.lightColor.Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetLightColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetLightColor();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.lightColor = Color.white;
            this.everyFrame = false;
        }
    }
}

