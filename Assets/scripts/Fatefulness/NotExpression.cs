namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class NotExpression : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("left", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("reviewing intent is not matched with logical expression!");
            }
            Transition port = base.GetPort("left");
            if (port == null)
            {
                throw new UnityException("can not find the left port");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            object obj2 = intent2.GetObject(IntentDecl.variable);
            if (obj2.GetType() != typeof(bool))
            {
                throw new UnityException("no matched with boolean type for expression input variable!");
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, !((bool) obj2));
        }

        public override Transition Launcher()
        {
            return base.GetPort("left");
        }

        public override string Name()
        {
            return "!";
        }
    }
}

