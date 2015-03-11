namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AddToList : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("Object", Transition.Direction.input);
            base.RegisterPort<Transition>("Collection", Transition.Direction.input);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with add to list concentrator!");
            }
            Transition port = base.GetPort("Object");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named Object");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            object item = intent2.GetObject(IntentDecl.variable);
            if (item != null)
            {
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
                list.Add(item);
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "Add To List";
        }
    }
}

