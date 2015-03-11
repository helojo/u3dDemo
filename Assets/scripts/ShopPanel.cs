using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class ShopPanel : GUIEntity
{
    public UILabel _CoinLabel;
    public GameObject _RefreshBtn;
    public GameObject _SingleShopItem;
    public UILabel _TimeLabel;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache10;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache11;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheE;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheF;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private List<ShopItem> mArenaLadderItemList;
    private ShopCoinType mCurrShopCoinType;
    private List<ShopItem> mFlameItemList;
    private bool mHaveFreeUseItem;
    private bool mIsStart;
    private int mNextRefreshTime;
    private Dictionary<int, GameObject> mShopItemDict = new Dictionary<int, GameObject>();
    private List<ShopItem> mShopItemList;

    private void CreateItemList(ShopCoinType type, List<ShopItem> itemList)
    {
        this.mCurrShopCoinType = type;
        UIGrid component = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        this.mShopItemDict.Clear();
        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject go = UnityEngine.Object.Instantiate(this._SingleShopItem) as GameObject;
            go.transform.parent = component.transform;
            go.transform.position = Vector3.zero;
            go.transform.localScale = Vector3.one;
            int id = -1;
            if (type == ShopCoinType.YuanZhengCoin)
            {
                flame_shop_config _config = ConfigMgr.getInstance().getByEntry<flame_shop_config>(itemList[i].entry);
                if (_config != null)
                {
                    id = _config.goods_entry;
                }
            }
            else if (type == ShopCoinType.ArenaLadderCoin)
            {
                arena_shop_config _config2 = ConfigMgr.getInstance().getByEntry<arena_shop_config>(itemList[i].entry);
                if (_config2 != null)
                {
                    id = _config2.goods_entry;
                }
            }
            else if (type == ShopCoinType.ArenaChallengeCoin)
            {
                lolarena_shop_config _config3 = ConfigMgr.getInstance().getByEntry<lolarena_shop_config>(itemList[i].entry);
                if (_config3 != null)
                {
                    id = _config3.goods_entry;
                }
            }
            else if (type == ShopCoinType.OutLandCoin)
            {
                outland_shop_config _config4 = ConfigMgr.getInstance().getByEntry<outland_shop_config>(itemList[i].entry);
                if (_config4 != null)
                {
                    id = _config4.goods_entry;
                }
            }
            item_config _config5 = ConfigMgr.getInstance().getByEntry<item_config>(id);
            if (_config5 != null)
            {
                int remainBuyCount = this.GetRemainBuyCount(itemList[i], type);
                this.SetItemInfo(go, type, itemList[i].buyCount, itemList[i].stackCount, _config5, itemList[i].cost);
                if (remainBuyCount > 0)
                {
                    GUIDataHolder.setData(go, i);
                    UIEventListener.Get(go.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickItem);
                    go.transform.FindChild("Tips").gameObject.SetActive(false);
                    go.transform.FindChild("bg_f").gameObject.SetActive(false);
                }
                else
                {
                    nguiTextureGrey.doChangeEnableGrey(go.transform.FindChild("texture").GetComponent<UITexture>(), true);
                    go.transform.FindChild("bg_f").gameObject.SetActive(true);
                    go.transform.FindChild("Tips").gameObject.SetActive(true);
                }
                this.mShopItemDict.Add(i, go);
            }
        }
        component.Reposition();
    }

    private string GetLocalTime(int _time)
    {
        return (string.Format("{0:00}", _time / 0xe10) + ":" + string.Format("{0:00}", (_time % 0xe10) / 60));
    }

    private int GetRemainBuyCount(ShopItem si, ShopCoinType type)
    {
        switch (type)
        {
            case ShopCoinType.YuanZhengCoin:
            {
                flame_shop_config _config = ConfigMgr.getInstance().getByEntry<flame_shop_config>(si.entry);
                if (_config == null)
                {
                    break;
                }
                return (_config.limit - si.buyCount);
            }
            case ShopCoinType.ArenaLadderCoin:
            {
                arena_shop_config _config2 = ConfigMgr.getInstance().getByEntry<arena_shop_config>(si.entry);
                if (_config2 == null)
                {
                    break;
                }
                return (_config2.limit - si.buyCount);
            }
            case ShopCoinType.OutLandCoin:
            {
                outland_shop_config _config4 = ConfigMgr.getInstance().getByEntry<outland_shop_config>(si.entry);
                if (_config4 == null)
                {
                    break;
                }
                return (_config4.limit - si.buyCount);
            }
            case ShopCoinType.ArenaChallengeCoin:
            {
                lolarena_shop_config _config3 = ConfigMgr.getInstance().getByEntry<lolarena_shop_config>(si.entry);
                if (_config3 != null)
                {
                    return (_config3.limit - si.buyCount);
                }
                break;
            }
        }
        return 0;
    }

    private void OnClickItem(GameObject go)
    {
        <OnClickItem>c__AnonStorey25E storeye = new <OnClickItem>c__AnonStorey25E {
            <>f__this = this
        };
        if (this.mShopItemList != null)
        {
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                storeye.slot = (int) obj2;
                storeye.si = this.mShopItemList[storeye.slot];
                switch (this.mCurrShopCoinType)
                {
                    case ShopCoinType.YuanZhengCoin:
                        GUIMgr.Instance.DoModelGUI("BuyEnterPanel", new Action<GUIEntity>(storeye.<>m__55C), null);
                        break;

                    case ShopCoinType.ArenaLadderCoin:
                        GUIMgr.Instance.DoModelGUI("BuyEnterPanel", new Action<GUIEntity>(storeye.<>m__55E), null);
                        break;

                    case ShopCoinType.OutLandCoin:
                        GUIMgr.Instance.DoModelGUI("BuyEnterPanel", new Action<GUIEntity>(storeye.<>m__55F), null);
                        break;

                    case ShopCoinType.ArenaChallengeCoin:
                        GUIMgr.Instance.DoModelGUI("BuyEnterPanel", new Action<GUIEntity>(storeye.<>m__55D), null);
                        break;
                }
            }
        }
    }

    private void OnClickRefreshBtn(GameObject go)
    {
        this.RefreshItem();
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
        this.SetRefreshBtn();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(this._RefreshBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRefreshBtn);
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                if (TimeMgr.Instance.ServerStampTime > this.mNextRefreshTime)
                {
                    this.SetNextRefleshTime();
                    switch (this.mCurrShopCoinType)
                    {
                        case ShopCoinType.YuanZhengCoin:
                            SocketMgr.Instance.RequestFlameBattleShopInfo();
                            break;

                        case ShopCoinType.ArenaLadderCoin:
                            SocketMgr.Instance.RequestArenaLadderShopInfo();
                            break;

                        case ShopCoinType.OutLandCoin:
                            SocketMgr.Instance.RequestOutlandShopInfo();
                            break;

                        case ShopCoinType.ArenaChallengeCoin:
                            SocketMgr.Instance.RequestChallengeShopInfo();
                            break;
                    }
                }
            }
        }
    }

    private void RefreshArenaChallengeShop()
    {
        <RefreshArenaChallengeShop>c__AnonStorey260 storey = new <RefreshArenaChallengeShop>c__AnonStorey260 {
            costCoin2 = CommonFunc.GetLolArenaLadderFreshCoin(ActorData.getInstance().UserInfo.refreshLoLArenaShopCount)
        };
        if (ActorData.getInstance().ArenaChallengeCoin < storey.costCoin2)
        {
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x898), null, null, true);
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cacheF, null);
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__563), null);
        }
    }

    private void RefreshArenaLadderShop()
    {
        <RefreshArenaLadderShop>c__AnonStorey261 storey = new <RefreshArenaLadderShop>c__AnonStorey261 {
            costCoin2 = CommonFunc.GetArenaLadderFreshCoin(ActorData.getInstance().UserInfo.refreshArenaLadderShopCount)
        };
        if (ActorData.getInstance().ArenaLadderCoin < storey.costCoin2)
        {
            if (<>f__am$cache10 == null)
            {
                <>f__am$cache10 = obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x83d), null, null, true);
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache10, null);
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__565), null);
        }
    }

    private void RefreshItem()
    {
        switch (this.mCurrShopCoinType)
        {
            case ShopCoinType.YuanZhengCoin:
                if (!this.mHaveFreeUseItem)
                {
                    this.RefreshYuanZhengShop();
                    break;
                }
                SocketMgr.Instance.RequestFlameBattleShopRefresh(true);
                break;

            case ShopCoinType.ArenaLadderCoin:
                if (!this.mHaveFreeUseItem)
                {
                    this.RefreshArenaLadderShop();
                    break;
                }
                SocketMgr.Instance.RequestArenaLadderShopRefresh(true);
                break;

            case ShopCoinType.OutLandCoin:
                if (!this.mHaveFreeUseItem)
                {
                    this.RefreshOutlandShop();
                    break;
                }
                SocketMgr.Instance.RequestOutlandShopRefresh(true);
                break;

            case ShopCoinType.ArenaChallengeCoin:
                if (this.mHaveFreeUseItem)
                {
                    SocketMgr.Instance.RequestRefreshChallengeShop(true);
                }
                else
                {
                    this.RefreshArenaChallengeShop();
                }
                break;
        }
    }

    private void RefreshOutlandShop()
    {
        <RefreshOutlandShop>c__AnonStorey25F storeyf = new <RefreshOutlandShop>c__AnonStorey25F {
            costCoin3 = CommonFunc.GetOutlandLadderFreshCoin(ActorData.getInstance().UserInfo.refreshOutlandCount)
        };
        if (ActorData.getInstance().OutlandCoin < storeyf.costCoin3)
        {
            if (<>f__am$cacheE == null)
            {
                <>f__am$cacheE = obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x4e25), null, null, true);
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cacheE, null);
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyf.<>m__561), null);
        }
    }

    private void RefreshYuanZhengShop()
    {
        <RefreshYuanZhengShop>c__AnonStorey262 storey = new <RefreshYuanZhengShop>c__AnonStorey262 {
            costCoin = CommonFunc.GetYuanZhengRefreshCoin(ActorData.getInstance().UserInfo.refreshFlameBattleShopCount)
        };
        if (ActorData.getInstance().FlamebattleCoin < storey.costCoin)
        {
            if (<>f__am$cache11 == null)
            {
                <>f__am$cache11 = obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x835), null, null, true);
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache11, null);
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__567), null);
        }
    }

    private void SetItemInfo(GameObject go, ShopCoinType type, int buyCount, int stackCount, item_config item_cfg, int itemPrice)
    {
        UILabel component = go.transform.FindChild("name").GetComponent<UILabel>();
        UILabel label2 = go.transform.FindChild("price").GetComponent<UILabel>();
        UITexture texture = go.transform.FindChild("texture").GetComponent<UITexture>();
        UILabel label3 = go.transform.FindChild("count").GetComponent<UILabel>();
        UISprite sprite = go.transform.FindChild("picon").GetComponent<UISprite>();
        UISprite sprite2 = go.transform.FindChild("frame").GetComponent<UISprite>();
        Transform transform = go.transform.FindChild("Tips");
        UIButton button = go.GetComponent<UIButton>();
        go.transform.FindChild("UpTips").gameObject.SetActive(XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(item_cfg.entry, stackCount).Count > 0);
        component.text = item_cfg.name;
        label2.text = itemPrice.ToString();
        label3.text = stackCount.ToString();
        switch (type)
        {
            case ShopCoinType.YuanZhengCoin:
                sprite.spriteName = "Ui_Yuanzheng_Icon_gongxun";
                break;

            case ShopCoinType.ArenaLadderCoin:
                sprite.spriteName = "Ui_Pk_Icon_coin";
                break;

            case ShopCoinType.OutLandCoin:
                sprite.spriteName = "Ui_Out_Icon_stone";
                sprite.height = 0x3e;
                sprite.width = 0x3e;
                sprite.transform.localPosition = new Vector3(59f, 38f, 0f);
                break;

            case ShopCoinType.ArenaChallengeCoin:
                sprite.spriteName = "Ui_Pk_Icon_coin1";
                break;
        }
        if (item_cfg != null)
        {
            if (item_cfg.type == 3)
            {
                texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(item_cfg.icon);
            }
            else
            {
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(item_cfg.icon);
            }
            if (item_cfg.type == 3)
            {
                texture.width = 0x7c;
                texture.height = 0x7c;
            }
            else
            {
                texture.width = 100;
                texture.height = 100;
            }
            CommonFunc.SetEquipQualityBorder(sprite2, item_cfg.quality, false);
        }
        if ((item_cfg.type == 2) || (item_cfg.type == 3))
        {
            go.transform.FindChild("chip").gameObject.SetActive(true);
        }
        else
        {
            go.transform.FindChild("chip").gameObject.SetActive(false);
        }
    }

    private void SetNextRefleshTime()
    {
        List<int> list = new List<int>();
        switch (this.mCurrShopCoinType)
        {
            case ShopCoinType.YuanZhengCoin:
            {
                ArrayList list2 = ConfigMgr.getInstance().getList<flame_shop_time_config>();
                for (int j = 0; j < list2.Count; j++)
                {
                    list.Add(((flame_shop_time_config) list2[j]).day_time);
                }
                break;
            }
            case ShopCoinType.ArenaLadderCoin:
            {
                ArrayList list3 = ConfigMgr.getInstance().getList<arena_shop_time_config>();
                for (int k = 0; k < list3.Count; k++)
                {
                    list.Add(((arena_shop_time_config) list3[k]).day_time);
                }
                break;
            }
            case ShopCoinType.OutLandCoin:
            {
                ArrayList list5 = ConfigMgr.getInstance().getList<outland_shop_time_config>();
                for (int m = 0; m < list5.Count; m++)
                {
                    list.Add(((outland_shop_time_config) list5[m]).day_time);
                }
                break;
            }
            case ShopCoinType.ArenaChallengeCoin:
            {
                ArrayList list4 = ConfigMgr.getInstance().getList<lolarena_shop_time_config>();
                for (int n = 0; n < list4.Count; n++)
                {
                    list.Add(((lolarena_shop_time_config) list4[n]).day_time);
                }
                break;
            }
        }
        if (list.Count < 1)
        {
            list.Add(0x12750);
        }
        int num5 = (int) TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime.Date);
        for (int i = 0; i < list.Count; i++)
        {
            int num7 = list[i];
            if (TimeMgr.Instance.ServerStampTime < (num5 + num7))
            {
                this._TimeLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0x2b0), this.GetLocalTime(num7));
                this.mNextRefreshTime = num5 + num7;
                return;
            }
        }
        int num8 = list[0];
        this.mNextRefreshTime = (num5 + 0x15180) + num8;
        this._TimeLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0x2b1), this.GetLocalTime(num8));
    }

    private void SetRefreshBtn()
    {
        this.mHaveFreeUseItem = false;
        Item ticketItemBySubType = null;
        UISprite component = base.transform.FindChild("Top/refresh_btn/Icon").GetComponent<UISprite>();
        switch (this.mCurrShopCoinType)
        {
            case ShopCoinType.YuanZhengCoin:
                ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Flame_Shop_Refresh);
                this.mHaveFreeUseItem = ticketItemBySubType != null;
                break;

            case ShopCoinType.ArenaLadderCoin:
                ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Arena_Shop_Refresh);
                this.mHaveFreeUseItem = ticketItemBySubType != null;
                break;

            case ShopCoinType.OutLandCoin:
                ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Outland_Shop_Refresh);
                this.mHaveFreeUseItem = ticketItemBySubType != null;
                break;

            case ShopCoinType.ArenaChallengeCoin:
                ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_LoLArena_Shop_Refresh);
                this.mHaveFreeUseItem = ticketItemBySubType != null;
                break;
        }
        if (ticketItemBySubType != null)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(ticketItemBySubType.entry);
            if (_config != null)
            {
                component.spriteName = _config.icon;
            }
        }
        component.ActiveSelfObject(this.mHaveFreeUseItem);
        base.transform.FindChild("Top/refresh_btn/Label").GetComponent<UILabel>().text = !this.mHaveFreeUseItem ? ConfigMgr.getInstance().GetWord(0x3e6) : ("X" + ticketItemBySubType.num);
    }

    public void SetShopType(ShopCoinType _type)
    {
        this.mCurrShopCoinType = _type;
        this.SetNextRefleshTime();
        UISprite component = base.transform.FindChild("Top/Coin/Icon").GetComponent<UISprite>();
        UILabel label = base.transform.FindChild("TopLabel").GetComponent<UILabel>();
        switch (_type)
        {
            case ShopCoinType.YuanZhengCoin:
                component.spriteName = "Ui_Yuanzheng_Icon_gongxun";
                label.text = ConfigMgr.getInstance().GetWord(0x84e);
                break;

            case ShopCoinType.ArenaLadderCoin:
                component.spriteName = "Ui_Pk_Icon_coin";
                label.text = ConfigMgr.getInstance().GetWord(0x84d);
                break;

            case ShopCoinType.OutLandCoin:
                component.spriteName = "Ui_Out_Icon_stone";
                component.height = 0x3e;
                component.width = 0x3e;
                component.transform.localPosition = new Vector3(-53f, 9f, 0f);
                label.text = ConfigMgr.getInstance().GetWord(0x4e24);
                break;

            case ShopCoinType.ArenaChallengeCoin:
                component.spriteName = "Ui_Pk_Icon_coin1";
                label.text = ConfigMgr.getInstance().GetWord(0x899);
                break;
        }
        CommonFunc.DeleteChildItem(base.transform.FindChild("List/Grid").GetComponent<UIGrid>().transform);
    }

    public void ShowItemData(ShopCoinType type, List<ShopItem> itemList)
    {
        this.mShopItemList = itemList;
        this.CreateItemList(type, itemList);
        this.SetRefreshBtn();
        this.UpdateCoinCount();
        this.mIsStart = true;
    }

    private void UpdateCoinCount()
    {
        switch (this.mCurrShopCoinType)
        {
            case ShopCoinType.YuanZhengCoin:
                this._CoinLabel.text = ActorData.getInstance().FlamebattleCoin.ToString();
                break;

            case ShopCoinType.ArenaLadderCoin:
                this._CoinLabel.text = ActorData.getInstance().ArenaLadderCoin.ToString();
                break;

            case ShopCoinType.OutLandCoin:
                this._CoinLabel.text = ActorData.getInstance().OutlandCoin.ToString();
                break;

            case ShopCoinType.ArenaChallengeCoin:
                this._CoinLabel.text = ActorData.getInstance().ArenaChallengeCoin.ToString();
                break;
        }
    }

    public void UpdateItemBySolt(int solt)
    {
        if (this.mShopItemDict.ContainsKey(solt))
        {
            GameObject obj2 = this.mShopItemDict[solt];
            nguiTextureGrey.doChangeEnableGrey(obj2.transform.FindChild("texture").GetComponent<UITexture>(), true);
            obj2.transform.FindChild("Tips").gameObject.SetActive(true);
            obj2.transform.FindChild("bg_f").GetComponent<UISprite>().gameObject.SetActive(true);
            UIEventListener listener1 = UIEventListener.Get(obj2.gameObject);
            listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Remove(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickItem));
        }
        this.UpdateCoinCount();
    }

    [CompilerGenerated]
    private sealed class <OnClickItem>c__AnonStorey25E
    {
        internal ShopPanel <>f__this;
        internal ShopItem si;
        internal int slot;

        internal void <>m__55C(GUIEntity guiE)
        {
            ((BuyEnterPanel) guiE).UpateData(this.<>f__this.mCurrShopCoinType, this.si, delegate {
                if (ActorData.getInstance().FlamebattleCoin < this.si.cost)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x835));
                }
                else
                {
                    BuyShopItemInfo buyInfo = new BuyShopItemInfo {
                        cost = this.si.cost,
                        slot = this.slot,
                        shopEntry = this.si.entry
                    };
                    SocketMgr.Instance.RequestFlameBattleShopBuy(buyInfo);
                    GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
                }
            });
        }

        internal void <>m__55D(GUIEntity guiE)
        {
            ((BuyEnterPanel) guiE).UpateData(this.<>f__this.mCurrShopCoinType, this.si, delegate {
                if (ActorData.getInstance().UserInfo.lol_arena_score < this.si.cost)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x898));
                }
                else
                {
                    BuyShopItemInfo info = new BuyShopItemInfo {
                        cost = this.si.cost,
                        shopEntry = this.si.entry,
                        slot = this.slot
                    };
                    SocketMgr.Instance.RequestChallengeShopBuy(info);
                    GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
                }
            });
        }

        internal void <>m__55E(GUIEntity guiE)
        {
            ((BuyEnterPanel) guiE).UpateData(this.<>f__this.mCurrShopCoinType, this.si, delegate {
                if (ActorData.getInstance().UserInfo.arena_ladder_score < this.si.cost)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x83d));
                }
                else
                {
                    BuyShopItemInfo buyInfo = new BuyShopItemInfo {
                        cost = this.si.cost,
                        shopEntry = this.si.entry,
                        slot = this.slot
                    };
                    SocketMgr.Instance.RequesArenaLadderShopBuy(buyInfo);
                    GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
                }
            });
        }

        internal void <>m__55F(GUIEntity guiE)
        {
            ((BuyEnterPanel) guiE).UpateData(this.<>f__this.mCurrShopCoinType, this.si, delegate {
                if (ActorData.getInstance().OutlandCoin < this.si.cost)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e25));
                }
                else
                {
                    BuyShopItemInfo info = new BuyShopItemInfo {
                        cost = this.si.cost,
                        slot = this.slot,
                        shopEntry = this.si.entry
                    };
                    SocketMgr.Instance.RequestOutlandShopBuy(info);
                    GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
                }
            });
        }

        internal void <>m__568()
        {
            if (ActorData.getInstance().FlamebattleCoin < this.si.cost)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x835));
            }
            else
            {
                BuyShopItemInfo buyInfo = new BuyShopItemInfo {
                    cost = this.si.cost,
                    slot = this.slot,
                    shopEntry = this.si.entry
                };
                SocketMgr.Instance.RequestFlameBattleShopBuy(buyInfo);
                GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
            }
        }

        internal void <>m__569()
        {
            if (ActorData.getInstance().UserInfo.lol_arena_score < this.si.cost)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x898));
            }
            else
            {
                BuyShopItemInfo info = new BuyShopItemInfo {
                    cost = this.si.cost,
                    shopEntry = this.si.entry,
                    slot = this.slot
                };
                SocketMgr.Instance.RequestChallengeShopBuy(info);
                GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
            }
        }

        internal void <>m__56A()
        {
            if (ActorData.getInstance().UserInfo.arena_ladder_score < this.si.cost)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x83d));
            }
            else
            {
                BuyShopItemInfo buyInfo = new BuyShopItemInfo {
                    cost = this.si.cost,
                    shopEntry = this.si.entry,
                    slot = this.slot
                };
                SocketMgr.Instance.RequesArenaLadderShopBuy(buyInfo);
                GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
            }
        }

        internal void <>m__56B()
        {
            if (ActorData.getInstance().OutlandCoin < this.si.cost)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e25));
            }
            else
            {
                BuyShopItemInfo info = new BuyShopItemInfo {
                    cost = this.si.cost,
                    slot = this.slot,
                    shopEntry = this.si.entry
                };
                SocketMgr.Instance.RequestOutlandShopBuy(info);
                GUIMgr.Instance.ExitModelGUI("BuyEnterPanel");
            }
        }
    }

    [CompilerGenerated]
    private sealed class <RefreshArenaChallengeShop>c__AnonStorey260
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal int costCoin2;

        internal void <>m__563(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            string str = "\n(" + string.Format(ConfigMgr.getInstance().GetWord(0x270), ActorData.getInstance().UserInfo.refreshLoLArenaShopCount + 1) + ")";
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = mb => SocketMgr.Instance.RequestRefreshChallengeShop(false);
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x89a), this.costCoin2) + str, <>f__am$cache1, null, false);
        }

        private static void <>m__56D(GameObject mb)
        {
            SocketMgr.Instance.RequestRefreshChallengeShop(false);
        }
    }

    [CompilerGenerated]
    private sealed class <RefreshArenaLadderShop>c__AnonStorey261
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal int costCoin2;

        internal void <>m__565(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            string str = "\n(" + string.Format(ConfigMgr.getInstance().GetWord(0x270), ActorData.getInstance().UserInfo.refreshArenaLadderShopCount + 1) + ")";
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = mb => SocketMgr.Instance.RequestArenaLadderShopRefresh(false);
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x84c), this.costCoin2) + str, <>f__am$cache1, null, false);
        }

        private static void <>m__56E(GameObject mb)
        {
            SocketMgr.Instance.RequestArenaLadderShopRefresh(false);
        }
    }

    [CompilerGenerated]
    private sealed class <RefreshOutlandShop>c__AnonStorey25F
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal int costCoin3;

        internal void <>m__561(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            string str = "\n(" + string.Format(ConfigMgr.getInstance().GetWord(0x270), ActorData.getInstance().UserInfo.refreshOutlandCount + 1) + ")";
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = mb => SocketMgr.Instance.RequestOutlandShopRefresh(false);
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x4e26), this.costCoin3) + str, <>f__am$cache1, null, false);
        }

        private static void <>m__56C(GameObject mb)
        {
            SocketMgr.Instance.RequestOutlandShopRefresh(false);
        }
    }

    [CompilerGenerated]
    private sealed class <RefreshYuanZhengShop>c__AnonStorey262
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal int costCoin;

        internal void <>m__567(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            string str = "\n(" + string.Format(ConfigMgr.getInstance().GetWord(0x270), ActorData.getInstance().UserInfo.refreshFlameBattleShopCount + 1) + ")";
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = mb => SocketMgr.Instance.RequestFlameBattleShopRefresh(false);
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x836), this.costCoin) + str, <>f__am$cache1, null, false);
        }

        private static void <>m__56F(GameObject mb)
        {
            SocketMgr.Instance.RequestFlameBattleShopRefresh(false);
        }
    }
}

