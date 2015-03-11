namespace Battle
{
    using FastBuf;
    using Fatefulness;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public class AiActor : IActor, ObjectTracer
    {
        private TssSdtInt _fixedAttack;
        private TssSdtFloat _fixedAttackSpeed;
        private TssSdtInt _fixedBeHealMod;
        private TssSdtInt _fixedCritDmg;
        private TssSdtInt _fixedCritRate;
        private TssSdtInt _fixedDodRate;
        private TssSdtInt _fixedEnergyRecoverOnAttack;
        private TssSdtInt _fixedHealMod;
        private TssSdtInt _fixedHitRate;
        private TssSdtInt _fixedMagicAttack;
        private TssSdtInt _fixedPhysicsAttack;
        private TssSdtInt _fixedPhysicsDefender;
        private TssSdtInt _fixedPhysicsDmgIncrease;
        private TssSdtInt _fixedPhysicsDmgReduce;
        private TssSdtInt _fixedPhysicsPierce;
        private TssSdtInt _fixedSpellDefence;
        private TssSdtInt _fixedSpellDmgIncrease;
        private TssSdtInt _fixedSpellDmgReduce;
        private TssSdtInt _fixedSpellPierce;
        private TssSdtFloat _fixedSuckRate;
        private TssSdtInt _fixedTenacity;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache32;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache33;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache34;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache35;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache36;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache37;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache38;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache39;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache3A;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache3B;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache3C;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache3D;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache3E;
        [CompilerGenerated]
        private static Func<AiBuff, bool> <>f__am$cache3F;
        private TssSdtInt hurtAfterCurCasting;
        public bool IsCanBeKilled;
        public bool IsSummonActor;
        private CombatDetailActorTss m_baseData;
        private List<AiBuff> m_buffList;
        private TssSdtInt m_energy;
        private List<Fragment> m_Fate;
        private bool m_HasDefender;
        private float m_idleUntilTime;
        private bool m_isAuto;
        private float m_skillCdBeLeft;
        private int m_skillIdxList;
        private AiState m_state;
        public TssSdtLong overHpHurt;
        private float pkHPScale;
        public float RangeScale;
        private List<SkillSelfCDInfo> selfCDOfskills;
        public int ServerIdx;
        public int SummonerId;
        public TssSdtInt SummonSkillLevel;

        public AiActor(CombatDetailActor baseData, AiManager aiManager, bool isAttacker, int _indexOfList, bool _IsBattleObj) : base(aiManager, isAttacker)
        {
            this._fixedTenacity = 0;
            this._fixedCritDmg = 0;
            this._fixedCritRate = 0;
            this._fixedAttackSpeed = 0f;
            this._fixedPhysicsDmgIncrease = 0;
            this._fixedSpellDmgReduce = 0;
            this._fixedPhysicsDmgReduce = 0;
            this._fixedSpellPierce = 0;
            this._fixedSpellDefence = 0;
            this._fixedPhysicsPierce = 0;
            this._fixedPhysicsDefender = 0;
            this._fixedAttack = 0;
            this._fixedDodRate = 0;
            this._fixedHitRate = 0;
            this._fixedMagicAttack = 0;
            this._fixedPhysicsAttack = 0;
            this._fixedEnergyRecoverOnAttack = 0;
            this._fixedBeHealMod = 0;
            this._fixedHealMod = 0;
            this._fixedSpellDmgIncrease = 0;
            this._fixedSuckRate = 0f;
            this.m_isAuto = AiDef.INIT_IS_AUTO;
            this.selfCDOfskills = new List<SkillSelfCDInfo>();
            this.m_buffList = new List<AiBuff>();
            this.m_Fate = new List<Fragment>();
            this.IsCanBeKilled = true;
            this.overHpHurt = 0L;
            this.m_energy = 0;
            this.hurtAfterCurCasting = 0;
            this.SummonSkillLevel = 1;
            this.SummonerId = -1;
            this.pkHPScale = 1f;
            this.RangeScale = 1f;
            this.PreferAngle = 0f;
            if (baseData != null)
            {
                this.m_baseData = new CombatDetailActorTss(baseData);
            }
            if (baseData != null)
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) this.m_baseData.cardEntry);
                if (_config != null)
                {
                    if (_config.pk_hp_ex_pct <= 0)
                    {
                        this.pkHPScale = 1f;
                    }
                    else
                    {
                        this.pkHPScale = 1f + (((float) _config.pk_hp_ex_pct) / AiDef.PK_HP_EX_PCT_VALUE);
                        if ((this.pkHPScale <= 0f) || !BattleState.GetInstance().CurGame.battleGameData.IsPKBattle())
                        {
                            this.pkHPScale = 1f;
                        }
                    }
                }
            }
            this.m_state = AiState.Idle;
            this.indexOfList = _indexOfList;
            this.IsBattleObj = _IsBattleObj;
            this.Init(false);
        }

        public AiBuff AddBuff(int entry, float startTime, int casterID, int skillLV, int _skillHitLV, bool needNotice, int skillEntry, int subSkillEntry)
        {
            if (!this.IsCanAddBuff(entry))
            {
                return null;
            }
            buff_config _config = ConfigMgr.getInstance().getByEntry<buff_config>(entry);
            if (_config == null)
            {
                return null;
            }
            int num = _config.max_layer;
            for (int i = this.m_buffList.Count - 1; i >= 0; i--)
            {
                AiBuff buff = this.m_buffList[i];
                if (buff.Entry == entry)
                {
                    num--;
                    if (num <= 0)
                    {
                        this.RemoveBuff(buff);
                        break;
                    }
                }
            }
            AiBuff item = new AiBuff();
            item.Init(base.m_aiManager.GetActorById(casterID), this, entry, startTime, skillLV, _skillHitLV, skillEntry, subSkillEntry);
            if (this.IsDead)
            {
                return null;
            }
            item.OnAdd();
            this.m_buffList.Add(item);
            if (needNotice)
            {
                base.m_aiManager.NoticeAddBuf(this.Id, entry, (float) item.EndTime, casterID);
            }
            this.CheckBuffChanged(item);
            this.OnAddBuffEvent(entry, (int) item.State);
            return item;
        }

        public void AddHurtAfterCurCasting(int hpChange)
        {
            this.hurtAfterCurCasting += hpChange;
            if (((long) this.hurtAfterCurCasting) > (this.MaxHp / ((long) AiDef.Hurt_Break_Skill_Scale)))
            {
                this.BreakSkill(true, true);
            }
        }

        private void AddLastPhaseBuff()
        {
            AiDataKeepManager.GetInstance().RestoreBuff(this);
        }

        private float AttributeEffect(float baseValue, SkillLogicEffectType type)
        {
            float attributeEffect = this.GetAttributeEffect(type, SkillLogicEffectOperateType.Multiple);
            float num2 = this.GetAttributeEffect(type, SkillLogicEffectOperateType.Add);
            return Mathf.Max((float) 0f, (float) ((baseValue * (1f + (attributeEffect / ((float) AiDef.ONE_HUNDRED_PCT_VALUE)))) + num2));
        }

        private float AttributeEffect_2(float baseValue, SkillLogicEffectType type)
        {
            float attributeEffect = this.GetAttributeEffect(type, SkillLogicEffectOperateType.Multiple);
            float num2 = this.GetAttributeEffect(type, SkillLogicEffectOperateType.Add);
            return ((baseValue * (1f + (attributeEffect / ((float) AiDef.ONE_HUNDRED_PCT_VALUE)))) + num2);
        }

        public void BreakSkill(bool force, bool setIdle = true)
        {
            if (force || !this.IsHasState(StateBuffType.DefendSkillBreak))
            {
                if (setIdle)
                {
                    this.State = AiState.Idle;
                }
                if (this.CastingSkill != null)
                {
                    base.m_aiManager.NoticeBreakSkill(this.Id, this.CastingSkill.SkillUniId);
                    this.CastingSkill = null;
                }
            }
        }

        private void BuffTick(float deltaTime)
        {
            float time = this.Time;
            for (int i = 0; i < this.m_buffList.Count; i++)
            {
                AiBuff buff = this.m_buffList[i];
                if (buff.EndTime <= time)
                {
                    this.RemoveBuff(buff);
                    i--;
                }
                else
                {
                    buff.Tick(deltaTime);
                }
            }
        }

        private void CalNextIdleUntil()
        {
            this.m_idleUntilTime = this.Time + UnityEngine.Random.Range((float) 0.1f, (float) 0.2f);
        }

        public void CalNextPreferAngle(float prob)
        {
            if (UnityEngine.Random.Range((float) 0f, (float) 1f) <= prob)
            {
                float num = base.PosInVertical * 1f;
                if (this.IsAttacker)
                {
                    num = -num;
                }
                this.PreferAngle = Mathf.Clamp((float) (UnityEngine.Random.Range((float) -15f, (float) 15f) + num), (float) -25f, (float) 25f);
            }
        }

        public void CastForemostSkill()
        {
            this.AddLastPhaseBuff();
            this.CastPrewarSkill(true);
            this.CastSkill((int) this.m_baseData.foremostSkill, SkillCategory.Foremost);
        }

        private void CastPrewarSkill(bool beforeBattle)
        {
            skill_config skillData = ConfigMgr.getInstance().getByEntry<skill_config>((int) this.m_baseData.prewarSkill);
            if ((skillData != null) && ((skillData.can_cast_after_dead != 1) || beforeBattle))
            {
                char[] separator = new char[] { '|' };
                foreach (string str in skillData.sub_skill_list.Split(separator))
                {
                    int id = StrParser.ParseDecInt(str);
                    if (id >= 0)
                    {
                        sub_skill_config _config2 = ConfigMgr.getInstance().getByEntry<sub_skill_config>(id);
                        if (_config2 != null)
                        {
                            this.AddBuff(_config2.buff_entry, base.m_aiManager.Time, this.Id, this.GetSkillLevel((int) this.m_baseData.prewarSkill), AiSkill.S_GetHitLevel(skillData, this), true, (int) this.m_baseData.prewarSkill, id);
                        }
                    }
                }
            }
        }

        private void CastSkill(int skillEntry, SkillCategory skillType)
        {
            AiActor target = this.FindTarget(skillEntry, null, null);
            if (target != null)
            {
                this.RemoveStateBuff(StateBuffType.Cloaking);
                if (this.CastingSkill != null)
                {
                    base.m_aiManager.NoticeBreakSkill(this.Id, this.CastingSkill.SkillUniId);
                    this.CastingSkill = null;
                }
                this.State = AiState.Casting;
                this.hurtAfterCurCasting = 0;
                this.CurTarget = target;
                AiSkill skill = new AiSkill(base.m_aiManager.NewSkillUniId(), skillEntry, this, target, base.m_aiManager) {
                    Category = skillType
                };
                this.CastingSkill = skill;
                CastNewSkill_Input input = new CastNewSkill_Input {
                    skillUniId = this.CastingSkill.SkillUniId,
                    skillEntry = this.CastingSkill.SkillEntry,
                    casterID = this.Id,
                    offset = this.Radius + target.Radius
                };
                input.mainTargetList.Add(target.Id);
                BattleSecurityManager.Instance.RegisterBaseSkill(skillEntry, this.ServerIdx);
                if (skillType == SkillCategory.Normal)
                {
                    this.OnNormalSkillCast(skillEntry);
                }
                else if (skillType == SkillCategory.Active)
                {
                    this.OnActiveSkillCast(skillEntry);
                }
                base.m_aiManager.NoticeCastNewSkill(input);
            }
        }

        public void ChangeBuffEffectActiveAndNotice(int buffId, string objName, bool active)
        {
            if (base.m_aiManager.EventSetBufferEffectActive != null)
            {
                base.m_aiManager.EventSetBufferEffectActive(this.Id, buffId, objName, active);
            }
        }

        public void ChangeEnergyAndNotice(int energyChange)
        {
            this.OnEnergyChange(energyChange, HitEnergyType.ByAIFate);
        }

        public SkillEffectResult ChangeHpAndNotice(long hpChange)
        {
            SkillEffectResult result = new SkillEffectResult(SkillHitType.Hit, (long) this.Hp) {
                effectType = (hpChange <= 0L) ? SubSkillType.Hurt : SubSkillType.Heal,
                targetID = this.Id
            };
            int num = 1;
            result.changeValue = (int) hpChange;
            if (hpChange < 0L)
            {
                if ((this.Hp - hpChange) < num)
                {
                    hpChange = (long) (this.Hp - num);
                }
                this.DecHp((int) -hpChange);
            }
            else
            {
                this.IncHp((int) hpChange);
            }
            result.value = (long) this.Hp;
            if (base.m_aiManager.EvetntOnSkillEffectResult != null)
            {
                base.m_aiManager.EvetntOnSkillEffectResult(result);
            }
            return result;
        }

        public void ChangeHpByUseSkill(int hpChange)
        {
            this.ChangeHpAndNotice((long) hpChange);
            BattleSecurityManager.Instance.AddHpChangeData(this.ServerIdx, ChangeHpType.HpType_ByAiFate, hpChange);
        }

        public void ChangeHpByUseSkill(float hpPrecent)
        {
            long hpChange = (long) (this.MaxHpInit * hpPrecent);
            this.ChangeHpAndNotice(hpChange);
            BattleSecurityManager.Instance.AddHpChangeData(this.ServerIdx, ChangeHpType.HpType_ByAiFate, (int) hpChange);
        }

        public void ChangeMaxHP(int changeValue)
        {
            this.MaxHp += changeValue;
            if (changeValue > 0)
            {
                this.Hp += changeValue;
            }
            if (this.Hp > this.MaxHp)
            {
                this.Hp = this.MaxHp;
            }
            if (base.m_aiManager.EventOnActorHPMaxHPCHange != null)
            {
                base.m_aiManager.EventOnActorHPMaxHPCHange(this.Id, (long) this.Hp, (long) this.MaxHp);
            }
        }

        public void ChangeSpeed(float changeValue)
        {
            this.MoveSpeed += changeValue;
            if (base.m_aiManager.EventOnActorMoveSpeedChange != null)
            {
                base.m_aiManager.EventOnActorMoveSpeedChange(this.Id, (float) this.MoveSpeed);
            }
        }

        private void CheckBuffChanged(AiBuff buff)
        {
            if (buff.IsTeamBuff())
            {
                base.SelfTeam.OnTeamBuffChange();
            }
            else if (buff.IsEnemyTeamBuff())
            {
                base.EnemyTeam.OnTeamBuffChange();
            }
            else
            {
                this.OnBuffChanged();
            }
        }

        private void CheckHpMax()
        {
            if (this.Hp > this.MaxHp)
            {
                this.Hp = this.MaxHp;
            }
        }

        internal void DecHp(int dmg)
        {
            if (dmg >= 0)
            {
                long hp = (long) this.Hp;
                if (this.Hp > dmg)
                {
                    this.Hp -= dmg;
                }
                else
                {
                    long num2 = dmg - this.Hp;
                    this.Hp = 0L;
                    if (!this.IsCanBeKilled)
                    {
                        this.overHpHurt += num2;
                        this.Hp = 1L;
                    }
                    if (this.IsHasBuff(this.GetNotDeadBuffEntry()))
                    {
                        this.Hp = 1L;
                    }
                }
                this.LogHpChange(-dmg);
                if (this.IsDead)
                {
                    this.BreakSkill(true, true);
                    if (!this.IsSummonActor)
                    {
                        this.m_buffList.ForEach(obj => obj.OnDeadEvent(this.Id));
                    }
                    this.RemoveAllBuffForce();
                    BattleSecurityManager.Instance.SetFigherDead(this.ServerIdx);
                }
            }
        }

        public void DecideFate(Fragment frag)
        {
            this.m_Fate.Add(frag);
        }

        public void DispatchFateEvent(Intent context)
        {
            <DispatchFateEvent>c__AnonStoreyCC ycc = new <DispatchFateEvent>c__AnonStoreyCC {
                context = context
            };
            this.m_Fate.ForEach(new Action<Fragment>(ycc.<>m__21));
        }

        public AiActor FindTarget(int skillEntry, AiActor lastTarget, List<AiActor> oldTargets)
        {
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(skillEntry);
            if (_config == null)
            {
                return null;
            }
            return this.FindTarget((Battle.EnemyType) _config.is_target_enemy, (TargetType) _config.target_type, (TargetRangeType) _config.range_type, _config.range, lastTarget, oldTargets);
        }

        private AiActor FindTarget(Battle.EnemyType isTargetEnemy, TargetType targetType, TargetRangeType rangeType, float range, AiActor lastTarget, List<AiActor> oldTargets)
        {
            List<AiActor> deadActorList;
            if (this.IsBeSeduced)
            {
                if (isTargetEnemy == Battle.EnemyType.SelfTeam)
                {
                    isTargetEnemy = Battle.EnemyType.OtherTeam;
                }
                else if (isTargetEnemy == Battle.EnemyType.OtherTeam)
                {
                    isTargetEnemy = Battle.EnemyType.SelfTeam;
                }
            }
            List<AiActor> targetList = new List<AiActor>();
            if (isTargetEnemy == Battle.EnemyType.SelfTeam)
            {
                targetList = base.SelfTeam.AliveActorList;
            }
            else if (isTargetEnemy == Battle.EnemyType.OtherTeam)
            {
                targetList = base.EnemyTeam.AliveActorList;
            }
            else if (isTargetEnemy == Battle.EnemyType.AnyTeam)
            {
                foreach (AiActor actor in base.SelfTeam.AliveActorList)
                {
                    targetList.Add(actor);
                }
                foreach (AiActor actor2 in base.EnemyTeam.AliveActorList)
                {
                    targetList.Add(actor2);
                }
            }
            switch (targetType)
            {
                case TargetType.Self:
                    return this;

                case TargetType.Nearest:
                {
                    AiActor actor3 = (rangeType != TargetRangeType.MustNew) ? this : lastTarget;
                    if (actor3 == null)
                    {
                        actor3 = this;
                    }
                    return AiUtil.FindNearestTarget(actor3, targetList, base.HeadDirection, oldTargets);
                }
                case TargetType.Farthest:
                    return AiUtil.FindFarthestTarget(this, targetList, base.HeadDirection);

                case TargetType.Dead:
                    deadActorList = new List<AiActor>();
                    if (isTargetEnemy != Battle.EnemyType.SelfTeam)
                    {
                        if (isTargetEnemy == Battle.EnemyType.OtherTeam)
                        {
                            deadActorList = base.EnemyTeam.DeadActorList;
                        }
                        else if (isTargetEnemy == Battle.EnemyType.AnyTeam)
                        {
                            foreach (AiActor actor5 in base.SelfTeam.DeadActorList)
                            {
                                deadActorList.Add(actor5);
                            }
                            foreach (AiActor actor6 in base.EnemyTeam.DeadActorList)
                            {
                                deadActorList.Add(actor6);
                            }
                        }
                        break;
                    }
                    deadActorList = base.SelfTeam.DeadActorList;
                    break;

                case TargetType.Random:
                    return AiUtil.FindRandomTarget(this, targetList, base.HeadDirection, range);

                case TargetType.MinHp:
                    return AiUtil.FindMinHpTarget(targetList, lastTarget);

                case TargetType.MinHpValue:
                {
                    AiActor actor7 = null;
                    if (rangeType == TargetRangeType.MustNew)
                    {
                        if (lastTarget == null)
                        {
                            actor7 = this;
                        }
                        else
                        {
                            actor7 = lastTarget;
                        }
                    }
                    return AiUtil.FindMinHpValueTarget(targetList, actor7);
                }
                case TargetType.MaxEnergyValue:
                {
                    AiActor actor8 = null;
                    if (rangeType == TargetRangeType.MustNew)
                    {
                        if (lastTarget == null)
                        {
                            actor8 = this;
                        }
                        else
                        {
                            actor8 = lastTarget;
                        }
                    }
                    return AiUtil.FindMaxEnergyValueTarget(targetList, actor8);
                }
                case TargetType.MaxEnergyButNotFull:
                {
                    AiActor actor9 = null;
                    if (rangeType == TargetRangeType.MustNew)
                    {
                        if (lastTarget != null)
                        {
                            actor9 = lastTarget;
                        }
                        else
                        {
                            actor9 = this;
                        }
                    }
                    return AiUtil.FindMaxEnergyNotFullTarget(targetList, actor9);
                }
                case TargetType.NearestCanBeSame:
                {
                    AiActor actor4 = (rangeType != TargetRangeType.MustNew) ? this : lastTarget;
                    if (actor4 == null)
                    {
                        actor4 = this;
                    }
                    return AiUtil.FindNearestTarget(actor4, targetList, base.HeadDirection, null);
                }
                default:
                    return null;
            }
            return ((deadActorList.Count <= 0) ? null : deadActorList[0]);
        }

        public void ForsakeFateFragment(Fragment frag)
        {
            this.m_Fate.Remove(frag);
        }

        private float GetAttributeEffect(SkillLogicEffectType type, SkillLogicEffectOperateType operateType)
        {
            float num = base.SelfTeam.GetSelfAttributeEffect(type, operateType) + base.EnemyTeam.GetEnemyTeamAttributeEffect(type, operateType);
            foreach (AiBuff buff in this.m_buffList)
            {
                if (buff.IsPlayerBuff())
                {
                    num += buff.GetAttributeEffect(type, operateType);
                }
            }
            return num;
        }

        public int GetBuffCount(int buffEntry)
        {
            int num = 0;
            foreach (AiBuff buff in this.m_buffList)
            {
                if (buff.Entry == buffEntry)
                {
                    num++;
                }
            }
            return num;
        }

        public List<int> GetBuffEntrys()
        {
            <GetBuffEntrys>c__AnonStoreyC8 yc = new <GetBuffEntrys>c__AnonStoreyC8 {
                buffEntrys = new List<int>()
            };
            this.m_buffList.ForEach(new Action<AiBuff>(yc.<>m__1D));
            return yc.buffEntrys;
        }

        public List<AiBuff> GetBuffs()
        {
            return this.m_buffList;
        }

        public int GetBuffStateCount(StateBuffType buffState)
        {
            int num = 0;
            foreach (AiBuff buff in this.m_buffList)
            {
                if (buff.State == buffState)
                {
                    num++;
                }
            }
            return num;
        }

        private int GetCirtDmgLevelAddRate()
        {
            float num = (int) this.AttributeEffect_2(0f, SkillLogicEffectType.CritDmgLevel);
            float num2 = num / (num + (this.Level * 90f));
            return (int) (num2 * AiDef.ONE_HUNDRED_PCT_VALUE);
        }

        private int GetCirtLevelAddRate()
        {
            float num = (int) this.AttributeEffect_2(0f, SkillLogicEffectType.CRILevel);
            float num2 = num / ((num + (this.Level * 70f)) + 75f);
            return (int) (num2 * AiDef.ONE_HUNDRED_PCT_VALUE);
        }

        private int GetDodLevelAddRate()
        {
            float f = this.AttributeEffect_2(0f, SkillLogicEffectType.DodLevel);
            float num2 = f / ((Mathf.Abs(f) + (this.Level * 300f)) + 80f);
            return (int) (num2 * AiDef.ONE_HUNDRED_PCT_VALUE);
        }

        public static AiActor GetEmptyAiactor()
        {
            return new AiActor(null, null, false, -1, false);
        }

        private int GetHitLevelAddRate()
        {
            float f = this.AttributeEffect_2(0f, SkillLogicEffectType.HitLevel);
            float num2 = f / ((Mathf.Abs(f) + (this.Level * 90f)) + 70f);
            return (int) (num2 * AiDef.ONE_HUNDRED_PCT_VALUE);
        }

        public int GetHurtHpShareBuffEntry()
        {
            foreach (AiBuff buff in this.GetBuffs())
            {
                if (buff.State == StateBuffType.HpShare)
                {
                    return buff.Entry;
                }
            }
            return -1;
        }

        public int GetNotDeadBuffEntry()
        {
            foreach (AiBuff buff in this.GetBuffs())
            {
                if (buff.State == StateBuffType.NotDead)
                {
                    return buff.Entry;
                }
            }
            return -1;
        }

        public int GetSkillLevel(int skillEntry)
        {
            <GetSkillLevel>c__AnonStoreyD2 yd = new <GetSkillLevel>c__AnonStoreyD2 {
                skillEntry = skillEntry
            };
            SkillLvInfoTss tss = this.m_baseData.skillLvList.Find(new Predicate<SkillLvInfoTss>(yd.<>m__28));
            if (tss != null)
            {
                return (int) tss.level;
            }
            return 1;
        }

        internal void IncHp(int heal)
        {
            long hp = (long) this.Hp;
            if ((this.Hp + heal) > this.MaxHp)
            {
                this.Hp = this.MaxHp;
            }
            else
            {
                this.Hp += heal;
            }
            this.LogHpChange(heal);
        }

        private void Init(bool isRevived = false)
        {
            if (this.m_baseData != null)
            {
                this.Hp = this.m_baseData.curHp;
                this.MaxHp = this.MaxHpInit;
                this.MoveSpeed = this.MoveSpeedInit;
                this.m_energy = this.m_baseData.energy;
                this.ToDeadTime = -1f;
                if (BattleSceneStarter.G_isTestEnable)
                {
                    this.SetEnergy(AiDef.MAX_ENERGY, HitEnergyType.None);
                }
                if (!isRevived)
                {
                    foreach (int num in this.m_baseData.normalSkills)
                    {
                        this.InitSkillSeldCD(num);
                    }
                    this.InitSkillSeldCD((int) this.m_baseData.activeSkill);
                }
                this.OnBuffChanged();
            }
        }

        private void InitSkillSeldCD(int skillEntry)
        {
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(skillEntry);
            if ((_config != null) && (_config.init_cd > 0f))
            {
                SkillSelfCDInfo item = new SkillSelfCDInfo {
                    skillEntry = skillEntry,
                    CDBeLeft = _config.init_cd
                };
                this.selfCDOfskills.Add(item);
            }
        }

        private bool IsBlockedMeleeActor()
        {
            if (!this.IsMelee)
            {
                return false;
            }
            int num = 0;
            foreach (AiActor actor in base.SelfTeam.AliveActorList)
            {
                if ((this.IsMelee && (actor != this)) && this.IsInFront(actor))
                {
                    num++;
                }
            }
            return (num >= AiDef.MAX_ACTOR_IN_ROW);
        }

        private bool IsCanAddBuff(int entry)
        {
            if (this.IsDead)
            {
                return false;
            }
            if (this.IsHasState(StateBuffType.DefendBadState))
            {
                bool flag2 = false;
                switch (((StateBuffType) ConfigMgr.getInstance().getByEntry<buff_config>(entry).type))
                {
                    case StateBuffType.Cloaking:
                    case StateBuffType.Sheep:
                    case StateBuffType.Daze:
                    case StateBuffType.Silent:
                    case StateBuffType.Fear:
                    case StateBuffType.GoAway:
                    case StateBuffType.AttackAway:
                    case StateBuffType.Freez:
                    case StateBuffType.Slow:
                    case StateBuffType.Seduce:
                    case StateBuffType.Sleep:
                    case StateBuffType.Freez:
                    case StateBuffType.BackToHole:
                        flag2 = true;
                        break;
                }
                if (flag2)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsCanBeAttackBySkillType(SkillHurtType hurtType)
        {
            <IsCanBeAttackBySkillType>c__AnonStoreyCF ycf = new <IsCanBeAttackBySkillType>c__AnonStoreyCF {
                hurtType = hurtType
            };
            return this.m_buffList.All<AiBuff>(new Func<AiBuff, bool>(ycf.<>m__25));
        }

        public bool IsCanUseActiveSkill()
        {
            if (this.IsDead)
            {
                return false;
            }
            return ((this.IsEnergyEnough && this.IsSkillCDOK((int) this.m_baseData.activeSkill)) && (this.IsSkillCanCast((int) this.m_baseData.activeSkill) && this.IsSkillInRange((int) this.m_baseData.activeSkill)));
        }

        public bool IsHasBuff(int buffEntry)
        {
            <IsHasBuff>c__AnonStoreyC9 yc = new <IsHasBuff>c__AnonStoreyC9 {
                buffEntry = buffEntry
            };
            return (this.m_buffList.Find(new Predicate<AiBuff>(yc.<>m__1E)) != null);
        }

        private bool IsHasState(StateBuffType type)
        {
            foreach (AiBuff buff in this.m_buffList)
            {
                if (buff.State == type)
                {
                    return true;
                }
            }
            return base.SelfTeam.HasStateInTeamBuff(StateBuffType.DefendBadState);
        }

        public bool IsHpLow()
        {
            if (this.IsDead)
            {
                return false;
            }
            return (((float) this.Hp) < (((float) this.MaxHp) * 0.25f));
        }

        private bool IsInFront(IActor target)
        {
            bool flag = base.PosInHorizontal < target.PosInHorizontal;
            return (this.IsAttacker == flag);
        }

        public bool IsInRange(AiActor target)
        {
            return this.IsInRange(target, this.Range);
        }

        private bool IsInRange(AiActor target, float range)
        {
            return (AiUtil.CalDistanceByX(this, target, base.m_aiManager.PathDirection) <= ((target.Radius + range) + 0.1f));
        }

        private bool IsSkillCanCast(int skillEntry)
        {
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(skillEntry);
            if (_config == null)
            {
                return false;
            }
            if (_config.type == 0)
            {
                if (!this.IsCanCastNormalAttack)
                {
                    return false;
                }
            }
            else if (_config.type == 1)
            {
                if (!this.IsCanCastMagicSkill)
                {
                    return false;
                }
            }
            else if (_config.type == 2)
            {
                if (!this.IsCanCastPhysicsSkill)
                {
                    return false;
                }
            }
            else if (!this.IsCanCastSkill)
            {
                return false;
            }
            return true;
        }

        private bool IsSkillCDOK(int skillEntry)
        {
            <IsSkillCDOK>c__AnonStoreyD0 yd = new <IsSkillCDOK>c__AnonStoreyD0 {
                skillEntry = skillEntry
            };
            SkillSelfCDInfo info = this.selfCDOfskills.Find(new Predicate<SkillSelfCDInfo>(yd.<>m__26));
            if (info != null)
            {
                return (info.CDBeLeft <= 0f);
            }
            return true;
        }

        public bool IsSkillEnergyFull()
        {
            if (this.IsDead)
            {
                return false;
            }
            return this.IsEnergyEnough;
        }

        private bool IsSkillInRange(int skillEntry)
        {
            AiActor target = this.FindTarget(skillEntry, null, null);
            if (target == null)
            {
                return false;
            }
            return this.IsInRange(target, AiUtil.GetSkillRange(skillEntry, this.RangeScale));
        }

        public void KillAttack(bool b)
        {
            this.m_HasDefender = b;
        }

        private void LogHpChange(int hpData)
        {
            if (AiSkill.G_IsShowHpLog)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Hp Change Log: Time:" + BattleState.GetInstance().CurGame.battleGameData.GetBattletime());
                object[] args = new object[] { this.Id, this.ServerIdx, hpData, this.Hp };
                builder.AppendFormat("ActorID:{0}, ActorServerID:{1}, ChangeValue:{2}, HP:{3}", args);
                Debug.Log(builder.ToString());
            }
        }

        public void MoveTo(Vector3 destPos, AiActor target = null)
        {
            int targetId = -1;
            float targetRadius = 0f;
            if (target != null)
            {
                targetId = target.Id;
                if (!this.IsRange)
                {
                    targetRadius = target.Radius;
                }
            }
            this.State = AiState.Move;
            base.m_aiManager.NoticeMoveActorToPos(this.Id, destPos, targetRadius, targetId);
        }

        private void MoveToActor(AiActor target)
        {
            if (((target != null) && !target.IsHasState(StateBuffType.AttackAway)) && this.IsCanMove)
            {
                this.CurTarget = target;
                if (this.IsInRange(target))
                {
                    this.StopMove(true);
                }
                else
                {
                    Vector3 vector = target.Pos - this.Pos;
                    Vector3 vector2 = Vector3.Project(vector, base.m_aiManager.PathDirection);
                    vector = this.Pos + ((Vector3) (vector2.normalized * Mathf.Max((float) 0f, (float) (((vector2.magnitude - target.Radius) - this.Range) + 0.1f))));
                    if (Vector3.Distance(vector, this.Pos) >= AiDef.MAX_IGNORE_POS_OFFSET)
                    {
                        this.MoveTo(vector, target);
                    }
                }
            }
        }

        private void NormalSkillIdxMoveNext()
        {
            this.m_skillIdxList++;
            this.m_skillIdxList = this.m_skillIdxList % this.m_baseData.normalSkills.Count;
        }

        private void OnActiveSkillCast(int skillEntry)
        {
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(skillEntry);
            this.m_skillCdBeLeft = _config.cd;
            this.UpdateSelfCDOnCast(skillEntry);
            this.SetEnergyPercent((int) this.m_baseData.energyRemain, HitEnergyType.UseActiveSkill);
        }

        public void OnActorAttackAwayFinish()
        {
            this.RemoveStateBuff(StateBuffType.AttackAway);
        }

        private void OnAddBuffEvent(int buffEntry, int buffState)
        {
            Battle.CombatEvent event2 = new Battle.CombatEvent {
                type = Battle.CombatEventType.AddBuff,
                actorID = this.Id,
                changeValue = buffEntry,
                relatedActorID = buffState
            };
            this.OnEvent(event2);
        }

        public void OnBuffChanged()
        {
            this._fixedAttackSpeed = this.GetAndComputeAttackSpeed;
            this._fixedCritRate = this.GetAndComputeCritRate;
            this._fixedCritDmg = this.GetAndComputeCritDmg;
            this._fixedTenacity = this.GetAndComputeTenacity;
            this._fixedHitRate = this.GetAndComputeHitRate;
            this._fixedDodRate = this.GetAndComputeDodRate;
            this._fixedAttack = this.GetAndComputeAttack;
            this._fixedPhysicsDefender = this.GetAndComputePhysicsDefender;
            this._fixedPhysicsPierce = this.GetAndComputePhysicsPierce;
            this._fixedSpellDefence = this.GetAndComputeSpellDefence;
            this._fixedSpellPierce = this.GetAndComputeSpellPierce;
            this._fixedPhysicsDmgReduce = this.GetAndComputePhysicsDmgReduce;
            this._fixedSpellDmgReduce = this.GetAndComputeSpellDmgReduce;
            this._fixedPhysicsDmgIncrease = this.GetAndComputePhysicsDmgIncrease;
            this._fixedSpellDmgIncrease = this.GetAndComputeSpellDmgIncrease;
            this._fixedHealMod = this.GetAndComputeHealMod;
            this._fixedBeHealMod = this.GetAndComputeBeHealMod;
            this._fixedEnergyRecoverOnAttack = this.GetAndComputeEnergyRecoverOnAttack;
            this._fixedPhysicsAttack = this.GetAndComputePhysicsAttack;
            this._fixedMagicAttack = this.GetAndComputeMagicAttack;
            this._fixedSuckRate = this.GetAndComputeSuckRate;
        }

        private void OnDeadEvent(Battle.CombatEvent _event)
        {
            if (((_event.type == Battle.CombatEventType.Dead) && (_event.relatedActorID == this.Id)) && !this.IsSummonActor)
            {
                variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
                AiActor actorById = base.m_aiManager.GetActorById(_event.actorID);
                bool flag = true;
                if (actorById != null)
                {
                    flag = !actorById.IsSummonActor;
                }
                if ((_config != null) && flag)
                {
                    this.OnEnergyChange(_config.inc_energy_on_killtarget, HitEnergyType.KillEnemy);
                }
            }
        }

        public void OnEnergyChange(int changeValue, HitEnergyType changeType)
        {
            if (this.IsEnergyCanBeAdded || (changeValue <= 0))
            {
                if ((base.m_aiManager != null) && (changeType != HitEnergyType.ByAIFate))
                {
                    base.m_aiManager.OnEnergyChangeEvent(changeValue, this.Id);
                }
                BattleSecurityManager.Instance.AddEnergyChangeData(this.ServerIdx, changeType, changeValue);
                int energy = (int) this.m_energy;
                this.m_energy += changeValue;
                this.m_energy = Mathf.Clamp((int) this.m_energy, 0, AiDef.MAX_ENERGY);
                if (base.m_aiManager.EvetntOnSkillEffectResult != null)
                {
                    SkillEffectResult result = new SkillEffectResult(SkillHitType.Hit, 0L) {
                        changeValue = changeValue,
                        targetID = this.Id,
                        value = this.Energy,
                        effectType = SubSkillType.AddEnergy,
                        energyChangeType = changeType
                    };
                    base.m_aiManager.EvetntOnSkillEffectResult(result);
                }
            }
        }

        public void OnEvent(Battle.CombatEvent _event)
        {
            <OnEvent>c__AnonStoreyCD ycd = new <OnEvent>c__AnonStoreyCD {
                _event = _event
            };
            this.OnDeadEvent(ycd._event);
            List<AiBuff> list = new List<AiBuff>();
            foreach (AiBuff buff in this.m_buffList)
            {
                if (buff != null)
                {
                    list.Add(buff);
                }
            }
            list.ForEach(new Action<AiBuff>(ycd.<>m__23));
        }

        private void OnNormalSkillCast(int skillEntry)
        {
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(skillEntry);
            this.m_skillCdBeLeft = _config.cd;
            this.UpdateSelfCDOnCast(skillEntry);
            this.NormalSkillIdxMoveNext();
        }

        public void OnOtherHurt(int hpChange)
        {
            SkillEffectResult result = new SkillEffectResult(SkillHitType.Hit, (long) this.Hp) {
                effectType = SubSkillType.Hurt,
                targetID = this.Id
            };
            int dmg = this.UseHurtReduce(hpChange);
            result.hurtReduce = hpChange - dmg;
            if (dmg == 0)
            {
                result.hitType = SkillHitType.Absorb;
            }
            else
            {
                this.DecHp(dmg);
                result.changeValue = -dmg;
                result.value = (long) this.Hp;
            }
            if (base.m_aiManager.EvetntOnSkillEffectResult != null)
            {
                base.m_aiManager.EvetntOnSkillEffectResult(result);
            }
            BattleSecurityManager.Instance.AddHpChangeData(this.ServerIdx, ChangeHpType.HpType_ByHpShare, -dmg);
        }

        private void OnSelfDead()
        {
            if (base.m_aiManager.EventOnActorDead != null)
            {
                base.m_aiManager.EventOnActorDead(this.Id);
            }
        }

        public override void OnSkillEffectFinish(int skillUniId)
        {
            if ((this.CastingSkill != null) && (this.CastingSkill.SkillUniId == skillUniId))
            {
                this.CastingSkill.OnFinish();
                this.CalNextPreferAngle(AiDef.DEFAULT_STROLL_PROB);
                this.State = AiState.Idle;
                this.CastingSkill = null;
            }
        }

        public void OnSkillPrepared()
        {
            this.CastingSkill.StartCast();
        }

        public void OnSubSkillFinish(int skillID)
        {
        }

        public void OnToDeadTime()
        {
            this.ToDeadTime = 0f;
            this.Hp = 0L;
            this.OnSelfDead();
            this.RemoveAllBuffForce();
            BattleSecurityManager.Instance.SetFigherDead(this.ServerIdx);
        }

        public void OnUserCastSkill()
        {
            if (!base.m_aiManager.IsPreFight)
            {
                Debug.Log("Use SKill!!");
                if ((((this.IsEnergyEnough && this.IsSkillCDOK((int) this.m_baseData.activeSkill)) && this.IsSkillCanCast((int) this.m_baseData.activeSkill)) && this.IsSkillInRange((int) this.m_baseData.activeSkill)) && !this.IsBeSeduced)
                {
                    if (this.CastingSkill != null)
                    {
                        base.m_aiManager.NoticeBreakSkill(this.Id, this.CastingSkill.SkillUniId);
                        this.CastingSkill = null;
                    }
                    this.CastSkill((int) this.m_baseData.activeSkill, SkillCategory.Active);
                }
            }
        }

        public void RemoveAllBuffAndKeepNeedBuff()
        {
            AiDataKeepManager.GetInstance().CheckNeedKeepBuff(this);
            for (int i = 0; i < this.m_buffList.Count; i++)
            {
                AiBuff item = this.m_buffList[i];
                if (item != null)
                {
                    if (item.isCanPastToNextPhase())
                    {
                        this.m_buffList.Remove(item);
                        item.OnRemove();
                    }
                    else
                    {
                        this.RemoveBuff(item);
                    }
                }
                i--;
            }
        }

        public void RemoveAllBuffForce()
        {
            for (int i = 0; i < this.m_buffList.Count; i++)
            {
                AiBuff buff = this.m_buffList[i];
                if (buff != null)
                {
                    this.RemoveBuff(buff);
                }
                i--;
            }
        }

        public void RemoveBuff(AiBuff buff)
        {
            buff.OnRemove();
            base.m_aiManager.NoticeDelBuff(this.Id, buff.Entry);
            this.m_buffList.Remove(buff);
            this.CheckBuffChanged(buff);
        }

        public void RemoveBuff(int entry, int number)
        {
            for (int i = 0; i < this.m_buffList.Count; i++)
            {
                AiBuff buff = this.m_buffList[i];
                if (buff.Entry == entry)
                {
                    this.RemoveBuff(buff);
                    i--;
                    number--;
                    if (number < 0)
                    {
                        break;
                    }
                }
            }
        }

        private void RemoveStateBuff(StateBuffType state)
        {
            for (int i = 0; i < this.m_buffList.Count; i++)
            {
                AiBuff buff = this.m_buffList[i];
                if (buff.State == state)
                {
                    this.RemoveBuff(buff);
                    i--;
                }
            }
        }

        public void ResetBuffStateTime(int state)
        {
            <ResetBuffStateTime>c__AnonStoreyCB ycb = new <ResetBuffStateTime>c__AnonStoreyCB {
                state = state,
                <>f__this = this
            };
            this.m_buffList.ForEach(new Action<AiBuff>(ycb.<>m__20));
        }

        public void ResetBuffTime(int buffEntry)
        {
            <ResetBuffTime>c__AnonStoreyCA yca = new <ResetBuffTime>c__AnonStoreyCA {
                buffEntry = buffEntry,
                <>f__this = this
            };
            this.m_buffList.ForEach(new Action<AiBuff>(yca.<>m__1F));
        }

        public void Revive(long _hp)
        {
            this.Init(true);
            this.Hp = _hp;
            this.m_energy = 0;
            this.CheckHpMax();
            base.m_aiManager.NoticeRevive(this.Id, (long) this.Hp, this.Energy);
            this.ReviveFormostSkill();
            if ((this.State == AiState.Idle) && (this.m_baseData != null))
            {
                AiActor target = this.FindTarget((int) this.m_baseData.defaultSkill, null, null);
                this.MoveToActor(target);
            }
        }

        public void ReviveFormostSkill()
        {
            this.CastPrewarSkill(false);
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>((int) this.m_baseData.foremostSkill);
            if ((_config != null) && (_config.can_cast_after_dead == 0))
            {
                this.CastSkill((int) this.m_baseData.foremostSkill, SkillCategory.Foremost);
            }
        }

        public void SetBuffActiveTime(int buffEntry, float time)
        {
            foreach (AiBuff buff in this.m_buffList)
            {
                if ((buff.Entry == buffEntry) && (buff.EndTime > 0f))
                {
                    buff.EndTime += time;
                }
            }
        }

        public void SetEnergy(int energy, HitEnergyType changeType)
        {
            energy = Mathf.Clamp(energy, 0, AiDef.MAX_ENERGY);
            this.OnEnergyChange(energy - this.Energy, changeType);
        }

        public void SetEnergyPercent(int percent, HitEnergyType changeType)
        {
            int energy = (this.Energy * percent) / AiDef.ONE_HUNDRED_PCT_VALUE;
            this.SetEnergy(energy, changeType);
        }

        private void StopMove(bool needFaceTo = true)
        {
            this.State = AiState.Idle;
            if ((needFaceTo && (this.CurTarget != null)) && this.CurTarget.IsAlive)
            {
                base.m_aiManager.NoticeFaceToActor(this.Id, this.CurTarget.Id);
            }
        }

        public override void Tick()
        {
            if (this.CastingSkill != null)
            {
                this.CastingSkill.OnTick();
            }
            List<AiBuff> list = new List<AiBuff>();
            foreach (AiBuff buff in this.m_buffList)
            {
                if (buff != null)
                {
                    list.Add(buff);
                }
            }
            foreach (AiBuff buff2 in list)
            {
                buff2.OnTick();
            }
        }

        public virtual void Trace(ObjectMetaData meta)
        {
            meta.descript = "...";
            Debugger.TraceDetails<int>(meta, "id", this.Id);
            Debugger.TraceDetails<int>(meta, "level", this.Level);
            Debugger.TraceDetails<TssSdtLong>(meta, "hp", this.Hp);
            Debugger.TraceDetails<TssSdtLong>(meta, "max hp", this.MaxHp);
            Debugger.TraceDetails<TssSdtFloat>(meta, "move speed", this.MoveSpeed);
            Debugger.TraceDetails<int>(meta, "energy", this.Energy);
            Debugger.TraceDetails<bool>(meta, "alive", this.IsAlive);
            Debugger.TraceDetails<bool>(meta, "empty", this.IsEmpty);
            Debugger.TraceDetails<AiState>(meta, "state", this.State);
            Debugger.TraceDetails<bool>(meta, "visible", this.IsVisible);
            Debugger.TraceDetails<bool>(meta, "is melee", this.IsMelee);
            Debugger.TraceDetails<bool>(meta, "can cast normal attack", this.IsCanCastNormalAttack);
            Debugger.TraceDetails<bool>(meta, "can cast skill", this.IsCanCastSkill);
            Debugger.TraceDetails<bool>(meta, "can move", this.IsCanMove);
            Debugger.TraceDetails<bool>(meta, "can heart", this.IsCanBehurt);
            Debugger.TraceDetails<bool>(meta, "be seduced", this.IsBeSeduced);
            Debugger.TraceDetails<bool>(meta, "be feard", this.IsBeFeard);
            Debugger.TraceDetails<bool>(meta, "started", this.IsStarted);
            Debugger.TraceDetails<bool>(meta, "auto", this.IsAuto);
            Debugger.TraceDetails<int>(meta, "hate", this.Hate);
            Debugger.TraceDetails<Vector3>(meta, "position", this.Pos);
            Debugger.TraceDetails<float>(meta, "range", this.Range);
            Debugger.TraceDetails<float>(meta, "radius", this.Radius);
            Debugger.TraceListDetails<AiBuff>(meta, "buff", this.GetBuffs());
        }

        private bool TryCastSkill()
        {
            if (((this.IsAuto && this.IsEnergyEnough) && (this.IsSkillCDOK((int) this.m_baseData.activeSkill) && this.IsSkillCanCast((int) this.m_baseData.activeSkill))) && (this.IsSkillInRange((int) this.m_baseData.activeSkill) && !this.IsBeSeduced))
            {
                this.CastSkill((int) this.m_baseData.activeSkill, SkillCategory.Active);
                return true;
            }
            if (this.m_skillCdBeLeft <= 0f)
            {
                int skillEntry = this.m_baseData.normalSkills[this.m_skillIdxList];
                if (!this.IsSkillCDOK(skillEntry))
                {
                    skillEntry = (int) this.m_baseData.defaultSkill;
                }
                else if (!this.IsSkillCanCast(skillEntry))
                {
                    skillEntry = (int) this.m_baseData.defaultSkill;
                }
                else if (this.FindTarget(skillEntry, null, null) == null)
                {
                    skillEntry = (int) this.m_baseData.defaultSkill;
                }
                if (this.IsSkillCanCast(skillEntry) && this.IsSkillInRange(skillEntry))
                {
                    this.CastSkill(skillEntry, SkillCategory.Normal);
                    return true;
                }
            }
            return false;
        }

        public override void Update(float detlaTime)
        {
            if (this.IsAlive)
            {
                if (this.ToDeadTime > 0f)
                {
                    this.ToDeadTime -= detlaTime;
                    if (this.ToDeadTime <= 0f)
                    {
                        this.OnToDeadTime();
                        return;
                    }
                }
                if (!base.m_aiManager.IsPreFight)
                {
                    if (this.CastingSkill != null)
                    {
                        this.CastingSkill.OnUpdate();
                    }
                    List<AiBuff> list = new List<AiBuff>();
                    foreach (AiBuff buff in this.m_buffList)
                    {
                        if (buff != null)
                        {
                            list.Add(buff);
                        }
                    }
                    foreach (AiBuff buff2 in list)
                    {
                        buff2.OnUpdate();
                    }
                    this.UpdateSkillCD(detlaTime);
                    this.BuffTick(detlaTime);
                    bool flag = true;
                    while (flag)
                    {
                        flag = false;
                        if (!this.IsAlive)
                        {
                            break;
                        }
                        switch (this.State)
                        {
                            case AiState.Idle:
                                this.WhenIdle();
                                break;

                            case AiState.Move:
                                flag = this.WhenMove();
                                break;

                            case AiState.MoveToPos:
                                flag = this.WhenMoveToPos();
                                break;

                            case AiState.Casting:
                                flag = this.WhenCasting();
                                break;
                        }
                    }
                }
            }
        }

        private void UpdateSelfCDOnCast(int skillEntry)
        {
            <UpdateSelfCDOnCast>c__AnonStoreyD1 yd = new <UpdateSelfCDOnCast>c__AnonStoreyD1 {
                skillEntry = skillEntry
            };
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(yd.skillEntry);
            SkillSelfCDInfo item = this.selfCDOfskills.Find(new Predicate<SkillSelfCDInfo>(yd.<>m__27));
            if (item == null)
            {
                item = new SkillSelfCDInfo {
                    skillEntry = yd.skillEntry
                };
                this.selfCDOfskills.Add(item);
            }
            item.CDBeLeft = _config.self_cd;
        }

        private void UpdateSkillCD(float deltaTime)
        {
            <UpdateSkillCD>c__AnonStoreyCE yce = new <UpdateSkillCD>c__AnonStoreyCE();
            float fixedAttackSpeed = this.FixedAttackSpeed;
            yce.delta = fixedAttackSpeed * deltaTime;
            this.m_skillCdBeLeft -= yce.delta;
            this.selfCDOfskills.ForEach(new Action<SkillSelfCDInfo>(yce.<>m__24));
        }

        public bool UseHurtDenfender()
        {
            return this.m_HasDefender;
        }

        public int UseHurtReduce(int hurtValue)
        {
            foreach (AiBuff buff in this.m_buffList)
            {
                hurtValue = buff.UseHurtReduce(hurtValue);
                if (hurtValue <= 0)
                {
                    return 0;
                }
            }
            hurtValue = base.SelfTeam.UseHurtReduce(hurtValue);
            return hurtValue;
        }

        private bool WhenCasting()
        {
            if ((this.CurTarget == null) || !this.CurTarget.IsAlive)
            {
                this.State = AiState.Idle;
                return true;
            }
            if (this.CastingSkill == null)
            {
                this.State = AiState.Idle;
                return true;
            }
            this.TryCastSkill();
            return false;
        }

        private bool WhenIdle()
        {
            this.TryCastSkill();
            if (this.State == AiState.Idle)
            {
                AiActor target = this.FindTarget((int) this.m_baseData.defaultSkill, null, null);
                this.MoveToActor(target);
            }
            return false;
        }

        private bool WhenMove()
        {
            if ((this.CurTarget != null) && !this.CurTarget.IsAlive)
            {
                this.State = AiState.Idle;
                return true;
            }
            if ((this.CurTarget != null) && !this.TryCastSkill())
            {
                this.MoveToActor(this.CurTarget);
            }
            return false;
        }

        private bool WhenMoveToPos()
        {
            return false;
        }

        public int BaseHitRate
        {
            get
            {
                return (int) this.m_baseData.hitRate;
            }
        }

        public AiSkill CastingSkill { get; set; }

        public AiActor CurTarget { get; set; }

        public int Energy
        {
            get
            {
                return (int) this.m_energy;
            }
        }

        public List<Fragment> Fate
        {
            get
            {
                return this.m_Fate;
            }
        }

        public int FixedAttack
        {
            get
            {
                return (int) this._fixedAttack;
            }
        }

        public float FixedAttackSpeed
        {
            get
            {
                return (float) this._fixedAttackSpeed;
            }
        }

        public int FixedBeHealMod
        {
            get
            {
                return (int) this._fixedBeHealMod;
            }
        }

        public int FixedCritDmg
        {
            get
            {
                return (int) this._fixedCritDmg;
            }
        }

        public int FixedCritRate
        {
            get
            {
                return (int) this._fixedCritRate;
            }
        }

        public int FixedDodRate
        {
            get
            {
                return (int) this._fixedDodRate;
            }
        }

        public int FixedEnergyRecoverOnAttack
        {
            get
            {
                return (int) this._fixedEnergyRecoverOnAttack;
            }
        }

        public int FixedHealMod
        {
            get
            {
                return (int) this._fixedHealMod;
            }
        }

        public int FixedHitRate
        {
            get
            {
                return (int) this._fixedHitRate;
            }
        }

        public int FixedMagicAttack
        {
            get
            {
                return (int) this._fixedMagicAttack;
            }
        }

        public int FixedPhysicsAttack
        {
            get
            {
                return (int) this._fixedPhysicsAttack;
            }
        }

        public int FixedPhysicsDefender
        {
            get
            {
                return (int) this._fixedPhysicsDefender;
            }
        }

        public int FixedPhysicsDmgIncrease
        {
            get
            {
                return (int) this._fixedPhysicsDmgIncrease;
            }
        }

        public int FixedPhysicsDmgReduce
        {
            get
            {
                return (int) this._fixedPhysicsDmgReduce;
            }
        }

        public int FixedPhysicsPierce
        {
            get
            {
                return (int) this._fixedPhysicsPierce;
            }
        }

        public int FixedSpellDefence
        {
            get
            {
                return (int) this._fixedSpellDefence;
            }
        }

        public int FixedSpellDmgIncrease
        {
            get
            {
                return (int) this._fixedSpellDmgIncrease;
            }
        }

        public int FixedSpellDmgReduce
        {
            get
            {
                return (int) this._fixedSpellDmgReduce;
            }
        }

        public int FixedSpellPierce
        {
            get
            {
                return (int) this._fixedSpellPierce;
            }
        }

        public float FixedSuckRate
        {
            get
            {
                return (float) this._fixedSuckRate;
            }
        }

        public int FixedTenacity
        {
            get
            {
                return (int) this._fixedTenacity;
            }
        }

        private int GetAndComputeAttack
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.attack, SkillLogicEffectType.Attack);
            }
        }

        private float GetAndComputeAttackSpeed
        {
            get
            {
                return this.AttributeEffect((float) this.m_baseData.attackSpeed, SkillLogicEffectType.SkillSpeed);
            }
        }

        private int GetAndComputeBeHealMod
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.beHealMod, SkillLogicEffectType.BeHealMod);
            }
        }

        private int GetAndComputeCritDmg
        {
            get
            {
                return (((int) this.AttributeEffect((float) this.m_baseData.critDmg, SkillLogicEffectType.CritDmg)) + this.GetCirtDmgLevelAddRate());
            }
        }

        private int GetAndComputeCritRate
        {
            get
            {
                return Mathf.Max(0, ((int) this.AttributeEffect((float) this.m_baseData.critRate, SkillLogicEffectType.CritRate)) + this.GetCirtLevelAddRate());
            }
        }

        private int GetAndComputeDodRate
        {
            get
            {
                return Mathf.Max(0, ((int) this.AttributeEffect((float) this.m_baseData.dodgeRate, SkillLogicEffectType.DodRate)) + this.GetDodLevelAddRate());
            }
        }

        private int GetAndComputeEnergyRecoverOnAttack
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.energyRecoverOnAttack, SkillLogicEffectType.EnergyRecoverOnAttack);
            }
        }

        private int GetAndComputeHealMod
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.healMod, SkillLogicEffectType.HealMod);
            }
        }

        private int GetAndComputeHitRate
        {
            get
            {
                return Mathf.Max(0, ((int) this.AttributeEffect((float) AiDef.ONE_HUNDRED_PCT_VALUE, SkillLogicEffectType.HitRate)) + this.GetHitLevelAddRate());
            }
        }

        private int GetAndComputeMagicAttack
        {
            get
            {
                int num = this.FixedAttack - this.m_baseData.attack;
                return (num + ((int) this.AttributeEffect((float) this.m_baseData.attack, SkillLogicEffectType.MagicAttack)));
            }
        }

        private int GetAndComputePhysicsAttack
        {
            get
            {
                int num = this.FixedAttack - this.m_baseData.attack;
                return (num + ((int) this.AttributeEffect((float) this.m_baseData.attack, SkillLogicEffectType.PhysicsAttack)));
            }
        }

        private int GetAndComputePhysicsDefender
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.physicsDefence, SkillLogicEffectType.PhysicsDefence);
            }
        }

        private int GetAndComputePhysicsDmgIncrease
        {
            get
            {
                return (int) this.AttributeEffect((float) AiDef.ONE_HUNDRED_PCT_VALUE, SkillLogicEffectType.PhysicsDmgIncrease);
            }
        }

        private int GetAndComputePhysicsDmgReduce
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.physicsDmgReduce, SkillLogicEffectType.PhysicsDmgReduce);
            }
        }

        private int GetAndComputePhysicsPierce
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.physicsPierce, SkillLogicEffectType.PhysicsPierce);
            }
        }

        private int GetAndComputeSpellDefence
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.spellDefence, SkillLogicEffectType.SpellDefence);
            }
        }

        private int GetAndComputeSpellDmgIncrease
        {
            get
            {
                return (int) this.AttributeEffect((float) AiDef.ONE_HUNDRED_PCT_VALUE, SkillLogicEffectType.SpellDmgIncrease);
            }
        }

        private int GetAndComputeSpellDmgReduce
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.spellDmgReduce, SkillLogicEffectType.SpellDmgReduce);
            }
        }

        private int GetAndComputeSpellPierce
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.spellPierce, SkillLogicEffectType.SpellPierce);
            }
        }

        private float GetAndComputeSuckRate
        {
            get
            {
                float num = this.AttributeEffect((float) this.m_baseData.suckLv, SkillLogicEffectType.SuckLv);
                return (num / (num + (this.Level * 0x5d)));
            }
        }

        private int GetAndComputeTenacity
        {
            get
            {
                return (int) this.AttributeEffect((float) this.m_baseData.tenacity, SkillLogicEffectType.Tenacity);
            }
        }

        public int Hate
        {
            get
            {
                if (this.IsBeFeard)
                {
                    return 0;
                }
                return (int) this.m_baseData.hate;
            }
        }

        public TssSdtLong Hp { get; set; }

        public override int Id
        {
            get
            {
                int num = !this.IsAttacker ? AiDef.MAX_ACTOR_EACH_SIDE : 0;
                return (num + this.indexOfList);
            }
        }

        public int indexOfList { get; set; }

        public bool IsAlive
        {
            get
            {
                return (!this.IsEmpty && (this.Hp > 0L));
            }
        }

        public bool IsAuto
        {
            get
            {
                return this.m_isAuto;
            }
            set
            {
                this.m_isAuto = value;
            }
        }

        public bool IsBattleObj { get; set; }

        public bool IsBeFeard
        {
            get
            {
                if (<>f__am$cache3D == null)
                {
                    <>f__am$cache3D = obj => obj.IsBeFeard;
                }
                return this.m_buffList.Any<AiBuff>(<>f__am$cache3D);
            }
        }

        public bool IsBeSeduced
        {
            get
            {
                if (<>f__am$cache3C == null)
                {
                    <>f__am$cache3C = obj => obj.IsBeSeduced;
                }
                return this.m_buffList.Any<AiBuff>(<>f__am$cache3C);
            }
        }

        public bool IsCanBeHealed
        {
            get
            {
                if (<>f__am$cache32 == null)
                {
                    <>f__am$cache32 = buff => buff.IsCanBeHealed;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache32);
            }
        }

        public bool IsCanBehurt
        {
            get
            {
                if (<>f__am$cache3B == null)
                {
                    <>f__am$cache3B = obj => obj.IsCanBehurt;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache3B);
            }
        }

        public bool IsCanBeHurtByMagic
        {
            get
            {
                if (<>f__am$cache3E == null)
                {
                    <>f__am$cache3E = obj => obj.IsCanBeHurtByMagic;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache3E);
            }
        }

        public bool IsCanBeHurtByPhysics
        {
            get
            {
                if (<>f__am$cache3F == null)
                {
                    <>f__am$cache3F = obj => obj.IsCanBeHurtByPhysics;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache3F);
            }
        }

        public bool IsCanBeTarget
        {
            get
            {
                if (<>f__am$cache34 == null)
                {
                    <>f__am$cache34 = buff => buff.IsCanBeTarget;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache34);
            }
        }

        public bool IsCanCastMagicSkill
        {
            get
            {
                if (<>f__am$cache39 == null)
                {
                    <>f__am$cache39 = buff => buff.IsCanCastMagicSkill;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache39);
            }
        }

        public bool IsCanCastNormalAttack
        {
            get
            {
                if (<>f__am$cache36 == null)
                {
                    <>f__am$cache36 = buff => buff.IsCanCastNormalAttack;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache36);
            }
        }

        public bool IsCanCastPhysicsSkill
        {
            get
            {
                if (<>f__am$cache38 == null)
                {
                    <>f__am$cache38 = buff => buff.IsCanCastPhysicsSkill;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache38);
            }
        }

        public bool IsCanCastSkill
        {
            get
            {
                if (<>f__am$cache37 == null)
                {
                    <>f__am$cache37 = buff => buff.IsCanCastSkill;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache37);
            }
        }

        public bool IsCanMove
        {
            get
            {
                if (this.m_baseData.canMove)
                {
                }
                return ((<>f__am$cache3A == null) && this.m_buffList.All<AiBuff>(<>f__am$cache3A));
            }
        }

        public bool IsCasting
        {
            get
            {
                return ((this.m_state == AiState.Casting) && (this.CastingSkill != null));
            }
        }

        public bool IsDead
        {
            get
            {
                return (!this.IsEmpty && (this.Hp == 0L));
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this.m_baseData == null);
            }
        }

        public bool IsEnergyCanBeAdded
        {
            get
            {
                if (<>f__am$cache33 == null)
                {
                    <>f__am$cache33 = buff => buff.IsEnergyCanBeAdded;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache33);
            }
        }

        private bool IsEnergyEnough
        {
            get
            {
                return (this.Energy >= AiDef.COST_ENERGY_FOR_ACTIVE_SKILL);
            }
        }

        public bool IsHpFull
        {
            get
            {
                return (this.Hp >= this.MaxHp);
            }
        }

        public bool IsMelee
        {
            get
            {
                return (AiUtil.GetSkillRange((int) this.m_baseData.defaultSkill, this.RangeScale) <= 0f);
            }
        }

        public bool IsRange
        {
            get
            {
                return (this.Range > 0f);
            }
        }

        public bool IsStarted
        {
            get
            {
                return (this.Time > (this.indexOfList * AiDef.START_BATTLE_DELAY_FACTOR));
            }
        }

        public bool IsVisible
        {
            get
            {
                if (<>f__am$cache35 == null)
                {
                    <>f__am$cache35 = buff => buff.IsVisible;
                }
                return this.m_buffList.All<AiBuff>(<>f__am$cache35);
            }
        }

        public int Level
        {
            get
            {
                if (this.IsSummonActor)
                {
                    return (int) this.SummonSkillLevel;
                }
                return (int) this.m_baseData.level;
            }
        }

        public TssSdtLong MaxHp { get; set; }

        public long MaxHpInit
        {
            get
            {
                return (long) this.m_baseData.maxHp;
            }
        }

        public TssSdtFloat MoveSpeed { get; set; }

        public float MoveSpeedInit
        {
            get
            {
                return BattleGlobal.DefaultMoveSpeed;
            }
        }

        public float PKHpScale
        {
            get
            {
                return this.pkHPScale;
            }
        }

        public override Vector3 Pos
        {
            get
            {
                return base.m_aiManager.BattleCom.GetActorPos(this.Id);
            }
        }

        public float PreferAngle { get; private set; }

        public float Radius
        {
            get
            {
                return (float) this.m_baseData.radius;
            }
        }

        public float Range
        {
            get
            {
                return AiUtil.GetSkillRange((int) this.m_baseData.defaultSkill, this.RangeScale);
            }
        }

        public AiState State
        {
            get
            {
                return this.m_state;
            }
            set
            {
                if (this.m_state != value)
                {
                    if (value == AiState.Idle)
                    {
                        this.CalNextIdleUntil();
                    }
                    if (this.m_state == AiState.Move)
                    {
                        base.m_aiManager.NoticeStopActorMove(this.Id);
                    }
                    this.m_state = value;
                }
            }
        }

        public float Time
        {
            get
            {
                return BattleState.GetInstance().GetBattletime();
            }
        }

        public TssSdtFloat ToDeadTime { get; set; }

        [CompilerGenerated]
        private sealed class <DispatchFateEvent>c__AnonStoreyCC
        {
            internal Intent context;

            internal void <>m__21(Fragment e)
            {
                e.Dispatch(this.context);
            }
        }

        [CompilerGenerated]
        private sealed class <GetBuffEntrys>c__AnonStoreyC8
        {
            internal List<int> buffEntrys;

            internal void <>m__1D(AiBuff obj)
            {
                this.buffEntrys.Add(obj.Entry);
            }
        }

        [CompilerGenerated]
        private sealed class <GetSkillLevel>c__AnonStoreyD2
        {
            internal int skillEntry;

            internal bool <>m__28(SkillLvInfoTss obj)
            {
                return (obj.entry == this.skillEntry);
            }
        }

        [CompilerGenerated]
        private sealed class <IsCanBeAttackBySkillType>c__AnonStoreyCF
        {
            internal SkillHurtType hurtType;

            internal bool <>m__25(AiBuff obj)
            {
                return obj.GetIsCanBeAttackBySkillType(this.hurtType);
            }
        }

        [CompilerGenerated]
        private sealed class <IsHasBuff>c__AnonStoreyC9
        {
            internal int buffEntry;

            internal bool <>m__1E(AiBuff obj)
            {
                return (obj.Entry == this.buffEntry);
            }
        }

        [CompilerGenerated]
        private sealed class <IsSkillCDOK>c__AnonStoreyD0
        {
            internal int skillEntry;

            internal bool <>m__26(SkillSelfCDInfo obj)
            {
                return (obj.skillEntry == this.skillEntry);
            }
        }

        [CompilerGenerated]
        private sealed class <OnEvent>c__AnonStoreyCD
        {
            internal Battle.CombatEvent _event;

            internal void <>m__23(AiBuff obj)
            {
                obj.OnEvent(this._event);
            }
        }

        [CompilerGenerated]
        private sealed class <ResetBuffStateTime>c__AnonStoreyCB
        {
            internal AiActor <>f__this;
            internal int state;

            internal void <>m__20(AiBuff obj)
            {
                if (obj.State == this.state)
                {
                    obj.ResetTime(this.<>f__this.m_aiManager.Time);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ResetBuffTime>c__AnonStoreyCA
        {
            internal AiActor <>f__this;
            internal int buffEntry;

            internal void <>m__1F(AiBuff obj)
            {
                if (obj.Entry == this.buffEntry)
                {
                    obj.ResetTime(this.<>f__this.m_aiManager.Time);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateSelfCDOnCast>c__AnonStoreyD1
        {
            internal int skillEntry;

            internal bool <>m__27(SkillSelfCDInfo obj)
            {
                return (obj.skillEntry == this.skillEntry);
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateSkillCD>c__AnonStoreyCE
        {
            internal float delta;

            internal void <>m__24(SkillSelfCDInfo obj)
            {
                obj.CDBeLeft -= this.delta;
            }
        }
    }
}

