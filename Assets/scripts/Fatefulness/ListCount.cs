namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ListCount : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("Collection", Transition.Direction.input);
            base.RegisterPort<Transition>("Entry", Transition.Direction.input);
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with access concentrator!");
            }
            Transition port = base.GetPort("Entry");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named Entry");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            int num = intent2.GetInt32(IntentDecl.variable);
            Transition transition2 = base.GetPort("Collection");
            if (transition2 == null)
            {
                throw new UnityException("unexisted transition port named Collection");
            }
            transition2.Review(intent2);
            List<object> list = intent2.GetObject<List<object>>(IntentDecl.list_datasource);
            if (list == null)
            {
                throw new UnityException("can not find list data source named source");
            }
            int num2 = 0;
            foreach (object obj2 in list)
            {
                if (((int) obj2) == num)
                {
                    num2++;
                }
            }
            if (base.GetPort("variable") == null)
            {
                throw new UnityException("unexisted transition port named variable");
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, num2);
        }

        public override Transition Launcher()
        {
            return base.GetPort("Collection");
        }

        public override string Name()
        {
            return "ListCount";
        }
    }
}

