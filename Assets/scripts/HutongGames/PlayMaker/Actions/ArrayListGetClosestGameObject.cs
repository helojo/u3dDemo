namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [Tooltip("Return the closest GameObject within an arrayList from a transform or position."), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListGetClosestGameObject : ArrayListActions
    {
        [UIHint(UIHint.Variable), ActionSection("Result")]
        public FsmGameObject closestGameObject;
        [UIHint(UIHint.Variable)]
        public FsmInt closestIndex;
        [Tooltip("Compare the distance of the items in the list to the position of this gameObject")]
        public FsmGameObject distanceFrom;
        public bool everyframe;
        [ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy)), RequiredField]
        public FsmOwnerDefault gameObject;
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
                        if (current != null)
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
        }
    }
}

