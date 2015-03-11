namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("unlock gui system."), ActionCategory("MTD-GUI")]
    public class ShowTitleBar : FsmStateAction
    {
        [RequiredField]
        public FsmBool Show;

        public override void OnEnter()
        {
            CommonFunc.ShowTitlebar(this.Show.Value);
            base.Finish();
        }

        public override void Reset()
        {
            this.Show = 1;
        }
    }
}

