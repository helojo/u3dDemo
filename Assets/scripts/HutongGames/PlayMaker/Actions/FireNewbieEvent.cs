namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using Newbie;
    using System;

    [Tooltip("fire newbie event"), ActionCategory("MTD")]
    public class FireNewbieEvent : FsmStateAction
    {
        [RequiredField, Tooltip("Event")]
        public FsmInt EventID;

        public override void OnEnter()
        {
            GuideSystem.FireEvent((GuideEvent) this.EventID.Value);
            base.Finish();
        }
    }
}

