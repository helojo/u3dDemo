namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Attach the transparent animation for gui entity."), ActionCategory("MTD-GUI/Animation")]
    public class AttachTransparentGUIAnimation : FsmStateAction
    {
        public FsmFloat duration;
        public FsmFloat from;
        public FsmFloat to;

        public override void OnEnter()
        {
            GUIEntity component = base.Owner.GetComponent<GUIEntity>();
            if (null != component)
            {
                GUITranspanetAnimation animation = component.Animate<GUITranspanetAnimation>();
                animation.duration = this.duration.Value;
                animation.alphaMin = this.from.Value;
                animation.alphaMax = this.to.Value;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.duration = 0.5f;
            this.from = 0f;
            this.to = 1f;
        }
    }
}

