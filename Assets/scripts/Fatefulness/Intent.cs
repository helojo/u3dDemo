namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Intent
    {
        private Dictionary<string, object> parameters;

        public Intent()
        {
            this.parameters = new Dictionary<string, object>();
        }

        public Intent(IntentType type)
        {
            this.parameters = new Dictionary<string, object>();
            this.PutEnumerator("intent_type", type);
        }

        public void Clear()
        {
            this.parameters.Clear();
        }

        public void Deserialization(IOContext context)
        {
            int ovalue = 0;
            context.DeserializationInt32(out ovalue);
            this.parameters.Clear();
            for (int i = 0; i != ovalue; i++)
            {
                string str = string.Empty;
                object obj2 = null;
                context.DeserializationString(out str);
                context.Deserialization<object>(out obj2);
                this.parameters.Add(str, obj2);
            }
        }

        public void Foreach(Iterator iter)
        {
            foreach (KeyValuePair<string, object> pair in this.parameters)
            {
                iter(pair.Key, pair.Value);
            }
        }

        public bool GetBoolean(string key)
        {
            object obj2 = this.GetObject(key);
            if (obj2 == null)
            {
                return false;
            }
            return Convert.ToBoolean(obj2);
        }

        public Enum GetEnumerator(string key)
        {
            return (this.GetObject(key) as Enum);
        }

        public float GetFloat(string key)
        {
            object obj2 = this.GetObject(key);
            if (obj2 == null)
            {
                return 0f;
            }
            return Convert.ToSingle(obj2);
        }

        public int GetInt32(string key)
        {
            object obj2 = this.GetObject(key);
            if (obj2 == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj2);
        }

        public object GetObject(string key)
        {
            object obj2 = null;
            if (!this.parameters.TryGetValue(key, out obj2))
            {
                return null;
            }
            return obj2;
        }

        public T GetObject<T>(string key)
        {
            return (T) this.GetObject(key);
        }

        public string GetString(string key)
        {
            object obj2 = this.GetObject(key);
            if (obj2 == null)
            {
                return string.Empty;
            }
            return Convert.ToString(obj2);
        }

        public Vector3 GetVector3(string key)
        {
            object obj2 = this.GetObject(key);
            if (obj2 == null)
            {
                return Vector3.zero;
            }
            return (Vector3) obj2;
        }

        public void PutBoolean(string key, bool value)
        {
            this.PutObject(key, value);
        }

        public void PutEnumerator(string key, Enum e)
        {
            this.PutObject(key, e);
        }

        public void PutFloat(string key, float value)
        {
            this.PutObject(key, value);
        }

        public void PutInt32(string key, int value)
        {
            this.PutObject(key, value);
        }

        public void PutObject(string key, object value)
        {
            if (!this.parameters.ContainsKey(key))
            {
                this.parameters.Add(key, null);
            }
            this.parameters[key] = value;
        }

        public void PutString(string key, string value)
        {
            this.PutObject(key, value);
        }

        public void PutVector3(string key, Vector3 v)
        {
            this.PutObject(key, v);
        }

        public void Serialization(IOContext context)
        {
            context.SerializationInt32(this.parameters.Count);
            foreach (KeyValuePair<string, object> pair in this.parameters)
            {
                string key = pair.Key;
                object obj2 = pair.Value;
                context.SerializationString(key);
                context.Serialization<object>(obj2);
            }
        }

        public IntentType Type
        {
            get
            {
                return (IntentType) this.GetEnumerator("intent_type");
            }
        }

        public enum IntentType
        {
            forward,
            review
        }

        public delegate void Iterator(string key, object value);
    }
}

