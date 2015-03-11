namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Variable : Concentrator
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$mapA;
        protected int stack_tag = -1;
        protected string var_name = string.Empty;

        public override Concentrator Clone()
        {
            Concentrator concentrator = base.Clone();
            if (concentrator == null)
            {
                return null;
            }
            concentrator.SetSegmentValue("name", this.var_name);
            return concentrator;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
            base.RegisterPort<Transition>("source", Transition.Direction.input);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationString(out this.var_name);
        }

        public override void Excude(Intent intent)
        {
            if (this.stack_tag < 0)
            {
                this.stack_tag = base.root_fragment.dispatch_count;
            }
            string str = intent.GetString(IntentDecl.port);
            if (string.IsNullOrEmpty(str))
            {
                this.GetVariable(intent);
            }
            string key = str;
            if (key != null)
            {
                int num;
                if (<>f__switch$mapA == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
                    dictionary.Add("input", 0);
                    dictionary.Add("variable", 1);
                    <>f__switch$mapA = dictionary;
                }
                if (<>f__switch$mapA.TryGetValue(key, out num))
                {
                    if (num == 0)
                    {
                        this.SetVariable(intent);
                    }
                    else if (num == 1)
                    {
                        this.GetVariable(intent);
                    }
                }
            }
        }

        public void GetVariable(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("reviewing intent is not matched with variable concentrator!");
            }
            object obj2 = base.root_fragment.stack_context.GetObject(this.VariableKey);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, obj2);
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "Variable";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("name", obj => this.var_name = obj.ToString(), () => this.var_name);
        }

        public override void Reset()
        {
            base.Reset();
            this.stack_tag = -1;
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationString(this.var_name);
        }

        public void SetVariable(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("forwding intent is not matched with variable concentrator!");
            }
            Transition port = base.GetPort("source");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named source");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            object obj2 = intent2.GetObject(IntentDecl.variable);
            if (obj2 != null)
            {
                base.root_fragment.stack_context.PutObject(this.VariableKey, obj2);
            }
        }

        protected virtual string VariableKey
        {
            get
            {
                return (this.var_name + "_" + this.stack_tag.ToString());
            }
        }
    }
}

