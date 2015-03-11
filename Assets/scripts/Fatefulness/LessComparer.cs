namespace Fatefulness
{
    using System;

    public class LessComparer : BinaryComparer
    {
        protected override bool CompareClassType(object left, object right)
        {
            return false;
        }

        protected override bool CompareNumericType(object left, object right)
        {
            float num = Convert.ToSingle(left);
            float num2 = Convert.ToSingle(right);
            return (num < num2);
        }

        public override string Name()
        {
            return "<";
        }
    }
}

