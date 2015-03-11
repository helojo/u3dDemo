namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Gets the camera tagged MainCamera from the scene"), ActionCategory(ActionCategory.Camera)]
    public class GetMainCamera : FsmStateAction
    {
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmGameObject storeGameObject;

        public override void OnEnter()
        {
            this.storeGameObject.Value = (Camera.main == null) ? null : Camera.main.gameObject;
            base.Finish();
        }

        public override void Reset()
        {
            this.storeGameObject = null;
        }
    }
}

