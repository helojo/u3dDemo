namespace Fatefulness
{
    using System;

    public class OwnerAdd : Concentrator
    {
        protected int addtional;
        protected int original;

        public override Concentrator Clone()
        {
            Concentrator concentrator = base.Clone();
            if (concentrator == null)
            {
                return null;
            }
            concentrator.SetSegmentValue("variable", (float) this.original);
            return concentrator;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.both);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationInt32(out this.original);
        }

        public override void Excude(Intent intent)
        {
            switch (intent.Type)
            {
                case Intent.IntentType.forward:
                    this.addtional++;
                    break;

                case Intent.IntentType.review:
                    base.ReturnAndStore(intent, "variable", IntentDecl.variable, (float) (this.original + this.addtional));
                    break;
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "++";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("variable", obj => this.original = Convert.ToInt32(obj), () => (float) this.original);
        }

        public override void Reset()
        {
            base.Reset();
            this.addtional = 0;
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationInt32(this.original);
        }
    }
}

