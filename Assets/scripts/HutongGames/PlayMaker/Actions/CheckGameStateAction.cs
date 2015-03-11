namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD"), Tooltip("check game state")]
    public class CheckGameStateAction : FsmStateAction
    {
        private FsmEvent Check()
        {
            FsmString str = base.Fsm.Variables.FindFsmString("TransitionTrigger");
            if (str != null)
            {
                FsmTransition[] transitions = base.State.Transitions;
                int index = 0;
                int length = transitions.Length;
                while (index != length)
                {
                    FsmTransition transition = transitions[index];
                    if ((transition.FsmEvent != null) && (transition.FsmEvent.Name == str.Value))
                    {
                        return transition.FsmEvent;
                    }
                    index++;
                }
            }
            return null;
        }

        public override void OnUpdate()
        {
            FsmEvent fsmEvent = this.Check();
            if (fsmEvent != null)
            {
                base.Fsm.Event(fsmEvent);
                base.Finish();
            }
        }
    }
}

