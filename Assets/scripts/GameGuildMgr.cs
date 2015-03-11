using FastBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class GameGuildMgr : XSingleton<GameGuildMgr>
{
    [CompilerGenerated]
    private static Predicate<GuildMember> <>f__am$cacheC;
    public List<int> CurReqPageItemIDList = new List<int>();
    private TrenchLock LastLock;
    public Dictionary<int, List<int>> PageIdToItemsDic = new Dictionary<int, List<int>>();
    public int perPageItemCnt = 10;

    public GameGuildMgr()
    {
        this.DeathHeros = new List<int>();
        this.UIOpenType = OpenType.None;
    }

    private void Clear()
    {
        this.GuildDupEnergy = 0;
        this.DeathHeros = new List<int>();
        this.DupStatus = null;
        this.LastLock = null;
    }

    public GuildDupStatusInfo GetDupStateByEntry(int id)
    {
        if (this.DupStatus != null)
        {
            foreach (GuildDupStatusInfo info in this.DupStatus)
            {
                if (info.guildDupEntry == id)
                {
                    return info;
                }
            }
        }
        return null;
    }

    public List<monster_config> GetMonster(int trench)
    {
        <GetMonster>c__AnonStorey28A storeya = new <GetMonster>c__AnonStorey28A();
        guilddup_trench_config _config = ConfigMgr.getInstance().getByEntry<guilddup_trench_config>(trench);
        battle_config _config2 = ConfigMgr.getInstance().getByEntry<battle_config>(_config.battlefield_entry);
        List<string> list = new List<string> {
            _config2.monster_0,
            _config2.monster_1,
            _config2.monster_2,
            _config2.monster_3,
            _config2.monster_4,
            _config2.monster_5
        };
        storeya.list = new List<int>();
        foreach (string str in list)
        {
            int result = -1;
            if (int.TryParse(str, out result) && (result >= 0))
            {
                storeya.list.Add(result);
            }
        }
        return ConfigMgr.getInstance().getListResult<monster_config>().Where<monster_config>(new Func<monster_config, bool>(storeya.<>m__5DF)).ToList<monster_config>();
    }

    internal bool IsDead(int entry)
    {
        foreach (int num in this.DeathHeros)
        {
            if (num == entry)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsLocking(int dup, int trench)
    {
        TrenchLock lastLock = this.LastLock;
        return ((((lastLock != null) && (lastLock.DupID == dup)) && (lastLock.TrenchID == trench)) && lastLock.IsLock);
    }

    public void Lock(int dup, int trench, int lockTime)
    {
        this.LockStatus = LockState.Locked;
        TrenchLock @lock = new TrenchLock {
            DupID = dup,
            TrenchID = trench,
            StartTime = TimeMgr.Instance.ServerStampTime,
            LockTime = lockTime
        };
        this.LastLock = @lock;
    }

    internal void ReleaseLock(bool send = true)
    {
        if (this.LastLock != null)
        {
            if (this.LastLock.IsLock && send)
            {
                this.LockStatus = LockState.Restrict;
                this.SetRestrictEndTime(-1);
                SocketMgr.Instance.RequestC2S_ReleaseGuildDupLock(this.LastLock.DupID, this.LastLock.TrenchID);
            }
            this.LastLock = null;
        }
    }

    internal void RequestDupState()
    {
        this.Clear();
        if ((this.DupStatus == null) || (this.DupStatus.Count == 0))
        {
            SocketMgr.Instance.RequestC2S_GetGuildDupInfo();
        }
    }

    public void SetRestrictEndTime(int time = -1)
    {
        if (time == -1)
        {
            this.RestrictEndTime = TimeMgr.Instance.ServerStampTime + 60;
        }
        else
        {
            this.RestrictEndTime = -1;
        }
    }

    public void UpdateDeathHero(List<int> heros)
    {
        this.DeathHeros = heros;
    }

    internal void UpdateDupState(List<GuildDupStatusInfo> list)
    {
        this.DupStatus = list;
    }

    public void UpdateGuildDupEnergy(int energy)
    {
        this.GuildDupEnergy = energy;
    }

    public int CurReqDupId { get; set; }

    public List<int> DeathHeros { get; private set; }

    public List<GuildDupStatusInfo> DupStatus { get; private set; }

    public int GuildDupEnergy { get; private set; }

    public bool IsLeader
    {
        get
        {
            GuildMemberData mGuildMemberData = ActorData.getInstance().mGuildMemberData;
            if (mGuildMemberData != null)
            {
                if (<>f__am$cacheC == null)
                {
                    <>f__am$cacheC = t => t.userInfo.id == ActorData.getInstance().SessionInfo.userid;
                }
                GuildMember member = mGuildMemberData.member.Find(<>f__am$cacheC);
                if ((member != null) && (member.position == 1))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public GuildDupStatusInfo LastGuildDup { get; set; }

    public int LimitTime
    {
        get
        {
            return Mathf.Max(0, (this.LastLock != null) ? ((this.LastLock.StartTime + 0x41) - TimeMgr.Instance.ServerStampTime) : 0);
        }
    }

    public LockState LockStatus { get; private set; }

    public int RestrictEndTime { get; private set; }

    public OpenType UIOpenType { get; set; }

    [CompilerGenerated]
    private sealed class <GetMonster>c__AnonStorey28A
    {
        internal List<int> list;

        internal bool <>m__5DF(monster_config t)
        {
            return this.list.Contains(t.entry);
        }
    }

    public class TrenchLock
    {
        public int DupID { get; set; }

        public bool IsLock
        {
            get
            {
                return (this.LockTime > TimeMgr.Instance.ServerStampTime);
            }
        }

        public int LockTime { get; set; }

        public int StartTime { get; set; }

        public int TrenchID { get; set; }
    }
}

