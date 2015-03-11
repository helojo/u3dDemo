using FastBuf;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Act_Rech_Reward : MonoBehaviour
{
    public BoxCollider boxColliderBtn;
    private card_config cc;
    public int cd_time;
    public int duration;
    public int entry;
    public UniversialRewardDrawFlag flag;
    private item_config ic;
    public GameObject itemPre;
    public List<Act_Press_R_Item> items = new List<Act_Press_R_Item>();
    public List<PressItem> itemStr = new List<PressItem>();
    private float lastTime;
    public ActiveBtn3 lingqu;
    public UILabel nameLabel;
    public string reward_items;
    public string reward_name;
    public int slotId;
    public GameObject spriteLeft;
    public GameObject spriteRight;
    public int start_time;
    public UIPanel statePanel;
    public UISprite stateSprite;
    private string stateSprite_CanGet = "Ui_Shop_Icon_djlq";
    private string stateSprite_CantGet = "Ui_gonghui_Btn_close";
    private string stateSprite_HadGot = "Ui_Shop_Icon_ylw";
    private string stateSprite_HadGot_Today = "Ui_Shop_Icon_jrylw";
    public bool subTimeEnable;
    public UIGrid uig;
    public UIScrollView uisv;

    public void AutoSetChildPanelDepth(UIPanel parentPnl, UIScrollView childListPanel)
    {
        UIPanel component = childListPanel.gameObject.GetComponent<UIPanel>();
        if (component != null)
        {
            component.depth = parentPnl.depth + 1;
        }
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
            if (!ActivePanel.inst.curInfo.rewards_configs[this.slotId].reward_items.Contains("#") && ((ActivePanel.inst.curInfo.rewards_configs[this.slotId].reward_items == null) || (ActivePanel.inst.curInfo.rewards_configs[this.slotId].reward_items == string.Empty)))
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
        string[] strArray = ActivePanel.inst.curInfo.rewards_configs[this.slotId].reward_items.Split(separator);
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
                if (this.cc != null)
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
        if ((((this.flag != UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_START) && (this.flag != UniversialRewardDrawFlag.E_UNIREWARD_FLAG_HAVE_DRAW)) && ((this.flag != UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_FINISH) && (this.flag != UniversialRewardDrawFlag.E_UNIREWARD_FLAG_TODAY_HAVE_DRAW))) && (Time.time >= (this.lastTime + 1f)))
        {
            ActivePanel.inst.ButtonEvent_LingQu(this.slotId);
            this.lastTime = Time.time;
        }
    }

    public void ResetData(ActiveShowType clientShowType)
    {
        this.nameLabel.text = string.Empty + this.reward_name;
        this.GetItems();
        bool flag = false;
        if (this.itemStr.Count > 4)
        {
            flag = true;
        }
        if (flag)
        {
            this.spriteLeft.SetActive(true);
            this.spriteRight.SetActive(true);
        }
        else
        {
            this.spriteLeft.SetActive(false);
            this.spriteRight.SetActive(false);
        }
        for (int i = 0; i < this.itemStr.Count; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this.itemPre) as GameObject;
            obj2.transform.parent = this.uig.transform;
            obj2.transform.localScale = Vector3.one;
            obj2.transform.localPosition = new Vector3(this.uig.cellWidth * i, 0f, 0f);
            UIDragScrollView component = obj2.GetComponent<UIDragScrollView>();
            Debug.Log("uisv2:_____" + component);
            component.scrollView = this.uisv;
            component.enabled = flag;
            Debug.Log("uisv2.enabled:_____" + component.enabled);
            this.uisv.GetComponent<UIPanel>().depth = 0xac;
            Act_Press_R_Item item = obj2.GetComponent<Act_Press_R_Item>();
            item.itemType = this.itemStr[i].itemType;
            item.itemId = this.itemStr[i].itemId;
            item.IconId = this.itemStr[i].itemIcon;
            item.itemName = this.itemStr[i].itemName;
            item.itemDescribe = this.itemStr[i].itemDes;
            item.itemNum = this.itemStr[i].itemNum;
            item.quality = this.itemStr[i].itemQuality;
            item.cardStar = this.itemStr[i].cardStar;
            this.items.Add(item);
            item.ResetData(clientShowType);
        }
        this.lingqu.flag = this.flag;
        this.SetRewardGetState(this.flag);
        if ((base.transform.parent != null) && (base.transform.parent.parent != null))
        {
            UIPanel parentPnl = base.transform.parent.parent.GetComponent<UIPanel>();
            if (parentPnl != null)
            {
                this.AutoSetChildPanelDepth(parentPnl, this.uisv);
            }
        }
        if ((ActivePanel.inst != null) && (ActivePanel.inst.uisv.gameObject.GetComponent<UIPanel>() != null))
        {
            this.uisv.gameObject.GetComponent<UIPanel>().depth = ActivePanel.inst.showTypes[1].ObjUisv.gameObject.GetComponent<UIPanel>().depth + 1;
            this.statePanel.depth = this.uisv.gameObject.GetComponent<UIPanel>().depth + 1;
        }
    }

    private void SetChildrenColliderState(bool state)
    {
        IEnumerator<Transform> enumerator = this.uig.GetChildList().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = enumerator.Current;
                current.GetComponent<BoxCollider>().enabled = state;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    private void SetGetRewardBoxColliderState(bool state)
    {
        this.boxColliderBtn.gameObject.SetActive(false);
        this.SetChildrenColliderState(!state);
    }

    private void SetRewardGetState(UniversialRewardDrawFlag flag)
    {
        Debug.Log("flag:___" + flag);
        switch (flag)
        {
            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_START:
                this.stateSprite.spriteName = string.Empty;
                this.stateSprite.gameObject.SetActive(false);
                this.SetGetRewardBoxColliderState(false);
                break;

            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_FINISH:
                this.stateSprite.spriteName = string.Empty;
                this.stateSprite.gameObject.SetActive(false);
                this.SetGetRewardBoxColliderState(false);
                break;

            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW:
                this.stateSprite.spriteName = this.stateSprite_CanGet;
                this.stateSprite.gameObject.SetActive(true);
                this.stateSprite.GetComponent<UISprite>().MakePixelPerfect();
                this.SetGetRewardBoxColliderState(true);
                Debug.Log("E_UNIREWARD_FLAG_CAN_DRAW!!!!!");
                break;

            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_HAVE_DRAW:
                this.stateSprite.spriteName = this.stateSprite_HadGot;
                this.stateSprite.gameObject.SetActive(true);
                this.stateSprite.GetComponent<UISprite>().MakePixelPerfect();
                this.SetGetRewardBoxColliderState(false);
                break;

            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_TODAY_HAVE_DRAW:
                this.stateSprite.spriteName = this.stateSprite_HadGot_Today;
                this.stateSprite.gameObject.SetActive(true);
                this.stateSprite.GetComponent<UISprite>().MakePixelPerfect();
                this.SetGetRewardBoxColliderState(false);
                break;

            default:
                this.stateSprite.spriteName = string.Empty;
                this.stateSprite.gameObject.SetActive(false);
                this.SetGetRewardBoxColliderState(false);
                break;
        }
    }

    private void Start()
    {
    }
}

