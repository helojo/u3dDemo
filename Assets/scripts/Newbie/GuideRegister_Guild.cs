namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Guild
    {
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache0;

        public static GuideController GenGuildContrller()
        {
            <GenGuildContrller>c__AnonStorey230 storey = new <GenGuildContrller>c__AnonStorey230();
            TalkBoxController controller = new TalkBoxController(70);
            storey.ctrl_talk1 = new TalkBoxController(0x4f);
            GuideFSM fSM = storey.ctrl_talk1.FSM;
            fSM.transition_response = (Action<GameObject>) Delegate.Combine(fSM.transition_response, new Action<GameObject>(storey.<>m__4B4));
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = () => ActorData.getInstance().Level >= 10;
            }
            controller.FSM.condition_reached = <>f__am$cache0;
            controller.next_step = storey.ctrl_talk1;
            return controller;
        }

        [CompilerGenerated]
        private sealed class <GenGuildContrller>c__AnonStorey230
        {
            internal TalkBoxController ctrl_talk1;

            internal void <>m__4B4(GameObject go)
            {
                GuideSystem.FinishEvent(GuideEvent.Function_Guild);
                this.ctrl_talk1.Complete();
            }
        }
    }
}

