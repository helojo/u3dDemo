using Battle;
using FastBuf;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class SelectHeroPanel : GUIEntity
{
    private string _text;
    [CompilerGenerated]
    private static Comparison<AssistUser> <>f__am$cache1F;
    [CompilerGenerated]
    private static Func<SocialUser, bool> <>f__am$cache20;
    [CompilerGenerated]
    private static Func<SocialUser, QQFriendUser> <>f__am$cache21;
    [CompilerGenerated]
    private static Func<QQFriendUser, TableData> <>f__am$cache22;
    [CompilerGenerated]
    private static Func<AssistUser, TableData> <>f__am$cache23;
    [CompilerGenerated]
    private static Func<Card, TableData> <>f__am$cache24;
    [CompilerGenerated]
    private static Comparison<TableData> <>f__am$cache25;
    [CompilerGenerated]
    private static Action<GameObject> <>f__am$cache26;
    public GameObject AssisPay;
    private Dictionary<long, TableData> AssistCardDict = new Dictionary<long, TableData>();
    private long AssistID = -1L;
    public List<int> DefaultHero;
    public List<long> DetainsDartHeroCardIDList = new List<long>();
    public List<int> DetainsDartHeroEntryList = new List<int>();
    private int DungeonsDifficult;
    public int EndTime;
    private UITableManager<SelectTableItem> HeroTable = new UITableManager<SelectTableItem>();
    private bool InAssistPage;
    private float lastCheckOutTime;
    private UILabel lb_timelimit;
    private const int LineCount = 6;
    public UIGrid ListRoot;
    private float m_interval = 1.3f;
    private float m_times;
    private int maxCardCnt = 5;
    private SelectTableItem mCurrPressItem;
    private BattleType mCurType;
    private dungeons_activity_config mDungeonsActivityCfg;
    private int mPressItemStartTime = -1;
    private int mTitleOldDepth;
    public Action<List<long>> OnSaved;
    public Action<List<long>> OnStartBattle;
    public List<int> RichActivityHeroEntryList = new List<int>();
    private const int RowCount = 2;
    private UIButton SaveButton;
    private float ScaleParam = 1.06f;
    private List<Card> SelectHeroList = new List<Card>();
    private List<GameObject> SelectHeroObjList = new List<GameObject>();
    public List<GameObject> SlotList = new List<GameObject>();
    private float startTime;

    public void CallClosePanel()
    {
        if ((BattleType.OutLandBattle_tollGate_0 <= this.mBattleType) && (BattleType.OutLandBattle_tollGate_3 >= this.mBattleType))
        {
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(false, false);
        }
        GUIMgr.Instance.ExitModelGUI(this);
        CommonFunc.SetTitleBarDepth(this.mTitleOldDepth);
    }

    private bool CardIsDefaultSel(Card _card)
    {
        foreach (Card card in ActorData.getInstance().mDefaultDupHeroList)
        {
            if (card.card_id == _card.card_id)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckHero(Card _data)
    {
        foreach (Card card in this.SelectHeroList)
        {
            if (card.cardInfo.entry == _data.cardInfo.entry)
            {
                return true;
            }
        }
        return false;
    }

    private void CheckNullData()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject obj2 in this.SelectHeroObjList)
        {
            if (obj2 == null)
            {
                list.Add(obj2);
            }
        }
        list.ForEach(obj => this.SelectHeroObjList.Remove(obj));
    }

    private void ClosePanel()
    {
        if ((this.mCurType == BattleType.Normal) || ((this.mCurType >= BattleType.OutLandBattle_tollGate_0) && (this.mCurType <= BattleType.OutLandBattle_tollGate_3)))
        {
            GUIMgr.Instance.ExitModelGUI("SelectHeroPanel");
        }
        else
        {
            GUIMgr.Instance.PopGUIEntity();
        }
    }

    private GameObject CreateSelItem(GameObject _obj, Card _card, bool _playAnim, out int _index)
    {
        GameObject go = UnityEngine.Object.Instantiate(_obj) as GameObject;
        TweenAlpha component = go.GetComponent<TweenAlpha>();
        if (null != component)
        {
            component.enabled = false;
        }
        int cardPosition = this.GetCardPosition(_card);
        this.ResetSlotPosAfterPush(cardPosition);
        go.name = "HeroIcon";
        go.transform.parent = this.SlotList[cardPosition].transform;
        go.transform.localScale = new Vector3(this.ScaleParam, this.ScaleParam, 1f);
        if (_playAnim)
        {
            go.transform.position = _obj.transform.position;
        }
        else
        {
            go.transform.localPosition = Vector3.zero;
        }
        GUIDataHolder.setData(go, _card);
        UIEventListener listener1 = UIEventListener.Get(go);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickPopHero));
        this.SelectHeroList.Add(_card);
        this.SelectHeroObjList.Add(go);
        GUIDataHolder.setData(this.SlotList[cardPosition], _card);
        _index = cardPosition;
        return go;
    }

    private void DefaultSelectHero()
    {
        this.SelectHeroList.Clear();
        this.SelectHeroObjList.Clear();
        this.ResetSlot();
        List<Card> formationListByType = new List<Card>();
        if (this.mCurType == BattleType.RichActivity)
        {
            if (this.DefaultHero != null)
            {
                formationListByType = (from t in ActorData.getInstance().CardList
                    where this.DefaultHero.Contains((int) t.cardInfo.entry)
                    select t).ToList<Card>();
            }
        }
        else if (this.mBattleType != BattleType.DetainsDartEscort)
        {
            formationListByType = ActorData.getInstance().GetFormationListByType(this.mBattleType);
        }
        formationListByType.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
        foreach (Card card in formationListByType)
        {
            Card cardByID = ActorData.getInstance().GetCardByID(card.card_id);
            if (cardByID != null)
            {
                int num = 0;
                if (this.mCurType == BattleType.YuanZhengPk)
                {
                    FlameBattleSelfData flameCardByEntry = ActorData.getInstance().GetFlameCardByEntry((int) card.cardInfo.entry);
                    if ((flameCardByEntry != null) && (flameCardByEntry.card_cur_hp == 0))
                    {
                        continue;
                    }
                }
                if ((this.mCurType >= BattleType.OutLandBattle_tollGate_0) && (this.mCurType <= BattleType.OutLandBattle_tollGate_3))
                {
                    OutlandBattleBackCardInfo outlandCardByEntry = ActorData.getInstance().GetOutlandCardByEntry((int) card.cardInfo.entry);
                    if ((outlandCardByEntry != null) && (outlandCardByEntry.card_cur_hp == 0))
                    {
                        continue;
                    }
                }
                if ((this.mCurType != BattleType.GuildDup) || !XSingleton<GameGuildMgr>.Singleton.IsDead((int) card.cardInfo.entry))
                {
                    IEnumerator<SelectTableItem> enumerator = this.HeroTable.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            SelectTableItem current = enumerator.Current;
                            if (current.ItemCard.card_id == card.card_id)
                            {
                                this.CreateSelItem(current.Root.gameObject, cardByID, false, out num);
                                continue;
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator == null)
                        {
                        }
                        enumerator.Dispose();
                    }
                }
            }
        }
        this.UpdateBufferState();
    }

    private void EnterBattle(List<long> cardInfo)
    {
        GUIMgr.Instance.Lock();
        ActorData.getInstance().mFriendAssistCardEntry = -1;
        Transform transform = base.transform.FindChild("StartBtn");
        Debug.LogWarning("mCurType______:" + this.mCurType);
        switch (this.mCurType)
        {
            case BattleType.Normal:
                SocketMgr.Instance.RequestEnterDup(ActorData.getInstance().CurDupEntry, ActorData.getInstance().CurTrenchEntry, ActorData.getInstance().CurDupType, cardInfo, this.AssistID);
                if (this.AssistCardDict.ContainsKey(this.AssistID))
                {
                    ActorData.getInstance().mFriendAssistCardEntry = (int) this.AssistCardDict[this.AssistID].CardData.cardInfo.entry;
                    ActorData.getInstance().mFriendAssistIsQQ = this.AssistCardDict[this.AssistID].IsQQ;
                }
                break;

            case BattleType.WorldBoss:
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                SocketMgr.Instance.RequestWorldBossCombat(0, cardInfo);
                break;

            case BattleType.FriendPk:
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                object obj2 = GUIDataHolder.getData(transform.gameObject);
                if (obj2 != null)
                {
                    S2C_GetFriendFormation formation = obj2 as S2C_GetFriendFormation;
                    SocketMgr.Instance.RequestPrepareFriendCombat(cardInfo);
                }
                break;
            }
            case BattleType.WorldCupPk:
                if (TimeMgr.Instance.ServerStampTime < ActorData.getInstance().mCurrWorldCupEndTime)
                {
                    object obj3 = GUIDataHolder.getData(transform.gameObject);
                    if (obj3 != null)
                    {
                        S2C_GetLeagueOpponentFormation formation2 = obj3 as S2C_GetLeagueOpponentFormation;
                        if ((ActorData.getInstance().LeagueOpponentInfo.userInfo.id == formation2.targetId) && (ActorData.getInstance().JoinLeagueInfo != null))
                        {
                            SocketMgr.Instance.RequestPrepareLeagueFight(formation2.leagueEntry, formation2.groupId, ActorData.getInstance().CurrWorldCupPkTargetId, cardInfo, ActorData.getInstance().LeagueOpponentInfo.rank, ActorData.getInstance().JoinLeagueInfo.rank, false);
                        }
                    }
                    break;
                }
                TipsDiag.SetText("当时非比赛时间段");
                GUIMgr.Instance.PopGUIEntity();
                return;

            case BattleType.WarmmatchPk:
            {
                object obj4 = GUIDataHolder.getData(transform.gameObject);
                if (obj4 == null)
                {
                    break;
                }
                if (TimeMgr.Instance.ServerStampTime < ActorData.getInstance().mCurrWarmmatchEndTime)
                {
                    S2C_WarmmatchTargetReq req = obj4 as S2C_WarmmatchTargetReq;
                    SocketMgr.Instance.RequestWarmmatchCombat(req.target_id, cardInfo);
                    break;
                }
                TipsDiag.SetText("本场比赛已结束！");
                GUIMgr.Instance.PopGUIEntity();
                return;
            }
            case BattleType.TowerPk:
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                object obj5 = GUIDataHolder.getData(transform.gameObject);
                if (obj5 != null)
                {
                    VoidTowerData data = obj5 as VoidTowerData;
                    SocketMgr.Instance.RequestVoidTowerCombat(cardInfo);
                }
                break;
            }
            case BattleType.Dungeons:
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                DupCommonData data2 = new DupCommonData {
                    dupType = DuplicateType.DupType_Undertown,
                    dupEntry = this.mDungeonsActivityCfg.entry,
                    trenchEntry = this.DungeonsDifficult
                };
                SocketMgr.Instance.RequestNewDungeonsCombat(cardInfo, data2);
                break;
            }
            case BattleType.YuanZhengPk:
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                SocketMgr.Instance.RequestFlameBattleStart(cardInfo);
                break;

            case BattleType.ArenaLadder:
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                object obj6 = GUIDataHolder.getData(transform.gameObject);
                if (obj6 != null)
                {
                    ArenaLadderEnemy enemy = obj6 as ArenaLadderEnemy;
                    SocketMgr.Instance.RequestArenaLadderCombat(enemy.targetId, cardInfo, enemy.order, enemy.target_type);
                }
                break;
            }
            case BattleType.GuildBattle:
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                object obj7 = GUIDataHolder.getData(transform.gameObject);
                if (obj7 != null)
                {
                    GuildBattleTargetInfo info = obj7 as GuildBattleTargetInfo;
                    SocketMgr.Instance.RequestGuildBattleCombat(info.resourceEntry, info.targetId, cardInfo);
                }
                break;
            }
            case BattleType.GuildDup:
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                if (this.OnStartBattle != null)
                {
                    this.OnStartBattle(cardInfo);
                }
                break;

            case BattleType.OutLandBattle_tollGate_0:
            case BattleType.OutLandBattle_tollGate_1:
            case BattleType.OutLandBattle_tollGate_2:
            case BattleType.OutLandBattle_tollGate_3:
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                SocketMgr.Instance.SelectHeroOutlandCombatReq(cardInfo);
                break;

            case BattleType.DerainsDartInterceptBattle:
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                int startTime = (int) TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
                Debug.LogWarning("xxStartBattleTime__:" + startTime);
                SocketMgr.Instance.RequestC2S_RobConvoyCombatBegin(XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTargetInfo.targetId, XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTargetInfo.convoyIndex, XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTargetInfo.type, cardInfo, startTime);
                break;
            }
            case BattleType.DetainsDartBattleBack:
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                int num2 = (int) TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
                Debug.LogWarning("xxxStartBattleTime:" + num2);
                SocketMgr.Instance.RequestC2S_ConvoyAvengeCombat(XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo.targetId, -1L, cardInfo, num2, XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo.convoyIndex);
                break;
            }
        }
    }

    private int GetCardPosition(Card _InsertData)
    {
        int num = 0;
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _InsertData.cardInfo.entry);
        foreach (GameObject obj2 in this.SlotList)
        {
            Card card = (Card) GUIDataHolder.getData(obj2);
            if (card != null)
            {
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) card.cardInfo.entry);
                if (_config.card_position > _config2.card_position)
                {
                    num++;
                }
            }
        }
        return num;
    }

    private int GetNullSlot()
    {
        int num = 0;
        foreach (GameObject obj2 in this.SlotList)
        {
            if (GUIDataHolder.getData(obj2) == null)
            {
                return num;
            }
            num++;
        }
        return 0;
    }

    private bool HasProcess(GameObject _obj, List<GameObject> _list)
    {
        foreach (GameObject obj2 in _list)
        {
            if (_obj == obj2)
            {
                return true;
            }
        }
        return false;
    }

    private void HideHeroGird6(bool isShow)
    {
    }

    private void On_Swipe(Gesture gesture)
    {
        this.mPressItemStartTime = -1;
    }

    public void OnAssistTabResponsed()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Fomation))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_select_assist, null);
        }
    }

    private void OnBuyCoolDown(SelectTableItem obj)
    {
        <OnBuyCoolDown>c__AnonStorey1D9 storeyd = new <OnBuyCoolDown>c__AnonStorey1D9 {
            obj = obj
        };
        MessageBox.ShowMessageBox(string.Format(ConfigMgr.getInstance().GetWord(0x2c43), GameConstValues.CLEAR_TX_FRIEND_COOLDOWN_COST_STONE_COUNT), new UIEventListener.VoidDelegate(storeyd.<>m__2E5), null, false);
    }

    private void OnClickHeroIcon(SelectTableItem obj)
    {
        Card itemCard = obj.ItemCard;
        if (ActorData.getInstance().GetCardByID(itemCard.card_id) == null)
        {
            if (this.AssistID > 0L)
            {
                if (!this.SelectHeroList.Contains(itemCard))
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4ec));
                    return;
                }
            }
            else if ((this.SelectHeroList.Count >= this.MaxCardCnt) && !this.SelectHeroList.Contains(itemCard))
            {
                TipsDiag.SetText("选择的人数已满!");
                return;
            }
        }
        else if (this.AssistID > 0L)
        {
            if ((this.SelectHeroList.Count >= this.MaxCardCnt) && !this.SelectHeroList.Contains(itemCard))
            {
                TipsDiag.SetText("选择的人数已满!");
                return;
            }
        }
        else if ((this.SelectHeroList.Count >= this.MaxCardCnt) && !this.SelectHeroList.Contains(itemCard))
        {
            TipsDiag.SetText("选择的人数已满!");
            return;
        }
        foreach (GameObject obj2 in this.SlotList)
        {
            object obj3 = GUIDataHolder.getData(obj2);
            if (obj3 != null)
            {
                Card card3 = (Card) obj3;
                if (itemCard.card_id == card3.card_id)
                {
                    this.PopCard(obj.Root.gameObject, itemCard);
                    return;
                }
            }
        }
        if (this.CheckHero(itemCard))
        {
            TipsDiag.SetText("请不要选择重复的英雄!");
        }
        else
        {
            this.PushCard(obj.Root.gameObject, itemCard);
            if (GuideSystem.MatchEvent(GuideEvent.Fomation))
            {
                GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_formation_select_hero, obj.Root.gameObject);
            }
        }
    }

    private void OnClickPopHero(GameObject obj)
    {
        Card card = (Card) GUIDataHolder.getData(obj);
        this.PopCard(obj, card);
    }

    private void OnClickSaveBtn()
    {
        List<long> list = new List<long>();
        foreach (Card card in this.SelectHeroList)
        {
            list.Add(card.card_id);
            Debug.Log(card.cardInfo.entry);
        }
        if (list.Count > 0)
        {
            if (this.mCurType == BattleType.RichActivity)
            {
                if (this.OnSaved != null)
                {
                    this.OnSaved(list);
                }
                this.CallClosePanel();
            }
            else
            {
                if (this.mCurType == BattleType.DerainsDartInterceptBattle)
                {
                    foreach (Card card2 in this.SelectHeroList)
                    {
                        this.DetainsDartHeroEntryList.Add((int) card2.cardInfo.entry);
                        this.DetainsDartHeroCardIDList.Add(card2.card_id);
                    }
                    if (this.OnSaved != null)
                    {
                        this.OnSaved(list);
                    }
                }
                else if ((this.mCurType != BattleType.DetainsDartBattleBack) && (this.mCurType == BattleType.DetainsDartEscort))
                {
                    this.DetainsDartHeroCardIDList.Clear();
                    XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortInfo = new ConvoyInfo();
                    XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortInfo.cards.Clear();
                    foreach (Card card3 in this.SelectHeroList)
                    {
                        this.DetainsDartHeroEntryList.Add((int) card3.cardInfo.entry);
                        this.DetainsDartHeroCardIDList.Add(card3.card_id);
                        XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortInfo.cards.Add(card3.card_id);
                        XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex = XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count;
                        if (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count >= 2)
                        {
                            TipsDiag.SetText("请去运送界面领取之前的运送奖励吧，然后才能进行再次护送哦！");
                            return;
                        }
                        if (!XSingleton<GameDetainsDartMgr>.Singleton.TeamIdToTeamCardsInfo.ContainsKey(XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex))
                        {
                            XSingleton<GameDetainsDartMgr>.Singleton.TeamIdToTeamCardsInfo.Add(XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex, this.DetainsDartHeroEntryList);
                        }
                        else
                        {
                            XSingleton<GameDetainsDartMgr>.Singleton.TeamIdToTeamCardsInfo[XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex] = this.DetainsDartHeroEntryList;
                        }
                    }
                    XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList.Clear();
                    XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList = this.DetainsDartHeroCardIDList;
                    if (this.OnSaved != null)
                    {
                        this.OnSaved(list);
                    }
                }
                if (this.mCurType == BattleType.WarmmatchDefense)
                {
                    SocketMgr.Instance.RequestSetBattleMember(list, BattleFormationType.BattleFormationType_Arena_Def);
                }
                else if (this.mCurType == BattleType.WorldCupDefense)
                {
                    SocketMgr.Instance.RequestSetBattleMember(list, BattleFormationType.BattleFormationType_League_Def);
                }
                else if (this.mCurType == BattleType.ChallengeDefense)
                {
                    if (list.Count < 8)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x5dc));
                        return;
                    }
                    SocketMgr.Instance.RequestSetChallengeArenaBattleMember(list);
                }
                this.CallClosePanel();
            }
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x1d));
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
        this.OnRegister();
    }

    private void OnHeroLongPress(SelectTableItem hero)
    {
        <OnHeroLongPress>c__AnonStorey1DA storeyda = new <OnHeroLongPress>c__AnonStorey1DA {
            hero = hero
        };
        GUIMgr.Instance.DoModelGUI<HeroInfoPanel>(new Action<GUIEntity>(storeyda.<>m__2E6), base.gameObject);
    }

    private void OnHeroPress(bool isPress, SelectTableItem hero)
    {
        this.mCurrPressItem = !isPress ? null : hero;
        this.mPressItemStartTime = !isPress ? -1 : TimeMgr.Instance.ServerStampTime;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.mTitleOldDepth = CommonFunc.GetTitleBarDepth();
        GUIMgr.Instance.FloatTitleBar();
        this.HeroTable.InitFromGrid(base.FindChild<UIGrid>("Grid"));
        this.HeroTable.Count = 0;
        this.HeroTable.Cache = false;
        this.SaveButton = base.transform.FindChild("SaveBtn").GetComponent<UIButton>();
        this.SaveButtonText = ConfigMgr.getInstance().GetWord(0x2c69);
        this.lb_timelimit = base.FindChild<UILabel>("lb_timelimit");
        this.lb_timelimit.text = string.Empty;
    }

    private void OnRegister()
    {
        EasyTouch.On_Swipe += new EasyTouch.SwipeHandler(this.On_Swipe);
    }

    public override void OnRelease()
    {
        ActorData.getInstance().AssistUserList.Clear();
        base.OnRelease();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        this.UnsubscribeEvent();
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
        if (GuideSystem.MatchEvent(GuideEvent.Start_EliteTrench_Fight))
        {
            Utility.NewbiestUnlock();
            Utility.EnforceClear();
            GuideSystem.FinishEvent(GuideEvent.Start_EliteTrench_Fight);
            GuideSystem.ActivedGuide.Complete();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mCurType == BattleType.GuildDup)
        {
            if (this.lastCheckOutTime == 0f)
            {
                this.lastCheckOutTime = Time.time;
                return;
            }
            if ((this.lastCheckOutTime + 1f) < Time.time)
            {
                this.lastCheckOutTime = Time.time;
                int num = this.EndTime - TimeMgr.Instance.ServerStampTime;
                if (num < 0)
                {
                    GUIMgr.Instance.ExitModelGUI(this);
                    return;
                }
                this.lb_timelimit.ActiveSelfObject(true);
                this.lb_timelimit.text = string.Format("请在{0}秒内开始战斗", num);
            }
        }
        for (int i = 0; i < this.HeroTable.Count; i++)
        {
            this.HeroTable[i].Update();
        }
        if (((this.mPressItemStartTime != -1) && (TimeMgr.Instance.ServerStampTime >= (this.mPressItemStartTime + 2))) && (this.mCurrPressItem != null))
        {
            <OnUpdate>c__AnonStorey1DC storeydc = new <OnUpdate>c__AnonStorey1DC();
            this.mPressItemStartTime = -1;
            storeydc.hero = this.mCurrPressItem;
            if (storeydc.hero != null)
            {
                GUIMgr.Instance.DoModelGUI<HeroInfoPanel>(new Action<GUIEntity>(storeydc.<>m__2F2), base.gameObject);
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator PlayAnimAndDestroyObj(GameObject obj)
    {
        return new <PlayAnimAndDestroyObj>c__Iterator78 { obj = obj, <$>obj = obj };
    }

    private void PopCard(GameObject _obj, Card _card)
    {
        GameObject gameObject = null;
        IEnumerator<SelectTableItem> enumerator = this.HeroTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                SelectTableItem current = enumerator.Current;
                if (current.ItemCard.card_id == _card.card_id)
                {
                    gameObject = current.Root.gameObject;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        gameObject = this.HeroTable[0].Root.gameObject;
        foreach (GameObject obj3 in this.SlotList)
        {
            object obj4 = GUIDataHolder.getData(obj3);
            if (obj4 != null)
            {
                Card card = (Card) obj4;
                if (card.card_id == _card.card_id)
                {
                    GameObject go = UnityEngine.Object.Instantiate(_obj) as GameObject;
                    go.transform.parent = obj3.transform.parent;
                    go.transform.position = obj3.transform.position;
                    go.transform.localScale = new Vector3(this.ScaleParam, this.ScaleParam, 1f);
                    UnityEngine.Object.DestroyImmediate(obj3.transform.FindChild("HeroIcon").gameObject);
                    GUIDataHolder.setData(obj3, null);
                    if (gameObject == null)
                    {
                        TweenTransform.Begin(go, 0.9f, this.ListRoot.transform).method = UITweener.Method.EaseOut;
                    }
                    else
                    {
                        TweenTransform.Begin(go, 0.9f, gameObject.transform).method = UITweener.Method.EaseOut;
                    }
                    TweenAlpha.Begin(go, 0.02f, 0f);
                    base.StartCoroutine(this.PlayAnimAndDestroyObj(go));
                    if (ActorData.getInstance().GetCardByID(_card.card_id) == null)
                    {
                        this.AssistID = -1L;
                    }
                }
            }
        }
        this.ResetSlotPos();
        this.SelectHeroObjList.Remove(_obj);
        this.SelectHeroList.Remove(_card);
        this.UpdateBufferState();
    }

    private void PushCard(GameObject _obj, Card _card)
    {
        Card cardByID = ActorData.getInstance().GetCardByID(_card.card_id);
        if ((this.AssistID > 0L) && (cardByID == null))
        {
            TipsDiag.SetText("只能选择一张助战卡牌!");
        }
        else
        {
            int num = 0;
            GameObject go = this.CreateSelItem(_obj, _card, true, out num);
            TweenPosition.Begin(go, 0.3f, Vector3.zero).method = UITweener.Method.EaseOut;
            if (cardByID == null)
            {
                go.transform.FindChild("AssistTag").gameObject.SetActive(true);
                this.AssistID = _card.card_id;
            }
            else
            {
                go.transform.FindChild("AssistTag").gameObject.SetActive(false);
            }
            this.UpdateBufferState();
        }
    }

    public bool RequestGenerateAssistTab()
    {
        if (!GuideSystem.MatchEvent(GuideEvent.Fomation))
        {
            return false;
        }
        Transform transform = base.transform.FindChild("CheckPart/Toggle5");
        if (null == transform)
        {
            return false;
        }
        GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Battle.tag_select_assist, transform.gameObject);
        return true;
    }

    public void RequestGenerateHeroSelector()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Fomation))
        {
            Transform transform = base.transform.FindChild("Scroll View/Grid");
            if (null != transform)
            {
                int count = this.SelectHeroList.Count;
                int num2 = ActorData.getInstance().CardList.Count;
                if ((count < num2) && (count < 5))
                {
                    int num3 = this.HeroTable.Count;
                    for (int i = 0; i != num3; i++)
                    {
                        SelectTableItem item = this.HeroTable[i];
                        if (item.State == SelectTableItem.CardState.None)
                        {
                            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Battle.tag_formation_select_hero, item.Root.gameObject);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void RequestGenerateStarter()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Start_EliteTrench_Fight))
        {
            Transform transform = base.transform.FindChild("StartBtn");
            if (null != transform)
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Battle.tag_start_elitetrench_fight, transform.gameObject);
            }
        }
    }

    private void ResetPanel(Transform tf)
    {
        SpringPanel component = tf.GetComponent<SpringPanel>();
        if (component != null)
        {
            component.target = Vector3.zero;
            component.enabled = false;
        }
        tf.GetComponent<UIScrollView>().ResetPosition();
        UIScrollBar bar = tf.GetComponent<UIScrollBar>();
        if (bar != null)
        {
            bar.value = 0f;
        }
        tf.GetComponent<ResetListPos>().PanelReset();
    }

    private void ResetSlot()
    {
        foreach (GameObject obj2 in this.SlotList)
        {
            Transform transform = obj2.transform.FindChild("HeroIcon");
            GUIDataHolder.setData(obj2, null);
            if (null != transform)
            {
                UnityEngine.Object.DestroyImmediate(transform.gameObject);
            }
        }
    }

    private void ResetSlotPos()
    {
        List<GameObject> list = new List<GameObject>();
        int num = 0;
        int num2 = 0;
        foreach (GameObject obj3 in this.SlotList)
        {
            num++;
            num2 = 0;
            if (GUIDataHolder.getData(obj3) == null)
            {
                foreach (GameObject obj5 in this.SlotList)
                {
                    num2++;
                    if ((num2 > num) && !this.HasProcess(obj5, list))
                    {
                        object data = GUIDataHolder.getData(obj5);
                        if (data != null)
                        {
                            GameObject gameObject = obj5.transform.FindChild("HeroIcon").gameObject;
                            gameObject.transform.parent = obj3.transform;
                            TweenPosition.Begin(gameObject, 0.3f, Vector3.zero).method = UITweener.Method.EaseOut;
                            GUIDataHolder.setData(obj3, data);
                            GUIDataHolder.setData(obj5, null);
                            list.Add(obj3);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void ResetSlotPosAfterPush(int _index)
    {
        this.CheckNullData();
        foreach (GameObject obj2 in this.SelectHeroObjList)
        {
            if (obj2 == null)
            {
                Debug.LogWarning("SelectHeroObjList Obj is Null!");
            }
            else
            {
                int num = Convert.ToInt32(obj2.transform.parent.name) - 1;
                object obj3 = GUIDataHolder.getData(this.SlotList[num]);
                if (num >= _index)
                {
                    int num2 = num + 1;
                    if (num2 >= (this.SlotList.Count - 1))
                    {
                        num2 = this.SlotList.Count - 1;
                    }
                    GameObject obj4 = this.SlotList[num2];
                    obj2.transform.parent = obj4.transform;
                    TweenPosition.Begin(obj2, 0.3f, Vector3.zero).method = UITweener.Method.EaseOut;
                }
            }
        }
        if (<>f__am$cache26 == null)
        {
            <>f__am$cache26 = obj => GUIDataHolder.setData(obj, null);
        }
        this.SlotList.ForEach(<>f__am$cache26);
        foreach (GameObject obj5 in this.SelectHeroObjList)
        {
            if (obj5 == null)
            {
                Debug.LogWarning("SelectHeroObjList Obj is Null!");
            }
            else
            {
                int num3 = Convert.ToInt32(obj5.transform.parent.name) - 1;
                Card data = (Card) GUIDataHolder.getData(obj5);
                GUIDataHolder.setData(this.SlotList[num3], data);
            }
        }
    }

    public bool SelectionFull()
    {
        int count = this.SelectHeroList.Count;
        int num2 = ActorData.getInstance().CardList.Count;
        return ((count >= num2) || (count >= 4));
    }

    public void SetArenaladderInfo(ArenaLadderEnemy info)
    {
        this.mCurType = BattleType.ArenaLadder;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
        this.HideHeroGird6(false);
        this.SetZhuZhanStat(false);
    }

    public void SetButtonState(BattleType _type)
    {
        this.mCurType = _type;
        if (_type == BattleType.GuildDup)
        {
            base.transform.FindChild("StartBtn").gameObject.SetActive(true);
            base.transform.FindChild("SaveBtn").gameObject.SetActive(false);
        }
        else
        {
            base.transform.FindChild("StartBtn").gameObject.SetActive(false);
            base.transform.FindChild("SaveBtn").gameObject.SetActive(true);
        }
        this.SetZhuZhanStat(false);
    }

    public void SetDetainsDartBattleBackSelf(ConvoyEnemyInfo info)
    {
        this.mCurType = BattleType.DetainsDartBattleBack;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
        this.HideHeroGird6(false);
        this.SetZhuZhanStat(false);
    }

    public void SetDetainsDartBattleInfo(GameDetainsDartMgr.EnermyItemData info)
    {
        this.mCurType = BattleType.ArenaLadder;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
        this.HideHeroGird6(false);
        this.SetZhuZhanStat(false);
    }

    public void SetDungeonsData(dungeons_activity_config _cfg, int Diff)
    {
        this.mDungeonsActivityCfg = _cfg;
        this.DungeonsDifficult = Diff;
    }

    public void SetDupMapState()
    {
        this.mCurType = BattleType.Normal;
        this.HideHeroGird6(true);
        this.SetZhuZhanStat(true);
    }

    public void SetGuildBattleInfo(GuildBattleTargetInfo info)
    {
        this.mCurType = BattleType.GuildBattle;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
        this.HideHeroGird6(false);
        this.SetZhuZhanStat(false);
    }

    public void SetLSXPkTargetInfo(S2C_WarmmatchTargetReq info)
    {
        this.mCurType = BattleType.WarmmatchPk;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
        this.HideHeroGird6(false);
        this.SetZhuZhanStat(false);
    }

    public void SetPkTargetInfo(S2C_GetFriendFormation info)
    {
        this.mCurType = BattleType.FriendPk;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
        this.HideHeroGird6(false);
        this.SetZhuZhanStat(false);
    }

    public void SetTeamPower(List<Card> cardList)
    {
        base.transform.FindChild("TeamPower").GetComponent<UILabel>().text = ActorData.getInstance().GetTeamPowerByCardList(cardList).ToString();
    }

    public void SetTowerInfo(VoidTowerData info)
    {
        this.mCurType = BattleType.TowerPk;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
        this.HideHeroGird6(false);
        this.SetZhuZhanStat(false);
    }

    public void SetWorldCupPkTargetInfo(S2C_GetLeagueOpponentFormation info)
    {
        this.mCurType = BattleType.WorldCupPk;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
        this.HideHeroGird6(false);
        this.SetZhuZhanStat(false);
    }

    public void SetZhuZhanStat(bool isShow)
    {
        base.transform.FindChild("CheckPart/Toggle5").gameObject.SetActive(isShow);
    }

    private void ShowTable(List<TableData> cards)
    {
        int num = 0;
        this.HeroTable.Count = cards.Count;
        for (int i = 0; i < cards.Count; i++)
        {
            this.HeroTable[i].CanDrag = false;
            this.HeroTable[i].mCurType = this.mCurType;
            this.HeroTable[i].State = SelectTableItem.CardState.None;
            this.HeroTable[i].Data = cards[i];
            if ((i == 0) || ((i % 6) == 0))
            {
                num++;
            }
            this.HeroTable[i].OnBuyCoolDown = new Action<SelectTableItem>(this.OnBuyCoolDown);
            this.HeroTable[i].OnClick = new Action<SelectTableItem>(this.OnClickHeroIcon);
            this.HeroTable[i].OnPress = new Action<bool, SelectTableItem>(this.OnHeroPress);
        }
        UIScrollView component = this.ListRoot.transform.parent.GetComponent<UIScrollView>();
        if (num <= 2)
        {
            component.movement = UIScrollView.Movement.Custom;
            component.customMovement = Vector2.zero;
        }
        else
        {
            component.movement = UIScrollView.Movement.Vertical;
            IEnumerator<SelectTableItem> enumerator = this.HeroTable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    SelectTableItem current = enumerator.Current;
                    current.CanDrag = true;
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            if (this.HeroTable.Count > 0)
            {
                Transform transform = component.transform.FindChild("LineDragBox");
                transform.position = new Vector3(transform.position.x, this.HeroTable[cards.Count - 1].ScrollView.transform.position.y, transform.position.z);
            }
        }
    }

    private void StartFight()
    {
        <StartFight>c__AnonStorey1DD storeydd = new <StartFight>c__AnonStorey1DD {
            <>f__this = this
        };
        if (BattleStaticEntry.IsInBattle)
        {
            Debug.LogWarning("Already In Battle !");
        }
        else
        {
            storeydd.cardInfo = new List<long>();
            this.SelectHeroList.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
            switch (this.mBattleType)
            {
                case BattleType.Normal:
                    ActorData.getInstance().mDefaultDupHeroList.Clear();
                    foreach (Card card in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card.card_id);
                        if (ActorData.getInstance().CardList.Contains(card))
                        {
                            ActorData.getInstance().mDefaultDupHeroList.Add(card);
                        }
                    }
                    break;

                case BattleType.WorldBoss:
                    ActorData.getInstance().mDefaultWorldBossList.Clear();
                    foreach (Card card6 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card6.card_id);
                        if (ActorData.getInstance().CardList.Contains(card6))
                        {
                            ActorData.getInstance().mDefaultWorldBossList.Add(card6);
                        }
                    }
                    break;

                case BattleType.FriendPk:
                    ActorData.getInstance().mDefaultFriendPkList.Clear();
                    foreach (Card card5 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card5.card_id);
                        if (ActorData.getInstance().CardList.Contains(card5))
                        {
                            ActorData.getInstance().mDefaultFriendPkList.Add(card5);
                        }
                    }
                    break;

                case BattleType.WorldCupPk:
                    ActorData.getInstance().mDefaultWorldCupPkList.Clear();
                    foreach (Card card4 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card4.card_id);
                        if (ActorData.getInstance().CardList.Contains(card4))
                        {
                            ActorData.getInstance().mDefaultWorldCupPkList.Add(card4);
                        }
                    }
                    break;

                case BattleType.WarmmatchPk:
                    ActorData.getInstance().mDefaultWarmmatchList.Clear();
                    foreach (Card card2 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card2.card_id);
                        if (ActorData.getInstance().CardList.Contains(card2))
                        {
                            ActorData.getInstance().mDefaultWarmmatchList.Add(card2);
                        }
                    }
                    break;

                case BattleType.TowerPk:
                    ActorData.getInstance().mDefaultTowerPkList.Clear();
                    foreach (Card card3 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card3.card_id);
                        if (ActorData.getInstance().CardList.Contains(card3))
                        {
                            ActorData.getInstance().mDefaultTowerPkList.Add(card3);
                        }
                    }
                    break;

                case BattleType.Dungeons:
                    ActorData.getInstance().mDefaultDupHeroList.Clear();
                    foreach (Card card7 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card7.card_id);
                        if (ActorData.getInstance().CardList.Contains(card7))
                        {
                            ActorData.getInstance().mDefaultDupHeroList.Add(card7);
                        }
                    }
                    break;

                case BattleType.YuanZhengPk:
                    ActorData.getInstance().mDefaultYuanZhengPkList.Clear();
                    foreach (Card card8 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card8.card_id);
                        if (ActorData.getInstance().CardList.Contains(card8))
                        {
                            ActorData.getInstance().mDefaultYuanZhengPkList.Add(card8);
                        }
                    }
                    break;

                case BattleType.ArenaLadder:
                    ActorData.getInstance().mDefaultArenaLadderList.Clear();
                    foreach (Card card13 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card13.card_id);
                        if (ActorData.getInstance().CardList.Contains(card13))
                        {
                            ActorData.getInstance().mDefaultArenaLadderList.Add(card13);
                        }
                    }
                    break;

                case BattleType.GuildBattle:
                    ActorData.getInstance().mDefaultGuildBattleList.Clear();
                    foreach (Card card14 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card14.card_id);
                        if (ActorData.getInstance().CardList.Contains(card14))
                        {
                            ActorData.getInstance().mDefaultGuildBattleList.Add(card14);
                        }
                    }
                    break;

                case BattleType.GuildDup:
                    ActorData.getInstance().mDefaultGuildDupBattleList.Clear();
                    foreach (Card card15 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card15.card_id);
                        if (ActorData.getInstance().CardList.Contains(card15))
                        {
                            ActorData.getInstance().mDefaultGuildDupBattleList.Add(card15);
                        }
                    }
                    break;

                case BattleType.OutLandBattle_tollGate_0:
                    ActorData.getInstance().mDefaultOutlandPkList_0.Clear();
                    foreach (Card card9 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card9.card_id);
                        if (ActorData.getInstance().CardList.Contains(card9))
                        {
                            ActorData.getInstance().mDefaultOutlandPkList_0.Add(card9);
                        }
                    }
                    break;

                case BattleType.OutLandBattle_tollGate_1:
                    ActorData.getInstance().mDefaultOutlandPkList_1.Clear();
                    foreach (Card card10 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card10.card_id);
                        if (ActorData.getInstance().CardList.Contains(card10))
                        {
                            ActorData.getInstance().mDefaultOutlandPkList_1.Add(card10);
                        }
                    }
                    break;

                case BattleType.OutLandBattle_tollGate_2:
                    ActorData.getInstance().mDefaultOutlandPkList_2.Clear();
                    foreach (Card card11 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card11.card_id);
                        if (ActorData.getInstance().CardList.Contains(card11))
                        {
                            ActorData.getInstance().mDefaultOutlandPkList_2.Add(card11);
                        }
                    }
                    break;

                case BattleType.OutLandBattle_tollGate_3:
                    ActorData.getInstance().mDefaultOutlandPkList_3.Clear();
                    foreach (Card card12 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card12.card_id);
                        if (ActorData.getInstance().CardList.Contains(card12))
                        {
                            ActorData.getInstance().mDefaultOutlandPkList_3.Add(card12);
                        }
                    }
                    break;

                case BattleType.DerainsDartInterceptBattle:
                    ActorData.getInstance().mDefaultDetainsDart_Intercept_List.Clear();
                    foreach (Card card16 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card16.card_id);
                        if (ActorData.getInstance().CardList.Contains(card16))
                        {
                            ActorData.getInstance().mDefaultDetainsDart_Intercept_List.Add(card16);
                        }
                    }
                    break;

                case BattleType.DetainsDartBattleBack:
                    ActorData.getInstance().mDefaultDetainsDart_BattleBackSelf_List.Clear();
                    foreach (Card card17 in this.SelectHeroList)
                    {
                        storeydd.cardInfo.Add(card17.card_id);
                        if (ActorData.getInstance().CardList.Contains(card17))
                        {
                            ActorData.getInstance().mDefaultDetainsDart_BattleBackSelf_List.Add(card17);
                        }
                    }
                    break;
            }
            if (GuideSystem.MatchEvent(GuideEvent.Start_EliteTrench_Fight))
            {
                GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_start_elitetrench_fight, null);
            }
            ActorData.getInstance().SaveFormationData(this.mBattleType);
            if (storeydd.cardInfo.Count == 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x590));
            }
            else if (((this.mCurType != BattleType.GuildDup) && (storeydd.cardInfo.Count < 5)) && (ActorData.getInstance().CardList.Count >= 5))
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeydd.<>m__2F3), null);
            }
            else
            {
                this.EnterBattle(storeydd.cardInfo);
            }
        }
    }

    private void TestFight()
    {
        List<long> cardGUIDs = new List<long>();
        foreach (Card card in this.SelectHeroList)
        {
            cardGUIDs.Add(card.card_id);
        }
        if (cardGUIDs.Count != 0)
        {
            SocketMgr.Instance.RequestEnterDup(0, 1, DuplicateType.DupType_Normal, cardGUIDs, -1L);
        }
    }

    public void UnsubscribeEvent()
    {
        EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
    }

    private void UpdateAssistCardList()
    {
        CommonFunc.ResetClippingPanel(base.transform.FindChild("Scroll View"));
        this.InAssistPage = true;
        if (<>f__am$cache1F == null)
        {
            <>f__am$cache1F = delegate (AssistUser user1, AssistUser user2) {
                if (user1.reward_eq != user2.reward_eq)
                {
                    return user2.reward_eq - user1.reward_eq;
                }
                return user2.userInfo.level - user1.userInfo.level;
            };
        }
        ActorData.getInstance().AssistUserList.Sort(<>f__am$cache1F);
        List<TableData> cards = new List<TableData>();
        if (XSingleton<SocialFriend>.Singleton.State == SocialFriend.SocialState.Ready)
        {
            if (<>f__am$cache20 == null)
            {
                <>f__am$cache20 = t => (t.UserInfo != null) && !t.Owner;
            }
            if (<>f__am$cache21 == null)
            {
                <>f__am$cache21 = t => t.QQUser;
            }
            if (<>f__am$cache22 == null)
            {
                <>f__am$cache22 = t => new TableData { CardData = t.userInfo.leaderInfo, User = null, UserID = t.userInfo.id, Name = t.userInfo.name, IsAssistCard = true, cdTime = t.cool_down_time, IsQQ = true, QQUser = t };
            }
            cards = XSingleton<SocialFriend>.Singleton.Users.Where<SocialUser>(<>f__am$cache20).Select<SocialUser, QQFriendUser>(<>f__am$cache21).Select<QQFriendUser, TableData>(<>f__am$cache22).ToList<TableData>();
        }
        if (<>f__am$cache23 == null)
        {
            <>f__am$cache23 = t => new TableData { CardData = t.userInfo.leaderInfo, IsAssistCard = true, User = t, UserID = t.userInfo.id, Name = t.userInfo.name };
        }
        IEnumerable<TableData> source = ActorData.getInstance().AssistUserList.Select<AssistUser, TableData>(<>f__am$cache23);
        cards.AddRange(source.ToList<TableData>());
        this.AssistCardDict.Clear();
        foreach (TableData data in cards)
        {
            data.CardData.card_id = data.UserID;
            if (!this.AssistCardDict.ContainsKey(data.UserID))
            {
                this.AssistCardDict.Add(data.UserID, data);
            }
        }
        this.ShowTable(cards);
        this.UpdateBufferState();
    }

    internal void UpdateAssitCard(long targetID)
    {
        IEnumerator<SelectTableItem> enumerator = this.HeroTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                SelectTableItem current = enumerator.Current;
                if (current.Data.CardData.card_id == targetID)
                {
                    current.Data.cdTime = 0;
                    current.Data = current.Data;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    private void UpdateBufferState()
    {
        this.SetTeamPower(this.SelectHeroList);
        IEnumerator<SelectTableItem> enumerator = this.HeroTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                SelectTableItem current = enumerator.Current;
                Card itemCard = current.ItemCard;
                if (this.SelectHeroList.Contains(itemCard))
                {
                    current.State = SelectTableItem.CardState.Selected;
                }
                else if (current.State == SelectTableItem.CardState.Selected)
                {
                    current.State = SelectTableItem.CardState.None;
                    current.Data = current.Data;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    public void UpdateCardList()
    {
        <UpdateCardList>c__AnonStorey1DB storeydb = new <UpdateCardList>c__AnonStorey1DB {
            <>f__this = this
        };
        this.InAssistPage = false;
        PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
        if (component != null)
        {
            this.ResetPanel(this.ListRoot.transform.parent);
            storeydb.type = component.FsmVariables.FindFsmInt("StandType");
            if (<>f__am$cache24 == null)
            {
                <>f__am$cache24 = t => new TableData { User = null, CardData = t };
            }
            List<TableData> cards = ActorData.getInstance().CardList.Where<Card>(new Func<Card, bool>(storeydb.<>m__2ED)).Select<Card, TableData>(<>f__am$cache24).ToList<TableData>();
            if (<>f__am$cache25 == null)
            {
                <>f__am$cache25 = delegate (TableData cl, TableData cr) {
                    Card cardData = cl.CardData;
                    Card card2 = cr.CardData;
                    if (cardData.cardInfo.level != card2.cardInfo.level)
                    {
                        return card2.cardInfo.level - cardData.cardInfo.level;
                    }
                    if (cardData.cardInfo.quality != card2.cardInfo.quality)
                    {
                        return card2.cardInfo.quality - cardData.cardInfo.quality;
                    }
                    if (cardData.cardInfo.starLv != card2.cardInfo.starLv)
                    {
                        return card2.cardInfo.starLv - cardData.cardInfo.starLv;
                    }
                    return 1;
                };
            }
            cards.Sort(<>f__am$cache25);
            this.ShowTable(cards);
            this.UpdateBufferState();
            if (this.mCurType == BattleType.RichActivity)
            {
                this.UpdateRichActivityState();
            }
        }
    }

    private void UpdateData()
    {
        this.UpdateCardList();
        this.DefaultSelectHero();
    }

    private void UpdateRichActivityState()
    {
        IEnumerator<SelectTableItem> enumerator = this.HeroTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                SelectTableItem current = enumerator.Current;
                Card itemCard = current.ItemCard;
                if ((itemCard != null) && this.RichActivityHeroEntryList.Contains((int) itemCard.cardInfo.entry))
                {
                    current.State = SelectTableItem.CardState.InLifeSkill;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    public int MaxCardCnt
    {
        get
        {
            return this.maxCardCnt;
        }
        set
        {
            this.maxCardCnt = value;
            for (int i = 0; i < this.SlotList.Count; i++)
            {
                this.SlotList[i].SetActive(i < this.maxCardCnt);
            }
        }
    }

    public BattleType mBattleType
    {
        get
        {
            return this.mCurType;
        }
        set
        {
            this.SaveButtonText = ConfigMgr.getInstance().GetWord(0x2c69);
            this.mCurType = value;
            float x = 1f;
            if (value == BattleType.RichActivity)
            {
                this.MaxCardCnt = 1;
                this.SelectHeroList.Clear();
            }
            else if (value == BattleType.ChallengeDefense)
            {
                this.MaxCardCnt = 8;
                x = 0.65f;
            }
            else
            {
                this.MaxCardCnt = 5;
            }
            this.lb_timelimit.ActiveSelfObject(value == BattleType.GuildDup);
            base.transform.FindChild("SelectHero").localScale = new Vector3(x, x, 1f);
            this.UpdateData();
        }
    }

    public string SaveButtonText
    {
        get
        {
            return this._text;
        }
        set
        {
            this._text = value;
            this.SaveButton.Text(value);
        }
    }

    [CompilerGenerated]
    private sealed class <OnBuyCoolDown>c__AnonStorey1D9
    {
        internal SelectHeroPanel.SelectTableItem obj;

        internal void <>m__2E5(GameObject o)
        {
            SocketMgr.Instance.RequestC2S_Clear_QQFriend_CoolDown(this.obj.Data.CardData.card_id);
        }
    }

    [CompilerGenerated]
    private sealed class <OnHeroLongPress>c__AnonStorey1DA
    {
        internal SelectHeroPanel.SelectTableItem hero;

        internal void <>m__2E6(GUIEntity obj)
        {
            ((HeroInfoPanel) obj).OnlyShowCardInfo(this.hero.Data.CardData, this.hero.Data.Name, (this.hero.Data.User == null) ? 0 : this.hero.Data.User.power);
        }
    }

    [CompilerGenerated]
    private sealed class <OnUpdate>c__AnonStorey1DC
    {
        internal SelectHeroPanel.SelectTableItem hero;

        internal void <>m__2F2(GUIEntity obj)
        {
            ((HeroInfoPanel) obj).OnlyShowCardInfo(this.hero.Data.CardData, this.hero.Data.Name, (this.hero.Data.User == null) ? ((this.hero.Data.QQUser == null) ? 0 : this.hero.Data.QQUser.power) : this.hero.Data.User.power);
        }
    }

    [CompilerGenerated]
    private sealed class <PlayAnimAndDestroyObj>c__Iterator78 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <$>obj;
        internal GameObject obj;

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
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    return true;

                case 1:
                    UnityEngine.Object.Destroy(this.obj);
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

    [CompilerGenerated]
    private sealed class <StartFight>c__AnonStorey1DD
    {
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        internal SelectHeroPanel <>f__this;
        internal List<long> cardInfo;

        internal void <>m__2F3(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (GameObject no) {
                };
            }
            box.SetDialog(ConfigMgr.getInstance().GetWord(0x591), yes => this.<>f__this.EnterBattle(this.cardInfo), <>f__am$cache2, false);
        }

        internal void <>m__2F4(GameObject yes)
        {
            this.<>f__this.EnterBattle(this.cardInfo);
        }

        private static void <>m__2F5(GameObject no)
        {
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateCardList>c__AnonStorey1DB
    {
        internal SelectHeroPanel <>f__this;
        internal FsmInt type;

        internal bool <>m__2ED(Card t)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) t.cardInfo.entry);
            if (_config == null)
            {
                return false;
            }
            if ((this.type.Value != 0) && (this.type.Value != _config.stand_type))
            {
                return false;
            }
            if (this.<>f__this.mBattleType == BattleType.RichActivity)
            {
                if (_config.type != 1)
                {
                    return false;
                }
            }
            else if (((this.<>f__this.mCurType >= BattleType.OutLandBattle_tollGate_0) && (this.<>f__this.mCurType <= BattleType.OutLandBattle_tollGate_3)) && ((ActorData.getInstance().outlandType == 2) && (_config.type == 1)))
            {
                return false;
            }
            return true;
        }
    }

    public class SelectTableItem : UITableItem
    {
        private Card _card;
        private SelectHeroPanel.TableData _data;
        private CardState _state;
        public AssistUser _UserData;
        private UISprite AssistTag;
        private bool canSelected = true;
        private UISlider Cd;
        private Transform CDInfo;
        private UISprite firendframe;
        private UISprite frame;
        private UISlider Hp;
        private UITexture Icon;
        private UISprite Icon_fr;
        private UISprite jobIcon;
        private UILabel Label;
        private UILabel Label_time;
        private float LastTime;
        private UISprite mask;
        private UILabel name;
        public Action<SelectHeroPanel.SelectTableItem> OnBuyCoolDown;
        public Action<SelectHeroPanel.SelectTableItem> OnClick;
        public Action<SelectHeroPanel.SelectTableItem> OnLongPress;
        public Action<bool, SelectHeroPanel.SelectTableItem> OnPress;
        private UISprite qqIcon;
        public UIDragScrollView ScrollView;
        private UISprite Selected;
        private Transform Star;
        private UILabel StateTipsLabel;
        private UITableManager<UIStartItem> TableStar = new UITableManager<UIStartItem>();

        public override void OnCreate()
        {
            this.Label = base.FindChild<UILabel>("Label");
            this.Icon = base.FindChild<UITexture>("Icon");
            this.frame = base.FindChild<UISprite>("frame");
            this.name = base.FindChild<UILabel>("name");
            this.mask = base.FindChild<UISprite>("mask");
            this.firendframe = base.FindChild<UISprite>("firendframe");
            this.AssistTag = base.FindChild<UISprite>("AssistTag");
            this.jobIcon = base.FindChild<UISprite>("jobIcon");
            this.Hp = base.FindChild<UISlider>("Hp");
            this.Cd = base.FindChild<UISlider>("Cd");
            this.StateTipsLabel = base.FindChild<UILabel>("StateTipsLabel");
            this.Icon_fr = base.FindChild<UISprite>("Icon_fr");
            this.Selected = base.FindChild<UISprite>("Selected");
            this.CDInfo = base.FindChild<Transform>("CDInfo");
            this.Star = base.Root.FindChild<Transform>("Star");
            this.Label_time = base.FindChild<UILabel>("Label_time");
            this.qqIcon = base.FindChild<UISprite>("qqIcon");
            this.ScrollView = base.Root.GetComponent<UIDragScrollView>();
            this.TableStar.InitFromGrid(this.Star.GetComponent<UIGrid>());
            this.TableStar.Count = 0;
            base.Root.OnUIMouseClick(delegate (object o) {
                if (this.State == CardState.CoolDown)
                {
                    if (this.OnBuyCoolDown != null)
                    {
                        this.OnBuyCoolDown(this);
                    }
                }
                else if (this.canSelected && (this.OnClick != null))
                {
                    Debug.Log(TimeMgr.Instance.ServerStampTime);
                    this.OnClick(this);
                }
            });
            base.Root.OnLongPress(delegate (object o) {
                if (this.Data.IsAssistCard && (this.OnLongPress != null))
                {
                    this.OnLongPress(this);
                }
            });
            base.Root.OnUIMousePress(delegate (bool isDown, object o) {
                if (this.Data.IsAssistCard && (this.OnPress != null))
                {
                    this.OnPress(isDown, this);
                }
            });
        }

        public void SetEnableClick(bool enable)
        {
            this.canSelected = enable;
        }

        public void Update()
        {
            if ((this.State == CardState.CoolDown) && ((this.Data.cdTime > 0) && ((this.LastTime + 1f) < Time.time)))
            {
                this.LastTime = Time.time;
                DateTime serverDateTime = TimeMgr.Instance.ServerDateTime;
                TimeSpan span = (TimeSpan) (TimeMgr.Instance.ConvertToDateTime((long) this.Data.cdTime) - serverDateTime);
                this.CDInfo.gameObject.SetActive(span.TotalSeconds > 0.0);
                this.Label_time.text = string.Format("{0:00}:{1:00}:{2:00}", Math.Floor(span.TotalHours), span.Minutes, span.Seconds);
                if (span.TotalSeconds <= 0.0)
                {
                    this.State = CardState.None;
                }
            }
        }

        private void UpdateHeroIconData(Card _data)
        {
            this.name.text = string.Empty;
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _data.cardInfo.entry);
            if (_config == null)
            {
                Debug.LogWarning("CardCfg Is Null! Entry is " + _data.cardInfo.entry);
            }
            else
            {
                this.jobIcon.spriteName = GameConstant.CardJobIcon[(_config.class_type >= 0) ? _config.class_type : 0];
                this.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                this.Label.text = _data.cardInfo.level.ToString();
                CommonFunc.SetQualityBorder(this.frame, _data.cardInfo.quality);
                this.Star.gameObject.SetActive(true);
                this.TableStar.Count = _data.cardInfo.starLv;
                if (this.Data.Name == null)
                {
                }
                this.name.text = string.Empty;
                if (this.mCurType == BattleType.YuanZhengPk)
                {
                    UISlider hp = this.Hp;
                    UISlider cd = this.Cd;
                    FlameBattleSelfData flameCardByEntry = ActorData.getInstance().GetFlameCardByEntry((int) _data.cardInfo.entry);
                    if (flameCardByEntry == null)
                    {
                        hp.value = 1f;
                        cd.value = 0f;
                    }
                    else
                    {
                        hp.value = ((float) flameCardByEntry.card_cur_hp) / ((float) flameCardByEntry.card_max_hp);
                        if (hp.value == 0f)
                        {
                            cd.value = 0f;
                        }
                        else
                        {
                            cd.value = ((float) flameCardByEntry.card_cur_energy) / ((float) AiDef.MAX_ENERGY);
                        }
                    }
                    this.State = (hp.value != 0f) ? CardState.None : CardState.Dead;
                    hp.gameObject.SetActive(true);
                    cd.gameObject.SetActive(true);
                }
                if ((this.mCurType >= BattleType.OutLandBattle_tollGate_0) && (this.mCurType <= BattleType.OutLandBattle_tollGate_3))
                {
                    UISlider slider3 = this.Hp;
                    UISlider slider4 = this.Cd;
                    OutlandBattleBackCardInfo outlandCardByEntry = ActorData.getInstance().GetOutlandCardByEntry((int) _data.cardInfo.entry);
                    if (outlandCardByEntry == null)
                    {
                        slider3.value = 1f;
                        slider4.value = 0f;
                    }
                    else
                    {
                        slider3.value = ((float) outlandCardByEntry.card_cur_hp) / ((float) CommonFunc.GetCardCurrMaxHp(_data));
                        if (slider3.value == 0f)
                        {
                            slider4.value = 0f;
                        }
                        else
                        {
                            slider4.value = ((float) outlandCardByEntry.card_cur_energy) / ((float) AiDef.MAX_ENERGY);
                        }
                    }
                    this.State = (slider3.value != 0f) ? CardState.None : CardState.Dead;
                    slider3.gameObject.SetActive(true);
                    slider4.gameObject.SetActive(true);
                }
                if (this.mCurType == BattleType.GuildDup)
                {
                    bool flag = XSingleton<GameGuildMgr>.Singleton.IsDead((int) _data.cardInfo.entry);
                    this.State = !flag ? CardState.None : CardState.Used;
                }
                if (this.mCurType == BattleType.DetainsDartEscort)
                {
                    List<ConvoyInfo> escortListInfo = XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo;
                    if ((escortListInfo != null) && (escortListInfo.Count != 0))
                    {
                        for (int i = 0; i < escortListInfo.Count; i++)
                        {
                            ConvoyInfo info2 = escortListInfo[i];
                            if (info2.cards.Contains(this._card.card_id))
                            {
                                this.State = CardState.Used;
                            }
                        }
                    }
                }
            }
        }

        public bool CanDrag
        {
            get
            {
                return this.ScrollView.enabled;
            }
            set
            {
                this.ScrollView.enabled = value;
            }
        }

        public SelectHeroPanel.TableData Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
                this._UserData = value.User;
                this.ItemCard = value.CardData;
                if (value.cdTime > 0)
                {
                    if (TimeMgr.Instance.ConvertToDateTime((long) value.cdTime) > TimeMgr.Instance.ServerDateTime)
                    {
                        this.State = CardState.CoolDown;
                    }
                }
                else if (this.State == CardState.CoolDown)
                {
                    this.State = CardState.None;
                }
                this.AssistTag.gameObject.SetActive(!value.IsQQ && this.Data.IsAssistCard);
                if (value.IsQQ)
                {
                    this.qqIcon.spriteName = (GameDefine.getInstance().GetTencentType() != TencentType.QQ) ? "Ui_Login_Icon_weixin" : "Ui_Login_Icon_QQ";
                    this.qqIcon.height = 0x27;
                    this.qqIcon.width = 0x27;
                }
                this.qqIcon.gameObject.SetActive(value.IsQQ);
            }
        }

        public Card ItemCard
        {
            get
            {
                return this._card;
            }
            set
            {
                this._card = value;
                this.UpdateHeroIconData(value);
            }
        }

        public BattleType mCurType { get; set; }

        public CardState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                this.Selected.gameObject.SetActive(false);
                switch (value)
                {
                    case CardState.None:
                        this.SetEnableClick(true);
                        this.StateTipsLabel.gameObject.SetActive(false);
                        this.Hp.gameObject.SetActive(false);
                        this.Cd.gameObject.SetActive(false);
                        this.Icon_fr.gameObject.SetActive(false);
                        this.CDInfo.gameObject.SetActive(false);
                        nguiTextureGrey.doChangeEnableGrey(this.Icon, false);
                        break;

                    case CardState.Dead:
                        this.SetEnableClick(false);
                        this.StateTipsLabel.text = ConfigMgr.getInstance().GetWord(0x854);
                        this.StateTipsLabel.gameObject.SetActive(true);
                        nguiTextureGrey.doChangeEnableGrey(this.Icon, true);
                        break;

                    case CardState.InLifeSkill:
                        nguiTextureGrey.doChangeEnableGrey(this.Icon, true);
                        this.StateTipsLabel.text = ConfigMgr.getInstance().GetWord(0x853);
                        this.StateTipsLabel.gameObject.SetActive(true);
                        this.SetEnableClick(false);
                        break;

                    case CardState.Selected:
                        nguiTextureGrey.doChangeEnableGrey(this.Icon, true);
                        this.SetEnableClick(true);
                        this.Selected.gameObject.SetActive(true);
                        break;

                    case CardState.CoolDown:
                        nguiTextureGrey.doChangeEnableGrey(this.Icon, true);
                        this.SetEnableClick(false);
                        break;

                    case CardState.Used:
                        this.SetEnableClick(false);
                        this.StateTipsLabel.text = ConfigMgr.getInstance().GetWord(0x856);
                        this.StateTipsLabel.gameObject.SetActive(true);
                        nguiTextureGrey.doChangeEnableGrey(this.Icon, true);
                        break;
                }
            }
        }

        public enum CardState
        {
            None,
            Dead,
            InLifeSkill,
            Selected,
            CoolDown,
            Used
        }

        public class UIStartItem : UITableItem
        {
            public override void OnCreate()
            {
            }
        }
    }

    public class TableData
    {
        public Card CardData { get; set; }

        public int cdTime { get; set; }

        public bool IsAssistCard { get; set; }

        public bool IsQQ { get; set; }

        public string Name { get; set; }

        public QQFriendUser QQUser { get; set; }

        public AssistUser User { get; set; }

        public long UserID { get; set; }
    }
}

