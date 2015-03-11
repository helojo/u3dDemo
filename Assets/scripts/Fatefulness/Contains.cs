namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Contains : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("Object", Transition.Direction.input);
            base.RegisterPort<Transition>("Collection", Transition.Direction.input);
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with contains concentrator!");
            }
            Transition port = base.GetPort("Object");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named Object");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            object obj2 = intent2.GetObject(IntentDecl.variable);
            if (obj2 == null)
            {
                base.ReturnAndStore(intent, "variable", IntentDecl.variable, false);
            }
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
            if (list.Count > 0)
            {
                System.Type conversionType = list[0].GetType();
                if (conversionType != obj2.GetType())
                {
                    obj2 = Convert.ChangeType(obj2, conversionType);
                }
            }
            if (base.GetPort("variable") == null)
            {
                throw new UnityException("unexisted transition port named variable");
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, list.Contains(obj2));
        }

        public override Transition Launcher()
        {
            return base.GetPort("Object");
        }

        public override string Name()
        {
            return "Contains";
        }
    }
}

