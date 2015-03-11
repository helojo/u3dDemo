namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Return the average value within an arrayList. It can use float, int, vector2 and vector3 ( uses magnitude), rect ( uses surface), gameobject ( using bounding box volume), and string ( use lenght)")]
    public class ArrayListGetAverageValue : ArrayListActions
    {
        private List<float> _floats;
        [CompilerGenerated]
        private static Func<float, float, float> <>f__am$cache5;
        [UIHint(UIHint.Variable), Tooltip("The average Value"), ActionSection("Result"), RequiredField]
        public FsmFloat averageValue;
        [Tooltip("Performs every frame. WARNING, it could be affecting performances.")]
        public bool everyframe;
        [ActionSection("Set up"), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy)), RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        private void DoGetAverageValue()
        {
            if (base.isProxyValid())
            {
                this._floats = new List<float>();
                IEnumerator enumerator = base.proxy.arrayList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        try
                        {
                            this._floats.Add(Convert.ToSingle(current));
                            continue;
                        }
                        finally
                        {
                        }
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
                if (this._floats.Count > 0)
                {
                    if (<>f__am$cache5 == null)
                    {
                        <>f__am$cache5 = (acc, cur) => acc + cur;
                    }
                    this.averageValue.Value = this._floats.Aggregate<float>(<>f__am$cache5) / ((float) this._floats.Count);
                }
                else
                {
                    this.averageValue.Value = 0f;
                }
            }
        }

        public override void OnEnter()
        {
            if (!base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                base.Finish();
            }
            this.DoGetAverageValue();
            if (!this.everyframe)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoGetAverageValue();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.averageValue = null;
            this.everyframe = true;
        }
    }
}

