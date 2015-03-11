namespace Fatefulness
{
    using System;
    using UnityEngine;

    public abstract class MathOperator : Concentrator
    {
        protected MathOperator()
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
                throw new UnityException("reviewing intent is not matched with math operator!");
            }
            Transition port = base.GetPort("left");
            Transition transition2 = base.GetPort("right");
            if ((port == null) || (transition2 == null))
            {
                throw new UnityException("can not find the left or right port");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            object left = intent2.GetObject(IntentDecl.variable);
            transition2.Review(intent2);
            object right = intent2.GetObject(IntentDecl.variable);
            object obj4 = this.Operate(left, right);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, obj4);
        }

        public override Transition Launcher()
        {
            return base.GetPort("left");
        }

        protected abstract object NumericOperate(object left, object right);
        protected object Operate(object left, object right)
        {
            System.Type type = left.GetType();
            if ((((typeof(int) == type) || (typeof(long) == type)) || ((typeof(ulong) == type) || (typeof(uint) == type))) || (((typeof(float) == type) || (typeof(double) == type)) || (typeof(bool) == type)))
            {
                return this.NumericOperate(left, right);
            }
            if (typeof(Vector3) != type)
            {
                throw new UnityException("unsupported type for math operator! type = " + type.ToString());
            }
            return this.Vector3Operate((Vector3) left, right);
        }

        protected abstract object Vector3Operate(Vector3 left, object right);
    }
}

