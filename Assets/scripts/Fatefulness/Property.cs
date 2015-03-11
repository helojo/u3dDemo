namespace Fatefulness
{
    using System;

    public class Property : Concentrator
    {
        protected Concentrator host;
        protected string key = string.Empty;
        protected string segment = string.Empty;

        public object DefaultValue(string segment)
        {
            if (this.host == null)
            {
                return null;
            }
            return this.host.GetSegmentValue(segment);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            int ovalue = -1;
            io_context.DeserializationInt32(out ovalue);
            io_context.DeserializationString(out this.key);
            io_context.DeserializationString(out this.segment);
            this.host = base.root_fragment.FindConcentratorByID(ovalue);
        }

        public override void Excude(Intent intent)
        {
            if (this.host != null)
            {
                object obj2 = intent.GetObject(this.key);
                if (obj2 != null)
                {
                    this.host.SetSegmentValue(this.segment, obj2);
                }
            }
        }

        public override Transition Launcher()
        {
            return null;
        }

        public override string Name()
        {
            return "Property";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("key", obj => this.key = obj.ToString(), () => this.key);
            base.RegisterSerializableSegment("segment", obj => this.segment = obj.ToString(), () => this.segment);
            base.RegisterSerializableSegment("host", obj => this.host = obj as Concentrator, () => this.host);
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationInt32((this.host != null) ? this.host.id : -1);
            io_context.SerializationString(this.key);
            io_context.SerializationString(this.segment);
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public string Value
        {
            get
            {
                if (this.host == null)
                {
                    return string.Empty;
                }
                return this.host.GetSegmentValue(this.Key).ToString();
            }
            set
            {
                if (this.host != null)
                {
                    this.host.SetSegmentValue(this.Key, value);
                }
            }
        }
    }
}

