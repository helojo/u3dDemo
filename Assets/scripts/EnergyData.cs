using System;
using System.Runtime.CompilerServices;

public class EnergyData
{
    public EnergyData(int actorId, HitEnergyType energyType, int ChangeValue)
    {
        this.ActorId = actorId;
        this.EnergyType = energyType;
        this.ChangeValue = ChangeValue;
        this.ChangeTime = BattleState.GetInstance().GetBattletime();
    }

    public float GetStartTime()
    {
        return this.ChangeTime;
    }

    public int ActorId { get; set; }

    public float ChangeTime { get; set; }

    public int ChangeValue { get; set; }

    public int DataIdx { get; set; }

    public HitEnergyType EnergyType { get; set; }
}

