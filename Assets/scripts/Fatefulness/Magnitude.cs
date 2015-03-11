namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class Magnitude : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("From", Transition.Direction.input);
            base.RegisterPort<Transition>("To", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with vector3 magnitude concentrator!");
            }
            Transition port = base.GetPort("From");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named From");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            Vector3 vector = intent2.GetVector3(IntentDecl.variable);
            Transition transition2 = base.GetPort("To");
            if (transition2 == null)
            {
                throw new UnityException("unexisted transition port named To");
            }
            transition2.Review(intent2);
            Vector3 vector2 = intent2.GetVector3(IntentDecl.variable);
            Vector3 vector3 = vector - vector2;
            float magnitude = vector3.magnitude;
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, magnitude);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "Magnitude";
        }
    }
}

