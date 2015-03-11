namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TalkBoxController : GuideController
    {
        public TalkBoxController(int talk_identity)
        {
            TalkBox box = new TalkBox();
            base.visual = box;
            box.identity = talk_identity;
            box.talk_over = new Action(this.<TalkBoxController>m__504);
            base.FSM.valid_tag = "guide_tag_talk_box";
            base.FSM.transition_generate = new Action<List<GameObject>>(this.<TalkBoxController>m__505);
        }

        [CompilerGenerated]
        private void <TalkBoxController>m__504()
        {
            base.RequestUIResponse("guide_tag_talk_box", null);
            base.GoNext();
        }

        [CompilerGenerated]
        private void <TalkBoxController>m__505(List<GameObject> focuses)
        {
            base.visual.Visualize(null, focuses);
        }

        public override void BeginStep()
        {
            base.BeginStep();
            base.RequestAwake();
            base.RequestGeneration("guide_tag_talk_box", null);
        }
    }
}

