namespace Battle
{
    using System;

    public class SkillEffectResult
    {
        public int _attack;
        public int _healValue;
        public int _PhysicsPierce;
        public int _SpellPierce;
        public int bufferEntry = -1;
        public int changeValue;
        public int criDmg;
        public bool DefendBadHurt;
        public SubSkillType effectType;
        public HitEnergyType energyChangeType;
        public int healMod;
        public SkillHitType hitType;
        public HitHpType hpChangeType;
        public int hurtReduce;
        public SkillHurtType hurtType;
        public int otherChangeValue;
        public SkillHitType otherHitType;
        public int subSkillIndex;
        public int suckValue;
        public int summonTargetID = -1;
        public int targetID;
        public long value;

        public SkillEffectResult(SkillHitType hitType, long _value)
        {
            this.hitType = hitType;
            this.effectType = SubSkillType.None;
            this.value = _value;
            this.hpChangeType = HitHpType.Normal;
            this.energyChangeType = HitEnergyType.Disable;
        }

        public bool IsEnergyChange()
        {
            return ((this.effectType == SubSkillType.AddEnergy) || (this.effectType == SubSkillType.SubEnergy));
        }

        public bool IsHpChange()
        {
            return ((this.effectType == SubSkillType.Heal) || (this.effectType == SubSkillType.Hurt));
        }
    }
}

