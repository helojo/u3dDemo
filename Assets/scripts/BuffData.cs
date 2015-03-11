using System;
using System.Runtime.CompilerServices;

public class BuffData
{
    private string buffId;
    public int DataEndIdx = -1;
    public int DataIdx = -1;

    public BuffData(string buffId, int buffEntry, int casterId, int targetId, int skillEntry, int subSkillEntry)
    {
        this.buffId = buffId;
        this.Entry = buffEntry;
        this.CasterId = casterId;
        this.TargetId = targetId;
        this.AddTime = BattleState.GetInstance().GetBattletime();
        this.SkillEntry = skillEntry;
        this.subSkillEntry = subSkillEntry;
    }

    public float GetBuffRemainTime()
    {
        return (this.DelTime - this.AddTime);
    }

    public float GetStartTime()
    {
        return this.AddTime;
    }

    public void SetBuffEnd()
    {
        this.DelTime = BattleState.GetInstance().GetBattletime();
    }

    public float AddTime { get; set; }

    public string BuffId
    {
        get
        {
            return this.buffId;
        }
    }

    public int CasterId { get; set; }

    public float DelTime { get; set; }

    public int Entry { get; set; }

    public int SkillEntry { get; set; }

    public int subSkillEntry { get; set; }

    public int TargetId { get; set; }
}

