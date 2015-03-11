namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Sets looping on the AudioSource component on a Game Object."), ActionCategory(ActionCategory.Audio)]
    public class SetAudioLoop : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(AudioSource))]
        public FsmOwnerDefault gameObject;
        public FsmBool loop;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                AudioSource audio = ownerDefaultTarget.audio;
                if (audio != null)
                {
                    audio.loop = this.loop.Value;
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.loop = 0;
        }
    }
}

