using FastBuf;
using System;

public class Packet
{
    private IExtensible _object;
    private OpcodeType _opcode;

    public Packet(OpcodeType type)
    {
        this._opcode = type;
    }

    public Packet(OpcodeType type, IExtensible obj)
    {
        this._opcode = type;
        this._object = obj;
    }

    public OpcodeType OpCode
    {
        get
        {
            return this._opcode;
        }
    }

    public IExtensible PacketObject
    {
        get
        {
            return this._object;
        }
        set
        {
            this._object = value;
        }
    }
}

