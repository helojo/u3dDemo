namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Attach the slide animation for gui entity."), ActionCategory("MTD-GUI/Animation")]
    public class AttachSlideGUIAnimation : FsmStateAction
    {
        public FsmFloat duration = 0.5f;

        public override void OnEnter()
        {
            GUIEntity component = base.Owner.GetComponent<GUIEntity>();
            if (null != component)
            {
                component.Animate<GUISlide>().duration = this.duration.Value;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.duration = 0f;
        }
    }
}

