using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChallengeArenaPanel : GUIEntity
{
    private UIButton _btn_close;
    private UIButton _btn_descript;
    private UIButton _btn_history;
    private UIButton _btn_rank;
    private UIButton _btn_shop;
    private ChallengeArenaTeamFormation _com_formation;
    private UILabel _label_attack_count;
    private UILabel _label_rank;
    private UILabel _label_refresh_cooldown;
    private UILabel _label_refresh_cooldown_title;
    private Transform _tips_status;
    [CompilerGenerated]
    private static Predicate<long> <>f__am$cacheD;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheE;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheF;
    private float cooldown_timer;
    private float cooldown_timer_update_interval = 1f;

    [DebuggerHidden]
    private IEnumerator AsyncRefreshEnemies()
    {
        return new <AsyncRefreshEnemies>c__Iterator53 { <>f__this = this };
    }

    private int BuyCostForVIP()
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        buy_cost_config _config = ConfigMgr.getInstance().getByEntry<buy_cost_config>(challengeData.attack_buy_count);
        if (_config == null)
        {
            return 0;
        }
        return _config.buy_arenaladder_attack_cost_stone;
    }

    private int BuyCountForVIP()
    {
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config == null)
        {
            return 0;
        }
        return _config.arenaladder_buyattack_times;
    }

    private void ForceRefreshCoolDown()
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        if (challengeData.remain_attack_time > TimeMgr.Instance.ServerStampTime)
        {
            string str = TimeMgr.Instance.GetRemainTime2(challengeData.remain_attack_time);
            this.RefreshCoolDonwLabel.text = string.Format(this.GetWordFromConfig(0x841), str);
            if (!this.RefreshCoolDonwLabel.gameObject.activeSelf)
            {
                this.RefreshCoolDonwLabel.gameObject.SetActive(true);
                this.RefreshCoolDownTitleLabel.gameObject.SetActive(true);
            }
        }
        else
        {
            this.RefreshCoolDonwLabel.gameObject.SetActive(false);
            this.RefreshCoolDownTitleLabel.gameObject.SetActive(false);
            this.UpdateCostStatus();
        }
        DateTime serverDateTime = TimeMgr.Instance.ServerDateTime;
        if (((GameConstValues.FRESH_USERDATA_TIME == serverDateTime.Hour) && (serverDateTime.Minute == 0)) && (serverDateTime.Second == 1))
        {
            SocketMgr.Instance.RequestChallengeArenaInfo();
        }
        if (((serverDateTime.Hour == 20) && (serverDateTime.Minute == 0)) && (serverDateTime.Second == 1))
        {
            SocketMgr.Instance.RequestGetMailList();
            TimeMgr.Instance.ArenaLadderRewardTime = TimeMgr.Instance.ServerDateTime.Date.AddDays(1.0).AddHours(20.0);
        }
    }

    private string GetWordFromConfig(int id)
    {
        return ConfigMgr.getInstance().GetWord(id);
    }

    public void Invalidate()
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        this.RankLabel.text = challengeData.rank.ToString();
        this.AttackCountLabel.text = challengeData.remain_attack_count.ToString() + "/" + 5.ToString();
        this.RefreshTipsObjectStatus();
        this.ForceRefreshCoolDown();
        this.UpdateCostStatus();
        base.StartCoroutine(this.AsyncRefreshEnemies());
    }

    private void OnClickBuyCountButton(GameObject go)
    {
        <OnClickBuyCountButton>c__AnonStorey170 storey = new <OnClickBuyCountButton>c__AnonStorey170 {
            <>f__this = this,
            cost = this.BuyCostForVIP()
        };
        GUIMgr.Instance.DoModelGUI<MessageBox>(new Action<GUIEntity>(storey.<>m__190), null);
    }

    private void OnClickCloseButton(GameObject go)
    {
        for (int i = 0; i != 3; i++)
        {
            int num2 = i + 1;
            Transform transform = base.transform.FindChild("Target/" + num2.ToString());
            if (null != transform)
            {
                transform.gameObject.SetActive(false);
            }
        }
        GUIMgr.Instance.PopGUIEntity();
    }

    private void OnClickDescriptButton(GameObject go)
    {
        if (<>f__am$cacheE == null)
        {
            <>f__am$cacheE = delegate (GUIEntity obj) {
                WorldCupRulePanel panel = (WorldCupRulePanel) obj;
                panel.Depth = 800;
                ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
                panel.ShowLoLArenaLadderRule(challengeData.rank, challengeData.top_rank);
            };
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cacheE, null);
    }

    private void OnClickFightButton(GameObject go)
    {
        BattleFormation challengeArenaFormation = ActorData.getInstance().ChallengeArenaFormation;
        if (challengeArenaFormation != null)
        {
            if (<>f__am$cacheD == null)
            {
                <>f__am$cacheD = e => e >= 0L;
            }
            if (challengeArenaFormation.card_id.FindAll(<>f__am$cacheD).Count < 8)
            {
                TipsDiag.SetText(this.GetWordFromConfig(0x5dc));
            }
            else
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                if (this.TryEnterChallegeFight())
                {
                    ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
                    challengeData.activity_enemy = GUIDataHolder.getData(go) as ArenaLadderEnemy;
                    if (challengeData.activity_enemy != null)
                    {
                        GUIMgr.Instance.PushGUIEntity<ArenaBanHero>(null);
                    }
                }
            }
        }
    }

    private void OnClickHistoryButton(GameObject go)
    {
        GUIMgr.Instance.DoModelGUI<LeagueHistoryPanel>(delegate (GUIEntity e) {
            LeagueHistoryPanel panel = e as LeagueHistoryPanel;
            this.RefreshTipsObjectStatus();
            ActorData.getInstance().HaveNewArenaChallengeLog = false;
            SocketMgr.Instance.RequestCallengeHistory();
        }, null);
    }

    private void OnClickRankButton(GameObject go)
    {
        AllRankInOnePanel.Open(EN_RankListType.EN_RANKLIST_LOL, false);
    }

    private void OnClickRefreshAttackCooldownButton(GameObject go)
    {
        <OnClickRefreshAttackCooldownButton>c__AnonStorey171 storey = new <OnClickRefreshAttackCooldownButton>c__AnonStorey171 {
            <>f__this = this,
            cost = 50
        };
        GUIMgr.Instance.DoModelGUI<MessageBox>(new Action<GUIEntity>(storey.<>m__191), null);
    }

    private void OnClickRefreshImmdiately(GameObject go)
    {
        SocketMgr.Instance.RequestChallengeArenaInfo();
    }

    private void OnClickShopButton(GameObject go)
    {
        if (<>f__am$cacheF == null)
        {
            <>f__am$cacheF = delegate (GUIEntity e) {
                (e as ShopPanel).SetShopType(ShopCoinType.ArenaChallengeCoin);
                SocketMgr.Instance.RequestChallengeShopInfo();
            };
        }
        GUIMgr.Instance.PushGUIEntity<ShopPanel>(<>f__am$cacheF);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (null != activityGUIEntity)
        {
            activityGUIEntity.OnlyShowFunBtn(true);
        }
        GUIMgr.Instance.FloatTitleBar();
        this.RefreshFormation();
        this.RefreshTipsObjectStatus();
        this.UpdateCostStatus();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(this.CloseButton.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCloseButton);
        UIEventListener.Get(this.DescriptButton.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickDescriptButton);
        UIEventListener.Get(this.RankButton.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRankButton);
        UIEventListener.Get(this.HistoryButton.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickHistoryButton);
        UIEventListener.Get(this.ShopButton.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShopButton);
        SocketMgr.Instance.RequestChallengeArenaInfo();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (null != activityGUIEntity)
        {
            activityGUIEntity.OnlyShowFunBtn(false);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.RefreshCoolDown();
        this.FormationGroup.RefreshCoolDown();
    }

    public void RefreshAttackCooldown()
    {
        this.ForceRefreshCoolDown();
    }

    public void RefreshAttackCountInfo()
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        this.AttackCountLabel.text = challengeData.remain_attack_count.ToString() + "/" + 5.ToString();
        this.ForceRefreshCoolDown();
    }

    public void RefreshCoolDown()
    {
        this.cooldown_timer += Time.deltaTime;
        if (this.cooldown_timer > this.cooldown_timer_update_interval)
        {
            this.ForceRefreshCoolDown();
            this.cooldown_timer = 0f;
        }
        this.RefreshTipsObjectStatus();
    }

    public void RefreshFormation()
    {
        this.FormationGroup.Invalidate();
    }

    public void RefreshTipsObjectStatus()
    {
        base.transform.FindChild("HistroyBtn/NewTips").gameObject.SetActive(ActorData.getInstance().HaveNewArenaChallengeLog);
    }

    private void ShowErrorFromConfig(int id)
    {
        TipsDiag.SetText(this.GetWordFromConfig(id));
    }

    private bool TryEnterChallegeFight()
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        if (challengeData.remain_attack_time > TimeMgr.Instance.ServerStampTime)
        {
            string str = TimeMgr.Instance.GetRemainTime2(challengeData.remain_attack_time);
            if (string.IsNullOrEmpty(str))
            {
                this.ShowErrorFromConfig(0x83e);
            }
            else
            {
                TipsDiag.SetText(string.Format(this.GetWordFromConfig(0x841), str) + this.GetWordFromConfig(0x4f9));
            }
            return false;
        }
        if (challengeData.remain_attack_count <= 0)
        {
            this.ShowErrorFromConfig(0x846);
            return false;
        }
        return true;
    }

    public void UpdateCostStatus()
    {
        Transform transform = base.transform.FindChild("BuyCost");
        Transform transform2 = base.transform.FindChild("UpdateBtn");
        UILabel component = transform.FindChild("Label").GetComponent<UILabel>();
        UILabel label2 = transform2.FindChild("Label").GetComponent<UILabel>();
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        bool flag = false;
        int id = 0;
        UIEventListener.VoidDelegate delegate2 = null;
        if ((challengeData.remain_attack_count < 1) && (this.BuyCountForVIP() > 0))
        {
            flag = true;
            id = 0x843;
            delegate2 = new UIEventListener.VoidDelegate(this.OnClickBuyCountButton);
            component.text = this.BuyCostForVIP().ToString();
        }
        else if (challengeData.remain_attack_time > TimeMgr.Instance.ServerStampTime)
        {
            flag = true;
            id = 0x842;
            delegate2 = new UIEventListener.VoidDelegate(this.OnClickRefreshAttackCooldownButton);
            component.text = 50.ToString();
        }
        else
        {
            flag = false;
            id = 0x844;
            delegate2 = new UIEventListener.VoidDelegate(this.OnClickRefreshImmdiately);
        }
        transform.gameObject.SetActive(flag);
        label2.text = this.GetWordFromConfig(id);
        UIEventListener.Get(transform2.gameObject).onClick = delegate2;
    }

    private UILabel AttackCountLabel
    {
        get
        {
            if (null == this._label_attack_count)
            {
                this._label_attack_count = base.transform.FindChild("Info/Count").GetComponent<UILabel>();
            }
            return this._label_attack_count;
        }
    }

    private UIButton CloseButton
    {
        get
        {
            if (null == this._btn_close)
            {
                this._btn_close = base.transform.FindChild("TopLeft/Close").GetComponent<UIButton>();
            }
            return this._btn_close;
        }
    }

    private UIButton DescriptButton
    {
        get
        {
            if (null == this._btn_descript)
            {
                this._btn_descript = base.transform.FindChild("ShuoMingBtn").GetComponent<UIButton>();
            }
            return this._btn_descript;
        }
    }

    private ChallengeArenaTeamFormation FormationGroup
    {
        get
        {
            if (null == this._com_formation)
            {
                this._com_formation = base.transform.FindChild("Formation").GetComponent<ChallengeArenaTeamFormation>();
            }
            return this._com_formation;
        }
    }

    private UIButton HistoryButton
    {
        get
        {
            if (null == this._btn_history)
            {
                this._btn_history = base.transform.FindChild("HistroyBtn").GetComponent<UIButton>();
            }
            return this._btn_history;
        }
    }

    private UIButton RankButton
    {
        get
        {
            if (null == this._btn_rank)
            {
                this._btn_rank = base.transform.FindChild("RankingBtn").GetComponent<UIButton>();
            }
            return this._btn_rank;
        }
    }

    private UILabel RankLabel
    {
        get
        {
            if (null == this._label_rank)
            {
                this._label_rank = base.transform.FindChild("Info/MyRank").GetComponent<UILabel>();
            }
            return this._label_rank;
        }
    }

    private UILabel RefreshCoolDonwLabel
    {
        get
        {
            if (null == this._label_refresh_cooldown)
            {
                this._label_refresh_cooldown = base.transform.FindChild("Info/Time").GetComponent<UILabel>();
            }
            return this._label_refresh_cooldown;
        }
    }

    private UILabel RefreshCoolDownTitleLabel
    {
        get
        {
            if (null == this._label_refresh_cooldown_title)
            {
                this._label_refresh_cooldown_title = base.transform.FindChild("Info/Time/Lb").GetComponent<UILabel>();
            }
            return this._label_refresh_cooldown_title;
        }
    }

    private UIButton ShopButton
    {
        get
        {
            if (null == this._btn_shop)
            {
                this._btn_shop = base.transform.FindChild("ShopBtn").GetComponent<UIButton>();
            }
            return this._btn_shop;
        }
    }

    private Transform TipsStatus
    {
        get
        {
            if (null == this._tips_status)
            {
                this._tips_status = base.transform.FindChild("HistroyBtn/NewTips");
            }
            return this._tips_status;
        }
    }

    [CompilerGenerated]
    private sealed class <AsyncRefreshEnemies>c__Iterator53 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ChallengeArenaPanel <>f__this;
        internal ChallengeArenaData <data>__0;
        internal ArenaEnemy <enemy>__3;
        internal int <i>__1;
        internal Transform <tns>__2;

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
                    this.<data>__0 = ActorData.getInstance().ChallengeData;
                    this.<i>__1 = 0;
                    break;

                case 1:
                    this.<i>__1++;
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_0146;

                default:
                    goto Label_0146;
            }
            if (this.<i>__1 != 3)
            {
                int num2 = this.<i>__1 + 1;
                this.<tns>__2 = this.<>f__this.transform.FindChild("Target/" + num2.ToString());
                this.<enemy>__3 = this.<tns>__2.GetComponent<ArenaEnemy>();
                if (this.<i>__1 >= this.<data>__0.enemies.Count)
                {
                    this.<tns>__2.gameObject.SetActive(false);
                }
                else
                {
                    this.<tns>__2.gameObject.SetActive(true);
                    this.<enemy>__3.Invalidate(this.<data>__0.enemies[this.<i>__1], new Action<GameObject>(this.<>f__this.OnClickFightButton));
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
        Label_0146:
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
    private sealed class <OnClickBuyCountButton>c__AnonStorey170
    {
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        private static UIEventListener.VoidDelegate <>f__am$cache3;
        internal ChallengeArenaPanel <>f__this;
        internal int cost;

        internal void <>m__190(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (ActorData.getInstance().Stone < this.cost)
            {
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = diag_btn => GUIMgr.Instance.PushGUIEntity<VipCardPanel>(null);
                }
                box.SetDialog(this.<>f__this.GetWordFromConfig(0x9d2abe), <>f__am$cache2, null, false);
            }
            else
            {
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = diag_btn => SocketMgr.Instance.RequestChallengeArenaBuyAttack();
                }
                box.SetDialog(string.Format(this.<>f__this.GetWordFromConfig(0x847), this.cost), <>f__am$cache3, null, false);
            }
        }

        private static void <>m__195(GameObject diag_btn)
        {
            GUIMgr.Instance.PushGUIEntity<VipCardPanel>(null);
        }

        private static void <>m__196(GameObject diag_btn)
        {
            SocketMgr.Instance.RequestChallengeArenaBuyAttack();
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickRefreshAttackCooldownButton>c__AnonStorey171
    {
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        private static UIEventListener.VoidDelegate <>f__am$cache3;
        internal ChallengeArenaPanel <>f__this;
        internal int cost;

        internal void <>m__191(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (ActorData.getInstance().Stone < this.cost)
            {
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = diag_btn => GUIMgr.Instance.PushGUIEntity<VipCardPanel>(null);
                }
                box.SetDialog(this.<>f__this.GetWordFromConfig(0x9d2abe), <>f__am$cache2, null, false);
            }
            else
            {
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = diag_btn => SocketMgr.Instance.RequestChallengeArenaResetAttackCooldown();
                }
                box.SetDialog(string.Format(this.<>f__this.GetWordFromConfig(0x845), this.cost), <>f__am$cache3, null, false);
            }
        }

        private static void <>m__197(GameObject diag_btn)
        {
            GUIMgr.Instance.PushGUIEntity<VipCardPanel>(null);
        }

        private static void <>m__198(GameObject diag_btn)
        {
            SocketMgr.Instance.RequestChallengeArenaResetAttackCooldown();
        }
    }
}

