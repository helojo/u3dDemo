namespace Fatefulness
{
    using System;

    public class OrExpression : Expression
    {
        public override string Name()
        {
            return "||";
        }

        protected override bool Operate(bool left, bool right)
        {
            return (left || right);
        }
    }
}

