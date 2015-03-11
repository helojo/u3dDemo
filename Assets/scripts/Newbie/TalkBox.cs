namespace Newbie
{
    using System;
    using System.Collections.Generic;

    public class TalkBox : Visualization
    {
        public int identity = -1;
        public System.Action talk_over;

        private void OnCreateEntity(NewbieEntity entity)
        {
            entity.ShowTalkBox(this.identity, new System.Action(this.OnTerminate));
        }

        private void OnTerminate()
        {
            if (this.talk_over != null)
            {
                this.talk_over();
            }
        }

        public override void Visualize(object vari, List<GameObject> host)
        {
            base.CreateEntity(new Action<NewbieEntity>(this.OnCreateEntity));
        }
    }
}

