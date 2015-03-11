namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("change game state"), ActionCategory("MTD")]
    public class ChangeGameStateAction : FsmStateAction
    {
        [RequiredField]
        public FsmString eventName = string.Empty;

        public override void OnEnter()
        {
            if (this.eventName.Value != null)
            {
                GameStateMgr.Instance.ChangeState(this.eventName.Value);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.eventName = string.Empty;
        }
    }
}

