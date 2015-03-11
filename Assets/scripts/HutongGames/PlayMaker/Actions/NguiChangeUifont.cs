namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("NGUI"), Tooltip("Changes the UIFont on a label")]
    public class NguiChangeUifont : FsmStateAction
    {
        [Tooltip("The new font to use"), RequiredField]
        public FsmGameObject NewFont;
        [Tooltip("NGUI Label to change font"), RequiredField]
        public FsmOwnerDefault NguiLabel;

        private void DoChangeFont()
        {
            if ((this.NguiLabel != null) && (this.NewFont != null))
            {
                UILabel component = base.Fsm.GetOwnerDefaultTarget(this.NguiLabel).GetComponent<UILabel>();
                if (component != null)
                {
                    UIFont font = this.NewFont.Value.GetComponent<UIFont>();
                    if (font == null)
                    {
                        Debug.Log("Font is null");
                    }
                    else
                    {
                        component.bitmapFont = font;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoChangeFont();
            base.Finish();
        }

        public override void Reset()
        {
            this.NguiLabel = null;
            this.NewFont = null;
        }
    }
}

