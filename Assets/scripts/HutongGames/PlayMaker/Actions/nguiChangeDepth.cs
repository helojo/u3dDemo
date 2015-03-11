namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("NGUI Object has Depth Value (UIlabel/UISprite/UIButton/UIInput) can be changed. Useful for group, reduce drawcall")]
    public class nguiChangeDepth : FsmStateAction
    {
        [Tooltip("Input your input Depth Number")]
        public FsmInt depthValue;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        private UIButton isButton;
        private UIInput isInput;
        private UILabel isLabel;
        private UISprite isSprite;
        [Tooltip("NGUI Object has Depth Value can be changed"), RequiredField]
        public FsmOwnerDefault NguiObject;

        private void doChangeDepth()
        {
            if (this.isLabel != null)
            {
                this.isLabel.depth = this.depthValue.Value;
            }
            else if (this.isSprite != null)
            {
                this.isSprite.depth = this.depthValue.Value;
            }
            else if (this.isButton != null)
            {
                this.isLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponentInChildren<UILabel>();
                if (this.isLabel != null)
                {
                    this.isLabel.depth = this.depthValue.Value;
                }
                this.isSprite = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponentInChildren<UISprite>();
                if (this.isSprite != null)
                {
                    this.isSprite.depth = this.depthValue.Value;
                }
            }
            else if (this.isInput != null)
            {
                this.isLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponentInChildren<UILabel>();
                if (this.isLabel != null)
                {
                    this.isLabel.depth = this.depthValue.Value;
                }
                this.isSprite = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponentInChildren<UISprite>();
                if (this.isSprite != null)
                {
                    this.isSprite.depth = this.depthValue.Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.isLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<UILabel>();
            this.isSprite = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<UISprite>();
            this.isButton = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<UIButton>();
            this.isInput = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<UIInput>();
            this.doChangeDepth();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.doChangeDepth();
        }

        public override void Reset()
        {
            this.NguiObject = null;
            this.depthValue = 0;
            this.everyFrame = false;
            this.isLabel = null;
            this.isSprite = null;
        }
    }
}

