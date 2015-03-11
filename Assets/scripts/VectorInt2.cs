using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct VectorInt2
{
    public int x;
    public int y;
    public VectorInt2(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }
}

