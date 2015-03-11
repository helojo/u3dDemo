namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Return the minimum value within an arrayList. It can use float, int, vector2 and vector3 ( uses magnitude), rect ( uses surface), gameobject ( using bounding box volume), and string ( use lenght)")]
    public class ArrayListGetMinValue : ArrayListActions
    {
        [Tooltip("Performs every frame. WARNING, it could be affecting performances.")]
        public bool everyframe;
        [ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("The Minimum Value"), ActionSection("Result"), RequiredField, UIHint(UIHint.Variable)]
        public FsmVar minimumValue;
        [Tooltip("The index of the Maximum Value within that arrayList"), UIHint(UIHint.Variable)]
        public FsmInt minimumValueIndex;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        private static VariableType[] supportedTypes;

        static ArrayListGetMinValue()
        {
            VariableType[] typeArray1 = new VariableType[7];
            typeArray1[1] = VariableType.Int;
            typeArray1[2] = VariableType.Rect;
            typeArray1[3] = VariableType.Vector2;
            typeArray1[4] = VariableType.Vector3;
            typeArray1[5] = VariableType.GameObject;
            typeArray1[6] = VariableType.String;
            supportedTypes = typeArray1;
        }

        private void DoFindMinimumValue()
        {
            if (base.isProxyValid())
            {
                VariableType targetType = this.minimumValue.Type;
                if (supportedTypes.Contains(this.minimumValue.Type))
                {
                    float positiveInfinity = float.PositiveInfinity;
                    int num2 = 0;
                    int num3 = 0;
                    IEnumerator enumerator = base.proxy.arrayList.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            object current = enumerator.Current;
                            try
                            {
                                float num4 = PlayMakerUtils.GetFloatFromObject(current, targetType, true);
                                if (positiveInfinity > num4)
                                {
                                    positiveInfinity = num4;
                                    num2 = num3;
                                }
                            }
                            finally
                            {
                            }
                            num3++;
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
                    this.minimumValueIndex.Value = num2;
                    PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, this.minimumValue, base.proxy.arrayList[num2]);
                }
            }
        }

        public override string ErrorCheck()
        {
            if (!supportedTypes.Contains(this.minimumValue.Type))
            {
                return ("A " + this.minimumValue.Type + " can not be processed as a minimum");
            }
            return string.Empty;
        }

        public override void OnEnter()
        {
            if (!base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                base.Finish();
            }
            this.DoFindMinimumValue();
            if (!this.everyframe)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoFindMinimumValue();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.minimumValue = null;
            this.minimumValueIndex = null;
            this.everyframe = true;
        }
    }
}

