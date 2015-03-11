namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("quit recruit scene."), ActionCategory("MTD-GUI")]
    public class QuitRecruitScene : FsmStateAction
    {
        public override void OnEnter()
        {
            GameDataMgr.Instance.boostRecruit.OnQuit();
        }
    }
}

