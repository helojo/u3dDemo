namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Mute/unmute the Audio Clip played by an Audio Source component on a Game Object."), ActionCategory(ActionCategory.Audio)]
    public class AudioMute : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(AudioSource)), Tooltip("The GameObject with an Audio Source component.")]
        public FsmOwnerDefault gameObject;
        [RequiredField, Tooltip("Check to mute, uncheck to unmute.")]
        public FsmBool mute;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                AudioSource audio = ownerDefaultTarget.audio;
                if (audio != null)
                {
                    audio.mute = this.mute.Value;
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.mute = 0;
        }
    }
}

