namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD-GUI/Animation"), Tooltip("Attach the drop down animation for gui entity.")]
    public class AttachDropDownGUIAnimation : FsmStateAction
    {
        public FsmFloat duration = 0.5f;

        public override void OnEnter()
        {
            GUIEntity component = base.Owner.GetComponent<GUIEntity>();
            if (null != component)
            {
                component.Animate<GUIDropDownAnimation>().duration = this.duration.Value;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.duration = 0f;
        }
    }
}

