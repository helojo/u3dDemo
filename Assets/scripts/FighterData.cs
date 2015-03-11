using System;
using System.Runtime.CompilerServices;

public class FighterData
{
    public FighterData(int id, long hp, int energy, int realID, bool isSummoned, int cardEntry, short qualityLevel, short starLevel, bool isCard, bool isActor, int summonerId)
    {
        this.FighterId = id;
        this.InitHp = hp;
        this.InitEnergy = energy;
        this.CurHp = hp;
        this.CurEnergy = energy;
        this.RealID = realID;
        this.TotalDemage = 0;
        this.IsSummoned = isSummoned;
        this.summonerId = summonerId;
        if (isSummoned)
        {
            this.StartTime = BattleState.GetInstance().GetBattletime();
        }
        else
        {
            this.StartTime = 0f;
        }
        this.EndTime = -1f;
        this.CardEntry = cardEntry;
        this.IsCard = isCard;
        this.IsActor = isActor;
        this.Quality = qualityLevel;
        this.StarLevel = starLevel;
    }

    public void AddDemage(int demage)
    {
        if (demage <= 0)
        {
            this.TotalDemage += demage;
        }
    }

    public float GetAliveTime()
    {
        if (this.EndTime < 0f)
        {
            return -1f;
        }
        return (this.EndTime - this.StartTime);
    }

    public float GetStartTime()
    {
        return this.StartTime;
    }

    public void SetDead()
    {
        this.EndTime = BattleState.GetInstance().GetBattletime();
    }

    public void SetHpAndEnergy(long hp, int energy)
    {
        this.CurHp = hp;
        this.CurEnergy = energy;
    }

    public int CardEntry { get; set; }

    public int CurEnergy { get; set; }

    public long CurHp { get; set; }

    public int EndSeq { get; set; }

    public float EndTime { get; set; }

    public int FighterId { get; set; }

    public int InitEnergy { get; set; }

    public long InitHp { get; set; }

    public bool IsActor { get; set; }

    public bool IsCard { get; set; }

    public bool IsSummoned { get; set; }

    public short Quality { get; set; }

    public int RealID { get; set; }

    public short StarLevel { get; set; }

    public float StartTime { get; set; }

    public int summonerId { get; set; }

    public int TotalDemage { get; set; }
}

