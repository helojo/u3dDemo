using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Act_Pres_Reward : MonoBehaviour
{
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache21;
    private card_config cc;
    public int cd_time;
    public CostType costType = CostType.E_CT_Stone;
    public UILabel describeLabel;
    public int duration;
    public int entry;
    public UniversialRewardDrawFlag flag;
    public UISprite fushi;
    private item_config ic;
    public GameObject itemPre;
    public List<Act_Press_R_Item> items = new List<Act_Press_R_Item>();
    public List<PressItem> itemStr = new List<PressItem>();
    private float lastTime;
    private string leftCntOfDaySpriteName = "Ui_Shop_Icon_jrsq";
    private string leftCntSpriteName = "Ui_Shop_Icon_sq";
    public UILabel nameLabel;
    public GameObject notEnough;
    public int price;
    public UILabel priceLabel;
    public int purchaseCount;
    public int purchaseCountOfDay;
    private C2S_TXBuyStoreItem req = new C2S_TXBuyStoreItem();
    public string reward_describe;
    public string reward_items;
    public string reward_name;
    public int serverPurCnt;
    public int serverPurCntOfDay;
    public int slotId;
    public int start_time;
    public bool subTimeEnable;
    public UIGrid uig;
    public UIScrollView uisv;
    public GameObject unableBox;

    public void AutoSetChildPanelDepth(UIPanel parentPnl, UIScrollView childListPanel)
    {
        UIPanel component = childListPanel.gameObject.GetComponent<UIPanel>();
        if (component != null)
        {
            component.depth = parentPnl.depth + 1;
        }
    }

    private int ChickNum()
    {
        if (this.purchaseCountOfDay != -1)
        {
            return this.purchaseCountOfDay;
        }
        if (this.serverPurCntOfDay != -1)
        {
            return this.serverPurCntOfDay;
        }
        if (this.purchaseCount != -1)
        {
            return this.purchaseCount;
        }
        if (this.serverPurCnt != -1)
        {
            return this.serverPurCnt;
        }
        return -1;
    }

    private void Delete_PresItem_List()
    {
        this.itemStr.Clear();
        for (int i = 0; i < this.items.Count; i++)
        {
            UnityEngine.Object.Destroy(this.items[i].gameObject);
        }
        this.items.Clear();
    }

    private void GetItems()
    {
        this.Delete_PresItem_List();
        if (ActivePanel.inst != null)
        {
            if (!ActivePanel.inst.curInfo.storeList[this.slotId].reward_items.Contains("#") && ((ActivePanel.inst.curInfo.storeList[this.slotId].reward_items == null) || (ActivePanel.inst.curInfo.storeList[this.slotId].reward_items == string.Empty)))
            {
                Debug.Log("检查字符串：----------------------:Error!!!");
                return;
            }
        }
        else
        {
            Debug.Log("检查字符串：----------------------:Error!!!");
            return;
        }
        char[] separator = new char[] { '#' };
        string[] strArray = ActivePanel.inst.curInfo.storeList[this.slotId].reward_items.Split(separator);
        for (int i = 0; i < strArray.Length; i++)
        {
            PressItem item = new PressItem();
            if (!strArray[i].Contains("|") && ((strArray[i] == null) || (strArray[i] == string.Empty)))
            {
                Debug.Log("检查字符串：----------------------:Error!!!");
                return;
            }
            char[] chArray2 = new char[] { '|' };
            string[] strArray2 = strArray[i].Split(chArray2);
            item.itemType = int.Parse(strArray2[0]);
            item.itemId = strArray2[1];
            item.itemNum = int.Parse(strArray2[2]);
            if (item.itemType == 0x63)
            {
                item.itemIcon = strArray2[1];
                item.itemName = strArray2[3];
                item.itemQuality = int.Parse(strArray2[4]);
                item.itemDes = strArray2[5];
            }
            else if (item.itemType == 1)
            {
                this.cc = ConfigMgr.getInstance().getByEntry<card_config>(int.Parse(item.itemId));
                if (this.ic != null)
                {
                    item.itemIcon = this.cc.image;
                    item.itemName = this.cc.name;
                    item.itemQuality = this.cc.quality;
                    item.itemDes = this.cc.describe;
                    item.cardStar = this.cc.evolve_lv;
                }
                else
                {
                    Debug.Log("!没有找到对应数据!:  " + item.itemId);
                }
            }
            else
            {
                this.ic = ConfigMgr.getInstance().getByEntry<item_config>(int.Parse(item.itemId));
                if (this.ic != null)
                {
                    item.itemIcon = this.ic.icon;
                    item.itemName = this.ic.name;
                    item.itemQuality = this.ic.quality;
                    item.itemDes = this.ic.describe;
                }
                else
                {
                    Debug.Log("!没有找到对应数据!:  " + item.itemId);
                }
            }
            this.itemStr.Add(item);
        }
    }

    private void OnClick()
    {
        if (this.flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_START)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186b2));
        }
        else if (Time.time >= (this.lastTime + 1f))
        {
            this.req.itemEntryId = this.entry;
            this.req.count = 1;
            this.req.costType = this.costType;
            this.lastTime = Time.time;
            GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
                MessageBox box = (MessageBox) obj;
                string str = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x186b3), GameConstant.DefaultTextRedColor + this.reward_name + GameConstant.DefaultTextColor);
                if (<>f__am$cache21 == null)
                {
                    <>f__am$cache21 = delegate (GameObject box) {
                    };
                }
                box.SetDialog(str, box => SocketMgr.Instance.C2S_TXBUYSTOREITEM(this.req), <>f__am$cache21, false);
            }, null);
        }
    }

    public void ResetData(ActiveShowType clientShowType)
    {
        this.priceLabel.text = string.Empty + this.price;
        this.nameLabel.text = string.Empty + this.reward_name;
        this.SetPresentBuyStateInfo();
        this.SelectFushi();
        this.GetItems();
        if (this.itemStr.Count < 1)
        {
            Debug.Log("ServerConfig DataError ! itemStr's Count < 1");
        }
        else
        {
            for (int i = 0; i < 1; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this.itemPre) as GameObject;
                obj2.transform.parent = this.uig.transform;
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localPosition = new Vector3(this.uig.cellWidth * i, 0f, 0f);
                UIDragScrollView component = obj2.GetComponent<UIDragScrollView>();
                component.scrollView = this.uisv;
                component.enabled = false;
                this.uisv.GetComponent<UIPanel>().depth = 0xac;
                Act_Press_R_Item item = obj2.GetComponent<Act_Press_R_Item>();
                item.itemType = this.itemStr[i].itemType;
                item.itemId = this.itemStr[i].itemId;
                item.IconId = this.itemStr[i].itemIcon;
                item.itemName = this.itemStr[i].itemName;
                item.itemDescribe = this.itemStr[i].itemDes;
                this.SetSellStateInfo(this.serverPurCntOfDay, this.serverPurCnt);
                if ((this.serverPurCntOfDay == 0) || (this.serverPurCnt == 0))
                {
                    if (this.serverPurCntOfDay == 0)
                    {
                        item.curPressentLeftState = Act_Press_R_Item.LeftCntState.LeftNullOfToday;
                    }
                    else if (this.serverPurCnt == 0)
                    {
                        item.curPressentLeftState = Act_Press_R_Item.LeftCntState.LeftNull;
                    }
                }
                else
                {
                    item.curPressentLeftState = Act_Press_R_Item.LeftCntState.Other;
                }
                item.itemNum = this.ChickNum();
                item.quality = this.itemStr[i].itemQuality;
                this.items.Add(item);
                item.ResetData(clientShowType);
            }
            if ((base.transform.parent != null) && (base.transform.parent.parent != null))
            {
                UIPanel parentPnl = base.transform.parent.parent.GetComponent<UIPanel>();
                if (parentPnl != null)
                {
                    this.AutoSetChildPanelDepth(parentPnl, this.uisv);
                }
            }
        }
    }

    private void SelectFushi()
    {
        if (this.costType == CostType.E_CT_Stone)
        {
            this.fushi.spriteName = "Item_Icon_Stone";
        }
        else if (this.costType == CostType.E_CT_Gold)
        {
            this.fushi.spriteName = "Item_Icon_Gold";
        }
    }

    private void SetPresentBuyStateInfo()
    {
        string str = string.Empty;
        string str2 = string.Empty;
        string str3 = string.Empty;
        string str4 = string.Empty;
        string str5 = string.Empty;
        string word = ConfigMgr.getInstance().GetWord(0x4e41);
        string str7 = ConfigMgr.getInstance().GetWord(0x4e42);
        string format = ConfigMgr.getInstance().GetWord(0x4e43);
        string str9 = ConfigMgr.getInstance().GetWord(0x4e44);
        string str10 = ConfigMgr.getInstance().GetWord(0x4e45);
        if (((this.purchaseCountOfDay == -1) && (this.serverPurCntOfDay == -1)) && ((this.purchaseCount == -1) && (this.serverPurCnt == -1)))
        {
            str = str10;
        }
        else
        {
            if (this.serverPurCntOfDay != -1)
            {
                str4 = word + this.serverPurCntOfDay;
            }
            else
            {
                str4 = string.Empty;
            }
            if (this.serverPurCnt != -1)
            {
                str5 = str7 + this.serverPurCnt;
            }
            else
            {
                str5 = string.Empty;
            }
            string str11 = string.Empty;
            if (str4 != string.Empty)
            {
                str11 = str4;
            }
            else
            {
                str11 = str5;
            }
            if (this.purchaseCountOfDay != -1)
            {
                str2 = string.Format(format, this.purchaseCountOfDay);
            }
            else
            {
                str2 = string.Empty;
            }
            if (this.purchaseCount != -1)
            {
                str3 = string.Format(str9, this.purchaseCount);
            }
            else
            {
                str3 = string.Empty;
            }
            string str12 = string.Empty;
            if (str2 != string.Empty)
            {
                str12 = str2;
            }
            else
            {
                str12 = str3;
            }
            if ((str11 != string.Empty) && (str12 != string.Empty))
            {
                str11 = str11 + "\n";
            }
            str = str11 + str12;
        }
        this.describeLabel.text = str;
    }

    private void SetSellStateInfo(int leftCntOfDay, int leftCnt)
    {
        if ((leftCntOfDay == 0) || (leftCnt == 0))
        {
            this.unableBox.SetActive(true);
            this.notEnough.SetActive(true);
            UISprite component = this.notEnough.GetComponent<UISprite>();
            UIWidget widget = this.notEnough.GetComponent<UIWidget>();
            if (leftCntOfDay == 0)
            {
                component.name = this.leftCntOfDaySpriteName;
                widget.MakePixelPerfect();
            }
            else if (leftCnt == 0)
            {
                component.name = this.leftCntSpriteName;
                widget.MakePixelPerfect();
            }
        }
        else
        {
            this.unableBox.SetActive(false);
            this.notEnough.SetActive(false);
        }
    }
}

