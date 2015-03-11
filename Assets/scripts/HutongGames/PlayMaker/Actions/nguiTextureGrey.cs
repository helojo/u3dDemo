namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("Ngui Actions"), Tooltip("Chnage Texture to Grey")]
    public class nguiTextureGrey : FsmStateAction
    {
        [Tooltip("Input is enable")]
        public FsmBool greyEnabled;
        [RequiredField, Tooltip("NGUI Texture to enable grey")]
        public FsmOwnerDefault NguiTextureObject;
        private UITexture theNguiTexture;

        public static void doChangeEnableGrey(UITexture texture, bool enabled)
        {
            if (enabled)
            {
                texture.shader = Shader.Find("Unlit/Transparent Colored (Grey)");
            }
            else
            {
                texture.shader = Shader.Find("Unlit/Transparent Colored");
            }
        }

        public override void OnEnter()
        {
            if (this.NguiTextureObject == null)
            {
                base.Finish();
            }
            this.theNguiTexture = base.Fsm.GetOwnerDefaultTarget(this.NguiTextureObject).GetComponent<UITexture>();
            if (this.theNguiTexture != null)
            {
                doChangeEnableGrey(this.theNguiTexture, this.greyEnabled.Value);
            }
        }

        public override void Reset()
        {
            this.NguiTextureObject = null;
            this.greyEnabled = 0;
        }
    }
}

