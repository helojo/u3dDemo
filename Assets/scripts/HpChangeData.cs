using System;
using System.Runtime.CompilerServices;

public class HpChangeData
{
    public HpChangeData(int actorId, ChangeHpType type, int changeValue)
    {
        this.ActorId = actorId;
        this.HpType = type;
        this.ChangeValue = changeValue;
    }

    public int ActorId { get; set; }

    public int ChangeValue { get; set; }

    public int DataIdx { get; set; }

    public ChangeHpType HpType { get; set; }
}

