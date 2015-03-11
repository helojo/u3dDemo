namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Ticker : Concentrator
    {
        [CompilerGenerated]
        private static Comparison<Transmation> <>f__am$cache2;
        protected float duration;
        protected List<Transmation> transmations = new List<Transmation>();

        public override Concentrator Clone()
        {
            Concentrator concentrator = base.Clone();
            if (concentrator == null)
            {
                return null;
            }
            concentrator.SetSegmentValue("time", this.duration);
            return concentrator;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("output", Transition.Direction.output);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationFloat(out this.duration);
        }

        protected void DoTick()
        {
            <DoTick>c__AnonStorey169 storey = new <DoTick>c__AnonStorey169 {
                time = this.CurrentTime
            };
            int count = this.transmations.Count;
            Transition port = base.GetPort("output");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named output");
            }
            List<Transmation> list = this.transmations.FindAll(new Predicate<Transmation>(storey.<>m__162));
            foreach (Transmation transmation in list)
            {
                this.transmations.Remove(transmation);
            }
            foreach (Transmation transmation2 in list)
            {
                port.Request(transmation2.intent);
            }
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with timer concentrator!");
            }
            Transmation item = new Transmation {
                time = this.CurrentTime + this.duration,
                intent = intent
            };
            this.transmations.Add(item);
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = (t1, t2) => (t1.time > t2.time) ? ((Comparison<Transmation>) 1) : ((Comparison<Transmation>) (-1));
            }
            this.transmations.Sort(<>f__am$cache2);
            this.Update();
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "WaitFor_RealTime";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("time", obj => this.duration = Convert.ToSingle(obj), () => this.duration);
        }

        public override void Reset()
        {
            base.Reset();
            this.transmations.Clear();
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationFloat(this.duration);
        }

        public override void Tick()
        {
            base.Tick();
            this.DoTick();
        }

        protected virtual float CurrentTime
        {
            get
            {
                return Time.time;
            }
        }

        [CompilerGenerated]
        private sealed class <DoTick>c__AnonStorey169
        {
            internal float time;

            internal bool <>m__162(Ticker.Transmation e)
            {
                return (e.time <= this.time);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct Transmation
        {
            public float time;
            public Intent intent;
        }
    }
}

