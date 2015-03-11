namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using Newbie;
    using System;

    [Tooltip("fire newbie event"), ActionCategory("MTD")]
    public class NewbieEventCompletedEvent : FsmStateAction
    {
        [RequiredField, Tooltip("Event")]
        public FsmInt EventID;
        [RequiredField, UIHint(UIHint.Variable)]
        public FsmBool storeValue;

        public override void OnEnter()
        {
            this.storeValue.Value = GuideSystem.Finished((GuideEvent) this.EventID.Value);
            base.Finish();
        }
    }
}

