namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;

    public class IOContext
    {
        protected BinaryFormatter formatter = new BinaryFormatter();
        protected MemoryStream stream = new MemoryStream();

        public byte[] Buffer()
        {
            return this.stream.ToArray();
        }

        public virtual void Deserialization<T>(out T obj)
        {
            object obj2 = this.formatter.Deserialize(this.stream);
            obj = (T) obj2;
        }

        public virtual void DeserializationBoolean(out bool ovalue)
        {
            this.Deserialization<bool>(out ovalue);
        }

        public virtual void DeserializationDictionary<T, K>(out Dictionary<T, K> dic)
        {
            dic = new Dictionary<T, K>();
            int ovalue = 0;
            this.DeserializationInt32(out ovalue);
            for (int i = 0; i != ovalue; i++)
            {
                T local = default(T);
                K local2 = default(K);
                this.Deserialization<T>(out local);
                this.Deserialization<K>(out local2);
                dic.Add(local, local2);
            }
        }

        public virtual void DeserializationFloat(out float ovalue)
        {
            this.Deserialization<float>(out ovalue);
        }

        public virtual void DeserializationInt32(out int ovalue)
        {
            this.Deserialization<int>(out ovalue);
        }

        public virtual void DeserializationString(out string ovalue)
        {
            this.Deserialization<string>(out ovalue);
        }

        public virtual void DeserializationVector2(out Vector2 v)
        {
            float ovalue = 0f;
            float num2 = 0f;
            this.DeserializationFloat(out ovalue);
            this.DeserializationFloat(out num2);
            v.x = ovalue;
            v.y = num2;
        }

        public bool EOF()
        {
            return (this.stream.Position >= this.stream.Length);
        }

        public void Reset()
        {
            this.SeekToBegin();
            this.stream.SetLength(0L);
        }

        public void SeekToBegin()
        {
            this.stream.Seek(0L, SeekOrigin.Begin);
        }

        public virtual void Serialization<T>(T obj)
        {
            this.formatter.Serialize(this.stream, obj);
        }

        public virtual void SerializationBoolean(bool value)
        {
            this.Serialization<bool>(value);
        }

        public virtual void SerializationDictionary<T, K>(Dictionary<T, K> dic)
        {
            int count = dic.Count;
            this.SerializationInt32(count);
            foreach (KeyValuePair<T, K> pair in dic)
            {
                this.Serialization<T>(pair.Key);
                this.Serialization<K>(pair.Value);
            }
        }

        public virtual void SerializationFloat(float value)
        {
            this.Serialization<float>(value);
        }

        public virtual void SerializationInt32(int value)
        {
            this.Serialization<int>(value);
        }

        public virtual void SerializationString(string value)
        {
            this.Serialization<string>(value);
        }

        public virtual void SerializationVector2(Vector2 v)
        {
            this.SerializationFloat(v.x);
            this.SerializationFloat(v.y);
        }

        public void Write(byte[] bytes)
        {
            this.stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteToFS(FileStream fs)
        {
            byte[] array = this.Buffer();
            fs.Seek(0L, SeekOrigin.Begin);
            fs.SetLength(0L);
            fs.Write(array, 0, array.Length);
        }
    }
}

