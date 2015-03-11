using FastBuf;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class BagPanel : GUIEntity
{
    private Transform _InfoPanel;
    [CompilerGenerated]
    private static Comparison<Item> <>f__am$cache11;
    [CompilerGenerated]
    private static Func<Item, BagItemData> <>f__am$cache12;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache13;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache14;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache15;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache16;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache17;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache18;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache19;
    [CompilerGenerated]
    private static Comparison<Item> <>f__am$cache1A;
    [CompilerGenerated]
    private static Func<Item, BagItemData> <>f__am$cache1B;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache1C;
    private Transform AllList;
    private Transform DetailsBtn;
    private Transform GemList;
    private bool isPressTabBtn;
    public UITableManager<BagTableItem> ItemGemTable = new UITableManager<BagTableItem>();
    public UITableManager<BagTableItem> ItemTable = new UITableManager<BagTableItem>();
    public BagTableItem mCurrSelectItemEntry;
    public int mCurrSelectItemID = -1;
    private bool mDataIsDirty;
    public BagTableItem mFirstItemEntry;
    public bool mFirstShowGemTable;
    private bool mIsFirstInit = true;
    private bool mIsMsgBack;
    private Dictionary<int, Transform> mItemGirdDict = new Dictionary<int, Transform>();
    public ShowItemType mShowItemType;
    private bool mUseItemSending;

    public void ClearIconCache()
    {
        IEnumerator<BagTableItem> enumerator = this.ItemTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                enumerator.Current.Clear();
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        IEnumerator<BagTableItem> enumerator2 = this.ItemGemTable.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                enumerator2.Current.Clear();
            }
        }
        finally
        {
            if (enumerator2 == null)
            {
            }
            enumerator2.Dispose();
        }
    }

    private void ClickOutlandShop()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Outland);
        if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_Outland))
        {
            if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().outlan)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().outlan));
            }
            else
            {
                if (<>f__am$cache19 == null)
                {
                    <>f__am$cache19 = delegate (GUIEntity obj) {
                        ((ShopPanel) obj).SetShopType(ShopCoinType.OutLandCoin);
                        SocketMgr.Instance.RequestOutlandShopInfo();
                    };
                }
                GUIMgr.Instance.PushGUIEntity("ShopPanel", <>f__am$cache19);
            }
        }
    }

    public void ClickYuanZheng()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Expedition);
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().flamebattle)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().flamebattle));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_FlameBattle))
        {
            YuanZhengPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<YuanZhengPanel>();
            if ((gUIEntity != null) && gUIEntity.Hidden)
            {
                if (<>f__am$cache13 == null)
                {
                    <>f__am$cache13 = obj => SocketMgr.Instance.RequestFlameBattleInfo();
                }
                GUIMgr.Instance.PushGUIEntity("YuanZhengPanel", <>f__am$cache13);
            }
            else
            {
                if (<>f__am$cache14 == null)
                {
                    <>f__am$cache14 = delegate (GUIEntity obj) {
                    };
                }
                GUIMgr.Instance.PushGUIEntity("YuanZhengPanel", <>f__am$cache14);
            }
        }
    }

    public void ClosePanel()
    {
        this.ClearIconCache();
        GUIMgr.Instance.PopGUIEntity();
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
        BundleMgr.Instance.ClearCache();
    }

    public void CreateItemList(bool resetClip = true)
    {
        if (resetClip)
        {
            this.ResetClipViewport(false);
        }
        List<Item> itemListByType = ActorData.getInstance().GetItemListByType(ShowItemType.All);
        if (<>f__am$cache11 == null)
        {
            <>f__am$cache11 = delegate (Item item1, Item item2) {
                int num = (item1.entry != 700) ? 0 : 1;
                int num2 = (item2.entry != 700) ? 0 : 1;
                if (num != num2)
                {
                    return num2 - num;
                }
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item1.entry);
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(item2.entry);
                if (_config == null)
                {
                    return 0;
                }
                if (_config2 == null)
                {
                    return 0;
                }
                int num3 = (_config.type != 4) ? 0 : 1;
                int num4 = (_config2.type != 4) ? 0 : 1;
                if (num3 != num4)
                {
                    return num4 - num3;
                }
                int num5 = (_config.type != 6) ? 0 : 1;
                int num6 = (_config2.type != 6) ? 0 : 1;
                if (num5 != num6)
                {
                    return num6 - num5;
                }
                int num7 = (_config.type != 2) ? 0 : 1;
                int num8 = (_config2.type != 2) ? 0 : 1;
                if (num7 != num8)
                {
                    return num8 - num7;
                }
                return _config2.quality - _config.quality;
            };
        }
        itemListByType.Sort(<>f__am$cache11);
        if (<>f__am$cache12 == null)
        {
            <>f__am$cache12 = t => new BagItemData { Data = t, Config = ConfigMgr.getInstance().getByEntry<item_config>(t.entry) };
        }
        BagItemData[] dataArray = itemListByType.Select<Item, BagItemData>(<>f__am$cache12).ToArray<BagItemData>();
        this.ItemTable.Count = dataArray.Length;
        for (int i = 0; i < dataArray.Length; i++)
        {
            BagTableItem item = this.ItemTable[i];
            item.ItemData = dataArray[i];
            item.OnClick = new Action<BagTableItem>(this.OnClickItemBtn);
        }
        if (this.ItemTable.Count < 1)
        {
            this._InfoPanel.gameObject.SetActive(false);
        }
        this.mIsFirstInit = false;
    }

    public void ForceSwitchXiaohao()
    {
        base.FindChild<UIToggle>("XiaoHaoBtn").value = true;
        base.FindChild<UIToggle>("CaiLiaoBtn").value = false;
        base.FindChild<UIToggle>("AllBtn").value = false;
        base.FindChild<UIToggle>("QiTaBtn").value = false;
        this.OnClickXiaoHaoBtn();
    }

    private void FunctionBuildingResponse(GuideEvent _event)
    {
        if (GuideSystem.MatchEvent(_event))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Function.tag_function_select_building, null);
        }
    }

    private void InitGuiControlEvent()
    {
        UIEventListener.Get(base.transform.FindChild("InfoPanel/SellBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSellBtn);
        this._InfoPanel = base.transform.FindChild("InfoPanel");
    }

    public void InitItemList(ShowItemType _Type)
    {
        this.mShowItemType = _Type;
        this.ShowTab(_Type);
    }

    private void OnClickAllBtn()
    {
        if (this.mShowItemType != ShowItemType.All)
        {
            this.isPressTabBtn = true;
            this.InitItemList(ShowItemType.All);
            this.isPressTabBtn = false;
        }
    }

    private void OnClickCaiLiaoBtn()
    {
        if (this.mShowItemType != ShowItemType.Material)
        {
            this.isPressTabBtn = true;
            this.InitItemList(ShowItemType.Material);
            this.isPressTabBtn = false;
        }
    }

    private void OnClickCombBtn(GameObject go)
    {
        if (this.mCurrSelectItemEntry != null)
        {
            GUIMgr.Instance.DoModelGUI("FragmentCombinePanel", delegate (GUIEntity obj) {
                FragmentCombinePanel panel = (FragmentCombinePanel) obj;
                panel.Depth = 300;
                panel.UpdateData(this.mCurrSelectItemEntry.ItemData.Data);
            }, null);
        }
    }

    private void OnClickEquipBtn()
    {
        if (this.mShowItemType != ShowItemType.Equip)
        {
            this.isPressTabBtn = true;
            this.InitItemList(ShowItemType.Equip);
            this.isPressTabBtn = false;
        }
    }

    private void OnClickFromBtn(GameObject go)
    {
        <OnClickFromBtn>c__AnonStorey177 storey = new <OnClickFromBtn>c__AnonStorey177();
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storey.info = obj2 as Item;
            if (storey.info != null)
            {
                ActorData.getInstance().IsPopPanel = true;
                GUIMgr.Instance.PushGUIEntity("DetailsPanel", new Action<GUIEntity>(storey.<>m__1B7));
            }
        }
    }

    private void OnClickGemBtn()
    {
        if (this.mShowItemType != ShowItemType.Gem)
        {
            this.isPressTabBtn = true;
            this.InitItemList(ShowItemType.Gem);
            this.isPressTabBtn = false;
        }
    }

    private void OnClickGongNengBtn()
    {
        if (this.mShowItemType != ShowItemType.Ticket)
        {
            this.isPressTabBtn = true;
            this.InitItemList(ShowItemType.Ticket);
            this.isPressTabBtn = false;
        }
    }

    private void OnClickItemBtn(BagTableItem item)
    {
        Item data = item.ItemData.Data;
        this.SetItemDetails(item);
        Debug.Log(data.entry + ":" + data.num);
        SoundManager.mInstance.PlaySFX("sound_ui_t_8");
    }

    private void OnClickJumpBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            Item item = obj2 as Item;
            if (item != null)
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
                if (_config != null)
                {
                    bool flag = false;
                    variable_config _config2 = ConfigMgr.getInstance().getByEntry<variable_config>(0);
                    if (_config2 != null)
                    {
                        flag = _config2.open_jump == 1;
                    }
                    switch (((TicketType) _config.ticket_sub_type))
                    {
                        case TicketType.Ticket_Arena_Shop_Refresh:
                            if (!flag)
                            {
                                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x857), ConfigMgr.getInstance().GetWord(0x863)));
                            }
                            else
                            {
                                this.OpenArenaLadderShop();
                            }
                            break;

                        case TicketType.Ticket_Arena_Shop_Buy:
                        case TicketType.Ticket_Arena_Reset:
                            if (!flag)
                            {
                                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x857), ConfigMgr.getInstance().GetWord(0x862)));
                            }
                            else
                            {
                                this.OpenArenaLadder();
                            }
                            break;

                        case TicketType.Ticket_SoulBox:
                            if (!flag)
                            {
                                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x857), ConfigMgr.getInstance().GetWord(0x864)));
                            }
                            else
                            {
                                this.OpenSoulBox();
                            }
                            break;

                        case TicketType.Ticket_Flame_Shop_Refresh:
                            if (!flag)
                            {
                                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x857), ConfigMgr.getInstance().GetWord(0x868)));
                            }
                            else
                            {
                                this.OpenFlameShop();
                            }
                            break;

                        case TicketType.Ticket_Gold_Draw:
                        case TicketType.Ticket_Gold_Ten_Draw:
                        case TicketType.Ticket_Stone_Draw:
                        case TicketType.Ticket_Stone_Ten_Draw:
                            if (!flag)
                            {
                                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x857), ConfigMgr.getInstance().GetWord(0x858)));
                                break;
                            }
                            this.OpenRecruitPanel();
                            break;

                        case TicketType.Ticket_Outland_Shop_Refresh:
                            if (!flag)
                            {
                                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x857), ConfigMgr.getInstance().GetWord(0x85a)));
                                break;
                            }
                            this.ClickOutlandShop();
                            break;

                        case TicketType.Ticket_Flame_Refresh:
                            if (!flag)
                            {
                                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x857), ConfigMgr.getInstance().GetWord(0x85c)));
                                break;
                            }
                            this.ClickYuanZheng();
                            break;
                    }
                    ActorData.getInstance().IsPopPanel = true;
                }
            }
        }
    }

    private void OnClickQiTaBtn()
    {
        if (this.mShowItemType != ShowItemType.QiTa)
        {
            this.isPressTabBtn = true;
            this.InitItemList(ShowItemType.QiTa);
            this.isPressTabBtn = false;
        }
    }

    private void OnClickSellBtn(GameObject go)
    {
        if (this.mCurrSelectItemEntry != null)
        {
            if (this.mCurrSelectItemEntry.ItemData.Config.sell_price > 0)
            {
                GUIMgr.Instance.DoModelGUI("SellPanel", delegate (GUIEntity obj) {
                    SellPanel panel = (SellPanel) obj;
                    panel.Depth = 500;
                    panel.SetSellItemData(this.mCurrSelectItemEntry.ItemData.Data);
                }, base.gameObject);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x8a3));
            }
        }
    }

    private void OnClickUesItemBtn(GameObject go)
    {
        if ((this.mCurrSelectItemEntry != null) && !this.mUseItemSending)
        {
            this.mUseItemSending = true;
            if (this.mCurrSelectItemEntry.ItemData.Config.type == 8)
            {
                SocketMgr.Instance.RequestUseItem(-1L, this.mCurrSelectItemEntry.ItemData.Data.entry, 1, -1);
            }
            else if (this.mCurrSelectItemEntry.ItemData.Config.type == 4)
            {
                if (GUIMgr.Instance.GetActivityGUIEntity<HeroListPanel>() == null)
                {
                    GUIMgr.Instance.PushGUIEntity("HeroListPanel", obj => ((HeroListPanel) obj).InitHeroList(this.mCurrSelectItemEntry.ItemData.Data.entry));
                }
            }
            else if (this.mCurrSelectItemEntry.ItemData.Config.type == 9)
            {
                SocketMgr.Instance.RequestUseItem(-1L, this.mCurrSelectItemEntry.ItemData.Data.entry, 1, -1);
            }
        }
    }

    private void OnClickXiaoHaoBtn()
    {
        if (this.mShowItemType != ShowItemType.Consumable)
        {
            this.isPressTabBtn = true;
            this.InitItemList(ShowItemType.Consumable);
            this.isPressTabBtn = false;
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.mUseItemSending = false;
        if (!ActorData.getInstance().IsPopPanel)
        {
            this.UpdateData(true);
        }
        ActorData.getInstance().IsPopPanel = false;
        GUIMgr.Instance.FloatTitleBar();
        GuideSystem.FireEvent(GuideEvent.Medicine_Using);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        this.DetailsBtn = base.transform.FindChild("InfoPanel/DetailsBtn");
        UIGrid grid = base.FindChild<UIGrid>("List/Grid");
        this.ItemTable.InitFromGrid(grid);
        grid = base.FindChild<UIGrid>("ListGem/Grid");
        this.ItemGemTable.InitFromGrid(grid);
        this.AllList = base.transform.FindChild("List");
        this.GemList = base.transform.FindChild("ListGem");
        this.InitGuiControlEvent();
        base.FindChild<UIToggle>("CaiLiaoBtn").OnUIMouseClick(u => this.OnClickCaiLiaoBtn());
        base.FindChild<UIToggle>("XiaoHaoBtn").OnUIMouseClick(u => this.OnClickXiaoHaoBtn());
        base.FindChild<UIToggle>("AllBtn").OnUIMouseClick(u => this.OnClickAllBtn());
        base.FindChild<UIToggle>("QiTaBtn").OnUIMouseClick(u => this.OnClickQiTaBtn());
        base.FindChild<UIToggle>("GongNengBtn").OnUIMouseClick(u => this.OnClickGongNengBtn());
        base.FindChild<UIToggle>("GemBtn").OnUIMouseClick(u => this.OnClickGemBtn());
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
        this.mCurrSelectItemID = -1;
        this.mFirstShowGemTable = false;
    }

    private void OpenArenaLadder()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Arena);
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().arena_ladder)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().arena_ladder));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_ArenaLadder))
        {
            ArenaLadderPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ArenaLadderPanel>();
            if ((gUIEntity != null) && gUIEntity.Hidden)
            {
                if (<>f__am$cache17 == null)
                {
                    <>f__am$cache17 = go => SocketMgr.Instance.RequestArenaLadderInfo();
                }
                GUIMgr.Instance.PushGUIEntity("ArenaLadderPanel", <>f__am$cache17);
            }
            else
            {
                GUIMgr.Instance.PushGUIEntity("ArenaLadderPanel", null);
            }
        }
    }

    private void OpenArenaLadderShop()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Arena);
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().arena_ladder)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().arena_ladder));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_ArenaLadder))
        {
            if (<>f__am$cache18 == null)
            {
                <>f__am$cache18 = delegate (GUIEntity obj) {
                    ((ShopPanel) obj).SetShopType(ShopCoinType.ArenaLadderCoin);
                    SocketMgr.Instance.RequestArenaLadderShopInfo();
                };
            }
            GUIMgr.Instance.PushGUIEntity("ShopPanel", <>f__am$cache18);
        }
    }

    private void OpenFlameShop()
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().flamebattle)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().flamebattle));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_FlameBattle))
        {
            if (<>f__am$cache15 == null)
            {
                <>f__am$cache15 = delegate (GUIEntity obj) {
                    ((ShopPanel) obj).SetShopType(ShopCoinType.YuanZhengCoin);
                    SocketMgr.Instance.RequestFlameBattleShopInfo();
                };
            }
            GUIMgr.Instance.PushGUIEntity("ShopPanel", <>f__am$cache15);
        }
    }

    private void OpenRecruitPanel()
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().pub)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().pub));
        }
        else
        {
            if (GuideSystem.MatchEvent(GuideEvent.GoldRecruit) || GuideSystem.MatchEvent(GuideEvent.StoneRecruit))
            {
                GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Recruit.tag_recruit_select_building, null);
            }
            GUIMgr.Instance.PushGUIEntity("RecruitPanel", null);
        }
    }

    private void OpenSoulBox()
    {
        if (SoulBox.FuncShowable())
        {
            GUIMgr.Instance.DoModelGUI<SoulBox>(null, null);
        }
        else
        {
            if (<>f__am$cache16 == null)
            {
                <>f__am$cache16 = delegate (GUIEntity e) {
                    MessageBox box = e as MessageBox;
                    if (<>f__am$cache1C == null)
                    {
                        <>f__am$cache1C = uigo => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                    }
                    box.SetDialog(ConfigMgr.getInstance().GetWord(0x26b6), <>f__am$cache1C, null, false);
                };
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache16, null);
        }
    }

    private void ResetClipViewport(bool beGem = false)
    {
        GUIMgr.Instance.ListRoot.gameObject.SetActive(true);
        Transform top = base.transform.FindChild("ListTopLeft");
        Transform bottom = base.transform.FindChild("ListBottomRight");
        Transform bounds = base.transform.FindChild("List");
        if (beGem)
        {
            bounds = base.transform.FindChild("ListGem");
        }
        GUIMgr.Instance.ResetListViewpot(top, bottom, bounds, 0f);
    }

    public void SelectDefaultCheckbox()
    {
        base.transform.FindChild("ItemPanel/Tab/AllBtn").GetComponent<UIToggle>().value = true;
    }

    public void SelectGemCheckbox()
    {
        base.transform.FindChild("ItemPanel/Tab/GemBtn").GetComponent<UIToggle>().value = true;
    }

    private void SetCurrSelectItemEntry()
    {
        int num = 0;
        Item item = ActorData.getInstance().ItemList.Find(e => e.entry == this.mCurrSelectItemID);
        if (item != null)
        {
            num = item.num;
        }
        IEnumerator<BagTableItem> enumerator = this.ItemTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                BagTableItem current = enumerator.Current;
                if (current.ItemData.Data.entry == this.mCurrSelectItemID)
                {
                    Debug.Log("0000  " + this.mCurrSelectItemID);
                    this.mCurrSelectItemEntry = current;
                    return;
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
        if (this.mShowItemType == ShowItemType.Gem)
        {
            Debug.Log("1111  " + this.mCurrSelectItemID);
            if (this.ItemGemTable.Count > 0)
            {
                this.mCurrSelectItemEntry = this.ItemGemTable[0];
            }
        }
        else
        {
            Debug.Log("22222  " + this.mCurrSelectItemID);
            if (this.ItemTable.Count > 0)
            {
                this.mCurrSelectItemEntry = this.ItemTable[0];
            }
        }
    }

    private void SetItemDetails(BagTableItem tItem)
    {
        if (this.mCurrSelectItemEntry != null)
        {
            this.mCurrSelectItemEntry.Checked.ActiveSelfObject(false);
        }
        this.mCurrSelectItemEntry = tItem;
        this.mCurrSelectItemEntry.Checked.ActiveSelfObject(true);
        Item data = tItem.ItemData.Data;
        if (data != null)
        {
            this.mCurrSelectItemID = data.entry;
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(data.entry);
            if (_config != null)
            {
                this._InfoPanel.FindChild("Item/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                CommonFunc.SetEquipQualityBorder(this._InfoPanel.FindChild("Item/QualityBorder").GetComponent<UISprite>(), _config.quality, false);
                this._InfoPanel.FindChild("Info/Name").GetComponent<UILabel>().text = _config.name;
                this._InfoPanel.FindChild("Info/Count").GetComponent<UILabel>().text = data.num.ToString();
                this._InfoPanel.FindChild("Describe/Desc").GetComponent<UILabel>().text = _config.describe;
                this._InfoPanel.FindChild("SellInfo/Price").GetComponent<UILabel>().text = (_config.sell_price <= 0) ? ConfigMgr.getInstance().GetWord(0x8a2) : _config.sell_price.ToString();
                GUIDataHolder.setData(base.transform.FindChild("InfoPanel/SellBtn").gameObject, data);
                GUIDataHolder.setData(this.DetailsBtn.gameObject, data);
                UILabel component = this.DetailsBtn.transform.FindChild("Label").GetComponent<UILabel>();
                if (((_config.type == 4) || (_config.type == 8)) || (_config.type == 9))
                {
                    this.DetailsBtn.GetComponent<UIButton>().isEnabled = true;
                    component.text = ConfigMgr.getInstance().GetWord(0x273a);
                    UIEventListener.Get(this.DetailsBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickUesItemBtn);
                }
                else if (_config.type == 10)
                {
                    component.text = ConfigMgr.getInstance().GetWord(0x273b);
                    variable_config _config2 = ConfigMgr.getInstance().getByEntry<variable_config>(0);
                    if (_config2 == null)
                    {
                        return;
                    }
                    if (_config.quality >= _config2.cardgem_maxquality)
                    {
                        this.DetailsBtn.GetComponent<UIButton>().isEnabled = false;
                    }
                    else
                    {
                        this.DetailsBtn.GetComponent<UIButton>().isEnabled = true;
                        UIEventListener.Get(this.DetailsBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCombBtn);
                    }
                }
                else if (_config.type == 11)
                {
                    this.DetailsBtn.GetComponent<UIButton>().isEnabled = true;
                    component.text = ConfigMgr.getInstance().GetWord(0x273c);
                    UIEventListener.Get(this.DetailsBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickJumpBtn);
                }
                else
                {
                    this.DetailsBtn.GetComponent<UIButton>().isEnabled = true;
                    component.text = ConfigMgr.getInstance().GetWord(0x2739);
                    UIEventListener.Get(this.DetailsBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickFromBtn);
                }
                if (!this._InfoPanel.active)
                {
                    this._InfoPanel.gameObject.SetActive(true);
                }
            }
        }
    }

    private void SetItemDragEnabled(Transform grid, bool isEnabled)
    {
        IEnumerator enumerator = grid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                for (int i = 0; i < 4; i++)
                {
                    current.transform.FindChild("Item" + (i + 1)).GetComponent<UIDragScrollView>().enabled = isEnabled;
                }
                current.FindChild("BGcollider").GetComponent<UIDragScrollView>().enabled = isEnabled;
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
    }

    private void ShowGemTab()
    {
        this.AllList.gameObject.SetActive(false);
        List<Item> itemListByType = ActorData.getInstance().GetItemListByType(ShowItemType.Gem);
        if (<>f__am$cache1A == null)
        {
            <>f__am$cache1A = delegate (Item item1, Item item2) {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item1.entry);
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(item2.entry);
                if (_config == null)
                {
                    return 0;
                }
                if (_config2 == null)
                {
                    return 0;
                }
                if (_config.sub_type != _config2.sub_type)
                {
                    return _config.sub_type - _config2.sub_type;
                }
                if (_config.quality != _config2.quality)
                {
                    return _config2.quality - _config.quality;
                }
                return 1;
            };
        }
        itemListByType.Sort(<>f__am$cache1A);
        if (<>f__am$cache1B == null)
        {
            <>f__am$cache1B = t => new BagItemData { Data = t, Config = ConfigMgr.getInstance().getByEntry<item_config>(t.entry) };
        }
        BagItemData[] dataArray = itemListByType.Select<Item, BagItemData>(<>f__am$cache1B).ToArray<BagItemData>();
        this.ItemGemTable.Count = dataArray.Length;
        for (int i = 0; i < dataArray.Length; i++)
        {
            BagTableItem item = this.ItemGemTable[i];
            item.ItemData = dataArray[i];
            item.OnClick = new Action<BagTableItem>(this.OnClickItemBtn);
        }
        if (this.ItemGemTable.Count < 1)
        {
            this._InfoPanel.gameObject.SetActive(false);
        }
        IEnumerator<BagTableItem> enumerator = this.ItemGemTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                BagTableItem current = enumerator.Current;
                if (current.ItemData.Config == null)
                {
                    current.DoHide(true);
                }
                else
                {
                    current.Drag.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
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
        if ((this.mCurrSelectItemEntry != null) && !this.isPressTabBtn)
        {
            this.SetCurrSelectItemEntry();
            this.SetItemDetails(this.mCurrSelectItemEntry);
        }
        else if (this.ItemGemTable.Count > 0)
        {
            this.SetItemDetails(this.ItemGemTable[0]);
        }
        bool flag = this.ItemGemTable.Count <= 0x10;
        for (int j = 0; j < this.ItemGemTable.Count; j++)
        {
            BagTableItem item3 = this.ItemGemTable[j];
            if ((item3.Drag != null) && flag)
            {
                item3.Drag.draggableCamera = null;
            }
        }
        this.ItemGemTable.RepositionLayout();
        if (this.isPressTabBtn)
        {
            this.ResetClipViewport(true);
        }
        this._InfoPanel.gameObject.SetActive(this.ItemGemTable.Count > 0);
    }

    private void ShowTab(ShowItemType mShowItemType)
    {
        Debug.Log(mShowItemType);
        this.mUseItemSending = false;
        bool flag = true;
        if (mShowItemType == ShowItemType.Gem)
        {
            if (this.mFirstShowGemTable)
            {
                this.mFirstShowGemTable = false;
            }
            else
            {
                this.GemList.gameObject.SetActive(true);
                this.ShowGemTab();
            }
        }
        else
        {
            bool flag2 = false;
            this.GemList.gameObject.SetActive(false);
            this.AllList.gameObject.SetActive(true);
            IEnumerator<BagTableItem> enumerator = this.ItemTable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    BagTableItem current = enumerator.Current;
                    item_config config = current.ItemData.Config;
                    if (config == null)
                    {
                        current.DoHide(true);
                        continue;
                    }
                    switch (mShowItemType)
                    {
                        case ShowItemType.All:
                            current.DoHide(((((config.type != 1) && (config.type != 4)) && ((config.type != 5) && (config.type != 7))) && (((config.type != 8) && (config.type != 11)) && ((config.type != 6) && (config.type != 9)))) && (config.type != 10));
                            break;

                        case ShowItemType.Material:
                            current.DoHide(config.type != 1);
                            break;

                        case ShowItemType.Consumable:
                            current.DoHide((((config.type != 4) && (config.type != 7)) && ((config.type != 8) && (config.type != 6))) && (config.type != 9));
                            break;

                        case ShowItemType.QiTa:
                            current.DoHide(config.type != 5);
                            break;

                        case ShowItemType.Gem:
                            current.DoHide(config.type != 10);
                            break;

                        case ShowItemType.Ticket:
                            current.DoHide(config.type != 11);
                            break;
                    }
                    if (((this.mCurrSelectItemEntry != null) && (this.mCurrSelectItemEntry.ItemData.Data.entry == current.ItemData.Data.entry)) && !current.Hide)
                    {
                        flag2 = true;
                    }
                    if (flag && current.Root.gameObject.activeSelf)
                    {
                        this.mFirstItemEntry = current;
                        flag = false;
                    }
                    current.Drag.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            if (((this.mCurrSelectItemEntry != null) && flag2) && !this.isPressTabBtn)
            {
                this.SetCurrSelectItemEntry();
                this.SetItemDetails(this.mCurrSelectItemEntry);
            }
            else if (flag)
            {
                this._InfoPanel.gameObject.SetActive(false);
            }
            else
            {
                this.SetItemDetails(this.mFirstItemEntry);
            }
            int totalCountNoHide = this.ItemTable.TotalCountNoHide;
            for (int i = 0; i < this.ItemTable.Count; i++)
            {
                BagTableItem item2 = this.ItemTable[i];
                if ((totalCountNoHide <= 0x10) && (item2.Drag != null))
                {
                    item2.Drag.draggableCamera = null;
                }
            }
            this.ItemTable.RepositionLayout();
            if (this.isPressTabBtn)
            {
                this.ResetClipViewport(false);
            }
        }
    }

    public void UpdateData(bool resetClip = true)
    {
        this.mIsMsgBack = true;
        this.CreateItemList(resetClip);
        this.ShowTab(this.mShowItemType);
    }

    internal void UpdateData(List<Item> list)
    {
        UITableManager<BagTableItem> itemTable = this.ItemTable;
        if (this.mShowItemType == ShowItemType.Gem)
        {
            itemTable = this.ItemGemTable;
        }
        bool flag = false;
        IEnumerator<BagTableItem> enumerator = itemTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                BagTableItem current = enumerator.Current;
                foreach (Item item2 in list)
                {
                    if (current.ItemData.Data.entry == item2.entry)
                    {
                        current.ItemData.Data.num = item2.num;
                        current.ItemData = current.ItemData;
                        if (this.mCurrSelectItemEntry.ItemData.Data.entry == item2.entry)
                        {
                            if (item2.num == 0)
                            {
                                BagTableItem fristItemNoHide = itemTable.FristItemNoHide;
                                if (fristItemNoHide == null)
                                {
                                    this._InfoPanel.gameObject.SetActive(false);
                                }
                                else
                                {
                                    this.SetItemDetails(fristItemNoHide);
                                }
                                flag = true;
                            }
                            else
                            {
                                this.SetItemDetails(current);
                            }
                        }
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
        bool flag2 = itemTable.Count <= 0x10;
        if ((itemTable.Count == 0x11) && flag)
        {
            flag2 = true;
        }
        for (int i = 0; i < itemTable.Count; i++)
        {
            BagTableItem item4 = itemTable[i];
            if ((item4.Drag != null) && flag2)
            {
                item4.Drag.draggableCamera = null;
            }
        }
        itemTable.RepositionLayout();
    }

    public void UpdateUIInfo(ShowItemType mShowItemType, bool resetClip = true)
    {
        this.CreateItemList(resetClip);
        this.ShowTab(mShowItemType);
    }

    [CompilerGenerated]
    private sealed class <OnClickFromBtn>c__AnonStorey177
    {
        internal Item info;

        internal void <>m__1B7(GUIEntity obj)
        {
            ((DetailsPanel) obj).InitItemDetails(this.info, EnterDupType.From_BagDetails);
        }
    }

    public class BagItemData
    {
        public item_config Config { get; set; }

        public Item Data { get; set; }
    }

    public class BagTableItem : UITableItem
    {
        private BagPanel.BagItemData _item;
        public UISprite Checked;
        private UISprite Chip;
        private UILabel Count;
        public UIDragCamera Drag;
        private UITexture Icon;
        private Transform Item1;
        public Action<BagPanel.BagTableItem> OnClick;
        private UISprite QualityBorder;

        internal void Clear()
        {
            this.Icon.mainTexture = null;
        }

        public void DoHide(bool flag)
        {
            if (this._item.Data.num == 0)
            {
                flag = true;
            }
            base.Hide = flag;
        }

        public override void OnCreate()
        {
            this.Icon = base.Root.FindChild<UITexture>("Icon");
            this.Chip = base.Root.FindChild<UISprite>("Chip");
            Transform ui = base.Root.FindChild<Transform>("Item1");
            this.QualityBorder = base.Root.FindChild<UISprite>("QualityBorder");
            this.Count = base.Root.FindChild<UILabel>("Count");
            this.Drag = ui.GetComponent<UIDragCamera>();
            this.Checked = base.FindChild<UISprite>("Checked");
            ui.OnUIMouseClick(delegate (object u) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
            this.Item1 = ui;
        }

        public BagPanel.BagItemData ItemData
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
                this.DoHide(this._item.Data.num == 0);
                item_config config = value.Config;
                if (config != null)
                {
                    this.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(config.icon);
                    CommonFunc.SetEquipQualityBorder(this.QualityBorder, config.quality, false);
                    this.Chip.gameObject.SetActive(false);
                    this.QualityBorder.gameObject.SetActive(true);
                    this.Checked.ActiveSelfObject(false);
                    if (value.Data.num > 1)
                    {
                        this.Count.text = value.Data.num.ToString();
                    }
                    else
                    {
                        this.Count.text = string.Empty;
                    }
                }
            }
        }
    }
}

