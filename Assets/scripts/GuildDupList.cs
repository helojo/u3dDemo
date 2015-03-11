using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class GuildDupList : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache6;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache7;
    protected UITableManager<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>> TableMapGrid = new UITableManager<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>>();

    public override void Initialize()
    {
        base.Initialize();
        if (<>f__am$cache6 == null)
        {
            <>f__am$cache6 = u => GUIMgr.Instance.PopGUIEntity();
        }
        this.Close.OnUIMouseClick(<>f__am$cache6);
        this.bt_assign_log.OnUIMouseClick(u => this.ReqItemHandOutRecord());
        this.bt_rule.OnUIMouseClick(u => this.ReqRuleInfo());
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.MapGrid = base.FindChild<UIGrid>("MapGrid");
        this.TopLabel = base.FindChild<UILabel>("TopLabel");
        this.Close = base.FindChild<UIButton>("Close");
        this.bt_rule = base.FindChild<UIButton>("bt_rule");
        this.bt_assign_log = base.FindChild<UIButton>("bt_assign_log");
        this.TableMapGrid.InitFromGrid(this.MapGrid);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
        this.ShowTable();
    }

    private void OnItemClick(MapGridItemModel model)
    {
        if (model.StateInfo != null)
        {
            switch ((model.StateInfo.status + 1))
            {
                case GuildDupStatusEnum.GuildDupStatus_Open:
                    if (!XSingleton<GameGuildMgr>.Singleton.IsLeader)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c73));
                        break;
                    }
                    SocketMgr.Instance.RequestC2S_OpenGuildDup(model.Config.entry);
                    return;

                case GuildDupStatusEnum.GuildDupStatus_Pass:
                case ((GuildDupStatusEnum) 4):
                    SocketMgr.Instance.RequestC2S_GetGuildDupTrenchInfo(model.Config.entry);
                    this.ShowTrenchMap(model);
                    break;
            }
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    private void ReqItemHandOutRecord()
    {
        SocketMgr.Instance.RequestC2S_GuildDupDistributeHistory();
    }

    private void ReqRuleInfo()
    {
        GuildBattleNewRulePanel panel = GUIMgr.Instance.GetGUIEntity<GuildBattleNewRulePanel>();
        if (panel != null)
        {
            panel.GetRuleInfo();
        }
        else
        {
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = delegate (GUIEntity u) {
                    GuildBattleNewRulePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildBattleNewRulePanel>();
                    if (gUIEntity != null)
                    {
                        gUIEntity.GetRuleInfo();
                    }
                };
            }
            GUIMgr.Instance.DoModelGUI<GuildBattleNewRulePanel>(<>f__am$cache7, null);
        }
    }

    internal void ShowGuildDup(List<GuildDupStatusInfo> list)
    {
        List<guilddup_config> list2 = ConfigMgr.getInstance().getListResult<guilddup_config>();
        this.TableMapGrid.Cache = false;
        this.TableMapGrid.Count = list2.Count;
        int num = 0;
        foreach (guilddup_config _config in list2)
        {
            guilddup_config _config2 = _config;
            UIAutoGenItem<MapGridItemTemplate, MapGridItemModel> item = this.TableMapGrid[num];
            item.Model.Config = _config2;
            num++;
            item.Model.OnClick = new Action<MapGridItemModel>(this.OnItemClick);
        }
        IEnumerator<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>> enumerator = this.TableMapGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<MapGridItemTemplate, MapGridItemModel> current = enumerator.Current;
                foreach (GuildDupStatusInfo info in list)
                {
                    if (current.Model.Config.entry == info.guildDupEntry)
                    {
                        current.Model.StateInfo = info;
                        continue;
                    }
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

    public void ShowTable()
    {
        if (XSingleton<GameGuildMgr>.Singleton.DupStatus != null)
        {
            this.ShowGuildDup(XSingleton<GameGuildMgr>.Singleton.DupStatus);
        }
        SocketMgr.Instance.RequestC2S_GetGuildDupInfo();
    }

    public void ShowTrenchMap(MapGridItemModel model)
    {
        <ShowTrenchMap>c__AnonStorey20C storeyc = new <ShowTrenchMap>c__AnonStorey20C {
            model = model
        };
        GUIMgr.Instance.PushGUIEntity<GuildDupTrenchMap>(new Action<GUIEntity>(storeyc.<>m__3B7));
    }

    protected UIButton bt_assign_log { get; set; }

    protected UIButton bt_rule { get; set; }

    protected UIButton Close { get; set; }

    protected UIGrid MapGrid { get; set; }

    protected UILabel TopLabel { get; set; }

    [CompilerGenerated]
    private sealed class <ShowTrenchMap>c__AnonStorey20C
    {
        internal GuildDupList.MapGridItemModel model;

        internal void <>m__3B7(GUIEntity u)
        {
            (u as GuildDupTrenchMap).ShowTrenchMap(this.model.Config, this.model.StateInfo);
        }
    }

    public class MapGridItemModel : TableItemModel<GuildDupList.MapGridItemTemplate>
    {
        private guilddup_config _Config;
        private GuildDupStatusInfo _StateInfo;
        public Action<GuildDupList.MapGridItemModel> OnClick;

        public string FormatTimeString(int time)
        {
            TimeSpan span = TimeSpan.FromSeconds((double) time);
            return (((span.TotalDays < 1.0) ? string.Empty : string.Format(ConfigMgr.getInstance().GetWord(0x2c6b), Mathf.FloorToInt((float) span.TotalDays))) + ((span.Hours <= 0) ? string.Empty : string.Format(ConfigMgr.getInstance().GetWord(0x2c6c), span.Hours)));
        }

        private string GetTimeLimit()
        {
            GuildDupStatusInfo info = this._StateInfo;
            return this.FormatTimeString(((int) info.end_time) - TimeMgr.Instance.ServerStampTime);
        }

        public override void Init(GuildDupList.MapGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            base.Template.bt_click.OnUIMouseClick(delegate (object u) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
            base.Template.bt_click.Disable(true);
            base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x2c76));
        }

        public guilddup_config Config
        {
            get
            {
                return this._Config;
            }
            set
            {
                this._Config = value;
                base.Template.lb_tip.enabled = false;
                base.Template.lb_MapName.text = value.name;
                base.Template.lb_destription.text = value.description;
                base.Template.bt_click.Disable(true);
                base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x2c72));
                base.Template.Icon1.mainTexture = BundleMgr.Instance.CreateGuildDupIcon(value.icon);
            }
        }

        public GuildDupStatusInfo StateInfo
        {
            get
            {
                return this._StateInfo;
            }
            set
            {
                this._StateInfo = value;
                string timeLimit = this.GetTimeLimit();
                base.Template.lb_destription.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c74), timeLimit);
                base.Template.lb_destription.ActiveSelfObject(!string.IsNullOrEmpty(timeLimit));
                base.Template.lb_tip.enabled = false;
                double num = 0.0;
                if (value.total_hp_amount > 0L)
                {
                    num = ((double) value.damage_amount) / ((double) value.total_hp_amount);
                }
                base.Template.lb_stageValue.text = string.Format("{0:0}/{1}", num * 100.0, 100);
                switch ((value.status + 1))
                {
                    case GuildDupStatusEnum.GuildDupStatus_Close:
                        base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x2c77));
                        base.Template.bt_click.Disable(false);
                        return;

                    case GuildDupStatusEnum.GuildDupStatus_Can_Open:
                        base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x2c76));
                        base.Template.bt_click.Disable(true);
                        return;

                    case GuildDupStatusEnum.GuildDupStatus_Open:
                        base.Template.lb_tip.enabled = true;
                        base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x2c75));
                        base.Template.bt_click.Disable(false);
                        return;

                    case GuildDupStatusEnum.GuildDupStatus_Pass:
                        base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x2c78));
                        base.Template.bt_click.Disable(false);
                        return;

                    case ((GuildDupStatusEnum) 4):
                    {
                        base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x2c79));
                        uint num2 = value.finish_time - value.start_time;
                        if (num2 <= (this.Config.time_limit * 60))
                        {
                            base.Template.lb_destription.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c8c), this.FormatTimeString((int) num2), this.Config.guildcon_get_timelimit);
                            break;
                        }
                        base.Template.lb_destription.text = string.Empty;
                        break;
                    }
                    default:
                        return;
                }
                base.Template.bt_click.Disable(false);
            }
        }
    }

    public class MapGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.lb_MapName = base.FindChild<UILabel>("lb_MapName");
            this.lb_tip = base.FindChild<UILabel>("lb_tip");
            this.lb_destription = base.FindChild<UILabel>("lb_destription");
            this.Grid = base.FindChild<UIGrid>("Grid");
            this.Icon1 = base.FindChild<UITexture>("Icon1");
            this.HeroBackground = base.FindChild<UISprite>("HeroBackground");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.bt_click = base.FindChild<UIButton>("bt_click");
            this.lb_stage = base.FindChild<UILabel>("lb_stage");
            this.lb_stageValue = base.FindChild<UILabel>("lb_stageValue");
        }

        public UIButton bt_click { get; private set; }

        public UIGrid Grid { get; private set; }

        public UISprite HeroBackground { get; private set; }

        public UITexture Icon1 { get; private set; }

        public UILabel lb_destription { get; private set; }

        public UILabel lb_MapName { get; private set; }

        public UILabel lb_stage { get; private set; }

        public UILabel lb_stageValue { get; private set; }

        public UILabel lb_tip { get; private set; }

        public UISprite QualityBorder { get; private set; }
    }
}

