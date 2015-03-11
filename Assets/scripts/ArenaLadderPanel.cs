using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArenaLadderPanel : GUIEntity
{
    public UILabel _Time;
    public GameObject _UpdateBtn;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheA;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheB;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheC;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheD;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cacheE;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cacheF;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private int mbuyAttackCount;
    private int mEndTime;
    private bool mIsStart;
    private int mMyRank;
    private int mMyTopRank;
    private int mRemainAttackCount;

    private void BuyPkCount()
    {
    }

    private void ClosePanel()
    {
        for (int i = 0; i < 3; i++)
        {
            base.transform.FindChild("Target/" + (i + 1)).gameObject.SetActive(false);
        }
        GUIMgr.Instance.PopGUIEntity();
    }

    private void EnemyRefresh(GameObject go)
    {
        SocketMgr.Instance.RequestArenaLadderInfo();
    }

    private int GetBuyCost()
    {
        buy_cost_config _config = ConfigMgr.getInstance().getByEntry<buy_cost_config>(this.mbuyAttackCount);
        if (_config != null)
        {
            return _config.buy_arenaladder_attack_cost_stone;
        }
        return 0;
    }

    private int GetBuyCount()
    {
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config != null)
        {
            Debug.Log(_config.arenaladder_buyattack_times + " ");
            return _config.arenaladder_buyattack_times;
        }
        return 0;
    }

    [DebuggerHidden]
    private IEnumerator InitTargetPlayer(List<ArenaLadderEnemy> _enemyList)
    {
        return new <InitTargetPlayer>c__Iterator51 { _enemyList = _enemyList, <$>_enemyList = _enemyList, <>f__this = this };
    }

    private void OnClickAllRankInOneBtn(GameObject go)
    {
        AllRankInOnePanel.Open(EN_RankListType.EN_RANKLIST_AREAN, false);
    }

    private void OnClickBuyCount(GameObject go)
    {
        if (ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Arena_Shop_Buy) != null)
        {
            SocketMgr.Instance.RequesArenaLadderBuyAttack(true);
        }
        else
        {
            <OnClickBuyCount>c__AnonStorey16E storeye = new <OnClickBuyCount>c__AnonStorey16E {
                cost = this.GetBuyCost()
            };
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeye.<>m__181), null);
        }
    }

    private void OnClickChangeTeam()
    {
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = delegate (GUIEntity entity) {
                SelectHeroPanel panel = (SelectHeroPanel) entity;
                panel.Depth = 600;
                panel.mBattleType = BattleType.WarmmatchDefense;
                panel.SetButtonState(BattleType.WarmmatchDefense);
                panel.SetZhuZhanStat(false);
            };
        }
        GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cacheA, null);
    }

    private void OnClickClearTimeBtn(GameObject go)
    {
        if (ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Arena_Reset) != null)
        {
            SocketMgr.Instance.RequesArenaLadderRefreshAttackTime(true);
        }
        else
        {
            if (<>f__am$cacheD == null)
            {
                <>f__am$cacheD = delegate (GUIEntity obj) {
                    MessageBox box = (MessageBox) obj;
                    if (ActorData.getInstance().Stone < 50)
                    {
                        string str = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0]);
                        if (<>f__am$cacheE == null)
                        {
                            <>f__am$cacheE = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                        }
                        box.SetDialog(str, <>f__am$cacheE, null, false);
                    }
                    else
                    {
                        if (<>f__am$cacheF == null)
                        {
                            <>f__am$cacheF =  => SocketMgr.Instance.RequesArenaLadderRefreshAttackTime(false);
                        }
                        box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x845), 50), <>f__am$cacheF, null, false);
                    }
                };
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cacheD, null);
        }
    }

    private void OnClickHistroyBtn(GameObject go)
    {
        GUIMgr.Instance.DoModelGUI("LeagueHistoryPanel", delegate (GUIEntity obj) {
            LeagueHistoryPanel panel = (LeagueHistoryPanel) obj;
            panel.Depth = 600;
            ActorData.getInstance().HaveNewArenaLog = false;
            this.UpdateLogNewTips();
            SocketMgr.Instance.RequesArenaLadderCombatHistory();
        }, null);
    }

    private void OnClickPkBtn(GameObject go)
    {
        <OnClickPkBtn>c__AnonStorey16D storeyd = new <OnClickPkBtn>c__AnonStorey16D();
        ActorData.getInstance().mCurrDupReturnPrePara = null;
        if (this.mIsStart)
        {
            string str = TimeMgr.Instance.GetRemainTime2(this.mEndTime);
            if (str == string.Empty)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x83e));
            }
            else
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x841), str) + ConfigMgr.getInstance().GetWord(0x4f9));
            }
        }
        else if (this.mRemainAttackCount < 1)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x846));
        }
        else
        {
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                storeyd.info = obj2 as ArenaLadderEnemy;
                if (storeyd.info != null)
                {
                    GUIMgr.Instance.DoModelGUI("SelectHeroPanel", new Action<GUIEntity>(storeyd.<>m__17B), null);
                }
            }
        }
    }

    private void OnClickRankingBtn(GameObject go)
    {
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = delegate (GUIEntity entity) {
                RankingListPanel panel = (RankingListPanel) entity;
                panel.Depth = 600;
                panel.transform.FindChild("WaitPanel").gameObject.SetActive(true);
                SocketMgr.Instance.RequestArenaLadderRankList();
            };
        }
        GUIMgr.Instance.DoModelGUI("RankingListPanel", <>f__am$cacheB, null);
    }

    private void OnClickShopBtn(GameObject go)
    {
        if (<>f__am$cacheC == null)
        {
            <>f__am$cacheC = delegate (GUIEntity obj) {
                ((ShopPanel) obj).SetShopType(ShopCoinType.ArenaLadderCoin);
                SocketMgr.Instance.RequestArenaLadderShopInfo();
            };
        }
        GUIMgr.Instance.PushGUIEntity("ShopPanel", <>f__am$cacheC);
    }

    private void OnClickShuoMingBtn(GameObject go)
    {
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", delegate (GUIEntity obj) {
            WorldCupRulePanel panel = (WorldCupRulePanel) obj;
            panel.Depth = 800;
            panel.ShowArenaLadderRule(this.mMyRank, this.mMyTopRank);
        }, null);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(true);
        }
        GUIMgr.Instance.FloatTitleBar();
        this.UpdateTeamInfo();
        this.UpdateLogNewTips();
        this.SetUpdateBtnStat();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        SocketMgr.Instance.RequestArenaLadderInfo();
        Transform transform = base.transform.FindChild("RankingBtn");
        Transform transform2 = transform.Find("Label");
        if (null != transform2)
        {
            transform2.GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0xbc5);
        }
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAllRankInOneBtn);
        UIEventListener.Get(base.transform.FindChild("ShopBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShopBtn);
        UIEventListener.Get(base.transform.FindChild("ShuoMingBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShuoMingBtn);
        UIEventListener.Get(base.transform.FindChild("HistroyBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickHistroyBtn);
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(false);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                if (this.mEndTime > TimeMgr.Instance.ServerStampTime)
                {
                    this._Time.text = string.Format(ConfigMgr.getInstance().GetWord(0x841), TimeMgr.Instance.GetRemainTime2(this.mEndTime));
                    if (!this._Time.gameObject.active)
                    {
                        this._Time.gameObject.SetActive(true);
                    }
                }
                else
                {
                    this._Time.gameObject.SetActive(false);
                    this.SetUpdateBtnStat();
                    this.mIsStart = false;
                }
                if (((TimeMgr.Instance.ServerDateTime.Hour == GameConstValues.FRESH_USERDATA_TIME) && (TimeMgr.Instance.ServerDateTime.Minute == 0)) && (TimeMgr.Instance.ServerDateTime.Second == 1))
                {
                    SocketMgr.Instance.RequestArenaLadderInfo();
                }
                if (((TimeMgr.Instance.ServerDateTime.Hour == 20) && (TimeMgr.Instance.ServerDateTime.Minute == 0)) && (TimeMgr.Instance.ServerDateTime.Second == 2))
                {
                    SocketMgr.Instance.RequestGetMailList();
                    TimeMgr.Instance.ArenaLadderRewardTime = TimeMgr.Instance.ServerDateTime.Date.AddDays(1.0).AddHours(20.0);
                }
            }
        }
        this.UpdateLogNewTips();
    }

    public void ResetAttactCount(int _buyAttackCount, int _attackCount)
    {
        this.mbuyAttackCount = _buyAttackCount;
        this.mEndTime = 0;
        this.mIsStart = false;
        this._Time.gameObject.SetActive(false);
        this.mRemainAttackCount = 5 - _attackCount;
        this.SetAttackCountInfo(this.mRemainAttackCount);
        this.SetUpdateBtnStat();
    }

    public void ResetAttactTime()
    {
        this.SetUpdateBtnStat();
        this.mEndTime = 0;
        this._Time.gameObject.SetActive(false);
        this.SetUpdateBtnStat();
        this.mIsStart = false;
    }

    private void SetAttackCountInfo(int remainCount)
    {
        base.transform.FindChild("Info/Count").GetComponent<UILabel>().text = remainCount + "/" + 5;
    }

    private void SetMyRank(int selfOrder)
    {
        base.transform.FindChild("Info/MyRank").GetComponent<UILabel>().text = selfOrder.ToString();
    }

    private void SetTeamInfo(Transform obj, Card _card)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                obj.FindChild("Level").GetComponent<UILabel>().text = _card.cardInfo.level.ToString();
                CommonFunc.SetQualityBorder(obj.FindChild("QualityBorder").GetComponent<UISprite>(), _card.cardInfo.quality);
                obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                CommonFunc.SetCardJobIcon(obj.FindChild("Job/jobIcon").GetComponent<UISprite>(), _config.class_type);
                for (int i = 0; i < 5; i++)
                {
                    UISprite component = obj.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                    component.gameObject.SetActive(i < _card.cardInfo.starLv);
                    component.transform.localPosition = new Vector3((float) (i * 0x13), 0f, 0f);
                }
                Transform transform = obj.transform.FindChild("Star");
                transform.localPosition = new Vector3(-6.8f - ((_card.cardInfo.starLv - 1) * 9.5f), transform.localPosition.y, 0f);
                transform.gameObject.SetActive(true);
            }
        }
    }

    public void SetTeamPower(List<Card> cardList)
    {
        base.transform.FindChild("Team/FightPower").GetComponent<UILabel>().text = ActorData.getInstance().GetTeamPowerByCardList(cardList).ToString();
    }

    private void SetUpdateBtnStat()
    {
        Transform transform = base.transform.FindChild("BuyCost");
        UILabel component = base.transform.FindChild("BuyCost/Label").GetComponent<UILabel>();
        UILabel label2 = this._UpdateBtn.transform.FindChild("Label").GetComponent<UILabel>();
        UISprite sprite = transform.FindChild("Icon").GetComponent<UISprite>();
        if ((this.mRemainAttackCount < 1) && (this.GetBuyCount() > 0))
        {
            label2.text = ConfigMgr.getInstance().GetWord(0x843);
            UIEventListener.Get(this._UpdateBtn).onClick = new UIEventListener.VoidDelegate(this.OnClickBuyCount);
            Item ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Arena_Shop_Buy);
            if (ticketItemBySubType != null)
            {
                component.text = "X" + ticketItemBySubType.num;
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(ticketItemBySubType.entry);
                if (_config != null)
                {
                    sprite.spriteName = _config.icon;
                }
            }
            else
            {
                component.text = this.GetBuyCost().ToString();
                sprite.spriteName = "Item_Icon_Stone";
            }
            transform.gameObject.SetActive(true);
        }
        else if (this.mEndTime > TimeMgr.Instance.ServerStampTime)
        {
            label2.text = ConfigMgr.getInstance().GetWord(0x842);
            UIEventListener.Get(this._UpdateBtn).onClick = new UIEventListener.VoidDelegate(this.OnClickClearTimeBtn);
            Item item2 = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Arena_Reset);
            if (item2 != null)
            {
                component.text = "X" + item2.num;
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(item2.entry);
                if (_config2 != null)
                {
                    sprite.spriteName = _config2.icon;
                }
            }
            else
            {
                component.text = 50 + string.Empty;
                sprite.spriteName = "Item_Icon_Stone";
            }
            transform.gameObject.SetActive(true);
        }
        else
        {
            label2.text = ConfigMgr.getInstance().GetWord(0x844);
            UIEventListener.Get(this._UpdateBtn).onClick = new UIEventListener.VoidDelegate(this.EnemyRefresh);
            sprite.spriteName = "Item_Icon_Stone";
            transform.gameObject.SetActive(false);
        }
    }

    public void UpdateDate(S2C_ArenaLadderInfo res)
    {
        this.SetMyRank(res.selfOrder);
        this.mMyRank = res.selfOrder;
        this.mMyTopRank = res.bestOrder;
        this.mbuyAttackCount = res.buyAttackCount;
        this.mRemainAttackCount = 5 - res.attackCount;
        if (this.mRemainAttackCount < 1)
        {
            this.mRemainAttackCount = 0;
        }
        this.SetAttackCountInfo(this.mRemainAttackCount);
        this.mEndTime = res.remainAttackTime;
        if (this.mEndTime > TimeMgr.Instance.ServerStampTime)
        {
            this.mIsStart = true;
        }
        this.SetUpdateBtnStat();
        base.StartCoroutine(this.InitTargetPlayer(res.enemys));
    }

    public void UpdateLogNewTips()
    {
        base.transform.FindChild("HistroyBtn/NewTips").gameObject.SetActive(ActorData.getInstance().HaveNewArenaLog);
    }

    public void UpdateTeamInfo()
    {
        if (ActorData.getInstance().ArenaFormation != null)
        {
            Debug.Log(ActorData.getInstance().ArenaFormation.card_id.Count);
            List<Card> cardList = new List<Card>();
            foreach (long num in ActorData.getInstance().ArenaFormation.card_id)
            {
                if (num != -1L)
                {
                    Card cardByID = ActorData.getInstance().GetCardByID(num);
                    if (cardByID != null)
                    {
                        cardList.Add(cardByID);
                    }
                }
            }
            cardList.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
            int num2 = 0;
            for (int i = 0; i < 5; i++)
            {
                Transform transform = base.transform.FindChild("Team/Pos" + (i + 1));
                if (num2 < cardList.Count)
                {
                    this.SetTeamInfo(transform, cardList[num2]);
                    num2++;
                    transform.gameObject.SetActive(true);
                }
                else
                {
                    transform.gameObject.SetActive(false);
                }
            }
            this.SetTeamPower(cardList);
        }
    }

    [CompilerGenerated]
    private sealed class <InitTargetPlayer>c__Iterator51 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<ArenaLadderEnemy> _enemyList;
        internal List<ArenaLadderEnemy> <$>_enemyList;
        internal ArenaLadderPanel <>f__this;
        internal ArenaEnemy <enemy>__3;
        internal int <i>__1;
        internal int <idx>__0;
        internal Transform <target>__2;

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
                    this.<idx>__0 = 0;
                    this.<i>__1 = 0;
                    break;

                case 1:
                    this.<i>__1++;
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_013E;

                default:
                    goto Label_013E;
            }
            if (this.<i>__1 < 3)
            {
                this.<target>__2 = this.<>f__this.transform.FindChild("Target/" + (this.<i>__1 + 1));
                this.<enemy>__3 = this.<target>__2.GetComponent<ArenaEnemy>();
                if (this.<idx>__0 < this._enemyList.Count)
                {
                    this.<target>__2.gameObject.SetActive(true);
                    this.<enemy>__3.Invalidate(this._enemyList[this.<idx>__0], new Action<GameObject>(this.<>f__this.OnClickPkBtn));
                    this.<idx>__0++;
                }
                else
                {
                    this.<target>__2.gameObject.SetActive(false);
                }
                this.$current = new WaitForSeconds(0.01f);
                this.$PC = 1;
            }
            else
            {
                this.$current = null;
                this.$PC = 2;
            }
            return true;
        Label_013E:
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
    private sealed class <OnClickBuyCount>c__AnonStorey16E
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        internal int cost;

        internal void <>m__181(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (ActorData.getInstance().Stone < this.cost)
            {
                string str = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0]);
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                }
                box.SetDialog(str, <>f__am$cache1, null, false);
            }
            else
            {
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 =  => SocketMgr.Instance.RequesArenaLadderBuyAttack(false);
                }
                box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x847), this.cost), <>f__am$cache2, null, false);
            }
        }

        private static void <>m__184(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }

        private static void <>m__185(GameObject)
        {
            SocketMgr.Instance.RequesArenaLadderBuyAttack(false);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickPkBtn>c__AnonStorey16D
    {
        internal ArenaLadderEnemy info;

        internal void <>m__17B(GUIEntity obj)
        {
            SelectHeroPanel panel = (SelectHeroPanel) obj;
            panel.Depth = 600;
            panel.mBattleType = BattleType.ArenaLadder;
            panel.SetArenaladderInfo(this.info);
        }
    }
}

