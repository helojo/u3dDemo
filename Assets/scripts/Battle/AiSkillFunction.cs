namespace Battle
{
    using FastBuf;
    using Fatefulness;
    using System;
    using System.Text;
    using UnityEngine;

    public class AiSkillFunction
    {
        private static uint randomSeed;

        public static SkillHitType ComputeHealType(int critRate, int random)
        {
            if (random <= critRate)
            {
                return SkillHitType.Cri;
            }
            return SkillHitType.Hit;
        }

        public static bool ComputeIsCRI(float _CritRate, AiActor target, int random)
        {
            int num = ((int) _CritRate) - target.FixedTenacity;
            return (random <= num);
        }

        public static SkillHitType ComputeSkillHit(SubSkillHitType hitType, AiActor caster, AiActor target, int hitLv)
        {
            int random = GetRandom();
            if (hitType == SubSkillHitType.None)
            {
                return SkillHitType.Hit;
            }
            if (hitType == SubSkillHitType.Common)
            {
                float baseHitRate = caster.BaseHitRate;
                float fixedDodRate = target.FixedDodRate;
                float fixedHitRate = caster.FixedHitRate;
                float num5 = ((baseHitRate / (baseHitRate + fixedDodRate)) + (fixedHitRate / ((float) AiDef.ONE_HUNDRED_PCT_VALUE))) - 1f;
                return ((random > ((int) (num5 * AiDef.ONE_HUNDRED_PCT_VALUE))) ? SkillHitType.Miss : SkillHitType.Hit);
            }
            int num6 = hitLv;
            if (num6 >= target.Level)
            {
                return SkillHitType.Hit;
            }
            float num7 = ((float) ConfigMgr.getInstance().getByEntry<variable_config>(0).hit_para) / ((float) AiDef.ONE_HUNDRED_PCT_VALUE);
            float num8 = 1f - ((((float) (target.Level - num6)) / 2f) * num7);
            if (random <= ((int) (num8 * AiDef.ONE_HUNDRED_PCT_VALUE)))
            {
                return SkillHitType.Hit;
            }
            return SkillHitType.Miss;
        }

        public static long ComputeSubSKillEffect(long dmgValue, SkillHitType hitType, sub_skill_config subSkillData, int skillLV, int CritDmg)
        {
            dmgValue = (dmgValue * subSkillData.HurtScale) / ((long) AiDef.ONE_HUNDRED_PCT_VALUE);
            dmgValue = (dmgValue + subSkillData.HurtValue) + (subSkillData.skill_levelup_value * (skillLV - 1));
            if (hitType == SkillHitType.Cri)
            {
                dmgValue = (dmgValue * CritDmg) / ((long) AiDef.ONE_HUNDRED_PCT_VALUE);
            }
            return dmgValue;
        }

        private static void DoHeal(SkillEffectResult result, sub_skill_config subSkillData, AiManager _aiManager, AiSkill skill, AiActor _caster, AiActor _target, int _subSkillEntry, Fragment _aiFate, int skillHitLv, int skillLv, AiBuffInfo buffCombatInfo, int criRandom)
        {
            result.hitType = ComputeHealType(_caster.FixedCritRate, criRandom);
            if (_target.IsAlive && _target.IsCanBeHealed)
            {
                long dmgValue = 0L;
                int critDmg = 0;
                int fixedHealMod = 0;
                if (result.hurtType == SkillHurtType.Physics)
                {
                    dmgValue = _caster.FixedPhysicsAttack;
                }
                else if (result.hurtType == SkillHurtType.Magic)
                {
                    dmgValue = _caster.FixedMagicAttack;
                }
                else
                {
                    dmgValue = _caster.FixedAttack;
                }
                critDmg = _caster.FixedCritDmg;
                fixedHealMod = _caster.FixedHealMod;
                result.healMod = fixedHealMod;
                result.criDmg = critDmg;
                result._healValue = (int) dmgValue;
                if ((_caster != null) && _caster.IsSummonActor)
                {
                    dmgValue = ComputeSubSKillEffect(dmgValue, result.hitType, subSkillData, (int) _caster.SummonSkillLevel, critDmg);
                }
                else
                {
                    dmgValue = ComputeSubSKillEffect(dmgValue, result.hitType, subSkillData, skillLv, critDmg);
                }
                dmgValue = (dmgValue * (AiDef.ONE_HUNDRED_PCT_VALUE + fixedHealMod)) / ((long) AiDef.ONE_HUNDRED_PCT_VALUE);
                dmgValue = (dmgValue * (AiDef.ONE_HUNDRED_PCT_VALUE + _target.FixedBeHealMod)) / ((long) AiDef.ONE_HUNDRED_PCT_VALUE);
                int b = (int) dmgValue;
                b = Mathf.Max(1, b);
                result.changeValue = b;
                _target.IncHp(b);
                BattleSecurityManager.Instance.AddHpChangeData(_target.ServerIdx, ChangeHpType.HpType_BySkill, b);
            }
            else
            {
                result.hitType = SkillHitType.Miss;
            }
            result.value = (long) _target.Hp;
        }

        private static void DoHurt(SkillEffectResult result, sub_skill_config subSkillData, AiManager _aiManager, AiSkill skill, AiActor _caster, AiActor _target, int _subSkillEntry, Fragment _aiFate, int skillHitLv, int skillLv, bool isSkillHit, AiBuffInfo buffCombatInfo, int criRandom)
        {
            bool flag = ComputeIsCRI((float) _caster.FixedCritRate, _target, criRandom);
            long dmgValue = 0L;
            if (!_target.IsDead && _target.IsCanBehurt)
            {
                if (_target.UseHurtDenfender())
                {
                    result.hitType = SkillHitType.Absorb;
                }
                else if (!_target.IsCanBeAttackBySkillType((SkillHurtType) subSkillData.HurtType))
                {
                    result.hitType = SkillHitType.DefendBadState;
                }
                else if (!_target.IsCanBeHurtByMagic && (subSkillData.HurtType == 1))
                {
                    result.hitType = SkillHitType.DefendBadState;
                    result.DefendBadHurt = true;
                }
                else if (!_target.IsCanBeHurtByPhysics && (subSkillData.HurtType == 0))
                {
                    result.hitType = SkillHitType.DefendBadState;
                    result.DefendBadHurt = true;
                }
                else
                {
                    long fixedPhysicsAttack = 0L;
                    long num3 = 0L;
                    int fixedPhysicsDmgReduce = 0;
                    int fixedPhysicsDmgIncrease = 0;
                    int critDmg = 0;
                    int fixedSpellPierce = 0;
                    int fixedPhysicsPierce = 0;
                    if (result.hurtType == SkillHurtType.Physics)
                    {
                        fixedPhysicsAttack = _caster.FixedPhysicsAttack;
                    }
                    else if (result.hurtType == SkillHurtType.Magic)
                    {
                        fixedPhysicsAttack = _caster.FixedMagicAttack;
                    }
                    else
                    {
                        fixedPhysicsAttack = _caster.FixedAttack;
                    }
                    critDmg = _caster.FixedCritDmg;
                    fixedSpellPierce = _caster.FixedSpellPierce;
                    fixedPhysicsPierce = _caster.FixedPhysicsPierce;
                    result.criDmg = critDmg;
                    result._SpellPierce = fixedSpellPierce;
                    result._attack = (int) fixedPhysicsAttack;
                    result._PhysicsPierce = fixedPhysicsPierce;
                    if (isSkillHit)
                    {
                        if (flag)
                        {
                            result.hitType = SkillHitType.Cri;
                        }
                        else
                        {
                            result.hitType = SkillHitType.Hit;
                        }
                    }
                    else
                    {
                        result.hitType = SkillHitType.Miss;
                    }
                    if ((result.hitType == SkillHitType.Hit) || (result.hitType == SkillHitType.Cri))
                    {
                        if (subSkillData.HurtType == 0)
                        {
                            num3 = _target.FixedPhysicsDefender - fixedPhysicsPierce;
                            fixedPhysicsDmgReduce = _target.FixedPhysicsDmgReduce;
                            fixedPhysicsDmgIncrease = _target.FixedPhysicsDmgIncrease;
                        }
                        else
                        {
                            num3 = _target.FixedSpellDefence - fixedSpellPierce;
                            fixedPhysicsDmgReduce = _target.FixedSpellDmgReduce;
                            fixedPhysicsDmgIncrease = _target.FixedSpellDmgIncrease;
                        }
                        if (fixedPhysicsAttack < 1L)
                        {
                            fixedPhysicsAttack = 1L;
                        }
                        if (num3 < 0L)
                        {
                            num3 = 0L;
                        }
                        dmgValue = (fixedPhysicsAttack * fixedPhysicsAttack) / (fixedPhysicsAttack + num3);
                        if ((_caster != null) && _caster.IsSummonActor)
                        {
                            dmgValue = ComputeSubSKillEffect(dmgValue, result.hitType, subSkillData, (int) _caster.SummonSkillLevel, critDmg);
                        }
                        else
                        {
                            dmgValue = ComputeSubSKillEffect(dmgValue, result.hitType, subSkillData, skillLv, critDmg);
                        }
                        variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
                        if (_config != null)
                        {
                            if ((subSkillData.HurtType == 0) && (fixedPhysicsDmgReduce >= _config.max_be_attack_pct_mod))
                            {
                                fixedPhysicsDmgReduce = _config.max_be_attack_pct_mod;
                            }
                            else if ((subSkillData.HurtType == 1) && (fixedPhysicsDmgReduce >= _config.max_be_spell_attack_pct_mod))
                            {
                                fixedPhysicsDmgReduce = _config.max_be_spell_attack_pct_mod;
                            }
                        }
                        dmgValue = (dmgValue * Mathf.Max(0, AiDef.ONE_HUNDRED_PCT_VALUE - fixedPhysicsDmgReduce)) / ((long) AiDef.ONE_HUNDRED_PCT_VALUE);
                        dmgValue = (dmgValue * fixedPhysicsDmgIncrease) / ((long) AiDef.ONE_HUNDRED_PCT_VALUE);
                        if (dmgValue < 1L)
                        {
                            dmgValue = 1L;
                        }
                        int hurtValue = (int) dmgValue;
                        int num11 = hurtValue;
                        hurtValue = _target.UseHurtReduce(hurtValue);
                        result.hurtReduce = num11 - hurtValue;
                        if (hurtValue == 0)
                        {
                            result.hitType = SkillHitType.Absorb;
                        }
                        else
                        {
                            int b = (int) ((hurtValue * 100L) / _target.MaxHpInit);
                            b = Mathf.Max(1, b);
                            float fixedSuckRate = _caster.FixedSuckRate;
                            int changeValue = 0;
                            if (((fixedSuckRate > 0f) && (subSkillData.SuckType != 1)) && !_target.IsSummonActor)
                            {
                                changeValue = (int) (hurtValue * fixedSuckRate);
                                result.suckValue = changeValue;
                                if (_caster.IsAlive)
                                {
                                    _caster.ChangeHpAndNotice((long) changeValue);
                                    BattleSecurityManager.Instance.AddHpChangeData(_caster.ServerIdx, ChangeHpType.HpType_BySuck, changeValue);
                                }
                            }
                            hurtValue = _target.SelfTeam.UseHurtHpShare(hurtValue, _target);
                            result.changeValue = -hurtValue;
                            _target.DecHp(hurtValue);
                            BattleSecurityManager.Instance.AddHpChangeData(_target.ServerIdx, ChangeHpType.HpType_BySkill, -hurtValue);
                            variable_config _config2 = ConfigMgr.getInstance().getByEntry<variable_config>(0);
                            if (_config2 != null)
                            {
                                _target.OnEnergyChange(b * _config2.inc_energy_on_hurt, HitEnergyType.None);
                            }
                        }
                    }
                }
            }
            else if (!_target.IsDead && !_target.IsCanBehurt)
            {
                result.hitType = SkillHitType.DefendBadState;
            }
            else
            {
                result.hitType = !isSkillHit ? SkillHitType.Miss : SkillHitType.Hit;
            }
            result.value = (long) _target.Hp;
        }

        private static void DoSummon(SkillEffectResult result, sub_skill_config subSkillData, AiManager _aiManager, AiActor _caster, AiActor _target, int _subSkillEntry, Fragment _aiFate, int skillHitLv, int skillLv)
        {
            int id = subSkillData.buff_entry;
            summon_config _config = ConfigMgr.getInstance().getByEntry<summon_config>(id);
            if (_config != null)
            {
                CombatDetailActor baseData = BattleGlobalFunc.CreateDetailInfoOnMonster(_config.monster_entry);
                baseData.attack += (_caster.FixedAttack * _config.attack_summon_scale) / AiDef.ONE_HUNDRED_PCT_VALUE;
                baseData.attack += (skillLv - 1) * _config.attack_level_up;
                int num2 = (skillLv - 1) * _config.hp_level_up;
                baseData.curHp += num2;
                baseData.maxHp += num2;
                baseData.level = (short) _caster.Level;
                AiActor creature = null;
                if (_config.type == 0)
                {
                    creature = _caster.SelfTeam.AddActor(baseData, _aiManager);
                }
                else
                {
                    creature = _caster.SelfTeam.AddNoBattleActor(baseData, _aiManager);
                }
                if (creature.IsAttacker)
                {
                    creature.ServerIdx = _caster.SelfTeam.GetAllActorCount();
                }
                else
                {
                    creature.ServerIdx = _caster.SelfTeam.GetAllActorCount() + AiDef.MAX_ACTOR_EACH_SIDE;
                }
                creature.ToDeadTime = _config.duration;
                creature.IsSummonActor = true;
                creature.SummonerId = _caster.Id;
                creature.SummonSkillLevel = skillLv;
                Vector3 pos = (Vector3) ((_target.Pos + (_caster.HeadDirection * _config.OffsetX)) + (Vector3.Cross(_caster.HeadDirection, Vector3.up) * _config.OffsetY));
                _aiManager.NoticeSummon(creature, _config.monster_entry, _caster, pos);
                result.summonTargetID = creature.Id;
                BattleSecurityManager.Instance.RegisterFigherData(creature.ServerIdx, (long) creature.Hp, creature.Energy, creature.Id, true, id, -1, -1, false, creature.IsAttacker, (_caster == null) ? -1 : _caster.Id);
                if (!string.IsNullOrEmpty(_config.born_effect))
                {
                    _aiManager.OnEventShowEffect(null, _config.born_effect, creature);
                }
                creature.CastForemostSkill();
            }
        }

        private static string EffectResult2FatePort(SkillEffectResult result)
        {
            switch (result.effectType)
            {
                case SubSkillType.Hurt:
                    return "Hurt";

                case SubSkillType.Heal:
                    return "Heal";

                case SubSkillType.Revive:
                    return "Revive";

                case SubSkillType.Summon:
                    return "Summon";
            }
            return string.Empty;
        }

        public static SkillEffectResult G_OnTakeSkillEffectByID(AiManager _aiManager, AiSkill skill, AiActor _caster, AiActor _target, int _subSkillEntry, Fragment _aiFate, int skillHitLv, int skillLv, AiBuffInfo buffCombatInfo)
        {
            int num3;
            SkillEffectResult result = new SkillEffectResult(SkillHitType.Hit, (long) _target.Hp);
            sub_skill_config subSkillData = ConfigMgr.getInstance().getByEntry<sub_skill_config>(_subSkillEntry);
            if (subSkillData == null)
            {
                return result;
            }
            if ((subSkillData != null) && (subSkillData.sub_target_type == 2))
            {
                _target = _caster;
            }
            result.hitType = ComputeSkillHit((SubSkillHitType) subSkillData.hit_type, _caster, _target, skillHitLv);
            int random = GetRandom();
            result.effectType = (SubSkillType) subSkillData.type;
            result.hurtType = (SkillHurtType) subSkillData.HurtType;
            result.subSkillIndex = BattleSecurityManager.Instance.GetBattleDataIndex();
            bool flag = false;
            switch (result.effectType)
            {
                case SubSkillType.Hurt:
                    if (_target.IsDead || !_target.IsCanBehurt)
                    {
                        flag = true;
                    }
                    DoHurt(result, subSkillData, _aiManager, skill, _caster, _target, _subSkillEntry, _aiFate, skillHitLv, skillLv, result.hitType == SkillHitType.Hit, buffCombatInfo, random);
                    goto Label_0319;

                case SubSkillType.Heal:
                    DoHeal(result, subSkillData, _aiManager, skill, _caster, _target, _subSkillEntry, _aiFate, skillHitLv, skillLv, buffCombatInfo, random);
                    goto Label_0319;

                case SubSkillType.State:
                    num3 = -1;
                    if (skill == null)
                    {
                        if (buffCombatInfo != null)
                        {
                            num3 = (int) buffCombatInfo.skillEntry;
                        }
                        break;
                    }
                    num3 = skill.SkillEntry;
                    break;

                case SubSkillType.Revive:
                {
                    long num2 = (((long) (_caster.FixedAttack * (((float) subSkillData.HurtScale) / ((float) AiDef.ONE_HUNDRED_PCT_VALUE)))) + subSkillData.HurtValue) + (subSkillData.skill_levelup_value * (skillLv - 1));
                    num2 = (long) (num2 * _target.PKHpScale);
                    _target.Revive(num2);
                    goto Label_0319;
                }
                case SubSkillType.Summon:
                    DoSummon(result, subSkillData, _aiManager, _caster, _target, _subSkillEntry, _aiFate, skillHitLv, skillLv);
                    goto Label_0319;

                case SubSkillType.SubEnergy:
                {
                    int energy = _target.Energy;
                    long num5 = ComputeSubSKillEffect((long) _target.Energy, SkillHitType.Hit, subSkillData, skillLv, _caster.FixedCritDmg);
                    _target.SetEnergy((int) num5, HitEnergyType.BySkill);
                    result.value = _target.Energy;
                    result.changeValue = _target.Energy - energy;
                    goto Label_0319;
                }
                case SubSkillType.AddEnergy:
                {
                    int num6 = _target.Energy;
                    long num7 = ComputeSubSKillEffect((long) _target.Energy, SkillHitType.Hit, subSkillData, skillLv, _caster.FixedCritDmg);
                    _target.SetEnergy((int) num7, HitEnergyType.BySkill);
                    result.value = _target.Energy;
                    result.changeValue = _target.Energy - num6;
                    goto Label_0319;
                }
                case SubSkillType.BurnEnergy:
                {
                    result.effectType = SubSkillType.Hurt;
                    int num8 = _target.Energy;
                    int num9 = (int) (num8 * (((float) subSkillData.HurtValue) / ((float) AiDef.ONE_HUNDRED_PCT_VALUE)));
                    float num10 = ((float) (subSkillData.HurtValueScale + ((skillLv - 1) * subSkillData.skill_levelup_value))) / 100f;
                    int dmg = (int) (num9 * num10);
                    _target.SetEnergy(num8 - num9, HitEnergyType.BySkill);
                    result.changeValue = -dmg;
                    _target.DecHp(dmg);
                    result.value = (long) _target.Hp;
                    goto Label_0319;
                }
                default:
                    goto Label_0319;
            }
            if ((result.hitType == SkillHitType.Hit) && _target.IsAlive)
            {
                if (_target.AddBuff(subSkillData.buff_entry, _aiManager.Time, _caster.Id, skillLv, skillHitLv, true, num3, _subSkillEntry) != null)
                {
                    result.bufferEntry = subSkillData.buff_entry;
                }
                else
                {
                    result.hitType = SkillHitType.DefendBadState;
                }
            }
            else
            {
                result.hitType = SkillHitType.Miss;
            }
        Label_0319:
            result.targetID = _target.Id;
            int skillEntry = 0;
            if (skill != null)
            {
                skillEntry = skill.SkillEntry;
            }
            else if (buffCombatInfo != null)
            {
                skillEntry = (int) buffCombatInfo.skillEntry;
            }
            int targetId = -1;
            if (result.effectType == SubSkillType.Summon)
            {
                AiActor actorById = _aiManager.GetActorById(result.summonTargetID);
                targetId = (actorById == null) ? -1 : actorById.ServerIdx;
            }
            else
            {
                targetId = (_target == null) ? -1 : _target.ServerIdx;
            }
            BattleSecurityManager.Instance.RegisterCasterSkillData((skill == null) ? string.Empty : skill.AiSkillId, skillEntry, _subSkillEntry, targetId, (_caster == null) ? -1 : _caster.ServerIdx, result.hitType, result.changeValue, result.otherHitType, result.otherChangeValue, (_caster == null) ? -1 : ((int) _caster.Hp), (_target == null) ? -1 : ((int) _target.Hp), (_target == null) ? -1 : _target.Energy, (_caster == null) ? -1 : _caster.Energy, result._SpellPierce, result._PhysicsPierce, result._attack, result.criDmg, result._healValue, result.healMod, result.subSkillIndex);
            if (flag)
            {
                result.hitType = SkillHitType.DefendBadState;
            }
            if (_aiManager.EvetntOnSkillEffectResult != null)
            {
                _aiManager.EvetntOnSkillEffectResult(result);
            }
            _aiManager.OnSkillTakeEffectEvent(_caster.Id, result);
            if (!string.IsNullOrEmpty(subSkillData.effects))
            {
                _aiManager.OnEventShowEffect(skill, subSkillData.effects, _target);
            }
            LogSkillEffect(result, skill, _subSkillEntry, _caster, _target);
            if (_aiFate != null)
            {
                Intent context = new Intent(Intent.IntentType.forward);
                context.PutObject(SkillIntentDecl.skill_effect_result, result);
                context.PutString(IntentDecl.port, EffectResult2FatePort(result));
                _aiFate.Dispatch(context);
                if ((result.effectType == SubSkillType.Hurt) && (result.value <= 0L))
                {
                    Intent intent2 = new Intent(Intent.IntentType.forward);
                    intent2.PutObject(SkillIntentDecl.skill_effect_result, result);
                    intent2.PutString(IntentDecl.port, "Killed");
                    _aiFate.Dispatch(intent2);
                }
            }
            return result;
        }

        public static int GetRandom()
        {
            randomSeed = ((0x41c64e6d * randomSeed) + 0x3039) & uint.MaxValue;
            uint num = (uint) (((ulong) (randomSeed * AiDef.ONE_HUNDRED_PCT_VALUE)) / 0xffffffffL);
            return (int) num;
        }

        private static void LogSkillEffect(SkillEffectResult result, AiSkill skill, int subSkillEntry, AiActor caster, AiActor target)
        {
            if (AiSkill.G_IsShowSkillLog)
            {
                int skillUniId = -1;
                int skillEntry = -1;
                if (skill != null)
                {
                    skillUniId = skill.SkillUniId;
                    skillEntry = skill.SkillEntry;
                }
                StringBuilder builder = new StringBuilder();
                builder.Append("SkillLog: Time:" + BattleState.GetInstance().CurGame.battleGameData.GetBattletime());
                builder.Append(result.effectType.ToString());
                switch (result.effectType)
                {
                    case SubSkillType.Hurt:
                    {
                        object[] objArray1 = new object[] { caster.Id, result.targetID, skillUniId, skillEntry, subSkillEntry, caster.GetSkillLevel(skillEntry), result.changeValue };
                        builder.AppendFormat(" caster:{0} target:{1} skillID:{2} skillEntry:{3} subSkillEntry:{4} skillLv:{5} value:{6}", objArray1);
                        object[] objArray2 = new object[] { caster.FixedHitRate, target.FixedDodRate, caster.FixedCritRate, target.FixedTenacity };
                        builder.AppendFormat(" HitRate:{0} DodRate:{1} CritRate:{2} Tenacity:{3}", objArray2);
                        object[] objArray3 = new object[] { result.hitType.ToString(), caster.FixedAttack, caster.FixedPhysicsPierce, target.FixedPhysicsDefender, target.FixedPhysicsDmgReduce, caster.FixedSpellPierce, target.FixedSpellDefence, target.FixedSpellDmgReduce };
                        builder.AppendFormat(" hitType:{0} attack:{1} PhysicsPierce:{2} PhysicsDefender:{3} PhysicsDmgReduce:{4} SpellPierce:{5} SpellDefence:{6} SpellDmgReduce:{7}", objArray3);
                        Debug.Log(builder.ToString());
                        return;
                    }
                    case SubSkillType.Heal:
                    {
                        object[] objArray4 = new object[] { caster.Id, result.targetID, skillUniId, skillEntry, subSkillEntry, caster.GetSkillLevel(skillEntry), result.changeValue };
                        builder.AppendFormat(" caster:{0} target:{1} skillID:{2} skillEntry:{3} subSkillEntry:{4} skillLv:{5} value:{6}", objArray4);
                        builder.AppendFormat(" CritRate:{0}", caster.FixedCritRate);
                        object[] objArray5 = new object[] { result.hitType.ToString(), caster.FixedAttack, caster.FixedHealMod, target.FixedBeHealMod };
                        builder.AppendFormat(" hitType:{0} attack:{1} HealMod:{2} BeHealMod:{3}", objArray5);
                        Debug.Log(builder.ToString());
                        return;
                    }
                }
                object[] args = new object[] { caster.Id, result.targetID, skillUniId, skillEntry, subSkillEntry, caster.GetSkillLevel(skillEntry), result.hitType.ToString() };
                builder.AppendFormat(" caster:{0} target:{1} skillID:{2} skillEntry:{3} subSkillEntry:{4} skillLv:{5} hitType:{6}", args);
                Debug.Log(builder.ToString());
            }
        }

        public static void SetRandomSeed(uint seed)
        {
            randomSeed = seed;
        }
    }
}

