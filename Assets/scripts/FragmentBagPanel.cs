using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class FragmentBagPanel : GUIEntity
{
    private Transform _InfoPanel;
    [CompilerGenerated]
    private static Comparison<Item> <>f__am$cache7;
    [CompilerGenerated]
    private static Func<Item, BagItemData> <>f__am$cache8;
    public UITableManager<BagTableItem> ItemTable = new UITableManager<BagTableItem>();
    public BagTableItem mCurrSelectItemEntry;
    private bool mDataIsDirty;
    private bool mIsFirstInit = true;
    private Dictionary<int, Transform> mItemGirdDict = new Dictionary<int, Transform>();
    private ShowItemType mShowItemType;

    public void ClearIconCache()
    {
    }

    public void ClosePanel()
    {
        this.ClearIconCache();
        GUIMgr.Instance.PopGUIEntity();
    }

    public void CreateItemList()
    {
        List<Item> fragmentListByType = ActorData.getInstance().GetFragmentListByType(ShowItemType.All);
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = delegate (Item item1, Item item2) {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item1.entry);
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(item2.entry);
                if (_config.quality != _config2.quality)
                {
                    return _config2.quality - _config.quality;
                }
                return 1;
            };
        }
        fragmentListByType.Sort(<>f__am$cache7);
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = t => new BagItemData { Data = t, Config = ConfigMgr.getInstance().getByEntry<item_config>(t.entry) };
        }
        BagItemData[] dataArray = fragmentListByType.Select<Item, BagItemData>(<>f__am$cache8).ToArray<BagItemData>();
        this.ItemTable.Count = dataArray.Length;
        for (int i = 0; i < dataArray.Length; i++)
        {
            BagTableItem item = this.ItemTable[i];
            item.ItemData = dataArray[i];
            item.OnClick = new Action<BagTableItem>(this.OnClickItemBtn);
            if ((this.ItemTable.Count > 0x11) && (item.Drag != null))
            {
                item.Drag.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
            }
        }
        if (this.ItemTable.Count < 1)
        {
            base.transform.FindChild("InfoPanel").gameObject.SetActive(false);
        }
        else
        {
            this.SetItemDetails(this.ItemTable[0]);
        }
        this.mIsFirstInit = false;
    }

    private int GetCombCardNeedCount(int itemEntry)
    {
        int id = -1;
        IEnumerator enumerator = ConfigMgr.getInstance().getList<card_ex_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                card_ex_config current = (card_ex_config) enumerator.Current;
                if (itemEntry == current.item_entry)
                {
                    id = current.card_entry;
                }
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
        if (id > -1)
        {
            card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(id);
            if (_config2 != null)
            {
                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(id, _config2.evolve_lv);
                if (cardExCfg != null)
                {
                    return cardExCfg.combine_need_item_num;
                }
            }
        }
        return -1;
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
            this.InitItemList(ShowItemType.All);
        }
    }

    private void OnClickCaiLiaoBtn()
    {
        if (this.mShowItemType != ShowItemType.Frag_Equip)
        {
            this.InitItemList(ShowItemType.Frag_Equip);
        }
    }

    private void OnClickCardBtn()
    {
        if (this.mShowItemType != ShowItemType.Frag_Card)
        {
            this.InitItemList(ShowItemType.Frag_Card);
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

    private void OnClickFromBtn(GameObject go)
    {
        <OnClickFromBtn>c__AnonStorey188 storey = new <OnClickFromBtn>c__AnonStorey188();
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storey.info = obj2 as Item;
            if (storey.info != null)
            {
                GUIMgr.Instance.PushGUIEntity("DetailsPanel", new Action<GUIEntity>(storey.<>m__1E6));
            }
        }
    }

    private void OnClickItemBtn(BagTableItem item)
    {
        Item data = item.ItemData.Data;
        this.SetItemDetails(item);
        Debug.Log(data.entry + ":" + data.num);
        SoundManager.mInstance.PlaySFX("sound_ui_t_8");
    }

    private void OnClickSellBtn(GameObject go)
    {
        if (this.mCurrSelectItemEntry != null)
        {
            GUIMgr.Instance.DoModelGUI("SellPanel", delegate (GUIEntity obj) {
                SellPanel panel = (SellPanel) obj;
                panel.Depth = 500;
                panel.SetSellItemData(this.mCurrSelectItemEntry.ItemData.Data);
            }, base.gameObject);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        if (!ActorData.getInstance().IsPopPanel)
        {
            this.ResetClipViewport();
            this.UpdateData();
        }
        ActorData.getInstance().IsPopPanel = false;
        GUIMgr.Instance.FloatTitleBar();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        this.ClearIconCache();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        UIGrid grid = base.FindChild<UIGrid>("Grid");
        this.ItemTable.InitFromGrid(grid);
        this.InitGuiControlEvent();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
        this.mShowItemType = ShowItemType.All;
    }

    private void ResetClipViewport()
    {
        Transform top = base.transform.FindChild("ListTopLeft");
        Transform bottom = base.transform.FindChild("ListBottomRight");
        GUIMgr.Instance.ResetListViewpot(top, bottom, base.transform.FindChild("List"), 0f);
    }

    public void SelectDefaultCheckbox()
    {
        base.transform.FindChild("ItemPanel/Tab/AllBtn").GetComponent<UIToggle>().value = true;
    }

    private unsafe void SetItemDetails(BagTableItem tItem)
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
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(data.entry);
            if (_config != null)
            {
                Transform transform = base.transform.FindChild("InfoPanel");
                UISprite component = transform.FindChild("Item/QualityBorder").GetComponent<UISprite>();
                component.color = *((Color*) &(GameConstant.ConstQuantityColor[_config.quality]));
                CommonFunc.SetEquipQualityBorder(component, _config.quality, false);
                component.gameObject.SetActive(true);
                transform.FindChild("Item/sprite").gameObject.SetActive(true);
                transform.FindChild("Info/Name").GetComponent<UILabel>().text = _config.name;
                transform.FindChild("Info/Count").GetComponent<UILabel>().text = data.num.ToString();
                int combCardNeedCount = 0;
                UITexture texture = transform.FindChild("Item/Icon").GetComponent<UITexture>();
                if (_config.type == 3)
                {
                    texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.icon);
                    texture.width = 0x7c;
                    texture.height = 0x7c;
                    combCardNeedCount = this.GetCombCardNeedCount(data.entry);
                }
                else if (_config.type == 2)
                {
                    texture.width = 0x65;
                    texture.height = 0x65;
                    texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(_config.param_0);
                    if (_config2 != null)
                    {
                        combCardNeedCount = _config2.param_1;
                    }
                }
                UILabel label3 = transform.FindChild("Describe/Desc").GetComponent<UILabel>();
                string str = (data.num < combCardNeedCount) ? string.Empty : "[015b17]";
                object[] objArray1 = new object[] { _config.describe, GameConstant.DefaultTextColor, "\n\n", ConfigMgr.getInstance().GetWord(0x4ee), ": ", str, data.num, "/", combCardNeedCount };
                label3.text = string.Concat(objArray1);
                transform.FindChild("SellInfo/Price").GetComponent<UILabel>().text = _config.sell_price.ToString();
                GUIDataHolder.setData(base.transform.FindChild("InfoPanel/SellBtn").gameObject, data);
                Transform transform3 = base.transform.FindChild("InfoPanel/DetailsBtn");
                GUIDataHolder.setData(transform3.gameObject, data);
                UILabel label5 = transform3.transform.FindChild("Label").GetComponent<UILabel>();
                if (_config.type != 3)
                {
                    label5.text = ConfigMgr.getInstance().GetWord(0x273b);
                    UIEventListener.Get(transform3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCombBtn);
                }
                else
                {
                    label5.text = ConfigMgr.getInstance().GetWord(0x2739);
                    UIEventListener.Get(transform3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickFromBtn);
                }
                if (!transform.active)
                {
                    transform.gameObject.SetActive(true);
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

    private void ShowTab(ShowItemType mShowItemType)
    {
        bool flag = true;
        IEnumerator<BagTableItem> enumerator = this.ItemTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                BagTableItem current = enumerator.Current;
                item_config config = current.ItemData.Config;
                if (config == null)
                {
                    current.Hide(true);
                    continue;
                }
                switch (mShowItemType)
                {
                    case ShowItemType.All:
                        current.Hide((config.type != 3) && (config.type != 2));
                        break;

                    case ShowItemType.Frag_Card:
                        current.Hide(config.type != 3);
                        break;

                    case ShowItemType.Frag_Equip:
                        current.Hide(config.type != 2);
                        break;
                }
                if (flag && current.Root.gameObject.activeSelf)
                {
                    this.SetItemDetails(current);
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
        this.ResetClipViewport();
        if (flag)
        {
            base.transform.FindChild("InfoPanel").gameObject.SetActive(false);
        }
    }

    public void UpdateData()
    {
        this.CreateItemList();
        this.ShowTab(this.mShowItemType);
        if (this.ItemTable.Count > 0)
        {
            this.SetItemDetails(this.ItemTable[0]);
        }
    }

    internal void UpdateData(List<Item> list)
    {
        IEnumerator<BagTableItem> enumerator = this.ItemTable.GetEnumerator();
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
                                BagTableItem fristItemNoHide = this.ItemTable.FristItemNoHide;
                                if (fristItemNoHide == null)
                                {
                                    this._InfoPanel.gameObject.SetActive(false);
                                }
                                else
                                {
                                    this.SetItemDetails(fristItemNoHide);
                                }
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
        int totalCountNoHide = this.ItemTable.TotalCountNoHide;
        for (int i = 0; i < this.ItemTable.Count; i++)
        {
            BagTableItem item4 = this.ItemTable[i];
            if ((totalCountNoHide <= 0x11) && (item4.Drag != null))
            {
                item4.Drag.draggableCamera = null;
            }
        }
        this.ItemTable.RepositionLayout();
    }

    [CompilerGenerated]
    private sealed class <OnClickFromBtn>c__AnonStorey188
    {
        internal Item info;

        internal void <>m__1E6(GUIEntity obj)
        {
            ((DetailsPanel) obj).InitItemDetails(this.info, EnterDupType.From_FragmentBagDetails);
        }
    }

    public class BagItemData
    {
        public item_config Config { get; set; }

        public Item Data { get; set; }
    }

    public class BagTableItem : UITableItem
    {
        private FragmentBagPanel.BagItemData _item;
        public UISprite Checked;
        private UISprite Chip;
        private UILabel Count;
        public UIDragCamera Drag;
        private UITexture Icon;
        private Transform Item1;
        public Action<FragmentBagPanel.BagTableItem> OnClick;
        private UISprite QualityBorder;

        public void Hide(bool flag)
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

        public FragmentBagPanel.BagItemData ItemData
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
                this.Hide(this._item.Data.num == 0);
                item_config config = value.Config;
                if (config != null)
                {
                    if (config.type == 3)
                    {
                        this.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(config.icon);
                        this.Icon.width = 0x7c;
                        this.Icon.height = 0x7c;
                    }
                    else
                    {
                        this.Icon.width = 0x66;
                        this.Icon.height = 0x66;
                        this.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(config.icon);
                    }
                    CommonFunc.SetEquipQualityBorder(this.QualityBorder, config.quality, false);
                    this.Chip.gameObject.SetActive(true);
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

