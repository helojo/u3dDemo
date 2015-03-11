namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("tutorials when born"), ActionCategory("MTD")]
    public class StopMusic : FsmStateAction
    {
        public FsmFloat time;

        public override void OnEnter()
        {
            SoundManager.mInstance.StopMusic(this.time.Value);
            base.Finish();
        }
    }
}

