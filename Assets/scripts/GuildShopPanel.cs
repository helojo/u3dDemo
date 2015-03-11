using FastBuf;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class GuildShopPanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache5;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache6;
    private int FreeRefreshCount;
    public UIGrid gridroot;
    private const int LineCount = 2;
    public GameObject ShopItem;

    private void ClickBuildLevUp()
    {
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = delegate (GUIEntity guiE) {
                GuildLevUpPanel panel = guiE.Achieve<GuildLevUpPanel>();
                panel.Depth = 400;
                panel.UpdateData(GuildFuncType.Shop);
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildLevUpPanel", <>f__am$cache5, null);
    }

    private void ClickRefresh()
    {
        if (this.FreeRefreshCount > 0)
        {
            SocketMgr.Instance.RequestGuildRefresh();
        }
        else
        {
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (GUIEntity guiE) {
                    MessageBox box = guiE.Achieve<MessageBox>();
                    if ((ActorData.getInstance().mGuildData == null) || (ActorData.getInstance().mUserGuildMemberData == null))
                    {
                        return;
                    }
                    guild_shop_config _config = ConfigMgr.getInstance().getByEntry<guild_shop_config>((int) ActorData.getInstance().mGuildData.tech.shop_level);
                    if (_config == null)
                    {
                        return;
                    }
                    int num = ActorData.getInstance().mUserGuildMemberData.refresh_shop_times - _config.free_fresh_times;
                    ArrayList list = ConfigMgr.getInstance().getList<guild_shop_refresh_config>();
                    int num2 = 0;
                    IEnumerator enumerator = list.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            guild_shop_refresh_config current = (guild_shop_refresh_config) enumerator.Current;
                            if (num < current.count)
                            {
                                goto Label_00D3;
                            }
                            num2 = current.cost_value;
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
                Label_00D3:
                    if (<>f__am$cache6 == null)
                    {
                        <>f__am$cache6 = go => SocketMgr.Instance.RequestGuildRefresh();
                    }
                    box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0xa6529d), num2), <>f__am$cache6, null, false);
                };
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache3, null);
        }
    }

    private void OnClickSellItem(GameObject obj)
    {
        <OnClickSellItem>c__AnonStorey205 storey = new <OnClickSellItem>c__AnonStorey205 {
            data = (ClientItemData) GUIDataHolder.getData(obj)
        };
        if (storey.data.Item.item_param > 0)
        {
            storey.si = new FastBuf.ShopItem();
            storey.si.cost = storey.data.Item.item_price;
            storey.si.entry = storey.data.Item.item_entry;
            storey.si.stackCount = storey.data.Item.item_param;
            GUIMgr.Instance.DoModelGUI("BuyEnterPanel", new Action<GUIEntity>(storey.<>m__382), null);
        }
    }

    public override void OnDeSerialization(GUIPersistence pefsers)
    {
        this.UpdateContribution();
        this.UpdateOwnContribution();
        this.UpdateBuildLev();
        this.UpdateShopItem();
        this.UpdateBuildLevUpLimit();
        this.UpdateFreeRefreshCount();
        GUIMgr.Instance.FloatTitleBar();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    private void OpenExplan()
    {
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = guiE => guiE.Achieve<GuildGxExplanPanel>().Depth = 400;
        }
        GUIMgr.Instance.DoModelGUI("GuildGxExplanPanel", <>f__am$cache4, null);
    }

    public void UpdateBuildLev()
    {
        base.gameObject.transform.FindChild("Lev/val").GetComponent<UILabel>().text = (ActorData.getInstance().mGuildData.tech.shop_level + 1).ToString();
    }

    private void UpdateBuildLevUpLimit()
    {
        GameObject gameObject = base.gameObject.transform.FindChild("BuildLvUp/Button").gameObject;
        if (ActorData.getInstance().mUserGuildMemberData.position == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateContribution()
    {
        base.gameObject.transform.FindChild("BuildLvUp/Val").GetComponent<UILabel>().text = ActorData.getInstance().mGuildData.cur_contribution.ToString();
    }

    public void UpdateFreeRefreshCount()
    {
        guild_shop_config _config = ConfigMgr.getInstance().getByEntry<guild_shop_config>((int) ActorData.getInstance().mGuildData.tech.shop_level);
        if (_config != null)
        {
            int num = _config.free_fresh_times - ActorData.getInstance().mUserGuildMemberData.refresh_shop_times;
            if (num < 0)
            {
                num = 0;
            }
            base.gameObject.transform.FindChild("NextRefreshTime/Button/Free").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa652b3), "[840100]" + num.ToString());
            this.FreeRefreshCount = num;
        }
    }

    private void UpdateItemData(GameObject obj, ItemProperty data)
    {
        UILabel component = obj.transform.FindChild("name").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("price").GetComponent<UILabel>();
        Transform transform = obj.transform.FindChild("SellOut");
        UILabel label3 = obj.transform.FindChild("Sale").GetComponent<UILabel>();
        obj.transform.FindChild("count").GetComponent<UILabel>().text = data.item_param.ToString();
        UISprite sprite = obj.transform.FindChild("bg_None").GetComponent<UISprite>();
        UISprite sprite2 = obj.transform.FindChild("frame").GetComponent<UISprite>();
        UITexture texture = obj.transform.FindChild("Icon").GetComponent<UITexture>();
        obj.transform.FindChild("UpTips").gameObject.SetActive(XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(data.item_entry, 1).Count > 0);
        GameObject gameObject = obj.transform.FindChild("patch").gameObject;
        if (data.item_param > 0)
        {
            label2.text = data.item_price.ToString();
            transform.gameObject.SetActive(false);
            if (sprite == null)
            {
            }
            sprite.gameObject.SetActive(false);
        }
        else
        {
            label2.text = string.Empty;
            transform.gameObject.SetActive(true);
            sprite.gameObject.SetActive(true);
        }
        switch (data.type)
        {
            case ItemType.ItemType_Card:
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(data.item_entry);
                component.text = _config.name;
                CommonFunc.SetQualityColor(sprite2, _config.quality);
                texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                texture.height = 0x7c;
                texture.width = 0x7c;
                gameObject.SetActive(false);
                break;
            }
            case ItemType.ItemType_Item:
            {
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(data.item_entry);
                if ((_config2.type == 3) || (_config2.type == 2))
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                component.text = _config2.name;
                if (_config2.type == 3)
                {
                    texture.height = 0x7c;
                    texture.width = 0x7c;
                }
                else
                {
                    texture.height = 0x68;
                    texture.width = 0x68;
                }
                CommonFunc.SetEquipQualityBorder(sprite2, _config2.quality, false);
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                break;
            }
        }
        guild_shop_config _config3 = ConfigMgr.getInstance().getByEntry<guild_shop_config>((int) ActorData.getInstance().mGuildData.tech.shop_level);
        if (_config3 != null)
        {
            if (_config3.on_sale == 1f)
            {
                label3.text = string.Empty;
            }
            else
            {
                label3.text = string.Format(ConfigMgr.getInstance().GetWord(0xa652b4), _config3.on_sale * 10f);
            }
        }
    }

    public void UpdateOwnContribution()
    {
        base.gameObject.transform.FindChild("Member/Cnt").GetComponent<UILabel>().text = ActorData.getInstance().mUserGuildMemberData.contribution.ToString();
    }

    public void UpdateShopItem()
    {
        UserGuildMemberData mUserGuildMemberData = ActorData.getInstance().mUserGuildMemberData;
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        GameObject obj2 = null;
        CommonFunc.ResetClippingPanel(base.transform.FindChild("Scroll View"));
        CommonFunc.DeleteChildItem(this.gridroot.transform);
        foreach (ItemProperty property in mUserGuildMemberData.goods.itemList)
        {
            if ((num == 0) || ((num % 2) == 0))
            {
                obj2 = new GameObject();
                obj2.name = (num2 + 1).ToString();
                obj2.transform.parent = this.gridroot.transform;
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.localScale = Vector3.one;
                num = 0;
                num2++;
            }
            GameObject go = UnityEngine.Object.Instantiate(this.ShopItem) as GameObject;
            go.transform.parent = obj2.transform;
            go.transform.localPosition = new Vector3((float) (-228 + (num * 0x1ca)), 0f, 0f);
            go.transform.localScale = Vector3.one;
            ClientItemData data = new ClientItemData {
                index = num3,
                Item = property
            };
            GUIDataHolder.setData(go, data);
            UIEventListener listener1 = UIEventListener.Get(go);
            listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickSellItem));
            this.UpdateItemData(go, property);
            num++;
            num3++;
        }
        this.gridroot.repositionNow = true;
    }

    [CompilerGenerated]
    private sealed class <OnClickSellItem>c__AnonStorey205
    {
        internal GuildShopPanel.ClientItemData data;
        internal ShopItem si;

        internal void <>m__382(GUIEntity guiE)
        {
            ((BuyEnterPanel) guiE).UpateData(ShopCoinType.GuildCoin, this.si, delegate {
                if (ActorData.getInstance().mUserGuildMemberData.contribution < this.si.cost)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x597));
                }
                else
                {
                    SocketMgr.Instance.RequestGuildBuy(this.data.index);
                    GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
                }
            });
        }

        internal void <>m__384()
        {
            if (ActorData.getInstance().mUserGuildMemberData.contribution < this.si.cost)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x597));
            }
            else
            {
                SocketMgr.Instance.RequestGuildBuy(this.data.index);
                GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ClientItemData
    {
        public int index;
        public ItemProperty Item;
    }
}

