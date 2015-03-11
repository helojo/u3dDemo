namespace Fatefulness
{
    using System;

    public class EqualComparer : BinaryComparer
    {
        protected override bool CompareClassType(object left, object right)
        {
            return object.ReferenceEquals(left, right);
        }

        protected override bool CompareNumericType(object left, object right)
        {
            float num = Convert.ToSingle(left);
            float num2 = Convert.ToSingle(right);
            return (num == num2);
        }

        public override string Name()
        {
            return "==";
        }
    }
}

