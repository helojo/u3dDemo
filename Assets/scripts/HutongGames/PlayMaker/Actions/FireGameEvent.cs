namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("fire game event"), ActionCategory("MTD")]
    public class FireGameEvent : FsmStateAction
    {
        public FsmInt event_id;

        public override void OnEnter()
        {
            EventCenter.Instance.DoEvent((EventCenter.EventType) this.event_id.Value, null);
            base.Finish();
        }
    }
}

