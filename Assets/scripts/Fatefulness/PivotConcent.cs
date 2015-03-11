namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class PivotConcent : Concentrator
    {
        public override Concentrator Clone()
        {
            return null;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.root_fragment.pivot_concent = this;
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            base.root_fragment.pivot_concent = this;
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("the type of intent is not matched witch pivot concent!");
            }
            string key = intent.GetString(IntentDecl.port);
            Transition port = base.GetPort(key);
            if (port == null)
            {
                throw new UnityException("unexisted transition port named " + key);
            }
            port.Request(intent);
        }

        public override Transition Launcher()
        {
            return null;
        }

        public override string Name()
        {
            return "Pivot";
        }
    }
}

