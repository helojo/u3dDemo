namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("close an unique gui entity."), ActionCategory("MTD-GUI")]
    public class CloseUniqueGUIAction : FsmStateAction
    {
        public FsmGameObject Entity;
        public FsmString Scene;

        public override void OnEnter()
        {
            if (null != this.Entity.Value)
            {
                GUIEntity component = this.Entity.Value.GetComponent<GUIEntity>();
                if (null != component)
                {
                    GUIMgr.Instance.CloseUniqueGUIEntity(component);
                    base.Finish();
                    return;
                }
            }
            GUIMgr.Instance.CloseUniqueGUIEntity(this.Scene.Value);
            base.Finish();
        }

        public override void Reset()
        {
            this.Scene = string.Empty;
            this.Entity = null;
        }
    }
}

