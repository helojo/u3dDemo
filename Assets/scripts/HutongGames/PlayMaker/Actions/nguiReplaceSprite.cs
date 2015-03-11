namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("replace/change Ngui Sprite. The new sprite must exist in current Atlas"), ActionCategory("Ngui Actions")]
    public class nguiReplaceSprite : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("New Sprite name")]
        public FsmString newSpriteName;
        [RequiredField, Tooltip("NGUI Sprite to change")]
        public FsmOwnerDefault NguiSpriteObject;
        private UISprite theNguiSprite;

        private void doChangeSprite()
        {
            this.theNguiSprite.spriteName = this.newSpriteName.Value;
        }

        public override void OnEnter()
        {
            if ((this.NguiSpriteObject == null) || (this.newSpriteName.Value == null))
            {
                base.Finish();
            }
            this.theNguiSprite = base.Fsm.GetOwnerDefaultTarget(this.NguiSpriteObject).GetComponent<UISprite>();
            if (this.theNguiSprite != null)
            {
                this.doChangeSprite();
                if (!this.everyFrame)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.doChangeSprite();
        }

        public override void Reset()
        {
            this.NguiSpriteObject = null;
            this.everyFrame = false;
            this.newSpriteName = null;
        }
    }
}

