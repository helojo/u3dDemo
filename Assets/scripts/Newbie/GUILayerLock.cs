namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GUILayerLock : Visualization
    {
        [CompilerGenerated]
        private static Action<NewbieEntity> <>f__am$cache0;

        public override void Visualize(object vari, List<GameObject> host)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = entity => entity.LockAllGUI();
            }
            base.CreateEntity(<>f__am$cache0);
        }
    }
}

