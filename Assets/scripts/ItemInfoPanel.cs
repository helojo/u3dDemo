using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class ItemInfoPanel : GUIEntity
{
    private void OnClickCloseBtn(GameObject go)
    {
        GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(base.transform.FindChild("Group/OkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCloseBtn);
    }

    public void ShowCardInfo(Card _card)
    {
        if (_card != null)
        {
            this.ShowCardInfo((int) _card.cardInfo.entry, _card.cardInfo.starLv, true);
        }
    }

    public void ShowCardInfo(int cardEntry, int starLv = 1, bool showStar = true)
    {
        <ShowCardInfo>c__AnonStorey19B storeyb = new <ShowCardInfo>c__AnonStorey19B {
            cardEntry = cardEntry
        };
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(storeyb.cardEntry);
        if (_config != null)
        {
            Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) storeyb.cardEntry);
            if (cardByEntry != null)
            {
                starLv = cardByEntry.cardInfo.starLv;
            }
            Transform transform = base.transform.FindChild("Group");
            UITexture component = transform.FindChild("Item/Icon").GetComponent<UITexture>();
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            component.height = 110;
            component.width = 110;
            CommonFunc.SetEquipQualityBorder(transform.FindChild("Item/QualityBorder").GetComponent<UISprite>(), _config.quality, false);
            transform.FindChild("Info/Name").GetComponent<UILabel>().text = _config.name;
            transform.FindChild("Info/Count").GetComponent<UILabel>().text = (ActorData.getInstance().CardList.Find(new Predicate<Card>(storeyb.<>m__216)) == null) ? "0" : "1";
            UILabel label3 = transform.FindChild("Describe/Desc").GetComponent<UILabel>();
            label3.text = _config.describe.Replace("[4d3325]", "[ffffff]");
            UISprite sprite2 = transform.FindChild("Bg").GetComponent<UISprite>();
            sprite2.height = (label3.height >= 0x38) ? (label3.height + 0x97) : sprite2.height;
            for (int i = 0; i < 5; i++)
            {
                UISprite sprite3 = transform.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                sprite3.gameObject.SetActive(i < starLv);
                sprite3.transform.localPosition = new Vector3((float) (i * 0x12), 0f, 0f);
            }
            Transform transform2 = transform.transform.FindChild("Star");
            transform2.localPosition = new Vector3(transform2.localPosition.x - ((starLv - 1) * 9), transform2.localPosition.y, 0f);
            transform2.gameObject.SetActive(showStar);
        }
    }

    public void ShowItemInfo(Item _item, List<Card> _cardList, Transform _clickObj)
    {
        if (_item != null)
        {
            this.UpdateData(_item.entry, _cardList);
            Transform transform = base.transform.FindChild("Group");
            Vector3 message = BattleGlobalFunc.GUIToWorld(_clickObj.position);
            Debug.Log(message);
            transform.localPosition = new Vector3(message.x - 100f, message.y, 0f);
        }
    }

    public void SoulBoxShowCard(int cardEntry, int starLv, bool showStar, bool _isChip = true)
    {
        this.ShowCardInfo(cardEntry, starLv, showStar);
        base.transform.FindChild("Group/Info/Count").GetComponent<UILabel>().text = string.Empty;
        base.transform.FindChild("Group/Item/Chip").gameObject.SetActive(_isChip);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(cardEntry);
        if (_config != null)
        {
            base.transform.FindChild("Group/Describe/Desc").GetComponent<UILabel>().text = _config.describe.Replace("[4d3325]", "[ffffff]");
            UILabel component = base.transform.FindChild("Group/Info/Label").GetComponent<UILabel>();
            card_ex_config cardExCfg = CommonFunc.GetCardExCfg(cardEntry, _config.evolve_lv);
            if (cardExCfg != null)
            {
                Item itemByEntry = ActorData.getInstance().GetItemByEntry(cardExCfg.item_entry);
                int num = (itemByEntry != null) ? itemByEntry.num : 0;
                string str = (num < cardExCfg.combine_need_item_num) ? "[ff0000]" : "[00ff00]";
                object[] objArray1 = new object[] { ConfigMgr.getInstance().GetWord(0x508), ":", str, num, "/", cardExCfg.combine_need_item_num };
                component.text = string.Concat(objArray1);
            }
            else
            {
                component.text = ConfigMgr.getInstance().GetWord(0x508) + ":[ff0000]0";
            }
        }
    }

    public void UpdateData(Item _item)
    {
        if (_item != null)
        {
            this.UpdateData(_item.entry, null);
        }
    }

    public void UpdateData(Item _item, Transform _clickObj)
    {
        if (_item != null)
        {
            this.UpdateData(_item.entry, null);
            Transform transform = base.transform.FindChild("Group");
            Vector3 message = BattleGlobalFunc.GUIToWorld(_clickObj.position);
            Debug.Log(message);
            transform.localPosition = new Vector3(message.x - 100f, message.y, 0f);
        }
    }

    public void UpdateData(int entry, List<Card> _cardList = null)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(entry);
        if (_config != null)
        {
            Transform transform = base.transform.FindChild("Group");
            transform.FindChild("Item/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            CommonFunc.SetEquipQualityBorder(transform.FindChild("Item/QualityBorder").GetComponent<UISprite>(), _config.quality, false);
            transform.FindChild("Info/Name").GetComponent<UILabel>().text = _config.name;
            Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(entry);
            transform.FindChild("Info/Count").GetComponent<UILabel>().text = ((itemByEntry != null) ? itemByEntry.num : 0).ToString();
            UILabel component = transform.FindChild("Describe/Desc").GetComponent<UILabel>();
            component.text = _config.describe;
            UISprite sprite2 = transform.FindChild("Bg").GetComponent<UISprite>();
            sprite2.height = (component.height >= 0x38) ? (component.height + 0x97) : sprite2.height;
            if (component.height > 0x38)
            {
                transform.localPosition = new Vector3(0f, (float) ((component.height - 0x38) / 2), 0f);
            }
            transform.FindChild("SellInfo/Price").GetComponent<UILabel>().text = _config.sell_price.ToString();
            transform.FindChild("Item/sprite").gameObject.SetActive((_config.type == 2) || (_config.type == 3));
            Transform transform2 = base.transform.FindChild("Group/CardGroup");
            transform2.transform.localPosition = new Vector3(transform2.transform.localPosition.x, (float) (-86 - (component.height - 0x1c)), 0f);
            if (_cardList == null)
            {
                _cardList = XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(entry, 1);
            }
            for (int i = 0; i < 4; i++)
            {
                Transform transform3 = transform2.FindChild((i + 1).ToString());
                if (i < _cardList.Count)
                {
                    UITexture texture2 = transform3.FindChild("Icon").GetComponent<UITexture>();
                    UISprite sprite3 = transform3.FindChild("frame").GetComponent<UISprite>();
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) _cardList[i].cardInfo.entry);
                    if (_config2 != null)
                    {
                        texture2.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                        CommonFunc.SetQualityBorder(sprite3, _cardList[i].cardInfo.quality);
                        transform3.gameObject.SetActive(true);
                    }
                    else
                    {
                        transform3.gameObject.SetActive(false);
                    }
                }
                else
                {
                    transform3.gameObject.SetActive(false);
                }
            }
            if (_cardList.Count > 0)
            {
                sprite2.height += 0x4b;
            }
            transform2.gameObject.SetActive(_cardList.Count > 0);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowCardInfo>c__AnonStorey19B
    {
        internal int cardEntry;

        internal bool <>m__216(Card e)
        {
            return (e.cardInfo.entry == this.cardEntry);
        }
    }
}

