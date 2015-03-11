using FastBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PacketMgr : MonoBehaviour
{
    private Dictionary<OpcodeType, HandlePacketDelegate> _gamePacketHandlers = new Dictionary<OpcodeType, HandlePacketDelegate>();
    private Dictionary<OpcodeType, System.Type> _gamePacketReflections = new Dictionary<OpcodeType, System.Type>();
    public static PacketMgr Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Packet DeserializeGamePacket(OpcodeType opcode, Stream stm)
    {
        System.Type type = null;
        if (this._gamePacketReflections.TryGetValue(opcode, out type))
        {
            if (type == null)
            {
                Debug.LogWarning("UnKnow Opcode:[" + opcode + "] On DeserializeGamePacket");
                return null;
            }
            IExtensible extensible = Activator.CreateInstance(type) as IExtensible;
            BufferReader source = new BufferReader(stm);
            try
            {
                extensible.Deserialize(source);
            }
            catch (Exception exception)
            {
                Debug.LogError("Deserialize Failed: OpCode:" + opcode.ToString() + " Error:" + exception.ToString());
            }
            return new Packet(opcode, extensible);
        }
        Debug.LogWarning("UnKnow Opcode:[" + opcode + "],spell check PacketHandlerAttribute on Receive Message");
        return null;
    }

    public bool HandleGamePacket(Packet packet)
    {
        HandlePacketDelegate delegate2 = null;
        if (this._gamePacketHandlers.TryGetValue(packet.OpCode, out delegate2))
        {
            delegate2(packet);
            return true;
        }
        return false;
    }

    private void Init()
    {
        this.RegisterObjectHandler(SocketMgr.Instance, this._gamePacketHandlers, this._gamePacketReflections);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void RegisterHandler(PacketHandlerAttribute attr, HandlePacketDelegate fun, Dictionary<OpcodeType, HandlePacketDelegate> handlers, Dictionary<OpcodeType, System.Type> reflections)
    {
        handlers[attr.PacketOpcodeID] = fun;
        reflections[attr.PacketOpcodeID] = attr.PacketHandleType;
    }

    protected void RegisterObjectHandler(object obj, Dictionary<OpcodeType, HandlePacketDelegate> handlers, Dictionary<OpcodeType, System.Type> reflections)
    {
        handlers.Clear();
        reflections.Clear();
        foreach (MethodInfo info in obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
        {
            PacketHandlerAttribute[] customAttributes = info.GetCustomAttributes(typeof(PacketHandlerAttribute), false) as PacketHandlerAttribute[];
            if (customAttributes.Length != 0)
            {
                try
                {
                    HandlePacketDelegate fun = (HandlePacketDelegate) Delegate.CreateDelegate(typeof(HandlePacketDelegate), obj, info);
                    foreach (PacketHandlerAttribute attribute in customAttributes)
                    {
                        this.RegisterHandler(attribute, fun, handlers, reflections);
                    }
                }
                catch (Exception exception)
                {
                    string str = obj.GetType().FullName + "." + info.Name;
                    throw new Exception("Unable to register PacketHandler " + str + ".\n" + exception.Message);
                }
            }
        }
    }

    private void Start()
    {
        this.Init();
    }

    public delegate void HandlePacketDelegate(Packet p);
}

