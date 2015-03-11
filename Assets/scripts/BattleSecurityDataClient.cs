using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleSecurityDataClient
{
    public List<BaseSkillData> BaseSkillDataList;
    public List<BuffData> BuffDataList;
    private List<CastSkillData> castSkillDataList;
    public int DataIdx;
    public List<EnergyData> EnergyDataList;
    public List<FighterData> FighterDataList;
    private List<GamePauseCondition> gameConditionList;
    public List<HpChangeData> HpChangeDataList;
    private static BattleSecurityDataClient instanceEmpty;

    public BattleSecurityDataClient(int battlePhase, BattleGameType baseGameType, BattleNormalGameType normalType)
    {
        this.CurBattlePhase = battlePhase;
        this.BaseGameType = baseGameType;
        this.NormalType = normalType;
        this.castSkillDataList = new List<CastSkillData>();
        this.gameConditionList = new List<GamePauseCondition>();
        this.BuffDataList = new List<BuffData>();
        this.FighterDataList = new List<FighterData>();
        this.EnergyDataList = new List<EnergyData>();
        this.HpChangeDataList = new List<HpChangeData>();
        this.BaseSkillDataList = new List<BaseSkillData>();
    }

    public void AddBuffData(string buffId, int buffEntry, int casterId, int targetId, int skillEntry, int subSkillEntry)
    {
        BuffData item = new BuffData(buffId, buffEntry, casterId, targetId, skillEntry, subSkillEntry);
        this.BuffDataList.Add(item);
        item.DataIdx = BattleSecurityManager.Instance.GetBattleDataIndex();
    }

    public void AddEngergyData(int actorId, HitEnergyType hitEnergyType, int changeValue)
    {
        EnergyData item = new EnergyData(actorId, hitEnergyType, changeValue) {
            DataIdx = BattleSecurityManager.Instance.GetBattleDataIndex()
        };
        this.EnergyDataList.Add(item);
    }

    public void AddFighterData(int id, long curHp, int curEnergy, int realID, bool isSummoned, int cardEntry, short qualityLevel, short starLevel, bool isCard, bool isActor, int summonerId)
    {
        FighterData item = new FighterData(id, curHp, curEnergy, realID, isSummoned, cardEntry, qualityLevel, starLevel, isCard, isActor, summonerId);
        this.FighterDataList.Add(item);
    }

    public void AddHpChangeData(int actorId, ChangeHpType type, int changeValue)
    {
        HpChangeData item = new HpChangeData(actorId, type, changeValue) {
            DataIdx = BattleSecurityManager.Instance.GetBattleDataIndex()
        };
        this.HpChangeDataList.Add(item);
    }

    public void AddNewBaseSkill(int skillEntry, int casterId)
    {
        BaseSkillData item = new BaseSkillData(skillEntry, casterId) {
            Index = BattleSecurityManager.Instance.GetBattleDataIndex()
        };
        this.BaseSkillDataList.Add(item);
    }

    public void AddNewSkillData(string skillId, int skillEntry, int subSkillEntry, int targetId, int casterId, SkillHitType hitType, int valueChange, SkillHitType otherHitType, int otherValueChange, int casterHp, int targetHp, int targetEnergy, int casterEnergy, int casterSpellPierce, int casterPhysicsPierce, int casterAttack, int casterCriDmg, int healValue, int healMod, int subSkillIdx)
    {
        CastSkillData item = new CastSkillData(skillId, skillEntry, subSkillEntry, targetId, casterId, hitType, valueChange);
        this.castSkillDataList.Add(item);
        item.DataIdx = subSkillIdx;
        item.OtherHitType = otherHitType;
        item.OtherValueChange = otherValueChange;
        item.CasterHp = casterHp;
        item.TargetHp = targetHp;
        item.CasterEnergy = casterEnergy;
        item.TargetEnergy = targetEnergy;
        item.CasterSpellPierce = casterSpellPierce;
        item.CasterPhysicsPierce = casterPhysicsPierce;
        item.CasterAttack = casterAttack;
        item.CasterCriDmg = casterCriDmg;
        item.HealValue = healValue;
        item.HealMod = healMod;
        foreach (FighterData data2 in this.FighterDataList)
        {
            if (data2.FighterId.Equals(casterId))
            {
                data2.AddDemage(valueChange);
                break;
            }
        }
    }

    public void AddPauseEndCondition()
    {
        if (this.gameConditionList.Count <= 0)
        {
            Debug.LogWarning("Warning: AddPauseEndCondition : gameConditionList.Count <= 0");
        }
        else
        {
            GamePauseCondition condition = this.gameConditionList[this.gameConditionList.Count - 1];
            if (!condition.PauseStartTime.HasValue)
            {
                Debug.LogWarning("Warning: AddPauseEndCondition : pauseCondition.PauseStartTime == null");
            }
            else
            {
                condition.PauseEndTime = new DateTime?(TimeMgr.Instance.ServerDateTime);
            }
        }
    }

    public void AddPauseStartCondition()
    {
        GamePauseCondition item = new GamePauseCondition {
            PauseStartTime = new DateTime?(TimeMgr.Instance.ServerDateTime)
        };
        this.gameConditionList.Add(item);
    }

    public static BattleSecurityDataClient BattleSecurityDataEmpty()
    {
        if (instanceEmpty == null)
        {
            instanceEmpty = new BattleSecurityDataClient(-1, BattleGameType.Normal, BattleNormalGameType.Dup);
        }
        return instanceEmpty;
    }

    public void Log()
    {
    }

    public void SetBuffDataEnd(string buffId)
    {
        <SetBuffDataEnd>c__AnonStorey14B storeyb = new <SetBuffDataEnd>c__AnonStorey14B {
            buffId = buffId
        };
        BuffData data = this.BuffDataList.Find(new Predicate<BuffData>(storeyb.<>m__10C));
        if (data != null)
        {
            data.SetBuffEnd();
            data.DataEndIdx = BattleSecurityManager.Instance.GetBattleDataIndex();
        }
    }

    public void SetFigherDead(int id)
    {
        <SetFigherDead>c__AnonStorey14D storeyd = new <SetFigherDead>c__AnonStorey14D {
            id = id
        };
        FighterData data = this.FighterDataList.Find(new Predicate<FighterData>(storeyd.<>m__10E));
        if (data != null)
        {
            data.SetDead();
            data.EndSeq = BattleSecurityManager.Instance.GetBattleDataIndex();
        }
    }

    public void SetFighterData(int id, long hp, int energy)
    {
        <SetFighterData>c__AnonStorey14C storeyc = new <SetFighterData>c__AnonStorey14C {
            id = id
        };
        FighterData data = this.FighterDataList.Find(new Predicate<FighterData>(storeyc.<>m__10D));
        if (data != null)
        {
            data.SetHpAndEnergy(hp, energy);
        }
    }

    public BattleGameType BaseGameType { get; set; }

    public int BattleTotalTime { get; set; }

    public List<CastSkillData> CastSkillDataList
    {
        get
        {
            return this.castSkillDataList;
        }
    }

    public int CurBattlePhase { get; set; }

    public bool IsPhaseWin { get; set; }

    public BattleNormalGameType NormalType { get; set; }

    public DateTime PhaseEndTime { get; set; }

    public long PhaseEndTimeInSec
    {
        get
        {
            return TimeMgr.Instance.ConvertToTimeStamp(this.PhaseEndTime);
        }
    }

    public DateTime PhaseStartTime { get; set; }

    public long PhaseStartTimeInSec
    {
        get
        {
            return TimeMgr.Instance.ConvertToTimeStamp(this.PhaseStartTime);
        }
    }

    public int PhaseTotalTimeInSec
    {
        get
        {
            return (int) (this.PhaseEndTimeInSec - this.PhaseStartTimeInSec);
        }
    }

    public int PuaseTotalTime
    {
        get
        {
            int num = 0;
            foreach (GamePauseCondition condition in this.gameConditionList)
            {
                num += condition.TotalPauseTime;
            }
            return num;
        }
    }

    [CompilerGenerated]
    private sealed class <SetBuffDataEnd>c__AnonStorey14B
    {
        internal string buffId;

        internal bool <>m__10C(BuffData mn)
        {
            return mn.BuffId.Equals(this.buffId);
        }
    }

    [CompilerGenerated]
    private sealed class <SetFigherDead>c__AnonStorey14D
    {
        internal int id;

        internal bool <>m__10E(FighterData mn)
        {
            return (mn.FighterId == this.id);
        }
    }

    [CompilerGenerated]
    private sealed class <SetFighterData>c__AnonStorey14C
    {
        internal int id;

        internal bool <>m__10D(FighterData mn)
        {
            return (mn.FighterId == this.id);
        }
    }
}

