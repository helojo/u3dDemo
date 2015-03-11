namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class ChangeEnergyByNum : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("Target", Transition.Direction.input);
            base.RegisterPort<Transition>("Percent", Transition.Direction.input);
            base.RegisterPort<Transition>("Energy", Transition.Direction.input);
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
                port = base.GetPort("Energy");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named Energy");
                }
                port.Review(intent2);
                int num = intent2.GetInt32(IntentDecl.variable);
                port = base.GetPort("Percent");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named Percent");
                }
                port.Review(intent2);
                int energyChange = (int) (intent2.GetFloat(IntentDecl.variable) * num);
                actor.ChangeEnergyAndNotice(energyChange);
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "ChangeEnergyByNum";
        }
    }
}

