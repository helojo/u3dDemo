namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Gets properties on the last event that caused a state change. Use Set Event Properties to define these values when sending events"), ActionCategory(ActionCategory.StateMachine)]
    public class GetEventProperties : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmVar[] datas;
        [CompoundArray("Event Properties", "Key", "Data")]
        public FsmString[] keys;

        public override void OnEnter()
        {
            try
            {
                if (SetEventProperties.properties == null)
                {
                    throw new ArgumentException("no properties");
                }
                for (int i = 0; i < this.keys.Length; i++)
                {
                    Debug.Log(this.keys[i].Value);
                    if (SetEventProperties.properties.ContainsKey(this.keys[i].Value))
                    {
                        Debug.Log("found");
                        PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, this.datas[i], SetEventProperties.properties[this.keys[i].Value]);
                    }
                    else
                    {
                        Debug.Log("not found");
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log("no properties found " + exception);
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

