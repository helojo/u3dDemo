namespace Newbie
{
    using System;

    public class FlexiableController : GuideController
    {
        public FlexiableController()
        {
            base.visual = new NulltyVisualization();
        }

        public override void BeginStep()
        {
            base.BeginStep();
            base.RequestAwake();
        }
    }
}

