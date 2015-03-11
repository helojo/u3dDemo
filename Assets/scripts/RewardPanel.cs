using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class RewardPanel : GUIEntity
{
    public GameObject _LookReward;
    public GameObject _ScrollBar;
    public GameObject _SingleRewardItem;
    public UISprite _TitleSprte;
    public GameObject _TrueReward;
    public System.Action CallBackOk;
    private bool mCurrFunBtn = true;
    private bool mCurrTitleBar = true;

    public void BuyOutlandItem(List<NewCard> newCard, Item item)
    {
        List<Item> list = new List<Item> {
            item
        };
        List<RewardItem> itemList = this.CreateRewardItemListByItem(newCard, list, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
    }

    public void BuyYuanZhengItem(List<NewCard> newCard, Item item)
    {
        this._TitleSprte.spriteName = "Ui_Tips_Label_gmcg";
        this._TitleSprte.MakePixelPerfect();
        List<Item> list = new List<Item> {
            item
        };
        List<RewardItem> itemList = this.CreateRewardItemListByItem(newCard, list, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
    }

    public void ClosePanel()
    {
        if (this.CallBackOk != null)
        {
            this.CallBackOk();
        }
    }

    private List<RewardItem> CreateRewardItemList(BattleReward data)
    {
        return this.CreateRewardItemListByItem(data.cards, data.items, (int) data.gold, (int) data.stone, (int) data.phyforce, (int) data.eq, 0, 0, 0, 0, 0);
    }

    private List<RewardItem> CreateRewardItemListByItem(List<NewCard> _NewCardList, List<Item> _ItemList, int _gold, int _stone, int _phyforce, int _Eq, int _ArenaLadderScore = 0, int _GuildContribute = 0, int _FlameBattleCoin = 0, int _OutlandCoin = 0, int _ArenaChallengeScore = 0)
    {
        base.Depth = 800;
        List<RewardItem> list = new List<RewardItem>();
        if (_NewCardList != null)
        {
            foreach (NewCard card in _NewCardList)
            {
                foreach (Card card2 in card.newCard)
                {
                    if (card2.card_id >= 0L)
                    {
                        RewardItem item = new RewardItem {
                            type = ItemType.ItemType_Card,
                            Data = card2
                        };
                        list.Add(item);
                    }
                }
                foreach (Item item2 in card.newItem)
                {
                    if (item2.entry >= 0)
                    {
                        RewardItem item3 = new RewardItem {
                            type = ItemType.ItemType_Item,
                            Data = item2
                        };
                        list.Add(item3);
                    }
                }
            }
        }
        if (_ItemList != null)
        {
            foreach (Item item4 in _ItemList)
            {
                if (item4.entry >= 0)
                {
                    RewardItem item5 = new RewardItem {
                        type = ItemType.ItemType_Item,
                        Data = item4
                    };
                    list.Add(item5);
                }
            }
        }
        if (_gold > 0)
        {
            RewardItem item6 = new RewardItem {
                type = ItemType.ItemType_Gold,
                Data = _gold
            };
            list.Add(item6);
        }
        if (_stone > 0)
        {
            RewardItem item7 = new RewardItem {
                type = ItemType.ItemType_Stone,
                Data = _stone
            };
            list.Add(item7);
        }
        if (_phyforce > 0)
        {
            RewardItem item8 = new RewardItem {
                type = ItemType.ItemType_Phyforce,
                Data = _phyforce
            };
            list.Add(item8);
        }
        if (_Eq > 0)
        {
            RewardItem item9 = new RewardItem {
                type = ItemType.ItemType_Eq,
                Data = _Eq
            };
            list.Add(item9);
        }
        if (_ArenaLadderScore > 0)
        {
            RewardItem item10 = new RewardItem {
                type = ItemType.ItemType_Arena_Ladder_Score,
                Data = _ArenaLadderScore
            };
            list.Add(item10);
        }
        if (_ArenaChallengeScore > 0)
        {
            RewardItem item11 = new RewardItem {
                type = ItemType.ItemType_LoL_Arena_Score,
                Data = _ArenaChallengeScore
            };
            list.Add(item11);
        }
        if (_GuildContribute > 0)
        {
            RewardItem item12 = new RewardItem {
                type = ItemType.ItemType_Guild_Contribute,
                Data = _GuildContribute
            };
            list.Add(item12);
        }
        if (_FlameBattleCoin > 0)
        {
            RewardItem item13 = new RewardItem {
                type = ItemType.ItemType_FlameBattleCoin,
                Data = _FlameBattleCoin
            };
            list.Add(item13);
        }
        if (_OutlandCoin > 0)
        {
            RewardItem item14 = new RewardItem {
                type = ItemType.ItemType_OutlandCoin,
                Data = _OutlandCoin
            };
            list.Add(item14);
        }
        return list;
    }

    private void OnClickItemBtn(GameObject go, bool isPress)
    {
        if (isPress)
        {
            if (GUIMgr.Instance.GetGUIEntity<ItemInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
            }
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                RewardItem item = obj2 as RewardItem;
                if (item != null)
                {
                    switch (item.type)
                    {
                        case ItemType.ItemType_Card:
                        {
                            <OnClickItemBtn>c__AnonStorey1A4 storeya2 = new <OnClickItemBtn>c__AnonStorey1A4 {
                                info = item.Data as Card
                            };
                            GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storeya2.<>m__221), base.gameObject);
                            break;
                        }
                        case ItemType.ItemType_Item:
                        {
                            <OnClickItemBtn>c__AnonStorey1A3 storeya = new <OnClickItemBtn>c__AnonStorey1A3 {
                                info = item.Data as Item
                            };
                            GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storeya.<>m__220), base.gameObject);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.transform.FindChild("Close").gameObject.SetActive(true);
        base.FindChild<Transform>("CancelBtn").OnUIMouseClick(delegate (object u) {
            GUIMgr.Instance.ExitModelGUI(this);
            this.ClosePanel();
        });
    }

    private void SetItemInfo(Transform obj, RewardItem _Itemdata)
    {
        if (_Itemdata != null)
        {
            UITexture component = obj.FindChild("Icon").GetComponent<UITexture>();
            UISprite sprite = obj.FindChild("QualityBorder").GetComponent<UISprite>();
            UISprite sprite2 = obj.FindChild("Border").GetComponent<UISprite>();
            UILabel label = obj.FindChild("Name").GetComponent<UILabel>();
            UILabel label2 = obj.FindChild("Count").GetComponent<UILabel>();
            GameObject gameObject = obj.FindChild("sprite").gameObject;
            GUIDataHolder.setData(obj.gameObject, _Itemdata);
            UIEventListener.Get(obj.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickItemBtn);
            switch (_Itemdata.type)
            {
                case ItemType.ItemType_Phyforce:
                    label.text = ConfigMgr.getInstance().GetWord(0x7a);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Phyforce");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    break;

                case ItemType.ItemType_Eq:
                    label.text = ConfigMgr.getInstance().GetWord(0x84);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_yqd");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    break;

                case ItemType.ItemType_Item:
                {
                    Item data = _Itemdata.Data as Item;
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(data.entry);
                    if (_config2 != null)
                    {
                        component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                        CommonFunc.SetEquipQualityBorder(sprite, _config2.quality, false);
                        label.text = _config2.name;
                        label2.text = data.diff.ToString();
                        if ((_config2.type == 3) || (_config2.type == 2))
                        {
                            gameObject.SetActive(true);
                        }
                        if (_config2.type == 3)
                        {
                            component.width = 0x69;
                            component.height = 0x69;
                        }
                        else
                        {
                            component.width = 0x55;
                            component.height = 0x55;
                        }
                    }
                    break;
                }
                case ItemType.ItemType_FlameBattleCoin:
                    label.text = ConfigMgr.getInstance().GetWord(0x83a);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Yuanzheng_Icon_gongxun");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    Debug.Log("RewardPanel FlameBattleCoin " + label2.text);
                    break;

                case ItemType.ItemType_OutlandCoin:
                    label.text = ConfigMgr.getInstance().GetWord(0x4e27);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Out_Icon_stone");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    Debug.Log("RewardPanel OutlandCoin " + label2.text);
                    break;

                case ItemType.ItemType_Arena_Ladder_Score:
                    label.text = ConfigMgr.getInstance().GetWord(0x88);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Pk_Icon_coin");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    Debug.Log("RewardPanel Arena Score " + label2.text);
                    break;

                case ItemType.ItemType_Guild_Contribute:
                    label.text = ConfigMgr.getInstance().GetWord(0x4e46);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Gonghui_Icon_ghjx");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    Debug.Log("RewardPanel Guild Contribute " + label2.text);
                    break;

                case ItemType.ItemType_LoL_Arena_Score:
                    label.text = ConfigMgr.getInstance().GetWord(0x9b);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Pk_Icon_coin1");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    Debug.Log("RewardPanel Arena Challenge Score " + label2.text);
                    break;

                case ItemType.ItemType_Gold:
                    label.text = ConfigMgr.getInstance().GetWord(0x89);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Gold");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    label2.text = _Itemdata.Data.ToString();
                    break;

                case ItemType.ItemType_Card:
                {
                    Card card = _Itemdata.Data as Card;
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.cardInfo.entry);
                    if (_config != null)
                    {
                        component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                        CommonFunc.SetEquipQualityBorder(sprite, _config.quality, false);
                        label.text = _config.name;
                        component.width = 0x69;
                        component.height = 0x69;
                    }
                    break;
                }
                case ItemType.ItemType_Stone:
                    label.text = ConfigMgr.getInstance().GetWord(0x31b);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
                    CommonFunc.SetEquipQualityBorder(sprite, 0, false);
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    break;
            }
        }
    }

    public static void ShowAchievementReward(S2C_CommitQuest res, Action<GUIEntity> action = null)
    {
        <ShowAchievementReward>c__AnonStorey19F storeyf = new <ShowAchievementReward>c__AnonStorey19F {
            res = res,
            action = action
        };
        GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeyf.<>m__21C), null);
    }

    public void ShowActiveReward(S2C_CollectExchange res)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(res.rewards.newCardList, res.rewards.item, res.rewards.gold - ActorData.getInstance().Gold, res.rewards.stone - ActorData.getInstance().Stone, (int) res.rewards.addPhyForce, 0, res.rewards.arenaLadderScore - ActorData.getInstance().ArenaLadderCoin, res.rewards.contribute - ActorData.getInstance().mUserGuildMemberData.contribution, res.rewards.flamebattleCoin - ActorData.getInstance().FlamebattleCoin, res.rewards.outland_coin - ActorData.getInstance().OutlandCoin, res.rewards.lolarenaScore - ActorData.getInstance().ArenaChallengeCoin);
        this.UpdateData(itemList);
    }

    public void ShowActiveReward(S2C_TXBuyStoreItem res)
    {
        this._TitleSprte.spriteName = "Ui_Tips_Label_gmcg";
        List<RewardItem> itemList = this.CreateRewardItemListByItem(res.rewards.newCardList, res.rewards.item, res.rewards.gold - ActorData.getInstance().Gold, res.rewards.stone - ActorData.getInstance().Stone, (int) res.rewards.addPhyForce, 0, res.rewards.arenaLadderScore - ActorData.getInstance().ArenaLadderCoin, res.rewards.contribute - ActorData.getInstance().mUserGuildMemberData.contribution, res.rewards.flamebattleCoin - ActorData.getInstance().FlamebattleCoin, res.rewards.outland_coin - ActorData.getInstance().OutlandCoin, res.rewards.lolarenaScore - ActorData.getInstance().ArenaChallengeCoin);
        this.UpdateData(itemList);
    }

    public void ShowActiveReward0(S2C_DrawActivityPrize res)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(res.rewards.newCardList, res.rewards.item, res.rewards.gold - ActorData.getInstance().Gold, res.rewards.stone - ActorData.getInstance().Stone, (int) res.rewards.addPhyForce, 0, res.rewards.arenaLadderScore - ActorData.getInstance().ArenaLadderCoin, res.rewards.contribute - ActorData.getInstance().mUserGuildMemberData.contribution, res.rewards.flamebattleCoin - ActorData.getInstance().FlamebattleCoin, res.rewards.outland_coin - ActorData.getInstance().OutlandCoin, res.rewards.lolarenaScore - ActorData.getInstance().ArenaChallengeCoin);
        this.UpdateData(itemList);
    }

    internal static void ShowActivityShopReward(ShopBuyResult shopBuyResult)
    {
        <ShowActivityShopReward>c__AnonStorey1A2 storeya = new <ShowActivityShopReward>c__AnonStorey1A2 {
            buyResult = shopBuyResult
        };
        GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeya.<>m__21F), null);
    }

    public void ShowBattleReward(BattleReward res)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(res.cards, res.items, (int) res.gold, (int) res.stone, (int) res.phyforce, (int) res.eq, (int) res.add_arena_ladder_score, (int) res.add_contribution, (int) res.add_flame_battle_coin, (int) res.outland_coin, 0);
        this.UpdateData(itemList);
    }

    public static void ShowCourageShopReward(S2C_CourageShopBuy res)
    {
        <ShowCourageShopReward>c__AnonStorey1A1 storeya = new <ShowCourageShopReward>c__AnonStorey1A1 {
            res = res
        };
        GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeya.<>m__21E), null);
    }

    public static void ShowDailyReward(UniversialReward reward, int level, int exp, int phy, Action<GUIEntity> action = null)
    {
        <ShowDailyReward>c__AnonStorey19E storeye = new <ShowDailyReward>c__AnonStorey19E {
            reward = reward,
            exp = exp,
            level = level,
            phy = phy,
            action = action
        };
        GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeye.<>m__21B), null);
    }

    public void ShowDetainsDartEscortReward(S2C_ConvoyEnd res)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(null, null, res.incGold, 0, 0, 0, res.incArenaLadderScore, 0, 0, 0, 0);
        this.UpdateData(itemList);
        ActorData.getInstance().Gold = res.currGold;
        ActorData.getInstance().ArenaLadderCoin = res.arenaLadderScore;
    }

    public void ShowExchangeReward(S2C_PickGift res)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(res.cardList, res.itemList, res.gold - ActorData.getInstance().Gold, res.stone - ActorData.getInstance().Stone, res.phyForce - ActorData.getInstance().PhyForce, res.eq, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
    }

    public void ShowHongBaoReward(int stone)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(null, null, 0, stone, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
    }

    public void ShowItemList(List<Item> _dataList, int _Cost)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(null, _dataList, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
        base.gameObject.transform.FindChild("Tips").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa344e4), _Cost);
    }

    internal void ShowLifeSkillCollectReward(S2C_NewLifeSkillEndHangUp result)
    {
        List<Item> list = new List<Item>();
        list.AddRange(result.items);
        List<RewardItem> itemList = this.CreateRewardItemListByItem(null, list, result.add_gold, 0, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
        foreach (Item item in result.items)
        {
            ActorData.getInstance().UpdateItem(item);
        }
        if (result.add_gold > 0)
        {
            ActorData data1 = ActorData.getInstance();
            data1.Gold += result.add_gold;
        }
    }

    internal void ShowLifeSkillRecvFriendReward(S2C_NewLifeRecvFriendReward result)
    {
        List<Item> list = new List<Item>();
        list.AddRange(result.items);
        List<RewardItem> itemList = this.CreateRewardItemListByItem(null, list, result.add_gold, 0, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
        foreach (Item item in result.items)
        {
            ActorData.getInstance().UpdateItem(item);
        }
        if (result.add_gold > 0)
        {
            ActorData data1 = ActorData.getInstance();
            data1.Gold += result.add_gold;
        }
    }

    public static void ShowLivenessReward(S2C_PickLivenessReward res)
    {
        <ShowLivenessReward>c__AnonStorey1A0 storeya = new <ShowLivenessReward>c__AnonStorey1A0 {
            res = res
        };
        LittleHelperPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<LittleHelperPanel>();
        GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeya.<>m__21D), (gUIEntity == null) ? null : gUIEntity.gameObject);
    }

    public void ShowMailPickAffix(S2C_PickMailAffix res)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(res.newCard, res.itemList, res.gold - ActorData.getInstance().Gold, res.stone - ActorData.getInstance().Stone, (int) res.addPhyForce, res.eq - ActorData.getInstance().Eq, res.arenaLadderScore - ActorData.getInstance().ArenaLadderCoin, res.contribute - ActorData.getInstance().mUserGuildMemberData.contribution, res.flamebattleCoin - ActorData.getInstance().FlamebattleCoin, res.outland_coin - ActorData.getInstance().OutlandCoin, res.lolarenaScore - ActorData.getInstance().ArenaChallengeCoin);
        Debug.Log(string.Concat(new object[] { "Guild Contribute: ", res.contribute, "  ", ActorData.getInstance().mUserGuildMemberData.contribution }));
        this.UpdateData(itemList);
    }

    public void ShowOutlandBattleReward(S2C_OutlandDropEvent dropRes, S2C_OutlandGetFloorBoxReward boxRes)
    {
        if (dropRes != null)
        {
            BattleReward reward = dropRes.reward;
            int num = dropRes.outland_coin;
            List<RewardItem> itemList = this.CreateRewardItemListByItem(reward.cards, reward.items, dropRes.gold - ActorData.getInstance().Gold, (int) reward.stone, 0, 0, 0, 0, 0, 0, 0);
            int num2 = num - ActorData.getInstance().OutlandCoin;
            ActorData.getInstance().OutlandCoin = num;
            ActorData.getInstance().Gold = dropRes.gold;
            if (num2 > 0)
            {
                RewardItem item = new RewardItem {
                    type = ItemType.ItemType_OutlandCoin,
                    Data = num2
                };
                itemList.Add(item);
            }
            ActorData.getInstance().UpdateBattleRewardData(reward);
            this.UpdateData(itemList);
        }
        if (boxRes != null)
        {
            BattleReward reward2 = boxRes.reward;
            int num3 = boxRes.outland_coin;
            List<RewardItem> list2 = this.CreateRewardItemListByItem(reward2.cards, reward2.items, boxRes.gold - ActorData.getInstance().Gold, (int) reward2.stone, 0, 0, 0, 0, 0, 0, 0);
            int num4 = num3 - ActorData.getInstance().OutlandCoin;
            ActorData.getInstance().OutlandCoin = num3;
            ActorData.getInstance().Gold = boxRes.gold;
            if (num4 > 0)
            {
                RewardItem item2 = new RewardItem {
                    type = ItemType.ItemType_OutlandCoin,
                    Data = num4
                };
                list2.Add(item2);
            }
            ActorData.getInstance().UpdateBattleRewardData(reward2);
            this.UpdateData(list2);
        }
    }

    public static void ShowPickDuplicateReward(S2C_PickDuplicateReward reward)
    {
        <ShowPickDuplicateReward>c__AnonStorey19D storeyd = new <ShowPickDuplicateReward>c__AnonStorey19D {
            reward = reward
        };
        GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeyd.<>m__21A), null);
    }

    internal void ShowRichActivityCollectReward(S2C_LifeSkillCollect reslut)
    {
        List<Item> list = new List<Item>();
        list.AddRange(reslut.items);
        List<RewardItem> itemList = this.CreateRewardItemListByItem(null, list, reslut.gold, 0, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
        foreach (Item item in reslut.items)
        {
            ActorData.getInstance().UpdateItem(item);
        }
        if (reslut.gold > 0)
        {
            ActorData data1 = ActorData.getInstance();
            data1.Gold += reslut.gold;
        }
    }

    public void ShowRichActivityTryReward(Item item)
    {
        List<Item> list = new List<Item> {
            item
        };
        List<RewardItem> itemList = this.CreateRewardItemListByItem(null, list, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
        ActorData.getInstance().UpdateItem(item);
    }

    public void ShowSignInReward(S2C_Registration res)
    {
        <ShowSignInReward>c__AnonStorey19C storeyc = new <ShowSignInReward>c__AnonStorey19C {
            allItem = new List<Item>()
        };
        res.itemList.ForEach(new Action<Item>(storeyc.<>m__218));
        res.itemListForVip.ForEach(new Action<Item>(storeyc.<>m__219));
        List<RewardItem> itemList = this.CreateRewardItemListByItem(res.newCardList, storeyc.allItem, res.gold - ActorData.getInstance().Gold, res.stone - ActorData.getInstance().Stone, 0, 0, 0, 0, 0, 0, 0);
        this.UpdateData(itemList);
    }

    public void ShowTips()
    {
        base.gameObject.transform.FindChild("Label").gameObject.SetActive(true);
    }

    public void ShowUseItemDropReward(S2C_UseItem res)
    {
        List<RewardItem> itemList = this.CreateRewardItemListByItem(res.reward.cards, res.reward.items, ((int) res.reward.gold) - ActorData.getInstance().Gold, ((int) res.reward.stone) - ActorData.getInstance().Stone, (int) res.reward.phyforce, 0, (int) res.reward.add_arena_ladder_score, (int) res.reward.add_contribution, (int) res.reward.add_flame_battle_coin, (int) res.reward.outland_coin, 0);
        this.UpdateData(itemList);
    }

    public void ShowYuanZhengBattleReward(S2C_GainFlameBattleReward res)
    {
        this._TitleSprte.spriteName = "Ui_Tips_Label_lqcg";
        this._TitleSprte.MakePixelPerfect();
        BattleReward reward = res.reward;
        int flamebattleCoin = res.flamebattleCoin;
        List<RewardItem> itemList = this.CreateRewardItemListByItem(reward.cards, reward.items, res.gold - ActorData.getInstance().Gold, (int) reward.stone, 0, 0, 0, 0, 0, 0, 0);
        int num2 = flamebattleCoin - ActorData.getInstance().FlamebattleCoin;
        ActorData.getInstance().FlamebattleCoin = flamebattleCoin;
        ActorData.getInstance().Gold = res.gold;
        if (num2 > 0)
        {
            RewardItem item = new RewardItem {
                type = ItemType.ItemType_FlameBattleCoin,
                Data = num2
            };
            itemList.Add(item);
        }
        ActorData.getInstance().UpdateBattleRewardData(reward);
        this.UpdateData(itemList);
    }

    private void UpdateData(List<RewardItem> itemList)
    {
        UIGrid component = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        Debug.Log("===========>" + itemList.Count);
        if (itemList.Count != 0)
        {
            int num = itemList.Count / 4;
            if ((itemList.Count % 4) != 0)
            {
                num++;
            }
            int count = itemList.Count;
            int num3 = 0;
            float y = 0f;
            for (int i = 0; i < num; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleRewardItem) as GameObject;
                obj2.transform.parent = component.transform;
                obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                int num6 = 0;
                for (int j = 0; j < 4; j++)
                {
                    Transform transform = obj2.transform.FindChild("Item" + (j + 1));
                    if (num3 < count)
                    {
                        this.SetItemInfo(transform, itemList[num3]);
                        num6++;
                        num3++;
                    }
                    else
                    {
                        transform.gameObject.SetActive(false);
                    }
                }
                obj2.transform.localPosition = new Vector3((float) ((4 - num6) * 70), y, -0.1f);
                y -= component.cellHeight;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1A3
    {
        internal Item info;

        internal void <>m__220(GUIEntity obj)
        {
            ((ItemInfoPanel) obj).UpdateData(this.info);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1A4
    {
        internal Card info;

        internal void <>m__221(GUIEntity obj)
        {
            ((ItemInfoPanel) obj).ShowCardInfo(this.info);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowAchievementReward>c__AnonStorey19F
    {
        internal Action<GUIEntity> action;
        internal S2C_CommitQuest res;

        internal void <>m__21C(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            List<RewardPanel.RewardItem> itemList = panel.CreateRewardItemListByItem(this.res.cardList, this.res.itemList, this.res.gold - ActorData.getInstance().Gold, this.res.stone - ActorData.getInstance().Stone, 0, 0, 0, 0, 0, 0, 0);
            panel.UpdateData(itemList);
            ActorData.getInstance().UserInfo.exp = this.res.exp;
            ActorData.getInstance().Level = this.res.level;
            ActorData.getInstance().Stone = this.res.stone;
            ActorData.getInstance().Gold = this.res.gold;
            ActorData.getInstance().PhyForce = this.res.phyForce;
            ActorData.getInstance().UpdateNewCard(this.res.cardList);
            ActorData.getInstance().UpdateItemList(this.res.itemList);
            ActorData.getInstance().AddTitle(this.res.newTitle);
            if (this.action != null)
            {
                this.action(entity);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ShowActivityShopReward>c__AnonStorey1A2
    {
        internal ShopBuyResult buyResult;

        internal void <>m__21F(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            panel._TitleSprte.spriteName = "Ui_Tips_Label_gmcg";
            List<Item> list = new List<Item> {
                this.buyResult.item
            };
            List<RewardPanel.RewardItem> itemList = panel.CreateRewardItemListByItem(this.buyResult.cardList, list, this.buyResult.currency_info.gold - ActorData.getInstance().Gold, this.buyResult.currency_info.stone - ActorData.getInstance().Stone, 0, 0, 0, 0, 0, 0, 0);
            panel.UpdateData(itemList);
            ActorData.getInstance().Stone = this.buyResult.currency_info.stone;
            ActorData.getInstance().Gold = this.buyResult.currency_info.gold;
            ActorData.getInstance().UpdateNewCard(this.buyResult.cardList);
            ActorData.getInstance().UpdateItem(this.buyResult.item);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowCourageShopReward>c__AnonStorey1A1
    {
        internal S2C_CourageShopBuy res;

        internal void <>m__21E(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            panel._TitleSprte.spriteName = "Ui_Tips_Label_gmcg";
            panel._TitleSprte.MakePixelPerfect();
            List<Item> list = new List<Item> {
                this.res.buyResult.item
            };
            List<RewardPanel.RewardItem> itemList = panel.CreateRewardItemListByItem(this.res.buyResult.cardList, list, this.res.buyResult.currency_info.gold - ActorData.getInstance().Gold, this.res.buyResult.currency_info.stone - ActorData.getInstance().Stone, 0, 0, 0, 0, 0, 0, 0);
            panel.UpdateData(itemList);
            ActorData.getInstance().Stone = this.res.buyResult.currency_info.stone;
            ActorData.getInstance().Gold = this.res.buyResult.currency_info.gold;
            ActorData.getInstance().UpdateNewCard(this.res.buyResult.cardList);
            ActorData.getInstance().UpdateItem(this.res.buyResult.item);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowDailyReward>c__AnonStorey19E
    {
        internal Action<GUIEntity> action;
        internal int exp;
        internal int level;
        internal int phy;
        internal UniversialReward reward;

        internal void <>m__21B(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            List<RewardPanel.RewardItem> itemList = panel.CreateRewardItemListByItem(this.reward.newCardList, this.reward.item, this.reward.gold - ActorData.getInstance().Gold, this.reward.stone - ActorData.getInstance().Stone, (int) this.reward.addPhyForce, 0, 0, 0, 0, 0, 0);
            panel.UpdateData(itemList);
            ActorData.getInstance().UserInfo.exp = this.exp;
            ActorData.getInstance().Level = this.level;
            ActorData.getInstance().PhyForce = this.phy;
            ActorData.getInstance().Stone = this.reward.stone;
            ActorData.getInstance().Gold = this.reward.gold;
            ActorData.getInstance().UpdateNewCard(this.reward.newCardList);
            ActorData.getInstance().UpdateItemList(this.reward.item);
            if (this.action != null)
            {
                this.action(entity);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ShowLivenessReward>c__AnonStorey1A0
    {
        internal S2C_PickLivenessReward res;

        internal void <>m__21D(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            List<Item> list = new List<Item> {
                this.res.item
            };
            int num = 0;
            liveness_reward_config _config = ConfigMgr.getInstance().getByEntry<liveness_reward_config>(this.res.rewardEntry);
            if ((_config != null) && (_config.reward_type == 4))
            {
                num = _config.reward_count;
            }
            List<RewardPanel.RewardItem> itemList = panel.CreateRewardItemListByItem(null, list, this.res.gold - ActorData.getInstance().Gold, this.res.stone - ActorData.getInstance().Stone, num, 0, 0, 0, 0, 0, 0);
            panel.UpdateData(itemList);
            ActorData.getInstance().UserInfo.exp = this.res.exp;
            ActorData.getInstance().Level = this.res.level;
            ActorData.getInstance().Stone = this.res.stone;
            ActorData.getInstance().Gold = this.res.gold;
            ActorData.getInstance().PhyForce = this.res.phyForce;
            ActorData.getInstance().UpdateItemList(list);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowPickDuplicateReward>c__AnonStorey19D
    {
        internal S2C_PickDuplicateReward reward;

        internal void <>m__21A(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            List<RewardPanel.RewardItem> itemList = panel.CreateRewardItemListByItem(null, this.reward.itemList, ((int) this.reward.gold) - ActorData.getInstance().Gold, ((int) this.reward.stone) - ActorData.getInstance().Stone, 0, 0, 0, 0, 0, 0, 0);
            panel.UpdateData(itemList);
            ActorData.getInstance().Stone = (int) this.reward.stone;
            ActorData.getInstance().Gold = (int) this.reward.gold;
            ActorData.getInstance().UpdateItemList(this.reward.itemList);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowSignInReward>c__AnonStorey19C
    {
        internal List<Item> allItem;

        internal void <>m__218(Item obj)
        {
            this.allItem.Add(obj);
        }

        internal void <>m__219(Item obj)
        {
            this.allItem.Add(obj);
        }
    }

    private class RewardItem
    {
        public object Data;
        public ItemType type;
    }
}

