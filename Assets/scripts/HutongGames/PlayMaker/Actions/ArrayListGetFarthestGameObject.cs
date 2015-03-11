namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Return the farthest GameObject within an arrayList from a transform or position.")]
    public class ArrayListGetFarthestGameObject : ArrayListActions
    {
        [Tooltip("Compare the distance of the items in the list to the position of this gameObject")]
        public FsmGameObject distanceFrom;
        public bool everyframe;
        [UIHint(UIHint.Variable), ActionSection("Result")]
        public FsmGameObject farthestGameObject;
        [UIHint(UIHint.Variable)]
        public FsmInt farthestIndex;
        [RequiredField, ActionSection("Set up"), CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        public FsmOwnerDefault gameObject;
        [Tooltip("If DistanceFrom declared, use OrDistanceFromVector3 as an offset")]
        public FsmVector3 orDistanceFromVector3;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        private void DoFindFarthestGo()
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
                                this.farthestGameObject.Value = current;
                                this.farthestIndex.Value = num2;
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
            this.DoFindFarthestGo();
            if (!this.everyframe)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoFindFarthestGo();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.distanceFrom = null;
            this.orDistanceFromVector3 = null;
            this.farthestGameObject = null;
            this.farthestIndex = null;
            this.everyframe = true;
        }
    }
}

