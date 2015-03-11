namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class ReduceHP : Concentrator
    {
        protected float persent = 1f;

        public override Concentrator Clone()
        {
            Concentrator concentrator = base.Clone();
            if (concentrator == null)
            {
                return null;
            }
            concentrator.SetSegmentValue("persent", this.persent);
            return concentrator;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("Target", Transition.Direction.input);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationFloat(out this.persent);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with reduce hp!");
            }
            Transition port = base.GetPort("Target");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named Target");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            AiActor actor = intent2.GetObject<AiActor>(IntentDecl.variable);
            if (actor != null)
            {
                actor.ChangeHpByUseSkill(this.persent);
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "ReduceHP";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("persent", obj => this.persent = Convert.ToSingle(obj), () => this.persent);
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationFloat(this.persent);
        }
    }
}

