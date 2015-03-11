namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ListCountConstant : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("parameter1", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            Intent intent2 = new Intent(Intent.IntentType.review);
            Transition port = base.GetPort("parameter1");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named parameter1");
            }
            port.Review(intent2);
            List<object> list = intent2.GetObject<List<object>>(IntentDecl.list_datasource);
            if (list == null)
            {
                throw new UnityException("can not find list data source named source");
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, list.Count);
        }

        public override Transition Launcher()
        {
            return base.GetPort("parameter1");
        }

        public override string Name()
        {
            return "ListCountConstant";
        }
    }
}

