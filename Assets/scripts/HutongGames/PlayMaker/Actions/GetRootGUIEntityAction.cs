namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("get root gui entity."), ActionCategory("MTD-GUI")]
    public class GetRootGUIEntityAction : FsmStateAction
    {
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmGameObject storeEntity;
        [RequiredField]
        public FsmOwnerDefault target;

        public override void OnEnter()
        {
            Transform parent;
            for (GameObject obj2 = base.Fsm.GetOwnerDefaultTarget(this.target); null != obj2.transform.parent; obj2 = parent.gameObject)
            {
                parent = obj2.transform.parent;
                if (null != parent.GetComponent<GUIEntity>())
                {
                    this.storeEntity.Value = parent.gameObject;
                    break;
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.target = null;
            this.storeEntity = null;
        }
    }
}

