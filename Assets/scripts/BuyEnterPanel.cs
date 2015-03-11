using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;

public class BuyEnterPanel : GUIEntity
{
    public System.Action _CallBackOkBtn;
    private int currBuyItemCount;
    private int currItemCount;
    private int mItemMaxPileNum = 0x3e7;

    private void CheckCardBreakTips(int itemEntry)
    {
        Transform transform = base.transform.FindChild("CardGroup");
        List<Card> cardListByLackOneItemByEquip = XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(itemEntry, 1);
        for (int i = 0; i < 5; i++)
        {
            Transform transform2 = transform.FindChild("Card" + (i + 1));
            if (i < cardListByLackOneItemByEquip.Count)
            {
                UITexture component = transform2.FindChild("Item/Icon").GetComponent<UITexture>();
                UISprite sprite = transform2.FindChild("Item/frame").GetComponent<UISprite>();
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) cardListByLackOneItemByEquip[i].cardInfo.entry);
                if (_config != null)
                {
                    component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    CommonFunc.SetQualityBorder(sprite, cardListByLackOneItemByEquip[i].cardInfo.quality);
                    transform2.gameObject.SetActive(true);
                }
                else
                {
                    transform2.gameObject.SetActive(false);
                }
            }
            else
            {
                transform2.gameObject.SetActive(false);
            }
        }
        transform.gameObject.SetActive(cardListByLackOneItemByEquip.Count > 0);
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

    public void OnClickOkBtn(GameObject go)
    {
        if ((this.currItemCount >= this.mItemMaxPileNum) || ((this.currItemCount + this.currBuyItemCount) >= this.mItemMaxPileNum))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e57));
        }
        else if (this._CallBackOkBtn != null)
        {
            this._CallBackOkBtn();
        }
    }

    public override void OnInitialize()
    {
        UIEventListener.Get(base.transform.FindChild("OkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOkBtn);
    }

    public void UpateData(ShopCoinType _coinType, ShopItem si, System.Action act)
    {
        base.Depth = 300;
        this._CallBackOkBtn = act;
        base.transform.FindChild("BuyInfo/LabelTips").GetComponent<UILabel>().text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x4fd), GameConstant.DefaultTextRedColor + si.stackCount + GameConstant.DefaultTextColor);
        this.currBuyItemCount = si.stackCount;
        base.transform.FindChild("BuyInfo/Price").GetComponent<UILabel>().text = si.cost.ToString();
        UISprite component = base.transform.FindChild("BuyInfo/Icon").GetComponent<UISprite>();
        int entry = -1;
        switch (_coinType)
        {
            case ShopCoinType.YuanZhengCoin:
            {
                component.spriteName = "Ui_Yuanzheng_Icon_gongxun";
                flame_shop_config _config3 = ConfigMgr.getInstance().getByEntry<flame_shop_config>(si.entry);
                if (_config3 != null)
                {
                    entry = _config3.goods_entry;
                }
                break;
            }
            case ShopCoinType.ArenaLadderCoin:
            {
                component.spriteName = "Ui_Pk_Icon_coin";
                arena_shop_config _config = ConfigMgr.getInstance().getByEntry<arena_shop_config>(si.entry);
                if (_config != null)
                {
                    entry = _config.goods_entry;
                }
                break;
            }
            case ShopCoinType.OutLandCoin:
            {
                component.spriteName = "Ui_Out_Icon_stone";
                outland_shop_config _config4 = ConfigMgr.getInstance().getByEntry<outland_shop_config>(si.entry);
                if (_config4 != null)
                {
                    entry = _config4.goods_entry;
                }
                break;
            }
            case ShopCoinType.CourageShopGold:
            {
                component.spriteName = "Item_Icon_Gold";
                courage_shop_config _config5 = ConfigMgr.getInstance().getByEntry<courage_shop_config>(si.entry);
                if (_config5 != null)
                {
                    entry = _config5.goods_entry;
                }
                break;
            }
            case ShopCoinType.CourageShopStone:
            {
                component.spriteName = "Item_Icon_Stone";
                courage_shop_config _config6 = ConfigMgr.getInstance().getByEntry<courage_shop_config>(si.entry);
                if (_config6 != null)
                {
                    entry = _config6.goods_entry;
                }
                break;
            }
            case ShopCoinType.GoblinShopGold:
            case ShopCoinType.GoblinShopStone:
            {
                component.spriteName = (_coinType != ShopCoinType.GoblinShopGold) ? "Item_Icon_Stone" : "Item_Icon_Gold";
                goblin_shop_config _config7 = ConfigMgr.getInstance().getByEntry<goblin_shop_config>(si.entry);
                if (_config7 != null)
                {
                    entry = _config7.goods_entry;
                }
                break;
            }
            case ShopCoinType.SecretShopGold:
            case ShopCoinType.SecretShopStone:
            {
                component.spriteName = (_coinType != ShopCoinType.SecretShopGold) ? "Item_Icon_Stone" : "Item_Icon_Gold";
                secret_shop_config _config8 = ConfigMgr.getInstance().getByEntry<secret_shop_config>(si.entry);
                if (_config8 != null)
                {
                    entry = _config8.goods_entry;
                }
                break;
            }
            case ShopCoinType.GuildCoin:
                component.spriteName = "Ui_Gonghui_Icon_ghjx";
                entry = si.entry;
                break;

            case ShopCoinType.ArenaChallengeCoin:
            {
                component.spriteName = "Ui_Pk_Icon_coin1";
                lolarena_shop_config _config2 = ConfigMgr.getInstance().getByEntry<lolarena_shop_config>(si.entry);
                if (_config2 != null)
                {
                    entry = _config2.goods_entry;
                }
                break;
            }
        }
        UITexture texture = base.transform.FindChild("Item/Icon").GetComponent<UITexture>();
        UISprite sprite2 = base.transform.FindChild("Item/QualityBorder").GetComponent<UISprite>();
        UILabel label3 = base.transform.FindChild("Info/Name").GetComponent<UILabel>();
        UILabel label4 = base.transform.FindChild("Info/Count").GetComponent<UILabel>();
        UILabel label5 = base.transform.FindChild("Describe/Desc").GetComponent<UILabel>();
        Transform transform = base.transform.FindChild("Item/Chip");
        Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(entry);
        int num2 = (itemByEntry != null) ? itemByEntry.num : 0;
        label4.text = num2.ToString();
        this.currItemCount = num2;
        item_config _config9 = ConfigMgr.getInstance().getByEntry<item_config>(entry);
        if (_config9 != null)
        {
            if (_config9.max_pile_num > 0)
            {
                this.mItemMaxPileNum = _config9.max_pile_num;
            }
            else
            {
                this.mItemMaxPileNum = 0x3e7;
            }
            this.CheckCardBreakTips(entry);
            Debug.Log(string.Concat(new object[] { "ItemEntry:", entry, "  Name:", _config9.name }));
            texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config9.icon);
            CommonFunc.SetEquipQualityBorder(sprite2, _config9.quality, false);
            label3.text = _config9.name;
            int combCardNeedCount = -1;
            if (_config9.type == 3)
            {
                combCardNeedCount = this.GetCombCardNeedCount(entry);
                string str = (num2 < combCardNeedCount) ? string.Empty : "[015b17]";
                object[] objArray2 = new object[] { _config9.describe, GameConstant.DefaultTextColor, "\n\n", ConfigMgr.getInstance().GetWord(0x4ee), ": ", str, num2, "/", combCardNeedCount };
                label5.text = string.Concat(objArray2);
            }
            else if (_config9.type == 2)
            {
                item_config _config10 = ConfigMgr.getInstance().getByEntry<item_config>(_config9.param_0);
                if (_config10 != null)
                {
                    combCardNeedCount = _config10.param_1;
                    string str2 = (num2 < combCardNeedCount) ? string.Empty : "[015b17]";
                    object[] objArray3 = new object[] { _config9.describe, GameConstant.DefaultTextColor, "\n\n", ConfigMgr.getInstance().GetWord(0x4ee), ": ", str2, num2, "/", combCardNeedCount };
                    label5.text = string.Concat(objArray3);
                }
            }
            else
            {
                label5.text = _config9.describe;
            }
            transform.gameObject.SetActive((_config9.type == 2) || (_config9.type == 3));
        }
    }
}

