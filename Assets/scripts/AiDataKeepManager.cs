using Battle;
using System;
using System.Collections.Generic;

public class AiDataKeepManager
{
    private static AiDataKeepManager _instance;
    private List<AiBuffKeepData> buffData = new List<AiBuffKeepData>();

    public void CheckNeedKeepBuff(AiActor actor)
    {
        foreach (AiBuff buff in actor.GetBuffs())
        {
            if (buff.isCanPastToNextPhase())
            {
                AiBuffKeepData item = new AiBuffKeepData {
                    actorID = actor.Id,
                    entry = buff.Entry,
                    skillLV = (int) buff.skillLV,
                    skillHitLV = (int) buff.skillHitLV,
                    skillEntry = buff.SkillEntry
                };
                this.buffData.Add(item);
            }
        }
    }

    public void ClearData()
    {
        this.buffData.Clear();
    }

    public static AiDataKeepManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new AiDataKeepManager();
        }
        return _instance;
    }

    public void RestoreBuff(AiActor actor)
    {
        List<AiBuffKeepData> list = new List<AiBuffKeepData>();
        foreach (AiBuffKeepData data in this.buffData)
        {
            if (data.actorID == actor.Id)
            {
                actor.AddBuff(data.entry, actor.m_aiManager.Time, actor.Id, data.skillLV, data.skillHitLV, false, data.skillEntry, data.subSkillEntry);
                list.Add(data);
            }
        }
        list.ForEach(obj => this.buffData.Remove(obj));
    }

    public class AiBuffKeepData
    {
        public int actorID;
        public int entry;
        public int skillEntry;
        public int skillHitLV;
        public int skillLV;
        public int subSkillEntry;
    }
}

