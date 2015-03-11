namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Input), Tooltip("Sends an Event when the user hits any Key or Mouse Button.")]
    public class AnyKeyStoreString : FsmStateAction
    {
        [RequiredField]
        public FsmEvent sendEvent;
        [UIHint(UIHint.Variable)]
        public FsmString storeResult;

        public override void OnUpdate()
        {
            this.storeResult.Value = Input.inputString;
            if (Input.anyKeyDown)
            {
                base.Fsm.Event(this.sendEvent);
            }
        }

        public override void Reset()
        {
            this.sendEvent = null;
            this.storeResult = null;
        }
    }
}

