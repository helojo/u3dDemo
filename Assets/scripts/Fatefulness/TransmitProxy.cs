namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class TransmitProxy : Concentrator
    {
        public string key = string.Empty;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Fatefulness.Transition.Direction.both);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationString(out this.key);
        }

        public Fatefulness.Transition.Direction Direction()
        {
            Transition port = base.GetPort("variable");
            if (port != null)
            {
                bool flag = port.export_tnsis.Count > 0;
                bool flag2 = port.import_tnsis.Count > 0;
                if (flag != flag2)
                {
                    return (!flag ? Fatefulness.Transition.Direction.output : Fatefulness.Transition.Direction.input);
                }
            }
            return Fatefulness.Transition.Direction.both;
        }

        public override void Excude(Intent intent)
        {
            Transition port = base.GetPort("variable");
            if (port == null)
            {
                throw new UnityException("unexisted proxy transition port named variable");
            }
            switch (intent.Type)
            {
                case Intent.IntentType.forward:
                    port.Request(intent);
                    break;

                case Intent.IntentType.review:
                    port.Review(intent);
                    break;
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "TransmitProxy";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("key", obj => this.key = obj.ToString(), () => this.key);
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationString(this.key);
        }
    }
}

