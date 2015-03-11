namespace Fatefulness
{
    using System;

    public class Terminator : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            base.root_fragment.Terminate();
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "Finish";
        }
    }
}

