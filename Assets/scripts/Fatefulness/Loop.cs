namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Loop : Concentrator
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map9;
        protected float delta_time = float.MaxValue;
        protected float duration;

        public override Concentrator Clone()
        {
            Concentrator concentrator = base.Clone();
            if (concentrator == null)
            {
                return null;
            }
            concentrator.SetSegmentValue("time", this.duration);
            return concentrator;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("output", Transition.Direction.output);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationFloat(out this.duration);
        }

        public override void Excude(Intent intent)
        {
            string str = intent.GetString(IntentDecl.port);
            if (string.IsNullOrEmpty(str))
            {
                this.Prepare(intent);
            }
            string key = str;
            if (key != null)
            {
                int num;
                if (<>f__switch$map9 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
                    dictionary.Add("input", 0);
                    dictionary.Add("output", 1);
                    <>f__switch$map9 = dictionary;
                }
                if (<>f__switch$map9.TryGetValue(key, out num))
                {
                    if (num == 0)
                    {
                        this.Prepare(intent);
                    }
                    else if (num == 1)
                    {
                        this.ReviewDeltaTime(intent);
                    }
                }
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "Loop";
        }

        private void Prepare(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("forwding intent is not matched with loop concentrator!");
            }
            this.delta_time = 0f;
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("time", obj => this.duration = Convert.ToSingle(obj), () => this.duration);
        }

        public override void Reset()
        {
            base.Reset();
            this.delta_time = float.MaxValue;
        }

        private void ReviewDeltaTime(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("reviewing intent is not matched with loop concentrator!");
            }
            base.ReturnAndStore(intent, "output", IntentDecl.variable, this.delta_time);
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationFloat(this.duration);
        }

        public override void Tick()
        {
            this.delta_time += Time.deltaTime;
            if (this.delta_time <= this.duration)
            {
                Transition port = base.GetPort("output");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named output");
                }
                port.Request(new Intent(Intent.IntentType.forward));
            }
        }
    }
}

