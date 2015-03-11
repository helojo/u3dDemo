namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class RandomConcentrator : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("Min", Transition.Direction.input);
            base.RegisterPort<Transition>("Max", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("reviewing intent is not matched with loop concentrator!");
            }
            Transition port = base.GetPort("Min");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named Min");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            float @float = intent2.GetFloat(IntentDecl.variable);
            port = base.GetPort("Max");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named Max");
            }
            port.Review(intent2);
            float max = intent2.GetFloat(IntentDecl.variable);
            float num3 = UnityEngine.Random.Range(@float, max);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, num3);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "RandomRange";
        }
    }
}

