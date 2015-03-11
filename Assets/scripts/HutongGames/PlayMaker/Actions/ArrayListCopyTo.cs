namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [Tooltip("Copy elements from one PlayMaker ArrayList Proxy component to another"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListCopyTo : ArrayListActions
    {
        [Tooltip("Optional amount of elements to copy, If not set, will copy all from start index.")]
        public FsmInt count;
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component to copy from"), RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy)), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy)), ActionSection("Result"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component to copy to")]
        public FsmOwnerDefault gameObjectTarget;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy from ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy to ( necessary if several component coexists on the same GameObject")]
        public FsmString referenceTarget;
        [Tooltip("Optional start index to copy from the source, if not set, starts from the beginning")]
        public FsmInt startIndex;

        public void DoArrayListCopyTo(ArrayList source)
        {
            if ((base.isProxyValid() && base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObjectTarget), this.referenceTarget.Value)) && base.isProxyValid())
            {
                int num = this.startIndex.Value;
                int count = source.Count;
                int num3 = source.Count;
                if (!this.count.IsNone)
                {
                    num3 = this.count.Value;
                }
                if ((num < 0) || (num >= source.Count))
                {
                    this.LogError("start index out of range");
                }
                else if (this.count.Value < 0)
                {
                    this.LogError("count can not be negative");
                }
                else
                {
                    count = Mathf.Min(num + num3, source.Count);
                    for (int i = num; i < count; i++)
                    {
                        base.proxy.arrayList.Add(source[i]);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoArrayListCopyTo(base.proxy.arrayList);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.gameObjectTarget = null;
            this.referenceTarget = null;
            FsmInt num = new FsmInt {
                UseVariable = true
            };
            this.startIndex = num;
            num = new FsmInt {
                UseVariable = true
            };
            this.count = num;
        }
    }
}

