namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Closure : Concentrator
    {
        protected Fragment _prefab;
        protected string prefab_group = string.Empty;
        protected string prefab_name = string.Empty;
        protected Dictionary<string, string> properties = new Dictionary<string, string>();

        public void Attach(string group, string res)
        {
            this._prefab = null;
            this.prefab_group = group;
            this.prefab_name = res;
        }

        public override Concentrator Clone()
        {
            Closure closure = base.Clone() as Closure;
            if (closure == null)
            {
                return null;
            }
            closure.prefab_group = this.prefab_group;
            closure.prefab_name = this.prefab_name;
            foreach (KeyValuePair<string, string> pair in this.properties)
            {
                closure.SetProperty(pair.Key, pair.Value);
            }
            return closure;
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationString(out this.prefab_name);
            io_context.DeserializationString(out this.prefab_group);
            io_context.DeserializationDictionary<string, string>(out this.properties);
        }

        public override void Excude(Intent intent)
        {
            if (this.Prefab != null)
            {
                string str = intent.GetString(IntentDecl.port);
                if (!string.IsNullOrEmpty(str))
                {
                    if (base.GetPort(str) == null)
                    {
                        throw new UnityException("unexisted transition port named " + str);
                    }
                    this.Prefab.ApplyProperties(this.properties);
                    this.Prefab.Dispatch(str, intent, base.root_fragment);
                }
            }
        }

        public string GetProperty(string key)
        {
            string str = string.Empty;
            this.properties.TryGetValue(key, out str);
            return str;
        }

        public override Transition Launcher()
        {
            return null;
        }

        public override string Name()
        {
            if (string.IsNullOrEmpty(this.prefab_name))
            {
                return "Closure";
            }
            return this.prefab_name;
        }

        public override void Relink()
        {
            if (this.Prefab == null)
            {
                base.RemoveAllPorts();
            }
            else
            {
                List<TransmitProxy> concentrators = this.Prefab.GetConcentrators<TransmitProxy>();
                foreach (TransmitProxy proxy in concentrators)
                {
                    string key = proxy.key;
                    Transition.Direction direction = proxy.Direction();
                    Transition port = base.GetPort(key);
                    if (port != null)
                    {
                        port.direction = direction;
                    }
                    else
                    {
                        base.RegisterPort<Transition>(key, direction);
                    }
                }
                List<Transition> list2 = new List<Transition>();
                <Relink>c__AnonStorey15E storeye = new <Relink>c__AnonStorey15E();
                using (Dictionary<string, Transition>.Enumerator enumerator2 = base.export_tnsis.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        storeye.duo = enumerator2.Current;
                        if (concentrators.Find(new Predicate<TransmitProxy>(storeye.<>m__13A)) == null)
                        {
                            list2.Add(storeye.duo.Value);
                        }
                    }
                }
                foreach (Transition transition2 in list2)
                {
                    base.RemovePort(transition2);
                }
                foreach (Property property in this.Prefab.GetConcentrators<Property>())
                {
                    string segmentValue = property.GetSegmentValue<string>("key");
                    if (!this.properties.ContainsKey(segmentValue))
                    {
                        string segment = property.GetSegmentValue<string>("segment");
                        string str4 = property.DefaultValue(segment).ToString();
                        this.properties.Add(segmentValue, str4);
                    }
                }
            }
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationString(this.prefab_name);
            io_context.SerializationString(this.prefab_group);
            io_context.SerializationDictionary<string, string>(this.properties);
        }

        public void SetProperty(string key, string value)
        {
            if (!this.properties.ContainsKey(key))
            {
                this.properties.Add(key, value);
            }
            else
            {
                this.properties[key] = value;
            }
        }

        public Fragment Prefab
        {
            get
            {
                if (this._prefab == null)
                {
                    this._prefab = Fragment.Load(this.prefab_group, this.prefab_name);
                }
                return this._prefab;
            }
        }

        [CompilerGenerated]
        private sealed class <Relink>c__AnonStorey15E
        {
            internal KeyValuePair<string, Transition> duo;

            internal bool <>m__13A(TransmitProxy e)
            {
                return (e.key == this.duo.Key);
            }
        }
    }
}

