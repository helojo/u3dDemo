namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class FingureGizmo : Visualization
    {
        public bool hang;
        public int layer = LayerMask.NameToLayer("GUI");
        public Vector3 offset = Vector3.zero;

        public FingureGizmo EnableHang(bool value)
        {
            this.hang = value;
            return this;
        }

        public FingureGizmo SetLayer(string layer_name)
        {
            this.layer = LayerMask.NameToLayer(layer_name);
            return this;
        }

        public FingureGizmo SetOffset(Vector3 vec)
        {
            this.offset = vec;
            return this;
        }

        public override void Visualize(object vari, List<GameObject> host)
        {
            <Visualize>c__AnonStorey245 storey = new <Visualize>c__AnonStorey245 {
                host = host,
                <>f__this = this,
                host_bounds = (Bounds) vari
            };
            base.CreateEntity(new Action<NewbieEntity>(storey.<>m__50E));
        }

        [CompilerGenerated]
        private sealed class <Visualize>c__AnonStorey245
        {
            internal FingureGizmo <>f__this;
            internal List<GameObject> host;
            internal Bounds host_bounds;

            internal void <>m__50E(NewbieEntity entity)
            {
                GameObject obj2 = null;
                if (this.<>f__this.hang && (this.host.Count > 0))
                {
                    obj2 = this.host[0];
                }
                entity.ShowFingure(this.host_bounds, this.<>f__this.offset, obj2, this.<>f__this.layer);
            }
        }
    }
}

