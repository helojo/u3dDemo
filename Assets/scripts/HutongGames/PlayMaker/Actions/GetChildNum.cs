namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), Tooltip("Gets the Child of a GameObject by Index.\nE.g., O to get the first child. HINT: Use this with an integer variable to iterate through children.")]
    public class GetChildNum : FsmStateAction
    {
        [Tooltip("The index of the child to find."), RequiredField]
        public FsmInt childIndex;
        [Tooltip("The GameObject to search."), RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("Store the child in a GameObject variable."), UIHint(UIHint.Variable), RequiredField]
        public FsmGameObject store;

        private GameObject DoGetChildNum(GameObject go)
        {
            return ((go != null) ? go.transform.GetChild(this.childIndex.Value % go.transform.childCount).gameObject : null);
        }

        public override void OnEnter()
        {
            this.store.Value = this.DoGetChildNum(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.childIndex = 0;
            this.store = null;
        }
    }
}

