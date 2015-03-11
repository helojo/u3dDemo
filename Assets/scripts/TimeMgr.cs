using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class TimeMgr : MonoBehaviour
{
    private bool _isServerDataTimeDirty = true;
    private DateTime AcriveByTime_TwentyHourUpdate;
    private int activeAutoUpdatehour__ = 0x16;
    private int activeAutoUpdateMins__;
    public DateTime ArenaLadderRewardTime;
    private DateTime centuryBegin;
    public DateTime ChallengeArenaRewardTime;
    private TimeSpan diff;
    public float DiffSecond;
    private DateTime dym_serverDateTime;
    private bool fireFourEvent;
    private bool fireZeroEvent;
    private DateTime fourHourUpdate;
    public static TimeMgr Instance;
    private int iTickPerMinutes;
    private int lastInteravl;
    private float m_timeCheck;
    private float m_timeCheckRewardInterval = 1f;
    private int mNextUpdateFriendListTime;
    private DateTime NextSyncServerDataTime;
    [HideInInspector]
    public int ServerInitStampTime;
    [HideInInspector]
    public bool ServerTimeInited;
    private DateTime StampTimeStart;
    private int SyncTick = 0x5dc;
    public readonly DateTime TimeStampZero = new DateTime(0x7b2, 1, 1, 0, 0, 0);
    [HideInInspector]
    public int timeZone;
    private DateTime zeroHourUpdate;

    private void Awake()
    {
        Instance = this;
        this.StampTimeStart = this.TimeStampZero.ToLocalTime();
        double totalHours = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
        this.StampTimeStart = this.StampTimeStart.AddHours(-totalHours);
        base.StartCoroutine(this.RunRefresh());
    }

    public DateTime ConvertToDateTime(long timeStamp)
    {
        return this.StampTimeStart.AddSeconds(timeStamp + (this.timeZone * 0xe10)).ToLocalTime();
    }

    public long ConvertToTimeStamp(DateTime value)
    {
        TimeSpan span = (TimeSpan) (value - this.StampTimeStart);
        return (((long) span.TotalSeconds) - (this.timeZone * 0xe10));
    }

    public string GetArenaTime(out bool isTimeOver, int _cdTime)
    {
        isTimeOver = true;
        if ((_cdTime - this.ServerStampTime) <= 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x71), 0);
        }
        isTimeOver = false;
        TimeSpan span = new TimeSpan(0, 0, _cdTime - this.ServerStampTime);
        if (span.Days > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x65), span.Days);
        }
        if (span.Hours > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x70), span.Hours, span.Minutes, span.Seconds);
        }
        if (span.Minutes > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x67), span.Minutes, span.Seconds);
        }
        return string.Format(ConfigMgr.getInstance().GetWord(0x71), span.Seconds);
    }

    public string GetDescTime(int _cdTime)
    {
        string str = string.Empty;
        if ((_cdTime - this.ServerStampTime) <= 0)
        {
            return str;
        }
        TimeSpan span = new TimeSpan(0, 0, _cdTime - this.ServerStampTime);
        if (span.Days > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x65), span.Days);
        }
        if (span.Hours > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x75), span.Hours, span.Minutes, span.Seconds);
        }
        return string.Format(ConfigMgr.getInstance().GetWord(0x76), span.Minutes, span.Seconds);
    }

    public string GetDisTime(int _cdTime)
    {
        TimeSpan span = new TimeSpan(0, 0, _cdTime - this.ServerStampTime);
        if (span.Days > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x68), span.Days);
        }
        if (span.Hours > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x69), span.Hours);
        }
        return string.Format(ConfigMgr.getInstance().GetWord(0x6a), span.Minutes + 1);
    }

    public string GetEndTime(int _cdTime)
    {
        TimeSpan span = new TimeSpan(0, 0, _cdTime - this.ServerStampTime);
        if (span.Days > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x65), span.Days);
        }
        if (span.Hours > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x66), span.Hours);
        }
        return (span.Minutes + ConfigMgr.getInstance().GetWord(0x57b));
    }

    public string GetFrozenTime(int _cdTime)
    {
        DateTime time = Instance.ConvertToDateTime((long) _cdTime);
        DateTime time2 = Instance.ConvertToDateTime((long) this.ServerStampTime);
        if (time.Year > time2.Year)
        {
            object[] objArray1 = new object[] { time.Year, ConfigMgr.getInstance().GetWord(0x57c), time.Month, ConfigMgr.getInstance().GetWord(0x57d), time.Day, ConfigMgr.getInstance().GetWord(0x57e), time.Hour, ConfigMgr.getInstance().GetWord(0x57f), time.Minute, ConfigMgr.getInstance().GetWord(0x580) };
            return string.Concat(objArray1);
        }
        if ((time.Month > time2.Month) || (time.Day > time2.Day))
        {
            object[] objArray2 = new object[] { time.Month, ConfigMgr.getInstance().GetWord(0x57d), time.Day, ConfigMgr.getInstance().GetWord(0x57e), time.Hour, ConfigMgr.getInstance().GetWord(0x57f), time.Minute, ConfigMgr.getInstance().GetWord(0x580) };
            return string.Concat(objArray2);
        }
        object[] objArray3 = new object[] { time.Hour, ConfigMgr.getInstance().GetWord(0x57f), time.Minute, ConfigMgr.getInstance().GetWord(0x580) };
        return string.Concat(objArray3);
    }

    public string GetMailTime(int _cdTime)
    {
        DateTime time = Instance.ConvertToDateTime((long) _cdTime);
        object[] args = new object[] { time.Year, time.Month, time.Day, time.Hour, time.Minute };
        return string.Format(ConfigMgr.getInstance().GetWord(0x6f), args);
    }

    public string GetRemainTime(int _cdTime)
    {
        string str = string.Empty;
        if ((_cdTime - this.ServerStampTime) > 0)
        {
            TimeSpan span = new TimeSpan(0, 0, _cdTime - this.ServerStampTime);
            str = string.Format(ConfigMgr.getInstance().GetWord(0x6b), span.Hours, span.Minutes, span.Seconds);
        }
        return str;
    }

    public string GetRemainTime2(int _cdTime)
    {
        string str = string.Empty;
        if ((_cdTime - this.ServerStampTime) > 0)
        {
            TimeSpan span = new TimeSpan(0, 0, _cdTime - this.ServerStampTime);
            str = string.Format(ConfigMgr.getInstance().GetWord(110), span.Minutes, span.Seconds);
        }
        return str;
    }

    public string GetRemainTime3(int _cdTime)
    {
        if ((_cdTime - this.ServerStampTime) <= 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x71), 0);
        }
        TimeSpan span = new TimeSpan(0, 0, _cdTime - this.ServerStampTime);
        if (span.Days > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x65), span.Days);
        }
        if (span.Hours > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x66), span.Hours);
        }
        if (span.Minutes > 0)
        {
            return (span.Minutes + ConfigMgr.getInstance().GetWord(0x57b));
        }
        return string.Format(ConfigMgr.getInstance().GetWord(0x71), span.Seconds);
    }

    public string GetSendTime(int _cdTime)
    {
        TimeSpan span = new TimeSpan(0, 0, this.ServerStampTime - _cdTime);
        if (span.Days > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x68), span.Days);
        }
        if (span.Hours > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x69), span.Hours);
        }
        if (span.Minutes == 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x6a), 1);
        }
        return string.Format(ConfigMgr.getInstance().GetWord(0x6a), span.Minutes);
    }

    public string GetTimeStr(int _cdTimeSum)
    {
        if (_cdTimeSum > 0x15180)
        {
            return ((_cdTimeSum / 0x15180) + ConfigMgr.getInstance().GetWord(0x579));
        }
        return ((_cdTimeSum / 60) + ConfigMgr.getInstance().GetWord(0x57b));
    }

    public string GetUpdateOverTime(int _OverTime)
    {
        string str = string.Empty;
        if ((_OverTime - this.ServerStampTime) < 0)
        {
            return string.Empty;
        }
        TimeSpan span = new TimeSpan(0, 0, _OverTime - this.ServerStampTime);
        if (span.Days > 0)
        {
            object[] args = new object[] { span.Days, span.Hours, span.Minutes, span.Seconds };
            return string.Format(ConfigMgr.getInstance().GetWord(0x74), args);
        }
        if (span.Hours > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x75), span.Hours, span.Minutes, span.Seconds);
        }
        if (span.Minutes > 0)
        {
            return string.Format(ConfigMgr.getInstance().GetWord(0x76), span.Minutes, span.Seconds);
        }
        if (span.Seconds > 0)
        {
            str = string.Format(ConfigMgr.getInstance().GetWord(0x77), span.Seconds);
        }
        return str;
    }

    public string GetUpdateTime(int _cdTime)
    {
        TimeSpan span = new TimeSpan(0, 0, _cdTime);
        return string.Format(ConfigMgr.getInstance().GetWord(0x6b), span.Hours, span.Minutes, span.Seconds);
    }

    public string GetWorldBossSelfDeadTime(int _cdTime)
    {
        string str = string.Empty;
        if ((_cdTime - this.ServerStampTime) > 0)
        {
            TimeSpan span = new TimeSpan(0, 0, _cdTime - this.ServerStampTime);
            str = string.Format(ConfigMgr.getInstance().GetWord(0xa037af), span.Minutes, span.Seconds);
        }
        return str;
    }

    public void InitServerTime(int _stamp, int _timeZone)
    {
        this.ServerTimeInited = true;
        this.timeZone = _timeZone;
        this.ServerInitStampTime = _stamp;
        DateTime time = this.TimeStampZero.AddSeconds((double) this.ServerInitStampTime);
        this.diff = (TimeSpan) (DateTime.Now - time);
        this.centuryBegin = DateTime.Now;
        this.lastInteravl = 0;
        this._isServerDataTimeDirty = true;
        Debug.Log("ServerTime:" + this.ServerDateTime);
        this.fourHourUpdate = (this.ServerDateTime.Hour >= 4) ? this.ServerDateTime.Date.AddDays(1.0).AddHours(4.0) : this.ServerDateTime.Date.AddHours(4.0);
        int num = 20;
        this.ArenaLadderRewardTime = (this.ServerDateTime.Hour >= num) ? this.ServerDateTime.Date.AddDays(1.0).AddHours((double) num) : this.ServerDateTime.Date.AddHours((double) num);
        this.ChallengeArenaRewardTime = (this.ServerDateTime.Hour >= num) ? this.ServerDateTime.Date.AddDays(1.0).AddHours((double) num) : this.ServerDateTime.Date.AddHours((double) num);
        this.zeroHourUpdate = this.ServerDateTime.Date.AddDays(1.0);
        this.AcriveByTime_TwentyHourUpdate = (this.ServerDateTime.Hour >= 0x16) ? this.ServerDateTime.Date.AddDays(1.0).AddHours(22.0) : this.ServerDateTime.Date.AddHours(22.0);
    }

    public bool NeedRequesetFriendList()
    {
        return (this.ServerStampTime > this.mNextUpdateFriendListTime);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void ResetNextUpdateFriendTime()
    {
        this.mNextUpdateFriendListTime = this.ServerStampTime + 900;
    }

    [DebuggerHidden]
    private IEnumerator RunRefresh()
    {
        return new <RunRefresh>c__IteratorB7 { <>f__this = this };
    }

    public DateTime ServerDateTimeOfToday(int hour, int minutes, int seconds)
    {
        DateTime serverDateTime = this.ServerDateTime;
        return new DateTime(serverDateTime.Year, serverDateTime.Month, serverDateTime.Day, hour, minutes, seconds);
    }

    private void Update()
    {
        if (this.ServerTimeInited)
        {
            ActorData.getInstance().Tick();
            long num = DateTime.Now.Ticks - this.centuryBegin.Ticks;
            float num2 = ((float) num) / 1E+07f;
            float f = num2 - this.lastInteravl;
            if (f > 1f)
            {
                int num4 = Mathf.FloorToInt(f);
                this.lastInteravl += num4;
                this._isServerDataTimeDirty = true;
                this.iTickPerMinutes++;
                if (this.iTickPerMinutes > 15)
                {
                    this.iTickPerMinutes = 0;
                }
            }
        }
    }

    public int NextUpdateFriendListTime
    {
        get
        {
            return this.mNextUpdateFriendListTime;
        }
        set
        {
            this.mNextUpdateFriendListTime = value;
        }
    }

    public DateTime ServerDateTime
    {
        get
        {
            this.dym_serverDateTime = this.StampTimeStart.AddSeconds((double) (this.ServerStampTime + (this.timeZone * 0xe10)));
            return this.dym_serverDateTime;
        }
    }

    public int ServerStampTime
    {
        get
        {
            DateTime time2 = DateTime.Now - this.diff;
            TimeSpan span = (TimeSpan) (time2 - this.TimeStampZero);
            return (int) span.TotalSeconds;
        }
    }

    [CompilerGenerated]
    private sealed class <RunRefresh>c__IteratorB7 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal TimeMgr <>f__this;
        internal float <delta>__0;
        internal int <hour>__1;
        internal int <minute>__2;
        internal int <second>__3;

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
                case 1:
                    break;

                case 2:
                    if (this.<>f__this.NextSyncServerDataTime < this.<>f__this.ServerDateTime)
                    {
                        this.<>f__this.NextSyncServerDataTime = this.<>f__this.ServerDateTime.AddSeconds((double) this.<>f__this.SyncTick);
                        SocketMgr.Instance.RequestSyncChangeData();
                    }
                    if (this.<>f__this.ServerDateTime > this.<>f__this.fourHourUpdate)
                    {
                        this.<>f__this.fourHourUpdate = this.<>f__this.ServerDateTime.Date.AddDays(1.0).AddHours(4.0);
                        ActorData.getInstance().FourClockRefreshData();
                        Debug.Log("FourClockRefreshData");
                    }
                    if (this.<>f__this.ServerDateTime > this.<>f__this.ArenaLadderRewardTime)
                    {
                        this.<>f__this.ArenaLadderRewardTime = this.<>f__this.ServerDateTime.Date.AddDays(1.0).AddHours(20.0);
                        SocketMgr.Instance.RequestGetMailList();
                        ActorData.getInstance().mHaveMailRefresh = true;
                    }
                    if (this.<>f__this.ServerDateTime > this.<>f__this.ChallengeArenaRewardTime)
                    {
                        this.<>f__this.ChallengeArenaRewardTime = this.<>f__this.ServerDateTime.Date.AddDays(1.0).AddHours(20.0);
                        SocketMgr.Instance.RequestGetMailList();
                        ActorData.getInstance().mHaveMailRefresh = true;
                    }
                    if (this.<>f__this.ServerDateTime > this.<>f__this.zeroHourUpdate)
                    {
                        this.<>f__this.zeroHourUpdate = this.<>f__this.ServerDateTime.Date.AddDays(1.0);
                        ActorData.getInstance().ZeroClockRefreshData();
                        Debug.Log("ZeroClockRefreshData");
                    }
                    if (this.<>f__this.ServerDateTime > this.<>f__this.AcriveByTime_TwentyHourUpdate)
                    {
                        this.<>f__this.AcriveByTime_TwentyHourUpdate = this.<>f__this.ServerDateTime.Date.AddDays(1.0).AddHours(22.0);
                        ActorData.getInstance().reqNewActiveDataInfo();
                        ActorData.getInstance().TwentyClockRefreshData();
                    }
                    this.<delta>__0 = Time.deltaTime;
                    this.<>f__this.m_timeCheck += this.<delta>__0;
                    if (this.<>f__this.m_timeCheck >= this.<>f__this.m_timeCheckRewardInterval)
                    {
                        this.<>f__this.m_timeCheckRewardInterval = 0f;
                        this.<hour>__1 = TimeMgr.Instance.ServerDateTime.Hour;
                        this.<minute>__2 = TimeMgr.Instance.ServerDateTime.Minute;
                        this.<second>__3 = TimeMgr.Instance.ServerDateTime.Second;
                        if (((((this.<hour>__1 == 12) && (this.<minute>__2 == 0)) && (this.<second>__3 == 0)) || (((this.<hour>__1 == 0x12) && (this.<minute>__2 == 0)) && (this.<second>__3 == 0))) || (((this.<hour>__1 == 0x15) && (this.<minute>__2 == 0)) && (this.<second>__3 == 0)))
                        {
                            SocketMgr.Instance.RequestRewardFlag();
                        }
                    }
                    break;

                default:
                    goto Label_041F;
            }
            if (!this.<>f__this.ServerTimeInited)
            {
                this.$current = new WaitForSeconds(60f);
                this.$PC = 1;
            }
            else
            {
                this.$current = new WaitForSeconds(60f);
                this.$PC = 2;
            }
            return true;
            this.$PC = -1;
        Label_041F:
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
}

