namespace Fatefulness
{
    using Battle;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BuffStateList : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("parameter1", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            <Excude>c__AnonStorey168 storey = new <Excude>c__AnonStorey168();
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with buff list getter!");
            }
            Transition port = base.GetPort("parameter1");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named parameter1");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            AiActor actor = intent2.GetObject<AiActor>(IntentDecl.variable);
            storey.list = new List<object>();
            if (actor != null)
            {
                actor.GetBuffs().ForEach(new Action<AiBuff>(storey.<>m__156));
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.list_datasource, storey.list);
        }

        public override Transition Launcher()
        {
            return base.GetPort("parameter1");
        }

        public override string Name()
        {
            return "BuffStateList";
        }

        [CompilerGenerated]
        private sealed class <Excude>c__AnonStorey168
        {
            internal List<object> list;

            internal void <>m__156(AiBuff e)
            {
                this.list.Add((int) e.State);
            }
        }
    }
}

