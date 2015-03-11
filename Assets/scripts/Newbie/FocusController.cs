namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class FocusController : GuideController
    {
        [CompilerGenerated]
        private static Action <>f__am$cache0;

        public FocusController()
        {
            GuideFSM fSM = base.FSM;
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Action(FocusController.<FocusController>m__506);
            }
            fSM.transition_awake = (Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache0);
            GuideFSM efsm2 = base.FSM;
            efsm2.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm2.transition_generate, new Action<List<GameObject>>(this.<FocusController>m__507));
            GuideFSM efsm3 = base.FSM;
            efsm3.transition_cancel = (Action) Delegate.Combine(efsm3.transition_cancel, new Action(this.<FocusController>m__508));
        }

        [CompilerGenerated]
        private static void <FocusController>m__506()
        {
            Utility.NewbiestLock();
        }

        [CompilerGenerated]
        private void <FocusController>m__507(List<GameObject> focuses)
        {
            Utility.NewbiestUnlock();
            FocusMask visual = base.visual as FocusMask;
            if (visual != null)
            {
                visual.Visualize(null, focuses);
            }
        }

        [CompilerGenerated]
        private void <FocusController>m__508()
        {
            base.Cancel();
            Utility.NewbiestUnlock();
        }

        public override void BeginStep()
        {
            base.BeginStep();
            base.RequestAwake();
        }
    }
}

