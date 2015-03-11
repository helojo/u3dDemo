namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("MTD-GUI"), Tooltip("open an model gui entity.")]
    public class DoModelGUIAction : FsmStateAction
    {
        public FsmBool Docked;
        public FsmEvent onCompleted;
        [RequiredField]
        public FsmString Scene;
        public FsmBool Unique;

        public override void OnEnter()
        {
            if (this.Unique.Value && !string.IsNullOrEmpty(this.Scene.Value))
            {
                GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity(this.Scene.Value);
                if ((null != gUIEntity) && gUIEntity.gameObject.activeSelf)
                {
                    base.Fsm.Event(this.onCompleted);
                    base.Finish();
                    return;
                }
            }
            GameObject dock = null;
            if (this.Docked.Value)
            {
                GameObject owner = base.Owner;
                while (null != owner.transform.parent)
                {
                    Transform parent = owner.transform.parent;
                    owner = parent.gameObject;
                    if (null != parent.GetComponent<GUIEntity>())
                    {
                        dock = owner;
                        break;
                    }
                }
            }
            GUIMgr.Instance.DoModelGUI(this.Scene.Value, delegate (GUIEntity entity) {
                base.Fsm.Event(this.onCompleted);
                base.Finish();
            }, dock);
            if (this.onCompleted == null)
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.Scene = string.Empty;
            this.onCompleted = null;
            this.Docked = 1;
            this.Unique = 1;
        }
    }
}

