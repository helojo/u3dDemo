namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections.Generic;

    [ActionCategory(ActionCategory.StateMachine), Tooltip("Sets Event Data before sending an event. Get the Event Data, Get Event Properties action.")]
    public class SetEventProperties : FsmStateAction
    {
        public FsmVar[] datas;
        [CompoundArray("Event Properties", "Key", "Data")]
        public FsmString[] keys;
        public static Dictionary<string, object> properties;

        public override void OnEnter()
        {
            properties = new Dictionary<string, object>();
            for (int i = 0; i < this.keys.Length; i++)
            {
                properties[this.keys[i].Value] = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, this.datas[i]);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.keys = new FsmString[1];
            this.datas = new FsmVar[1];
        }
    }
}

