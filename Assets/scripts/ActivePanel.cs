using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ActivePanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache18;
    public GameObject activeItem;
    public GameObject activeReward;
    public ActiveInfo curInfo = new ActiveInfo();
    public UILabel dataLabel_day;
    public UILabel dataLabel_month;
    public UILabel dataLabel_year;
    public UILabel fushiLabel;
    public UIGrid grid;
    public GameObject[] infoBox = new GameObject[2];
    public UILabel infoLabel_describe;
    public UILabel infoLabel_recharge;
    public static ActivePanel inst;
    public UITexture[] itemImage = new UITexture[2];
    public List<ActiveItem> items = new List<ActiveItem>();
    public UILabel labelHaveNoActive;
    public int lastSlot;
    public UILabel[] parameter_2;
    public GameObject PriceBox;
    public Act_Press_R_Item[] priceItems = new Act_Press_R_Item[2];
    public ActTypeMgr[] showTypes = new ActTypeMgr[5];
    public ActiveTips TipsDlg;
    public UILabel titleLabel;
    public Transform tr;
    public UIScrollView uisv;

    public void AutoSetDepth(UIPanel parentPnl, UIScrollView childListPanel)
    {
        base.AutoSetChildPanelDepth(parentPnl, childListPanel);
    }

    public void AutoSetDepthError()
    {
        base.AutoSetChildPanelDepth(base.NGUIPanel, this.uisv);
    }

    private void Awake()
    {
        inst = this;
        this.Resetdata();
        this.tr = base.transform;
    }

    private bool BoolTime(ActiveInfo info)
    {
        return this.IsActivityEnable(info.startTime, info.holdOnTime, info.cdTime);
    }

    public void ButtonEvent(ActiveBtnType type)
    {
        ActiveBtnType type2 = type;
        switch (type2)
        {
            case ActiveBtnType.chongzhi:
                Debug.Log("-------------------------");
                if (<>f__am$cache18 == null)
                {
                    <>f__am$cache18 = gui => VipCardPanel panel = (VipCardPanel) gui;
                }
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", <>f__am$cache18);
                break;

            case ActiveBtnType.lingqu:
            {
                C2S_DrawActivityPrize req = new C2S_DrawActivityPrize {
                    subEntry = this.curInfo.rewards_configs[0].entry,
                    activity_entry = this.curInfo.entry
                };
                SocketMgr.Instance.DRAWACTIVITYPRIZE(req);
                break;
            }
            default:
                if (type2 == ActiveBtnType.close)
                {
                    base.gameObject.SetActive(false);
                    GUIMgr.Instance.PopGUIEntity();
                }
                break;
        }
    }

    public void ButtonEvent_CollectExchange(int index)
    {
        C2S_CollectExchange req = new C2S_CollectExchange {
            subEntry = this.curInfo.rewards_configs[index].entry,
            activity_entry = this.curInfo.entry
        };
        SocketMgr.Instance.C2S_CollectExchange(req);
    }

    public void ButtonEvent_LingQu(int index)
    {
        C2S_DrawActivityPrize req = new C2S_DrawActivityPrize {
            subEntry = this.curInfo.rewards_configs[index].entry,
            activity_entry = this.curInfo.entry
        };
        SocketMgr.Instance.DRAWACTIVITYPRIZE(req);
    }

    private void CreateActiveItem(int index)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate(this.activeItem) as GameObject;
        obj2.transform.parent = this.grid.transform;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localPosition = new Vector3(0f, -this.grid.cellHeight * index, 0f);
        ActiveItem component = obj2.GetComponent<ActiveItem>();
        component.curIndex = index;
        component.info = ActiveList.actives[index];
        this.items.Add(component);
    }

    private void CreateActives()
    {
        for (int i = 0; i < ActiveList.actives.Count; i++)
        {
            this.CreateActiveItem(i);
        }
        base.AutoSetChildPanelDepth(base.NGUIPanel, this.uisv);
        this.SetScrollListInitPos(this.uisv);
    }

    public void DeletAllActive()
    {
        for (int i = 0; i < this.items.Count; i++)
        {
            UnityEngine.Object.Destroy(this.items[i].gameObject);
        }
        this.items.Clear();
    }

    private void FirstInitPanelData(bool b_haveActive)
    {
        this.labelHaveNoActive.gameObject.SetActive(b_haveActive);
        this.titleLabel.gameObject.SetActive(!b_haveActive);
        int length = this.showTypes.Length;
        for (int i = 0; i < length; i++)
        {
            this.showTypes[i].gameObject.SetActive(!b_haveActive);
        }
    }

    public void InitActiveBtnInfo()
    {
        this.DeletAllActive();
        this.CreateActives();
        this.ResetItems();
        if (ActiveList.actives.Count > 0)
        {
            this.FirstInitPanelData(false);
            if (ActiveList.actives.Count > this.lastSlot)
            {
                this.ResetInfo(ActiveList.actives[this.lastSlot], true, this.BoolTime(ActiveList.actives[this.lastSlot]));
            }
        }
        else
        {
            this.FirstInitPanelData(true);
        }
        this.SetActiveCompleteIsOk();
        GUIMgr.Instance.FloatTitleBar();
    }

    private bool IsActivityEnable(uint startTime, int duration, int cdTime)
    {
        long num = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
        if (num < startTime)
        {
            return false;
        }
        if (duration == -1)
        {
            return true;
        }
        if (cdTime == -1)
        {
            return (num < (startTime + (duration * 60)));
        }
        long num2 = (num - startTime) % ((long) ((duration + cdTime) * 60));
        return (num2 < (duration * 60));
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        if (this.TipsDlg != null)
        {
            this.TipsDlg.gameObject.SetActive(false);
        }
        this.InitActiveBtnInfo();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public void ResetBuyStoreState(int commodityId, bool canbuy)
    {
        ActiveInfo info = new ActiveInfo();
        for (int i = 0; i < ActiveList.actives.Count; i++)
        {
            if (ActiveList.actives[i].activity_type == ActivityType.e_tencent_activity_shop_package)
            {
                info = ActiveList.actives[i];
                for (int j = 0; j < ActiveList.actives[i].storeList.Count; j++)
                {
                    if (commodityId == ActiveList.actives[i].storeList[j].commodityId)
                    {
                        ActiveList.actives[i].storeList[j].canBuy = canbuy;
                    }
                }
            }
        }
        if ((info == this.curInfo) && (info != null))
        {
            this.ResetInfo(info, false, true);
        }
    }

    public void ResetBuyStoreState2(TencentStoreCommodity _commodityInfo)
    {
        int num = 0;
        ActiveInfo info = new ActiveInfo();
        for (int i = 0; i < ActiveList.actives.Count; i++)
        {
            if (ActiveList.actives[i].activity_type == ActivityType.e_tencent_activity_shop_package)
            {
                for (int j = 0; j < ActiveList.actives[i].storeList.Count; j++)
                {
                    if (_commodityInfo.commodityId == ActiveList.actives[i].storeList[j].commodityId)
                    {
                        ActiveList.actives[i].storeList[j].canBuy = _commodityInfo.canBuy;
                        ActiveList.actives[i].storeList[j].purchaseCountOfDay = _commodityInfo.purchaseCountOfDay;
                        ActiveList.actives[i].storeList[j].purchaseCount = _commodityInfo.purchaseCount;
                        ActiveList.actives[i].storeList[j].serverPurCntOfDay = _commodityInfo.serverPurCntOfDay;
                        ActiveList.actives[i].storeList[j].serverPurCnt = _commodityInfo.serverPurCnt;
                        num = j;
                    }
                }
                if (this.lastSlot == i)
                {
                    info = ActiveList.actives[i];
                }
            }
        }
        if ((info == this.curInfo) && (info != null))
        {
            this.ResetInfo(info, false, true);
        }
    }

    private void Resetdata()
    {
        this.dataLabel_year.text = string.Empty + TimeMgr.Instance.ServerDateTime.Year;
        this.dataLabel_month.text = string.Empty + TimeMgr.Instance.ServerDateTime.Month;
        this.dataLabel_day.text = string.Empty + TimeMgr.Instance.ServerDateTime.Day;
    }

    public void ResetInfo(ActiveInfo info, bool bResetPos = true, bool canBuy = true)
    {
        this.SetCurActiveCompleteIsOk(info);
        SocketMgr.Instance.CheckActiveState(ActiveList.actives);
        if (info != null)
        {
            this.curInfo = info;
            if (this.curInfo.rewards_configs.Count > 0)
            {
            }
            this.SelectShow(info.showType);
            if (this.showTypes.Length > info.showType)
            {
                this.showTypes[(int) info.showType].ResetInfo(info, bResetPos, canBuy);
                this.Resetdata();
                this.ResetItem(info);
            }
        }
    }

    private void ResetItem(ActiveInfo info)
    {
        for (int i = 0; i < this.items.Count; i++)
        {
            if (this.items[i].info == info)
            {
                this.items[i].SelectButton(false);
            }
            else
            {
                this.items[i].SelectButton(true);
            }
        }
    }

    private void ResetItems()
    {
        for (int i = 0; i < this.items.Count; i++)
        {
            this.items[i].ResetCardInfo();
        }
    }

    public void ResetRewardState(TencentStoreCommodity _commodityInfo)
    {
    }

    public void ResetState(ActivityType activity_type, int actEntry, int rewardsEntry, UniversialRewardDrawFlag flag)
    {
        ActiveInfo info = new ActiveInfo();
        for (int i = 0; i < ActiveList.actives.Count; i++)
        {
            int num2;
            int num3;
            int num4;
            if (actEntry != ActiveList.actives[i].entry)
            {
                continue;
            }
            info = ActiveList.actives[i];
            ActivityType type = activity_type;
            switch (type)
            {
                case ActivityType.e_tencent_activity_login:
                    num2 = 0;
                    goto Label_00B1;

                case ActivityType.e_tencent_activity_charge:
                    goto Label_00D1;

                default:
                    switch (type)
                    {
                        case ActivityType.e_tencent_activity_consume:
                            goto Label_00D1;

                        case ActivityType.e_tencent_activity_first_charge:
                        {
                            ActiveList.actives[i].rewards_configs[0].flag = flag;
                            continue;
                        }
                        case ActivityType.e_tencent_activity_notice:
                        {
                            continue;
                        }
                        case ActivityType.e_tencent_activity_collect_exchange:
                            num4 = 0;
                            goto Label_01AA;

                        default:
                        {
                            continue;
                        }
                    }
                    break;
            }
        Label_006F:
            if (rewardsEntry == ActiveList.actives[i].rewards_configs[num2].entry)
            {
                ActiveList.actives[i].rewards_configs[num2].flag = flag;
            }
            num2++;
        Label_00B1:
            if (num2 < ActiveList.actives[i].rewards_configs.Count)
            {
                goto Label_006F;
            }
            continue;
        Label_00D1:
            num3 = 0;
            while (num3 < ActiveList.actives[i].rewards_configs.Count)
            {
                if (rewardsEntry == ActiveList.actives[i].rewards_configs[num3].entry)
                {
                    ActiveList.actives[i].rewards_configs[num3].flag = flag;
                }
                num3++;
            }
            continue;
        Label_0164:
            if (rewardsEntry == ActiveList.actives[i].rewards_configs[num4].entry)
            {
                ActiveList.actives[i].rewards_configs[num4].flag = flag;
            }
            num4++;
        Label_01AA:
            if (num4 < ActiveList.actives[i].rewards_configs.Count)
            {
                goto Label_0164;
            }
        }
        if ((actEntry == this.curInfo.entry) && (info != null))
        {
            this.ResetInfo(info, false, true);
        }
    }

    private void SelectShow(ActiveShowType showType)
    {
        for (int i = 0; i < this.showTypes.Length; i++)
        {
            if (i == showType)
            {
                this.showTypes[i].gameObject.SetActive(true);
            }
            else
            {
                this.showTypes[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetActiveCompleteIsOk()
    {
        int count = ActiveList.actives.Count;
        for (int i = 0; i < count; i++)
        {
            ActiveInfo info = ActiveList.actives[i];
            int num3 = info.rewards_configs.Count;
            bool flag = false;
            for (int j = 0; j < num3; j++)
            {
                if (info.activity_type == ActivityType.e_tencent_activity_collect_exchange)
                {
                    if (ActorData.getInstance().GetNeedPressItemIsOkOrNot(info))
                    {
                        flag = true;
                        break;
                    }
                    flag = false;
                }
                else
                {
                    if (info.rewards_configs[j].flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW)
                    {
                        flag = true;
                        break;
                    }
                    flag = false;
                }
            }
            if (((this.items != null) && (this.items.Count > i)) && ((this.items[i] != null) && (this.items[i].spriteComplete.gameObject != null)))
            {
                this.items[i].spriteComplete.gameObject.SetActive(flag);
            }
        }
    }

    public void SetCurActiveCompleteIsOk(ActiveInfo activeInfo)
    {
        int count = ActiveList.actives.Count;
        for (int i = 0; i < count; i++)
        {
            if (ActiveList.actives[i].entry != activeInfo.entry)
            {
                continue;
            }
            int num3 = ActiveList.actives[i].rewards_configs.Count;
            bool flag = false;
            for (int j = 0; j < num3; j++)
            {
                if (ActiveList.actives[i].rewards_configs[j].flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW)
                {
                    flag = true;
                    break;
                }
                if (ActiveList.actives[i].rewards_configs[j].flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_FINISH)
                {
                    if (activeInfo.activity_type == ActivityType.e_tencent_activity_collect_exchange)
                    {
                        if (ActorData.getInstance().GetNeedPressItemIsOkOrNot(activeInfo))
                        {
                            flag = true;
                            break;
                        }
                        flag = false;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            if (((this.items != null) && (this.items.Count > i)) && ((this.items[i] != null) && (this.items[i].spriteComplete.gameObject != null)))
            {
                this.items[i].spriteComplete.gameObject.SetActive(flag);
            }
        }
    }

    public void SetScrollListInitPos(UIScrollView scrList)
    {
        if (scrList != null)
        {
            UIPanel component = scrList.gameObject.GetComponent<UIPanel>();
            if (component != null)
            {
                component.clipOffset = new Vector2(component.clipOffset.x, 0f);
                component.gameObject.transform.localPosition = new Vector3(component.gameObject.transform.localPosition.x, 0f, scrList.transform.localPosition.z);
                Debug.Log("ActivePanel's activeItem init, set the initData for show right!");
            }
            SpringPanel panel2 = scrList.GetComponent<SpringPanel>();
            if (panel2 != null)
            {
                panel2.enabled = true;
                panel2.target = new Vector3(panel2.target.x, 0f, panel2.target.z);
                panel2.enabled = false;
            }
        }
    }
}

