using FastBuf;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SellPanel : GUIEntity
{
    private int mCurrSellCount;
    private int mMaxSellCount;
    private int mSellItemEntry;
    private int mSinglePrice;

    private void OnClickAddBtn(GameObject go)
    {
        if (this.mCurrSellCount < this.mMaxSellCount)
        {
            UILabel component = base.transform.FindChild("Count/Label").GetComponent<UILabel>();
            this.mCurrSellCount++;
            component.text = this.mCurrSellCount + "/" + this.mMaxSellCount;
            this.SetSellGold();
        }
    }

    private void OnClickMaxBtn(GameObject go)
    {
        base.transform.FindChild("Count/Label").GetComponent<UILabel>().text = this.mMaxSellCount + "/" + this.mMaxSellCount;
        this.mCurrSellCount = this.mMaxSellCount;
        this.SetSellGold();
    }

    private void OnClickSellBtn(GameObject go)
    {
        List<Item> items = new List<Item>();
        Item item = new Item {
            entry = this.mSellItemEntry,
            num = this.mCurrSellCount
        };
        items.Add(item);
        SocketMgr.Instance.RequestSellItem(items);
        GUIMgr.Instance.ExitModelGUI("SellPanel");
    }

    private void OnClickSubBtn(GameObject go)
    {
        if (this.mCurrSellCount > 1)
        {
            UILabel component = base.transform.FindChild("Count/Label").GetComponent<UILabel>();
            this.mCurrSellCount--;
            component.text = this.mCurrSellCount + "/" + this.mMaxSellCount;
            this.SetSellGold();
        }
    }

    private void SetSellGold()
    {
        base.transform.FindChild("GetGold/Count").GetComponent<UILabel>().text = (this.mSinglePrice * this.mCurrSellCount).ToString();
    }

    public void SetSellItemData(Item _item)
    {
        if (_item != null)
        {
            this.mSellItemEntry = _item.entry;
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_item.entry);
            if (_config != null)
            {
                base.transform.FindChild("Item/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                CommonFunc.SetEquipQualityBorder(base.transform.FindChild("Item/QualityBorder").GetComponent<UISprite>(), _config.quality, false);
                base.transform.FindChild("Item/sprite").GetComponent<UISprite>().gameObject.SetActive((_config.type == 2) || (_config.type == 3));
                Debug.Log(_config.type + ",,,,");
                base.transform.FindChild("Info/Count").GetComponent<UILabel>().text = _item.num.ToString();
                this.mMaxSellCount = _item.num;
                base.transform.FindChild("Count/Label").GetComponent<UILabel>().text = "1/" + _item.num;
                this.mCurrSellCount = 1;
                base.transform.FindChild("Info/Name").GetComponent<UILabel>().text = _config.name;
                base.transform.FindChild("SellInfo/Price").GetComponent<UILabel>().text = _config.sell_price.ToString();
                base.transform.FindChild("GetGold/Count").GetComponent<UILabel>().text = _config.sell_price.ToString();
                this.mSinglePrice = _config.sell_price;
                UIEventListener.Get(base.transform.FindChild("Count/AddBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAddBtn);
                UIEventListener.Get(base.transform.FindChild("Count/SubBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSubBtn);
                UIEventListener.Get(base.transform.FindChild("Count/MaxBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMaxBtn);
                UIEventListener.Get(base.transform.FindChild("SellBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSellBtn);
            }
        }
    }
}

