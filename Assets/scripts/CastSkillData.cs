using System;
using System.Runtime.CompilerServices;

public class CastSkillData
{
    private string skillId = string.Empty;

    public CastSkillData(string skillId, int skillEntry, int subSkillEntry, int targetId, int casterId, SkillHitType hitType, int valueChange)
    {
        this.skillId = skillId;
        this.SkillEntry = skillEntry;
        this.SubSkillEntry = subSkillEntry;
        this.TargetId = targetId;
        this.CasterId = casterId;
        this.HitType = hitType;
        this.OtherHitType = hitType;
        this.ValueChange = valueChange;
        this.CastTime = BattleState.GetInstance().GetBattletime();
    }

    public void Log()
    {
    }

    public int CasterAttack { get; set; }

    public int CasterCriDmg { get; set; }

    public int CasterEnergy { get; set; }

    public int CasterHp { get; set; }

    public int CasterId { get; set; }

    public int CasterPhysicsPierce { get; set; }

    public int CasterSpellPierce { get; set; }

    public float CastTime { get; set; }

    public int DataIdx { get; set; }

    public int HealMod { get; set; }

    public int HealValue { get; set; }

    public SkillHitType HitType { get; set; }

    public SkillHitType OtherHitType { get; set; }

    public int OtherValueChange { get; set; }

    public int SkillEntry { get; set; }

    public string SkillId
    {
        get
        {
            return this.skillId;
        }
    }

    public int SubSkillEntry { get; set; }

    public int TargetEnergy { get; set; }

    public int TargetHp { get; set; }

    public int TargetId { get; set; }

    public int ValueChange { get; set; }
}

