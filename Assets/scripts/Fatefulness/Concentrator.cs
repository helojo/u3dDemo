namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class Concentrator
    {
        protected Dictionary<string, Transition> export_tnsis = new Dictionary<string, Transition>();
        public Fragment root_fragment;
        protected List<Segment> serializable_segments = new List<Segment>();
        public Vector2 tag_position = Vector2.zero;

        protected Concentrator()
        {
        }

        public virtual Concentrator Clone()
        {
            Concentrator concentrator = Factory.CreateConcentrator(base.GetType().Name);
            if (concentrator == null)
            {
                return null;
            }
            concentrator.root_fragment = this.root_fragment;
            concentrator.tag_position = this.tag_position;
            concentrator.Concreate();
            concentrator.RegisterSerializableSegment();
            return concentrator;
        }

        public virtual void Concreate()
        {
            this.export_tnsis.Clear();
            this.root_fragment.concent_pool.Add(this);
        }

        public void Derive(Concentrator concent)
        {
            foreach (KeyValuePair<string, Transition> pair in this.export_tnsis)
            {
                <Derive>c__AnonStorey15A storeya = new <Derive>c__AnonStorey15A();
                string key = pair.Key;
                Transition src = pair.Value;
                if (src != null)
                {
                    storeya.dst_tnsi = concent.GetPort(key);
                    if (storeya.dst_tnsi != null)
                    {
                        foreach (Transition transition2 in src.import_tnsis)
                        {
                            transition2.Replace(src, storeya.dst_tnsi);
                        }
                        src.export_tnsis.ForEach(new Action<Transition>(storeya.<>m__136));
                    }
                }
            }
            concent.root_fragment = this.root_fragment;
            concent.tag_position = this.tag_position;
        }

        public virtual void Deserialization(IOContext io_context)
        {
            this.export_tnsis.Clear();
            int ovalue = 0;
            io_context.DeserializationInt32(out ovalue);
            for (int i = 0; i != ovalue; i++)
            {
                string str = null;
                string str2 = null;
                io_context.DeserializationString(out str2);
                io_context.DeserializationString(out str);
                Transition transition = Factory.CreateTransition(str);
                if (transition == null)
                {
                    throw new UnityException("deserialization transition failed by type = " + str);
                }
                transition.key = str2;
                transition.root_fragment = this.root_fragment;
                transition.lanch_concent = this;
                transition.Deserialization(io_context);
                this.export_tnsis.Add(str2, transition);
                this.root_fragment.transition_pool.Add(transition);
            }
            io_context.DeserializationVector2(out this.tag_position);
        }

        public virtual System.Type EnumeratorType()
        {
            return null;
        }

        public abstract void Excude(Intent intent);
        public Transition GetPort(int id)
        {
            <GetPort>c__AnonStorey15D storeyd = new <GetPort>c__AnonStorey15D {
                id = id
            };
            return this.root_fragment.transition_pool.Find(new Predicate<Transition>(storeyd.<>m__139));
        }

        public Transition GetPort(string key)
        {
            Transition transition = null;
            if (!this.export_tnsis.TryGetValue(key, out transition))
            {
                return null;
            }
            return transition;
        }

        public string GetSegment(int index)
        {
            if (index >= this.serializable_segments.Count)
            {
                return string.Empty;
            }
            return this.serializable_segments[index].key;
        }

        public object GetSegmentValue(string key)
        {
            <GetSegmentValue>c__AnonStorey15B storeyb = new <GetSegmentValue>c__AnonStorey15B {
                key = key
            };
            Segment segment = this.serializable_segments.Find(new Predicate<Segment>(storeyb.<>m__137));
            if (segment == null)
            {
                return null;
            }
            if (segment.getter == null)
            {
                return null;
            }
            return segment.getter();
        }

        public T GetSegmentValue<T>(string key)
        {
            object segmentValue = this.GetSegmentValue(key);
            if (segmentValue == null)
            {
                return default(T);
            }
            return (T) segmentValue;
        }

        public string KeyOfTransition(Transition tnsi)
        {
            int id = tnsi.id;
            foreach (KeyValuePair<string, Transition> pair in this.export_tnsis)
            {
                if (pair.Value.id == id)
                {
                    return pair.Key;
                }
            }
            return string.Empty;
        }

        public abstract Transition Launcher();
        public virtual string Name()
        {
            return "Unknown";
        }

        protected Transition RegisterPort<T>(string key, Transition.Direction direction) where T: Transition
        {
            Transition transition = Factory.CreateTransition(typeof(T).Name);
            if (transition == null)
            {
                throw new UnityException("failed to register transition type = " + typeof(T).Name);
            }
            transition.key = key;
            transition.lanch_concent = this;
            transition.direction = direction;
            this.export_tnsis.Add(key, transition);
            this.root_fragment.RegisterTransition(transition);
            return transition;
        }

        public virtual void RegisterSerializableSegment()
        {
        }

        protected void RegisterSerializableSegment(string key, Action<object> setter, Func<object> getter)
        {
            Segment item = new Segment {
                key = key,
                getter = getter,
                setter = setter
            };
            this.serializable_segments.Add(item);
        }

        public virtual void Relink()
        {
        }

        public void RemoveAllPorts()
        {
            foreach (Transition transition in this.tnsi_list)
            {
                this.RemovePortBothway(transition);
            }
            this.export_tnsis.Clear();
        }

        public void RemovePort(Transition tnsi)
        {
            this.RemovePortBothway(tnsi);
            this.export_tnsis.Remove(tnsi.key);
        }

        private void RemovePortBothway(Transition tnsi)
        {
            foreach (Transition transition in tnsi.import_tnsis)
            {
                transition.export_tnsis.Remove(tnsi);
            }
            tnsi.export_tnsis.Clear();
            this.root_fragment.transition_pool.Remove(tnsi);
        }

        public void RemvoeSelf()
        {
            if (this.root_fragment != null)
            {
                this.root_fragment.RemoveConcentrator(this);
            }
        }

        public virtual bool Repair()
        {
            return true;
        }

        protected bool RepairPort<T>(string key, Transition.Direction direction) where T: Transition
        {
            Transition transition = null;
            if (!this.export_tnsis.TryGetValue(key, out transition))
            {
                this.RegisterPort<T>(key, direction);
                return false;
            }
            if (transition.direction == direction)
            {
                return true;
            }
            transition.direction = direction;
            return false;
        }

        public virtual void Reset()
        {
        }

        protected void ReturnAndStore(Intent context, string port, string key, object value)
        {
            context.PutObject(key, value);
        }

        public int SegmentCount()
        {
            return this.serializable_segments.Count;
        }

        public virtual void Serialization(IOContext io_context)
        {
            string name = base.GetType().Name;
            io_context.SerializationString(name);
            int count = this.export_tnsis.Count;
            io_context.SerializationInt32(count);
            foreach (KeyValuePair<string, Transition> pair in this.export_tnsis)
            {
                string key = pair.Key;
                Transition transition = pair.Value;
                io_context.SerializationString(key);
                transition.Serialization(io_context);
            }
            io_context.SerializationVector2(this.tag_position);
        }

        public void SetSegmentValue(string key, object value)
        {
            <SetSegmentValue>c__AnonStorey15C storeyc = new <SetSegmentValue>c__AnonStorey15C {
                key = key
            };
            Segment segment = this.serializable_segments.Find(new Predicate<Segment>(storeyc.<>m__138));
            if ((segment != null) && (segment.setter != null))
            {
                segment.setter(value);
            }
        }

        public virtual void Tick()
        {
        }

        public virtual void Update()
        {
        }

        public int id
        {
            get
            {
                return this.root_fragment.concent_pool.IndexOf(this);
            }
        }

        public Dictionary<string, Transition>.KeyCollection tnsi_key_list
        {
            get
            {
                return this.export_tnsis.Keys;
            }
        }

        public Dictionary<string, Transition>.ValueCollection tnsi_list
        {
            get
            {
                return this.export_tnsis.Values;
            }
        }

        [CompilerGenerated]
        private sealed class <Derive>c__AnonStorey15A
        {
            internal Transition dst_tnsi;

            internal void <>m__136(Transition e)
            {
                this.dst_tnsi.export_tnsis.Add(e);
            }
        }

        [CompilerGenerated]
        private sealed class <GetPort>c__AnonStorey15D
        {
            internal int id;

            internal bool <>m__139(Transition e)
            {
                return (e.id == this.id);
            }
        }

        [CompilerGenerated]
        private sealed class <GetSegmentValue>c__AnonStorey15B
        {
            internal string key;

            internal bool <>m__137(Concentrator.Segment e)
            {
                return (e.key == this.key);
            }
        }

        [CompilerGenerated]
        private sealed class <SetSegmentValue>c__AnonStorey15C
        {
            internal string key;

            internal bool <>m__138(Concentrator.Segment e)
            {
                return (e.key == this.key);
            }
        }

        protected class Segment
        {
            public Func<object> getter;
            public string key = string.Empty;
            public Action<object> setter;
        }
    }
}

