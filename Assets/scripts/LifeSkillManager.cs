using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Toolbox;

public class LifeSkillManager : XSingleton<LifeSkillManager>
{
    [CompilerGenerated]
    private static Func<NewLifeSkillCardState, bool> <>f__am$cache7;
    [CompilerGenerated]
    private static Func<NewLifeSkillCardState, int> <>f__am$cache8;
    private Dictionary<NewLifeSkillType, NewLifeSkillData> MapDatas = new Dictionary<NewLifeSkillType, NewLifeSkillData>();

    public bool CanCollect(NewLifeSkillType type)
    {
        if (this.LifeSkillSimpleInfo == null)
        {
            return false;
        }
        int num = (int) type;
        if (this.LifeSkillSimpleInfo.Count <= num)
        {
            return false;
        }
        NewLifeSkillSimpleInfo info = this.LifeSkillSimpleInfo[num];
        if (info.end_time < 0)
        {
            return false;
        }
        DateTime serverDateTime = TimeMgr.Instance.ServerDateTime;
        DateTime time2 = TimeMgr.Instance.ConvertToDateTime((long) info.starttime);
        return (TimeMgr.Instance.ConvertToDateTime((long) info.end_time) < serverDateTime);
    }

    public void CompletedSimplyData(S2C_NewLifeSkillMapSimpleData result)
    {
        this.LifeSkillSimpleInfo = result.simple_info;
        this.HandTimes = result.remain;
        this.State = States.COMPLETED;
    }

    public NewLifeCellInfo GetCellInfo(NewLifeSkillType type, int cellIndex)
    {
        NewLifeSkillData data = this[type];
        if (data != null)
        {
            foreach (NewLifeCellInfo info in data.cell)
            {
                if (info.cell_num == cellIndex)
                {
                    return info;
                }
            }
        }
        return null;
    }

    public List<int> GetUsingCard()
    {
        if (this.CardStates == null)
        {
            return new List<int>();
        }
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = t => t.cell_index != 3;
        }
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = t => t.card_entry;
        }
        return this.CardStates.card_state.Where<NewLifeSkillCardState>(<>f__am$cache7).Select<NewLifeSkillCardState, int>(<>f__am$cache8).ToList<int>();
    }

    public void Init()
    {
        this.State = States.NOSTART;
        this.RequestServerForSimplyData();
    }

    public void RequestServerForSimplyData()
    {
        SocketMgr.Instance.RequestC2S_NewLifeSkillMapSimpleData();
        this.State = States.REQUESTING;
    }

    internal void UpdateCardData(NewLifeSkillCardData newLifeSkillCardData)
    {
        this.CardStates = newLifeSkillCardData;
    }

    internal void UpdateLifeSkillMapData(NewLifeSkillData newLifeSkillData)
    {
        NewLifeSkillType key = (NewLifeSkillType) newLifeSkillData.skill_entry;
        if (this.MapDatas.ContainsKey(key))
        {
            this.MapDatas[key] = newLifeSkillData;
        }
        else
        {
            this.MapDatas.Add(key, newLifeSkillData);
        }
    }

    public void UpdateNextCostStone(int next)
    {
        this.next_cost_stone = next;
    }

    public void UpdateRemain(int remain)
    {
        this.HandTimes = remain;
    }

    private NewLifeSkillCardData CardStates { get; set; }

    public bool HadChanged { get; set; }

    public int HandTimes { get; private set; }

    public bool HaveCollect
    {
        get
        {
            IEnumerator enumerator = Enum.GetValues(typeof(NewLifeSkillType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (this.CanCollect((NewLifeSkillType) ((int) current)))
                    {
                        return true;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            return false;
        }
    }

    public NewLifeSkillData this[NewLifeSkillType key]
    {
        get
        {
            NewLifeSkillData data;
            if (this.MapDatas.TryGetValue(key, out data))
            {
                return data;
            }
            return null;
        }
    }

    private List<NewLifeSkillSimpleInfo> LifeSkillSimpleInfo { get; set; }

    public int next_cost_stone { get; private set; }

    public States State { get; private set; }

    public enum States
    {
        NOSTART,
        REQUESTING,
        COMPLETED
    }
}

