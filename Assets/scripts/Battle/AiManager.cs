namespace Battle
{
    using FastBuf;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AiManager
    {
        private bool _IsPreFight = true;
        [CompilerGenerated]
        private static Func<AiActor, bool> <>f__am$cache20;
        public Action<int, int, float, int> EventAddBuff;
        public Action<bool, bool> EventBattleFinish;
        public Action<int, int> EventBreakSkill;
        public Action<CastNewSkill_Input> EventCastNewSkill;
        public Action<int, int> EventDelBuff;
        public Action<int, int> EventFaceToActor;
        public Action<int, Vector3, float, int> EventMoveActorToPos;
        public Action<int> EventOnActorDead;
        public Action<int, long, long> EventOnActorHPMaxHPCHange;
        public Action<int, float> EventOnActorMoveSpeedChange;
        public Action<int, string, List<int>> EventOnDoSkillCasting;
        public System.Action EventOnStartBattle;
        public Action<int, long, int> EventRevive;
        public Action<int, int, string, bool> EventSetBufferEffectActive;
        public Action<int, Vector3> EventSetPos;
        public Action<int, int> EventSkillFinish;
        public Action<int> EventStopActorMove;
        public Action<int, int, Vector3> EventSummon;
        public Action<SkillEffectResult> EvetntOnSkillEffectResult;
        public Action<int, string, int> EvetntOnSubShowEffect;
        private readonly Dictionary<int, IActor> m_actorMap = new Dictionary<int, IActor>();
        private AiTeam m_attackerTeam = new AiTeam();
        private Vector3 m_basePoint = Vector3.zero;
        private AiTeam m_defenderTeam = new AiTeam();
        private float m_detlaTime;
        private int m_nextActorId = (AiDef.MAX_ACTOR_EACH_SIDE * 2);
        private int m_nextSkillUniId;
        private Vector3 m_pathDirection;
        private readonly Dictionary<int, AiSkill> m_skillMap = new Dictionary<int, AiSkill>();

        public void AddActorToMap(IActor actor)
        {
            if (!this.m_actorMap.ContainsKey(actor.Id))
            {
                this.m_actorMap.Add(actor.Id, actor);
            }
        }

        public void AddSkill(AiSkill skill)
        {
            this.m_skillMap.Add(skill.SkillUniId, skill);
        }

        public void Clear()
        {
            if (this.AttackerTeam != null)
            {
                this.AttackerTeam.CrearActorData();
            }
            if (this.DefenderTeam != null)
            {
                this.DefenderTeam.CrearActorData();
            }
            this.EventCastNewSkill = null;
            this.EventMoveActorToPos = null;
            this.EventStopActorMove = null;
            this.EventBattleFinish = null;
            this.EventAddBuff = null;
            this.EventDelBuff = null;
            this.EventSetBufferEffectActive = null;
            this.EventSummon = null;
            this.EventBreakSkill = null;
            this.EventRevive = null;
            this.EventFaceToActor = null;
            this.EventOnActorHPMaxHPCHange = null;
            this.EvetntOnSubShowEffect = null;
            this.EvetntOnSkillEffectResult = null;
            this.EventOnDoSkillCasting = null;
            this.EventOnActorMoveSpeedChange = null;
            this.EventOnActorDead = null;
            this.EventOnStartBattle = null;
        }

        public SkillEffectCastResult DoSkillCasting(int skillUniId)
        {
            AiSkill skill;
            if (this.m_skillMap.TryGetValue(skillUniId, out skill))
            {
                return skill.DoSkillCasting();
            }
            return null;
        }

        public AiActor GetActorById(int id)
        {
            return (this.GetBaseActorById(id) as AiActor);
        }

        public int GetAttackerHeadActor()
        {
            AiActor actor = AiUtil.FindHeadActor(this.m_attackerTeam, null);
            if (actor != null)
            {
                return actor.Id;
            }
            return -1;
        }

        public int GetAttackerLastActor()
        {
            AiActor actor = AiUtil.FindLastActor(this.m_attackerTeam, null);
            if (actor != null)
            {
                return actor.Id;
            }
            return -1;
        }

        public IActor GetBaseActorById(int id)
        {
            IActor actor;
            this.m_actorMap.TryGetValue(id, out actor);
            return actor;
        }

        public RealTimeBuffer GetBuffer(int actorID, int bufferEntry)
        {
            return this.BattleCom.GetBuffer(actorID, bufferEntry);
        }

        public int GetDefenderHeadActor()
        {
            AiActor actor = AiUtil.FindHeadActor(this.m_defenderTeam, null);
            if (actor != null)
            {
                return actor.Id;
            }
            return -1;
        }

        public int GetDefenderLastActor()
        {
            AiActor actor = AiUtil.FindLastActor(this.m_defenderTeam, null);
            if (actor != null)
            {
                return actor.Id;
            }
            return -1;
        }

        public float GetDistanceOfTwoActor(AiActor actor1, AiActor actor2)
        {
            return AiUtil.CalDistanceByXWithDir(actor1, actor2);
        }

        public AiSkillObj GetSkillAiById(int id)
        {
            return (this.GetBaseActorById(id) as AiSkillObj);
        }

        public AiSkill GetSkillByUniId(int skillUniId)
        {
            if (!this.m_skillMap.ContainsKey(skillUniId))
            {
                return null;
            }
            return this.m_skillMap[skillUniId];
        }

        public AiTeam GetTeam(bool isAttacker)
        {
            return (!isAttacker ? this.DefenderTeam : this.AttackerTeam);
        }

        public void Init(ActorDataList attackerActorData, ActorDataList defenderActorData, BattleCom_Runtime battleCom, Vector3 pathDirection)
        {
            AiUtil.Log("init", new object[0]);
            this.PathDirection = pathDirection;
            this.IsPreFight = true;
            battleCom.eventInterface.OnSetSkillCasting = (Action<int, int>) Delegate.Combine(battleCom.eventInterface.OnSetSkillCasting, new Action<int, int>(this.OnSkillPrepared));
            battleCom.eventInterface.OnSkillFinish = (Action<int, int>) Delegate.Combine(battleCom.eventInterface.OnSkillFinish, new Action<int, int>(this.OnSkillEffectFinish));
            battleCom.eventInterface.OnQuestCastSkill = (Action<int>) Delegate.Combine(battleCom.eventInterface.OnQuestCastSkill, new Action<int>(this.OnUserCastSkill));
            battleCom.eventInterface.OnMoveFinish = (Action<int>) Delegate.Combine(battleCom.eventInterface.OnMoveFinish, new Action<int>(this.OnMoveFinish));
            battleCom.eventInterface.OnSubSkillFinish = (Action<int, int>) Delegate.Combine(battleCom.eventInterface.OnSubSkillFinish, new Action<int, int>(this.OnSubSkillFinish));
            battleCom.eventInterface.OnActorAttackAwayFinish = (Action<int>) Delegate.Combine(battleCom.eventInterface.OnActorAttackAwayFinish, new Action<int>(this.OnActorAttackAwayFinish));
            this.BattleCom = battleCom;
            this.AttackerTeam.IsAttacker = true;
            this.DefenderTeam.IsAttacker = false;
            int num = 0;
            foreach (CombatDetailActor actor in attackerActorData.ActorList)
            {
                AiActor actor2 = this.AttackerTeam.AddActor(actor, this);
                if (num < attackerActorData.ServerIndexList.Count)
                {
                    actor2.ServerIdx = attackerActorData.ServerIndexList[num];
                }
                if (actor != null)
                {
                    BattleSecurityManager.Instance.RegisterFigherData(actor2.ServerIdx, (long) actor2.Hp, actor2.Energy, actor2.Id, actor2.IsSummonActor, actor.entry, actor.quality, actor.starlv, actor.isCard, true, -1);
                }
                num++;
                this.TrySetBasePoint(actor2);
            }
            num = 0;
            foreach (CombatDetailActor actor3 in defenderActorData.ActorList)
            {
                AiActor actor4 = this.DefenderTeam.AddActor(actor3, this);
                if (num < defenderActorData.ServerIndexList.Count)
                {
                    actor4.ServerIdx = defenderActorData.ServerIndexList[num];
                }
                if (actor3 != null)
                {
                    BattleSecurityManager.Instance.RegisterFigherData(actor4.ServerIdx, (long) actor4.Hp, actor4.Energy, actor4.Id, actor4.IsSummonActor, actor3.entry, actor3.quality, actor3.starlv, actor3.isCard, false, -1);
                }
                num++;
                this.TrySetBasePoint(actor4);
            }
            Vector3 zero = Vector3.zero;
            foreach (AiActor actor5 in this.AttackerTeam.AliveActorList)
            {
                zero += actor5.Pos;
            }
            this.CenterPos = (Vector3) (zero / ((float) this.AttackerTeam.AliveActorList.Count));
        }

        public bool IsActorCanUseActiveSkill(int actorID)
        {
            AiActor actorById = this.GetActorById(actorID);
            return ((actorById != null) && actorById.IsCanUseActiveSkill());
        }

        public bool IsActorHpLow(int actorID)
        {
            AiActor actorById = this.GetActorById(actorID);
            return ((actorById != null) && actorById.IsHpLow());
        }

        public bool IsActorSkillEnergyFull(int actorID)
        {
            AiActor actorById = this.GetActorById(actorID);
            return ((actorById != null) && actorById.IsSkillEnergyFull());
        }

        public int NewActorId()
        {
            return this.m_nextActorId++;
        }

        public int NewSkillUniId()
        {
            return this.m_nextSkillUniId++;
        }

        public void NoticeAddBuf(int actorId, int entry, float endTime, int casterID)
        {
            if (this.EventAddBuff != null)
            {
                this.EventAddBuff(actorId, entry, endTime, casterID);
            }
        }

        public void NoticeBattleFinish(bool isWin)
        {
            if (this.EventBattleFinish != null)
            {
                this.EventBattleFinish(isWin, false);
            }
        }

        public void NoticeBreakSkill(int actorId, int skillUniId)
        {
            AiUtil.Log("NoticeBreakSkill " + actorId, new object[0]);
            if (this.EventBreakSkill != null)
            {
                this.EventBreakSkill(actorId, skillUniId);
            }
        }

        public void NoticeCastNewSkill(CastNewSkill_Input input)
        {
            if (this.EventCastNewSkill != null)
            {
                this.EventCastNewSkill(input);
            }
        }

        public void NoticeDelBuff(int actorId, int entry)
        {
            if (this.EventDelBuff != null)
            {
                this.EventDelBuff(actorId, entry);
            }
        }

        public void NoticeFaceToActor(int actorId, int targetId)
        {
            if (this.EventFaceToActor != null)
            {
                this.EventFaceToActor(actorId, targetId);
            }
        }

        public void NoticeMoveActorToPos(int actorId, Vector3 destPos, float targetRadius, int targetId)
        {
            if (this.EventMoveActorToPos != null)
            {
                this.EventMoveActorToPos(actorId, destPos, targetRadius, targetId);
            }
        }

        public void NoticeRevive(int actorId, long hp, int energy)
        {
            if (this.EventRevive != null)
            {
                this.EventRevive(actorId, hp, energy);
            }
        }

        public void NoticeSetBufferEffectActive(int actorId, int entry, string objName, bool active)
        {
            if (this.EventSetBufferEffectActive != null)
            {
                this.EventSetBufferEffectActive(actorId, entry, objName, active);
            }
        }

        public void NoticeSetPos(int actorId, Vector3 pos)
        {
            if (this.EventSetPos != null)
            {
                this.EventSetPos(actorId, pos);
            }
        }

        public void NoticeStopActorMove(int actorId)
        {
            if (this.EventStopActorMove != null)
            {
                this.EventStopActorMove(actorId);
            }
        }

        public void NoticeSummon(IActor creature, int creatureEntry, AiActor caster, Vector3 pos)
        {
            if (this.EventSummon != null)
            {
                this.EventSummon(creature.Id, creatureEntry, pos);
            }
        }

        private void OnActorAttackAwayFinish(int actorId)
        {
            AiActor actorById = this.GetActorById(actorId);
            if (actorById != null)
            {
                actorById.OnActorAttackAwayFinish();
            }
        }

        public void OnCastSkillConfirmedEvent(int casterID, int targetID, int skillId)
        {
            Battle.CombatEvent event2 = new Battle.CombatEvent {
                type = Battle.CombatEventType.CastSkillConfirmed,
                actorID = targetID,
                relatedActorID = casterID,
                changeValue = skillId
            };
            this.OnEvent(event2);
        }

        public void OnCastSkillEvent(int casterID, int skillId)
        {
            Battle.CombatEvent event2 = new Battle.CombatEvent {
                type = Battle.CombatEventType.CastSkill,
                actorID = casterID,
                changeValue = skillId
            };
            this.OnEvent(event2);
        }

        public void OnCastSkillFinishedEvent(int casterID, int skillId)
        {
            Battle.CombatEvent event2 = new Battle.CombatEvent {
                type = Battle.CombatEventType.CastSkillFinished,
                actorID = casterID,
                changeValue = skillId
            };
            this.OnEvent(event2);
        }

        public void OnEnergyChangeEvent(int changeValue, int targetId)
        {
            Battle.CombatEvent event2 = new Battle.CombatEvent {
                type = Battle.CombatEventType.EnergyChange,
                changeValue = changeValue,
                actorID = targetId
            };
            this.OnEvent(event2);
        }

        public void OnEvent(Battle.CombatEvent _event)
        {
            <OnEvent>c__AnonStoreyD4 yd = new <OnEvent>c__AnonStoreyD4 {
                _event = _event
            };
            this.m_attackerTeam.AliveActorList.ForEach(new Action<AiActor>(yd.<>m__2C));
            this.m_defenderTeam.AliveActorList.ForEach(new Action<AiActor>(yd.<>m__2D));
        }

        public void OnEventDoSkillCasting(AiSkill skill, string castName, List<AiActor> targetList)
        {
            if (this.EventOnDoSkillCasting != null)
            {
                <OnEventDoSkillCasting>c__AnonStoreyD5 yd = new <OnEventDoSkillCasting>c__AnonStoreyD5 {
                    targetIDs = new List<int>()
                };
                targetList.ForEach(new Action<AiActor>(yd.<>m__2E));
                if (yd.targetIDs.Count > 0)
                {
                    this.EventOnDoSkillCasting(skill.SkillUniId, castName, yd.targetIDs);
                }
            }
        }

        public void OnEventShowEffect(AiSkill skill, string effectName, AiActor target)
        {
            if (this.EvetntOnSubShowEffect != null)
            {
                this.EvetntOnSubShowEffect((skill == null) ? -1 : skill.SkillUniId, effectName, target.Id);
            }
        }

        public void OnEventSubEffect(AiSkill skill, int subSkillID, AiActor target, AiActor caster, int skillLV, int skillHitLV, AiBuffInfo buffCombatInfo)
        {
            if (target != null)
            {
                if (caster == null)
                {
                    caster = target;
                }
                if (skill != null)
                {
                    skill.OnTakeSkillEffectByID(target, subSkillID);
                }
                else
                {
                    AiSkillFunction.G_OnTakeSkillEffectByID(this, null, caster, target, subSkillID, null, skillHitLV, skillLV, buffCombatInfo);
                }
            }
        }

        private void OnMoveFinish(int actorId)
        {
            AiActor actorById = this.GetActorById(actorId);
            if ((actorById != null) && ((actorById.State == AiState.Move) || (actorById.State == AiState.MoveToPos)))
            {
                actorById.State = AiState.Idle;
            }
        }

        public void OnRegisterActorFinalData()
        {
            if (this.m_attackerTeam != null)
            {
                foreach (AiActor actor in this.m_attackerTeam.ActorList)
                {
                    BattleSecurityManager.Instance.SetFigherData(actor.ServerIdx, (long) actor.Hp, actor.Energy);
                }
            }
            if (this.m_defenderTeam != null)
            {
                foreach (AiActor actor2 in this.m_defenderTeam.ActorList)
                {
                    BattleSecurityManager.Instance.SetFigherData(actor2.ServerIdx, (long) actor2.Hp, actor2.Energy);
                }
            }
        }

        public void OnSkillAICastFinish(int skillID)
        {
            AiSkill skill;
            if (this.m_skillMap.TryGetValue(skillID, out skill))
            {
                skill.OnSkillAICastFinish();
            }
        }

        public void OnSkillAICastStart(int skillID)
        {
            AiSkill skill;
            if (this.m_skillMap.TryGetValue(skillID, out skill))
            {
                skill.OnSkillAICastStart();
            }
        }

        public void OnSkillEffectFinish(int actorId, int skillUniId)
        {
            AiActor actorById = this.GetActorById(actorId);
            if (actorById != null)
            {
                actorById.OnSkillEffectFinish(skillUniId);
            }
        }

        internal void OnSkillFinish(int casterId)
        {
        }

        public void OnSkillPrepared(int actorId, int skillUniId)
        {
            this.GetActorById(actorId).OnSkillPrepared();
        }

        public void OnSkillTakeEffectEvent(int casterID, SkillEffectResult result)
        {
            Battle.CombatEvent event2 = new Battle.CombatEvent();
            if ((result.effectType == SubSkillType.Hurt) && (((result.hitType == SkillHitType.Hit) || (result.hitType == SkillHitType.Cri)) || (result.hitType == SkillHitType.Absorb)))
            {
                event2.type = Battle.CombatEventType.Hurt;
                event2.actorID = result.targetID;
                event2.relatedActorID = casterID;
                event2.hurtType = result.hurtType;
                event2.hitType = result.hitType;
                event2.changeValue = result.changeValue;
                event2.hpValue = result.value;
                this.OnEvent(event2);
                if (result.value == 0)
                {
                    bool flag = true;
                    AiActor actorById = this.GetActorById(result.targetID);
                    if ((actorById != null) && actorById.IsSummonActor)
                    {
                        flag = false;
                    }
                    event2.type = Battle.CombatEventType.Dead;
                    if (flag)
                    {
                        this.OnEvent(event2);
                    }
                }
            }
        }

        private void OnSubSkillFinish(int actorId, int skillID)
        {
            AiActor actorById = this.GetActorById(actorId);
            if (actorById != null)
            {
                actorById.OnSubSkillFinish(skillID);
            }
        }

        public SkillEffectResult OnTakeSkillEffect(int actorId, int skillUniId, int subSkillIdx)
        {
            AiSkill skill;
            if (this.m_skillMap.TryGetValue(skillUniId, out skill))
            {
                return skill.OnTakeSkillEffect(this.GetActorById(actorId), subSkillIdx);
            }
            return null;
        }

        public void OnUserCastSkill(int actorId)
        {
            this.GetActorById(actorId).OnUserCastSkill();
        }

        public void Ready()
        {
            foreach (AiActor actor in this.AllAliveActors)
            {
                actor.CastForemostSkill();
            }
        }

        public void SystemTick()
        {
            foreach (IActor actor in this.m_actorMap.Values)
            {
                actor.Tick();
            }
        }

        public void TestCastSkill(int casterId, int targetId)
        {
            this.GetActorById(casterId);
            this.GetActorById(targetId);
        }

        public void Tick(float deltaTime)
        {
            this.Update(deltaTime);
        }

        public void TrySetBasePoint(AiActor actor)
        {
            if ((this.BasePoint == Vector3.zero) && !actor.IsEmpty)
            {
                this.BasePoint = actor.Pos - ((Vector3) (AiDef.BASE_POINT_OFFSET_UNIT * this.PathDirection));
            }
        }

        private void Update(float deltaTime)
        {
            if (this.IsPreFight)
            {
                if (<>f__am$cache20 == null)
                {
                    <>f__am$cache20 = actor => actor.State != AiState.Casting;
                }
                if (this.AllAliveActors.All<AiActor>(<>f__am$cache20))
                {
                    this.IsPreFight = false;
                    if (this.EventOnStartBattle != null)
                    {
                        this.EventOnStartBattle();
                    }
                }
            }
            foreach (IActor actor in this.m_actorMap.Values)
            {
                actor.Update(deltaTime);
            }
            if (this.AttackerTeam.AliveActorList.Count == 0)
            {
                this.NoticeBattleFinish(false);
            }
            else if (this.DefenderTeam.AliveActorList.Count == 0)
            {
                this.NoticeBattleFinish(true);
            }
        }

        public List<AiActor> AllAliveActors
        {
            get
            {
                List<AiActor> list = new List<AiActor>();
                list.AddRange(this.AttackerTeam.AliveActorList);
                list.AddRange(this.DefenderTeam.AliveActorList);
                return list;
            }
        }

        public List<AiActor> AttackerAiList
        {
            get
            {
                return this.AttackerTeam.ActorList;
            }
        }

        public AiTeam AttackerTeam
        {
            get
            {
                return this.m_attackerTeam;
            }
            set
            {
                this.m_attackerTeam = value;
            }
        }

        public Vector3 BasePoint
        {
            get
            {
                return this.m_basePoint;
            }
            set
            {
                this.m_basePoint = value;
            }
        }

        public BattleCom_Runtime BattleCom { get; set; }

        public Vector3 CenterPos { get; set; }

        public List<AiActor> DefenderAiList
        {
            get
            {
                return this.DefenderTeam.ActorList;
            }
        }

        public AiTeam DefenderTeam
        {
            get
            {
                return this.m_defenderTeam;
            }
            set
            {
                this.m_defenderTeam = value;
            }
        }

        public bool IsPreFight
        {
            get
            {
                return this._IsPreFight;
            }
            set
            {
                this._IsPreFight = value;
            }
        }

        public Vector3 PathDirection
        {
            get
            {
                return this.m_pathDirection;
            }
            set
            {
                this.m_pathDirection = value;
            }
        }

        public float Time
        {
            get
            {
                return BattleState.GetInstance().GetBattletime();
            }
        }

        public Vector3 VerticalDirection
        {
            get
            {
                return Vector3.Cross(this.PathDirection, Vector3.up);
            }
        }

        [CompilerGenerated]
        private sealed class <OnEvent>c__AnonStoreyD4
        {
            internal Battle.CombatEvent _event;

            internal void <>m__2C(AiActor obj)
            {
                obj.OnEvent(this._event);
            }

            internal void <>m__2D(AiActor obj)
            {
                obj.OnEvent(this._event);
            }
        }

        [CompilerGenerated]
        private sealed class <OnEventDoSkillCasting>c__AnonStoreyD5
        {
            internal List<int> targetIDs;

            internal void <>m__2E(AiActor obj)
            {
                if (obj != null)
                {
                    this.targetIDs.Add(obj.Id);
                }
            }
        }
    }
}

