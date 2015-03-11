namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class ResetConcentrator : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("output", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with conditional concentrator!");
            }
            Transition port = base.GetPort("output");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named output");
            }
            port.RequestReset(new Intent(Intent.IntentType.forward));
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "Reset";
        }
    }
}

