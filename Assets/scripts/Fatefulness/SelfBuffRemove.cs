namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class SelfBuffRemove : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.root_fragment.self_buff_remove = this;
            base.RegisterPort<Transition>("input", Transition.Direction.output);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            base.root_fragment.self_buff_remove = this;
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("the type of intent is not matched witch pivot concent!");
            }
            string key = "input";
            Transition port = base.GetPort(key);
            if (port != null)
            {
                port.Request(intent);
            }
        }

        public override Transition Launcher()
        {
            return null;
        }

        public override string Name()
        {
            return "SelfBuffRemove";
        }
    }
}

