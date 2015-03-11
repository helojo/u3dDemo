namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class ReduceHPByNum : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("Target", Transition.Direction.input);
            base.RegisterPort<Transition>("Percent", Transition.Direction.input);
            base.RegisterPort<Transition>("Demage", Transition.Direction.input);
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
                port = base.GetPort("Demage");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named Demage");
                }
                port.Review(intent2);
                int num = intent2.GetInt32(IntentDecl.variable);
                port = base.GetPort("Percent");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named Percent");
                }
                port.Review(intent2);
                int num3 = (int) (intent2.GetFloat(IntentDecl.variable) * num);
                actor.ChangeHpByUseSkill(-num3);
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "ReduceHPByNum";
        }
    }
}

