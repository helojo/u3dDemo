using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class CourageShopPanel : MonoBehaviour
{
    private UILabel _badgeLabel;
    private UILabel _courageLabel;
    private UIGrid _grid;
    private UIButton _refreshButton;
    private UILabel _tickLabel;
    public GameObject itemPrefab;

    private void AutoSellItem()
    {
        <AutoSellItem>c__AnonStorey258 storey = new <AutoSellItem>c__AnonStorey258 {
            <>f__this = this,
            SellList = new List<Item>()
        };
        int num = 0;
        int num2 = 0;
        foreach (Item item in ActorData.getInstance().ItemList)
        {
            if (num2 >= 100)
            {
                break;
            }
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
            if ((_config.type == 5) && (_config.sell_price > 0))
            {
                num += _config.sell_price;
                storey.SellList.Add(item);
                num2++;
            }
        }
        if (storey.SellList.Count > 0)
        {
            this.DelayCallBack(0.2f, new System.Action(storey.<>m__54D));
        }
    }

    private void DeleteAllItems()
    {
        int childCount = this.itemGrid.transform.childCount;
        Transform[] transformArray = new Transform[childCount];
        for (int i = 0; i != childCount; i++)
        {
            transformArray[i] = this.itemGrid.transform.GetChild(i);
        }
        foreach (Transform transform in transformArray)
        {
            transform.parent = null;
            UnityEngine.Object.Destroy(transform.gameObject);
        }
    }

    private int GetCostOfRefresh(int time)
    {
        ArrayList list = ConfigMgr.getInstance().getList<courage_shop_refresh_config>();
        for (int i = 0; i != list.Count; i++)
        {
            courage_shop_refresh_config _config = (courage_shop_refresh_config) list[i];
            if (((time + 1) < _config.count) && (i > 0))
            {
                courage_shop_refresh_config _config2 = (courage_shop_refresh_config) list[i - 1];
                return _config2.cost_value;
            }
        }
        courage_shop_refresh_config _config3 = (courage_shop_refresh_config) list[list.Count - 1];
        return _config3.cost_value;
    }

    private void OnClickItem(GameObject go)
    {
        <OnClickItem>c__AnonStorey259 storey = new <OnClickItem>c__AnonStorey259 {
            context = go.GetComponent<CourageContext>()
        };
        if (null != storey.context)
        {
            storey.cfg = ConfigMgr.getInstance().getByEntry<courage_shop_config>(storey.context.entry);
            if (storey.cfg != null)
            {
                GUIMgr.Instance.DoModelGUI("BuyEnterPanel", new Action<GUIEntity>(storey.<>m__54E), null);
            }
        }
    }

    private void OnForceRefreshClick(GameObject go)
    {
        <OnForceRefreshClick>c__AnonStorey25A storeya = new <OnForceRefreshClick>c__AnonStorey25A();
        int refreshCourageShopCount = ActorData.getInstance().UserInfo.refreshCourageShopCount;
        int costOfRefresh = this.GetCostOfRefresh(refreshCourageShopCount);
        string str = "\n(" + string.Format(ConfigMgr.getInstance().GetWord(330), refreshCourageShopCount) + ")";
        storeya.title = string.Format(ConfigMgr.getInstance().GetWord(910), costOfRefresh) + str;
        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeya.<>m__54F), base.gameObject);
    }

    public void OnHidePage()
    {
    }

    public void OnInitializePage()
    {
        UIEventListener listener1 = UIEventListener.Get(this.RefreshButton.gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnForceRefreshClick));
    }

    public void OnShowPage()
    {
        this.Refresh();
        this.RefreshCurrency();
        this.AutoSellItem();
    }

    public void Refresh()
    {
        this.DeleteAllItems();
        foreach (ShopItem item in ActorData.getInstance().courageShopItemList)
        {
            courage_shop_config _config = ConfigMgr.getInstance().getByEntry<courage_shop_config>(item.entry);
            if (_config != null)
            {
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(_config.goods_entry);
                GameObject go = UnityEngine.Object.Instantiate(this.itemPrefab) as GameObject;
                go.transform.parent = this.itemGrid.transform;
                go.transform.position = Vector3.zero;
                go.transform.localScale = Vector3.one;
                UILabel component = go.transform.FindChild("name").GetComponent<UILabel>();
                UILabel label2 = go.transform.FindChild("price").GetComponent<UILabel>();
                UITexture texture = go.transform.FindChild("texture").GetComponent<UITexture>();
                UILabel label3 = go.transform.FindChild("count").GetComponent<UILabel>();
                UISprite sprite = go.transform.FindChild("picon").GetComponent<UISprite>();
                UISprite sprite2 = go.transform.FindChild("frame").GetComponent<UISprite>();
                Transform transform = go.transform.FindChild("Tips");
                UIButton button = go.GetComponent<UIButton>();
                component.text = _config.name;
                label2.text = item.cost.ToString();
                label3.text = item.stackCount.ToString();
                go.transform.FindChild("UpTips").gameObject.SetActive(XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(_config2.entry, item.stackCount).Count > 0);
                if ((_config.limit - item.buyCount) <= 0)
                {
                    button.enabled = false;
                    nguiTextureGrey.doChangeEnableGrey(texture, true);
                    go.transform.FindChild("bg_f").gameObject.SetActive(true);
                    transform.gameObject.SetActive(true);
                }
                else
                {
                    transform.gameObject.SetActive(false);
                }
                CourageContext context = go.AddComponent<CourageContext>();
                context.entry = item.entry;
                context.slot = item.slot;
                context.cost = item.cost;
                context.type = _config.cost_type;
                context.item = item;
                if (_config2 != null)
                {
                    if (_config2.type == 3)
                    {
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.icon);
                    }
                    else
                    {
                        texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                    }
                    CommonFunc.SetEquipQualityBorder(sprite2, _config2.quality, false);
                }
                if (_config.cost_type == 1)
                {
                    sprite.spriteName = "Item_Icon_Gold";
                }
                else
                {
                    sprite.spriteName = "Item_Icon_Stone";
                }
                if ((_config2.type == 2) || (_config2.type == 3))
                {
                    go.transform.FindChild("chip").gameObject.SetActive(true);
                }
                else
                {
                    go.transform.FindChild("chip").gameObject.SetActive(false);
                }
                if ((_config.limit - item.buyCount) > 0)
                {
                    UIEventListener listener1 = UIEventListener.Get(go);
                    listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickItem));
                }
                int num = (_config2.type != 3) ? 0x6c : 0x7b;
                texture.height = num;
                texture.width = num;
                sprite2.depth = (_config2.type != 3) ? 30 : 11;
            }
        }
        this.itemGrid.Reposition();
    }

    public void RefreshCurrency()
    {
        this.CourageLable.text = ActorData.getInstance().Courage.ToString();
    }

    private void Update()
    {
        this.TickLabel.text = GameDataMgr.Instance.NextShopRefreshTime();
    }

    private UILabel BadgeLabel
    {
        get
        {
            if (null == this._badgeLabel)
            {
                this._badgeLabel = base.transform.FindChild("Bottom/badge").GetComponent<UILabel>();
            }
            return this._badgeLabel;
        }
    }

    private UILabel CourageLable
    {
        get
        {
            if (null == this._courageLabel)
            {
                this._courageLabel = base.transform.FindChild("Bottom/courage").GetComponent<UILabel>();
            }
            return this._courageLabel;
        }
    }

    private UIGrid itemGrid
    {
        get
        {
            if (null == this._grid)
            {
                this._grid = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
            }
            return this._grid;
        }
    }

    private UIButton RefreshButton
    {
        get
        {
            if (null == this._refreshButton)
            {
                this._refreshButton = base.transform.FindChild("Top/refresh_btn").GetComponent<UIButton>();
            }
            return this._refreshButton;
        }
    }

    private UILabel TickLabel
    {
        get
        {
            if (null == this._tickLabel)
            {
                this._tickLabel = base.transform.FindChild("Top/time").GetComponent<UILabel>();
            }
            return this._tickLabel;
        }
    }

    [CompilerGenerated]
    private sealed class <AutoSellItem>c__AnonStorey258
    {
        internal CourageShopPanel <>f__this;
        internal List<Item> SellList;

        internal void <>m__54D()
        {
            if (null == GUIMgr.Instance.GetActivityGUIEntity<SellItemListPanel>())
            {
                GUIMgr.Instance.DoModelGUI("SellItemListPanel", delegate (GUIEntity obj) {
                    SellItemListPanel panel = obj as SellItemListPanel;
                    panel.ShowItemList(this.SellList);
                    panel.Depth = 210;
                    panel.CallBackOk = () => SocketMgr.Instance.RequestSellItem(this.SellList);
                }, this.<>f__this.gameObject);
            }
        }

        internal void <>m__550(GUIEntity obj)
        {
            SellItemListPanel panel = obj as SellItemListPanel;
            panel.ShowItemList(this.SellList);
            panel.Depth = 210;
            panel.CallBackOk = () => SocketMgr.Instance.RequestSellItem(this.SellList);
        }

        internal void <>m__551()
        {
            SocketMgr.Instance.RequestSellItem(this.SellList);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItem>c__AnonStorey259
    {
        internal courage_shop_config cfg;
        internal CourageContext context;

        internal void <>m__54E(GUIEntity e)
        {
            (e as BuyEnterPanel).UpateData((this.cfg.cost_type != 1) ? ShopCoinType.CourageShopStone : ShopCoinType.CourageShopGold, this.context.item, delegate {
                SocketMgr.Instance.RequestCourageShopBuy(this.context.slot, this.context.entry, this.context.cost, this.context.type);
                GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
            });
        }

        internal void <>m__552()
        {
            SocketMgr.Instance.RequestCourageShopBuy(this.context.slot, this.context.entry, this.context.cost, this.context.type);
            GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
        }
    }

    [CompilerGenerated]
    private sealed class <OnForceRefreshClick>c__AnonStorey25A
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__54F(GUIEntity e)
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => SocketMgr.Instance.RequestForceRefreshCourageShop();
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__553(GameObject _go)
        {
            SocketMgr.Instance.RequestForceRefreshCourageShop();
        }
    }
}

