namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Attach the cascade naviable for gui entity."), ActionCategory("MTD-GUI/Animation")]
    public class AttachCascadeNaviable : FsmStateAction
    {
        public override void OnEnter()
        {
            GUIEntity component = base.Owner.GetComponent<GUIEntity>();
            if ((null != component) && (null == component.GetComponent<GUICascadeNaviable>()))
            {
                component.gameObject.AddComponent<GUICascadeNaviable>();
            }
            base.Finish();
        }
    }
}

