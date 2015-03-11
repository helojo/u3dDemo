namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [Tooltip("Returns the Gameobject within an arrayList which have the max float value in its FSM"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListGetGameobjectMaxFsmFloatIndex : ArrayListActions
    {
        public bool everyframe;
        private PlayMakerFSM fsm;
        [UIHint(UIHint.FsmName), Tooltip("Optional name of FSM on Game Object")]
        public FsmString fsmName;
        [RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        private GameObject goLastFrame;
        [UIHint(UIHint.Variable)]
        public FsmGameObject maxGameObject;
        [UIHint(UIHint.Variable)]
        public FsmInt maxIndex;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [ActionSection("Result"), UIHint(UIHint.Variable)]
        public FsmFloat storeMaxValue;
        [UIHint(UIHint.FsmFloat), RequiredField]
        public FsmString variableName;

        private void DoFindMaxGo()
        {
            float num = 0f;
            if (!this.storeMaxValue.IsNone && base.isProxyValid())
            {
                int num2 = 0;
                IEnumerator enumerator = base.proxy.arrayList.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        GameObject current = (GameObject) enumerator.Current;
                        if (current != null)
                        {
                            this.fsm = ActionHelpers.GetGameObjectFsm(current, this.fsmName.Value);
                            if (this.fsm == null)
                            {
                                return;
                            }
                            FsmFloat fsmFloat = this.fsm.FsmVariables.GetFsmFloat(this.variableName.Value);
                            if (fsmFloat == null)
                            {
                                return;
                            }
                            if (fsmFloat.Value > num)
                            {
                                this.storeMaxValue.Value = fsmFloat.Value;
                                num = fsmFloat.Value;
                                this.maxGameObject.Value = current;
                                this.maxIndex.Value = num2;
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
            this.DoFindMaxGo();
            if (!this.everyframe)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoFindMaxGo();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.maxGameObject = null;
            this.maxIndex = null;
            this.everyframe = true;
            this.fsmName = string.Empty;
            this.storeMaxValue = null;
        }
    }
}

