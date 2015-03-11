namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("tutorials when born"), ActionCategory("MTD")]
    public class ResumeMusic : FsmStateAction
    {
        public FsmFloat time;

        public override void OnEnter()
        {
            SoundManager.mInstance.ResumeMusic(this.time.Value);
            base.Finish();
        }
    }
}

