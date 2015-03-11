namespace Fatefulness
{
    using System;
    using UnityEngine;

    public abstract class Expression : Concentrator
    {
        protected Expression()
        {
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("left", Transition.Direction.input);
            base.RegisterPort<Transition>("right", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("reviewing intent is not matched with logical expression!");
            }
            Transition port = base.GetPort("left");
            Transition transition2 = base.GetPort("right");
            if ((port == null) || (transition2 == null))
            {
                throw new UnityException("can not find the left or right port");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            object obj2 = intent2.GetObject(IntentDecl.variable);
            transition2.Review(intent2);
            object obj3 = intent2.GetObject(IntentDecl.variable);
            if ((obj2.GetType() != typeof(bool)) || (obj3.GetType() != typeof(bool)))
            {
                throw new UnityException("no matched with boolean type for expression input variable!");
            }
            bool flag = this.Operate((bool) obj2, (bool) obj3);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, flag);
        }

        public override Transition Launcher()
        {
            return base.GetPort("left");
        }

        protected abstract bool Operate(bool left, bool right);
    }
}

