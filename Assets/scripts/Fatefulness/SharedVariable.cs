namespace Fatefulness
{
    using System;

    public class SharedVariable : Variable
    {
        public override string Name()
        {
            return "Shared Variable";
        }

        protected override string VariableKey
        {
            get
            {
                return base.var_name;
            }
        }
    }
}

