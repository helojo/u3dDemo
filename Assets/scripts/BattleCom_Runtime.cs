using Battle;
using FastBuf;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_Runtime : BattleCom_Base
{
    [CompilerGenerated]
    private static Action<int> <>f__am$cache8;
    public EventInterface eventInterface = new EventInterface();
    public static bool IsForceAuto;
    public bool isRealBattleStarted;
    public bool isRuning;
    public bool isTestAutoFinish;
    public AiManager mAIManager;
    private List<int> showTimeObjIDs = new List<int>();
    public RealTimeSkillManager skillManager = new RealTimeSkillManager();

    private void CacheCardResources(int cardEntry, int cardQuality)
    {
        CardPlayer.CacheCardResource(cardEntry, cardQuality);
        this.skillManager.PrepareCardSkillResources(cardEntry);
    }

    public void CacheResources()
    {
        if (base.battleGameData.attActor != null)
        {
            base.battleGameData.attActor.ForEach(delegate (CombatDetailActor obj) {
                if (obj != null)
                {
                    this.CacheCardResources(obj.entry, obj.quality);
                }
            });
            base.battleGameData.defActor.ForEach(delegate (CombatTeam team) {
                if (team != null)
                {
                    team.actor.ForEach(delegate (CombatDetailActor obj) {
                        if (obj != null)
                        {
                            if (obj.isCard)
                            {
                                this.CacheCardResources(obj.entry, obj.quality);
                            }
                            else
                            {
                                monster_config _config = ConfigMgr.getInstance().getByEntry<monster_config>(obj.entry);
                                if (_config != null)
                                {
                                    this.CacheCardResources(_config.card_entry, obj.quality);
                                }
                            }
                        }
                    });
                }
            });
        }
    }

    private void Clear()
    {
        this.ClearEvent();
        if (this.mAIManager != null)
        {
            this.mAIManager.Clear();
            this.mAIManager.EventCastNewSkill = (Action<CastNewSkill_Input>) Delegate.Remove(this.mAIManager.EventCastNewSkill, new Action<CastNewSkill_Input>(this.EventCastNewSkill));
            this.mAIManager.EventMoveActorToPos = (Action<int, Vector3, float, int>) Delegate.Remove(this.mAIManager.EventMoveActorToPos, new Action<int, Vector3, float, int>(this.EventMoveActorToPos));
            this.mAIManager.EventStopActorMove = (Action<int>) Delegate.Remove(this.mAIManager.EventStopActorMove, new Action<int>(this.EventStopActorMove));
            this.mAIManager.EventBattleFinish = (Action<bool, bool>) Delegate.Remove(this.mAIManager.EventBattleFinish, new Action<bool, bool>(this.EventBattleFinish));
            this.mAIManager.EventAddBuff = (Action<int, int, float, int>) Delegate.Remove(this.mAIManager.EventAddBuff, new Action<int, int, float, int>(this.EventAddBuff));
            this.mAIManager.EventDelBuff = (Action<int, int>) Delegate.Remove(this.mAIManager.EventDelBuff, new Action<int, int>(this.EventDelBuff));
            this.mAIManager.EventSummon = (Action<int, int, Vector3>) Delegate.Remove(this.mAIManager.EventSummon, new Action<int, int, Vector3>(this.EventSummon));
            this.mAIManager.EventBreakSkill = (Action<int, int>) Delegate.Remove(this.mAIManager.EventBreakSkill, new Action<int, int>(this.EventBreakSkill));
            this.mAIManager.EventRevive = (Action<int, long, int>) Delegate.Remove(this.mAIManager.EventRevive, new Action<int, long, int>(this.EventRevive));
            this.mAIManager.EventFaceToActor = (Action<int, int>) Delegate.Remove(this.mAIManager.EventFaceToActor, new Action<int, int>(this.EventFaceToActor));
            this.mAIManager.EventOnActorHPMaxHPCHange = (Action<int, long, long>) Delegate.Remove(this.mAIManager.EventOnActorHPMaxHPCHange, new Action<int, long, long>(this.EventOnActorHPMaxHPCHange));
            this.mAIManager.EvetntOnSubShowEffect = (Action<int, string, int>) Delegate.Remove(this.mAIManager.EvetntOnSubShowEffect, new Action<int, string, int>(this.EvetntOnSubShowEffect));
            this.mAIManager.EvetntOnSkillEffectResult = (Action<SkillEffectResult>) Delegate.Remove(this.mAIManager.EvetntOnSkillEffectResult, new Action<SkillEffectResult>(this.EvetntOnSkillEffectResult));
            this.mAIManager.EventOnDoSkillCasting = (Action<int, string, List<int>>) Delegate.Remove(this.mAIManager.EventOnDoSkillCasting, new Action<int, string, List<int>>(this.EventOnDoSkillCasting));
            this.mAIManager.EventOnActorMoveSpeedChange = (Action<int, float>) Delegate.Remove(this.mAIManager.EventOnActorMoveSpeedChange, new Action<int, float>(this.EventOnActorMoveSpeedChange));
            this.mAIManager.EventOnActorDead = (Action<int>) Delegate.Remove(this.mAIManager.EventOnActorDead, new Action<int>(this.EventOnActorDead));
            this.mAIManager.EventOnStartBattle = (System.Action) Delegate.Remove(this.mAIManager.EventOnStartBattle, new System.Action(this.EventOnStartBattle));
        }
        this.mAIManager = null;
    }

    private void ClearEvent()
    {
        this.eventInterface.Clear();
    }

    public SkillEffectCastResult DoSkillCasting(int skillID)
    {
        return this.mAIManager.DoSkillCasting(skillID);
    }

    public SkillEffectResult DoSkillLogicEffectResult(int targetID, int skillID, int subSkillIndex)
    {
        return this.mAIManager.OnTakeSkillEffect(targetID, skillID, subSkillIndex);
    }

    private void EventAddBuff(int actorID, int bufferEntry, float remainTime, int casterID)
    {
        BattleCom_FighterManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        BattleFighter fighter = component.GetFighter(actorID);
        if (fighter != null)
        {
            RealTimeBuffer newBuffer = new RealTimeBuffer();
            newBuffer.Init(bufferEntry, actorID, fighter, component.GetFighter(casterID));
            fighter.AddBuffer(newBuffer);
        }
    }

    private void EventBattleFinish(bool isWin, bool isTimeOut)
    {
        if (this.isRuning)
        {
            base.battleGameData.timeScale_ShowTime = 1f;
            BattleGlobal.SetShowTimeScale(1f);
            this.mAIManager.OnRegisterActorFinalData();
            if (base.battleGameData.normalGameType == BattleNormalGameType.WorldBoss)
            {
                BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(BattleGlobal.FighterNumberMax);
                if (fighter != null)
                {
                    base.battleGameData.worldBossFinishedHP = (long) fighter.HP;
                    base.battleGameData.worldBossOverHurtHP = 0L;
                    AiActor actorById = this.mAIManager.GetActorById(fighter.PosIndex);
                    if (actorById != null)
                    {
                        base.battleGameData.worldBossOverHurtHP = (long) actorById.overHpHurt;
                    }
                }
            }
            base.battleGameData.isRealTimeBattleWin = isWin;
            this.isRuning = false;
            this.isRealBattleStarted = false;
            this.Clear();
            base.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().UnblockScene();
            Debug.Log("Finish");
            if (base.battleGameData.OnMsgBattleRunningChange != null)
            {
                base.battleGameData.OnMsgBattleRunningChange(false);
            }
            this.skillManager.Clear();
            BattleSecurityManager.Instance.RegisterBattleRemainTime(base.battleGameData.GetBattletimeInt());
            BattleSecurityManager.Instance.RegisterBattlePhaseResult(isWin);
            BattleSecurityManager.Instance.RegisterPhaseEndTime();
            if (isWin)
            {
                if (base.battleGameData.CurBattlePhase == (base.battleGameData.phaseNumber - 1))
                {
                    base.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().BeginBattleFinished(true, isTimeOut);
                }
                else
                {
                    base.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().BeginPhaseFinishing();
                }
            }
            else
            {
                base.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().BeginBattleFinished(false, isTimeOut);
            }
        }
    }

    private void EventBreakSkill(int casterID, int skillID)
    {
        this.skillManager.BreakSkill(skillID);
    }

    private void EventCastNewSkill(CastNewSkill_Input param)
    {
        this.skillManager.CastNewSkill(param.skillEntry, param.skillUniId, param.casterID, param.mainTargetList, param.offset);
    }

    private void EventDelBuff(int casterID, int bufferEntry)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(casterID);
        if (fighter != null)
        {
            fighter.RemoveBuffer(bufferEntry);
        }
    }

    private void EventFaceToActor(int actorID, int targetID)
    {
        BattleCom_FighterManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        BattleFighter fighter = component.GetFighter(actorID);
        BattleFighter fighter2 = component.GetFighter(targetID);
        if ((fighter != null) && (fighter2 != null))
        {
            fighter.moveControler.StartTurnToPos(fighter2.GetAnimObj().transform.position);
        }
    }

    private void EventMoveActorToPos(int index, Vector3 pos, float radius, int targetIdx)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(index);
        if ((fighter != null) && !fighter.IsDead())
        {
            fighter.moveControler.StartMoveToPos(pos, radius);
        }
    }

    private void EventOnActorDead(int actorID)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(actorID);
        if (fighter != null)
        {
            fighter.SetDead();
        }
    }

    private void EventOnActorHPMaxHPCHange(int actorID, long hp, long maxHp)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(actorID);
        if (fighter != null)
        {
            fighter.SetMaxHP(maxHp);
            fighter.SetHp(hp);
        }
    }

    private void EventOnActorMoveSpeedChange(int actorID, float moveSpeed)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(actorID);
        if (fighter != null)
        {
            fighter.moveControler.SetMoveSpeed(moveSpeed);
        }
    }

    private void EventOnDoSkillCasting(int skillID, string effectName, List<int> targetList)
    {
        this.skillManager.EventOnDoSkillCasting(skillID, effectName, targetList);
    }

    private void EventOnStartBattle()
    {
        this.isRealBattleStarted = true;
        if (base.battleGameData.IsBossBattle)
        {
            base.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().EndAttachToPlayer();
        }
        else
        {
            base.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().EndAttachToPlayerAndSetMiddlePos();
        }
        if (base.battleGameData.OnMsgStartRealBattle != null)
        {
            base.battleGameData.OnMsgStartRealBattle();
        }
    }

    private void EventRevive(int actorID, long _hp, int _energy)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(actorID);
        if (fighter != null)
        {
            fighter.Revive(_hp, _energy);
        }
    }

    private void EventSetBuffEffectActive(int actorId, int bufferEntry, string name, bool active)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(actorId);
        if (fighter != null)
        {
            fighter.SetBufferEffectActive(bufferEntry, name, active);
        }
    }

    private void EventStopActorMove(int index)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(index);
        if (fighter != null)
        {
            fighter.moveControler.StopMove();
        }
    }

    private void EventSummon(int actorIdx, int monsterEntry, Vector3 pos)
    {
        GameObject obj2 = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().createMonsterFighter(monsterEntry, actorIdx, BattleGlobalFunc.CreateDetailInfoOnMonster(monsterEntry), actorIdx);
        if (obj2 != null)
        {
            obj2.GetComponent<BattleFighter>().SetPosition(pos);
            Vector3 sceneFighterDirByPhase = base.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
            if (actorIdx > BattleGlobal.FighterNumberMax)
            {
                sceneFighterDirByPhase = -sceneFighterDirByPhase;
            }
            obj2.transform.rotation = Quaternion.LookRotation(sceneFighterDirByPhase);
        }
        AiActor actorById = this.mAIManager.GetActorById(actorIdx);
        if (actorById != null)
        {
            actorById.IsAuto = false;
        }
    }

    private void EvetntOnSkillEffectResult(SkillEffectResult result)
    {
        if (result != null)
        {
            BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(result.targetID);
            if (fighter != null)
            {
                fighter.ProcessSkillResult(result);
            }
        }
    }

    private void EvetntOnSubShowEffect(int skillID, string effectName, int targetID)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(targetID);
        if (fighter != null)
        {
            fighter.OnSubShowEffect(effectName);
        }
    }

    public void FixedUpdate()
    {
        if (this.isRuning)
        {
            if (!this.isRealBattleStarted)
            {
                base.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().BeginAttachToPlayerAndTeamDir(false);
            }
            if ((BattleGlobal.GetShowTimeScale() >= 1f) && (!BattleSceneStarter.G_isTestEnable || !BattleSceneStarter.G_isTestSkill))
            {
                if (!GuideSimulate_Battle.sim_mode)
                {
                    base.battleGameData.OnBattletime(Time.deltaTime);
                    if (base.battleGameData.GetBattleRemainTime() <= 0f)
                    {
                        this.OnTimeOut();
                    }
                }
                if (this.mAIManager != null)
                {
                    this.mAIManager.Tick(Time.deltaTime);
                }
            }
            this.skillManager.SkillTick();
        }
        if (this.mAIManager != null)
        {
            this.mAIManager.SystemTick();
        }
    }

    public Vector3 GetActorHeadDir(int actorID)
    {
        if (this.mAIManager != null)
        {
            AiActor actorById = this.mAIManager.GetActorById(actorID);
            if (actorById != null)
            {
                return actorById.HeadDirection;
            }
        }
        return Vector3.one;
    }

    public Vector3 GetActorPos(int index)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(index);
        if (fighter != null)
        {
            return fighter.moveControler.GetPosition();
        }
        Debug.LogWarning("Get Pos Error " + index.ToString());
        return Vector3.zero;
    }

    public AiActor GetAiActor(int actorIndex)
    {
        return this.mAIManager.GetActorById(actorIndex);
    }

    public BattleFighter GetAttackHeadFighter()
    {
        if (this.mAIManager != null)
        {
            int attackerHeadActor = this.mAIManager.GetAttackerHeadActor();
            return base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(attackerHeadActor);
        }
        return null;
    }

    public BattleFighter GetAttackLastFighter()
    {
        if (this.mAIManager != null)
        {
            int attackerLastActor = this.mAIManager.GetAttackerLastActor();
            return base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(attackerLastActor);
        }
        return null;
    }

    public RealTimeBuffer GetBuffer(int actorID, int bufferEntry)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(actorID);
        if (fighter != null)
        {
            return fighter.GetBufferByEntry(bufferEntry);
        }
        return null;
    }

    public BattleFighter GetDefenderHeadFighter()
    {
        if (this.mAIManager != null)
        {
            int defenderHeadActor = this.mAIManager.GetDefenderHeadActor();
            return base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(defenderHeadActor);
        }
        return null;
    }

    public BattleFighter GetDefenderLastFighter()
    {
        if (this.mAIManager != null)
        {
            int defenderLastActor = this.mAIManager.GetDefenderLastActor();
            return base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(defenderLastActor);
        }
        return null;
    }

    private static void InitActorInfoesHelperFunc(BattleFighter actor, int index, ActorDataList actorDataList, int listIndexOffset)
    {
        if (actor != null)
        {
            actorDataList.ActorList[index - listIndexOffset] = actor.detailActor;
            actorDataList.ServerIndexList[index - listIndexOffset] = actor.ServerIdx;
        }
    }

    public bool IsActorCanUseActiveSkill(int actorID)
    {
        return ((this.mAIManager != null) && this.mAIManager.IsActorCanUseActiveSkill(actorID));
    }

    public bool IsActorEnergyFull(int actorID)
    {
        return ((this.mAIManager != null) && this.mAIManager.IsActorSkillEnergyFull(actorID));
    }

    public bool IsActorHpLow(int actorID)
    {
        return ((this.mAIManager != null) && this.mAIManager.IsActorHpLow(actorID));
    }

    public bool IsAnySkillRunning()
    {
        return this.skillManager.IsAnySkillRunning();
    }

    public void OnActorAttackAwayFinish(int actorID)
    {
        if (this.eventInterface.OnActorAttackAwayFinish != null)
        {
            this.eventInterface.OnActorAttackAwayFinish(actorID);
        }
    }

    public void OnActorMoveFinish(int fighterIndex)
    {
        if (this.eventInterface.OnMoveFinish != null)
        {
            this.eventInterface.OnMoveFinish(fighterIndex);
        }
    }

    public override void OnCreateInit()
    {
        this.skillManager.battleGameData = base.battleGameData;
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgPhaseStartFinish = (System.Action) Delegate.Combine(battleGameData.OnMsgPhaseStartFinish, new System.Action(this.StartBattleRuntime));
        BattleData data2 = base.battleGameData;
        data2.OnMsgSkillShowTimeClean = (System.Action) Delegate.Combine(data2.OnMsgSkillShowTimeClean, new System.Action(this.OnMsgSkillShowTimeClean));
        BattleData data3 = base.battleGameData;
        data3.OnMsgPhaseChange = (System.Action) Delegate.Combine(data3.OnMsgPhaseChange, new System.Action(this.OnMsgPhaseChange));
        BattleData data4 = base.battleGameData;
        data4.OnMsgPlayerMoveFinished = (Action<int>) Delegate.Combine(data4.OnMsgPlayerMoveFinished, new Action<int>(this.OnActorMoveFinish));
        BattleData data5 = base.battleGameData;
        data5.OnMsgLeave = (System.Action) Delegate.Combine(data5.OnMsgLeave, new System.Action(this.OnMsgLeave));
        BattleData data6 = base.battleGameData;
        data6.OnMsgAutoChange = (Action<bool>) Delegate.Combine(data6.OnMsgAutoChange, new Action<bool>(this.OnMsgAutoChange));
        BattleData data7 = base.battleGameData;
        data7.OnMsgGridGameFinishOneBattle = (Action<bool, bool, BattleNormalGameResult>) Delegate.Combine(data7.OnMsgGridGameFinishOneBattle, new Action<bool, bool, BattleNormalGameResult>(this.OnMsgGridGameFinishOneBattle));
    }

    private void OnMsgAutoChange(bool isAuto)
    {
    }

    private void OnMsgGridGameFinishOneBattle(bool isWin, bool isBreak, BattleNormalGameResult result)
    {
        ObjectManager.HideTempObj();
        this.Clear();
        this.skillManager.Clear();
        this.isRuning = false;
        AiDataKeepManager.GetInstance().ClearData();
        AiFragmentManager.GetInstance().ClearData();
    }

    private void OnMsgLeave()
    {
        this.skillManager.Clear();
        this.isRuning = false;
        this.mAIManager = null;
        AiDataKeepManager.GetInstance().ClearData();
        AiFragmentManager.GetInstance().ClearData();
    }

    private void OnMsgPhaseChange()
    {
        this.skillManager.Clear();
    }

    private void OnMsgSkillShowTimeClean()
    {
        this.skillManager.OnMsgSkillShowTimeClean();
    }

    public void OnSkillAICastFinish(int skillID)
    {
        this.mAIManager.OnSkillAICastFinish(skillID);
    }

    public void OnSkillAICastStart(int skillID)
    {
        this.mAIManager.OnSkillAICastStart(skillID);
    }

    public void OnSkillFinish(int casterID, int skillID)
    {
        if (this.eventInterface.OnSkillFinish != null)
        {
            this.eventInterface.OnSkillFinish(casterID, skillID);
        }
    }

    private void OnTimeOut()
    {
        if (!BattleSceneStarter.G_isTestEnable || !BattleSceneStarter.G_isTestSkill)
        {
            this.EventBattleFinish(false, true);
        }
    }

    public void PauseGame()
    {
        this.showTimeObjIDs.Clear();
        TimeManager.GetInstance().GetShowTimeObj(this.showTimeObjIDs);
        TimeManager.GetInstance().ClearShowTimeObj();
        base.battleGameData.timeScale_ShowTime = BattleGlobal.PauseTimeScale;
    }

    public bool QuestCastSkill(int fighterIndex)
    {
        if (!base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(fighterIndex).IsDead())
        {
            if (this.mAIManager == null)
            {
                return false;
            }
            if (this.eventInterface.OnQuestCastSkill != null)
            {
                this.eventInterface.OnQuestCastSkill(fighterIndex);
                return true;
            }
        }
        return false;
    }

    public void ResumeGame()
    {
        TimeManager.GetInstance().ClearShowTimeObj();
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = obj => TimeManager.GetInstance().AddShowTimeObj(obj);
        }
        this.showTimeObjIDs.ForEach(<>f__am$cache8);
        base.battleGameData.timeScale_ShowTime = 1f;
    }

    public void SendSkillState(int casterID, int skillID, RealTimeSkillState state)
    {
        if ((state == RealTimeSkillState.Finished) && (this.eventInterface.OnSkillFinish != null))
        {
            this.eventInterface.OnSkillFinish(casterID, skillID);
        }
    }

    public void StartBattleRuntime()
    {
        <StartBattleRuntime>c__AnonStoreyE1 ye = new <StartBattleRuntime>c__AnonStoreyE1 {
            <>f__this = this
        };
        this.Clear();
        this.isRuning = true;
        if (base.battleGameData.CurBattlePhase == 0)
        {
            AiSkillFunction.SetRandomSeed(base.battleGameData.randomSeed);
        }
        BattleSecurityManager.Instance.RegisterBattleStartTime();
        BattleCom_FighterManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        ye.attackerData = new ActorDataList();
        component.DoToPlayerAllFighter(new Action<BattleFighter, int>(ye.<>m__45));
        ye.defenderData = new ActorDataList();
        component.DoToMonsterAllFighter(new Action<BattleFighter, int>(ye.<>m__46));
        this.skillManager.Clear();
        this.mAIManager = new AiManager();
        Vector3 sceneFighterDirByPhase = base.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
        this.mAIManager.Init(ye.attackerData, ye.defenderData, this, sceneFighterDirByPhase);
        component.DoToPlayerAllFighter(new Action<BattleFighter, int>(ye.<>m__47));
        component.DoToAllFighter(new Action<BattleFighter, int>(ye.<>m__48));
        if ((base.battleGameData.normalGameType == BattleNormalGameType.WorldBoss) && !base.battleGameData.worldBossIsKilled)
        {
            AiActor actorById = this.mAIManager.GetActorById(BattleGlobal.FighterNumberMax);
            if (actorById != null)
            {
                actorById.IsCanBeKilled = false;
            }
        }
        this.mAIManager.EventCastNewSkill = (Action<CastNewSkill_Input>) Delegate.Combine(this.mAIManager.EventCastNewSkill, new Action<CastNewSkill_Input>(this.EventCastNewSkill));
        this.mAIManager.EventMoveActorToPos = (Action<int, Vector3, float, int>) Delegate.Combine(this.mAIManager.EventMoveActorToPos, new Action<int, Vector3, float, int>(this.EventMoveActorToPos));
        this.mAIManager.EventStopActorMove = (Action<int>) Delegate.Combine(this.mAIManager.EventStopActorMove, new Action<int>(this.EventStopActorMove));
        this.mAIManager.EventBattleFinish = (Action<bool, bool>) Delegate.Combine(this.mAIManager.EventBattleFinish, new Action<bool, bool>(this.EventBattleFinish));
        this.mAIManager.EventAddBuff = (Action<int, int, float, int>) Delegate.Combine(this.mAIManager.EventAddBuff, new Action<int, int, float, int>(this.EventAddBuff));
        this.mAIManager.EventDelBuff = (Action<int, int>) Delegate.Combine(this.mAIManager.EventDelBuff, new Action<int, int>(this.EventDelBuff));
        this.mAIManager.EventSetBufferEffectActive = (Action<int, int, string, bool>) Delegate.Combine(this.mAIManager.EventSetBufferEffectActive, new Action<int, int, string, bool>(this.EventSetBuffEffectActive));
        this.mAIManager.EventSummon = (Action<int, int, Vector3>) Delegate.Combine(this.mAIManager.EventSummon, new Action<int, int, Vector3>(this.EventSummon));
        this.mAIManager.EventBreakSkill = (Action<int, int>) Delegate.Combine(this.mAIManager.EventBreakSkill, new Action<int, int>(this.EventBreakSkill));
        this.mAIManager.EventRevive = (Action<int, long, int>) Delegate.Combine(this.mAIManager.EventRevive, new Action<int, long, int>(this.EventRevive));
        this.mAIManager.EventFaceToActor = (Action<int, int>) Delegate.Combine(this.mAIManager.EventFaceToActor, new Action<int, int>(this.EventFaceToActor));
        this.mAIManager.EventOnActorHPMaxHPCHange = (Action<int, long, long>) Delegate.Combine(this.mAIManager.EventOnActorHPMaxHPCHange, new Action<int, long, long>(this.EventOnActorHPMaxHPCHange));
        this.mAIManager.EvetntOnSubShowEffect = (Action<int, string, int>) Delegate.Combine(this.mAIManager.EvetntOnSubShowEffect, new Action<int, string, int>(this.EvetntOnSubShowEffect));
        this.mAIManager.EvetntOnSkillEffectResult = (Action<SkillEffectResult>) Delegate.Combine(this.mAIManager.EvetntOnSkillEffectResult, new Action<SkillEffectResult>(this.EvetntOnSkillEffectResult));
        this.mAIManager.EventOnDoSkillCasting = (Action<int, string, List<int>>) Delegate.Combine(this.mAIManager.EventOnDoSkillCasting, new Action<int, string, List<int>>(this.EventOnDoSkillCasting));
        this.mAIManager.EventOnActorMoveSpeedChange = (Action<int, float>) Delegate.Combine(this.mAIManager.EventOnActorMoveSpeedChange, new Action<int, float>(this.EventOnActorMoveSpeedChange));
        this.mAIManager.EventOnActorDead = (Action<int>) Delegate.Combine(this.mAIManager.EventOnActorDead, new Action<int>(this.EventOnActorDead));
        this.mAIManager.EventOnStartBattle = (System.Action) Delegate.Combine(this.mAIManager.EventOnStartBattle, new System.Action(this.EventOnStartBattle));
        base.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().BlockScene();
        if (this.isTestAutoFinish)
        {
            base.StartCoroutine(this.Test());
        }
        base.battleGameData.ResetBattletime();
        if (!BattleSceneStarter.G_isTestEnable || !BattleSceneStarter.G_isTestSkill)
        {
            this.mAIManager.Ready();
        }
        if (base.battleGameData.OnMsgBattleRunningChange != null)
        {
            base.battleGameData.OnMsgBattleRunningChange(true);
        }
    }

    public void StopActorMove(int index)
    {
        BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(index);
        if (fighter != null)
        {
            fighter.moveControler.StopMove();
        }
    }

    [DebuggerHidden]
    private IEnumerator Test()
    {
        return new <Test>c__Iterator9 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <StartBattleRuntime>c__AnonStoreyE1
    {
        internal BattleCom_Runtime <>f__this;
        internal ActorDataList attackerData;
        internal ActorDataList defenderData;

        internal void <>m__45(BattleFighter arg1, int arg2)
        {
            BattleCom_Runtime.InitActorInfoesHelperFunc(arg1, arg2, this.attackerData, 0);
        }

        internal void <>m__46(BattleFighter arg1, int arg2)
        {
            BattleCom_Runtime.InitActorInfoesHelperFunc(arg1, arg2, this.defenderData, BattleGlobal.FighterNumberMax);
        }

        internal void <>m__47(BattleFighter arg1, int arg2)
        {
            AiActor actorById = this.<>f__this.mAIManager.GetActorById(arg2);
            if (actorById != null)
            {
                actorById.IsAuto = BattleCom_Runtime.IsForceAuto;
            }
        }

        internal void <>m__48(BattleFighter arg1, int arg2)
        {
            AiActor actorById = this.<>f__this.mAIManager.GetActorById(arg2);
            if (actorById != null)
            {
                actorById.RangeScale = arg1.Scale;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Test>c__Iterator9 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleCom_Runtime <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSeconds(5f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.EventBattleFinish(true, false);
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    public class EventInterface
    {
        public Action<int> OnActorAttackAwayFinish;
        public Action<int> OnMoveFinish;
        public Action<int> OnQuestCastSkill;
        public Func<RealSkillCDInfo, int> OnQuestGetSkillCDInfo;
        public Action<int, int> OnSetSkillCasting;
        public Action<int, int> OnSkillFinish;
        public Action<int, int> OnSubSkillFinish;

        public void Clear()
        {
            this.OnSetSkillCasting = null;
            this.OnSkillFinish = null;
            this.OnSubSkillFinish = null;
            this.OnQuestCastSkill = null;
            this.OnQuestGetSkillCDInfo = null;
            this.OnMoveFinish = null;
            this.OnActorAttackAwayFinish = null;
        }
    }
}

