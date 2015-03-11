namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD-GUI"), Tooltip("close an model gui entity.")]
    public class ExitModelGUIAction : FsmStateAction
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
                    GUIMgr.Instance.ExitModelGUI(component);
                    base.Finish();
                    return;
                }
            }
            GUIMgr.Instance.ExitModelGUI(this.Scene.Value);
            base.Finish();
        }

        public override void Reset()
        {
            this.Scene = string.Empty;
            this.Entity = null;
        }
    }
}

