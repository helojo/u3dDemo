using FastBuf;
using Newbie;
using System;
using System.Collections;
using UnityEngine;

public class GameDataMgr : MonoBehaviour
{
    public int ActorRewardStage;
    private bool bActiveIsNew__;
    public BoostRecruit boostRecruit = new BoostRecruit();
    public bool CardActivityFlag;
    public bool CardEquipLvUpFlag;
    public bool DirtyActiveCard;
    public bool DirtyActorStage;
    public bool DirtyCardEquipLvUp;
    public bool FreeRecruitFlag;
    private static GameDataMgr instance;
    private bool m_confirmedTime;
    private long m_nextShopRefreshTime;
    private float m_timeCheck;
    private float m_timeCheckStatageChanged;
    private float m_updateIntervalCheck = 3f;
    private float m_updateIntervalStatageChanged = 30f;

    public long GetNextShopRefreshTime()
    {
        long num = this.LastShopRefreshTime();
        DateTime serverDateTime = TimeMgr.Instance.ServerDateTime;
        TimeSpan span = (TimeSpan) (serverDateTime - serverDateTime.Date);
        long totalSeconds = (long) span.TotalSeconds;
        DateTime now = DateTime.Now;
        TimeSpan span2 = (TimeSpan) (now - now.Date);
        double num3 = span2.TotalSeconds;
        if (totalSeconds >= num)
        {
            return ((this.m_nextShopRefreshTime + 0x15180L) - ((long) num3));
        }
        return (this.m_nextShopRefreshTime - ((long) num3));
    }

    private long LastShopRefreshTime()
    {
        ArrayList list = ConfigMgr.getInstance().getList<courage_shop_time_config>();
        int count = list.Count;
        if (count < 1)
        {
            return -1L;
        }
        return (long) (list[count - 1] as courage_shop_time_config).day_time;
    }

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;
        this.m_timeCheck += deltaTime;
        this.m_timeCheckStatageChanged += deltaTime;
        if (this.m_timeCheck > this.m_updateIntervalCheck)
        {
            this.m_timeCheck = 0f;
            this.UpdateCourageShopState();
            if (this.DirtyCardEquipLvUp)
            {
                this.CardEquipLvUpFlag = (CommonFunc.RealCheckCardCanLevUp() || CommonFunc.CheckHaveCardCanBreak()) || CommonFunc.CheckHaveCardCanUpStar();
                this.DirtyCardEquipLvUp = false;
            }
            if (this.DirtyActiveCard)
            {
                this.CardActivityFlag = CommonFunc.RealCheckActiveCard();
                this.DirtyActiveCard = false;
            }
            this.FreeRecruitFlag = this.HasFreeRecruit;
        }
        if (this.DirtyActorStage && (this.m_timeCheckStatageChanged > this.m_updateIntervalStatageChanged))
        {
            this.m_timeCheckStatageChanged = 0f;
            this.DirtyActorStage = false;
            SocketMgr.Instance.RequestRewardFlag();
        }
        if (((ActorData.getInstance().bUseStone || ActorData.getInstance().bPlayerLevelUp) || (ActorData.getInstance().IsVipChange || ActorData.getInstance().bGuidLvChange)) || (ActorData.getInstance().bChargeChange || ActorData.getInstance().bMonthVipCardChange))
        {
            if (ActorData.getInstance().bUseStone)
            {
                ActorData.getInstance().bUseStone = false;
            }
            if (ActorData.getInstance().bPlayerLevelUp)
            {
                ActorData.getInstance().bPlayerLevelUp = false;
            }
            if (ActorData.getInstance().IsVipChange)
            {
                ActorData.getInstance().IsVipChange = false;
            }
            if (ActorData.getInstance().bGuidLvChange)
            {
                ActorData.getInstance().bGuidLvChange = false;
            }
            if (ActorData.getInstance().bChargeChange)
            {
                ActorData.getInstance().bChargeChange = false;
            }
            if (ActorData.getInstance().bMonthVipCardChange)
            {
                ActorData.getInstance().bMonthVipCardChange = false;
            }
            ActorData.getInstance().reqActiveCompleteIsOrNot();
        }
    }

    public void Login()
    {
        this.m_confirmedTime = true;
        BattleStaticEntry.IsInBattle = false;
        SocketMgr.Instance.RequestCourageShopItemList();
        this.RecalculateNextShopRefreshTime();
        this.boostRecruit.Reset();
        GuideSystem.OnLogon();
    }

    public string NextShopRefreshTime()
    {
        long num = this.LastShopRefreshTime();
        DateTime serverDateTime = TimeMgr.Instance.ServerDateTime;
        TimeSpan span = (TimeSpan) (serverDateTime - serverDateTime.Date);
        long totalSeconds = (long) span.TotalSeconds;
        return string.Format(ConfigMgr.getInstance().GetWord((totalSeconds < num) ? 470 : 0x1d7), this.m_nextShopRefreshTime / 0xe10L);
    }

    public void Quit()
    {
        this.m_nextShopRefreshTime = 0L;
        this.m_confirmedTime = false;
        this.boostRecruit.OnQuit();
    }

    private void RecalculateNextShopRefreshTime()
    {
        ArrayList list = ConfigMgr.getInstance().getList<courage_shop_time_config>();
        int count = list.Count;
        if (count >= 1)
        {
            long[] numArray = new long[count];
            for (int i = 0; i != count; i++)
            {
                numArray[i] = (list[i] as courage_shop_time_config).day_time;
            }
            TimeSpan span = (TimeSpan) (TimeMgr.Instance.ServerDateTime - TimeMgr.Instance.ServerDateTime.Date);
            long totalSeconds = (long) span.TotalSeconds;
            for (int j = count - 1; j >= 0; j--)
            {
                long num5 = numArray[j];
                if (totalSeconds > num5)
                {
                    if ((j + 1) <= (count - 1))
                    {
                        this.m_nextShopRefreshTime = numArray[j + 1];
                        return;
                    }
                    break;
                }
            }
            this.m_nextShopRefreshTime = numArray[0];
        }
    }

    public long RemainShopRefreshTime()
    {
        DateTime serverDateTime = TimeMgr.Instance.ServerDateTime;
        long num = ((serverDateTime.Hour * 0xe10) + (serverDateTime.Minute * 60)) + serverDateTime.Second;
        if (num <= (this.m_nextShopRefreshTime + 2L))
        {
            return (long) Mathf.Max((float) (this.m_nextShopRefreshTime - num), 0f);
        }
        return ((0x15180L + this.m_nextShopRefreshTime) - num);
    }

    private void Start()
    {
    }

    private void UpdateCourageShopState()
    {
        if (this.m_confirmedTime)
        {
            TimeSpan span = (TimeSpan) (TimeMgr.Instance.ServerDateTime - TimeMgr.Instance.ServerDateTime.Date);
            long totalSeconds = (long) span.TotalSeconds;
            long num2 = this.LastShopRefreshTime();
            if (((totalSeconds <= num2) || (this.m_nextShopRefreshTime >= num2)) && (totalSeconds >= this.m_nextShopRefreshTime))
            {
                SocketMgr.Instance.RequestCourageShopItemList();
                this.RecalculateNextShopRefreshTime();
                if (SettingMgr.mInstance.PushNotification_Enable && SettingMgr.mInstance.ShopRefreshPush)
                {
                    PushMgr.mInstance.addNotification(2, ConfigMgr.getInstance().GetWord(0xa95f60), ConfigMgr.getInstance().GetWord(0xa95f63), 3);
                    Debug.Log("PushSend Msg ----------------->" + ConfigMgr.getInstance().GetWord(0xa95f63));
                }
            }
        }
    }

    public bool AchievementStage
    {
        get
        {
            return (0 != ((this.ActorRewardStage >> 1) & 1));
        }
    }

    public bool ActiveIsNew
    {
        get
        {
            return this.bActiveIsNew__;
        }
        set
        {
            this.bActiveIsNew__ = value;
        }
    }

    public bool ActiveStage
    {
        get
        {
            return (0 != ((this.ActorRewardStage >> 3) & 1));
        }
    }

    public bool DailyStage
    {
        get
        {
            return (0 != ((this.ActorRewardStage >> 2) & 1));
        }
    }

    public bool HasFreeRecruit
    {
        get
        {
            IEnumerator enumerator = ConfigMgr.getInstance().getList<lottery_card_option_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    lottery_card_option_config current = (lottery_card_option_config) enumerator.Current;
                    long time = 0L;
                    if (this.boostRecruit.FreeTime(current.entry, out time))
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
            if (((ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Gold_Draw) == null) && (ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Gold_Ten_Draw) == null)) && ((ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Stone_Draw) == null) && (ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Stone_Ten_Draw) == null)))
            {
                return false;
            }
            return true;
        }
    }

    public static GameDataMgr Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject target = new GameObject {
                    name = "GameDataMgr_Container"
                };
                UnityEngine.Object.DontDestroyOnLoad(target);
                instance = target.AddComponent<GameDataMgr>();
            }
            return instance;
        }
    }
}

