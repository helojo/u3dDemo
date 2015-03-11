using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class PaokuPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Predicate<guildrun_config> <>f__am$cache10;
    [CompilerGenerated]
    private static Action<object> <>f__am$cacheD;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheE;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheF;
    public bool mIsSendParkourStart;
    protected UITableManager<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>> TableMapGrid = new UITableManager<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>>();

    public override void Initialize()
    {
        base.Initialize();
        this.bt_rule.OnUIMouseClick(u => this.ReqRuleInfo());
        this.bt_open.OnUIMouseClick(u => this.OpenNewPass());
        if (<>f__am$cacheD == null)
        {
            <>f__am$cacheD = delegate (object u) {
                ActorData.getInstance().isLeavePaoku = false;
                GUIMgr.Instance.PopGUIEntity();
            };
        }
        this.Close.OnUIMouseClick(<>f__am$cacheD);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.MapGrid = base.FindChild<UIGrid>("MapGrid");
        this.TopLabel = base.FindChild<UILabel>("TopLabel");
        this.Close = base.FindChild<UIButton>("Close");
        this.bt_rule = base.FindChild<UIButton>("bt_rule");
        this.pk_level = base.FindChild<Transform>("pk_level");
        this.LabelLevel = base.FindChild<UILabel>("LabelLevel");
        this.pk_count = base.FindChild<Transform>("pk_count");
        this.countLabel = base.FindChild<UILabel>("countLabel");
        this.gxLabel = base.FindChild<UILabel>("gxLabel");
        this.getCountLabel = base.FindChild<UILabel>("getCountLabel");
        this.bt_open = base.FindChild<UIButton>("bt_open");
        this.TableMapGrid.InitFromGrid(this.MapGrid);
    }

    public void InitUIData()
    {
        this.LabelLevel.text = (ActorData.getInstance().mGuildData.tech.parkour_level + 1).ToString();
        object[] objArray1 = new object[] { ActorData.getInstance().mUserGuildMemberData.parkour_times.ToString(), "/", GameConstValues.MAX_GUILD_PARKOUR_TIMES, "次" };
        this.countLabel.text = string.Concat(objArray1);
        int num = GameConstValues.MIN_GUILD_PARKOUR_TIMES + (ActorData.getInstance().mUserGuildMemberData.contribution_of_day / GameConstValues.GUILD_PARKOUR_TIMES_CONTRIBUTION);
        int num2 = (num >= GameConstValues.MAX_GUILD_PARKOUR_TIMES) ? GameConstValues.MAX_GUILD_PARKOUR_TIMES : num;
        object[] objArray2 = new object[] { num2, "/", GameConstValues.MAX_GUILD_PARKOUR_TIMES, "次" };
        this.getCountLabel.text = string.Concat(objArray2);
        this.gxLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0x186cb), GameConstValues.GUILD_PARKOUR_TIMES_CONTRIBUTION);
        if (ActorData.getInstance().mUserGuildMemberData.position == 1)
        {
            this.bt_open.Disable(false);
        }
        else
        {
            this.bt_open.Disable(true);
        }
        List<guildrun_config> list = new List<guildrun_config>();
        parkour_config _config = ConfigMgr.getInstance().getByEntry<parkour_config>((int) ActorData.getInstance().mGuildData.tech.parkour_level);
        if (_config != null)
        {
            foreach (string str in StrParser.ParseStringList(_config.map_entry, "|"))
            {
                guildrun_config _config2 = ConfigMgr.getInstance().getByEntry<guildrun_config>(StrParser.ParseDecInt(str, -1));
                if (_config2 != null)
                {
                    list.Add(_config2);
                }
            }
            if (<>f__am$cache10 == null)
            {
                <>f__am$cache10 = p => p.entry <= ActorData.getInstance().mGuildData.tech.parkour_level;
            }
            List<guildrun_config> list3 = list.FindAll(<>f__am$cache10);
            this.TableMapGrid.Cache = false;
            this.TableMapGrid.Count = list3.Count;
            int num3 = 0;
            foreach (guildrun_config _config3 in list3)
            {
                guildrun_config _config4 = _config3;
                UIAutoGenItem<MapGridItemTemplate, MapGridItemModel> item = this.TableMapGrid[num3];
                item.Model.Config = _config4;
                num3++;
                item.Model.OnClick = new Action<MapGridItemModel>(this.OnItemClick);
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
        this.mIsSendParkourStart = false;
        this.InitUIData();
    }

    private void OnItemClick(MapGridItemModel model)
    {
        if ((model.Config != null) && !this.mIsSendParkourStart)
        {
            this.mIsSendParkourStart = true;
            SocketMgr.Instance.RequestParkourStart(model.Config.entry, 0);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    private void OpenNewPass()
    {
        if (<>f__am$cacheF == null)
        {
            <>f__am$cacheF = delegate (GUIEntity guiE) {
                GuildLevUpPanel panel = guiE.Achieve<GuildLevUpPanel>();
                panel.Depth = 400;
                panel.UpdateData(GuildFuncType.PaoKu);
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildLevUpPanel", <>f__am$cacheF, null);
    }

    private void ReqRuleInfo()
    {
        if (<>f__am$cacheE == null)
        {
            <>f__am$cacheE = obj => ((WorldCupRulePanel) obj).SetPaokuRule();
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cacheE, null);
    }

    protected UIButton bt_open { get; set; }

    protected UIButton bt_rule { get; set; }

    protected UIButton Close { get; set; }

    protected UILabel countLabel { get; set; }

    protected UILabel getCountLabel { get; set; }

    protected UILabel gxLabel { get; set; }

    protected UILabel LabelLevel { get; set; }

    protected UIGrid MapGrid { get; set; }

    protected Transform pk_count { get; set; }

    protected Transform pk_level { get; set; }

    protected UILabel TopLabel { get; set; }

    public class MapGridItemModel : TableItemModel<PaokuPanel.MapGridItemTemplate>
    {
        private guildrun_config _Config;
        public Action<PaokuPanel.MapGridItemModel> OnClick;

        public override void Init(PaokuPanel.MapGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            base.Template.bt_click.OnUIMouseClick(delegate (object u) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
        }

        public guildrun_config Config
        {
            get
            {
                return this._Config;
            }
            set
            {
                this._Config = value;
                base.Template.Icon1.mainTexture = BundleMgr.Instance.CreateGuildDupIcon(value.icon);
                base.Template.lb_MapName.text = value.desc;
                base.Template.lb_tip.text = string.Format(ConfigMgr.getInstance().GetWord(0x186cc), value.unlock_level);
                base.Template.lb_stageValue.text = value.max_chaptergold_num.ToString();
                if (ActorData.getInstance().UserInfo.level >= value.unlock_level)
                {
                    base.Template.bt_click.Disable(false);
                    base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x186d5));
                }
                else
                {
                    base.Template.bt_click.Disable(true);
                    base.Template.bt_click.Text(ConfigMgr.getInstance().GetWord(0x9ba3d9));
                }
                int num = ActorData.getInstance().mUserGuildMemberData.parkour_star.star[value.entry];
                switch ((num + 1))
                {
                    case 2:
                        base.Template.sp1.gameObject.SetActive(true);
                        break;

                    case 3:
                        base.Template.sp1.gameObject.SetActive(true);
                        base.Template.sp2.gameObject.SetActive(true);
                        break;

                    case 4:
                        base.Template.sp1.gameObject.SetActive(true);
                        base.Template.sp2.gameObject.SetActive(true);
                        base.Template.sp3.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    public class MapGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.Map = base.FindChild<UIDragScrollView>("Map");
            this.lb_MapName = base.FindChild<UILabel>("lb_MapName");
            this.lb_tip = base.FindChild<UILabel>("lb_tip");
            this.Icon1 = base.FindChild<UITexture>("Icon1");
            this.HeroBackground = base.FindChild<UISprite>("HeroBackground");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.bt_click = base.FindChild<UIButton>("bt_click");
            this.lb_stageValue = base.FindChild<UILabel>("lb_stageValue");
            this.sp_star = base.FindChild<Transform>("sp_star");
            this.sp1 = base.FindChild<UISprite>("sp1");
            this.sp2 = base.FindChild<UISprite>("sp2");
            this.sp3 = base.FindChild<UISprite>("sp3");
        }

        public UIButton bt_click { get; private set; }

        public UISprite HeroBackground { get; private set; }

        public UITexture Icon1 { get; private set; }

        public UILabel lb_MapName { get; private set; }

        public UILabel lb_stageValue { get; private set; }

        public UILabel lb_tip { get; private set; }

        public UIDragScrollView Map { get; private set; }

        public UISprite QualityBorder { get; private set; }

        public Transform sp_star { get; private set; }

        public UISprite sp1 { get; private set; }

        public UISprite sp2 { get; private set; }

        public UISprite sp3 { get; private set; }
    }
}

