using Battle;
using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class BattleSecurityManager
{
    private int battleDataIndex;
    private List<BattleSecurityDataClient> btlSecDtaList;
    private int curBattlePhase;
    private static BattleSecurityManager instance;

    private BattleSecurityManager()
    {
    }

    public void AddEnergyChangeData(int id, HitEnergyType energyType, int changeValue)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].AddEngergyData(id, energyType, changeValue);
        }
    }

    public void AddHpChangeData(int id, ChangeHpType hpType, int changeValue)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].AddHpChangeData(id, hpType, changeValue);
        }
    }

    public void AddNewBattlePhase(int battlePhase, BattleGameType baseGameType, BattleNormalGameType normalType)
    {
        BattleSecurityDataClient item = new BattleSecurityDataClient(battlePhase, baseGameType, normalType);
        if (this.btlSecDtaList == null)
        {
            this.Init();
        }
        this.btlSecDtaList.Add(item);
        this.curBattlePhase = battlePhase;
    }

    public void Clear()
    {
        this.btlSecDtaList.Clear();
    }

    public void EndPause()
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].AddPauseEndCondition();
        }
    }

    public int GetBattleDataIndex()
    {
        return this.battleDataIndex++;
    }

    public int GetBattleTotalDemage(bool isMyTeam)
    {
        int num = 0;
        int count = this.btlSecDtaList.Count;
        foreach (FighterData data in this.btlSecDtaList[count - 1].FighterDataList)
        {
            if (isMyTeam && (data.RealID < AiDef.MAX_ACTOR_EACH_SIDE))
            {
                num += data.TotalDemage;
            }
            else if (!isMyTeam && (data.RealID >= AiDef.MAX_ACTOR_EACH_SIDE))
            {
                num += data.TotalDemage;
            }
        }
        return num;
    }

    public List<FighterBattleData> GetFighterBattleData()
    {
        List<FighterBattleData> list = new List<FighterBattleData>();
        int count = this.btlSecDtaList.Count;
        <GetFighterBattleData>c__AnonStorey14E storeye = new <GetFighterBattleData>c__AnonStorey14E();
        using (List<FighterData>.Enumerator enumerator = this.btlSecDtaList[count - 1].FighterDataList.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                storeye.fighterData = enumerator.Current;
                if (storeye.fighterData.IsSummoned)
                {
                    FighterBattleData data = list.Find(new Predicate<FighterBattleData>(storeye.<>m__10F));
                    if (data != null)
                    {
                        data.TotalDemage += storeye.fighterData.TotalDemage;
                    }
                }
                else
                {
                    FighterBattleData item = new FighterBattleData {
                        Id = storeye.fighterData.RealID,
                        CardEntry = storeye.fighterData.CardEntry,
                        IsCard = storeye.fighterData.IsCard,
                        IsActor = storeye.fighterData.IsActor,
                        TotalDemage = storeye.fighterData.TotalDemage,
                        Quality = storeye.fighterData.Quality,
                        StarLevel = storeye.fighterData.StarLevel
                    };
                    list.Add(item);
                }
            }
        }
        return list;
    }

    public BattleSecurityData GetSecrityData(int phase)
    {
        BattleSecurityData data = new BattleSecurityData();
        if ((phase < 0) || (phase >= this.btlSecDtaList.Count))
        {
            data.cur_battle_phase = -1;
            return data;
        }
        data.battle_time = this.btlSecDtaList[phase].BattleTotalTime;
        data.cur_battle_phase = phase;
        data.is_phase_win = this.btlSecDtaList[phase].IsPhaseWin;
        data.total_pause_time = this.btlSecDtaList[phase].PuaseTotalTime;
        data.phase_time = this.btlSecDtaList[phase].PhaseTotalTimeInSec;
        foreach (CastSkillData data2 in this.btlSecDtaList[phase].CastSkillDataList)
        {
            FastBuf.CastSkillData item = new FastBuf.CastSkillData {
                caster_id = (short) data2.CasterId,
                target_id = (short) data2.TargetId,
                hit_type = (byte) data2.HitType,
                skill_entry = (short) data2.SkillEntry,
                sub_skill_entry = (short) data2.SubSkillEntry,
                value_change = data2.ValueChange,
                skill_id = data2.SkillId,
                time = (short) data2.CastTime,
                seq = (short) data2.DataIdx,
                other_type = (byte) data2.OtherHitType,
                other_value_change = data2.OtherValueChange
            };
            int num = 0;
            variable_2_config _config = ConfigMgr.getInstance().getByEntry<variable_2_config>(0);
            if (_config != null)
            {
                num = _config.max_security_skill_data_to_svr;
            }
            if (data.cast_skill_data_list.Count >= num)
            {
                break;
            }
            data.cast_skill_data_list.Add(item);
        }
        foreach (BaseSkillData data4 in this.btlSecDtaList[phase].BaseSkillDataList)
        {
            MainSkillData data5 = new MainSkillData {
                caster_id = (short) data4.CasterID,
                seq = (short) data4.Index,
                skill_entry = (short) data4.SkillEntry,
                time = data4.CastTime
            };
            int num2 = 0;
            variable_2_config _config2 = ConfigMgr.getInstance().getByEntry<variable_2_config>(0);
            if (_config2 != null)
            {
                num2 = _config2.max_security_main_skill_data_to_svr;
            }
            if (data.main_skill_data_list.Count >= num2)
            {
                break;
            }
            data.main_skill_data_list.Add(data5);
        }
        foreach (BuffData data6 in this.btlSecDtaList[phase].BuffDataList)
        {
            FastBuf.BuffData data7 = new FastBuf.BuffData {
                buff_id = data6.BuffId,
                buff_entry = (short) data6.Entry,
                remain_time = data6.GetBuffRemainTime(),
                time = data6.GetStartTime(),
                caster_id = (short) data6.CasterId,
                target_id = (short) data6.TargetId,
                skill_entry = (short) data6.SkillEntry,
                seq = (short) data6.DataIdx,
                endseq = (short) data6.DataEndIdx,
                sub_skill_entry = (short) data6.subSkillEntry
            };
            int num3 = 0;
            variable_2_config _config3 = ConfigMgr.getInstance().getByEntry<variable_2_config>(0);
            if (_config3 != null)
            {
                num3 = _config3.max_security_buff_data_to_svr;
            }
            if (data.buff_data_list.Count >= num3)
            {
                break;
            }
            data.buff_data_list.Add(data7);
        }
        foreach (EnergyData data8 in this.btlSecDtaList[phase].EnergyDataList)
        {
            FastBuf.EnergyData data9 = new FastBuf.EnergyData {
                actor_Id = (byte) data8.ActorId,
                energy_type = (byte) data8.EnergyType,
                change_value = (short) data8.ChangeValue,
                time = data8.GetStartTime(),
                seq = (short) data8.DataIdx
            };
            int num4 = 0;
            variable_2_config _config4 = ConfigMgr.getInstance().getByEntry<variable_2_config>(0);
            if (_config4 != null)
            {
                num4 = _config4.max_security_energy_change_to_svr;
            }
            if (data.energy_change_list.Count >= num4)
            {
                break;
            }
            data.energy_change_list.Add(data9);
        }
        foreach (HpChangeData data10 in this.btlSecDtaList[phase].HpChangeDataList)
        {
            if (data10.HpType == ChangeHpType.HpType_ByAiFate)
            {
                HPChangeData data11 = new HPChangeData {
                    actor_Id = (byte) data10.ActorId,
                    hp_change_type = (byte) data10.HpType,
                    change_value = (short) data10.ChangeValue,
                    seq = (short) data10.DataIdx
                };
                int num5 = 0;
                variable_2_config _config5 = ConfigMgr.getInstance().getByEntry<variable_2_config>(0);
                if (_config5 != null)
                {
                    num5 = _config5.max_security_hp_data_to_svr;
                }
                if (data.hp_change_data_list.Count >= num5)
                {
                    break;
                }
                data.hp_change_data_list.Add(data11);
            }
        }
        foreach (FighterData data12 in this.btlSecDtaList[phase].FighterDataList)
        {
            FastBuf.FighterData data13 = new FastBuf.FighterData {
                fighter_id = data12.FighterId,
                cur_hp = data12.CurHp,
                cur_energy = data12.CurEnergy,
                alive_time = data12.GetAliveTime(),
                init_hp = data12.InitHp,
                init_energy = data12.InitEnergy,
                time = data12.GetStartTime(),
                endseq = (short) data12.EndSeq,
                is_summon = data12.IsSummoned
            };
            int num6 = 0;
            variable_2_config _config6 = ConfigMgr.getInstance().getByEntry<variable_2_config>(0);
            if (_config6 != null)
            {
                num6 = _config6.max_security_fighter_data_to_svr;
            }
            if (data.figher_data_list.Count >= num6)
            {
                return data;
            }
            data.figher_data_list.Add(data13);
        }
        return data;
    }

    public BattleSecurityDataList GetSecurityDataList()
    {
        BattleSecurityDataList list = new BattleSecurityDataList {
            security_data_list = new List<BattleSecurityData>()
        };
        for (int i = 0; i < this.btlSecDtaList.Count; i++)
        {
            list.security_data_list.Add(this.GetSecrityData(i));
        }
        return list;
    }

    public void Init()
    {
        this.btlSecDtaList = new List<BattleSecurityDataClient>();
        this.battleDataIndex = 0;
    }

    public void LogBattleData()
    {
        foreach (BattleSecurityDataClient client in this.btlSecDtaList)
        {
            client.Log();
        }
    }

    public void RegisterBaseSkill(int skillEntry, int casterId)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].AddNewBaseSkill(skillEntry, casterId);
        }
    }

    public void RegisterBattlePhaseResult(bool isWin)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            BattleSecurityDataClient client = this.btlSecDtaList[this.curBattlePhase];
            client.IsPhaseWin = isWin;
        }
    }

    public void RegisterBattleRemainTime(int totalTime)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            BattleSecurityDataClient client = this.btlSecDtaList[this.curBattlePhase];
            client.BattleTotalTime = totalTime;
        }
    }

    public void RegisterBattleStartTime()
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            BattleSecurityDataClient client = this.btlSecDtaList[this.curBattlePhase];
        }
    }

    public void RegisterBuffData(string buffId, int buffEntry, int casterId, int targetId, int skillEntry, int subSkillEntry)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].AddBuffData(buffId, buffEntry, casterId, targetId, skillEntry, subSkillEntry);
        }
    }

    public void RegisterCasterSkillData(string skillId, int skillEntry, int subSkillEntry, int targetId, int casterId, SkillHitType skillhitType, int changeValue, SkillHitType otherHitType, int otherValueChange, int casterHp, int targetHp, int targetEnergy, int casterEnergy, int casterSpellPierce, int casterPhysicsPierce, int casterAttack, int casterCriDmg, int healValue, int healMod, int subSkillIdx)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].AddNewSkillData(skillId, skillEntry, subSkillEntry, targetId, casterId, skillhitType, changeValue, otherHitType, otherValueChange, casterHp, targetHp, targetEnergy, casterEnergy, casterSpellPierce, casterPhysicsPierce, casterAttack, casterCriDmg, healValue, healMod, subSkillIdx);
        }
    }

    public void RegisterFigherData(int id, long curHp, int curEnergy, int realID, bool isSummoned, int cardEntry, short qualityLevel, short starLevel, bool isCard, bool isActor, int summonerId = -1)
    {
        if ((this.curBattlePhase < this.btlSecDtaList.Count) && (id != -1))
        {
            this.btlSecDtaList[this.curBattlePhase].AddFighterData(id, curHp, curEnergy, realID, isSummoned, cardEntry, qualityLevel, starLevel, isCard, isActor, summonerId);
        }
    }

    public void RegisterPhaseEndTime()
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            BattleSecurityDataClient client = this.btlSecDtaList[this.curBattlePhase];
            client.PhaseEndTime = TimeMgr.Instance.ServerDateTime;
        }
    }

    public void RegisterPhaseStartTime()
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            BattleSecurityDataClient client = this.btlSecDtaList[this.curBattlePhase];
            client.PhaseStartTime = TimeMgr.Instance.ServerDateTime;
        }
    }

    public void SetBuffEnd(string buffId)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].SetBuffDataEnd(buffId);
        }
    }

    public void SetFigherData(int id, long curHp, int curEnergy)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].SetFighterData(id, curHp, curEnergy);
        }
    }

    public void SetFigherDead(int id)
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].SetFigherDead(id);
        }
    }

    public void StartPause()
    {
        if (this.curBattlePhase < this.btlSecDtaList.Count)
        {
            this.btlSecDtaList[this.curBattlePhase].AddPauseStartCondition();
        }
    }

    public static BattleSecurityManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BattleSecurityManager();
            }
            return instance;
        }
    }

    [CompilerGenerated]
    private sealed class <GetFighterBattleData>c__AnonStorey14E
    {
        internal FighterData fighterData;

        internal bool <>m__10F(FighterBattleData mn)
        {
            return (mn.Id == this.fighterData.summonerId);
        }
    }
}

