namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class HangerMask : FocusMask
    {
        public int hang_layer = LayerMask.NameToLayer("GUI");

        protected override Bounds GenMask(NewbieEntity entity, List<GameObject> host_objects)
        {
            return entity.GenerateHangerMask(base.extract_flag, host_objects, this.hang_layer);
        }
    }
}

