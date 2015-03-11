namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Check if an item is contains in a particula PlayMaker ArrayList Proxy component")]
    public class ArrayListContains : ArrayListActions
    {
        [RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up"), CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;
        [Tooltip("Store in a bool wether it contains or not that element (described below)"), ActionSection("Result"), UIHint(UIHint.Variable)]
        public FsmBool isContained;
        [UIHint(UIHint.FsmEvent), Tooltip("Event sent if this arraList contains that element ( described below)")]
        public FsmEvent isContainedEvent;
        [Tooltip("Event sent if this arraList does not contains that element ( described below)"), UIHint(UIHint.FsmEvent)]
        public FsmEvent isNotContainedEvent;
        [UIHint(UIHint.FsmString), Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
        public FsmString reference;
        [ActionSection("Data"), RequiredField, Tooltip("The variable to check.")]
        public FsmVar variable;

        public void doesArrayListContains()
        {
            if (base.isProxyValid())
            {
                bool flag = false;
                PlayMakerUtils.RefreshValueFromFsmVar(base.Fsm, this.variable);
                switch (this.variable.Type)
                {
                    case VariableType.Float:
                        flag = base.proxy.arrayList.Contains(this.variable.floatValue);
                        break;

                    case VariableType.Int:
                        flag = base.proxy.arrayList.Contains(this.variable.intValue);
                        break;

                    case VariableType.Bool:
                        flag = base.proxy.arrayList.Contains(this.variable.boolValue);
                        break;

                    case VariableType.GameObject:
                        flag = base.proxy.arrayList.Contains(this.variable.gameObjectValue);
                        break;

                    case VariableType.String:
                        flag = base.proxy.arrayList.Contains(this.variable.stringValue);
                        break;

                    case VariableType.Vector2:
                        flag = base.proxy.arrayList.Contains(this.variable.vector2Value);
                        break;

                    case VariableType.Vector3:
                        flag = base.proxy.arrayList.Contains(this.variable.vector3Value);
                        break;

                    case VariableType.Color:
                        flag = base.proxy.arrayList.Contains(this.variable.colorValue);
                        break;

                    case VariableType.Rect:
                        flag = base.proxy.arrayList.Contains(this.variable.rectValue);
                        break;

                    case VariableType.Material:
                        flag = base.proxy.arrayList.Contains(this.variable.materialValue);
                        break;

                    case VariableType.Texture:
                        flag = base.proxy.arrayList.Contains(this.variable.textureValue);
                        break;

                    case VariableType.Quaternion:
                        flag = base.proxy.arrayList.Contains(this.variable.quaternionValue);
                        break;

                    case VariableType.Object:
                        flag = base.proxy.arrayList.Contains(this.variable.objectReference);
                        break;
                }
                this.isContained.Value = flag;
                if (flag)
                {
                    base.Fsm.Event(this.isContainedEvent);
                }
                else
                {
                    base.Fsm.Event(this.isNotContainedEvent);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.doesArrayListContains();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.variable = null;
            this.isContained = null;
            this.isContainedEvent = null;
            this.isNotContainedEvent = null;
        }
    }
}

