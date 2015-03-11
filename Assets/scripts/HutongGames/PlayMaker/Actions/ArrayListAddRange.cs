namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Add several items to a PlayMaker Array List Proxy component")]
    public class ArrayListAddRange : ArrayListActions
    {
        [RequiredField, CheckForComponent(typeof(PlayMakerArrayListProxy)), Tooltip("The gameObject with the PlayMaker ArrayList Proxy component"), ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [UIHint(UIHint.FsmString), Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
        public FsmString reference;
        [Tooltip("The variables to add."), ActionSection("Data"), RequiredField]
        public FsmVar[] variables;

        public void DoArrayListAddRange()
        {
            if (base.isProxyValid())
            {
                foreach (FsmVar var in this.variables)
                {
                    base.proxy.Add(PlayMakerUtils.GetValueFromFsmVar(base.Fsm, var), var.Type.ToString(), true);
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoArrayListAddRange();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.variables = new FsmVar[2];
        }
    }
}

