namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class Visualization
    {
        protected Visualization()
        {
        }

        public void CreateEntity(Action<NewbieEntity> call_back)
        {
            <CreateEntity>c__AnonStorey243 storey = new <CreateEntity>c__AnonStorey243 {
                call_back = call_back
            };
            NewbieEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<NewbieEntity>();
            if (null != activityGUIEntity)
            {
                if (storey.call_back != null)
                {
                    storey.call_back(activityGUIEntity);
                }
            }
            else
            {
                GUIMgr.Instance.OpenUniqueGUIEntity("NewbieEntity", new Action<GUIEntity>(storey.<>m__50C));
            }
        }

        public abstract void Visualize(object vari, List<GameObject> host);

        [CompilerGenerated]
        private sealed class <CreateEntity>c__AnonStorey243
        {
            internal Action<NewbieEntity> call_back;

            internal void <>m__50C(GUIEntity e)
            {
                e.Depth = 0x3e8;
                if (this.call_back != null)
                {
                    this.call_back(e as NewbieEntity);
                }
            }
        }
    }
}

