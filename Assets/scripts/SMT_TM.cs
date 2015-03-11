using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct SMT_TM
{
    public int day;
    public int hour;
    public int minute;
    public int second;
    public MTTM_Type type;
    public void Clear()
    {
        this.day = -1;
        this.hour = -1;
        this.minute = -1;
        this.second = -1;
        this.type = MTTM_Type.TM_CLOSE;
    }
    public enum MTTM_Type
    {
        TM_OPEN,
        TM_CLOSE,
        TM_NEVERMORE
    }
}

