namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class FocusMask : Visualization
    {
        protected int extract_flag = 3;
        protected List<Visualization> gizmos = new List<Visualization>();

        public FingureGizmo AttachFingure()
        {
            FingureGizmo item = new FingureGizmo();
            this.gizmos.Add(item);
            return item;
        }

        public GUILayerLock AttachGUILock()
        {
            GUILayerLock item = new GUILayerLock();
            this.gizmos.Add(item);
            return item;
        }

        public void AttachImage2DTips(List<GameObject> objects, bool prominence)
        {
            <AttachImage2DTips>c__AnonStorey247 storey = new <AttachImage2DTips>c__AnonStorey247 {
                prominence = prominence,
                objects = objects
            };
            base.CreateEntity(new Action<NewbieEntity>(storey.<>m__511));
        }

        public TipsGizmo AttachTips(int identity, TipsGizmo.Anchor anchor)
        {
            TipsGizmo item = new TipsGizmo {
                identity = identity,
                anchor = anchor
            };
            this.gizmos.Add(item);
            return item;
        }

        protected virtual Bounds GenMask(NewbieEntity entity, List<GameObject> host_objects)
        {
            return entity.GenerateMaskBounds(this.extract_flag, host_objects);
        }

        public bool GetExtractFlag(ExtractFlag flag)
        {
            return (0 != (this.extract_flag & flag));
        }

        public void SetExtractFlag(ExtractFlag flag, bool value)
        {
            int num = (int) flag;
            if (value)
            {
                this.extract_flag |= num;
            }
            else
            {
                this.extract_flag &= ~num;
            }
        }

        public override void Visualize(object vari, List<GameObject> host)
        {
            <Visualize>c__AnonStorey246 storey = new <Visualize>c__AnonStorey246 {
                host = host,
                <>f__this = this
            };
            base.CreateEntity(new Action<NewbieEntity>(storey.<>m__510));
        }

        [CompilerGenerated]
        private sealed class <AttachImage2DTips>c__AnonStorey247
        {
            internal List<GameObject> objects;
            internal bool prominence;

            internal void <>m__511(NewbieEntity entity)
            {
                entity.Generate2DTips(!this.prominence ? 0 : 2, this.objects);
            }
        }

        [CompilerGenerated]
        private sealed class <Visualize>c__AnonStorey246
        {
            internal FocusMask <>f__this;
            internal List<GameObject> host;

            internal void <>m__510(NewbieEntity entity)
            {
                Bounds vari = this.<>f__this.GenMask(entity, this.host);
                int count = this.<>f__this.gizmos.Count;
                for (int i = 0; i != count; i++)
                {
                    Visualization visualization = this.<>f__this.gizmos[i];
                    if (visualization != null)
                    {
                        visualization.Visualize(vari, this.host);
                    }
                }
            }
        }

        public enum ExtractFlag
        {
            Complex = 0x10,
            Darkness = 4,
            Enforce = 1,
            PerVertex = 8,
            Prominence = 2
        }
    }
}

