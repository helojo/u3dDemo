namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class MathAbs : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("Numeric", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("reviewing intent is not matched with math abs concentrator!");
            }
            Transition port = base.GetPort("Numeric");
            if (port == null)
            {
                throw new UnityException("can not find the Numeric port");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            float num = Convert.ToSingle(intent2.GetObject(IntentDecl.variable));
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, num);
        }

        public override Transition Launcher()
        {
            return base.GetPort("Numeric");
        }

        public override string Name()
        {
            return "Abs";
        }
    }
}

