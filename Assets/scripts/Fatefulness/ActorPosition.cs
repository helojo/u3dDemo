namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class ActorPosition : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("Actor", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with actor position getter!");
            }
            Transition port = base.GetPort("Actor");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named Actor");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            AiActor actor = intent2.GetObject<AiActor>(IntentDecl.variable);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, actor.Pos);
        }

        public override Transition Launcher()
        {
            return base.GetPort("Actor");
        }

        public override string Name()
        {
            return "ActorPosition";
        }
    }
}

