namespace Newbie
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Focus2DMask : FocusMask
    {
        protected override Bounds GenMask(NewbieEntity entity, List<GameObject> host_objects)
        {
            return entity.Generate2DMaskSampler(base.extract_flag, host_objects);
        }
    }
}

