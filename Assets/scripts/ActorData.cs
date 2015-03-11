using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class ActorData
{
    public float _displayGetMsgTime = 60f;
    public float _showGetMsgTime = 10f;
    [CompilerGenerated]
    private static Comparison<Friend> <>f__am$cacheA8;
    private List<string> adAutoInfoList__ = new List<string>();
    public BattleFormation ArenaFormation;
    public int ArenaRefleshTime = -1;
    public List<AssistUser> AssistUserList = new List<AssistUser>();
    private bool bChargeChange__;
    private bool bGuidLvChange__;
    private bool bMonthVipCardChange__;
    public bool bOpenOutlandTitleInfo;
    public bool bOpenThirdPanel;
    public bool bOpenUI;
    public int BossAtkRebornInterval;
    private bool bPlayerLevelUp__;
    private bool bUseStone__;
    public Dictionary<long, int> CardFightPowerDict = new Dictionary<long, int>();
    public BattleFormation ChallengeArenaFormation = new BattleFormation();
    public ChallengeArenaData ChallengeData = new ChallengeArenaData();
    private int coolDownUsedTime;
    public List<ShopItem> courageShopItemList = new List<ShopItem>();
    public int CurDupEntry;
    public DuplicateType CurDupType;
    public S2C_GetFriendFormation CurrFriendPkInfo;
    public S2C_GetLeagueReward CurrLeagueReward;
    public S2C_GetLeagueOpponentFormation CurrWorldCupPkInfo;
    public long CurrWorldCupPkTargetId = -1L;
    public int CurTrenchEntry;
    public List<int> DailySoulCardEntries = new List<int>();
    private int deltaStampTime;
    private bool DirtyLiveness = true;
    private bool DirtyQuestList = true;
    public float DupListVal;
    public Vector2 DupMapPos;
    public float DupMapVal;
    public List<DuplicateRewardInfo> DupRewardInfo = new List<DuplicateRewardInfo>();
    public int EliteProgress;
    public bool EncourageAtkTip = true;
    public bool EncourageCritTip = true;
    public bool EncouragePopMsg;
    public bool FirstCharge;
    public bool FriendPkEndReturn;
    public List<GuildMsgItem> guildMsgItems = new List<GuildMsgItem>();
    public int guildMsgKey;
    public bool haveReceiveDailyReward;
    public int HeroProgress;
    private static ActorData instance;
    public bool IsCostStone;
    public bool isInOutland;
    public bool isLeavePaoku;
    public bool IsOnlyShowTargetTeam;
    public bool IsOpenWorldBoss;
    public bool isOutlandGrid;
    public bool isPaokuWin;
    public bool IsPopPanel;
    public bool isPreOutlandFight;
    public bool IsSendPak;
    private bool isVipChange__;
    public bool IsWaitingLoadData;
    public S2C_GetJoinLeague JoinLeagueInfo;
    public bool JumpMainReplayDup;
    private int lastCalAddForceTime = -1;
    private int lastCalAddSkillTime = -1;
    private int lastUpdateTime;
    public LeagueOpponent LeagueOpponentInfo;
    public int Liveness;
    public float LoadingUserInfoProgress;
    public int m_entryFrom;
    public int m_nDungeonCostPhysical;
    public int m_nEliteDupCostPhysical;
    public int m_nNormalDupCostPhysical;
    public int m_resourceEntry;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private float m_updateIntervalCheck = 5f;
    public List<string> mAdList = new List<string>();
    public int mask_guildMsg_time;
    public bool mAutoAtkWorldBoss;
    public List<FastBuf.Broadcast> mBroadcastList = new List<FastBuf.Broadcast>();
    public CardChangedType mCardChangedType;
    public List<Card> mCards = new List<Card>();
    public List<FastBuf.Broadcast> mChatList = new List<FastBuf.Broadcast>();
    public List<FlameBattleBackCardInfo> mCurrAttackersList = new List<FlameBattleBackCardInfo>();
    public List<FlameBattleBackCardInfo> mCurrdefendersList = new List<FlameBattleBackCardInfo>();
    public DupReturnPrePanelPara mCurrDupReturnPrePara;
    public int mCurrWarmmatchEndTime;
    public int mCurrWorldCupEndTime;
    public List<Card> mDefaultArenaLadderList = new List<Card>();
    public List<Card> mDefaultDetainsDart_BattleBackSelf_List = new List<Card>();
    public List<Card> mDefaultDetainsDart_Intercept_List = new List<Card>();
    public List<Card> mDefaultDupHeroList = new List<Card>();
    public List<Card> mDefaultFriendPkList = new List<Card>();
    public List<Card> mDefaultGuildBattleList = new List<Card>();
    public List<Card> mDefaultGuildDupBattleList = new List<Card>();
    public List<Card> mDefaultOutlandPkList_0 = new List<Card>();
    public List<Card> mDefaultOutlandPkList_1 = new List<Card>();
    public List<Card> mDefaultOutlandPkList_2 = new List<Card>();
    public List<Card> mDefaultOutlandPkList_3 = new List<Card>();
    public List<Card> mDefaultTowerPkList = new List<Card>();
    public List<Card> mDefaultWarmmatchList = new List<Card>();
    public List<Card> mDefaultWorldBossList = new List<Card>();
    public List<Card> mDefaultWorldCupPkList = new List<Card>();
    public List<Card> mDefaultYuanZhengPkList = new List<Card>();
    public FlameBattleInfo mFlameBattleInfo;
    public int mFriendAssistCardEntry = -1;
    public bool mFriendAssistIsQQ;
    public List<Friend> mFriendList = new List<Friend>();
    public List<BriefUser> mFriendReqList = new List<BriefUser>();
    public FriendReward mFriendReward;
    private List<GuildApplication> mGuildApplyList = new List<GuildApplication>();
    public GuildData mGuildData = new GuildData();
    public GuildMemberData mGuildMemberData = new GuildMemberData();
    public bool mHaveMailRefresh;
    private bool mHaveNewArenaChallengeLog;
    private bool mHaveNewArenaLog;
    private bool mHaveNewDetainsDartLog;
    public bool mHaveNewTitle;
    public bool mItemDataDirty;
    private bool mLivenessCanPick;
    private List<LivenessTask> mLivenessList = new List<LivenessTask>();
    public List<LotteryCardDiscount> mLotteryCardDicountList = new List<LotteryCardDiscount>();
    public List<LotteryCard> mLotteryCardList = new List<LotteryCard>();
    private List<Mail> mMailList = new List<Mail>();
    private bool mNeedUpdateTitleBar;
    private List<Quest> mQuestList = new List<Quest>();
    public bool mRequestCallCard;
    private SkillPoint mSkillPoint = new SkillPoint();
    public List<int> mTitleEntryList = new List<int>();
    private List<TitleData> mTitleList = new List<TitleData>();
    public bool mTodayOpenGoblinShopTips;
    public bool mTodayOpenSecretShopTips;
    public UserGuildMemberData mUserGuildMemberData = new UserGuildMemberData();
    private User mUserInfo;
    public int nBoxCopperKeys;
    public int nBoxGoldKeys;
    public int nBoxSliverKeys;
    public int nCurAttackTimes;
    public int NextGetSkillTime;
    public int NextRefreshFriendListTime;
    public int NextRefreshNewMailTime;
    public int NormalProgress;
    public int nTotalAttackTime;
    public int OffsetY;
    public bool OpenNewDup;
    public bool[] outlandAllHerosDeadList = new bool[4];
    public List<OutlandBattleBackCardInfo> OutlandCardStatList = new List<OutlandBattleBackCardInfo>();
    public int outlandEntry = -1;
    public List<OutlandFloorSimpleInfo> outlandFloorList = new List<OutlandFloorSimpleInfo>();
    public int outlandLevel;
    public int outlandPageEntry = -1;
    public List<OutlandTitle> outlandTitles = new List<OutlandTitle>();
    public int outlandType;
    public int paokuCardEntry = -1;
    public int paokuMapEntry = -1;
    public int paokuPropIndex = -1;
    private float remainTimeOfWaitingLoadData;
    public Session SessionInfo;
    private long stampSuperVip;
    private long stampVip;
    private bool startAddPhyForce;
    public outland_map_type_config tempOutlandMapTypeConfig;
    public int TowerRemainFightCount;
    public Dictionary<int, List<FastBuf.TrenchData>> TrenchEliteDataDict = new Dictionary<int, List<FastBuf.TrenchData>>();
    public Dictionary<int, List<FastBuf.TrenchData>> TrenchHeroDataDict = new Dictionary<int, List<FastBuf.TrenchData>>();
    public Dictionary<int, List<FastBuf.TrenchData>> TrenchNormalDataDict = new Dictionary<int, List<FastBuf.TrenchData>>();
    public MonthCardData VipData_MonthCard = new MonthCardData();
    public int WeekSoulCardEntry = -1;
    public BattleFormation WorldCupFormation;
    public List<FlameBattleSelfData> YuanZhengCardStatList = new List<FlameBattleSelfData>();

    private ActorData()
    {
    }

    public void AddAdInfo(string strInfo)
    {
        this.adAutoInfoList__.Insert(0, strInfo);
    }

    public void AddAdInfoList(List<string> list)
    {
        foreach (string str in list)
        {
            this.adAutoInfoList__.Add(str);
        }
    }

    public void AddBroadCast(FastBuf.Broadcast bc)
    {
        this.mBroadcastList.Insert(0, bc);
    }

    public void AddBroadCastList(List<FastBuf.Broadcast> list)
    {
        foreach (FastBuf.Broadcast broadcast in list)
        {
            this.mBroadcastList.Add(broadcast);
        }
    }

    public void AddSessionProtocolKey()
    {
        if (this.SessionInfo != null)
        {
            this.SessionInfo.protocolKey++;
        }
    }

    public void AddTitle(List<TitleData> _titleList)
    {
        if (_titleList.Count > 0)
        {
            GameDataMgr.Instance.DirtyActorStage = true;
        }
        foreach (TitleData data in _titleList)
        {
            if (!this.mTitleEntryList.Contains(data.title_entry))
            {
                this.mTitleEntryList.Add(data.title_entry);
                this.mTitleList.Add(data);
            }
        }
    }

    private void CalAddForce()
    {
        if (this.lastCalAddForceTime >= 0)
        {
            int serverStampTime = TimeMgr.Instance.ServerStampTime;
            int num2 = serverStampTime - this.lastCalAddForceTime;
            if (num2 > 600)
            {
                this.lastCalAddForceTime = -1;
                SocketMgr.Instance.RequestSyncChangeData();
            }
            else
            {
                int num3 = 300;
                if (this.PhyForce >= this.UserInfo.maxPhyForce)
                {
                    this.lastCalAddForceTime = serverStampTime;
                }
                else if (num2 > num3)
                {
                    this.lastCalAddForceTime = serverStampTime;
                    int phyForce = this.PhyForce;
                    int min = 1;
                    int num6 = Mathf.Clamp(min * (num2 / num3), min, this.UserInfo.maxPhyForce - this.PhyForce);
                    this.PhyForce += num6;
                    object[] args = new object[] { TimeMgr.Instance.ConvertToDateTime((long) this.lastCalAddForceTime), num6, num2, phyForce, this.PhyForce };
                    Debug.Log(string.Format("Last:{0:yy/MM/dd HH:mm:ss} addForce:{1} deltaTime:{2}S PhyForce:{3}->{4}", args));
                }
            }
        }
    }

    public void CardMachining(List<NewCard> _newCardList)
    {
        if (_newCardList.Count > 0)
        {
            GameDataMgr.Instance.DirtyActorStage = true;
        }
        foreach (NewCard card in _newCardList)
        {
            if (card.newCard.Count > 0)
            {
                if (card.newCard[0] == null)
                {
                    break;
                }
                if (this.GetCardByEntry(card.newCard[0].cardInfo.entry) == null)
                {
                    this.mCards.Add(card.newCard[0]);
                    CardPool.SynCardObject((int) card.newCard[0].cardInfo.entry, card.newCard[0], 3);
                }
                else
                {
                    Debug.Log("error update Card!!");
                }
            }
            if (card.newItem.Count > 0)
            {
                this.UpdateItemList(card.newItem);
            }
        }
    }

    private void CheckFormatList(List<Card> _List)
    {
        List<Card> list = new List<Card>();
        HashSet<long> set = new HashSet<long>();
        foreach (Card card in _List)
        {
            if (set.Contains(card.card_id))
            {
                list.Add(card);
            }
            else
            {
                set.Add(card.card_id);
            }
        }
        foreach (Card card2 in list)
        {
            Debug.LogWarning("Save Card Is Error !");
            _List.Remove(card2);
        }
    }

    public void Clear()
    {
        this.SessionInfo = null;
    }

    public void ClearGuildData()
    {
        this.mUserGuildMemberData = null;
        this.mGuildData = null;
        this.mGuildMemberData = null;
    }

    public void ClearLivenessDataDirtyFlag()
    {
        this.DirtyLiveness = false;
    }

    public void ClearQuestDataDirtyFlag()
    {
        this.DirtyQuestList = false;
    }

    private void CreateEquipBreakDict()
    {
        XSingleton<EquipBreakMateMgr>.Reset();
        for (int i = 0; i < this.mCards.Count; i++)
        {
            Card card = this.mCards[i];
            if (card != null)
            {
                for (int j = 0; j < 6; j++)
                {
                    EquipInfo info = card.equipInfo[j];
                    break_equip_config _config = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
                    if ((_config != null) && (_config.break_equip_entry >= 0))
                    {
                        XSingleton<EquipBreakMateMgr>.Singleton.Update(card, j);
                    }
                }
            }
        }
    }

    public bool CurrHaveTiLiCanPick()
    {
        foreach (Friend friend in this.mFriendList)
        {
            if (getInstance().UserInfo.remainPhyForceAccept > 0)
            {
                if (friend.isGivePhyForceNow)
                {
                    return true;
                }
            }
            else if ((this.UserInfo.redpackage_enable && this.UserInfo.redpackage_game_friend_can_draw) && (!friend.redpackage_isdraw && (friend.redpackage_num > 0)))
            {
                return true;
            }
        }
        return false;
    }

    public void DeleteFriend(long targetId)
    {
        foreach (Friend friend in this.mFriendList)
        {
            if (friend.userInfo.id == targetId)
            {
                this.mFriendList.Remove(friend);
                GameDataMgr.Instance.DirtyActorStage = true;
                break;
            }
        }
    }

    public void DelItemFriendReqList(long targetId)
    {
        foreach (BriefUser user in this.mFriendReqList)
        {
            if (user.id == targetId)
            {
                this.mFriendReqList.Remove(user);
                break;
            }
        }
    }

    public void DelMailList(List<long> _mailList)
    {
        foreach (long num in _mailList)
        {
            this.DelteMail(num);
        }
    }

    public void DelteMail(long _mailId)
    {
        foreach (Mail mail in this.mMailList)
        {
            if (mail.mail_id == _mailId)
            {
                this.mMailList.Remove(mail);
                break;
            }
        }
    }

    public void FourClockRefreshData()
    {
        SocketMgr.Instance.RequestGetUserInfo();
        SocketMgr.Instance.RequestGetFriendList();
        this.reqNewActiveDataInfo();
        SocketMgr.Instance.RequestRefreshQQFriendInGame();
        getInstance().UserInfo.phyforce_buy_times = 0;
        if (getInstance().Level >= CommonFunc.LevelLimitCfg().guild)
        {
            SocketMgr.Instance.RequestGuildData(true, null);
        }
    }

    public string GetAdMsg()
    {
        string str = string.Empty;
        if (this.adAutoInfoList__.Count > 0)
        {
            string str2 = this.adAutoInfoList__[0];
            str = "[00FF00]AD[FFFF00]:" + str2;
            this.InsetAdMsg("[0000FF]AD:" + GameConstant.DefaultTextColor + str2);
            this.adAutoInfoList__.RemoveAt(0);
        }
        return str;
    }

    public bool GetAllActiveCompleteIsOk()
    {
        bool flag = false;
        int count = ActiveList.actives.Count;
        for (int i = 0; i < count; i++)
        {
            ActiveInfo info = ActiveList.actives[i];
            int num3 = info.rewards_configs.Count;
            bool flag2 = false;
            for (int j = 0; j < num3; j++)
            {
                if (info.activity_type == ActivityType.e_tencent_activity_collect_exchange)
                {
                    if (getInstance().GetNeedPressItemIsOkOrNot(info))
                    {
                        flag2 = true;
                        break;
                    }
                    flag2 = false;
                }
                else
                {
                    if (info.rewards_configs[j].flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW)
                    {
                        flag2 = true;
                        break;
                    }
                    flag2 = false;
                }
            }
            flag = flag2;
            if (flag)
            {
                return flag;
            }
        }
        return flag;
    }

    public Card GetCardByEntry(uint entry)
    {
        foreach (Card card in this.mCards)
        {
            if (card.cardInfo.entry == entry)
            {
                return card;
            }
        }
        return null;
    }

    public Card GetCardByID(long _id)
    {
        foreach (Card card in this.mCards)
        {
            if (card.card_id == _id)
            {
                return card;
            }
        }
        return null;
    }

    public int GetCardFightPowerById(long cardId)
    {
        int num = 0;
        if (this.CardFightPowerDict.TryGetValue(cardId, out num))
        {
            return this.CardFightPowerDict[cardId];
        }
        return 0;
    }

    public Item GetCopperKey()
    {
        <GetCopperKey>c__AnonStorey27C storeyc = new <GetCopperKey>c__AnonStorey27C {
            data = null
        };
        XSingleton<UserItemPackageMgr>.Singleton.Each(new UserItemPackageMgr.ForeachCondition(storeyc.<>m__5CB));
        return storeyc.data;
    }

    public int GetCurUserHadPressItemNum(PressItem pressItem)
    {
        int num = 0;
        if (pressItem == null)
        {
            return num;
        }
        switch ((pressItem.affixType + 1))
        {
            case AffixType.AffixType_Card:
                return 0;

            case AffixType.AffixType_Equip:
                foreach (Card card in getInstance().CardList)
                {
                    if (card.cardInfo.entry == long.Parse(pressItem.itemId))
                    {
                        return 1;
                    }
                    num = 0;
                }
                return num;

            case AffixType.AffixType_Gem:
                foreach (Item item in getInstance().ItemList)
                {
                    if (item.entry == int.Parse(pressItem.itemId))
                    {
                        return item.num;
                    }
                    num = 0;
                }
                return num;

            case AffixType.AffixType_Gold:
                foreach (Item item2 in getInstance().ItemList)
                {
                    if (item2.entry == int.Parse(pressItem.itemId))
                    {
                        return item2.num;
                    }
                    num = 0;
                }
                return num;

            case AffixType.AffixType_Stone:
                return getInstance().Gold;

            case AffixType.AffixType_Courage:
                return getInstance().Stone;

            case AffixType.AffixType_Eq:
                return getInstance().Courage;

            case AffixType.AffixType_RealStone:
                return 0;

            case AffixType.AffixType_DonateStone:
                return 0;

            case AffixType.AffixType_PhyForce:
                return 0;

            case AffixType.AffixType_SkillBook:
                return getInstance().PhyForce;

            case AffixType.AffixType_GoldKey:
                return 0;

            case AffixType.AffixType_SilverKey:
                return getInstance().GetGoldKey().num;

            case AffixType.AffixType_CopperKey:
                return getInstance().GetSliverKey().num;

            case AffixType.AffixType_Item:
                return getInstance().GetCopperKey().num;

            case AffixType.AffixType_ArenaLadderScore:
                foreach (Item item3 in getInstance().ItemList)
                {
                    if (item3.entry == int.Parse(pressItem.itemId))
                    {
                        return item3.num;
                    }
                    num = 0;
                }
                return num;

            case AffixType.AffixType_Contribute:
                return getInstance().ArenaLadderCoin;

            case AffixType.AffixType_OutlandCoin:
                return getInstance().mGuildData.contribution;

            case AffixType.AffixType_FlameBattleCoin:
                return getInstance().OutlandCoin;

            case AffixType.AffixType_LoLArenaScore:
                return getInstance().FlamebattleCoin;

            case AffixType.AffixType_Num:
                return getInstance().ArenaChallengeCoin;

            case (AffixType.AffixType_Num | AffixType.AffixType_Equip):
                return 0;
        }
        return 0;
    }

    public List<Card> GetDefaultOutlandPkList(int outlandType)
    {
        switch (outlandType)
        {
            case 0:
                return this.mDefaultOutlandPkList_0;

            case 1:
                return this.mDefaultOutlandPkList_1;

            case 2:
                return this.mDefaultOutlandPkList_2;

            case 3:
                return this.mDefaultOutlandPkList_3;
        }
        return new List<Card>();
    }

    public int GetDupSmashTimes()
    {
        Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(0x3a8);
        if (itemByEntry != null)
        {
            return itemByEntry.num;
        }
        return 0;
    }

    public FlameBattleSelfData GetFlameCardByEntry(int cardEntry)
    {
        <GetFlameCardByEntry>c__AnonStorey282 storey = new <GetFlameCardByEntry>c__AnonStorey282 {
            cardEntry = cardEntry
        };
        FlameBattleSelfData data = this.YuanZhengCardStatList.Find(new Predicate<FlameBattleSelfData>(storey.<>m__5D1));
        if (data != null)
        {
            return data;
        }
        return null;
    }

    public void GetFormationData()
    {
        List<long> list = new List<long>();
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.Normal.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultDupHeroList.Clear();
        foreach (long num in list)
        {
            if (num > 0L)
            {
                Card cardByID = this.GetCardByID(num);
                if (cardByID != null)
                {
                    this.mDefaultDupHeroList.Add(cardByID);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.WorldBoss.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultWorldBossList.Clear();
        foreach (long num2 in list)
        {
            if (num2 > 0L)
            {
                Card item = this.GetCardByID(num2);
                if (item != null)
                {
                    this.mDefaultWorldBossList.Add(item);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.FriendPk.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultFriendPkList.Clear();
        foreach (long num3 in list)
        {
            if (num3 > 0L)
            {
                Card card3 = this.GetCardByID(num3);
                if (card3 != null)
                {
                    this.mDefaultFriendPkList.Add(card3);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.WorldCupPk.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultWorldCupPkList.Clear();
        foreach (long num4 in list)
        {
            if (num4 > 0L)
            {
                Card card4 = this.GetCardByID(num4);
                if (card4 != null)
                {
                    this.mDefaultWorldCupPkList.Add(card4);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.WarmmatchPk.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultWarmmatchList.Clear();
        foreach (long num5 in list)
        {
            if (num5 > 0L)
            {
                Card card5 = this.GetCardByID(num5);
                if (card5 != null)
                {
                    this.mDefaultWarmmatchList.Add(card5);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.TowerPk.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultTowerPkList.Clear();
        foreach (long num6 in list)
        {
            if (num6 > 0L)
            {
                Card card6 = this.GetCardByID(num6);
                if (card6 != null)
                {
                    this.mDefaultTowerPkList.Add(card6);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.YuanZhengPk.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultYuanZhengPkList.Clear();
        foreach (long num7 in list)
        {
            if (num7 > 0L)
            {
                Card card7 = this.GetCardByID(num7);
                if (card7 != null)
                {
                    this.mDefaultYuanZhengPkList.Add(card7);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.ArenaLadder.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultArenaLadderList.Clear();
        foreach (long num8 in list)
        {
            if (num8 > 0L)
            {
                Card card8 = this.GetCardByID(num8);
                if (card8 != null)
                {
                    this.mDefaultArenaLadderList.Add(card8);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.OutLandBattle_tollGate_0.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultOutlandPkList_0.Clear();
        foreach (long num9 in list)
        {
            if (num9 > 0L)
            {
                Card card9 = this.GetCardByID(num9);
                if (card9 != null)
                {
                    this.mDefaultOutlandPkList_0.Add(card9);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.OutLandBattle_tollGate_1.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultOutlandPkList_1.Clear();
        foreach (long num10 in list)
        {
            if (num10 > 0L)
            {
                Card card10 = this.GetCardByID(num10);
                if (card10 != null)
                {
                    this.mDefaultOutlandPkList_1.Add(card10);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.OutLandBattle_tollGate_2.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultOutlandPkList_2.Clear();
        foreach (long num11 in list)
        {
            if (num11 > 0L)
            {
                Card card11 = this.GetCardByID(num11);
                if (card11 != null)
                {
                    this.mDefaultOutlandPkList_2.Add(card11);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.OutLandBattle_tollGate_3.ToString().ToUpper(), string.Empty), 3L);
        this.mDefaultOutlandPkList_3.Clear();
        foreach (long num12 in list)
        {
            if (num12 > 0L)
            {
                Card card12 = this.GetCardByID(num12);
                if (card12 != null)
                {
                    this.mDefaultOutlandPkList_3.Add(card12);
                }
            }
        }
        list = StrParser.ParseLongList(SettingMgr.mInstance.GetFormation(BattleType.GuildBattle.ToString().ToUpper(), string.Empty), 0L);
        this.mDefaultGuildBattleList.Clear();
        foreach (long num13 in list)
        {
            if (num13 > 0L)
            {
                Card card13 = this.GetCardByID(num13);
                if (card13 != null)
                {
                    this.mDefaultGuildBattleList.Add(card13);
                }
            }
        }
    }

    public List<Card> GetFormationListByType(BattleType type)
    {
        if (type == BattleType.WarmmatchPk)
        {
            return this.mDefaultWarmmatchList;
        }
        if (type == BattleType.WorldCupPk)
        {
            return this.mDefaultWorldCupPkList;
        }
        if (type == BattleType.FriendPk)
        {
            return this.mDefaultFriendPkList;
        }
        if (type == BattleType.TowerPk)
        {
            return this.mDefaultTowerPkList;
        }
        if (type != BattleType.Dungeons)
        {
            if (type == BattleType.ArenaLadder)
            {
                return this.mDefaultArenaLadderList;
            }
            if (type == BattleType.YuanZhengPk)
            {
                return this.mDefaultYuanZhengPkList;
            }
            if (type == BattleType.OutLandBattle_tollGate_0)
            {
                return this.mDefaultOutlandPkList_0;
            }
            if (type == BattleType.OutLandBattle_tollGate_1)
            {
                return this.mDefaultOutlandPkList_1;
            }
            if (type == BattleType.OutLandBattle_tollGate_2)
            {
                return this.mDefaultOutlandPkList_2;
            }
            if (type == BattleType.OutLandBattle_tollGate_3)
            {
                return this.mDefaultOutlandPkList_3;
            }
            if (type == BattleType.WorldBoss)
            {
                return this.mDefaultWorldBossList;
            }
            if (type == BattleType.WarmmatchDefense)
            {
                List<Card> list = new List<Card>();
                foreach (long num in this.ArenaFormation.card_id)
                {
                    Card cardByID = this.GetCardByID(num);
                    if (cardByID != null)
                    {
                        list.Add(cardByID);
                    }
                }
                list.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
                return list;
            }
            if (type == BattleType.ChallengeDefense)
            {
                List<Card> list2 = new List<Card>();
                foreach (long num2 in this.ChallengeArenaFormation.card_id)
                {
                    Card item = this.GetCardByID(num2);
                    if (item != null)
                    {
                        list2.Add(item);
                    }
                }
                list2.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
                return list2;
            }
            if (type == BattleType.WorldCupDefense)
            {
                List<Card> list3 = new List<Card>();
                foreach (long num3 in this.WorldCupFormation.card_id)
                {
                    Card card3 = this.GetCardByID(num3);
                    if (card3 != null)
                    {
                        list3.Add(card3);
                    }
                }
                list3.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
                return list3;
            }
            if (type == BattleType.GuildBattle)
            {
                return this.mDefaultGuildBattleList;
            }
            if (type == BattleType.GuildDup)
            {
                return this.mDefaultGuildDupBattleList;
            }
            if (type == BattleType.DerainsDartInterceptBattle)
            {
                return this.mDefaultDetainsDart_Intercept_List;
            }
            if (type == BattleType.DetainsDartBattleBack)
            {
                return this.mDefaultDetainsDart_BattleBackSelf_List;
            }
        }
        return this.mDefaultDupHeroList;
    }

    public List<Item> GetFragmentListByType(ShowItemType _type)
    {
        List<ItemCategory> types = new List<ItemCategory>();
        switch (_type)
        {
            case ShowItemType.All:
                types.Add(ItemCategory.ItemCategory_Card_Chip);
                types.Add(ItemCategory.ItemCategory_Material);
                break;

            case ShowItemType.Frag_Card:
                types.Add(ItemCategory.ItemCategory_Card_Chip);
                break;

            case ShowItemType.Frag_Equip:
                types.Add(ItemCategory.ItemCategory_Material);
                break;
        }
        return XSingleton<UserItemPackageMgr>.Singleton.GetItemsByTypes(types);
    }

    public Item GetGoldKey()
    {
        <GetGoldKey>c__AnonStorey27A storeya = new <GetGoldKey>c__AnonStorey27A {
            data = null
        };
        XSingleton<UserItemPackageMgr>.Singleton.Each(new UserItemPackageMgr.ForeachCondition(storeya.<>m__5C9));
        return storeya.data;
    }

    public static ActorData getInstance()
    {
        if (instance == null)
        {
            instance = new ActorData();
        }
        return instance;
    }

    public Item GetItemByEntry(int _entry)
    {
        return XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_entry);
    }

    public List<Item> GetItemListByType(ShowItemType _type)
    {
        List<ItemCategory> types = new List<ItemCategory>();
        switch (_type)
        {
            case ShowItemType.All:
                types.Add(ItemCategory.ItemCategory_Equip_Scroll);
                types.Add(ItemCategory.ItemCategory_Exp);
                types.Add(ItemCategory.ItemCategory_Gold);
                types.Add(ItemCategory.ItemCategory_Smash_Voucher);
                types.Add(ItemCategory.ItemCategory_Title_Item);
                types.Add(ItemCategory.ItemCategory_Key);
                types.Add(ItemCategory.ItemCategory_Drop_Package);
                types.Add(ItemCategory.ItemCategory_CardGem);
                types.Add(ItemCategory.ItemCategory_Ticket);
                break;

            case ShowItemType.Material:
                types.Add(ItemCategory.ItemCategory_Equip_Scroll);
                break;

            case ShowItemType.Consumable:
                types.Add(ItemCategory.ItemCategory_Exp);
                types.Add(ItemCategory.ItemCategory_Smash_Voucher);
                types.Add(ItemCategory.ItemCategory_Title_Item);
                types.Add(ItemCategory.ItemCategory_Key);
                types.Add(ItemCategory.ItemCategory_Drop_Package);
                break;

            case ShowItemType.QiTa:
                types.Add(ItemCategory.ItemCategory_Gold);
                break;

            case ShowItemType.Gem:
                types.Add(ItemCategory.ItemCategory_CardGem);
                break;

            case ShowItemType.Ticket:
                types.Add(ItemCategory.ItemCategory_Ticket);
                break;
        }
        return XSingleton<UserItemPackageMgr>.Singleton.GetItemsByTypes(types);
    }

    public bool GetNeedItemIsOkOrNot(List<Item> needDecItems)
    {
        bool flag = false;
        if (needDecItems.Count > 0)
        {
            foreach (Item item in needDecItems)
            {
                Item itemByEntry = this.GetItemByEntry(item.entry);
                if (itemByEntry != null)
                {
                    if (itemByEntry.num >= item.num)
                    {
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return flag;
        }
        flag = true;
        Debug.LogWarning("S2C_CollectExchange needs item is null,so can exchange it! ");
        return flag;
    }

    public bool GetNeedPressItemIsOkOrNot(PressItem[] pressItems)
    {
        bool flag = false;
        if (pressItems.Length > 0)
        {
            foreach (PressItem item in pressItems)
            {
                bool flag2 = false;
                switch ((item.affixType + 1))
                {
                    case AffixType.AffixType_Card:
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_Equip:
                        foreach (Card card in getInstance().CardList)
                        {
                            if (card.cardInfo.entry == int.Parse(item.itemId))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        goto Label_03EC;

                    case AffixType.AffixType_Gem:
                        foreach (Item item2 in getInstance().ItemList)
                        {
                            if ((item2.entry == int.Parse(item.itemId)) && (item2.num >= item.itemNum))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        goto Label_03EC;

                    case AffixType.AffixType_Gold:
                        foreach (Item item3 in getInstance().ItemList)
                        {
                            if ((item3.entry == int.Parse(item.itemId)) && (item3.num >= item.itemNum))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        goto Label_03EC;

                    case AffixType.AffixType_Stone:
                        if (getInstance().Gold < item.itemNum)
                        {
                            break;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_Courage:
                        if (getInstance().Stone < item.itemNum)
                        {
                            goto Label_0221;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_Eq:
                        if (getInstance().Courage < item.itemNum)
                        {
                            goto Label_0246;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_RealStone:
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_DonateStone:
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_PhyForce:
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_SkillBook:
                        if (getInstance().PhyForce < item.itemNum)
                        {
                            goto Label_0283;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_GoldKey:
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_SilverKey:
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_CopperKey:
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_Item:
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_ArenaLadderScore:
                        foreach (Item item4 in getInstance().ItemList)
                        {
                            if ((item4.entry == int.Parse(item.itemId)) && (item4.num >= item.itemNum))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        goto Label_03EC;

                    case AffixType.AffixType_Contribute:
                        if (getInstance().ArenaLadderCoin < item.itemNum)
                        {
                            goto Label_033B;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_OutlandCoin:
                        if (getInstance().mUserGuildMemberData.contribution < item.itemNum)
                        {
                            goto Label_038A;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_FlameBattleCoin:
                        if (getInstance().OutlandCoin < item.itemNum)
                        {
                            goto Label_03AF;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_LoLArenaScore:
                        if (getInstance().FlamebattleCoin < item.itemNum)
                        {
                            goto Label_03D4;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case AffixType.AffixType_Num:
                        if (getInstance().ArenaChallengeCoin < item.itemNum)
                        {
                            goto Label_0360;
                        }
                        flag2 = true;
                        goto Label_03EC;

                    case (AffixType.AffixType_Num | AffixType.AffixType_Equip):
                        flag2 = true;
                        goto Label_03EC;

                    default:
                        flag2 = true;
                        goto Label_03EC;
                }
                flag2 = false;
                goto Label_03EC;
            Label_0221:
                flag2 = false;
                goto Label_03EC;
            Label_0246:
                flag2 = false;
                goto Label_03EC;
            Label_0283:
                flag2 = false;
                goto Label_03EC;
            Label_033B:
                flag2 = false;
                goto Label_03EC;
            Label_0360:
                flag2 = false;
                goto Label_03EC;
            Label_038A:
                flag2 = false;
                goto Label_03EC;
            Label_03AF:
                flag2 = false;
                goto Label_03EC;
            Label_03D4:
                flag2 = false;
            Label_03EC:
                if (!flag2)
                {
                    return false;
                }
                flag = true;
            }
        }
        return flag;
    }

    public bool GetNeedPressItemIsOkOrNot(ActiveInfo info)
    {
        bool flag = false;
        for (int i = 0; i < info.rewards_configs.Count; i++)
        {
            if (info.rewards_configs[i].exchangeNeedConfig == null)
            {
                return false;
            }
            if (info.rewards_configs[i].exchangeNeedConfig.Count > 0)
            {
                foreach (TxActivityCollectConfig config in info.rewards_configs[i].exchangeNeedConfig)
                {
                    bool flag2 = false;
                    switch ((config.affixType + 1))
                    {
                        case AffixType.AffixType_Card:
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_Equip:
                            foreach (Card card in getInstance().CardList)
                            {
                                if (card.cardInfo.entry == config.collectId)
                                {
                                    flag2 = true;
                                    break;
                                }
                            }
                            goto Label_0423;

                        case AffixType.AffixType_Gem:
                            foreach (Item item in getInstance().ItemList)
                            {
                                if ((item.entry == config.collectId) && (item.num >= config.collectCount))
                                {
                                    flag2 = true;
                                    break;
                                }
                            }
                            goto Label_0423;

                        case AffixType.AffixType_Gold:
                            foreach (Item item2 in getInstance().ItemList)
                            {
                                if ((item2.entry == config.collectId) && (item2.num >= config.collectCount))
                                {
                                    flag2 = true;
                                    break;
                                }
                            }
                            goto Label_0423;

                        case AffixType.AffixType_Stone:
                            if (getInstance().Gold < config.collectCount)
                            {
                                break;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_Courage:
                            if (getInstance().Stone < config.collectCount)
                            {
                                goto Label_025D;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_Eq:
                            if (getInstance().Courage < config.collectCount)
                            {
                                goto Label_0282;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_RealStone:
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_DonateStone:
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_PhyForce:
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_SkillBook:
                            if (getInstance().PhyForce < config.collectCount)
                            {
                                goto Label_02BF;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_GoldKey:
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_SilverKey:
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_CopperKey:
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_Item:
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_ArenaLadderScore:
                            foreach (Item item3 in getInstance().ItemList)
                            {
                                if ((item3.entry == config.collectId) && (item3.num >= config.collectCount))
                                {
                                    flag2 = true;
                                    break;
                                }
                            }
                            goto Label_0423;

                        case AffixType.AffixType_Contribute:
                            if (getInstance().ArenaLadderCoin < config.collectCount)
                            {
                                goto Label_0372;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_OutlandCoin:
                            if (getInstance().mUserGuildMemberData.contribution < config.collectCount)
                            {
                                goto Label_03C1;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_FlameBattleCoin:
                            if (getInstance().OutlandCoin < config.collectCount)
                            {
                                goto Label_03E6;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_LoLArenaScore:
                            if (getInstance().FlamebattleCoin < config.collectCount)
                            {
                                goto Label_040B;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case AffixType.AffixType_Num:
                            if (getInstance().ArenaChallengeCoin < config.collectCount)
                            {
                                goto Label_0397;
                            }
                            flag2 = true;
                            goto Label_0423;

                        case (AffixType.AffixType_Num | AffixType.AffixType_Equip):
                            flag2 = true;
                            goto Label_0423;

                        default:
                            flag2 = true;
                            goto Label_0423;
                    }
                    flag2 = false;
                    goto Label_0423;
                Label_025D:
                    flag2 = false;
                    goto Label_0423;
                Label_0282:
                    flag2 = false;
                    goto Label_0423;
                Label_02BF:
                    flag2 = false;
                    goto Label_0423;
                Label_0372:
                    flag2 = false;
                    goto Label_0423;
                Label_0397:
                    flag2 = false;
                    goto Label_0423;
                Label_03C1:
                    flag2 = false;
                    goto Label_0423;
                Label_03E6:
                    flag2 = false;
                    goto Label_0423;
                Label_040B:
                    flag2 = false;
                Label_0423:
                    if (!flag2)
                    {
                        flag = false;
                        break;
                    }
                    flag = true;
                }
            }
            if (flag)
            {
                break;
            }
            flag = false;
        }
        Debug.LogWarning("bNeedItemsIsOk____________________:" + flag);
        return flag;
    }

    public OutlandBattleBackCardInfo GetOutlandCardByEntry(int cardEntry)
    {
        <GetOutlandCardByEntry>c__AnonStorey284 storey = new <GetOutlandCardByEntry>c__AnonStorey284 {
            cardEntry = cardEntry
        };
        OutlandBattleBackCardInfo info = this.OutlandCardStatList.Find(new Predicate<OutlandBattleBackCardInfo>(storey.<>m__5D3));
        if (info != null)
        {
            return info;
        }
        return null;
    }

    public int GetPhyForceFullAllTime()
    {
        if (this.UserInfo != null)
        {
            return ((this.UserInfo.maxPhyForce - this.PhyForce) * 300);
        }
        return -1;
    }

    public int GetSkillLev(long Cardid, int _Pos)
    {
        Card cardByID = this.GetCardByID(Cardid);
        if (cardByID != null)
        {
            foreach (SkillInfo info in cardByID.cardInfo.skillInfo)
            {
                if (info.skillPos == _Pos)
                {
                    return info.skillLevel;
                }
            }
        }
        return 0;
    }

    public int GetSkillPointFullTime()
    {
        return (((this.mSkillPoint.maxPoint - this.mSkillPoint.totalPoint) * 360) - this.mSkillPoint.passTime);
    }

    public Item GetSliverKey()
    {
        <GetSliverKey>c__AnonStorey27B storeyb = new <GetSliverKey>c__AnonStorey27B {
            data = null
        };
        XSingleton<UserItemPackageMgr>.Singleton.Each(new UserItemPackageMgr.ForeachCondition(storeyb.<>m__5CA));
        return storeyb.data;
    }

    public int GetTeamPowerByCardList(List<Card> cardList)
    {
        List<long> cardIdList = new List<long>();
        foreach (Card card in cardList)
        {
            cardIdList.Add(card.card_id);
        }
        return this.GetTeamPowerByIdList(cardIdList);
    }

    public int GetTeamPowerByIdList(List<long> cardIdList)
    {
        int num = 0;
        foreach (long num2 in cardIdList)
        {
            int num3 = -1;
            if (this.CardFightPowerDict.TryGetValue(num2, out num3))
            {
                num += this.CardFightPowerDict[num2];
            }
            else
            {
                num += XSingleton<SocialFriend>.Singleton.GetPowerByUserID(num2);
            }
        }
        return num;
    }

    public Item GetTicketItemBySubType(TicketType type)
    {
        List<ItemCategory> types = new List<ItemCategory> { 11 };
        List<Item> itemsByTypes = XSingleton<UserItemPackageMgr>.Singleton.GetItemsByTypes(types);
        if (itemsByTypes.Count > 0)
        {
            for (int i = 0; i < itemsByTypes.Count; i++)
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(itemsByTypes[i].entry);
                if ((_config != null) && (_config.ticket_sub_type == type))
                {
                    return itemsByTypes[i];
                }
            }
        }
        return null;
    }

    public bool HaveInAcitveCard()
    {
        List<int> list = new List<int>();
        foreach (Card card in getInstance().CardList)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.cardInfo.entry);
            if (_config != null)
            {
                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(_config.entry, _config.evolve_lv);
                if (cardExCfg != null)
                {
                    list.Add(cardExCfg.item_entry);
                }
            }
        }
        IEnumerator enumerator = ConfigMgr.getInstance().getList<card_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                card_config current = (card_config) enumerator.Current;
                <HaveInAcitveCard>c__AnonStorey280 storey = new <HaveInAcitveCard>c__AnonStorey280();
                if (current.is_show)
                {
                    storey.cec = CommonFunc.GetCardExCfg(current.entry, current.evolve_lv);
                    if (((storey.cec != null) && !list.Contains(storey.cec.item_entry)) && (getInstance().ItemList.Find(new Predicate<Item>(storey.<>m__5CF)) != null))
                    {
                        return true;
                    }
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

    public bool HaveNewMail()
    {
        foreach (Mail mail in this.mMailList)
        {
            if (SettingMgr.mInstance.GetMailInt(mail.mail_id.ToString(), 0) == 0)
            {
                return true;
            }
        }
        return false;
    }

    public void Init(Session _session)
    {
        this.SessionInfo = _session;
    }

    public void InitAddPhyForce(int passTime)
    {
        this.startAddPhyForce = true;
        this.deltaStampTime = passTime;
        this.lastUpdateTime = TimeMgr.Instance.ServerStampTime;
        this.coolDownUsedTime = passTime;
        this.lastCalAddForceTime = this.lastUpdateTime - passTime;
    }

    private void InsetAdMsg(string str)
    {
        this.mAdList.Add(str);
        if (this.mAdList.Count >= 100)
        {
            this.mAdList.RemoveAt(0);
        }
    }

    private void InsetChatMsg(FastBuf.Broadcast bc)
    {
        this.mChatList.Add(bc);
        if (this.mChatList.Count >= 50)
        {
            this.mChatList.RemoveAt(0);
        }
    }

    public bool IsInited()
    {
        return (this.SessionInfo != null);
    }

    public bool IsLivenessDataDirty()
    {
        return this.DirtyLiveness;
    }

    public bool IsQuestDataDirty()
    {
        return this.DirtyQuestList;
    }

    public void MakeLivenessDataDirty()
    {
        this.DirtyLiveness = true;
    }

    public void MakeQuestDataDirty()
    {
        this.DirtyQuestList = true;
    }

    public void OnExit()
    {
        this.startAddPhyForce = false;
        this.TrenchNormalDataDict.Clear();
        this.guildMsgItems.Clear();
    }

    public void OnLoadDataFinish()
    {
        if (this.IsWaitingLoadData)
        {
            getInstance().LoadingUserInfoProgress = 1f;
            this.IsWaitingLoadData = false;
            LoadingPerfab.EndTransition();
            GameStateMgr.Instance.ChangeState("LOGIN_COMMUNITY_EVENT");
        }
    }

    public string PeekBroadMsg()
    {
        string str = string.Empty;
        if (this.mBroadcastList.Count > 0)
        {
            FastBuf.Broadcast bc = this.mBroadcastList[0];
            if (bc.userId > 0L)
            {
                str = "[f79646]" + bc.userName + "[ffc000]:" + ConfigMgr.getInstance().GetMaskWord(bc.content);
            }
            else
            {
                str = "[f79646]" + bc.userName + "[ffc000]:" + bc.content;
            }
            this.InsetChatMsg(bc);
            this.mBroadcastList.RemoveAt(0);
        }
        return str;
    }

    public bool ProductCharged(string key)
    {
        char[] separator = new char[] { '|' };
        foreach (string str in this.UserInfo.charge_product_id_list.Split(separator))
        {
            if (str == key)
            {
                return true;
            }
        }
        return false;
    }

    private void PushNotification()
    {
        this.PushSkillFullNotify();
        if (((this.PushSendTiliFullMsg == 0) && (this.PhyForce >= this.UserInfo.maxPhyForce)) && (Time.realtimeSinceStartup > 900f))
        {
            this.PushSendTiliFullMsg = 1;
            PushMgr.mInstance.addNotification(0, ConfigMgr.getInstance().GetWord(0xa95f60), ConfigMgr.getInstance().GetWord(0xa95f61), 3);
            Debug.Log("PushSend Msg ----------------->" + ConfigMgr.getInstance().GetWord(0xa95f61));
        }
        if (GameDataMgr.Instance.boostRecruit.valid)
        {
            long time = 0L;
            if (((this.PushSendBoostRecruit == 0) && SettingMgr.mInstance.FreeTakeCardPush) && GameDataMgr.Instance.boostRecruit.FreeTime(0, out time))
            {
                this.PushSendBoostRecruit = 1;
                PushMgr.mInstance.addNotification(1, ConfigMgr.getInstance().GetWord(0xa95f60), ConfigMgr.getInstance().GetWord(0xa95f62), 15);
                Debug.Log("PushSend Msg ----------------->" + ConfigMgr.getInstance().GetWord(0xa95f62));
            }
        }
    }

    private void PushSkillFullNotify()
    {
        if ((this.mSkillPoint != null) && ((this.PushSendSkillPointFull == 0) && (this.mSkillPoint.totalPoint >= this.mSkillPoint.maxPoint)))
        {
            this.PushSendSkillPointFull = 1;
            PushMgr.mInstance.addNotification(0, ConfigMgr.getInstance().GetWord(0xa95f60), ConfigMgr.getInstance().GetWord(0xa95f65), 3);
            Debug.Log("PushSend Msg ----------------->" + ConfigMgr.getInstance().GetWord(0xa95f65));
        }
    }

    public void ReadShopTips()
    {
        this.mTodayOpenGoblinShopTips = SettingMgr.mInstance.GetOpenShopTips("GOBLIN", 0) > 0;
        this.mTodayOpenSecretShopTips = SettingMgr.mInstance.GetOpenShopTips("SECRET", 0) > 0;
    }

    public void reqActiveCompleteIsOrNot()
    {
        SocketMgr.Instance.RequestRewardFlag();
    }

    public void reqNewActiveDataInfo()
    {
        SocketMgr.Instance.GetActiveList();
        Debug.LogWarning("reqNewActiveDataInfo!!!!!!!!!!!!!!");
    }

    public void RequestAllInfo()
    {
        SocketMgr.Instance.RequestGetUserInfo();
        XSingleton<SocialFriend>.Singleton.Init();
        XSingleton<SocialFriend>.Singleton.RequestGameServerForGetFriendList();
        XSingleton<LifeSkillManager>.Singleton.Init();
        XSingleton<GameGuildMgr>.Singleton.RequestDupState();
    }

    public void RequestOtherInfo()
    {
        SocketMgr.Instance.RequestGetDuplicateProgress();
        SocketMgr.Instance.RequestGetBattleFormation(BattleFormationType.BattleFormationType_Arena_Def);
        SocketMgr.Instance.RequestGetBattleFormation(BattleFormationType.BattleFormationType_League_Def);
        SocketMgr.Instance.RequestChallengeFormation();
        SocketMgr.Instance.RequestGetItemList();
        SocketMgr.Instance.RequestCardBag();
        SocketMgr.Instance.RequestGetFriendList();
        SocketMgr.Instance.RequestGetFriendReqList();
        SocketMgr.Instance.RequestGetTitleList();
        SocketMgr.Instance.RequestGuildData(false, null);
        SocketMgr.Instance.RequestBroadcastList();
        SocketMgr.Instance.RequestVoidTower(0);
        SocketMgr.Instance.RequestDupRewardInfo();
        SocketMgr.Instance.SendQueryTxBalance();
        this.IsWaitingLoadData = true;
        this.remainTimeOfWaitingLoadData = 10f;
        SocketMgr.Instance.SendQueryVIP();
        SocketMgr.Instance.SendQueryCaifuTong();
        SocketMgr.Instance.RequestRewardFlag();
        SocketMgr.Instance.RequestSyncChangeData();
        PayMgr.Instance.RequstMPInfo();
    }

    public void SaveFormationData(BattleType _type)
    {
        string str;
        switch (_type)
        {
            case BattleType.Normal:
                str = this.TransListToStr(this.mDefaultDupHeroList);
                SettingMgr.mInstance.SetFormation(BattleType.Normal.ToString().ToUpper(), str);
                break;

            case BattleType.WorldBoss:
                str = this.TransListToStr(this.mDefaultWorldBossList);
                SettingMgr.mInstance.SetFormation(BattleType.WorldBoss.ToString().ToUpper(), str);
                break;

            case BattleType.FriendPk:
                str = this.TransListToStr(this.mDefaultFriendPkList);
                SettingMgr.mInstance.SetFormation(BattleType.FriendPk.ToString().ToUpper(), str);
                break;

            case BattleType.WorldCupPk:
                str = this.TransListToStr(this.mDefaultWorldCupPkList);
                SettingMgr.mInstance.SetFormation(BattleType.WorldCupPk.ToString().ToUpper(), str);
                break;

            case BattleType.WarmmatchPk:
                str = this.TransListToStr(this.mDefaultWarmmatchList);
                SettingMgr.mInstance.SetFormation(BattleType.WarmmatchPk.ToString().ToUpper(), str);
                break;

            case BattleType.TowerPk:
                str = this.TransListToStr(this.mDefaultTowerPkList);
                SettingMgr.mInstance.SetFormation(BattleType.TowerPk.ToString().ToUpper(), str);
                break;

            case BattleType.YuanZhengPk:
                str = this.TransListToStr(this.mDefaultYuanZhengPkList);
                SettingMgr.mInstance.SetFormation(BattleType.YuanZhengPk.ToString().ToUpper(), str);
                break;

            case BattleType.ArenaLadder:
                str = this.TransListToStr(this.mDefaultArenaLadderList);
                SettingMgr.mInstance.SetFormation(BattleType.ArenaLadder.ToString().ToUpper(), str);
                break;

            case BattleType.GuildBattle:
                str = this.TransListToStr(this.mDefaultGuildBattleList);
                SettingMgr.mInstance.SetFormation(BattleType.GuildBattle.ToString().ToUpper(), str);
                break;

            case BattleType.OutLandBattle_tollGate_0:
                str = this.TransListToStr(this.mDefaultOutlandPkList_0);
                SettingMgr.mInstance.SetFormation(BattleType.OutLandBattle_tollGate_0.ToString().ToUpper(), str);
                break;

            case BattleType.OutLandBattle_tollGate_1:
                str = this.TransListToStr(this.mDefaultOutlandPkList_1);
                SettingMgr.mInstance.SetFormation(BattleType.OutLandBattle_tollGate_1.ToString().ToUpper(), str);
                break;

            case BattleType.OutLandBattle_tollGate_2:
                str = this.TransListToStr(this.mDefaultOutlandPkList_2);
                SettingMgr.mInstance.SetFormation(BattleType.OutLandBattle_tollGate_2.ToString().ToUpper(), str);
                break;

            case BattleType.OutLandBattle_tollGate_3:
                str = this.TransListToStr(this.mDefaultOutlandPkList_3);
                SettingMgr.mInstance.SetFormation(BattleType.OutLandBattle_tollGate_3.ToString().ToUpper(), str);
                break;
        }
        SettingMgr.mInstance.SaveSetting();
    }

    public void SetAllFriendTiLiState(List<Friend> _friendList)
    {
        <SetAllFriendTiLiState>c__AnonStorey27E storeye = new <SetAllFriendTiLiState>c__AnonStorey27E();
        using (List<Friend>.Enumerator enumerator = _friendList.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                storeye.f = enumerator.Current;
                Friend friend = this.mFriendList.Find(new Predicate<Friend>(storeye.<>m__5CD));
                if (friend != null)
                {
                    friend.isGivePhyForceNow = storeye.f.isGivePhyForceNow;
                }
            }
        }
    }

    public void SetGuildData(GuildData _GData, GuildMemberData gmData)
    {
        this.mGuildData = _GData;
        this.bGuidLvChange__ = true;
        this.SetGuildMemberData(gmData);
    }

    public void SetGuildData(GuildData _GData, UserGuildMemberData _UGMData)
    {
        this.mGuildData = _GData;
        this.bGuidLvChange__ = true;
        this.mUserGuildMemberData = _UGMData;
    }

    public void SetGuildData(GuildData _GData, UserGuildMemberData _UGMData, GuildMemberData gmData)
    {
        this.mGuildData = _GData;
        this.bGuidLvChange__ = true;
        this.mUserGuildMemberData = _UGMData;
        this.SetGuildMemberData(gmData);
    }

    public void SetGuildMemberData(GuildMemberData gmData)
    {
        this.mGuildMemberData = gmData;
        this.mGuildMemberData.member.Sort(new CompareGuildMebmer());
    }

    public void SetLingQuTiLiState(long targetId)
    {
        <SetLingQuTiLiState>c__AnonStorey27F storeyf = new <SetLingQuTiLiState>c__AnonStorey27F {
            targetId = targetId
        };
        Friend friend = this.mFriendList.Find(new Predicate<Friend>(storeyf.<>m__5CE));
        if (friend != null)
        {
            friend.isGivePhyForceNow = false;
        }
    }

    public void SetVIP(VipLevel vip)
    {
        this.isVipChange__ = true;
        this.mUserInfo.vip_level.level = vip.level + 1;
        this.mUserInfo.vip_level.change_stone = vip.change_stone;
        this.mUserInfo.dup_smash_times = vip.dup_smash_times;
        this.mUserInfo.max_phy_buy_times = vip.max_phy_buy_times;
        this.mUserInfo.void_tower_times = vip.void_tower;
        MainUI gUIEntity = GUIMgr.Instance.GetGUIEntity<MainUI>();
        if (null != gUIEntity)
        {
            gUIEntity.UpdateVip();
        }
        TowerPanel panel = GUIMgr.Instance.GetGUIEntity<TowerPanel>();
        if (null != panel)
        {
            panel.ResetRemainCount(this.mUserInfo.void_tower_times);
        }
        VipCardPanel panel2 = GUIMgr.Instance.GetGUIEntity<VipCardPanel>();
        if (null != panel2)
        {
            panel2.Refresh();
        }
        YuanZhengPanel panel3 = GUIMgr.Instance.GetGUIEntity<YuanZhengPanel>();
        if (null != panel3)
        {
            panel3.UpdateShuTuiBtnStat();
        }
        GameDataMgr.Instance.DirtyActorStage = true;
    }

    public void SetZhenSongTiLiState(long targetId)
    {
        foreach (Friend friend in this.mFriendList)
        {
            if (targetId == 0)
            {
                friend.alreadyGivePhyForceToday = true;
            }
            else if (friend.userInfo.id == targetId)
            {
                friend.alreadyGivePhyForceToday = true;
                break;
            }
        }
        FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.UpdateFriendByID(targetId);
        }
    }

    private void SkillPointTick()
    {
        if ((this.mSkillPoint != null) && (this.lastCalAddSkillTime >= 0))
        {
            int serverStampTime = TimeMgr.Instance.ServerStampTime;
            int num2 = serverStampTime - this.lastCalAddSkillTime;
            int num3 = 360;
            if (num2 > num3)
            {
                this.lastCalAddSkillTime = serverStampTime;
                int totalPoint = this.mSkillPoint.totalPoint;
                int min = 1;
                int num6 = Mathf.Clamp(min * (num2 / num3), min, this.mSkillPoint.maxPoint - this.mSkillPoint.totalPoint);
                this.mSkillPoint.totalPoint += num6;
            }
            this.NextGetSkillTime = num3 - (num2 % num3);
        }
    }

    public bool TargetInFriendList(long id)
    {
        foreach (Friend friend in this.mFriendList)
        {
            if (friend.userInfo.id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void Tick()
    {
        this.m_time += Time.deltaTime;
        if (this.m_time > this.m_updateInterval)
        {
            this.m_time = 0f;
            this.UpdateDataInfo();
        }
        if (this.IsWaitingLoadData)
        {
            this.remainTimeOfWaitingLoadData -= Time.deltaTime;
            if (this.remainTimeOfWaitingLoadData < 0f)
            {
                Debug.LogWarning("remainTimeOfWaitingLoadData is out!");
                this.OnLoadDataFinish();
            }
        }
    }

    public string TransListToStr(List<Card> _List)
    {
        string str = string.Empty;
        this.CheckFormatList(_List);
        int num = 0;
        foreach (Card card in _List)
        {
            if (num > 5)
            {
                Debug.LogWarning("Save Card Is Too More!");
            }
            else
            {
                str = str + card.card_id + ",";
                num++;
            }
        }
        return str;
    }

    public void TwentyClockRefreshData()
    {
        this.reqNewActiveDataInfo();
        this.reqActiveCompleteIsOrNot();
    }

    public void UpateCardFightPower(List<long> cardIdList, List<int> cardPowerList)
    {
        for (int i = 0; i < cardIdList.Count; i++)
        {
            int num2 = -1;
            if (this.CardFightPowerDict.TryGetValue(cardIdList[i], out num2))
            {
                this.CardFightPowerDict[cardIdList[i]] = cardPowerList[i];
            }
            else
            {
                this.CardFightPowerDict.Add(cardIdList[i], cardPowerList[i]);
            }
        }
    }

    public void UpdateActiveData(S2C_CollectExchange res)
    {
        foreach (Item item in res.exchangeDecItems)
        {
            this.UpdateItem(item);
        }
        this.FlamebattleCoin = res.rewards.flamebattleCoin;
        this.ArenaLadderCoin = res.rewards.arenaLadderScore;
        this.ArenaChallengeCoin = res.rewards.lolarenaScore;
        this.OutlandCoin = res.rewards.outland_coin;
        this.Stone = res.rewards.stone;
        this.Gold = res.rewards.gold;
        this.PhyForce += (int) res.rewards.addPhyForce;
        foreach (NewCard card in res.rewards.newCardList)
        {
            this.UpdateNewCard(card);
        }
        foreach (Item item2 in res.rewards.item)
        {
            this.UpdateItem(item2);
        }
    }

    public void UpdateActiveData(S2C_TXBuyStoreItem res)
    {
        this.Stone = res.rewards.stone;
        this.Gold = res.rewards.gold;
        this.PhyForce += (int) res.rewards.addPhyForce;
        foreach (NewCard card in res.rewards.newCardList)
        {
            this.UpdateNewCard(card);
        }
        foreach (Item item in res.rewards.item)
        {
            this.UpdateItem(item);
        }
    }

    public void UpdateActiveData0(S2C_DrawActivityPrize res)
    {
        this.Stone = res.rewards.stone;
        this.Gold = res.rewards.gold;
        this.PhyForce += (int) res.rewards.addPhyForce;
        foreach (NewCard card in res.rewards.newCardList)
        {
            this.UpdateNewCard(card);
        }
        foreach (Item item in res.rewards.item)
        {
            this.UpdateItem(item);
        }
    }

    public void UpdateAssistToTowerData()
    {
        foreach (AssistUser user in this.AssistUserList)
        {
            if (!this.CardFightPowerDict.ContainsKey(user.userInfo.id))
            {
                this.CardFightPowerDict.Add(user.userInfo.id, user.power);
            }
            else
            {
                this.CardFightPowerDict[user.userInfo.id] = user.power;
            }
        }
    }

    public void UpdateBattleEndData()
    {
        this.UpdateYuanZhengCardList(this.mCurrAttackersList);
        foreach (FlameBattleBackCardInfo info in this.mCurrdefendersList)
        {
            this.UpdateFlameTargetCardInfo(info.card_entry, info.card_cur_hp, info.card_cur_energy);
        }
    }

    public void UpdateBattleRewardData(BattleReward _BRData)
    {
        GameDataMgr.Instance.DirtyActorStage = true;
        if (_BRData.cards != null)
        {
            this.UpdateNewCard(_BRData.cards);
        }
        if (_BRData.items != null)
        {
            foreach (Item item in _BRData.items)
            {
                this.UpdateItem(item);
            }
        }
        if (_BRData.copper_key > 0)
        {
            this.mUserInfo.copperKey += (int) _BRData.copper_key;
        }
        if (_BRData.gold_key > 0)
        {
            this.mUserInfo.gold += (int) _BRData.gold_key;
        }
        if (_BRData.copper_key > 0)
        {
            this.mUserInfo.silverKey += (int) _BRData.silver_key;
        }
        this.Courage += (int) _BRData.courage;
        this.Gold += (int) _BRData.gold;
        this.mUserInfo.exp += (int) _BRData.exp;
        this.Stone += (int) _BRData.stone;
        this.mUserInfo.wine += (int) _BRData.wine;
    }

    public void UpdateCard(Card _card)
    {
        if (_card != null)
        {
            Card cardByEntry = this.GetCardByEntry(_card.cardInfo.entry);
            if (cardByEntry == null)
            {
                this.mCards.Add(_card);
                List<long> cardIdList = new List<long> {
                    _card.card_id
                };
                SocketMgr.Instance.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            }
            else
            {
                int index = this.mCards.IndexOf(cardByEntry);
                this.mCards[index] = _card;
            }
            GameDataMgr.Instance.DirtyActorStage = true;
            CardPool.SynCardObject((int) _card.cardInfo.entry, _card, 3);
        }
    }

    public void UpdateCardList(List<Card> _cardList)
    {
        foreach (Card card in _cardList)
        {
            this.UpdateCard(card);
        }
    }

    public void UpdateCardSkill(long _cardid, int _skillindex, int _skillLev)
    {
        Card cardByID = this.GetCardByID(_cardid);
        int index = this.mCards.IndexOf(cardByID);
        foreach (SkillInfo info in cardByID.cardInfo.skillInfo)
        {
            if (info.skillPos == _skillindex)
            {
                info.skillLevel = _skillLev;
            }
        }
        this.mCards[index] = cardByID;
        if (cardByID != null)
        {
            GameDataMgr.Instance.DirtyActorStage = true;
            CardPool.SynCardObject((int) cardByID.cardInfo.entry, cardByID, 1);
        }
    }

    public void UpdateDataInfo()
    {
        if (this.IsInited())
        {
            this.SkillPointTick();
            if (this.startAddPhyForce)
            {
                this.CalAddForce();
            }
        }
    }

    public void UpdateFlameCardState(FlameBattleBackCardInfo info)
    {
        <UpdateFlameCardState>c__AnonStorey283 storey = new <UpdateFlameCardState>c__AnonStorey283 {
            info = info
        };
        FlameBattleSelfData data = this.YuanZhengCardStatList.Find(new Predicate<FlameBattleSelfData>(storey.<>m__5D2));
        if (data != null)
        {
            data.card_cur_hp = storey.info.card_cur_hp;
            data.card_cur_energy = storey.info.card_cur_energy;
        }
        else
        {
            FlameBattleSelfData item = new FlameBattleSelfData {
                card_entry = storey.info.card_entry,
                card_cur_hp = storey.info.card_cur_hp,
                card_cur_energy = storey.info.card_cur_energy,
                card_max_hp = storey.info.card_max_hp
            };
            this.YuanZhengCardStatList.Add(item);
        }
    }

    public void UpdateFlameTargetCardInfo(int cardEntry, uint currHp, ushort currEnergy)
    {
        if (this.mFlameBattleInfo != null)
        {
            int num = this.mFlameBattleInfo.cur_node / 2;
            if ((num >= 0) || (num <= 5))
            {
                foreach (TargetCard card in this.mFlameBattleInfo.target_data_list[num].target_card_list)
                {
                    if (cardEntry == card.card_entry)
                    {
                        card.card_cur_hp = currHp;
                        card.card_cur_energy = currEnergy;
                    }
                }
            }
        }
    }

    public void UpdateFriendHongBaoInfo(long friendid, int currStone, bool redpackage_isdraw, int friend_redpackage_num)
    {
        <UpdateFriendHongBaoInfo>c__AnonStorey27D storeyd = new <UpdateFriendHongBaoInfo>c__AnonStorey27D {
            friendid = friendid
        };
        Friend friend = this.mFriendList.Find(new Predicate<Friend>(storeyd.<>m__5CC));
        if (friend != null)
        {
            friend.redpackage_isdraw = redpackage_isdraw;
            friend.redpackage_num = friend_redpackage_num;
            if (friend.redpackage_num < 0)
            {
                friend.redpackage_num = 0;
            }
        }
    }

    public void UpdateGuildMemberInfo(long friendid, int currStone, bool redpackage_isdraw, int friend_redpackage_num)
    {
        <UpdateGuildMemberInfo>c__AnonStorey281 storey = new <UpdateGuildMemberInfo>c__AnonStorey281 {
            friendid = friendid
        };
        GuildMember member = this.mGuildMemberData.member.Find(new Predicate<GuildMember>(storey.<>m__5D0));
        if (member != null)
        {
            member.userInfo.redpackage_isdraw = redpackage_isdraw;
            member.userInfo.redpackage_num = friend_redpackage_num;
            if (member.userInfo.redpackage_num < 0)
            {
                member.userInfo.redpackage_num = 0;
            }
        }
    }

    public void UpdateItem(Item _item)
    {
        if ((_item != null) && (_item.entry >= 0))
        {
            if (_item.num >= CommonFunc.GetItemMaxPileNum(_item.entry))
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_item.entry);
                if (_config != null)
                {
                    TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x152), _config.name));
                }
            }
            XSingleton<UserItemPackageMgr>.Singleton.Update(_item);
            GameDataMgr.Instance.DirtyActorStage = true;
            CardPool.SynCardItemChanged(_item.entry);
            CardPool.SynEquipMaterialChanged(_item.entry);
        }
    }

    public void UpdateItemList(List<Item> _itemList)
    {
        foreach (Item item in _itemList)
        {
            this.UpdateItem(item);
        }
    }

    public void UpdateLeagueReward(LeagueRewardType type)
    {
        if (this.CurrLeagueReward != null)
        {
            switch (type)
            {
                case LeagueRewardType.LeagueRewardType_Daily:
                    this.CurrLeagueReward.rewardInfo.dailyReward = -1;
                    break;

                case LeagueRewardType.LeagueRewardType_Pre_Match:
                    this.CurrLeagueReward.rewardInfo.preMatchReward = -1;
                    break;

                case LeagueRewardType.LeagueRewardType_World:
                    this.CurrLeagueReward.rewardInfo.worldReward = -1;
                    break;
            }
        }
    }

    public void UpdateNewCard(NewCard newCard)
    {
        foreach (Card card in newCard.newCard)
        {
            this.UpdateCard(card);
        }
        foreach (Item item in newCard.newItem)
        {
            this.UpdateItem(item);
        }
    }

    public void UpdateNewCard(List<NewCard> _newCardList)
    {
        foreach (NewCard card in _newCardList)
        {
            this.UpdateNewCard(card);
        }
    }

    public void UpdateOutlandCardStatList(OutlandBattleBackCardInfo info)
    {
        <UpdateOutlandCardStatList>c__AnonStorey285 storey = new <UpdateOutlandCardStatList>c__AnonStorey285 {
            info = info
        };
        OutlandBattleBackCardInfo info2 = this.OutlandCardStatList.Find(new Predicate<OutlandBattleBackCardInfo>(storey.<>m__5D4));
        if (info2 != null)
        {
            Debug.Log(string.Concat(new object[] { "复活修改卡牌数据id=", storey.info.card_entry, "hp=", info2.card_cur_hp, "energy=", storey.info.card_cur_energy }));
            info2.card_cur_hp = storey.info.card_cur_hp;
            info2.card_cur_energy = storey.info.card_cur_energy;
        }
        else
        {
            OutlandBattleBackCardInfo item = new OutlandBattleBackCardInfo {
                card_entry = storey.info.card_entry,
                card_cur_hp = storey.info.card_cur_hp,
                card_cur_energy = storey.info.card_cur_energy
            };
            this.OutlandCardStatList.Add(item);
        }
    }

    public void UpdateOutlandCardStatList(List<OutlandBattleBackCardInfo> attackersList)
    {
        this.OutlandCardStatList.Clear();
        this.OutlandCardStatList = attackersList;
    }

    public void UpdateRegistrationData(S2C_Registration res)
    {
        this.Stone = res.stone;
        this.Gold = res.gold;
        foreach (NewCard card in res.newCardList)
        {
            this.UpdateNewCard(card);
        }
        foreach (Item item in res.itemList)
        {
            this.UpdateItem(item);
        }
        foreach (Item item2 in res.itemListForVip)
        {
            this.UpdateItem(item2);
        }
    }

    public void UpdateTicketItem(Item _item)
    {
        this.UpdateItem(_item);
        BagPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<BagPanel>();
        if (gUIEntity != null)
        {
            List<Item> list = new List<Item> {
                _item
            };
            gUIEntity.UpdateData(list);
        }
    }

    public void UpdateYuanZhengCardList(List<FlameBattleBackCardInfo> attackersList)
    {
        foreach (FlameBattleBackCardInfo info in attackersList)
        {
            this.UpdateFlameCardState(info);
        }
    }

    public void ZeroClockRefreshData()
    {
        SocketMgr.Instance.RequestGetUserInfo();
        this.RegistrationReward = 0;
        this.reqNewActiveDataInfo();
        this.reqActiveCompleteIsOrNot();
    }

    public List<string> AdAutoInfoList
    {
        get
        {
            return this.adAutoInfoList__;
        }
    }

    public int ArenaChallengeCoin
    {
        get
        {
            return this.mUserInfo.lol_arena_score;
        }
        set
        {
            if (this.mUserInfo.lol_arena_score != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            this.mUserInfo.lol_arena_score = value;
        }
    }

    public int ArenaLadderCoin
    {
        get
        {
            return this.mUserInfo.arena_ladder_score;
        }
        set
        {
            if (this.mUserInfo.arena_ladder_score != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            this.mUserInfo.arena_ladder_score = value;
        }
    }

    public bool bChargeChange
    {
        get
        {
            return this.bChargeChange__;
        }
        set
        {
            this.bChargeChange__ = value;
        }
    }

    public bool bGuidLvChange
    {
        get
        {
            return this.bGuidLvChange__;
        }
        set
        {
            this.bGuidLvChange__ = value;
        }
    }

    public bool bMonthVipCardChange
    {
        get
        {
            return this.bMonthVipCardChange__;
        }
        set
        {
            this.bMonthVipCardChange__ = value;
        }
    }

    public bool bPlayerLevelUp
    {
        get
        {
            return this.bPlayerLevelUp__;
        }
        set
        {
            this.bPlayerLevelUp__ = value;
        }
    }

    public List<FastBuf.Broadcast> BroadcastList
    {
        get
        {
            return this.mBroadcastList;
        }
    }

    public bool bUseStone
    {
        get
        {
            return this.bUseStone__;
        }
        set
        {
            this.bUseStone__ = value;
        }
    }

    public List<Card> CardList
    {
        get
        {
            return this.mCards;
        }
        set
        {
            CardPool.ClearCache();
            this.mCards = value;
            GameDataMgr.Instance.DirtyActorStage = true;
            CardPool.RebuildAll(false);
            this.CreateEquipBreakDict();
        }
    }

    public bool CheckShopTipsStatBool
    {
        get
        {
            return (((!this.goblinFixed && !this.mTodayOpenGoblinShopTips) && this.mShowGoblinShopTipsNew) || ((!this.secretFixed && !this.mTodayOpenSecretShopTips) && this.mShowSecretShopTipsNew));
        }
    }

    public int Courage
    {
        get
        {
            return this.UserInfo.courage;
        }
        set
        {
            if (this.UserInfo.courage != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            this.UserInfo.courage = value;
        }
    }

    public int CurrJoinGroupId
    {
        get
        {
            if (this.JoinLeagueInfo == null)
            {
                return -1;
            }
            return this.JoinLeagueInfo.groupId;
        }
    }

    public int CurrJoinLeague
    {
        get
        {
            if (this.JoinLeagueInfo == null)
            {
                return -1;
            }
            return this.JoinLeagueInfo.leagueEntry;
        }
    }

    public SkillPoint CurSkillPoint
    {
        get
        {
            return this.mSkillPoint;
        }
        set
        {
            GameDataMgr.Instance.DirtyActorStage = true;
            this.mSkillPoint.buyCount = value.buyCount;
            this.mSkillPoint.maxPoint = value.maxPoint;
            this.mSkillPoint.passTime = value.passTime;
            this.mSkillPoint.totalPoint = value.totalPoint;
            this.lastCalAddSkillTime = TimeMgr.Instance.ServerStampTime - value.passTime;
            this.NextGetSkillTime = 360 - this.mSkillPoint.passTime;
        }
    }

    public int Eq
    {
        get
        {
            return this.UserInfo.eq;
        }
        set
        {
            if (this.UserInfo.eq != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            this.UserInfo.eq = value;
        }
    }

    public int EquipAutoLevUpRemainCount
    {
        get
        {
            if (this.VipType >= this.EquipAutoLevUpUnlimitedVipLv)
            {
                return -1;
            }
            vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(getInstance().VipType);
            if (_config != null)
            {
                int num = _config.auto_equip_lv_up_count - this.mUserInfo.auto_equip_lv_up_count;
                return ((num >= 0) ? num : 0);
            }
            return 0;
        }
    }

    public int EquipAutoLevUpUnlimitedVipLv
    {
        get
        {
            IEnumerator enumerator = ConfigMgr.getInstance().getList<vip_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    vip_config current = (vip_config) enumerator.Current;
                    if (current.auto_equip_lv_up_count == -1)
                    {
                        return current.entry;
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
            return 0;
        }
    }

    public int FlamebattleCoin
    {
        get
        {
            return this.mUserInfo.flamebattleCoin;
        }
        set
        {
            if (this.mUserInfo.flamebattleCoin != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            this.mUserInfo.flamebattleCoin = value;
            YuanZhengPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<YuanZhengPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.UpdateFameCoin();
            }
        }
    }

    public List<Friend> FriendList
    {
        get
        {
            return this.mFriendList;
        }
        set
        {
            GameDataMgr.Instance.DirtyActorStage = true;
            this.mFriendList = value;
            if (<>f__am$cacheA8 == null)
            {
                <>f__am$cacheA8 = delegate (Friend f1, Friend f2) {
                    int num = !f1.isGivePhyForceNow ? 0 : 1;
                    int num2 = !f2.isGivePhyForceNow ? 0 : 1;
                    if (num != num2)
                    {
                        return num2 - num;
                    }
                    return f2.userInfo.level - f1.userInfo.level;
                };
            }
            this.mFriendList.Sort(<>f__am$cacheA8);
        }
    }

    public List<BriefUser> FriendReqList
    {
        get
        {
            return this.mFriendReqList;
        }
        set
        {
            this.mFriendReqList = value;
        }
    }

    public int GetNextCalForceTime
    {
        get
        {
            int num = TimeMgr.Instance.ServerStampTime - this.lastCalAddForceTime;
            int num2 = 300;
            return (num2 - num);
        }
    }

    public bool goblinFixed
    {
        get
        {
            return (this.UserInfo.goblinShopIsFixed == 1);
        }
        set
        {
            this.UserInfo.goblinShopIsFixed = !value ? 0 : 1;
        }
    }

    public int goblinShopDuration
    {
        get
        {
            return this.UserInfo.goblinShopDuration;
        }
        set
        {
            this.UserInfo.goblinShopDuration = value;
        }
    }

    public int goblinShopOpenTime
    {
        get
        {
            return this.UserInfo.goblinShopOpenTime;
        }
        set
        {
            this.UserInfo.goblinShopOpenTime = value;
        }
    }

    public int Gold
    {
        get
        {
            return this.UserInfo.gold;
        }
        set
        {
            if (this.UserInfo.gold != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            this.mNeedUpdateTitleBar = this.UserInfo.gold != value;
            this.UserInfo.gold = value;
            TitleBar instance = TitleBar.Instance;
            if ((null != instance) && this.mNeedUpdateTitleBar)
            {
                instance.UpdateGold(true);
            }
        }
    }

    public List<GuildApplication> GuildApplyList
    {
        get
        {
            return this.mGuildApplyList;
        }
        set
        {
            this.mGuildApplyList = value;
        }
    }

    public int GuildMsgNoTime
    {
        get
        {
            variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
            if (_config != null)
            {
                return _config.guild_msg_no_time;
            }
            return 5;
        }
    }

    public bool HaveNewArenaChallengeLog
    {
        get
        {
            return this.mHaveNewArenaChallengeLog;
        }
        set
        {
            this.mHaveNewArenaChallengeLog = value;
        }
    }

    public bool HaveNewArenaLog
    {
        get
        {
            return this.mHaveNewArenaLog;
        }
        set
        {
            this.mHaveNewArenaLog = value;
        }
    }

    public bool HaveNewDetainsDartLog
    {
        get
        {
            return this.mHaveNewDetainsDartLog;
        }
        set
        {
            this.mHaveNewDetainsDartLog = value;
        }
    }

    public int HeadEntry
    {
        get
        {
            return this.mUserInfo.headEntry;
        }
        set
        {
            this.mUserInfo.headEntry = value;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("MainUI");
            if (gUIEntity != null)
            {
                MainUI nui = (MainUI) gUIEntity;
                if (nui != null)
                {
                    nui.UpdateHead();
                }
            }
        }
    }

    public int HeadFrameEntry
    {
        get
        {
            return ((this.mUserInfo.headFrameEntry != -1) ? this.mUserInfo.headFrameEntry : 0);
        }
        set
        {
            this.mUserInfo.headFrameEntry = value;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("MainUI");
            if (gUIEntity != null)
            {
                MainUI nui = (MainUI) gUIEntity;
                if (nui != null)
                {
                    nui.UpdateHeadFrame();
                }
            }
        }
    }

    public bool IsVipChange
    {
        get
        {
            return this.isVipChange__;
        }
        set
        {
            this.isVipChange__ = value;
        }
    }

    public List<Item> ItemList
    {
        get
        {
            return XSingleton<UserItemPackageMgr>.Singleton.Items;
        }
        set
        {
            XSingleton<UserItemPackageMgr>.Singleton.Clear();
            foreach (Item item in value)
            {
                XSingleton<UserItemPackageMgr>.Singleton.Update(item);
            }
        }
    }

    public int Level
    {
        get
        {
            return this.UserInfo.level;
        }
        set
        {
            if (this.UserInfo.level != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            if ((value - this.UserInfo.level) > 0)
            {
                <>c__AnonStorey279 storey = new <>c__AnonStorey279();
                this.bPlayerLevelUp__ = true;
                storey.SrcLev = this.UserInfo.level;
                storey.SrcCurPhyForce = this.UserInfo.phyForce;
                storey.tolev = value;
                GUIMgr.Instance.DoModelGUI("PlayerLevUpPanel", new Action<GUIEntity>(storey.<>m__5C7), null);
                user_lv_up_config _config = ConfigMgr.getInstance().getByEntry<user_lv_up_config>(storey.tolev);
                if (_config != null)
                {
                    this.UserInfo.maxPhyForce = _config.phy_force_max;
                }
                TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
                if (activityGUIEntity != null)
                {
                    activityGUIEntity.UpdateTiLi(true);
                }
            }
            this.UserInfo.level = value;
            MainUI gUIEntity = GUIMgr.Instance.GetGUIEntity<MainUI>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateLevel();
            }
        }
    }

    public bool LivenessCanPick
    {
        get
        {
            return this.mLivenessCanPick;
        }
        set
        {
            this.mLivenessCanPick = value;
            TitleBar gUIEntity = GUIMgr.Instance.GetGUIEntity<TitleBar>();
            if ((gUIEntity != null) && !gUIEntity.Hidden)
            {
                gUIEntity.CheckLiveness();
            }
        }
    }

    public List<LivenessTask> LivenessTaskList
    {
        get
        {
            return this.mLivenessList;
        }
        set
        {
            this.mLivenessList = value;
        }
    }

    public List<Mail> MailList
    {
        get
        {
            return this.mMailList;
        }
        set
        {
            this.mMailList = value;
        }
    }

    public int MaxArenaCount
    {
        get
        {
            vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(this.VipType);
            if (_config == null)
            {
                return 0;
            }
            return _config.warmmatch_pk_times;
        }
    }

    public int MaxFriendCount
    {
        get
        {
            return 200;
        }
    }

    public int MaxWorldCupCount
    {
        get
        {
            vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(this.VipType);
            if (_config == null)
            {
                return 0;
            }
            return _config.league_pk_times;
        }
    }

    public bool mShowGoblinShopTipsNew
    {
        get
        {
            return ((TimeMgr.Instance.ServerStampTime > this.goblinShopOpenTime) && (TimeMgr.Instance.ServerStampTime < (this.goblinShopOpenTime + this.goblinShopDuration)));
        }
    }

    public bool mShowSecretShopTipsNew
    {
        get
        {
            return ((TimeMgr.Instance.ServerStampTime > this.secretShopOpenTime) && (TimeMgr.Instance.ServerStampTime < (this.secretShopOpenTime + this.secretShopDuration)));
        }
    }

    public int OutlandCoin
    {
        get
        {
            return this.mUserInfo.outlandCoin;
        }
        set
        {
            if (this.mUserInfo.outlandCoin != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            this.mUserInfo.outlandCoin = value;
            if (this.isPreOutlandFight)
            {
                OutlandSecondPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandSecondPanel>();
                if (null != gUIEntity)
                {
                    gUIEntity.UpdateFameCoin();
                }
            }
            else
            {
                OutlandFristPanel panel2 = GUIMgr.Instance.GetGUIEntity<OutlandFristPanel>();
                if (null != panel2)
                {
                    panel2.UpdateFameCoin();
                }
            }
        }
    }

    public int PhyForce
    {
        get
        {
            return this.UserInfo.phyForce;
        }
        set
        {
            if (this.UserInfo != null)
            {
                if (this.UserInfo.phyForce != value)
                {
                    GameDataMgr.Instance.DirtyActorStage = true;
                }
                this.mNeedUpdateTitleBar = this.UserInfo.phyForce != value;
                this.UserInfo.phyForce = value;
                TitleBar gUIEntity = GUIMgr.Instance.GetGUIEntity<TitleBar>();
                if ((null != gUIEntity) && this.mNeedUpdateTitleBar)
                {
                    gUIEntity.UpdateTiLi(true);
                }
            }
        }
    }

    public int PushSendBoostRecruit
    {
        get
        {
            return PlayerPrefs.GetInt(this.SessionInfo.userid + "PushSendBoostRecruit", 0);
        }
        set
        {
            PlayerPrefs.SetInt(this.SessionInfo.userid + "PushSendBoostRecruit", value);
        }
    }

    public int PushSendShopRefresh
    {
        get
        {
            return PlayerPrefs.GetInt(this.SessionInfo.userid + "PushSendShopRefresh", 0);
        }
        set
        {
            PlayerPrefs.SetInt(this.SessionInfo.userid + "PushSendShopRefresh", value);
        }
    }

    public int PushSendSkillPointFull
    {
        get
        {
            return PlayerPrefs.GetInt(this.SessionInfo.userid + "PushSendSkillPointFull", 0);
        }
        set
        {
            PlayerPrefs.SetInt(this.SessionInfo.userid + "PushSendSkillPointFull", value);
        }
    }

    public int PushSendTiliFullMsg
    {
        get
        {
            return PlayerPrefs.GetInt(this.SessionInfo.userid + "PushTiLiMsg", 0);
        }
        set
        {
            PlayerPrefs.SetInt(this.SessionInfo.userid + "PushTiLiMsg", value);
        }
    }

    public int PushSendWorldCupApply
    {
        get
        {
            return PlayerPrefs.GetInt(this.SessionInfo.userid + "PushSendWorldCupApply", 0);
        }
        set
        {
            PlayerPrefs.SetInt(this.SessionInfo.userid + "PushSendWorldCupApply", value);
        }
    }

    public List<Quest> QuestList
    {
        get
        {
            return this.mQuestList;
        }
        set
        {
            this.mQuestList = value;
            this.DirtyQuestList = false;
        }
    }

    public int RegistrationCount
    {
        get
        {
            return this.mUserInfo.registrationCount;
        }
        set
        {
            this.mUserInfo.registrationCount = value;
        }
    }

    public int RegistrationReward
    {
        get
        {
            return this.mUserInfo.registrationReward;
        }
        set
        {
            if (this.mUserInfo != null)
            {
                this.mUserInfo.registrationReward = value;
            }
        }
    }

    public int RemainBuyPk
    {
        get
        {
            if (this.JoinLeagueInfo == null)
            {
                return 0;
            }
            return this.JoinLeagueInfo.remainBuyPk;
        }
    }

    public int RemainPk
    {
        get
        {
            if (this.JoinLeagueInfo == null)
            {
                return 0;
            }
            return this.JoinLeagueInfo.remainPk;
        }
    }

    public bool secretFixed
    {
        get
        {
            return (this.UserInfo.secretShopIsFixed == 1);
        }
        set
        {
            this.UserInfo.secretShopIsFixed = !value ? 0 : 1;
        }
    }

    public int secretShopDuration
    {
        get
        {
            return this.UserInfo.secretShopDuration;
        }
        set
        {
            this.UserInfo.secretShopDuration = value;
        }
    }

    public int secretShopOpenTime
    {
        get
        {
            return this.UserInfo.secretShopOpenTime;
        }
        set
        {
            this.UserInfo.secretShopOpenTime = value;
        }
    }

    public bool ShowGoblinShopTipsNew
    {
        get
        {
            return ((!this.goblinFixed && !this.mTodayOpenGoblinShopTips) && this.mShowGoblinShopTipsNew);
        }
    }

    public bool ShowSecretShopTipsNew
    {
        get
        {
            return ((!this.secretFixed && !this.mTodayOpenSecretShopTips) && this.mShowSecretShopTipsNew);
        }
    }

    public int Stone
    {
        get
        {
            return this.UserInfo.stone;
        }
        set
        {
            if (this.UserInfo.stone != value)
            {
                GameDataMgr.Instance.DirtyActorStage = true;
            }
            this.mNeedUpdateTitleBar = this.UserInfo.stone != value;
            if (value == -2)
            {
                SocketMgr.Instance.SendQueryTxBalance();
            }
            else
            {
                if (value < this.UserInfo.stone)
                {
                    this.bUseStone__ = true;
                }
                this.UserInfo.stone = value;
                TitleBar instance = TitleBar.Instance;
                if ((null != instance) && this.mNeedUpdateTitleBar)
                {
                    instance.UpdateStone(true);
                }
            }
        }
    }

    public int SuperVipRemainTime
    {
        get
        {
            long num = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
            return Mathf.Max(this.UserInfo.vip_data.vip_2_time - ((int) (num - this.stampVip)), 0);
        }
        set
        {
            this.stampSuperVip = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
            this.UserInfo.vip_data.vip_2_time = value;
            GameDataMgr.Instance.DirtyActorStage = true;
        }
    }

    public int TitleCount
    {
        get
        {
            return PlayerPrefs.GetInt(this.SessionInfo.userid + "TitleCount", 1);
        }
        set
        {
            PlayerPrefs.SetInt(this.SessionInfo.userid + "TitleCount", value);
        }
    }

    public List<TitleData> TitleList
    {
        get
        {
            return this.mTitleList;
        }
        set
        {
            this.mTitleList = value;
            this.mTitleEntryList.Clear();
            GameDataMgr.Instance.DirtyActorStage = true;
            foreach (TitleData data in this.mTitleList)
            {
                this.mTitleEntryList.Add(data.title_entry);
            }
        }
    }

    public bool TodayOpenGoblinShopTips
    {
        set
        {
            SettingMgr.mInstance.SetOpenShopTips("GOBLIN", !value ? 0 : 1);
            this.mTodayOpenGoblinShopTips = value;
        }
    }

    public bool TodayOpenSecretShopTips
    {
        set
        {
            SettingMgr.mInstance.SetOpenShopTips("SECRET", !value ? 0 : 1);
            this.mTodayOpenSecretShopTips = value;
        }
    }

    public User UserInfo
    {
        get
        {
            return this.mUserInfo;
        }
        set
        {
            this.mUserInfo = value;
            this.VipRemainTime = value.vip_data.vip_1_time;
            this.SuperVipRemainTime = value.vip_data.vip_2_time;
            MainUI gUIEntity = GUIMgr.Instance.GetGUIEntity<MainUI>();
            if (gUIEntity != null)
            {
                gUIEntity.Create3DRole(value.headEntry);
            }
        }
    }

    public int VipRemainTime
    {
        get
        {
            long num = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
            return Mathf.Max(this.UserInfo.vip_data.vip_1_time - ((int) (num - this.stampVip)), 0);
        }
        set
        {
            this.stampVip = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
            this.UserInfo.vip_data.vip_1_time = value;
            GameDataMgr.Instance.DirtyActorStage = true;
        }
    }

    public int VipType
    {
        get
        {
            if ((this.mUserInfo.vip_level.level - 1) < 0)
            {
                return 0;
            }
            return (this.mUserInfo.vip_level.level - 1);
        }
    }

    public int WorldBossState
    {
        get
        {
            return this.mUserInfo.world_boss_state;
        }
        set
        {
            this.mUserInfo.world_boss_state = value;
        }
    }

    public float YuanZhenMapVal
    {
        get
        {
            return PlayerPrefs.GetFloat(this.SessionInfo.userid + "YuanZhenMapVal", 0f);
        }
        set
        {
            PlayerPrefs.SetFloat(this.SessionInfo.userid + "YuanZhenMapVal", value);
        }
    }

    [CompilerGenerated]
    private sealed class <>c__AnonStorey279
    {
        internal int SrcCurPhyForce;
        internal int SrcLev;
        internal int tolev;

        internal void <>m__5C7(GUIEntity obj)
        {
            PlayerLevUpPanel panel = (PlayerLevUpPanel) obj;
            panel.Depth = 810;
            panel.UpdateData(this.SrcLev, this.tolev, this.SrcCurPhyForce);
        }
    }

    [CompilerGenerated]
    private sealed class <GetCopperKey>c__AnonStorey27C
    {
        internal Item data;

        internal bool <>m__5CB(UserItemPackageMgr.UserItem item)
        {
            if ((item.Config.type == 6) && (item.Config.param_0 == 2))
            {
                this.data = item.Item;
                return true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <GetFlameCardByEntry>c__AnonStorey282
    {
        internal int cardEntry;

        internal bool <>m__5D1(FlameBattleSelfData e)
        {
            return (e.card_entry == this.cardEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <GetGoldKey>c__AnonStorey27A
    {
        internal Item data;

        internal bool <>m__5C9(UserItemPackageMgr.UserItem item)
        {
            if ((item.Config.type == 6) && (item.Config.param_0 == 0))
            {
                this.data = item.Item;
                return true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <GetOutlandCardByEntry>c__AnonStorey284
    {
        internal int cardEntry;

        internal bool <>m__5D3(OutlandBattleBackCardInfo e)
        {
            return (e.card_entry == this.cardEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <GetSliverKey>c__AnonStorey27B
    {
        internal Item data;

        internal bool <>m__5CA(UserItemPackageMgr.UserItem item)
        {
            if ((item.Config.type == 6) && (item.Config.param_0 == 1))
            {
                this.data = item.Item;
                return true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <HaveInAcitveCard>c__AnonStorey280
    {
        internal card_ex_config cec;

        internal bool <>m__5CF(Item e)
        {
            return ((e.entry == this.cec.item_entry) && (e.num >= this.cec.combine_need_item_num));
        }
    }

    [CompilerGenerated]
    private sealed class <SetAllFriendTiLiState>c__AnonStorey27E
    {
        internal Friend f;

        internal bool <>m__5CD(Friend e)
        {
            return (e.userInfo.id == this.f.userInfo.id);
        }
    }

    [CompilerGenerated]
    private sealed class <SetLingQuTiLiState>c__AnonStorey27F
    {
        internal long targetId;

        internal bool <>m__5CE(Friend e)
        {
            return (e.userInfo.id == this.targetId);
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateFlameCardState>c__AnonStorey283
    {
        internal FlameBattleBackCardInfo info;

        internal bool <>m__5D2(FlameBattleSelfData e)
        {
            return (e.card_entry == this.info.card_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateFriendHongBaoInfo>c__AnonStorey27D
    {
        internal long friendid;

        internal bool <>m__5CC(Friend e)
        {
            return (e.userInfo.id == this.friendid);
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateGuildMemberInfo>c__AnonStorey281
    {
        internal long friendid;

        internal bool <>m__5D0(GuildMember e)
        {
            return (e.userInfo.id == this.friendid);
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateOutlandCardStatList>c__AnonStorey285
    {
        internal OutlandBattleBackCardInfo info;

        internal bool <>m__5D4(OutlandBattleBackCardInfo e)
        {
            return (e.card_entry == this.info.card_entry);
        }
    }

    private class CompareGuildMebmer : IComparer<GuildMember>
    {
        public int Compare(GuildMember l, GuildMember r)
        {
            if (l.position != r.position)
            {
                return (r.position - l.position);
            }
            return (r.userInfo.level - l.userInfo.level);
        }
    }
}

