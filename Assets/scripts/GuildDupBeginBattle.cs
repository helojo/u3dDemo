using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class GuildDupBeginBattle : GUIPanelEntity
{
    private ButtonState _State;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache29;
    [CompilerGenerated]
    private static System.Action <>f__am$cache2A;
    private int bossId__;
    private string[] btSpriteNames = new string[] { "Ui_Battle_Btn_playup", "Ui_Battle_Btn_gh" };
    private int curBuyBuffCnt;
    private guilddup_config dup;
    private int dupId__;
    private guilddup_trench_config dupTrench;
    private GuildDupTrenchInfo Information;
    private float lastTime;
    private const int MaxItemCount = 5;
    private List<monster_config> Monster;
    private float NextTryLock;
    private int supportCnt;
    protected UITableManager<UIAutoGenItem<BossGridItemTemplate, BossGridItemModel>> TableBossGrid = new UITableManager<UIAutoGenItem<BossGridItemTemplate, BossGridItemModel>>();
    protected UITableManager<UIAutoGenItem<DonetGridItemTemplate, DonetGridItemModel>> TableDonetGrid = new UITableManager<UIAutoGenItem<DonetGridItemTemplate, DonetGridItemModel>>();
    protected UITableManager<UIAutoGenItem<ItemGridItemTemplate, ItemGridItemModel>> TableItemGrid = new UITableManager<UIAutoGenItem<ItemGridItemTemplate, ItemGridItemModel>>();

    internal void BeginBattle(guilddup_config guilddup_config, guilddup_trench_config guilddup_trench_config, GuildDupTrenchInfo info)
    {
        this.bt_start.Disable(true);
        this.lb_timeLimit.text = string.Empty;
        this.dup = guilddup_config;
        this.dupTrench = guilddup_trench_config;
        this.Information = info;
        this.lb_bossTitle.text = guilddup_trench_config.name;
        if (this.Information.breakLockTime > TimeMgr.Instance.ServerStampTime)
        {
            if (this.Information.lockUserId == ActorData.getInstance().SessionInfo.userid)
            {
                XSingleton<GameGuildMgr>.Singleton.Lock(this.dup.entry, this.dupTrench.entry, (int) this.Information.breakLockTime);
                this.State = ButtonState.Start;
            }
            else
            {
                this.State = ButtonState.Lock;
            }
        }
        else if (XSingleton<GameGuildMgr>.Singleton.RestrictEndTime > TimeMgr.Instance.ServerStampTime)
        {
            this.State = ButtonState.Restrict;
        }
        else if (!XSingleton<GameGuildMgr>.Singleton.IsLocking(this.dup.entry, this.dupTrench.entry))
        {
            this.State = ButtonState.TryLock;
            this.NextTryLock = Time.time + 5f;
            SocketMgr.Instance.RequestC2S_TryLockGuildDup(this.dup.entry, this.dupTrench.entry);
        }
        else
        {
            this.State = ButtonState.Start;
        }
        this.Monster = XSingleton<GameGuildMgr>.Singleton.GetMonster(guilddup_trench_config.entry);
        string timeLimit = this.GetTimeLimit();
        this.lb_dupStageDescription.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c6d), timeLimit);
        this.lb_dupStageDescription.ActiveSelfObject(!string.IsNullOrEmpty(timeLimit));
        this.TableBossGrid.Count = this.Monster.Count;
        int num = 0;
        IEnumerator<UIAutoGenItem<BossGridItemTemplate, BossGridItemModel>> enumerator = this.TableBossGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<BossGridItemTemplate, BossGridItemModel> current = enumerator.Current;
                current.Model.Config = this.Monster[num];
                num++;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        this.s_rate.value = (float) (((double) info.damage_amount) / ((double) info.total_hp_amount));
        this.lb_dupStageRate.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c6f), this.s_rate.value * 100f);
        this.lb_oneBoss.ActiveSelfObject(this.TableBossGrid.Count == 1);
        this.lb_twoBoss.ActiveSelfObject(this.TableBossGrid.Count >= 2);
        this.lb_bufferAdd.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c6e), this.Information.buffCount);
        string desc = this.dupTrench.desc;
        this.lb_oneBoss.text = desc;
        this.lb_twoBoss.text = desc;
        int[] numArray = guilddup_trench_config.dorp_desc_entry.SplitToInt('|');
        this.TableItemGrid.Count = Mathf.Max(5, numArray.Length);
        num = 0;
        IEnumerator<UIAutoGenItem<ItemGridItemTemplate, ItemGridItemModel>> enumerator2 = this.TableItemGrid.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                UIAutoGenItem<ItemGridItemTemplate, ItemGridItemModel> item2 = enumerator2.Current;
                item2.Model.Config = null;
            }
        }
        finally
        {
            if (enumerator2 == null)
            {
            }
            enumerator2.Dispose();
        }
        foreach (int num2 in numArray)
        {
            if (num >= 5)
            {
                break;
            }
            this.TableItemGrid[num].Model.Config = ConfigMgr.getInstance().getByEntry<item_config>(num2);
            num++;
        }
        SocketMgr.Instance.RequestC2S_GuildDupSupportTrenchRank(this.dup.entry, this.dupTrench.entry);
    }

    private bool CanTryLock()
    {
        if (XSingleton<GameGuildMgr>.Singleton.RestrictEndTime > TimeMgr.Instance.ServerStampTime)
        {
            return false;
        }
        return (XSingleton<GameGuildMgr>.Singleton.LimitTime <= 0);
    }

    private void GetDupAndBossInfo()
    {
        if (this.dup != null)
        {
            this.dupId__ = this.dup.entry;
        }
        else
        {
            Debug.LogWarning("ReqKillRank's dupId__ is Error!");
            return;
        }
        if (this.dupTrench != null)
        {
            this.bossId__ = this.dupTrench.entry;
        }
        else
        {
            Debug.LogWarning("ReqKillRank's bossId__ is Error!");
        }
    }

    private string GetTimeLimit()
    {
        TimeSpan span = TimeSpan.FromSeconds((double) (this.Information.end_time - TimeMgr.Instance.ServerStampTime));
        return (((span.TotalDays <= 0.0) ? string.Empty : string.Format(ConfigMgr.getInstance().GetWord(0x2c6b), Mathf.FloorToInt((float) span.TotalDays))) + ((span.Hours <= 0) ? string.Empty : string.Format(ConfigMgr.getInstance().GetWord(0x2c6c), span.Hours)));
    }

    public override void Initialize()
    {
        base.Initialize();
        if (<>f__am$cache29 == null)
        {
            <>f__am$cache29 = delegate (object u) {
                int restrictEndTime = XSingleton<GameGuildMgr>.Singleton.RestrictEndTime;
                XSingleton<GameGuildMgr>.Singleton.ReleaseLock(true);
                if (restrictEndTime < TimeMgr.Instance.ServerStampTime)
                {
                    XSingleton<GameGuildMgr>.Singleton.SetRestrictEndTime(0);
                }
                GUIMgr.Instance.PopGUIEntity();
                GuildDupTrenchMap gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupTrenchMap>();
                if (gUIEntity != null)
                {
                    gUIEntity.ShowLast();
                }
            };
        }
        this.Close.OnUIMouseClick(<>f__am$cache29);
        this.TableItemGrid.Count = 5;
        this.TableBossGrid.Count = 2;
        this.lb_oneBoss.ActiveSelfObject(false);
        this.lb_twoBoss.ActiveSelfObject(false);
        this.TableDonetGrid.Count = 0;
        this.bt_start.OnUIMouseClick(u => GUIMgr.Instance.DoModelGUI<SelectHeroPanel>(delegate (GUIEntity obj) {
            SelectHeroPanel panel = obj as SelectHeroPanel;
            panel.Depth = 600;
            panel.EndTime = (XSingleton<GameGuildMgr>.Singleton.LimitTime - 5) + TimeMgr.Instance.ServerStampTime;
            panel.mBattleType = BattleType.GuildDup;
            panel.SetButtonState(BattleType.GuildDup);
            panel.OnSaved = null;
            panel.OnStartBattle = new Action<List<long>>(this.OnStartBattle);
            obj.StopAllCoroutines();
            if (<>f__am$cache2A == null)
            {
                <>f__am$cache2A = () => GUIMgr.Instance.ExitModelGUI<SelectHeroPanel>();
            }
            obj.DelayCallBack((float) XSingleton<GameGuildMgr>.Singleton.LimitTime, <>f__am$cache2A);
        }, null));
        this.bt_Rank.OnUIMouseClick(u => this.ReqKillRank());
        this.bt_donet.OnUIMouseClick(u => this.ReqRicherHelp());
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.TopLabel = base.FindChild<UILabel>("TopLabel");
        this.Close = base.FindChild<UIButton>("Close");
        this.bt_Rank = base.FindChild<UIButton>("bt_Rank");
        this.rightLayout = base.FindChild<UISprite>("rightLayout");
        this.DonetGrid = base.FindChild<UIGrid>("DonetGrid");
        this.lb_bufferAdd = base.FindChild<UILabel>("lb_bufferAdd");
        this.lb_bossbufferTitle = base.FindChild<UILabel>("lb_bossbufferTitle");
        this.bt_donet = base.FindChild<UIButton>("bt_donet");
        this.lb_bossTitle = base.FindChild<UILabel>("lb_bossTitle");
        this.lb_bossdrop = base.FindChild<UILabel>("lb_bossdrop");
        this.bt_start = base.FindChild<UIButton>("bt_start");
        this.TBackground = base.FindChild<UISprite>("TBackground");
        this.lb_timeLimit = base.FindChild<UILabel>("lb_timeLimit");
        this.BattleHero = base.FindChild<Transform>("BattleHero");
        this.HeroBackground = base.FindChild<UISprite>("HeroBackground");
        this.heroQualityBorder = base.FindChild<UISprite>("heroQualityBorder");
        this.heroIcon1 = base.FindChild<UITexture>("heroIcon1");
        this.lb_batting = base.FindChild<UILabel>("lb_batting");
        this.lb_battleUserName = base.FindChild<UILabel>("lb_battleUserName");
        this.lb_oneBoss = base.FindChild<UILabel>("lb_oneBoss");
        this.lb_twoBoss = base.FindChild<UILabel>("lb_twoBoss");
        this.lb_dupStageDescription = base.FindChild<UILabel>("lb_dupStageDescription");
        this.lb_dupStageRate = base.FindChild<UILabel>("lb_dupStageRate");
        this.s_rate = base.FindChild<UISlider>("s_rate");
        this.BossGrid = base.FindChild<UIGrid>("BossGrid");
        this.ItemGrid = base.FindChild<UIGrid>("ItemGrid");
        this.TableDonetGrid.InitFromGrid(this.DonetGrid);
        this.TableBossGrid.InitFromGrid(this.BossGrid);
        this.TableItemGrid.InitFromGrid(this.ItemGrid);
    }

    internal void LockGuildResult(S2C_TryLockGuildDup result)
    {
        this.State = ButtonState.Start;
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        this.lb_timeLimit.text = string.Empty;
    }

    private void OnStartBattle(List<long> obj)
    {
        SocketMgr.Instance.RequestC2S_GuildDupCombatBegin(this.dup.entry, this.dupTrench.entry, obj);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        switch (this.State)
        {
            case ButtonState.Start:
                if ((this.lastTime + 1f) <= Time.time)
                {
                    this.lastTime = Time.time;
                    int num2 = XSingleton<GameGuildMgr>.Singleton.LimitTime - 5;
                    this.lb_timeLimit.text = string.Empty;
                    if (num2 > 0)
                    {
                        this.lb_timeLimit.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c70), num2);
                    }
                    else
                    {
                        this.lb_timeLimit.text = string.Empty;
                        this.bt_start.Disable(true);
                        this.BattleHero.ActiveSelfObject(false);
                        this.bt_start.Text("已超时");
                        if (XSingleton<GameGuildMgr>.Singleton.LimitTime <= 0)
                        {
                            this.State = ButtonState.Restrict;
                            XSingleton<GameGuildMgr>.Singleton.ReleaseLock(true);
                        }
                    }
                    break;
                }
                return;

            case ButtonState.Lock:
            case ButtonState.TryLock:
                if (this.NextTryLock >= Time.time)
                {
                    break;
                }
                this.NextTryLock = Time.time + 65f;
                if (!this.CanTryLock())
                {
                    break;
                }
                SocketMgr.Instance.RequestC2S_TryLockGuildDup(this.dup.entry, this.dupTrench.entry);
                return;

            case ButtonState.Restrict:
                if ((this.lastTime + 1f) <= Time.time)
                {
                    this.lastTime = Time.time;
                    int num = XSingleton<GameGuildMgr>.Singleton.RestrictEndTime - TimeMgr.Instance.ServerStampTime;
                    if (num < 0)
                    {
                        this.State = ButtonState.TryLock;
                    }
                    else
                    {
                        this.lb_timeLimit.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c70), num);
                    }
                    break;
                }
                return;
        }
    }

    private void ReqKillRank()
    {
        this.GetDupAndBossInfo();
        SocketMgr.Instance.RequestC2S_GuildWholeSvrTrenchDamageRank(this.dupId__, this.bossId__);
    }

    public void ReqRicherHelp()
    {
        this.GetDupAndBossInfo();
        GUIMgr.Instance.DoModelGUI<GuidBattleBossRicherHelpPanel>(delegate (GUIEntity u) {
            GuidBattleBossRicherHelpPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuidBattleBossRicherHelpPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.DupId = this.dupId__;
                gUIEntity.BossId = this.bossId__;
            }
            SocketMgr.Instance.RequestC2S_GuildDupSupportTrenchRank(this.dupId__, this.bossId__);
        }, null);
    }

    internal void SetSupportCout(S2C_GuildDupSupportTrenchRank res)
    {
        this.Information.buffCount = res.supportCount;
        this.lb_bufferAdd.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c6e), this.Information.buffCount);
        this.TableDonetGrid.Count = res.supports.Count;
        int num = 0;
        IEnumerator<UIAutoGenItem<DonetGridItemTemplate, DonetGridItemModel>> enumerator = this.TableDonetGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<DonetGridItemTemplate, DonetGridItemModel> current = enumerator.Current;
                current.Model.Support = res.supports[num];
                num++;
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

    internal void ShowSupportList(List<GuildDupSupportSimpleInfo> list)
    {
        this.TableDonetGrid.Count = list.Count;
        int num = 0;
        IEnumerator<UIAutoGenItem<DonetGridItemTemplate, DonetGridItemModel>> enumerator = this.TableDonetGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<DonetGridItemTemplate, DonetGridItemModel> current = enumerator.Current;
                current.Model.Support = list[num];
                num++;
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

    protected Transform BattleHero { get; set; }

    protected UIGrid BossGrid { get; set; }

    protected UIButton bt_donet { get; set; }

    protected UIButton bt_Rank { get; set; }

    protected UIButton bt_start { get; set; }

    protected UIButton Close { get; set; }

    protected UIGrid DonetGrid { get; set; }

    protected UISprite HeroBackground { get; set; }

    protected UITexture heroIcon1 { get; set; }

    protected UISprite heroQualityBorder { get; set; }

    protected UIGrid ItemGrid { get; set; }

    protected UILabel lb_batting { get; set; }

    protected UILabel lb_battleUserName { get; set; }

    protected UILabel lb_bossbufferTitle { get; set; }

    protected UILabel lb_bossdrop { get; set; }

    protected UILabel lb_bossTitle { get; set; }

    protected UILabel lb_bufferAdd { get; set; }

    protected UILabel lb_dupStageDescription { get; set; }

    protected UILabel lb_dupStageRate { get; set; }

    protected UILabel lb_oneBoss { get; set; }

    protected UILabel lb_timeLimit { get; set; }

    protected UILabel lb_twoBoss { get; set; }

    protected UISprite rightLayout { get; set; }

    protected UISlider s_rate { get; set; }

    public ButtonState State
    {
        get
        {
            return this._State;
        }
        set
        {
            this.lb_timeLimit.text = string.Empty;
            this._State = value;
            this.TBackground.spriteName = this.btSpriteNames[0];
            switch (this._State)
            {
                case ButtonState.Start:
                    this.bt_start.Disable(false);
                    this.BattleHero.gameObject.SetActive(false);
                    this.bt_start.Text("开始战斗");
                    break;

                case ButtonState.Lock:
                {
                    this.BattleHero.gameObject.SetActive(true);
                    this.bt_start.Text(string.Empty);
                    this.bt_start.Disable(true);
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(this.Information.headEntry);
                    if (_config != null)
                    {
                        this.heroIcon1.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    }
                    this.lb_batting.text = "正在战斗中";
                    this.lb_battleUserName.text = this.Information.name;
                    this.TBackground.spriteName = this.btSpriteNames[1];
                    break;
                }
                case ButtonState.Restrict:
                case ButtonState.TryLock:
                    this.bt_start.Disable(true);
                    this.BattleHero.gameObject.SetActive(false);
                    this.bt_start.Text("已超时");
                    break;
            }
        }
    }

    protected UISprite TBackground { get; set; }

    protected UILabel TopLabel { get; set; }

    public class BossGridItemModel : TableItemModel<GuildDupBeginBattle.BossGridItemTemplate>
    {
        private monster_config _config;

        public override void Init(GuildDupBeginBattle.BossGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }

        public monster_config Config
        {
            get
            {
                return this._config;
            }
            set
            {
                this._config = value;
                base.Template.Grid.ActiveSelfObject(false);
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(value.card_entry);
                if (_config != null)
                {
                    CommonFunc.SetQualityBorder(base.Template.QualityBorder, value.quality);
                    base.Template.Icon1.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                }
            }
        }
    }

    public class BossGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.Grid = base.FindChild<UIGrid>("Grid");
            this.Icon1 = base.FindChild<UITexture>("Icon1");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
        }

        public UIGrid Grid { get; private set; }

        public UITexture Icon1 { get; private set; }

        public UISprite QualityBorder { get; private set; }
    }

    public enum ButtonState
    {
        Start,
        Lock,
        Restrict,
        TryLock
    }

    public class DonetGridItemModel : TableItemModel<GuildDupBeginBattle.DonetGridItemTemplate>
    {
        public override void Init(GuildDupBeginBattle.DonetGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }

        private GuildDupSupportSimpleInfo support { get; set; }

        public GuildDupSupportSimpleInfo Support
        {
            get
            {
                return this.support;
            }
            set
            {
                this.support = value;
                base.Template.lb_donetNum.text = string.Format("{0}", this.support.stone);
                base.Template.lb_userName.text = this.support.name;
            }
        }
    }

    public class DonetGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.lb_userName = base.FindChild<UILabel>("lb_userName");
            this.lb_donetNum = base.FindChild<UILabel>("lb_donetNum");
        }

        public UILabel lb_donetNum { get; private set; }

        public UILabel lb_userName { get; private set; }
    }

    public class ItemGridItemModel : TableItemModel<GuildDupBeginBattle.ItemGridItemTemplate>
    {
        private item_config _Config;

        public override void Init(GuildDupBeginBattle.ItemGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            item.Root.OnUIMousePressOver(delegate (object u) {
                if ((this.Config != null) && (GUITipManager.Current != null))
                {
                    GUITipManager.Current.DrawItemTip(this.ID, GUITipManager.Current.OffsetPos(base.Item.Root.transform), new Vector3(150f, 120f, 0f), this._Config.entry);
                }
            });
        }

        public item_config Config
        {
            get
            {
                return this._Config;
            }
            set
            {
                this.ID = string.Format("GUILD_BEGIN_{0}", base.Item.Root.name).GetHashCode();
                this._Config = value;
                if (value != null)
                {
                    base.Template.Emptyframe.gameObject.SetActive(false);
                    base.Template.ItemBgSprite.gameObject.SetActive(true);
                    base.Template.EquipBroder.ActiveSelfObject(true);
                    base.Template.Texture.mainTexture = BundleMgr.Instance.CreateItemIcon(value.icon);
                    CommonFunc.SetEquipQualityBorder(base.Template.EquipBroder, value.quality, false);
                    base.Template.IsPart.ActiveSelfObject(value.type == 2);
                }
                else
                {
                    base.Template.Emptyframe.gameObject.SetActive(true);
                    base.Template.ItemBgSprite.gameObject.SetActive(false);
                    base.Template.Texture.mainTexture = null;
                    base.Template.EquipBroder.ActiveSelfObject(false);
                    base.Template.IsPart.ActiveSelfObject(false);
                }
            }
        }

        private int ID { get; set; }
    }

    public class ItemGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.ItemBgSprite = base.FindChild<UISprite>("ItemBgSprite");
            this.Emptyframe = base.FindChild<UISprite>("Emptyframe");
            this.Texture = base.FindChild<UITexture>("Texture");
            this.EquipBroder = base.FindChild<UISprite>("EquipBroder");
            this.IsPart = base.FindChild<UISprite>("IsPart");
        }

        public UISprite Emptyframe { get; private set; }

        public UISprite EquipBroder { get; private set; }

        public UISprite IsPart { get; private set; }

        public UISprite ItemBgSprite { get; private set; }

        public UITexture Texture { get; private set; }
    }
}

