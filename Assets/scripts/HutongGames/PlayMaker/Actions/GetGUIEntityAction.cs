namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("get gui entity."), ActionCategory("MTD-GUI")]
    public class GetGUIEntityAction : FsmStateAction
    {
        [RequiredField]
        public FsmString Scene;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmGameObject storeEntity;

        public override void OnEnter()
        {
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity(this.Scene.Value);
            this.storeEntity.Value = (null != gUIEntity) ? gUIEntity.gameObject : null;
            base.Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            if (this.storeEntity != null)
            {
                this.storeEntity.Value = null;
            }
        }

        public override void Reset()
        {
            this.Scene = string.Empty;
            this.storeEntity = null;
        }
    }
}

