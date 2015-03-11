using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class LifeSkillSoltPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache3A;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache3B;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3C;
    private int cardEntry = -1;
    private int CellHeight = 90;
    private NewSkillCellType ColIndex;
    private long FriendID = -1L;
    private bool haveInit;
    private bool HavResult;
    private bool IsSolting;
    private bool ok1;
    private bool ok2;
    private bool ok3;
    private int Schem_Entry;
    public float Speed = 1f;
    private int StopLimit = 1;
    protected UITableManager<UIAutoGenItem<GridCol1ItemTemplate, GridCol1ItemModel>> TableGridCol1 = new UITableManager<UIAutoGenItem<GridCol1ItemTemplate, GridCol1ItemModel>>();
    protected UITableManager<UIAutoGenItem<GridCol2ItemTemplate, GridCol2ItemModel>> TableGridCol2 = new UITableManager<UIAutoGenItem<GridCol2ItemTemplate, GridCol2ItemModel>>();
    protected UITableManager<UIAutoGenItem<GridCol3ItemTemplate, GridCol3ItemModel>> TableGridCol3 = new UITableManager<UIAutoGenItem<GridCol3ItemTemplate, GridCol3ItemModel>>();
    protected UITableManager<UIAutoGenItem<StarGridItemTemplate, StarGridItemModel>> TableStarGrid = new UITableManager<UIAutoGenItem<StarGridItemTemplate, StarGridItemModel>>();
    private Vector3 To1Pos;
    private Vector3 To2Pos;
    private Vector3 To3Pos;
    private Vector3 Zero1Pos = new Vector3(68f, -110f, 0f);
    private Vector3 Zero2Pos = new Vector3(180f, -110f, 0f);
    private Vector3 Zero3Pos = new Vector3(290f, -110f, 0f);

    public void BeginSolt()
    {
        this.IsSolting = true;
        this.ok1 = this.ok2 = this.ok3 = false;
        this.To1Pos = new Vector3(this.Zero1Pos.x, this.Zero1Pos.y + ((this.TableGridCol1.Count - 1) * this.CellHeight), 0f);
        this.To2Pos = new Vector3(this.Zero2Pos.x, this.Zero2Pos.y + ((this.TableGridCol2.Count - 1) * this.CellHeight), 0f);
        this.To3Pos = new Vector3(this.Zero3Pos.x, this.Zero3Pos.y + ((this.TableGridCol3.Count - 1) * this.CellHeight), 0f);
        this.GridCol1.transform.localPosition = this.Zero1Pos;
        this.GridCol2.transform.localPosition = this.Zero2Pos;
        this.GridCol3.transform.localPosition = this.Zero3Pos;
    }

    private void BtOnClick(bool free)
    {
        if (XSingleton<LifeSkillManager>.Singleton.HandTimes > 0)
        {
            this.ShowSolt(free);
        }
        else
        {
            if (<>f__am$cache3B == null)
            {
                <>f__am$cache3B = delegate (GameObject go) {
                    if (<>f__am$cache3C == null)
                    {
                        <>f__am$cache3C = u => (u as LifeSkillFriendAndRewardPanel).ShowTap(LifeSkillFriendAndRewardPanel.TabTypes.FRIEND);
                    }
                    GUIMgr.Instance.PushGUIEntity<LifeSkillFriendAndRewardPanel>(<>f__am$cache3C);
                };
            }
            MessageBox.ShowMessageBox(ConfigMgr.getInstance().GetWord(0x2c60), <>f__am$cache3B, null, false);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        this.TableStarGrid.Count = 5;
        if (<>f__am$cache3A == null)
        {
            <>f__am$cache3A = s => GUIMgr.Instance.PopGUIEntity();
        }
        this.bt_close.OnUIMouseClick(<>f__am$cache3A);
        this.bt_begin.OnUIMouseClick(delegate (object u) {
            GUIMgr.Instance.PopGUIEntity();
            SocketMgr.Instance.RequestC2S_NewLifeSkillHangUp(this.CurrentType, this.ColIndex, this.cardEntry, this.Schem_Entry, this.FriendID);
        });
        this.bt_soltFree.OnUIMouseClick(u => this.BtOnClick(true));
        this.bt_soltPay.OnUIMouseClick(u => this.BtOnClick(false));
        int num = 30;
        this.TableGridCol3.Count = num;
        this.TableGridCol2.Count = num;
        this.TableGridCol1.Count = num;
        this.lb_Coin.text = string.Format("{0:0}", 10);
        this.DescriptLabel.text = ConfigMgr.getInstance().GetWord(0x2c6a);
        this.bt_soltFree.Text(XSingleton<ConfigMgr>.Singleton[0x2c88]);
        this.bt_soltPay.Text(XSingleton<ConfigMgr>.Singleton[0x2c89]);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.s_cardQuality = base.FindChild<UISprite>("s_cardQuality");
        this.StarGrid = base.FindChild<UIGrid>("StarGrid");
        this.IconTexture = base.FindChild<UITexture>("IconTexture");
        this.lb_mapName = base.FindChild<UILabel>("lb_mapName");
        this.lb_rewardAppend = base.FindChild<UILabel>("lb_rewardAppend");
        this.lb_durtionTime = base.FindChild<UILabel>("lb_durtionTime");
        this.DescriptLabel = base.FindChild<UILabel>("DescriptLabel");
        this.bt_soltFree = base.FindChild<UIButton>("bt_soltFree");
        this.bt_soltPay = base.FindChild<UIButton>("bt_soltPay");
        this.bt_begin = base.FindChild<UIButton>("bt_begin");
        this.GridCol1 = base.FindChild<UIGrid>("GridCol1");
        this.GridCol2 = base.FindChild<UIGrid>("GridCol2");
        this.GridCol3 = base.FindChild<UIGrid>("GridCol3");
        this.lb_Coin = base.FindChild<UILabel>("lb_Coin");
        this.lb_tryTime = base.FindChild<UILabel>("lb_tryTime");
        this.bt_close = base.FindChild<UIButton>("bt_close");
        this.tipPanel = base.FindChild<UIPanel>("tipPanel");
        this.tipDesc = base.FindChild<UILabel>("tipDesc");
        this.tipBg = base.FindChild<UISprite>("tipBg");
        this.s_tipIcon = base.FindChild<UITexture>("s_tipIcon");
        this.tipPart = base.FindChild<UISprite>("tipPart");
        this.tipCount = base.FindChild<UILabel>("tipCount");
        this.tipQualityBorder = base.FindChild<UISprite>("tipQualityBorder");
        this.TipInfo = base.FindChild<Transform>("TipInfo");
        this.TipHaveCount = base.FindChild<UILabel>("TipHaveCount");
        this.TipName = base.FindChild<UILabel>("TipName");
        this.Item = base.FindChild<Transform>("Item");
        this.IIcon = base.FindChild<UITexture>("IIcon");
        this.IQualityBorder = base.FindChild<UISprite>("IQualityBorder");
        this.Background = base.FindChild<UISprite>("Background");
        this.IPart = base.FindChild<UISprite>("IPart");
        this.INum = base.FindChild<UILabel>("INum");
        this.s_gold = base.FindChild<UISprite>("s_gold");
        this.lb_rewardgold = base.FindChild<UILabel>("lb_rewardgold");
        this.TableStarGrid.InitFromGrid(this.StarGrid);
        this.TableGridCol1.InitFromGrid(this.GridCol1);
        this.TableGridCol2.InitFromGrid(this.GridCol2);
        this.TableGridCol3.InitFromGrid(this.GridCol3);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        this.OnTipShow(null, false, 1);
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        if (!this.haveInit)
        {
            this.haveInit = true;
            base.StartCoroutine(this.SetDefault());
        }
    }

    public void OnTipShow(new_life_skill_rand_config config, bool show, int index)
    {
        if (this.tipPanel.gameObject.activeSelf != show)
        {
            this.tipPanel.ActiveSelfObject(show);
            if (config == null)
            {
                this.tipPanel.ActiveSelfObject(false);
            }
            else if (this.tipPanel.gameObject.activeSelf)
            {
                this.tipPanel.transform.localPosition = new Vector3((float) (-55 + ((index - 1) * 100)), 100f, 0f);
                this.s_tipIcon.mainTexture = BundleMgr.Instance.CreateItemIcon(config.icon);
                this.tipQualityBorder.ActiveSelfObject(config.quality > -1);
                if (config.quality > -1)
                {
                    CommonFunc.SetEquipQualityBorder(this.tipQualityBorder, config.quality, false);
                }
                this.tipBg.ActiveSelfObject(config.parameter1 != 0);
                this.TipInfo.ActiveSelfObject(config.parameter1 != 0);
                this.tipPart.enabled = config.IsPart == 1;
                int result = 1;
                int num2 = -1;
                char[] separator = new char[] { '|' };
                string[] strArray = config.parameter_definition1.Split(separator);
                if (strArray.Length == 2)
                {
                    int.TryParse(strArray[1], out result);
                    int.TryParse(strArray[0], out num2);
                }
                this.tipCount.text = string.Format("{0:0}", result);
                this.tipCount.ActiveSelfObject(config.parameter1 != 0);
                this.tipCount.ActiveSelfObject(result > 1);
                string name = config.buff_name;
                string describe = config.describe;
                if (config.parameter2 != 0)
                {
                    item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(num2);
                    if (_config != null)
                    {
                        name = _config.name;
                        describe = _config.describe;
                    }
                }
                int itemCountByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemCountByEntry(num2);
                this.TipHaveCount.text = string.Format("{0:0}", itemCountByEntry);
                this.tipDesc.text = describe;
                this.TipName.text = name;
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.IsSolting)
        {
            if (this.GridCol1.transform.localPosition.y >= (this.To1Pos.y - 90f))
            {
                this.GridCol1.transform.localPosition = new Vector3(this.To1Pos.x, this.To1Pos.y - (this.StopLimit * this.CellHeight), this.To1Pos.z);
                this.ok1 = true;
            }
            if (this.GridCol2.transform.localPosition.y >= (this.To2Pos.y - 90f))
            {
                this.GridCol2.transform.localPosition = new Vector3(this.To2Pos.x, this.To2Pos.y - (this.StopLimit * this.CellHeight), this.To2Pos.z);
                this.ok2 = true;
            }
            if (this.GridCol3.transform.localPosition.y >= (this.To3Pos.y - 90f))
            {
                this.GridCol3.transform.localPosition = new Vector3(this.To3Pos.x, this.To3Pos.y - (this.StopLimit * this.CellHeight), this.To3Pos.z);
                this.ok3 = true;
            }
            if (!this.ok1)
            {
                this.GridCol1.transform.localPosition = Vector3.Lerp(this.GridCol1.transform.localPosition, this.To1Pos, Time.deltaTime * this.Speed);
            }
            if (!this.ok2)
            {
                this.GridCol2.transform.localPosition = Vector3.Lerp(this.GridCol2.transform.localPosition, this.To2Pos, Time.deltaTime * this.Speed);
            }
            if (!this.ok3)
            {
                this.GridCol3.transform.localPosition = Vector3.Lerp(this.GridCol3.transform.localPosition, this.To3Pos, Time.deltaTime * this.Speed);
            }
            if ((this.ok1 && this.ok2) && this.ok3)
            {
                this.IsSolting = false;
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator SetDefault()
    {
        return new <SetDefault>c__Iterator86 { <>f__this = this };
    }

    public void ShowCard(NewLifeSkillType type, int entry, NewSkillCellType index, int schem_entry, long userID = -1)
    {
        this.FriendID = userID;
        this.s_gold.ActiveSelfObject(false);
        this.Item.ActiveSelfObject(false);
        this.HavResult = false;
        this.bt_begin.Text(ConfigMgr.getInstance().GetWord((type != NewLifeSkillType.NEW_LIFE_SKILL_FISHING) ? 0x2c62 : 0x2c61));
        new_life_skill_time_config _config = ConfigMgr.getInstance().getByEntry<new_life_skill_time_config>(schem_entry);
        if (_config != null)
        {
            TimeSpan span = TimeSpan.FromMinutes((double) _config.min);
            this.lb_durtionTime.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c63), span.TotalHours);
            this.lb_mapName.text = _config.name;
        }
        this.CurrentType = type;
        this.cardEntry = entry;
        this.lb_tryTime.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c64), XSingleton<LifeSkillManager>.Singleton.HandTimes);
        this.ColIndex = index;
        this.Schem_Entry = schem_entry;
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(entry);
        if (_config2 != null)
        {
            this.IconTexture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
        }
        if (index == NewSkillCellType.CELL_TYPE4)
        {
            CommonFunc.SetQualityBorder(this.s_cardQuality, 0);
            this.TableStarGrid.Count = 0;
        }
        else
        {
            Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) entry);
            if (cardByEntry != null)
            {
                CommonFunc.SetQualityBorder(this.s_cardQuality, cardByEntry.cardInfo.quality);
                this.TableStarGrid.Count = cardByEntry.cardInfo.starLv;
            }
        }
        this.haveInit = false;
        this.lb_Coin.text = string.Format("{0:0}", XSingleton<LifeSkillManager>.Singleton.next_cost_stone);
        LifeSkillPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<LifeSkillPanel>();
        if (gUIEntity != null)
        {
            NewLifeBaseReward baseReward = gUIEntity.GetBaseReward(schem_entry);
            if (baseReward != null)
            {
                this.s_gold.ActiveSelfObject(baseReward.add_gold > 0);
                this.lb_rewardgold.text = string.Format("{0}", baseReward.add_gold);
                this.Item.ActiveSelfObject(baseReward.items.Count > 0);
                if (baseReward.items.Count > 0)
                {
                    FastBuf.Item item = baseReward.items[0];
                    item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
                    if (_config3 != null)
                    {
                        this.IIcon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config3.icon);
                        CommonFunc.SetEquipQualityBorder(this.IQualityBorder, _config3.quality, false);
                        this.IPart.enabled = _config3.type == 2;
                        int num = item.num;
                        this.INum.text = string.Format("{0:0}", num);
                    }
                }
            }
        }
    }

    internal void ShowResult(List<NewLifeSkillRandInfo> list)
    {
        <ShowResult>c__AnonStorey214 storey = new <ShowResult>c__AnonStorey214 {
            list = list,
            <>f__this = this
        };
        this.lb_Coin.text = string.Format("{0:0}", XSingleton<LifeSkillManager>.Singleton.next_cost_stone);
        if (storey.list.Count >= 1)
        {
            this.HavResult = true;
            this.TableGridCol1.Count = GRandomer.RandomMinAndMax(10, 30);
            this.TableGridCol2.Count = GRandomer.RandomMinAndMax(10, 30);
            this.TableGridCol3.Count = GRandomer.RandomMinAndMax(10, 30);
            this.BeginSolt();
            List<new_life_skill_rand_config> source = new List<new_life_skill_rand_config>(0x80);
            IEnumerator enumerator = ConfigMgr.getInstance().getList<new_life_skill_rand_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    source.Add(current as new_life_skill_rand_config);
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
            new_life_skill_rand_config _config = source.SingleOrDefault<new_life_skill_rand_config>(new Func<new_life_skill_rand_config, bool>(storey.<>m__415));
            new_life_skill_rand_config _config2 = source.SingleOrDefault<new_life_skill_rand_config>(new Func<new_life_skill_rand_config, bool>(storey.<>m__416));
            new_life_skill_rand_config _config3 = source.SingleOrDefault<new_life_skill_rand_config>(new Func<new_life_skill_rand_config, bool>(storey.<>m__417));
            for (int i = 0; i < this.TableGridCol1.Count; i++)
            {
                this.TableGridCol1[i].Model.OnPress = null;
                if (i == ((this.TableGridCol1.Count - 1) - this.StopLimit))
                {
                    this.TableGridCol1[i].Model.RandConfig = _config;
                    this.TableGridCol1[i].Model.OnPress = new Action<bool, new_life_skill_rand_config>(storey.<>m__418);
                }
                else
                {
                    this.TableGridCol1[i].Model.RandConfig = GRandomer.RandomList<new_life_skill_rand_config>(source);
                }
            }
            for (int j = 0; j < this.TableGridCol2.Count; j++)
            {
                this.TableGridCol2[j].Model.OnPress = null;
                if (j == ((this.TableGridCol2.Count - 1) - this.StopLimit))
                {
                    this.TableGridCol2[j].Model.RandConfig = _config2;
                    this.TableGridCol2[j].Model.OnPress = new Action<bool, new_life_skill_rand_config>(storey.<>m__419);
                }
                else
                {
                    this.TableGridCol2[j].Model.RandConfig = GRandomer.RandomList<new_life_skill_rand_config>(source);
                }
            }
            for (int k = 0; k < this.TableGridCol3.Count; k++)
            {
                this.TableGridCol3[k].Model.OnPress = null;
                if (k == ((this.TableGridCol3.Count - 1) - this.StopLimit))
                {
                    this.TableGridCol3[k].Model.RandConfig = _config3;
                    this.TableGridCol3[k].Model.OnPress = new Action<bool, new_life_skill_rand_config>(storey.<>m__41A);
                }
                else
                {
                    this.TableGridCol3[k].Model.RandConfig = GRandomer.RandomList<new_life_skill_rand_config>(source);
                }
            }
            this.lb_tryTime.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c64), XSingleton<LifeSkillManager>.Singleton.HandTimes);
            this.UpdateButtons();
        }
    }

    private void ShowSolt(bool free)
    {
        <ShowSolt>c__AnonStorey213 storey = new <ShowSolt>c__AnonStorey213 {
            free = free,
            <>f__this = this
        };
        if (this.HavResult)
        {
            MessageBox.ShowMessageBox(ConfigMgr.getInstance().GetWord(0x2c67), new UIEventListener.VoidDelegate(storey.<>m__414), null, false);
        }
        else
        {
            SocketMgr.Instance.RequestC2S_NewLifeSkillRandReward(this.CurrentType, this.ColIndex, !storey.free ? 1 : 0);
        }
    }

    private void UpdateButtons()
    {
    }

    public void UpdateRemain()
    {
        this.lb_tryTime.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c64), XSingleton<LifeSkillManager>.Singleton.HandTimes);
    }

    protected UISprite Background { get; set; }

    protected UIButton bt_begin { get; set; }

    protected UIButton bt_close { get; set; }

    protected UIButton bt_soltFree { get; set; }

    protected UIButton bt_soltPay { get; set; }

    public NewLifeSkillType CurrentType { get; set; }

    protected UILabel DescriptLabel { get; set; }

    protected UIGrid GridCol1 { get; set; }

    protected UIGrid GridCol2 { get; set; }

    protected UIGrid GridCol3 { get; set; }

    protected UITexture IconTexture { get; set; }

    protected UITexture IIcon { get; set; }

    protected UILabel INum { get; set; }

    protected UISprite IPart { get; set; }

    protected UISprite IQualityBorder { get; set; }

    protected Transform Item { get; set; }

    protected UILabel lb_Coin { get; set; }

    protected UILabel lb_durtionTime { get; set; }

    protected UILabel lb_mapName { get; set; }

    protected UILabel lb_rewardAppend { get; set; }

    protected UILabel lb_rewardgold { get; set; }

    protected UILabel lb_tryTime { get; set; }

    protected UISprite s_cardQuality { get; set; }

    protected UISprite s_gold { get; set; }

    protected UITexture s_tipIcon { get; set; }

    protected UIGrid StarGrid { get; set; }

    protected UISprite tipBg { get; set; }

    protected UILabel tipCount { get; set; }

    protected UILabel tipDesc { get; set; }

    protected UILabel TipHaveCount { get; set; }

    protected Transform TipInfo { get; set; }

    protected UILabel TipName { get; set; }

    protected UIPanel tipPanel { get; set; }

    protected UISprite tipPart { get; set; }

    protected UISprite tipQualityBorder { get; set; }

    [CompilerGenerated]
    private sealed class <SetDefault>c__Iterator86 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal IEnumerator<UIAutoGenItem<LifeSkillSoltPanel.GridCol1ItemTemplate, LifeSkillSoltPanel.GridCol1ItemModel>> <$s_745>__0;
        internal IEnumerator<UIAutoGenItem<LifeSkillSoltPanel.GridCol2ItemTemplate, LifeSkillSoltPanel.GridCol2ItemModel>> <$s_746>__2;
        internal IEnumerator<UIAutoGenItem<LifeSkillSoltPanel.GridCol3ItemTemplate, LifeSkillSoltPanel.GridCol3ItemModel>> <$s_747>__4;
        internal IEnumerator <$s_748>__9;
        internal IEnumerator<UIAutoGenItem<LifeSkillSoltPanel.GridCol1ItemTemplate, LifeSkillSoltPanel.GridCol1ItemModel>> <$s_749>__11;
        internal IEnumerator<UIAutoGenItem<LifeSkillSoltPanel.GridCol2ItemTemplate, LifeSkillSoltPanel.GridCol2ItemModel>> <$s_750>__13;
        internal IEnumerator<UIAutoGenItem<LifeSkillSoltPanel.GridCol3ItemTemplate, LifeSkillSoltPanel.GridCol3ItemModel>> <$s_751>__15;
        internal LifeSkillSoltPanel <>f__this;
        internal NewLifeCellInfo <cell>__6;
        internal List<new_life_skill_rand_config> <config>__7;
        internal new_life_skill_rand_config <config1>__17;
        internal new_life_skill_rand_config <config2>__18;
        internal new_life_skill_rand_config <config3>__19;
        internal ArrayList <configs>__8;
        internal UIAutoGenItem<LifeSkillSoltPanel.GridCol1ItemTemplate, LifeSkillSoltPanel.GridCol1ItemModel> <i>__1;
        internal object <i>__10;
        internal UIAutoGenItem<LifeSkillSoltPanel.GridCol1ItemTemplate, LifeSkillSoltPanel.GridCol1ItemModel> <i>__12;
        internal UIAutoGenItem<LifeSkillSoltPanel.GridCol2ItemTemplate, LifeSkillSoltPanel.GridCol2ItemModel> <i>__14;
        internal UIAutoGenItem<LifeSkillSoltPanel.GridCol3ItemTemplate, LifeSkillSoltPanel.GridCol3ItemModel> <i>__16;
        internal UIAutoGenItem<LifeSkillSoltPanel.GridCol2ItemTemplate, LifeSkillSoltPanel.GridCol2ItemModel> <i>__3;
        internal UIAutoGenItem<LifeSkillSoltPanel.GridCol3ItemTemplate, LifeSkillSoltPanel.GridCol3ItemModel> <i>__5;
        internal int <index>__20;

        internal bool <>m__41C(new_life_skill_rand_config t)
        {
            return (t.entry == this.<cell>__6.RandInfo[0].entry);
        }

        internal bool <>m__41D(new_life_skill_rand_config t)
        {
            return (t.entry == this.<cell>__6.RandInfo[1].entry);
        }

        internal bool <>m__41E(new_life_skill_rand_config t)
        {
            return (t.entry == this.<cell>__6.RandInfo[2].entry);
        }

        internal void <>m__41F(bool ispress, new_life_skill_rand_config item)
        {
            this.<>f__this.OnTipShow(item, ispress, 1);
        }

        internal void <>m__420(bool ispress, new_life_skill_rand_config item)
        {
            this.<>f__this.OnTipShow(item, ispress, 2);
        }

        internal void <>m__421(bool ispress, new_life_skill_rand_config item)
        {
            this.<>f__this.OnTipShow(item, ispress, 3);
        }

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
                {
                    int num2 = 3;
                    this.<>f__this.TableGridCol3.Count = num2;
                    this.<>f__this.TableGridCol2.Count = num2;
                    this.<>f__this.TableGridCol1.Count = num2;
                    this.<$s_745>__0 = this.<>f__this.TableGridCol1.GetEnumerator();
                    try
                    {
                        while (this.<$s_745>__0.MoveNext())
                        {
                            this.<i>__1 = this.<$s_745>__0.Current;
                            this.<i>__1.Model.SetUnkown();
                        }
                    }
                    finally
                    {
                        if (this.<$s_745>__0 == null)
                        {
                        }
                        this.<$s_745>__0.Dispose();
                    }
                    this.<$s_746>__2 = this.<>f__this.TableGridCol2.GetEnumerator();
                    try
                    {
                        while (this.<$s_746>__2.MoveNext())
                        {
                            this.<i>__3 = this.<$s_746>__2.Current;
                            this.<i>__3.Model.SetUnkown();
                        }
                    }
                    finally
                    {
                        if (this.<$s_746>__2 == null)
                        {
                        }
                        this.<$s_746>__2.Dispose();
                    }
                    this.<$s_747>__4 = this.<>f__this.TableGridCol3.GetEnumerator();
                    try
                    {
                        while (this.<$s_747>__4.MoveNext())
                        {
                            this.<i>__5 = this.<$s_747>__4.Current;
                            this.<i>__5.Model.SetUnkown();
                        }
                    }
                    finally
                    {
                        if (this.<$s_747>__4 == null)
                        {
                        }
                        this.<$s_747>__4.Dispose();
                    }
                    this.<cell>__6 = XSingleton<LifeSkillManager>.Singleton.GetCellInfo(this.<>f__this.CurrentType, (int) this.<>f__this.ColIndex);
                    if (((this.<cell>__6 != null) && (this.<cell>__6.RandInfo != null)) && (this.<cell>__6.RandInfo.Count == 3))
                    {
                        this.<>f__this.HavResult = true;
                        this.<config>__7 = new List<new_life_skill_rand_config>(0x80);
                        this.<configs>__8 = ConfigMgr.getInstance().getList<new_life_skill_rand_config>();
                        this.<$s_748>__9 = this.<configs>__8.GetEnumerator();
                        try
                        {
                            while (this.<$s_748>__9.MoveNext())
                            {
                                this.<i>__10 = this.<$s_748>__9.Current;
                                this.<config>__7.Add(this.<i>__10 as new_life_skill_rand_config);
                            }
                        }
                        finally
                        {
                            IDisposable disposable = this.<$s_748>__9 as IDisposable;
                            if (disposable == null)
                            {
                            }
                            disposable.Dispose();
                        }
                        this.<$s_749>__11 = this.<>f__this.TableGridCol1.GetEnumerator();
                        try
                        {
                            while (this.<$s_749>__11.MoveNext())
                            {
                                this.<i>__12 = this.<$s_749>__11.Current;
                                this.<i>__12.Model.RandConfig = GRandomer.RandomList<new_life_skill_rand_config>(this.<config>__7);
                                this.<i>__12.Model.OnPress = null;
                            }
                        }
                        finally
                        {
                            if (this.<$s_749>__11 == null)
                            {
                            }
                            this.<$s_749>__11.Dispose();
                        }
                        this.<$s_750>__13 = this.<>f__this.TableGridCol2.GetEnumerator();
                        try
                        {
                            while (this.<$s_750>__13.MoveNext())
                            {
                                this.<i>__14 = this.<$s_750>__13.Current;
                                this.<i>__14.Model.RandConfig = GRandomer.RandomList<new_life_skill_rand_config>(this.<config>__7);
                                this.<i>__14.Model.OnPress = null;
                            }
                        }
                        finally
                        {
                            if (this.<$s_750>__13 == null)
                            {
                            }
                            this.<$s_750>__13.Dispose();
                        }
                        this.<$s_751>__15 = this.<>f__this.TableGridCol3.GetEnumerator();
                        try
                        {
                            while (this.<$s_751>__15.MoveNext())
                            {
                                this.<i>__16 = this.<$s_751>__15.Current;
                                this.<i>__16.Model.RandConfig = GRandomer.RandomList<new_life_skill_rand_config>(this.<config>__7);
                                this.<i>__16.Model.OnPress = null;
                            }
                        }
                        finally
                        {
                            if (this.<$s_751>__15 == null)
                            {
                            }
                            this.<$s_751>__15.Dispose();
                        }
                        this.<config1>__17 = this.<config>__7.SingleOrDefault<new_life_skill_rand_config>(new Func<new_life_skill_rand_config, bool>(this.<>m__41C));
                        this.<config2>__18 = this.<config>__7.SingleOrDefault<new_life_skill_rand_config>(new Func<new_life_skill_rand_config, bool>(this.<>m__41D));
                        this.<config3>__19 = this.<config>__7.SingleOrDefault<new_life_skill_rand_config>(new Func<new_life_skill_rand_config, bool>(this.<>m__41E));
                        this.<>f__this.TableGridCol1[1].Model.RandConfig = this.<config1>__17;
                        this.<>f__this.TableGridCol2[1].Model.RandConfig = this.<config2>__18;
                        this.<>f__this.TableGridCol3[1].Model.RandConfig = this.<config3>__19;
                        this.<>f__this.TableGridCol1[1].Model.OnPress = new Action<bool, new_life_skill_rand_config>(this.<>m__41F);
                        this.<>f__this.TableGridCol2[1].Model.OnPress = new Action<bool, new_life_skill_rand_config>(this.<>m__420);
                        this.<>f__this.TableGridCol3[1].Model.OnPress = new Action<bool, new_life_skill_rand_config>(this.<>m__421);
                    }
                    this.<>f__this.To1Pos = new Vector3(this.<>f__this.Zero1Pos.x, this.<>f__this.Zero1Pos.y + ((this.<>f__this.TableGridCol1.Count - 2) * this.<>f__this.CellHeight), 0f);
                    this.<>f__this.To2Pos = new Vector3(this.<>f__this.Zero2Pos.x, this.<>f__this.Zero2Pos.y + ((this.<>f__this.TableGridCol2.Count - 2) * this.<>f__this.CellHeight), 0f);
                    this.<>f__this.To3Pos = new Vector3(this.<>f__this.Zero3Pos.x, this.<>f__this.Zero3Pos.y + ((this.<>f__this.TableGridCol3.Count - 2) * this.<>f__this.CellHeight), 0f);
                    this.<>f__this.GridCol1.transform.localPosition = this.<>f__this.To1Pos;
                    this.<>f__this.GridCol2.transform.localPosition = this.<>f__this.To2Pos;
                    this.<>f__this.GridCol3.transform.localPosition = this.<>f__this.To3Pos;
                    this.<index>__20 = 0;
                    break;
                }
                case 1:
                    this.<>f__this.GridCol1.transform.localPosition = this.<>f__this.To1Pos;
                    this.<>f__this.GridCol2.transform.localPosition = this.<>f__this.To2Pos;
                    this.<>f__this.GridCol3.transform.localPosition = this.<>f__this.To3Pos;
                    if (this.<index>__20++ <= 10)
                    {
                        break;
                    }
                    this.<>f__this.UpdateButtons();
                    this.$PC = -1;
                    goto Label_073C;

                default:
                    goto Label_073C;
            }
            this.$current = new WaitForEndOfFrame();
            this.$PC = 1;
            return true;
        Label_073C:
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
    private sealed class <ShowResult>c__AnonStorey214
    {
        internal LifeSkillSoltPanel <>f__this;
        internal List<NewLifeSkillRandInfo> list;

        internal bool <>m__415(new_life_skill_rand_config t)
        {
            return (t.entry == this.list[0].entry);
        }

        internal bool <>m__416(new_life_skill_rand_config t)
        {
            return (t.entry == this.list[1].entry);
        }

        internal bool <>m__417(new_life_skill_rand_config t)
        {
            return (t.entry == this.list[2].entry);
        }

        internal void <>m__418(bool ispress, new_life_skill_rand_config item)
        {
            this.<>f__this.OnTipShow(item, ispress, 1);
        }

        internal void <>m__419(bool ispress, new_life_skill_rand_config item)
        {
            this.<>f__this.OnTipShow(item, ispress, 2);
        }

        internal void <>m__41A(bool ispress, new_life_skill_rand_config item)
        {
            this.<>f__this.OnTipShow(item, ispress, 3);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowSolt>c__AnonStorey213
    {
        internal LifeSkillSoltPanel <>f__this;
        internal bool free;

        internal void <>m__414(GameObject go)
        {
            SocketMgr.Instance.RequestC2S_NewLifeSkillRandReward(this.<>f__this.CurrentType, this.<>f__this.ColIndex, !this.free ? 1 : 0);
        }
    }

    public class GridCol1ItemModel : TableItemModel<LifeSkillSoltPanel.GridCol1ItemTemplate>
    {
        private new_life_skill_rand_config _RandConfig;
        public Action<bool, new_life_skill_rand_config> OnPress;

        public override void Init(LifeSkillSoltPanel.GridCol1ItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            base.Item.Root.OnUIMousePress(delegate (bool flag, object u) {
                if (this.OnPress != null)
                {
                    this.OnPress(flag, this._RandConfig);
                }
            });
        }

        public void SetUnkown()
        {
            this._RandConfig = null;
            base.Template.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Skill_Icon_unknown");
            base.Template.Part.enabled = false;
            base.Template.Num.text = string.Empty;
            CommonFunc.SetEquipQualityBorder(base.Template.QualityBorder, 0, false);
            base.Template.QualityBorder.ActiveSelfObject(true);
            base.Template.Background.ActiveSelfObject(true);
        }

        public new_life_skill_rand_config RandConfig
        {
            get
            {
                return this._RandConfig;
            }
            set
            {
                this._RandConfig = value;
                if (value != null)
                {
                    bool isHead = false;
                    base.Template.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(value.icon, out isHead);
                    int num2 = !isHead ? 80 : 0x5f;
                    base.Template.Icon.height = num2;
                    base.Template.Icon.width = num2;
                    base.Template.Icon.depth = !isHead ? 2 : 4;
                    CommonFunc.SetEquipQualityBorder(base.Template.QualityBorder, value.quality, false);
                    base.Template.Part.enabled = value.IsPart == 1;
                    base.Template.QualityBorder.ActiveSelfObject(value.parameter1 == 1);
                    base.Template.Background.ActiveSelfObject(value.parameter1 != 0);
                    int result = 1;
                    char[] separator = new char[] { '|' };
                    string[] strArray = value.parameter_definition1.Split(separator);
                    if (strArray.Length == 2)
                    {
                        int.TryParse(strArray[1], out result);
                    }
                    base.Template.Num.text = string.Format("{0:0}", result);
                    base.Template.Num.ActiveSelfObject(result > 1);
                }
            }
        }
    }

    public class GridCol1ItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.Icon = base.FindChild<UITexture>("Icon");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.Background = base.FindChild<UISprite>("Background");
            this.Part = base.FindChild<UISprite>("Part");
            this.Num = base.FindChild<UILabel>("Num");
        }

        public UISprite Background { get; private set; }

        public UITexture Icon { get; private set; }

        public UILabel Num { get; private set; }

        public UISprite Part { get; private set; }

        public UISprite QualityBorder { get; private set; }
    }

    public class GridCol2ItemModel : TableItemModel<LifeSkillSoltPanel.GridCol2ItemTemplate>
    {
        private new_life_skill_rand_config _RandConfig;
        public Action<bool, new_life_skill_rand_config> OnPress;

        public override void Init(LifeSkillSoltPanel.GridCol2ItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            base.Item.Root.OnUIMousePress(delegate (bool flag, object u) {
                if (this.OnPress != null)
                {
                    this.OnPress(flag, this._RandConfig);
                }
            });
        }

        public void SetUnkown()
        {
            this._RandConfig = null;
            base.Template.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Skill_Icon_unknown");
            base.Template.Part.enabled = false;
            base.Template.Num.text = string.Empty;
            CommonFunc.SetEquipQualityBorder(base.Template.QualityBorder, 0, false);
            base.Template.QualityBorder.ActiveSelfObject(true);
            base.Template.Background.ActiveSelfObject(true);
        }

        public new_life_skill_rand_config RandConfig
        {
            get
            {
                return this._RandConfig;
            }
            set
            {
                this._RandConfig = value;
                if (value != null)
                {
                    bool isHead = false;
                    base.Template.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(value.icon, out isHead);
                    int num2 = !isHead ? 80 : 0x5f;
                    base.Template.Icon.height = num2;
                    base.Template.Icon.width = num2;
                    base.Template.Icon.depth = !isHead ? 2 : 4;
                    CommonFunc.SetEquipQualityBorder(base.Template.QualityBorder, value.quality, false);
                    base.Template.QualityBorder.ActiveSelfObject(value.parameter1 == 1);
                    base.Template.Part.enabled = value.IsPart == 1;
                    base.Template.Background.ActiveSelfObject(value.parameter1 != 0);
                    int result = 1;
                    char[] separator = new char[] { '|' };
                    string[] strArray = value.parameter_definition1.Split(separator);
                    if (strArray.Length == 2)
                    {
                        int.TryParse(strArray[1], out result);
                    }
                    base.Template.Num.text = string.Format("{0:0}", result);
                    base.Template.Num.ActiveSelfObject(result > 1);
                }
            }
        }
    }

    public class GridCol2ItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.Icon = base.FindChild<UITexture>("Icon");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.Background = base.FindChild<UISprite>("Background");
            this.Part = base.FindChild<UISprite>("Part");
            this.Num = base.FindChild<UILabel>("Num");
        }

        public UISprite Background { get; private set; }

        public UITexture Icon { get; private set; }

        public UILabel Num { get; private set; }

        public UISprite Part { get; private set; }

        public UISprite QualityBorder { get; private set; }
    }

    public class GridCol3ItemModel : TableItemModel<LifeSkillSoltPanel.GridCol3ItemTemplate>
    {
        private new_life_skill_rand_config _RandConfig;
        public Action<bool, new_life_skill_rand_config> OnPress;

        public override void Init(LifeSkillSoltPanel.GridCol3ItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            base.Item.Root.OnUIMousePress(delegate (bool flag, object u) {
                if (this.OnPress != null)
                {
                    this.OnPress(flag, this._RandConfig);
                }
            });
        }

        public void SetUnkown()
        {
            this._RandConfig = null;
            base.Template.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Skill_Icon_unknown");
            base.Template.Part.enabled = false;
            base.Template.Num.text = string.Empty;
            CommonFunc.SetEquipQualityBorder(base.Template.QualityBorder, 0, false);
            base.Template.QualityBorder.ActiveSelfObject(true);
            base.Template.Background.ActiveSelfObject(true);
        }

        public new_life_skill_rand_config RandConfig
        {
            get
            {
                return this._RandConfig;
            }
            set
            {
                this._RandConfig = value;
                if (value != null)
                {
                    bool isHead = false;
                    base.Template.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(value.icon, out isHead);
                    int num2 = !isHead ? 80 : 0x5f;
                    base.Template.Icon.height = num2;
                    base.Template.Icon.width = num2;
                    base.Template.Icon.depth = !isHead ? 2 : 4;
                    base.Template.QualityBorder.ActiveSelfObject(value.parameter1 == 1);
                    if (value.quality >= 0)
                    {
                        CommonFunc.SetEquipQualityBorder(base.Template.QualityBorder, value.quality, false);
                    }
                    base.Template.Part.enabled = value.IsPart == 1;
                    base.Template.Background.ActiveSelfObject(value.parameter1 != 0);
                    int result = 1;
                    char[] separator = new char[] { '|' };
                    string[] strArray = value.parameter_definition1.Split(separator);
                    if (strArray.Length == 2)
                    {
                        int.TryParse(strArray[1], out result);
                    }
                    base.Template.Num.text = string.Format("{0:0}", result);
                    base.Template.Num.ActiveSelfObject(result > 1);
                }
            }
        }
    }

    public class GridCol3ItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.Icon = base.FindChild<UITexture>("Icon");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.Background = base.FindChild<UISprite>("Background");
            this.Part = base.FindChild<UISprite>("Part");
            this.Num = base.FindChild<UILabel>("Num");
        }

        public UISprite Background { get; private set; }

        public UITexture Icon { get; private set; }

        public UILabel Num { get; private set; }

        public UISprite Part { get; private set; }

        public UISprite QualityBorder { get; private set; }
    }

    public class StarGridItemModel : TableItemModel<LifeSkillSoltPanel.StarGridItemTemplate>
    {
        public override void Init(LifeSkillSoltPanel.StarGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }
    }

    public class StarGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
        }
    }
}

