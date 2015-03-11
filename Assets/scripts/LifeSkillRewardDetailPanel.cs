using FastBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class LifeSkillRewardDetailPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache6;
    [CompilerGenerated]
    private static Func<NewLifeCellReward, bool> <>f__am$cache7;
    protected UITableManager<UIAutoGenItem<GridItemTemplate, GridItemModel>> TableGrid = new UITableManager<UIAutoGenItem<GridItemTemplate, GridItemModel>>();
    private NewLifeSkillType type;

    public override void Initialize()
    {
        base.Initialize();
        if (<>f__am$cache6 == null)
        {
            <>f__am$cache6 = s => GUIMgr.Instance.PopGUIEntity();
        }
        this.Close.OnUIMouseClick(<>f__am$cache6);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.L_Title = base.FindChild<UILabel>("L_Title");
        this.Close = base.FindChild<UIButton>("Close");
        this.Grid = base.FindChild<UIGrid>("Grid");
        this.lb_empty = base.FindChild<UILabel>("lb_empty");
        this.TableGrid.InitFromGrid(this.Grid);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
    }

    private void OnItemClick(GridItemModel obj)
    {
        if (obj.CanCollect)
        {
            SocketMgr.Instance.RequestC2S_NewLifeSkillEndHangUp(this.type, (NewSkillCellType) obj.Cell.cell_num);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        IEnumerator<UIAutoGenItem<GridItemTemplate, GridItemModel>> enumerator = this.TableGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<GridItemTemplate, GridItemModel> current = enumerator.Current;
                current.Model.Update();
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

    internal void ShowRewardDetail(NewLifeSkillProfit newLifeSkillProfit)
    {
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = t => t.card_entry >= 0;
        }
        NewLifeCellReward[] rewardArray = newLifeSkillProfit.reward.Where<NewLifeCellReward>(<>f__am$cache7).ToArray<NewLifeCellReward>();
        this.TableGrid.Count = rewardArray.Length;
        int index = 0;
        IEnumerator<UIAutoGenItem<GridItemTemplate, GridItemModel>> enumerator = this.TableGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<GridItemTemplate, GridItemModel> current = enumerator.Current;
                current.Model.Cell = rewardArray[index];
                current.Model.OnItemClick = new Action<GridItemModel>(this.OnItemClick);
                index++;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        this.lb_empty.enabled = this.TableGrid.Count == 0;
    }

    public void ShowSkillType(NewLifeSkillType type)
    {
        this.type = type;
        SocketMgr.Instance.RequestC2S_NewLifeSkillProfit(type);
    }

    protected UIButton Close { get; set; }

    protected UIGrid Grid { get; set; }

    protected UILabel L_Title { get; set; }

    protected UILabel lb_empty { get; set; }

    public class GridItemModel : TableItemModel<LifeSkillRewardDetailPanel.GridItemTemplate>
    {
        private NewLifeCellReward _Cell;
        private UITableManager<ItemItem> ItemTable = new UITableManager<ItemItem>();
        public Action<LifeSkillRewardDetailPanel.GridItemModel> OnItemClick;
        private UITableManager<StartItem> startTable = new UITableManager<StartItem>();

        public override void Init(LifeSkillRewardDetailPanel.GridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.startTable.InitFromGrid(base.Template.StarGrid);
            this.ItemTable.InitFromGrid(base.Template.ItemGrid);
            base.Template.ShouHuoBtn.OnUIMouseClick(delegate (object u) {
                if (this.OnItemClick != null)
                {
                    this.OnItemClick(this);
                }
            });
        }

        public void Update()
        {
            if (this._Cell != null)
            {
                TimeSpan span = (TimeSpan) (TimeMgr.Instance.ConvertToDateTime((long) this._Cell.end_time) - TimeMgr.Instance.ServerDateTime);
                if (span.TotalSeconds < 0.0)
                {
                    span = TimeSpan.FromSeconds(0.0);
                }
                base.Template.Time.text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float) span.TotalHours), span.Minutes, span.Seconds);
                if (span.TotalSeconds < 1.0)
                {
                    base.Template.Time.gameObject.SetActive(false);
                    base.Template.ShouHuoBtn.gameObject.SetActive(true);
                }
                else
                {
                    base.Template.Time.gameObject.SetActive(true);
                    base.Template.ShouHuoBtn.gameObject.SetActive(false);
                }
            }
        }

        public bool CanCollect
        {
            get
            {
                if (this._Cell == null)
                {
                    return false;
                }
                TimeSpan span = (TimeSpan) (TimeMgr.Instance.ConvertToDateTime((long) this._Cell.end_time) - TimeMgr.Instance.ServerDateTime);
                return (span.TotalSeconds < 0.0);
            }
        }

        public NewLifeCellReward Cell
        {
            get
            {
                return this._Cell;
            }
            set
            {
                this._Cell = value;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(this._Cell.card_entry);
                if (_config != null)
                {
                    base.Template.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                }
                base.Template.lb_goldValue.text = string.Format("{0:0}", (this._Cell.add_gold <= 0) ? 0 : this._Cell.add_gold);
                base.Template.Gold.gameObject.SetActive(this._Cell.add_gold > 0);
                base.Template.ExpItem.gameObject.SetActive(this._Cell.add_exp_item_entry >= 0);
                base.Template.Friend.gameObject.SetActive(value.cell_num == 3);
                if (this._Cell.add_exp_item_entry >= 0)
                {
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(value.add_exp_item_entry);
                    if (_config2 == null)
                    {
                        return;
                    }
                    base.Template.ExpIcon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                    CommonFunc.SetEquipQualityBorder(base.Template.ExpQualityBorder, _config2.quality, false);
                    base.Template.ExpCount.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c68), this._Cell.add_exp_item_num);
                    base.Template.lb_goldReward.text = ConfigMgr.getInstance().GetWord(0x2c5c);
                }
                if (this._Cell.add_gold > 0)
                {
                    base.Template.lb_goldReward.text = ConfigMgr.getInstance().GetWord(0x2c5d);
                }
                List<Item> items = this._Cell.items;
                this.ItemTable.Count = items.Count;
                int num = 0;
                base.Template.lb_empty.gameObject.SetActive(items.Count == 0);
                IEnumerator<ItemItem> enumerator = this.ItemTable.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ItemItem current = enumerator.Current;
                        current.Item = items[num];
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
                if (value.cell_num == 3)
                {
                    CommonFunc.SetQualityBorder(base.Template.QualityBorder, 0);
                    this.startTable.Count = 0;
                }
                else
                {
                    Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) this._Cell.card_entry);
                    if (cardByEntry != null)
                    {
                        CommonFunc.SetQualityBorder(base.Template.QualityBorder, cardByEntry.cardInfo.quality);
                        this.startTable.Count = cardByEntry.cardInfo.starLv;
                    }
                }
            }
        }

        public class ItemItem : UITableItem
        {
            private FastBuf.Item _item;
            private UILabel Count;
            private UITexture Icon;
            private UISprite Part;
            private UISprite QualityBorder;

            public override void OnCreate()
            {
                this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
                this.Icon = base.FindChild<UITexture>("Icon");
                this.Count = base.FindChild<UILabel>("Count");
                this.Part = base.FindChild<UISprite>("Part");
            }

            public FastBuf.Item Item
            {
                get
                {
                    return this._item;
                }
                set
                {
                    this._item = value;
                    item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(value.entry);
                    if (_config != null)
                    {
                        bool isHead = false;
                        this.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon, out isHead);
                        int num = !isHead ? 0x38 : 70;
                        this.Icon.height = num;
                        this.Icon.width = num;
                        this.Icon.depth = !isHead ? 9 : 11;
                        CommonFunc.SetEquipQualityBorder(this.QualityBorder, _config.quality, false);
                        this.Count.text = string.Format("{0}", this._item.num);
                        this.Count.ActiveSelfObject(this._item.num > 1);
                        this.Part.enabled = (_config.type == 2) || (_config.type == 3);
                    }
                }
            }
        }

        public class StartItem : UITableItem
        {
            public override void OnCreate()
            {
            }
        }
    }

    public class GridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.lb_goldReward = base.FindChild<UILabel>("lb_goldReward");
            this.Icon = base.FindChild<UITexture>("Icon");
            this.ShouHuoBtn = base.FindChild<UIButton>("ShouHuoBtn");
            this.Time = base.FindChild<UILabel>("Time");
            this.Gold = base.FindChild<Transform>("Gold");
            this.lb_goldValue = base.FindChild<UILabel>("lb_goldValue");
            this.lb_itemReward = base.FindChild<UILabel>("lb_itemReward");
            this.StarGrid = base.FindChild<UIGrid>("StarGrid");
            this.ItemGrid = base.FindChild<UIGrid>("ItemGrid");
            this.ExpItem = base.FindChild<Transform>("ExpItem");
            this.ExpQualityBorder = base.FindChild<UISprite>("ExpQualityBorder");
            this.ExpIcon = base.FindChild<UITexture>("ExpIcon");
            this.ExpCount = base.FindChild<UILabel>("ExpCount");
            this.lb_empty = base.FindChild<UILabel>("lb_empty");
            this.Friend = base.FindChild<UISprite>("Friend");
        }

        public UILabel ExpCount { get; private set; }

        public UITexture ExpIcon { get; private set; }

        public Transform ExpItem { get; private set; }

        public UISprite ExpQualityBorder { get; private set; }

        public UISprite Friend { get; private set; }

        public Transform Gold { get; private set; }

        public UITexture Icon { get; private set; }

        public UIGrid ItemGrid { get; private set; }

        public UILabel lb_empty { get; private set; }

        public UILabel lb_goldReward { get; private set; }

        public UILabel lb_goldValue { get; private set; }

        public UILabel lb_itemReward { get; private set; }

        public UISprite QualityBorder { get; private set; }

        public UIButton ShouHuoBtn { get; private set; }

        public UIGrid StarGrid { get; private set; }

        public UILabel Time { get; private set; }
    }
}

