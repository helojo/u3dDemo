namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("unlock gui system."), ActionCategory("MTD-GUI")]
    public class ShowMainUIFuncList : FsmStateAction
    {
        [RequiredField]
        public FsmBool Show;

        public override void OnEnter()
        {
            CommonFunc.ShowFuncList(this.Show.Value);
            base.Finish();
        }

        public override void Reset()
        {
            this.Show = 1;
        }
    }
}

