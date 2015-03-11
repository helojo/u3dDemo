namespace Fatefulness
{
    using System;
    using UnityEngine;

    public abstract class BinaryComparer : Concentrator
    {
        protected BinaryComparer()
        {
        }

        protected bool Compare(object left, object right)
        {
            System.Type type = left.GetType();
            System.Type type2 = right.GetType();
            if (type.IsClass && (type == type2))
            {
                return this.CompareClassType(left, right);
            }
            if ((((typeof(int) != type) && (typeof(long) != type)) && ((typeof(ulong) != type) && (typeof(uint) != type))) && (((typeof(float) != type) && (typeof(double) != type)) && (typeof(bool) != type)))
            {
                return left.Equals(right);
            }
            return this.CompareNumericType(left, right);
        }

        protected abstract bool CompareClassType(object left, object right);
        protected abstract bool CompareNumericType(object left, object right);
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
                throw new UnityException("reviewing intent is not matched with binary compare!");
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
            bool flag = this.Compare(left, right);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, flag);
        }

        public override Transition Launcher()
        {
            return base.GetPort("left");
        }
    }
}

