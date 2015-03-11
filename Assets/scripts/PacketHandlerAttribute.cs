using FastBuf;
using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, AllowMultiple=true, Inherited=true)]
public class PacketHandlerAttribute : Attribute
{
    public PacketHandlerAttribute(OpcodeType identifier, System.Type _type)
    {
        this.PacketOpcodeID = identifier;
        this.PacketHandleType = _type;
    }

    public System.Type PacketHandleType { get; set; }

    public OpcodeType PacketOpcodeID { get; set; }
}

