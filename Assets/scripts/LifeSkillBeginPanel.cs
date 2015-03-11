using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;

internal class LifeSkillBeginPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cacheB;
    [CompilerGenerated]
    private static Func<long, int> <>f__am$cacheC;
    private int cardEntry;
    private NewSkillCellType CellIndex;
    private int LastSchemEntry;
    protected UITableManager<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>> TableMapGrid = new UITableManager<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>>();

    public override void Initialize()
    {
        base.Initialize();
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = u => GUIMgr.Instance.PopGUIEntity();
        }
        this.Close.OnUIMouseClick(<>f__am$cacheB);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.MapGrid = base.FindChild<UIGrid>("MapGrid");
        this.TopLabel = base.FindChild<UILabel>("TopLabel");
        this.Close = base.FindChild<UIButton>("Close");
        this.TableMapGrid.InitFromGrid(this.MapGrid);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
    }

    public void OnItemClick(MapGridItemModel item)
    {
        GUIMgr.Instance.PopGUIEntity();
        this.LastSchemEntry = item.TimeConfig.entry;
        if (this.FriendID > 0L)
        {
            GUIMgr.Instance.PushGUIEntity<LifeSkillSoltPanel>(soltui => (soltui as LifeSkillSoltPanel).ShowCard(this.SkillType, this.cardEntry, this.CellIndex, this.LastSchemEntry, this.FriendID));
        }
        else
        {
            GUIMgr.Instance.DoModelGUI<SelectHeroPanel>(delegate (GUIEntity obj) {
                SelectHeroPanel panel = obj as SelectHeroPanel;
                panel.Depth = 600;
                panel.RichActivityHeroEntryList = XSingleton<LifeSkillManager>.Singleton.GetUsingCard();
                panel.SetZhuZhanStat(false);
                panel.mBattleType = BattleType.RichActivity;
                panel.SetButtonState(BattleType.RichActivity);
                panel.SaveButtonText = ConfigMgr.getInstance().GetWord((this.SkillType != NewLifeSkillType.NEW_LIFE_SKILL_FISHING) ? 0x2c5a : 0x2c59);
                panel.OnSaved = delegate (List<long> list) {
                    if (<>f__am$cacheC == null)
                    {
                        <>f__am$cacheC = t => (int) ActorData.getInstance().GetCardByID(t).cardInfo.entry;
                    }
                    List<int> entrys = list.Select<long, int>(<>f__am$cacheC).ToList<int>();
                    this.SelectHero(entrys);
                };
            }, null);
        }
    }

    private void SelectHero(List<int> entrys)
    {
        <SelectHero>c__AnonStorey210 storey = new <SelectHero>c__AnonStorey210 {
            entrys = entrys,
            <>f__this = this
        };
        GUIMgr.Instance.PushGUIEntity<LifeSkillSoltPanel>(new Action<GUIEntity>(storey.<>m__3EB));
    }

    internal void ShowLifeSkillEnter(NewSkillCellType LastSelectIndex, NewLifeSkillType type, long userID = -1, int cardID = -1)
    {
        <ShowLifeSkillEnter>c__AnonStorey211 storey = new <ShowLifeSkillEnter>c__AnonStorey211 {
            type = type
        };
        this.FriendID = userID;
        this.CellIndex = LastSelectIndex;
        this.SkillType = storey.type;
        this.cardEntry = cardID;
        this.TopLabel.text = (this.SkillType != NewLifeSkillType.NEW_LIFE_SKILL_FISHING) ? ConfigMgr.getInstance().GetWord(0x2c58) : ConfigMgr.getInstance().GetWord(0x2c57);
        ArrayList list = ConfigMgr.getInstance().getList<new_life_skill_time_config>();
        List<new_life_skill_time_config> source = new List<new_life_skill_time_config>();
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                source.Add(current as new_life_skill_time_config);
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
        List<new_life_skill_time_config> list3 = source.Where<new_life_skill_time_config>(new Func<new_life_skill_time_config, bool>(storey.<>m__3EC)).ToList<new_life_skill_time_config>();
        this.TableMapGrid.Count = list3.Count;
        for (int i = 0; i < this.TableMapGrid.Count; i++)
        {
            UIAutoGenItem<MapGridItemTemplate, MapGridItemModel> item = this.TableMapGrid[i];
            item.Model.OnItemClick = new Action<MapGridItemModel>(this.OnItemClick);
            item.Model.LifeSkillName = (storey.type != NewLifeSkillType.NEW_LIFE_SKILL_FISHING) ? ConfigMgr.getInstance().GetWord(0x2c5a) : ConfigMgr.getInstance().GetWord(0x2c59);
            item.Model.TimeConfig = list3[i];
        }
    }

    protected UIButton Close { get; set; }

    private int ColIndex { get; set; }

    private long FriendID { get; set; }

    private int MapEntry { get; set; }

    protected UIGrid MapGrid { get; set; }

    public NewLifeSkillType SkillType { get; set; }

    protected UILabel TopLabel { get; set; }

    [CompilerGenerated]
    private sealed class <SelectHero>c__AnonStorey210
    {
        internal LifeSkillBeginPanel <>f__this;
        internal List<int> entrys;

        internal void <>m__3EB(GUIEntity soltui)
        {
            (soltui as LifeSkillSoltPanel).ShowCard(this.<>f__this.SkillType, this.entrys[0], this.<>f__this.CellIndex, this.<>f__this.LastSchemEntry, -1L);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowLifeSkillEnter>c__AnonStorey211
    {
        internal NewLifeSkillType type;

        internal bool <>m__3EC(new_life_skill_time_config t)
        {
            return (t.type == this.type);
        }
    }

    public class MapGridItemModel : TableItemModel<LifeSkillBeginPanel.MapGridItemTemplate>
    {
        private new_life_skill_time_config _TimeConfig;
        public string LifeSkillName = string.Empty;
        public Action<LifeSkillBeginPanel.MapGridItemModel> OnItemClick;

        public override void Init(LifeSkillBeginPanel.MapGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            base.Item.Root.OnUIMouseClick(delegate (object u) {
                if (this.OnItemClick != null)
                {
                    this.OnItemClick(this);
                }
            });
        }

        public new_life_skill_time_config TimeConfig
        {
            get
            {
                return this._TimeConfig;
            }
            set
            {
                this._TimeConfig = value;
                base.Template.lb_MapName.text = value.name;
                base.Template.lb_MapTime_Time.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c5b), TimeSpan.FromMinutes((double) value.min).TotalHours, this.LifeSkillName);
                base.Template.MapIcon.spriteName = value.icon;
            }
        }
    }

    public class MapGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.lb_MapName = base.FindChild<UILabel>("lb_MapName");
            this.lb_MapTime_Time = base.FindChild<UILabel>("lb_MapTime_Time");
            this.MapIcon = base.FindChild<UISprite>("MapIcon");
        }

        public UILabel lb_MapName { get; private set; }

        public UILabel lb_MapTime_Time { get; private set; }

        public UISprite MapIcon { get; private set; }
    }
}

