using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct OutlandPressInfo
{
    public int outlantType;
    public int toll_gate_entry;
    public int limitLevel;
}

