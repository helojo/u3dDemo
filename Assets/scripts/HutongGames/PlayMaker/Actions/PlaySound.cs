namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("play background music"), ActionCategory("MTD")]
    public class playSound : FsmStateAction
    {
        public FsmEvent finishEvent;
        [RequiredField, Tooltip("SoundMgr play sound name")]
        public FsmString soundName;

        public override void OnEnter()
        {
            if (this.soundName == null)
            {
                base.Finish();
            }
            SoundManager.mInstance.PlayMusic(this.soundName.Value);
            base.Finish();
            if (this.finishEvent != null)
            {
                base.Fsm.Event(this.finishEvent);
            }
        }

        public override void OnUpdate()
        {
        }

        public override void Reset()
        {
            this.soundName = null;
        }
    }
}

