namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class MathAdd : MathOperator
    {
        public override string Name()
        {
            return "+";
        }

        protected override object NumericOperate(object left, object right)
        {
            System.Type type = left.GetType();
            if (typeof(int) == type)
            {
                return (Convert.ToInt32(left) + Convert.ToInt32(right));
            }
            if (typeof(long) == type)
            {
                return (Convert.ToInt64(left) + Convert.ToInt64(right));
            }
            if (typeof(ulong) == type)
            {
                return (Convert.ToUInt64(left) + Convert.ToUInt64(right));
            }
            if (typeof(uint) == type)
            {
                return (Convert.ToUInt32(left) + Convert.ToUInt32(right));
            }
            if (typeof(float) == type)
            {
                return (Convert.ToSingle(left) + Convert.ToSingle(right));
            }
            if (typeof(double) != type)
            {
                throw new UnityException("unsupported type for math operator! type = boolean");
            }
            return (Convert.ToDouble(left) + Convert.ToDouble(right));
        }

        protected override object Vector3Operate(Vector3 left, object right)
        {
            if (typeof(Vector3) != right.GetType())
            {
                throw new UnityException("add operate must be used between vector3 ");
            }
            return (left + ((Vector3) right));
        }
    }
}

