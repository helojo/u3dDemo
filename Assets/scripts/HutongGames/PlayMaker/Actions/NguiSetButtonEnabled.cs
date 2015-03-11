namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("NGUI"), Tooltip("Sets an NGUI Buttons's Enabled property")]
    public class NguiSetButtonEnabled : FsmStateAction
    {
        [Tooltip("The new value to assign the Enabled property"), RequiredField]
        public FsmBool enabled;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [RequiredField, Tooltip("NGUI object to update")]
        public FsmOwnerDefault NguiButtonToUpdate;

        private void DoChangeEnabledValue()
        {
            if ((this.NguiButtonToUpdate != null) && (this.enabled != null))
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.NguiButtonToUpdate);
                UIImageButton component = ownerDefaultTarget.GetComponent<UIImageButton>();
                if (component != null)
                {
                    component.isEnabled = this.enabled.Value;
                }
                else
                {
                    UIButton button2 = ownerDefaultTarget.GetComponent<UIButton>();
                    if (button2 != null)
                    {
                        button2.isEnabled = this.enabled.Value;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoChangeEnabledValue();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoChangeEnabledValue();
        }

        public override void Reset()
        {
            this.NguiButtonToUpdate = null;
            this.enabled = null;
            this.everyFrame = false;
        }
    }
}

