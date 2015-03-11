namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Change Alpha in Ngui Sprite. This will override Alpha Color and Affect on all of its Children"), ActionCategory("Ngui Actions")]
    public class nguiChangeSpriteAlpha : FsmStateAction
    {
        [HasFloatSlider(0f, 1f), Tooltip("Input your input Alpha")]
        public FsmFloat AlphaValue;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [RequiredField, Tooltip("NGUI Sprite to change Alpha")]
        public FsmOwnerDefault NguiSpriteObject;
        private UISprite theNguiSprite;

        private void doChangeAlPha()
        {
            this.theNguiSprite.alpha = this.AlphaValue.Value;
        }

        public override void OnEnter()
        {
            if (this.NguiSpriteObject == null)
            {
                base.Finish();
            }
            this.theNguiSprite = base.Fsm.GetOwnerDefaultTarget(this.NguiSpriteObject).GetComponent<UISprite>();
            if (this.theNguiSprite != null)
            {
                this.doChangeAlPha();
                if (!this.everyFrame)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.doChangeAlPha();
        }

        public override void Reset()
        {
            this.NguiSpriteObject = null;
            this.everyFrame = false;
        }
    }
}

