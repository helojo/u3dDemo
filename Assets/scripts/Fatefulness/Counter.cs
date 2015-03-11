namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class Counter : Concentrator
    {
        protected int count;
        protected int total;

        public override Concentrator Clone()
        {
            Concentrator concentrator = base.Clone();
            if (concentrator == null)
            {
                return null;
            }
            concentrator.SetSegmentValue("count", this.total);
            return concentrator;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("Again", Transition.Direction.output);
            base.RegisterPort<Transition>("Finish", Transition.Direction.output);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationInt32(out this.total);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with counter concentrator!");
            }
            string key = (this.count++ < this.total) ? "Again" : "Finish";
            Transition port = base.GetPort(key);
            if (port == null)
            {
                throw new UnityException("unexisted transition port named " + key);
            }
            Debug.Log("counter");
            port.Request(new Intent(Intent.IntentType.forward));
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "Counter";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("count", obj => this.total = Convert.ToInt32(obj), () => this.total);
        }

        public override void Reset()
        {
            base.Reset();
            this.count = 0;
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationInt32(this.total);
        }
    }
}

