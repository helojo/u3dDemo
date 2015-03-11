namespace Battle
{
    using FastBuf;
    using Fatefulness;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AiBuff : ObjectTracer
    {
        private AiActor actor;
        private Fragment aiFate;
        private string buffId = string.Empty;
        private int castSkillsTimes;
        private buff_config cfg;
        public AiBuffInfo combatInfo = new AiBuffInfo();
        public static int counter;
        private float curDHOTTimeToNext;
        private TssSdtInt dmgToBreakBuff = 0;
        public TssSdtInt HpReducer;
        private BuffInfluenceType influenceType;
        private StateBuffType m_state = StateBuffType.None;
        public TssSdtInt skillHitLV;
        public TssSdtInt skillLV;

        public float GetAttributeEffect(SkillLogicEffectType type, SkillLogicEffectOperateType operateType)
        {
            if (this.cfg != null)
            {
                if ((this.cfg.effectType1 == type) && (this.cfg.effectOperateType1 == operateType))
                {
                    return (this.cfg.effectValue1 + this.GetAttributeEffectLevelUp(type, this.cfg.effectValue1_lvup));
                }
                if ((this.cfg.effectType2 == type) && (this.cfg.effectOperateType2 == operateType))
                {
                    return (this.cfg.effectValue2 + this.GetAttributeEffectLevelUp(type, this.cfg.effectValue2_lvup));
                }
                if ((this.cfg.effectType3 == type) && (this.cfg.effectOperateType3 == operateType))
                {
                    return (this.cfg.effectValue3 + this.GetAttributeEffectLevelUp(type, this.cfg.effectValue3_lvup));
                }
            }
            return 0f;
        }

        private float GetAttributeEffectLevelUp(SkillLogicEffectType type, float lvup)
        {
            return (lvup * ((float) (this.skillLV - 1)));
        }

        public bool GetIsCanBeAttackBySkillType(SkillHurtType hurtType)
        {
            if ((hurtType == SkillHurtType.Physics) && (this.m_state == StateBuffType.GoAway))
            {
                return false;
            }
            return true;
        }

        public void Init(AiActor _caster, AiActor _actor, int entry, float startTime, int _skillLV, int _skillHitLV, int skillEntry, int subSkillEntry)
        {
            this.Entry = entry;
            this.caster = _caster;
            this.actor = _actor;
            this.skillLV = _skillLV;
            this.skillHitLV = _skillHitLV;
            this.cfg = ConfigMgr.getInstance().getByEntry<buff_config>(this.Entry);
            this.influenceType = (BuffInfluenceType) this.cfg.influenceType;
            this.State = (StateBuffType) this.cfg.type;
            this.EndTime = startTime + this.cfg.duration;
            this.curDHOTTimeToNext = this.cfg.DHOTCD;
            this.HpReducer = this.cfg.HPReducer + (this.cfg.HPReducer_lvup * (this.skillLV - 1));
            this.combatInfo.casterAttack = this.caster.FixedAttack;
            this.combatInfo.casterCritDmg = this.caster.FixedCritDmg;
            this.combatInfo.casterHealMod = this.caster.FixedHealMod;
            this.combatInfo.casterCritRate = this.caster.FixedCritRate;
            this.combatInfo.casterSpellPierce = this.caster.FixedSpellPierce;
            this.combatInfo.casterPhysicsPierce = this.caster.FixedPhysicsPierce;
            this.combatInfo.skillEntry = skillEntry;
            this.SkillEntry = skillEntry;
            this.SubSkillEntry = subSkillEntry;
            this.buffId = string.Format("{0}_{1}_{2}", this.caster.ServerIdx, this.actor.ServerIdx, counter++);
            this.InitAI();
        }

        private void InitAI()
        {
            if (!string.IsNullOrEmpty(this.cfg.ai_script))
            {
                this.aiFate = AiFragmentManager.GetInstance().GetFrament(this.cfg.ai_script);
                if (this.aiFate == null)
                {
                    Debug.LogWarning("failed to load skill fate named" + this.cfg.ai_script);
                }
                else
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutObject(SkillIntentDecl.skill_ai_manager, this.actor.m_aiManager);
                    context.PutInt32(SkillIntentDecl.skill_caster_id, this.caster.Id);
                    context.PutInt32(SkillIntentDecl.skill_target_id, this.actor.Id);
                    context.PutObject(SkillIntentDecl.skill_ai_skill, null);
                    context.PutObject(SkillIntentDecl.skill_ai_buffer, this);
                    context.PutInt32(SkillIntentDecl.skill_entry, this.Entry);
                    this.aiFate.Deploy(context);
                    Intent intent2 = new Intent(Intent.IntentType.forward);
                    intent2.PutString(IntentDecl.port, "Preload");
                    this.aiFate.stack_context.PutObject(SkillIntentDecl.skill_caster_id, this.caster.Id);
                    this.aiFate.stack_context.PutObject(SkillIntentDecl.skill_target_id, this.actor.Id);
                    this.aiFate.stack_context.PutObject("BuffEntry", this.Entry);
                    this.aiFate.Dispatch(intent2);
                }
            }
        }

        public bool isCanPastToNextPhase()
        {
            return (this.cfg.succeed_type == 1);
        }

        public bool IsEnemyTeamBuff()
        {
            return (this.influenceType == BuffInfluenceType.EnemyTeam);
        }

        public bool IsPlayerBuff()
        {
            return (this.influenceType == BuffInfluenceType.Player);
        }

        public bool IsTeamBuff()
        {
            return (this.influenceType == BuffInfluenceType.Team);
        }

        public void OnAdd()
        {
            if (this.IsCanBreakSkill)
            {
                bool setIdle = true;
                if (this.actor.IsBeFeard && (this.m_state == StateBuffType.Seduce))
                {
                    setIdle = false;
                }
                this.actor.BreakSkill(false, setIdle);
            }
            else if (this.IsCanBreakMagicSkill || this.IsCanBreakPhysicsSkill)
            {
                if (this.actor.IsBeFeard)
                {
                    this.actor.BreakSkill(false, false);
                }
                else if (this.actor.CastingSkill != null)
                {
                    skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(this.actor.CastingSkill.SkillEntry);
                    if ((_config != null) && (((_config.type == 1) && this.IsCanBreakPhysicsSkill) || ((_config.type == 2) && this.IsCanBreakPhysicsSkill)))
                    {
                        this.actor.BreakSkill(false, true);
                    }
                }
            }
            if ((this.m_state == StateBuffType.Fear) && this.actor.IsCanMove)
            {
                Vector3 destPos = this.actor.Pos - ((Vector3) (this.actor.HeadDirection * AiDef.FEAR_MOVE_DISTANCE));
                this.actor.MoveTo(destPos, null);
            }
            if ((this.m_state == StateBuffType.Sleep) || (this.m_state == StateBuffType.Seduce))
            {
                this.dmgToBreakBuff = (int) (this.actor.MaxHpInit / ((long) AiDef.Sleep_Hurt_Break_Skill_Scale));
            }
            this.OnEffectAdd((SkillLogicEffectType) this.cfg.effectType1, (SkillLogicEffectOperateType) this.cfg.effectOperateType1, this.cfg.effectValue1);
            this.OnEffectAdd((SkillLogicEffectType) this.cfg.effectType2, (SkillLogicEffectOperateType) this.cfg.effectOperateType2, this.cfg.effectValue2);
            this.OnEffectAdd((SkillLogicEffectType) this.cfg.effectType3, (SkillLogicEffectOperateType) this.cfg.effectOperateType3, this.cfg.effectValue3);
            BattleSecurityManager.Instance.RegisterBuffData(this.buffId, this.Entry, (this.caster == null) ? -1 : this.caster.ServerIdx, (this.actor == null) ? -1 : this.actor.ServerIdx, this.SkillEntry, this.SubSkillEntry);
        }

        private void OnBuffAddEvent(int buffId, int buffState)
        {
            try
            {
                if (this.aiFate != null)
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutString(IntentDecl.port, "AddBuff");
                    this.aiFate.stack_context.PutObject("add_buff_entry", buffId);
                    this.aiFate.stack_context.PutObject("add_buff_state", buffState);
                    this.aiFate.Dispatch(context);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Warning:" + exception.Message);
            }
        }

        private void OnCastSkillConfirmedEvent(int targetId, int casterId, int skillId, int buffCastSkillTimes)
        {
            try
            {
                if (this.aiFate != null)
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutString(IntentDecl.port, "Confirm");
                    context.PutObject(SkillIntentDecl.skill_target_id, targetId);
                    context.PutInt32(SkillIntentDecl.skill_entry, skillId);
                    AiActor actorById = this.actor.m_aiManager.GetActorById(casterId);
                    this.aiFate.stack_context.PutObject("skill_cast_target", actorById);
                    this.aiFate.Dispatch(context);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Warning:" + exception.Message);
            }
        }

        private void OnCastSkillEvent(int skillId)
        {
            try
            {
                if (this.aiFate != null)
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutString(IntentDecl.port, "Cast");
                    this.aiFate.stack_context.PutObject("skill_entry", skillId);
                    this.aiFate.Dispatch(context);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Warning:" + exception.Message);
            }
        }

        private void OnCastSkillFinishedEvent(int skillId)
        {
            try
            {
                if (this.aiFate != null)
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutString(IntentDecl.port, "CastFinish");
                    this.aiFate.stack_context.PutObject("skill_entry", skillId);
                    this.aiFate.Dispatch(context);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Warning:" + exception.Message);
            }
        }

        public void OnDeadEvent(int deaderId)
        {
            this.OnDeadEvent(deaderId, -1);
        }

        public void OnDeadEvent(int deaderId, int killerId)
        {
            try
            {
                if (this.aiFate != null)
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutString(IntentDecl.port, "Killed");
                    AiActor actorById = this.actor.m_aiManager.GetActorById(deaderId);
                    AiActor actor2 = this.actor.m_aiManager.GetActorById(killerId);
                    actorById = (actorById != null) ? actorById : AiActor.GetEmptyAiactor();
                    actor2 = (actor2 != null) ? actor2 : AiActor.GetEmptyAiactor();
                    this.aiFate.stack_context.PutObject("DeaderObj", actorById);
                    this.aiFate.stack_context.PutObject("KillerObj", actor2);
                    this.aiFate.Dispatch(context);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Warning:" + exception.Message);
            }
        }

        private void OnEffectAdd(SkillLogicEffectType type, SkillLogicEffectOperateType operateType, float value)
        {
            if ((type != SkillLogicEffectType.MaxHp) && (type == SkillLogicEffectType.MoveSpeed))
            {
                float changeValue = 0f;
                if (operateType == SkillLogicEffectOperateType.Multiple)
                {
                    changeValue = (this.actor.MoveSpeedInit * value) / ((float) AiDef.ONE_HUNDRED_PCT_VALUE);
                }
                else if (operateType == SkillLogicEffectOperateType.Add)
                {
                    changeValue = value / ((float) AiDef.ONE_HUNDRED_PCT_VALUE);
                }
                this.actor.ChangeSpeed(changeValue);
            }
        }

        private void OnEffectRemove(SkillLogicEffectType type, SkillLogicEffectOperateType operateType, float value)
        {
            if ((type != SkillLogicEffectType.MaxHp) && (type == SkillLogicEffectType.MoveSpeed))
            {
                float num = 0f;
                if (operateType == SkillLogicEffectOperateType.Multiple)
                {
                    num = (this.actor.MoveSpeedInit * value) / ((float) AiDef.ONE_HUNDRED_PCT_VALUE);
                }
                else if (operateType == SkillLogicEffectOperateType.Add)
                {
                    num = value / ((float) AiDef.ONE_HUNDRED_PCT_VALUE);
                }
                this.actor.ChangeSpeed(-num);
            }
        }

        private void OnEnergyChangedEvent(int valueChanged, int targetId, int buffCasterId)
        {
            try
            {
                if (this.aiFate != null)
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutString(IntentDecl.port, "EnergyChange");
                    context.PutInt32(SkillIntentDecl.energy_changed_value, valueChanged);
                    context.PutObject(SkillIntentDecl.skill_target_id, targetId);
                    context.PutObject(SkillIntentDecl.skill_caster_id, buffCasterId);
                    this.aiFate.Dispatch(context);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Warning:" + exception.Message);
            }
        }

        public void OnEvent(Battle.CombatEvent _event)
        {
            if (_event.type == Battle.CombatEventType.Hurt)
            {
                if (_event.actorID == this.actor.Id)
                {
                    if (this.aiFate != null)
                    {
                        Intent context = new Intent(Intent.IntentType.forward);
                        SkillEffectResult result = new SkillEffectResult(_event.hitType, 0L) {
                            changeValue = _event.changeValue,
                            targetID = _event.actorID,
                            hurtType = _event.hurtType
                        };
                        context.PutObject(SkillIntentDecl.skill_effect_result, result);
                        context.PutString(IntentDecl.port, "Hurt");
                        this.aiFate.Dispatch(context);
                    }
                    if (this.dmgToBreakBuff > 0)
                    {
                        this.dmgToBreakBuff += _event.changeValue;
                        if (this.dmgToBreakBuff <= 0)
                        {
                            this.EndTime = -1f;
                        }
                    }
                }
            }
            else if (_event.type == Battle.CombatEventType.Dead)
            {
                this.OnDeadEvent(_event.actorID, _event.relatedActorID);
            }
            else if (_event.type == Battle.CombatEventType.CastSkillConfirmed)
            {
                if ((_event.relatedActorID == this.actor.Id) && (_event.relatedActorID != _event.actorID))
                {
                    this.OnCastSkillConfirmedEvent(_event.relatedActorID, _event.actorID, _event.changeValue, this.castSkillsTimes++);
                }
            }
            else if (_event.type == Battle.CombatEventType.EnergyChange)
            {
                if ((_event.actorID == this.actor.Id) && (this.actor.Id != this.caster.Id))
                {
                    this.OnEnergyChangedEvent(_event.changeValue, _event.actorID, this.caster.Id);
                }
            }
            else if (_event.type == Battle.CombatEventType.CastSkill)
            {
                if (_event.actorID == this.actor.Id)
                {
                    this.OnCastSkillEvent(_event.changeValue);
                }
            }
            else if (_event.type == Battle.CombatEventType.CastSkillFinished)
            {
                if (_event.actorID == this.actor.Id)
                {
                    this.OnCastSkillFinishedEvent(_event.changeValue);
                }
            }
            else if ((_event.type == Battle.CombatEventType.AddBuff) && (_event.actorID == this.actor.Id))
            {
                this.OnBuffAddEvent(_event.changeValue, _event.relatedActorID);
            }
        }

        public void OnRemove()
        {
            this.OnEffectRemove((SkillLogicEffectType) this.cfg.effectType1, (SkillLogicEffectOperateType) this.cfg.effectOperateType1, this.cfg.effectValue1);
            this.OnEffectRemove((SkillLogicEffectType) this.cfg.effectType2, (SkillLogicEffectOperateType) this.cfg.effectOperateType2, this.cfg.effectValue2);
            this.OnEffectRemove((SkillLogicEffectType) this.cfg.effectType3, (SkillLogicEffectOperateType) this.cfg.effectOperateType3, this.cfg.effectValue3);
            BattleSecurityManager.Instance.SetBuffEnd(this.buffId);
            if ((this.aiFate != null) && !string.IsNullOrEmpty(this.cfg.ai_script))
            {
                AiFragmentManager.GetInstance().PushFrament(this.cfg.ai_script, this.aiFate);
            }
            this.aiFate = null;
        }

        public void OnTick()
        {
            if (this.aiFate != null)
            {
                this.aiFate.Tick();
            }
        }

        public void OnUpdate()
        {
            if (this.aiFate != null)
            {
                this.aiFate.Update();
            }
        }

        public void ResetTime(float startTime)
        {
            this.EndTime = startTime + this.cfg.duration;
        }

        public void Tick(float deltaTime)
        {
            int subSkillID = this.cfg.DHOT_subSkillEntry;
            if (subSkillID >= 0)
            {
                this.curDHOTTimeToNext -= deltaTime;
                if (this.curDHOTTimeToNext <= 0f)
                {
                    this.curDHOTTimeToNext += this.cfg.DHOTCD;
                    this.actor.m_aiManager.OnEventSubEffect(null, subSkillID, this.actor, this.caster, (int) this.skillLV, (int) this.skillHitLV, this.combatInfo);
                }
            }
        }

        public virtual void Trace(ObjectMetaData meta)
        {
            meta.descript = "...";
            Debugger.TraceDetails<int>(meta, "entry", this.Entry);
            Debugger.TraceDetails<StateBuffType>(meta, "state type", this.State);
            Debugger.TraceDetails<TssSdtFloat>(meta, "end time", this.EndTime);
            Debugger.TraceDetails<BuffInfluenceType>(meta, "influence type", this.influenceType);
            Debugger.TraceDetails<TssSdtInt>(meta, "hp reducer", this.HpReducer);
            Debugger.TraceDetails<TssSdtInt>(meta, "break damage", this.dmgToBreakBuff);
        }

        public int UseHurtReduce(int hurtValue)
        {
            if (this.HpReducer > 0)
            {
                if (this.HpReducer > hurtValue)
                {
                    this.HpReducer -= hurtValue;
                    hurtValue = 0;
                    return hurtValue;
                }
                if (this.HpReducer == hurtValue)
                {
                    this.HpReducer = 0;
                    hurtValue = 0;
                    this.EndTime = -1f;
                    return hurtValue;
                }
                hurtValue -= this.HpReducer;
                this.HpReducer = 0;
                this.EndTime = -1f;
            }
            return hurtValue;
        }

        public AiActor caster { get; set; }

        public TssSdtFloat EndTime { get; set; }

        public int Entry { get; set; }

        public bool IsBeFeard
        {
            get
            {
                return (this.m_state == StateBuffType.Fear);
            }
        }

        public bool IsBeSeduced
        {
            get
            {
                return (this.m_state == StateBuffType.Seduce);
            }
        }

        public bool IsCanAct
        {
            get
            {
                return (((this.m_state != StateBuffType.Sheep) && (this.m_state != StateBuffType.BackToHole)) && (this.m_state != StateBuffType.Daze));
            }
        }

        public bool IsCanBeHealed
        {
            get
            {
                if (this.m_state == StateBuffType.UnBeHealed)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsCanBehurt
        {
            get
            {
                return ((this.m_state != StateBuffType.FreezDefend) && (this.m_state != StateBuffType.UnBeAttacked));
            }
        }

        public bool IsCanBeHurtByMagic
        {
            get
            {
                if (this.m_state == StateBuffType.UnBeAttackedByMagic)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsCanBeHurtByPhysics
        {
            get
            {
                if (this.m_state == StateBuffType.UnBeAttackedByPhysics)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsCanBeTarget
        {
            get
            {
                if (this.m_state == StateBuffType.UnBeTartget)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsCanBreakMagicSkill
        {
            get
            {
                return (this.IsCanBreakSkill || (this.m_state == StateBuffType.Silent));
            }
        }

        public bool IsCanBreakPhysicsSkill
        {
            get
            {
                return (this.IsCanBreakSkill || (this.m_state == StateBuffType.WeaponTakeOff));
            }
        }

        public bool IsCanBreakSkill
        {
            get
            {
                if ((((this.m_state != StateBuffType.Daze) && (this.m_state != StateBuffType.Sheep)) && ((this.m_state != StateBuffType.BackToHole) && (this.m_state != StateBuffType.Fear))) && ((((this.m_state != StateBuffType.GoAway) && (this.m_state != StateBuffType.AttackAway)) && ((this.m_state != StateBuffType.FreezDefend) && (this.m_state != StateBuffType.Seduce))) && ((this.m_state != StateBuffType.Sleep) && (this.m_state != StateBuffType.Freez))))
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsCanCastMagicSkill
        {
            get
            {
                if (!this.IsCanCastSkill)
                {
                    return false;
                }
                if (this.m_state == StateBuffType.Silent)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsCanCastNormalAttack
        {
            get
            {
                return ((((this.m_state != StateBuffType.Daze) && (this.m_state != StateBuffType.Sheep)) && ((this.m_state != StateBuffType.BackToHole) && (this.m_state != StateBuffType.Fear))) && (((this.m_state != StateBuffType.GoAway) && (this.m_state != StateBuffType.AttackAway)) && (((this.m_state != StateBuffType.FreezDefend) && (this.m_state != StateBuffType.Sleep)) && (this.m_state != StateBuffType.Freez))));
            }
        }

        public bool IsCanCastPhysicsSkill
        {
            get
            {
                if (!this.IsCanCastSkill)
                {
                    return false;
                }
                if (this.m_state == StateBuffType.WeaponTakeOff)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsCanCastSkill
        {
            get
            {
                return ((((this.m_state != StateBuffType.Daze) && (this.m_state != StateBuffType.Sheep)) && ((this.m_state != StateBuffType.BackToHole) && (this.m_state != StateBuffType.Fear))) && (((this.m_state != StateBuffType.GoAway) && (this.m_state != StateBuffType.AttackAway)) && (((this.m_state != StateBuffType.FreezDefend) && (this.m_state != StateBuffType.Sleep)) && (this.m_state != StateBuffType.Freez))));
            }
        }

        public bool IsCanMove
        {
            get
            {
                return ((((this.m_state != StateBuffType.Sheep) && (this.m_state != StateBuffType.BackToHole)) && ((this.m_state != StateBuffType.Daze) && (this.m_state != StateBuffType.GoAway))) && (((this.m_state != StateBuffType.Fear) && (this.m_state != StateBuffType.Freez)) && ((this.m_state != StateBuffType.FreezDefend) && (this.m_state != StateBuffType.Sleep))));
            }
        }

        public bool IsEnergyCanBeAdded
        {
            get
            {
                if (this.m_state == StateBuffType.UnBeEnergyAdded)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsVisible
        {
            get
            {
                if (this.m_state == StateBuffType.Cloaking)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsWeaponTakenOff
        {
            get
            {
                if (this.m_state == StateBuffType.WeaponTakeOff)
                {
                    return false;
                }
                return true;
            }
        }

        public int SkillEntry { get; set; }

        public StateBuffType State
        {
            get
            {
                return this.m_state;
            }
            set
            {
                this.m_state = value;
            }
        }

        public int SubSkillEntry { get; set; }
    }
}

