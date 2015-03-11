namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Exit Guild dup battle"), ActionCategory("MTD")]
    public class ExitGuildDupBattleAction : FsmStateAction
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GuildDupTrenchMap gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupTrenchMap>();
            if (gUIEntity != null)
            {
                gUIEntity.ShowLast();
            }
            base.Finish();
        }
    }
}

