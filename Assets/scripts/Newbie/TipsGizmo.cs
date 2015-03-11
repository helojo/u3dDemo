namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TipsGizmo : Visualization
    {
        public Anchor anchor;
        public bool hang;
        public int identity = -1;
        public int layer = LayerMask.NameToLayer("TipsUI");
        public Vector3 offset = Vector3.zero;
        public bool screen_redirect = true;

        public TipsGizmo EnableHang(bool value)
        {
            this.hang = value;
            return this;
        }

        public TipsGizmo RedirectInScreenRect(bool value)
        {
            this.screen_redirect = value;
            return this;
        }

        public TipsGizmo SetAnchor(Anchor ah)
        {
            this.anchor = ah;
            return this;
        }

        public TipsGizmo SetIdentity(int id)
        {
            this.identity = id;
            return this;
        }

        public TipsGizmo SetLayer(string layer_name)
        {
            this.layer = LayerMask.NameToLayer(layer_name);
            return this;
        }

        public TipsGizmo SetOffset(Vector3 vec)
        {
            this.offset = vec;
            return this;
        }

        public override void Visualize(object vari, List<GameObject> host)
        {
            <Visualize>c__AnonStorey244 storey = new <Visualize>c__AnonStorey244 {
                host = host,
                <>f__this = this,
                host_bounds = (Bounds) vari
            };
            base.CreateEntity(new Action<NewbieEntity>(storey.<>m__50D));
        }

        [CompilerGenerated]
        private sealed class <Visualize>c__AnonStorey244
        {
            internal TipsGizmo <>f__this;
            internal List<GameObject> host;
            internal Bounds host_bounds;

            internal void <>m__50D(NewbieEntity entity)
            {
                GameObject obj2 = null;
                if (this.<>f__this.hang && (this.host.Count > 0))
                {
                    obj2 = this.host[0];
                }
                entity.ShowTips(this.<>f__this.identity, this.host_bounds, this.<>f__this.anchor, this.<>f__this.offset, this.<>f__this.layer, obj2, this.<>f__this.screen_redirect);
            }
        }

        public enum Anchor
        {
            LEFT,
            RIGHT,
            TOP,
            BOTTOM,
            TOP_LEFT,
            BOTTOM_LEFT,
            TOP_RIGHT,
            BOTTOM_RIGHT
        }
    }
}

