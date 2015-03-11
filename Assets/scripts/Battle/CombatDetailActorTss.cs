namespace Battle
{
    using FastBuf;
    using System;
    using System.Collections.Generic;

    public class CombatDetailActorTss
    {
        public TssSdtInt activeSkill;
        public TssSdtInt attack;
        public TssSdtFloat attackSpeed;
        public TssSdtInt backupSkill;
        public TssSdtInt beHealMod;
        public bool canMove;
        public TssSdtInt cardEntry;
        public TssSdtInt critDmg;
        public TssSdtInt critRate;
        public TssSdtLong curHp;
        public TssSdtInt defaultSkill;
        public TssSdtInt dodgeRate;
        public TssSdtInt energy;
        public TssSdtInt energyRecover;
        public TssSdtInt energyRecoverOnAttack;
        public TssSdtInt energyRemain;
        public TssSdtShort entry;
        public CombatEquip equip = new CombatEquip();
        public TssSdtInt foremostSkill;
        public TssSdtInt hate;
        public TssSdtInt healMod;
        public TssSdtInt hitRate;
        public TssSdtInt hpRecover;
        public TssSdtInt hpRecoverOnAttack;
        public bool isCard;
        public bool isImmune;
        public TssSdtShort level;
        public TssSdtLong maxHp;
        public List<int> normalSkills = new List<int>();
        public TssSdtInt physicsDefence;
        public TssSdtInt physicsDmgReduce;
        public TssSdtInt physicsPierce;
        public TssSdtInt prewarSkill;
        public TssSdtShort quality;
        public TssSdtFloat radius;
        public List<SkillLvInfoTss> skillLvList = new List<SkillLvInfoTss>();
        public TssSdtInt spellDefence;
        public TssSdtInt spellDmgReduce;
        public TssSdtInt spellPierce;
        public TssSdtInt suckLv;
        public TssSdtInt tenacity;

        public CombatDetailActorTss(CombatDetailActor baseData)
        {
            this.activeSkill = baseData.activeSkill;
            this.attack = baseData.attack;
            this.attackSpeed = baseData.attackSpeed;
            this.backupSkill = baseData.backupSkill;
            this.beHealMod = baseData.beHealMod;
            this.canMove = baseData.canMove;
            this.critDmg = baseData.critDmg;
            this.critRate = baseData.critRate;
            this.curHp = (TssSdtLong) baseData.curHp;
            this.defaultSkill = baseData.defaultSkill;
            this.dodgeRate = baseData.dodgeRate;
            this.energy = baseData.energy;
            this.energyRecover = baseData.energyRecover;
            this.energyRecoverOnAttack = baseData.energyRecoverOnAttack;
            this.energyRemain = baseData.energyRemain;
            this.entry = baseData.entry;
            this.equip = baseData.equip;
            this.normalSkills = baseData.normalSkills;
            this.InitSkillLevelList(baseData.skillLvList);
            this.foremostSkill = baseData.foremostSkill;
            this.prewarSkill = baseData.prewarSkill;
            this.hate = baseData.hate;
            this.healMod = baseData.healMod;
            this.hitRate = baseData.hitRate;
            this.hpRecover = baseData.hpRecover;
            this.hpRecoverOnAttack = baseData.hpRecoverOnAttack;
            this.isCard = baseData.isCard;
            this.isImmune = baseData.isImmune;
            this.level = baseData.level;
            this.maxHp = (TssSdtLong) baseData.maxHp;
            this.physicsDefence = baseData.physicsDefence;
            this.physicsDmgReduce = baseData.physicsDmgReduce;
            this.physicsPierce = baseData.physicsPierce;
            this.quality = baseData.quality;
            this.radius = baseData.radius;
            this.spellDefence = baseData.spellDefence;
            this.spellDmgReduce = baseData.spellDmgReduce;
            this.spellPierce = baseData.spellPierce;
            this.suckLv = baseData.suckLv;
            this.tenacity = baseData.tenacity;
            if (baseData.isCard)
            {
                this.cardEntry = (TssSdtInt) baseData.entry;
            }
            else
            {
                monster_config _config = ConfigMgr.getInstance().getByEntry<monster_config>(baseData.entry);
                if (_config != null)
                {
                    this.cardEntry = _config.card_entry;
                }
            }
        }

        private void InitSkillLevelList(List<SkillLvInfo> m_skillLvList)
        {
            foreach (SkillLvInfo info in m_skillLvList)
            {
                SkillLvInfoTss item = new SkillLvInfoTss {
                    entry = info.entry,
                    level = info.level
                };
                this.skillLvList.Add(item);
            }
        }
    }
}

