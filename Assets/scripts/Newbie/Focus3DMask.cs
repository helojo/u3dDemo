namespace Newbie
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Focus3DMask : FocusMask
    {
        protected override Bounds GenMask(NewbieEntity entity, List<GameObject> host_objects)
        {
            return entity.Generate3DMaskSampler(base.extract_flag, host_objects);
        }
    }
}

