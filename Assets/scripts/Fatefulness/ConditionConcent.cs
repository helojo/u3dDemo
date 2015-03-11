namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class ConditionConcent : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("condition", Transition.Direction.input);
            base.RegisterPort<Transition>("True", Transition.Direction.output);
            base.RegisterPort<Transition>("False", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with conditional concentrator!");
            }
            Transition port = base.GetPort("condition");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named condition");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            string key = !intent2.GetBoolean(IntentDecl.variable) ? "False" : "True";
            Transition transition2 = base.GetPort(key);
            if (transition2 == null)
            {
                throw new UnityException("unexisted transition port named " + key);
            }
            transition2.Request(new Intent(Intent.IntentType.forward));
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "IF";
        }
    }
}

