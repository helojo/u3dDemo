namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Concat joins two or more arrayList proxy components. if a target is specified, the method use the target store the concatenation, else the ")]
    public class ArrayListConcat : ArrayListActions
    {
        [CompoundArray("ArrayLists", "ArrayList GameObject", "Reference"), ObjectType(typeof(PlayMakerArrayListProxy)), Tooltip("The GameObject with the PlayMaker ArrayList Proxy component to copy to"), ActionSection("ArrayLists to concatenate"), RequiredField]
        public FsmOwnerDefault[] arrayListGameObjectTargets;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy)), ActionSection("Storage")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to store the concatenation ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy to ( necessary if several component coexists on the same GameObject")]
        public FsmString[] referenceTargets;

        public void DoArrayListConcat(ArrayList source)
        {
            if (base.isProxyValid())
            {
                for (int i = 0; i < this.arrayListGameObjectTargets.Length; i++)
                {
                    if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.arrayListGameObjectTargets[i]), this.referenceTargets[i].Value) && base.isProxyValid())
                    {
                        IEnumerator enumerator = base.proxy.arrayList.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                object current = enumerator.Current;
                                source.Add(current);
                                Debug.Log("count " + source.Count);
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
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoArrayListConcat(base.proxy.arrayList);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.arrayListGameObjectTargets = null;
            this.referenceTargets = null;
        }
    }
}

