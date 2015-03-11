namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Gets the event that caused the transition to the current state, and stores it in a String Variable."), ActionCategory(ActionCategory.StateMachine)]
    public class GetDayOrNightEvent : FsmStateAction
    {
        public override void OnEnter()
        {
            PlayMakerFSM component = GUIMgr.Instance.Root.GetComponent<PlayMakerFSM>();
            if (null != component)
            {
                FsmString str = component.FsmVariables.FindFsmString("zjmSceneName");
                if ((TimeMgr.Instance.ServerDateTime.Hour >= 7) && (TimeMgr.Instance.ServerDateTime.Hour <= 0x11))
                {
                    str.Value = "cj_xzjm2_baitian";
                    Debug.Log(TimeMgr.Instance.ServerDateTime.Hour + "   <---------------");
                }
                else
                {
                    str.Value = "cj_xzjm2";
                }
            }
        }

        [UIHint(UIHint.Variable)]
        public override void Reset()
        {
        }
    }
}

