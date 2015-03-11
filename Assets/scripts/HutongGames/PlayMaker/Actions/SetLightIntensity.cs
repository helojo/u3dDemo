namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Sets the Intensity of a Light."), ActionCategory(ActionCategory.Lights)]
    public class SetLightIntensity : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField, CheckForComponent(typeof(Light))]
        public FsmOwnerDefault gameObject;
        public FsmFloat lightIntensity;

        private void DoSetLightIntensity()
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
                    light.intensity = this.lightIntensity.Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetLightIntensity();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetLightIntensity();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.lightIntensity = 1f;
            this.everyFrame = false;
        }
    }
}

