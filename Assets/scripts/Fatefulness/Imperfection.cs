namespace Fatefulness
{
    using System;

    public class Imperfection : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("variable", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }
    }
}

