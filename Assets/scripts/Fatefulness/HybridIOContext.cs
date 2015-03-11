namespace Fatefulness
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class HybridIOContext : IOContext
    {
        public override void DeserializationBoolean(out bool ovalue)
        {
            bool flag = base.stream.ReadByte() != 0;
            ovalue = flag;
        }

        public override void DeserializationFloat(out float ovalue)
        {
            int count = 4;
            if ((base.stream.Position + count) > base.stream.Length)
            {
                ovalue = 0f;
            }
            else
            {
                byte[] buffer = new byte[count];
                base.stream.Read(buffer, 0, count);
                float num2 = BitConverter.ToSingle(buffer, 0);
                ovalue = num2;
            }
        }

        public override void DeserializationInt32(out int ovalue)
        {
            int count = 4;
            if ((base.stream.Position + count) > base.stream.Length)
            {
                ovalue = 0;
            }
            else
            {
                byte[] buffer = new byte[count];
                base.stream.Read(buffer, 0, count);
                int num2 = BitConverter.ToInt32(buffer, 0);
                ovalue = num2;
            }
        }

        public override void DeserializationString(out string ovalue)
        {
            int num = 0;
            this.DeserializationInt32(out num);
            if (num <= 0)
            {
                ovalue = string.Empty;
            }
            else if ((base.stream.Position + num) > base.stream.Length)
            {
                ovalue = string.Empty;
            }
            else
            {
                byte[] buffer = new byte[num];
                base.stream.Read(buffer, 0, num);
                string str = Encoding.ASCII.GetString(buffer);
                ovalue = str;
            }
        }

        public override void SerializationBoolean(bool value)
        {
            base.stream.WriteByte(!value ? ((byte) 0) : ((byte) 1));
        }

        public override void SerializationFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            base.stream.Write(bytes, 0, bytes.Length);
        }

        public override void SerializationInt32(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            base.stream.Write(bytes, 0, bytes.Length);
        }

        public override void SerializationString(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            int length = bytes.Length;
            this.SerializationInt32(length);
            base.stream.Write(bytes, 0, length);
        }
    }
}

