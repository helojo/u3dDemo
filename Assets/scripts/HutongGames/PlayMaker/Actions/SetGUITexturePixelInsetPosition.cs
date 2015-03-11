namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Sets the Pixel Inset Position of the GUITexture attached to a Game Object. Useful for moving GUI elements around."), ActionCategory(ActionCategory.GUIElement)]
    public class SetGUITexturePixelInsetPosition : FsmStateAction
    {
        public FsmBool AsIncrement;
        public bool everyFrame;
        [CheckForComponent(typeof(GUITexture)), RequiredField]
        public FsmOwnerDefault gameObject;
        [RequiredField]
        public FsmFloat PixelInsetX;
        public FsmFloat PixelInsetY;

        private void DoGUITexturePixelInsetPosition()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if ((ownerDefaultTarget != null) && (ownerDefaultTarget.guiTexture != null))
            {
                Rect pixelInset = ownerDefaultTarget.guiTexture.pixelInset;
                if (this.AsIncrement.Value)
                {
                    pixelInset.x += this.PixelInsetX.Value;
                    pixelInset.y += this.PixelInsetY.Value;
                }
                else
                {
                    pixelInset.x = this.PixelInsetX.Value;
                    pixelInset.y = this.PixelInsetY.Value;
                }
                ownerDefaultTarget.guiTexture.pixelInset = pixelInset;
            }
        }

        public override void OnEnter()
        {
            this.DoGUITexturePixelInsetPosition();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGUITexturePixelInsetPosition();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.PixelInsetX = null;
            this.PixelInsetY = null;
            this.AsIncrement = null;
            this.everyFrame = false;
        }
    }
}

