namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Material), Tooltip("Sets the Offset of a named texture in a Game Object's Material. Useful for scrolling texture effects.")]
    public class SetTextureOffset : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField, CheckForComponent(typeof(Renderer))]
        public FsmOwnerDefault gameObject;
        public FsmInt materialIndex;
        [UIHint(UIHint.NamedColor), RequiredField]
        public FsmString namedTexture;
        [RequiredField]
        public FsmFloat offsetX;
        [RequiredField]
        public FsmFloat offsetY;

        private void DoSetTextureOffset()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                if (ownerDefaultTarget.renderer == null)
                {
                    this.LogError("Missing Renderer!");
                }
                else if (ownerDefaultTarget.renderer.material == null)
                {
                    this.LogError("Missing Material!");
                }
                else if (this.materialIndex.Value == 0)
                {
                    ownerDefaultTarget.renderer.material.SetTextureOffset(this.namedTexture.Value, new Vector2(this.offsetX.Value, this.offsetY.Value));
                }
                else if (ownerDefaultTarget.renderer.materials.Length > this.materialIndex.Value)
                {
                    Material[] materials = ownerDefaultTarget.renderer.materials;
                    materials[this.materialIndex.Value].SetTextureOffset(this.namedTexture.Value, new Vector2(this.offsetX.Value, this.offsetY.Value));
                    ownerDefaultTarget.renderer.materials = materials;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetTextureOffset();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetTextureOffset();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.materialIndex = 0;
            this.namedTexture = "_MainTex";
            this.offsetX = 0f;
            this.offsetY = 0f;
            this.everyFrame = false;
        }
    }
}

