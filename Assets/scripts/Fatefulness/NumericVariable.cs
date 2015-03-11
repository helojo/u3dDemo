namespace Fatefulness
{
    using System;

    public class NumericVariable : Concentrator
    {
        protected float variable;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.both);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationFloat(out this.variable);
        }

        public override void Excude(Intent intent)
        {
            switch (intent.Type)
            {
                case Intent.IntentType.forward:
                    this.variable = intent.GetFloat(IntentDecl.variable);
                    break;

                case Intent.IntentType.review:
                    base.ReturnAndStore(intent, "variable", IntentDecl.variable, this.variable);
                    break;
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "Numeric";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("variable", obj => this.variable = Convert.ToSingle(obj), () => this.variable);
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationFloat(this.variable);
        }
    }
}

