namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Change Fill Amount of a Ngui Sprite.")]
    public class nguiChangeSpriteFill : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        public UIBasicSprite.FillDirection fillDir = UIBasicSprite.FillDirection.Radial360;
        [HasFloatSlider(0f, 1f), Tooltip("Input your input Fill Amount")]
        public FsmFloat FillValue;
        public UIBasicSprite.Flip flip;
        [Tooltip("NGUI Sprite to change Alpha"), RequiredField]
        public FsmOwnerDefault NguiSpriteObject;
        private UISprite theNguiSprite;

        private void doChangeAlPha()
        {
            this.theNguiSprite.fillAmount = this.FillValue.Value;
        }

        public override void OnEnter()
        {
            if (this.NguiSpriteObject == null)
            {
                base.Finish();
            }
            this.theNguiSprite = base.Fsm.GetOwnerDefaultTarget(this.NguiSpriteObject).GetComponent<UISprite>();
            this.theNguiSprite.type = UIBasicSprite.Type.Filled;
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
            this.FillValue = 0.5f;
            this.everyFrame = true;
        }
    }
}

