using FastBuf;
using System;
using System.Collections;
using UnityEngine;

public class FragmentCombinePanel : GUIEntity
{
    private int mDestEntry = -1;
    private int mItemCount;
    private int mNeedCount;
    private int mTargetEntry;
    private int mTargeType;

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
        if (id > 0)
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

    private void OnClickCombineBtn(GameObject go)
    {
        if ((this.mNeedCount != 0) && (this.mItemCount >= this.mNeedCount))
        {
            Item itemByEntry = ActorData.getInstance().GetItemByEntry(this.mDestEntry);
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.mDestEntry);
            if (_config != null)
            {
                if ((itemByEntry != null) && (itemByEntry.num >= 0x3e7))
                {
                    TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x4e58), _config.name));
                }
                else
                {
                    SocketMgr.Instance.RequestItemMachining(this.mTargeType, this.mTargetEntry);
                }
            }
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a67));
        }
    }

    public void UpdateData(Item _item)
    {
        if (_item != null)
        {
            this.mItemCount = _item.num;
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_item.entry);
            if (_config != null)
            {
                this.mDestEntry = _config.param_0;
                base.transform.FindChild("Item/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                base.transform.FindChild("Info/Count").GetComponent<UILabel>().text = _item.num.ToString();
                CommonFunc.SetEquipQualityBorder(base.transform.FindChild("Item/QualityBorder").GetComponent<UISprite>(), _config.quality, false);
                base.transform.FindChild("Info/Name").GetComponent<UILabel>().text = _config.name;
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(_config.param_0);
                if (_config2 != null)
                {
                    base.transform.FindChild("TargetItem/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                    CommonFunc.SetEquipQualityBorder(base.transform.FindChild("TargetItem/QualityBorder").GetComponent<UISprite>(), _config2.quality, false);
                    base.transform.FindChild("TargetItem/Label").GetComponent<UILabel>().text = _config2.name;
                    UILabel component = base.transform.FindChild("CostGold/Label").GetComponent<UILabel>();
                    UILabel label5 = base.transform.FindChild("PartCount").GetComponent<UILabel>();
                    UILabel label6 = base.transform.FindChild("Item/Count").GetComponent<UILabel>();
                    if (_config2.type == 10)
                    {
                        this.mTargetEntry = _config.entry;
                        this.mTargeType = 2;
                        component.text = _config.combine_price.ToString();
                        base.transform.FindChild("sprite").gameObject.SetActive(false);
                        this.mNeedCount = _config.param_1;
                        string str = (_item.num < this.mNeedCount) ? string.Empty : "[015b17]";
                        object[] objArray1 = new object[] { GameConstant.DefaultTextColor, "合成需要", _config.name, ": ", str, _item.num, "/", this.mNeedCount };
                        label5.text = string.Concat(objArray1);
                        label6.gameObject.SetActive(false);
                    }
                    else
                    {
                        this.mTargetEntry = _config.param_0;
                        this.mTargeType = (_config2.type != 3) ? 1 : 0;
                        component.text = _config2.combine_price.ToString();
                        base.transform.FindChild("sprite").gameObject.SetActive(true);
                        this.mNeedCount = _config2.param_1;
                        string str2 = (_item.num < this.mNeedCount) ? string.Empty : "[015b17]";
                        object[] objArray2 = new object[] { GameConstant.DefaultTextColor, ConfigMgr.getInstance().GetWord(0x4ee), ": ", str2, _item.num, "/", this.mNeedCount };
                        label5.text = string.Concat(objArray2);
                        label6.gameObject.SetActive(true);
                    }
                    label6.text = this.mNeedCount.ToString();
                    UIEventListener.Get(base.transform.FindChild("CombineBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCombineBtn);
                }
            }
        }
    }
}

