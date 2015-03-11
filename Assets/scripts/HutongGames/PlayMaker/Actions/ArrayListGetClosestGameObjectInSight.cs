namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [Tooltip("Return the closest GameObject within an arrayList from a transform or position which does not have a collider between itself and another GameObject"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListGetClosestGameObjectInSight : ArrayListActions
    {
        [UIHint(UIHint.Variable), ActionSection("Result")]
        public FsmGameObject closestGameObject;
        [UIHint(UIHint.Variable)]
        public FsmInt closestIndex;
        [Tooltip("Compare the distance of the items in the list to the position of this gameObject")]
        public FsmGameObject distanceFrom;
        public bool everyframe;
        [Tooltip("The line start of the sweep."), ActionSection("Raycast Settings")]
        public FsmOwnerDefault fromGameObject;
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
        public FsmBool invertMask;
        [UIHint(UIHint.Layer), Tooltip("Pick only from these layers.")]
        public FsmInt[] layerMask;
        [Tooltip("If DistanceFrom declared, use OrDistanceFromVector3 as an offset")]
        public FsmVector3 orDistanceFromVector3;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        private void DoFindClosestGo()
        {
            if (base.isProxyValid())
            {
                Vector3 vector = this.orDistanceFromVector3.Value;
                GameObject obj2 = this.distanceFrom.Value;
                if (obj2 != null)
                {
                    vector += obj2.transform.position;
                }
                float positiveInfinity = float.PositiveInfinity;
                int num2 = 0;
                IEnumerator enumerator = base.proxy.arrayList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        GameObject current = (GameObject) enumerator.Current;
                        if ((current != null) && this.DoLineCast(current))
                        {
                            Vector3 vector2 = current.transform.position - vector;
                            float sqrMagnitude = vector2.sqrMagnitude;
                            if (sqrMagnitude <= positiveInfinity)
                            {
                                positiveInfinity = sqrMagnitude;
                                this.closestGameObject.Value = current;
                                this.closestIndex.Value = num2;
                            }
                        }
                        num2++;
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
            }
        }

        private bool DoLineCast(GameObject toGameObject)
        {
            RaycastHit hit;
            Vector3 position = base.Fsm.GetOwnerDefaultTarget(this.fromGameObject).transform.position;
            Vector3 end = toGameObject.transform.position;
            bool flag = !Physics.Linecast(position, end, out hit, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
            base.Fsm.RaycastHitInfo = hit;
            return flag;
        }

        public override void OnEnter()
        {
            if (!base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                base.Finish();
            }
            this.DoFindClosestGo();
            if (!this.everyframe)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoFindClosestGo();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.distanceFrom = null;
            this.orDistanceFromVector3 = null;
            this.closestGameObject = null;
            this.closestIndex = null;
            this.everyframe = true;
            this.fromGameObject = null;
            this.layerMask = new FsmInt[0];
            this.invertMask = 0;
        }
    }
}

