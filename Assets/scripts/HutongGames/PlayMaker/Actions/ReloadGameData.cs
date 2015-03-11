namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD"), Tooltip("reload the game data")]
    public class ReloadGameData : FsmStateAction
    {
        public override void OnEnter()
        {
            GameDataMgr.Instance.Quit();
            base.Finish();
        }
    }
}

