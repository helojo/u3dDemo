namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class SwitchOver<T> : Concentrator where T: struct
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("variable", Transition.Direction.input);
            base.RegisterPort<Transition>("Default", Transition.Direction.output);
            if (!typeof(T).IsEnum)
            {
                throw new UnityException("need some enumerable parameter for SwitchOver template");
            }
            foreach (string str in Enum.GetNames(typeof(T)))
            {
                base.RegisterPort<Transition>(str, Transition.Direction.output);
            }
        }

        public override System.Type EnumeratorType()
        {
            System.Type type = typeof(T);
            if (type.IsEnum)
            {
                return type;
            }
            return null;
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("forwarding intent is not matched with switch over!");
            }
            Transition port = base.GetPort("variable");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named variable");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            Enum enumerator = intent2.GetEnumerator(IntentDecl.variable);
            Transition transition2 = base.GetPort(enumerator.ToString());
            if (transition2 == null)
            {
                throw new UnityException("can not find the option named " + enumerator.ToString() + "from enum type " + typeof(T).Name);
            }
            if (transition2.Accessed())
            {
                transition2.Request(new Intent(Intent.IntentType.forward));
            }
            else
            {
                transition2 = base.GetPort("Default");
                if (transition2 == null)
                {
                    throw new UnityException("can not find the option named Default");
                }
                transition2.Request(new Intent(Intent.IntentType.forward));
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }
    }
}

