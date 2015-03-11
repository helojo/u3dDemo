namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DynamicCounter : Concentrator
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map8;
        protected int count;
        protected List<object> list;
        protected int total;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("source", Transition.Direction.input);
            base.RegisterPort<Transition>("Again", Transition.Direction.output);
            base.RegisterPort<Transition>("Finish", Transition.Direction.output);
            base.RegisterPort<Transition>("Index", Transition.Direction.output);
        }

        private void CounterInvoke(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with dynamical counter concentrator!");
            }
            if (this.list == null)
            {
                Transition transition = base.GetPort("source");
                if (transition == null)
                {
                    throw new UnityException("unexisted transition port named source");
                }
                Intent intent2 = new Intent(Intent.IntentType.review);
                transition.Review(intent2);
                this.list = intent2.GetObject<List<object>>(IntentDecl.list_datasource);
                if (this.list == null)
                {
                    throw new UnityException("unexisted data source for the reviewing of dynamical counter!");
                }
            }
            this.total = this.list.Count;
            string key = (this.count++ < this.total) ? "Again" : "Finish";
            Transition port = base.GetPort(key);
            if (port == null)
            {
                throw new UnityException("unexisted transition port named " + key);
            }
            port.Request(new Intent(Intent.IntentType.forward));
        }

        public override void Excude(Intent intent)
        {
            string key = intent.GetString(IntentDecl.port);
            if (key != null)
            {
                int num;
                if (<>f__switch$map8 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
                    dictionary.Add("Index", 0);
                    <>f__switch$map8 = dictionary;
                }
                if (<>f__switch$map8.TryGetValue(key, out num) && (num == 0))
                {
                    this.IndexReview(intent);
                    return;
                }
            }
            this.CounterInvoke(intent);
        }

        private void IndexReview(Intent intent)
        {
            base.ReturnAndStore(intent, "Index", IntentDecl.variable, Mathf.Max(0, Mathf.Min((int) (this.count - 1), (int) (this.total - 1))));
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "DynamicCounter";
        }

        public override void Reset()
        {
            base.Reset();
            this.count = 0;
            this.list = null;
        }
    }
}

