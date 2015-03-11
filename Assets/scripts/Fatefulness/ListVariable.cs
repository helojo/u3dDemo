namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ListVariable : Concentrator
    {
        protected List<object> list = new List<object>();

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with list concentrator!");
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.list_datasource, this.list);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "List";
        }

        public override void Reset()
        {
            this.list.Clear();
        }
    }
}

