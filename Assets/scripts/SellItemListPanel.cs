using FastBuf;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SellItemListPanel : GUIEntity
{
    public GameObject _SingleSellItem;
    public System.Action CallBackOk;
    private int sellSumCost;

    public override void OnDeSerialization(GUIPersistence pers)
    {
    }

    public override void OnInitialize()
    {
    }

    public override void OnSerialization(GUIPersistence pers)
    {
    }

    public void SellCallBackPanel()
    {
        if (this.CallBackOk != null)
        {
            this.CallBackOk();
        }
    }

    private void SetItemInfo(Transform obj, Item info)
    {
        if (info != null)
        {
            UITexture component = obj.FindChild("Icon").GetComponent<UITexture>();
            UISprite sprite = obj.FindChild("QualityBorder").GetComponent<UISprite>();
            UISprite sprite2 = obj.FindChild("Border").GetComponent<UISprite>();
            UILabel label = obj.FindChild("Name").GetComponent<UILabel>();
            UILabel label2 = obj.FindChild("Count").GetComponent<UILabel>();
            GameObject gameObject = obj.FindChild("sprite").gameObject;
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                this.sellSumCost += _config.sell_price * info.num;
                component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                CommonFunc.SetEquipQualityBorder(sprite, _config.quality, false);
                label.text = _config.name;
                if (info.num > 1)
                {
                    label2.text = info.num.ToString();
                }
                else
                {
                    label2.text = string.Empty;
                }
                if ((_config.type == 3) || (_config.type == 2))
                {
                    gameObject.SetActive(true);
                }
                if (_config.type == 3)
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
        }
    }

    public void ShowItemList(List<Item> itemList)
    {
        if (itemList.Count != 0)
        {
            UIGrid component = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
            CommonFunc.DeleteChildItem(component.transform);
            int num = itemList.Count / 4;
            if ((itemList.Count % 4) != 0)
            {
                num++;
            }
            int count = itemList.Count;
            int num3 = 0;
            float y = 0f;
            this.sellSumCost = 0;
            for (int i = 0; i < num; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleSellItem) as GameObject;
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
            base.transform.FindChild("Gold").GetComponent<UILabel>().text = this.sellSumCost.ToString();
        }
    }
}

