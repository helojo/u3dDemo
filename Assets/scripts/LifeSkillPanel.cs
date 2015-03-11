using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class LifeSkillPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache9;
    [CompilerGenerated]
    private static System.Action <>f__am$cacheA;
    [CompilerGenerated]
    private static Action<int, bool> <>f__am$cacheB;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheC;
    public List<NewLifeBaseReward> BaseReward;
    private NewSkillCellType LastSelectIndex;
    public System.Action OnClose;
    protected UITableManager<UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel>> TableHeroGrid = new UITableManager<UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel>>();

    internal void AddCard(int cellIndex, int cardEntry)
    {
        NewLifeCellInfo cellInfo = XSingleton<LifeSkillManager>.Singleton.GetCellInfo(this.CurrentType, cellIndex);
        if (cellInfo != null)
        {
            this.TableHeroGrid[cellIndex].Model.Card = cellInfo;
            Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) cardEntry);
            if (cardByEntry != null)
            {
                switch (this.CurrentType)
                {
                    case NewLifeSkillType.NEW_LIFE_SKILL_MINING:
                    {
                        MiningAcitivity acitivity = UnityEngine.Object.FindObjectOfType<MiningAcitivity>();
                        if (acitivity == null)
                        {
                            return;
                        }
                        if (cellIndex < 3)
                        {
                            acitivity.BeginMinig(cardEntry, cardByEntry.cardInfo.quality);
                        }
                        break;
                    }
                    case NewLifeSkillType.NEW_LIFE_SKILL_FISHING:
                    {
                        FishingActivity activity = UnityEngine.Object.FindObjectOfType<FishingActivity>();
                        if (activity == null)
                        {
                            return;
                        }
                        if (cellIndex < 3)
                        {
                            activity.ShowFishUp(cellIndex, cardEntry, cardByEntry.cardInfo.quality);
                        }
                        break;
                    }
                }
                SocketMgr.Instance.RequestC2S_NewLifeSkillMapSimpleData();
            }
        }
    }

    public NewLifeBaseReward GetBaseReward(int entry)
    {
        if (this.BaseReward != null)
        {
            foreach (NewLifeBaseReward reward in this.BaseReward)
            {
                if (reward.entry == entry)
                {
                    return reward;
                }
            }
        }
        return null;
    }

    public override void Initialize()
    {
        base.Initialize();
        this.bt_Close.OnUIMouseClick(delegate (object u) {
            GUIMgr.Instance.PopGUIEntity();
            if (this.OnClose != null)
            {
                this.OnClose();
            }
        });
        if (<>f__am$cache9 == null)
        {
            <>f__am$cache9 = delegate (object u) {
                if (<>f__am$cacheC == null)
                {
                    <>f__am$cacheC = ui => (ui as LifeSkillFriendAndRewardPanel).ShowTap(LifeSkillFriendAndRewardPanel.TabTypes.FRIEND);
                }
                GUIMgr.Instance.PushGUIEntity<LifeSkillFriendAndRewardPanel>(<>f__am$cacheC);
            };
        }
        this.s_friend.OnUIMouseClick(<>f__am$cache9);
        this.s_reward.OnUIMouseClick(u => GUIMgr.Instance.PushGUIEntity<LifeSkillRewardDetailPanel>(ui => (ui as LifeSkillRewardDetailPanel).ShowSkillType(this.CurrentType)));
        this.TableHeroGrid.Count = 4;
        int num = 0;
        IEnumerator<UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel>> enumerator = this.TableHeroGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel> current = enumerator.Current;
                current.Model.Index = (NewSkillCellType) num++;
                current.Model.OnClick = new Action<HeroGridItemModel>(this.ItemOnClick);
                current.Model.State = HeroItemState.Empty;
                current.Model.StarCount = num;
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

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.bt_Close = base.FindChild<UIButton>("bt_Close");
        this.s_reward = base.FindChild<UISprite>("s_reward");
        this.s_friend = base.FindChild<UISprite>("s_friend");
        this.HeroGrid = base.FindChild<UIGrid>("HeroGrid");
        this.TableHeroGrid.InitFromGrid(this.HeroGrid);
    }

    public void ItemOnClick(HeroGridItemModel model)
    {
        if ((model.State == HeroItemState.Hero) || (model.State == HeroItemState.Friend))
        {
            if (model.CanCollect)
            {
                SocketMgr.Instance.RequestC2S_NewLifeSkillEndHangUp(this.CurrentType, model.Index);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c4e));
            }
        }
        else if (model.State == HeroItemState.Empty)
        {
            switch (model.Index)
            {
                case NewSkillCellType.CELL_TYPE4:
                    GUIMgr.Instance.PushGUIEntity<LifeSkillSelectFriendPanel>(u => (u as LifeSkillSelectFriendPanel).ShowSelect(this.CurrentType));
                    return;
            }
            this.LastSelectIndex = model.Index;
            GUIMgr.Instance.PushGUIEntity<LifeSkillBeginPanel>(u => (u as LifeSkillBeginPanel).ShowLifeSkillEnter(this.LastSelectIndex, this.CurrentType, -1L, -1));
        }
    }

    private int NeedVipLevel(int pos)
    {
        ArrayList list = ConfigMgr.getInstance().getList<vip_config>();
        for (int i = 0; i < list.Count; i++)
        {
            vip_config _config = list[i] as vip_config;
            if (_config.lifeskill_card_num >= (pos + 1))
            {
                return _config.entry;
            }
        }
        return 0;
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        IEnumerator<UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel>> enumerator = this.TableHeroGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel> current = enumerator.Current;
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

    internal void RemoveCard(int cellIndex, int cardEntry)
    {
        this.TableHeroGrid[cellIndex].Model.State = HeroItemState.Empty;
        switch (this.CurrentType)
        {
            case NewLifeSkillType.NEW_LIFE_SKILL_MINING:
            {
                MiningAcitivity acitivity = UnityEngine.Object.FindObjectOfType<MiningAcitivity>();
                if (acitivity == null)
                {
                    return;
                }
                acitivity.EndMinig(cardEntry);
                break;
            }
            case NewLifeSkillType.NEW_LIFE_SKILL_FISHING:
            {
                FishingActivity activity = UnityEngine.Object.FindObjectOfType<FishingActivity>();
                if (activity == null)
                {
                    return;
                }
                activity.EndFish(cardEntry);
                break;
            }
        }
        SocketMgr.Instance.RequestC2S_NewLifeSkillMapSimpleData();
    }

    private void ShowFish()
    {
        NewLifeSkillData data = XSingleton<LifeSkillManager>.Singleton[NewLifeSkillType.NEW_LIFE_SKILL_FISHING];
        if (data != null)
        {
            FishingActivity activity = UnityEngine.Object.FindObjectOfType<FishingActivity>();
            if (activity != null)
            {
                activity.Actived(true);
                if (<>f__am$cacheB == null)
                {
                    <>f__am$cacheB = delegate (int index, bool flag) {
                    };
                }
                activity.OnRelease = <>f__am$cacheB;
                IEnumerator<UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel>> enumerator = this.TableHeroGrid.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel> current = enumerator.Current;
                        current.Model.WorkString = ConfigMgr.getInstance().GetWord(0x2c4a);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                foreach (NewLifeCellInfo info in data.cell)
                {
                    if (info.card_entry >= 0)
                    {
                        this.TableHeroGrid[info.cell_num].Model.Card = info;
                        this.TableHeroGrid[info.cell_num].Model.FullString = ConfigMgr.getInstance().GetWord(0x2c4c);
                        Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) info.card_entry);
                        if ((cardByEntry != null) && (info.cell_num < 3))
                        {
                            activity.ShowFishUp(info.cell_num, info.card_entry, cardByEntry.cardInfo.quality);
                        }
                    }
                }
            }
        }
    }

    public void ShowMapData(NewLifeSkillType type)
    {
        this.CurrentType = type;
        this.TableHeroGrid[3].Model.IsFriend = true;
        IEnumerator<UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel>> enumerator = this.TableHeroGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel> current = enumerator.Current;
                current.Model.State = current.Model.State;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        for (int i = 0; i < this.TableHeroGrid.Count; i++)
        {
            if (i < 3)
            {
                this.TableHeroGrid[i].Model.SetVip(this.NeedVipLevel(i) + 1, ActorData.getInstance().VipType + 1);
            }
        }
        SocketMgr.Instance.RequestC2S_NewLifeSkillMapEnter(type);
    }

    private void ShowMini()
    {
        NewLifeSkillData data = XSingleton<LifeSkillManager>.Singleton[NewLifeSkillType.NEW_LIFE_SKILL_MINING];
        if (data != null)
        {
            MiningAcitivity acitivity = UnityEngine.Object.FindObjectOfType<MiningAcitivity>();
            if (acitivity != null)
            {
                if (<>f__am$cacheA == null)
                {
                    <>f__am$cacheA = delegate {
                    };
                }
                acitivity.OnTapChest = <>f__am$cacheA;
                acitivity.SetActived(true);
                acitivity.Clear();
                IEnumerator<UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel>> enumerator = this.TableHeroGrid.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        UIAutoGenItem<HeroGridItemTemplate, HeroGridItemModel> current = enumerator.Current;
                        current.Model.WorkString = ConfigMgr.getInstance().GetWord(0x2c4b);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                foreach (NewLifeCellInfo info in data.cell)
                {
                    if (info.card_entry >= 0)
                    {
                        this.TableHeroGrid[info.cell_num].Model.Card = info;
                        this.TableHeroGrid[info.cell_num].Model.FullString = ConfigMgr.getInstance().GetWord(0x2c4d);
                        Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) info.card_entry);
                        if ((cardByEntry != null) && (info.cell_num < 3))
                        {
                            acitivity.BeginMinig(info.card_entry, cardByEntry.cardInfo.quality);
                        }
                    }
                }
            }
        }
    }

    internal void ShowResult(S2C_NewLifeSkillMapEnter result)
    {
        XSingleton<LifeSkillManager>.Singleton.UpdateCardData(result.card_data);
        XSingleton<LifeSkillManager>.Singleton.UpdateLifeSkillMapData(result.data);
        XSingleton<LifeSkillManager>.Singleton.UpdateNextCostStone(result.next_cost_stone);
        this.BaseReward = result.baseReward;
        switch (((NewLifeSkillType) result.entry))
        {
            case NewLifeSkillType.NEW_LIFE_SKILL_MINING:
                this.ShowMini();
                break;

            case NewLifeSkillType.NEW_LIFE_SKILL_FISHING:
                this.ShowFish();
                break;
        }
    }

    protected UIButton bt_Close { get; set; }

    public NewLifeSkillType CurrentType { get; set; }

    protected UIGrid HeroGrid { get; set; }

    protected UISprite s_friend { get; set; }

    protected UISprite s_reward { get; set; }

    public class HeroGridItemModel : TableItemModel<LifeSkillPanel.HeroGridItemTemplate>
    {
        private NewLifeCellInfo _Card;
        private LifeSkillPanel.HeroItemState _s;
        private UITableManager<UIBufferItem> BufferTable = new UITableManager<UIBufferItem>();
        private string FullImage;
        public string FullString;
        private bool hadSet;
        public bool IsFriend;
        public Action<LifeSkillPanel.HeroGridItemModel> OnClick;
        private UITableManager<UIStarItem> StarTable = new UITableManager<UIStarItem>();
        public string WorkString;

        public override void Init(LifeSkillPanel.HeroGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            base.Item.Root.OnUIMouseClick(delegate (object u) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
            this.StarTable.InitFromGrid(base.Template.Grid);
            this.BufferTable.InitFromGrid(base.Template.BufferGrid);
        }

        public void SetVip(int v, int lvel)
        {
            this.State = (lvel >= v) ? LifeSkillPanel.HeroItemState.Empty : LifeSkillPanel.HeroItemState.Lock;
            if (this.State != LifeSkillPanel.HeroItemState.Empty)
            {
                base.Template.VSumalRoot.gameObject.SetActive(lvel < v);
                base.Template.VLabel.text = ConfigMgr.getInstance().GetWord(0x2c4f);
                base.Template.VLevel.spriteName = string.Format("Ui_Main_Icon_{0}", v);
                base.Template.Add.gameObject.SetActive(lvel >= v);
            }
        }

        public void Update()
        {
            base.Template.Label.color = Color.white;
            if ((this.State == LifeSkillPanel.HeroItemState.Hero) || (this.State == LifeSkillPanel.HeroItemState.Friend))
            {
                TimeSpan span = (TimeSpan) (TimeMgr.Instance.ConvertToDateTime((long) this._Card.end_time) - TimeMgr.Instance.ServerDateTime);
                if (span.TotalSeconds < 0.0)
                {
                    if (!this.hadSet)
                    {
                        this.hadSet = true;
                        base.Template.Icon1.mainTexture = BundleMgr.Instance.CreateHeadIcon(this.FullImage);
                        base.Template.Icon1.width = 0x83;
                        base.Template.Icon1.height = 0x95;
                    }
                    span = TimeSpan.FromSeconds(0.0);
                    base.Template.Label.text = this.FullString;
                    base.Template.lb_time.text = string.Empty;
                }
                else
                {
                    base.Template.lb_time.text = string.Format("{0:00}:{1:00}:{2:00}", Math.Floor(span.TotalHours), span.Minutes, span.Seconds);
                }
            }
        }

        public bool CanCollect
        {
            get
            {
                if ((this.State != LifeSkillPanel.HeroItemState.Hero) && (this.State != LifeSkillPanel.HeroItemState.Friend))
                {
                    return false;
                }
                if (this.Card == null)
                {
                    return false;
                }
                return (TimeMgr.Instance.ConvertToDateTime((long) this.Card.end_time) < TimeMgr.Instance.ServerDateTime);
            }
        }

        public NewLifeCellInfo Card
        {
            get
            {
                return this._Card;
            }
            set
            {
                base.Template.CardBg.ActiveSelfObject(true);
                this.hadSet = false;
                this._Card = value;
                this.State = LifeSkillPanel.HeroItemState.Hero;
                base.Template.Grid.gameObject.SetActive(this._Card.cell_num != 3);
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(value.card_entry);
                if (_config != null)
                {
                    base.Template.Icon1.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    base.Template.Icon1.width = 120;
                    base.Template.Icon1.height = 120;
                    this.FullImage = _config.image + "_nu";
                    List<MeBufferData> list = new List<MeBufferData>();
                    for (int i = 0; i < value.RandInfo.Count; i++)
                    {
                        int[] numArray;
                        if (value.RandInfo[i].entry < 0)
                        {
                            break;
                        }
                        NewLifeSkillRandInfo info = value.RandInfo[i];
                        new_life_skill_rand_config _config2 = ConfigMgr.getInstance().getByEntry<new_life_skill_rand_config>(value.RandInfo[i].entry);
                        MeBufferData item = new MeBufferData {
                            Quality = _config2.quality
                        };
                        switch (i)
                        {
                            case 0:
                                numArray = _config2.parameter_definition1.SplitToInt('|');
                                item.Count = ((numArray == null) || (numArray.Length <= 1)) ? 0 : numArray[1];
                                item.Icon = _config2.icon;
                                item.IsPart = _config2.IsPart == 1;
                                item.Param = _config2.parameter1;
                                break;

                            case 1:
                                numArray = _config2.parameter_definition2.SplitToInt('|');
                                item.Count = ((numArray == null) || (numArray.Length <= 1)) ? 0 : numArray[1];
                                item.Icon = _config2.icon;
                                item.IsPart = _config2.IsPart == 1;
                                item.Param = _config2.parameter2;
                                break;

                            case 2:
                                numArray = _config2.parameter_definition3.SplitToInt('|');
                                item.Count = ((numArray == null) || (numArray.Length <= 1)) ? 0 : numArray[1];
                                item.Icon = _config2.icon;
                                item.IsPart = _config2.IsPart == 1;
                                item.Param = _config2.parameter3;
                                break;
                        }
                        list.Add(item);
                    }
                    this.BufferTable.Count = list.Count;
                    for (int j = 0; j < list.Count; j++)
                    {
                        this.BufferTable[j].RandConfig = list[j];
                    }
                    FastBuf.Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) value.card_entry);
                    if (this._Card.cell_num == 3)
                    {
                        cardByEntry = null;
                        CommonFunc.SetPlayerHeadFrame(base.Template.HeadFrameBorder, base.Template.QIcon, value.head_frame_entry);
                        this.State = LifeSkillPanel.HeroItemState.Friend;
                        base.Template.CardBg.ActiveSelfObject(false);
                    }
                    if (cardByEntry == null)
                    {
                        this.StarCount = 0;
                        CommonFunc.SetQualityBorder(base.Template.QualityBorder, 0);
                    }
                    else
                    {
                        this.StarCount = cardByEntry.cardInfo.starLv;
                        CommonFunc.SetQualityBorder(base.Template.QualityBorder, cardByEntry.cardInfo.quality);
                    }
                }
            }
        }

        public NewSkillCellType Index { get; set; }

        public int StarCount
        {
            get
            {
                return this.StarTable.Count;
            }
            set
            {
                this.StarTable.Count = value;
                Vector3 localPosition = base.Template.Grid.transform.localPosition;
                base.Template.Grid.transform.localPosition = new Vector3((float) (-10 + (value * -9)), localPosition.y, localPosition.z);
            }
        }

        public LifeSkillPanel.HeroItemState State
        {
            get
            {
                return this._s;
            }
            set
            {
                this._s = value;
                base.Template.Label.text = this.WorkString;
                base.Template.Add.gameObject.SetActive(false);
                base.Template.Icon1.gameObject.SetActive(false);
                base.Template.Label.gameObject.SetActive(false);
                base.Template.lb_time.gameObject.SetActive(false);
                base.Template.VSumalRoot.gameObject.SetActive(false);
                base.Template.Grid.gameObject.SetActive(false);
                base.Template.Friend.gameObject.SetActive(this.IsFriend);
                base.Template.QIcon.gameObject.SetActive(false);
                base.Template.HeadFrameBorder.gameObject.SetActive(false);
                base.Template.QualityBorder.gameObject.SetActive(true);
                base.Template.CardBg.ActiveSelfObject(true);
                switch (this._s)
                {
                    case LifeSkillPanel.HeroItemState.Empty:
                        base.Template.Add.gameObject.SetActive(true);
                        CommonFunc.SetQualityBorder(base.Template.QualityBorder, 0);
                        this.StarTable.Count = 0;
                        this.BufferTable.Count = 0;
                        base.Template.HeroBackground.color = (Color) new Color32(0x27, 30, 30, 0x7d);
                        break;

                    case LifeSkillPanel.HeroItemState.Lock:
                        base.Template.VSumalRoot.gameObject.SetActive(true);
                        break;

                    case LifeSkillPanel.HeroItemState.Hero:
                        base.Template.HeroBackground.color = (Color) new Color32(0x27, 30, 30, 0xff);
                        base.Template.Icon1.gameObject.SetActive(true);
                        base.Template.Grid.gameObject.SetActive(true);
                        base.Template.lb_time.gameObject.SetActive(true);
                        base.Template.Label.gameObject.SetActive(true);
                        base.Template.Icon1.gameObject.SetActive(true);
                        break;

                    case LifeSkillPanel.HeroItemState.Friend:
                        base.Template.Icon1.gameObject.SetActive(true);
                        base.Template.QIcon.gameObject.SetActive(true);
                        base.Template.HeadFrameBorder.gameObject.SetActive(true);
                        base.Template.QualityBorder.gameObject.SetActive(false);
                        base.Template.lb_time.gameObject.SetActive(true);
                        base.Template.Label.gameObject.SetActive(true);
                        break;
                }
            }
        }

        public class MeBufferData
        {
            public int Count { get; set; }

            public string Icon { get; set; }

            public bool IsPart { get; set; }

            public int Param { get; set; }

            public int Quality { get; set; }
        }

        public class UIBufferItem : UITableItem
        {
            private LifeSkillPanel.HeroGridItemModel.MeBufferData _rand;
            private UISprite b1bg;
            private UISprite bg;
            private UILabel Label1;
            private UISprite part1;
            private UITexture tBuffer1prite;

            public override void OnCreate()
            {
                this.tBuffer1prite = base.FindChild<UITexture>("tBuffer1prite");
                this.part1 = base.FindChild<UISprite>("part1");
                this.b1bg = base.FindChild<UISprite>("b1bg");
                this.bg = base.FindChild<UISprite>("bg");
                this.Label1 = base.FindChild<UILabel>("Label1");
            }

            public LifeSkillPanel.HeroGridItemModel.MeBufferData RandConfig
            {
                set
                {
                    this._rand = value;
                    this.Label1.text = string.Empty;
                    if (value != null)
                    {
                        this.b1bg.ActiveSelfObject(value.Param != 0);
                        this.bg.ActiveSelfObject(value.Param != 0);
                        this.tBuffer1prite.mainTexture = BundleMgr.Instance.CreateItemIcon(value.Icon);
                        this.Label1.text = string.Format("{0:0}", value.Count);
                        this.Label1.ActiveSelfObject(value.Count > 1);
                        this.part1.ActiveSelfObject(value.IsPart);
                        CommonFunc.SetEquipQualityBorder(this.b1bg, value.Quality, false);
                    }
                }
            }
        }

        public class UIStarItem : UITableItem
        {
            public override void OnCreate()
            {
            }
        }
    }

    public class HeroGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.Grid = base.FindChild<UIGrid>("Grid");
            this.Icon1 = base.FindChild<UITexture>("Icon1");
            this.CardBg = base.FindChild<UISprite>("CardBg");
            this.HeroBackground = base.FindChild<UISprite>("HeroBackground");
            this.Add = base.FindChild<UITexture>("Add");
            this.VSumalRoot = base.FindChild<Transform>("VSumalRoot");
            this.VLabel = base.FindChild<UILabel>("VLabel");
            this.VLevel = base.FindChild<UISprite>("VLevel");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.Label = base.FindChild<UILabel>("Label");
            this.lb_time = base.FindChild<UILabel>("lb_time");
            this.Friend = base.FindChild<UISprite>("Friend");
            this.BufferGrid = base.FindChild<UIGrid>("BufferGrid");
            this.HeadFrameBorder = base.FindChild<UISprite>("HeadFrameBorder");
            this.QIcon = base.FindChild<UISprite>("QIcon");
        }

        public UITexture Add { get; private set; }

        public UIGrid BufferGrid { get; private set; }

        public UISprite CardBg { get; private set; }

        public UISprite Friend { get; private set; }

        public UIGrid Grid { get; private set; }

        public UISprite HeadFrameBorder { get; private set; }

        public UISprite HeroBackground { get; private set; }

        public UITexture Icon1 { get; private set; }

        public UILabel Label { get; private set; }

        public UILabel lb_time { get; private set; }

        public UISprite QIcon { get; private set; }

        public UISprite QualityBorder { get; private set; }

        public UILabel VLabel { get; private set; }

        public UISprite VLevel { get; private set; }

        public Transform VSumalRoot { get; private set; }
    }

    public enum HeroItemState
    {
        Empty,
        Lock,
        Hero,
        Friend
    }
}

