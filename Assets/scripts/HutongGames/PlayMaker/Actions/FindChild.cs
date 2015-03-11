namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), Tooltip("Finds the Child of a GameObject by Name.\nNote, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger. If you need to specify a tag, use GetChild.")]
    public class FindChild : FsmStateAction
    {
        [Tooltip("The name of the child. Note, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger"), RequiredField]
        public FsmString childName;
        [Tooltip("The GameObject to search."), RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("Store the child in a GameObject variable."), UIHint(UIHint.Variable), RequiredField]
        public FsmGameObject storeResult;

        private void DoFindChild()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                Transform transform = ownerDefaultTarget.transform.FindChild(this.childName.Value);
                this.storeResult.Value = (transform == null) ? null : transform.gameObject;
            }
        }

        public override void OnEnter()
        {
            this.DoFindChild();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.childName = string.Empty;
            this.storeResult = null;
        }
    }
}

