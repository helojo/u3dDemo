namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using Toolbox;

    [Tooltip("Select GuildDup Trench Type Action"), ActionCategory("MTD")]
    public class SelectGuildDupTrenchTypeAction : FsmStateAction
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (XSingleton<GameGuildMgr>.Singleton.UIOpenType == OpenType.FromTrench)
            {
                base.Fsm.Event("GO_TO_TRENCH");
            }
            else
            {
                base.Fsm.Event("GO_TO_GUILD");
            }
            base.Finish();
        }
    }
}

