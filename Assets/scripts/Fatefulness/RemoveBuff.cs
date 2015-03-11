namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class RemoveBuff : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("Target", Transition.Direction.input);
            base.RegisterPort<Transition>("Entry", Transition.Direction.input);
            base.RegisterPort<Transition>("Count", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with skill remove buff!");
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
                port = base.GetPort("Entry");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named Entry");
                }
                port.Review(intent2);
                int entry = intent2.GetInt32(IntentDecl.variable);
                port = base.GetPort("Count");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named Count");
                }
                port.Review(intent2);
                int number = intent2.GetInt32(IntentDecl.variable);
                actor.RemoveBuff(entry, number);
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "RemoveBuff";
        }
    }
}

