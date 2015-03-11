using System;

public class BaseSkillData
{
    public int CasterID;
    public float CastTime;
    public int Index;
    public int SkillEntry;

    public BaseSkillData(int skillEntry, int casterId)
    {
        this.SkillEntry = skillEntry;
        this.CasterID = casterId;
        this.CastTime = BattleState.GetInstance().GetBattletime();
    }
}

