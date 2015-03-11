namespace Fatefulness
{
    using System;

    public class AndExpression : Expression
    {
        public override string Name()
        {
            return "&&";
        }

        protected override bool Operate(bool left, bool right)
        {
            return (left && right);
        }
    }
}

