using FastBuf;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class DupLevInfoPanel : GUIEntity
{
    private UIEventListener.VoidDelegate _OkCallBack;
    [CompilerGenerated]
    private static Action<GameObject> <>f__am$cache18;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache19;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1A;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache1B;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache1C;
    private List<GameObject> CreateDropBuffer = new List<GameObject>();
    private List<GameObject> CreateMonsterBuffer = new List<GameObject>();
    private DuplicateType CurDupType;
    private int Difficult;
    private int DungeonsCount;
    public GameObject DupDropObj;
    public GameObject HeroIconObj;
    private bool isSmashing;
    private int m_nUnderTownCostPhysical;
    private dungeons_activity_config mDungeonsActivityCfg;
    private int mGrade;
    public bool mIsDupEnter;
    public GameObject MosterTipsDlg;
    private int mOutlandCostPhysical;
    private int mOutlandEntry = -1;
    private int mOutlandFloorCost;
    private int mOutlandType = -1;
    public bool OpenTypeIsPush;
    private int RushCount = 10;
    private bool ShowBtn;
    public GameObject TipsDlg;
    private trench_elite_config TrenchEliteCfg;
    private int TrenchEntry;

    private void BuyTimes()
    {
        <BuyTimes>c__AnonStorey1BD storeybd = new <BuyTimes>c__AnonStorey1BD {
            <>f__this = this
        };
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config == null)
        {
            TipsDiag.SetText("vip_config Error");
        }
        else
        {
            storeybd.data = this.GetTrenchData();
            if (storeybd.data == null)
            {
                TipsDiag.SetText("数据异常，请退出副本重试");
            }
            else
            {
                if (storeybd.data.buy_times < 0)
                {
                    storeybd.data.buy_times = 0;
                }
                storeybd.BuyCfg = ConfigMgr.getInstance().getByEntry<buy_cost_config>(storeybd.data.buy_times);
                if (storeybd.BuyCfg == null)
                {
                    TipsDiag.SetText("buy_cost_config Error");
                }
                else if (_config.elite_buy_times <= 0)
                {
                    TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), CommonFunc.GetVipMinLevelEliteBuyTimes()));
                }
                else if (storeybd.data.buy_times >= _config.elite_buy_times)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x286d));
                }
                else if (ActorData.getInstance().Stone < storeybd.BuyCfg.buy_elite_cost_stone)
                {
                    <BuyTimes>c__AnonStorey1BC storeybc = new <BuyTimes>c__AnonStorey1BC {
                        title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                    };
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeybc.<>m__295), null);
                }
                else if (GUIMgr.Instance.GetActivityGUIEntity<MessageBox>() == null)
                {
                    storeybd.MAX_count = 3;
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeybd.<>m__296), base.gameObject);
                }
            }
        }
    }

    private bool CanAtk(int Count)
    {
        if (this.CurDupType == DuplicateType.DupType_Elite)
        {
            if (!PhyIsEnough(ConfigMgr.getInstance().getByEntry<trench_elite_config>(ActorData.getInstance().CurTrenchEntry).cost_phy_force * Count))
            {
                return false;
            }
            if (this.RushCount <= 0)
            {
                int vipMinLevelEliteBuyTimes = CommonFunc.GetVipMinLevelEliteBuyTimes();
                Debug.Log("DDD:" + ActorData.getInstance().UserInfo.vip_level.level);
                if (ActorData.getInstance().UserInfo.vip_level.level < vipMinLevelEliteBuyTimes)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa037c5));
                }
                else
                {
                    this.BuyTimes();
                }
                return false;
            }
        }
        else if ((this.CurDupType == DuplicateType.DupType_Normal) && !PhyIsEnough(ConfigMgr.getInstance().getByEntry<trench_normal_config>(ActorData.getInstance().CurTrenchEntry).cost_phy_force * Count))
        {
            return false;
        }
        return true;
    }

    private void ClearDropBuffer()
    {
        foreach (GameObject obj2 in this.CreateDropBuffer)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        this.CreateDropBuffer.Clear();
    }

    private void ClearMonsterBuffer()
    {
        if (<>f__am$cache18 == null)
        {
            <>f__am$cache18 = obj => UnityEngine.Object.Destroy(obj);
        }
        this.CreateMonsterBuffer.ForEach(<>f__am$cache18);
        this.CreateMonsterBuffer.Clear();
    }

    private void DungeonsOutput(dungeons_activity_config _cfg, int _Difficult)
    {
        if (_Difficult == 0)
        {
            List<int> configEntry = CommonFunc.GetConfigEntry(_cfg.easy_dorp_desc_type);
            List<int> entryList = CommonFunc.GetConfigEntry(_cfg.easy_dorp_desc_entry);
            this.UdpateDropItemData(configEntry, entryList);
        }
        else if (_Difficult == 1)
        {
            List<int> typeList = CommonFunc.GetConfigEntry(_cfg.normal_dorp_desc_type);
            List<int> list4 = CommonFunc.GetConfigEntry(_cfg.normal_dorp_desc_entry);
            this.UdpateDropItemData(typeList, list4);
        }
        else if (_Difficult == 2)
        {
            List<int> list5 = CommonFunc.GetConfigEntry(_cfg.hard_dorp_desc_type);
            List<int> list6 = CommonFunc.GetConfigEntry(_cfg.hard_dorp_desc_entry);
            this.UdpateDropItemData(list5, list6);
        }
    }

    private void DupOutput(object _data)
    {
        switch (this.CurDupType)
        {
            case DuplicateType.DupType_Normal:
            {
                trench_normal_config _config = (trench_normal_config) _data;
                List<int> configEntry = CommonFunc.GetConfigEntry(_config.dorp_desc_type);
                List<int> entryList = CommonFunc.GetConfigEntry(_config.dorp_desc_entry);
                this.UdpateDropItemData(configEntry, entryList);
                break;
            }
            case DuplicateType.DupType_Elite:
            {
                trench_elite_config _config2 = (trench_elite_config) _data;
                List<int> typeList = CommonFunc.GetConfigEntry(_config2.dorp_desc_type);
                List<int> list4 = CommonFunc.GetConfigEntry(_config2.dorp_desc_entry);
                this.UdpateDropItemData(typeList, list4);
                break;
            }
        }
    }

    private void FloatTitleBar()
    {
        GUIMgr.Instance.FloatTitleBar();
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.transform.FindChild("TopRight").gameObject.SetActive(false);
            activityGUIEntity.transform.FindChild("TopLeft").gameObject.SetActive(true);
        }
    }

    private DupCommonData GetDupData()
    {
        return new DupCommonData { dupEntry = ActorData.getInstance().CurDupEntry, dupType = ActorData.getInstance().CurDupType, trenchEntry = ActorData.getInstance().CurTrenchEntry };
    }

    private int GetTimes(int _entry)
    {
        if (_entry == 0)
        {
            return ActorData.getInstance().UserInfo.dungeons_1_times;
        }
        if (_entry == 1)
        {
            return ActorData.getInstance().UserInfo.dungeons_2_times;
        }
        return 0;
    }

    private FastBuf.TrenchData GetTrenchData()
    {
        List<FastBuf.TrenchData> list = new List<FastBuf.TrenchData>();
        ActorData.getInstance().TrenchEliteDataDict.TryGetValue(ActorData.getInstance().CurDupEntry, out list);
        if (list != null)
        {
            foreach (FastBuf.TrenchData data in list)
            {
                if (data.entry == ActorData.getInstance().CurTrenchEntry)
                {
                    return data;
                }
            }
        }
        return null;
    }

    private void HideGrade()
    {
        for (int i = 0; i <= 3; i++)
        {
            if (i != 0)
            {
                if (i > 3)
                {
                    return;
                }
                string name = "Info/Star/" + i;
                base.gameObject.transform.FindChild(name).gameObject.SetActive(false);
            }
        }
    }

    private void HideSomeUI()
    {
        CommonFunc.ShowFuncList(false);
        CommonFunc.ShowTitlebar(false);
    }

    private void OnClickRushOutland(GameObject go)
    {
        if (ActorData.getInstance().Stone < this.mOutlandFloorCost)
        {
            <OnClickRushOutland>c__AnonStorey1BB storeybb = new <OnClickRushOutland>c__AnonStorey1BB {
                title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
            };
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeybb.<>m__292), null);
        }
        else
        {
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                SocketMgr.Instance.RequestOutlandSweepReq((int) obj2, false);
            }
        }
    }

    private void OnClickSerialSmash()
    {
        if ((this.CurDupType != DuplicateType.DupType_Outland) && !this.isSmashing)
        {
            int vipLevelByLockFunc = 0;
            if (this.RushCount > 1)
            {
                vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.SMASH_TEN);
            }
            else if (this.RushCount == 1)
            {
                vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.SMASH);
            }
            if (ActorData.getInstance().UserInfo.vip_level.level < vipLevelByLockFunc)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), vipLevelByLockFunc));
            }
            else if (this.CanAtk(this.RushCount))
            {
                if (ActorData.getInstance().GetDupSmashTimes() < this.RushCount)
                {
                    this.OnClickSmashBuy();
                }
                else
                {
                    GUIMgr.Instance.DoModelGUI("RushResultPanel", delegate (GUIEntity obj) {
                        RushResultPanel panel = (RushResultPanel) obj;
                        panel.Depth = 600;
                        this.StartSmash();
                        SocketMgr.Instance.RequestDuplicateSmash(this.GetDupData(), this.RushCount);
                    }, null);
                }
            }
        }
    }

    private void OnClickSmash()
    {
        if ((this.CurDupType != DuplicateType.DupType_Outland) && !this.isSmashing)
        {
            int vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.SMASH);
            if (ActorData.getInstance().UserInfo.vip_level.level < vipLevelByLockFunc)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), vipLevelByLockFunc));
            }
            else if (this.CanAtk(1))
            {
                if (ActorData.getInstance().GetDupSmashTimes() <= 0)
                {
                    TipsDiag.SetText("扫荡次数不足");
                }
                else
                {
                    GUIMgr.Instance.DoModelGUI("RushResultPanel", delegate (GUIEntity obj) {
                        RushResultPanel panel = (RushResultPanel) obj;
                        panel.Depth = 600;
                        this.StartSmash();
                        SocketMgr.Instance.RequestDuplicateSmash(this.GetDupData(), 1);
                    }, null);
                }
            }
        }
    }

    private void OnClickSmashBuy()
    {
        <OnClickSmashBuy>c__AnonStorey1BE storeybe = new <OnClickSmashBuy>c__AnonStorey1BE();
        int vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.BUY_SMASH_COUNT);
        if (ActorData.getInstance().UserInfo.vip_level.level < vipLevelByLockFunc)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), vipLevelByLockFunc));
        }
        else
        {
            storeybe.Count = GameConstValues.BUY_DUP_SMASH_TIMES;
            storeybe.Cost = GameConstValues.BUY_DUP_SMASH_TIMES_COST;
            storeybe.Cost *= 10;
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeybe.<>m__29B), base.gameObject);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.DelayCallBack(0.5f, () => this.ShowRushBtn(this.ShowBtn));
        base.OnDeSerialization(pers);
        this.FloatTitleBar();
    }

    public override void OnInitialize()
    {
    }

    private void OnPressItem(GameObject obj, bool _press)
    {
        item_config _config = (item_config) GUIDataHolder.getData(obj);
        if (_press)
        {
            this.TipsDlg.transform.parent = obj.transform.parent;
            this.TipsDlg.transform.localPosition = Vector3.zero;
            this.TipsDlg.transform.localScale = Vector3.one;
            this.TipsDlg.transform.FindChild("Name").GetComponent<UILabel>().text = CommonFunc.GetItemNameByQuality(_config.quality) + _config.name;
            UILabel component = this.TipsDlg.transform.FindChild("Desc").GetComponent<UILabel>();
            component.text = _config.describe;
            UILabel label2 = this.TipsDlg.transform.FindChild("Count").GetComponent<UILabel>();
            Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_config.entry);
            label2.text = ((itemByEntry != null) ? itemByEntry.num : 0).ToString();
            UISprite sprite = this.TipsDlg.transform.FindChild("Border").GetComponent<UISprite>();
            sprite.height = component.height + 90;
            Transform transform = this.TipsDlg.transform.FindChild("Group");
            transform.transform.localPosition = new Vector3(0f, (float) (0x67 - (component.height - 30)), 0f);
            List<Card> cardListByLackOneItemByEquip = XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(_config.entry, 1);
            for (int i = 0; i < 4; i++)
            {
                Transform transform2 = transform.FindChild((i + 1).ToString());
                if (i < cardListByLackOneItemByEquip.Count)
                {
                    UITexture texture = transform2.FindChild("Icon").GetComponent<UITexture>();
                    UISprite sprite2 = transform2.FindChild("frame").GetComponent<UISprite>();
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) cardListByLackOneItemByEquip[i].cardInfo.entry);
                    if (_config2 != null)
                    {
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                        CommonFunc.SetQualityBorder(sprite2, cardListByLackOneItemByEquip[i].cardInfo.quality);
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
            if (cardListByLackOneItemByEquip.Count > 0)
            {
                sprite.height += 0x3a;
            }
            transform.gameObject.SetActive(cardListByLackOneItemByEquip.Count > 0);
            this.TipsDlg.SetActive(true);
        }
        else
        {
            this.TipsDlg.SetActive(false);
        }
    }

    private void OnPressMonsterItem(GameObject go, bool isPress)
    {
        if (isPress)
        {
            this.MosterTipsDlg.transform.parent = go.transform.parent;
            this.MosterTipsDlg.transform.localPosition = Vector3.zero;
            this.MosterTipsDlg.transform.localScale = Vector3.one;
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                monster_config _config = (monster_config) obj2;
                if (_config != null)
                {
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(_config.card_entry);
                    if (_config2 != null)
                    {
                        UISprite component = this.MosterTipsDlg.transform.FindChild("Group/IconHead/QualityBorder").GetComponent<UISprite>();
                        UITexture texture = this.MosterTipsDlg.transform.FindChild("Group/IconHead/Icon").GetComponent<UITexture>();
                        CommonFunc.SetQualityBorder(component, _config.quality);
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                        this.MosterTipsDlg.transform.FindChild("Group/Name").GetComponent<UILabel>().text = _config2.name;
                        this.MosterTipsDlg.transform.FindChild("Group/Level").GetComponent<UILabel>().text = "LV." + _config.level;
                        this.MosterTipsDlg.transform.FindChild("Group/Boss").gameObject.SetActive(_config.type > 1);
                        UILabel label3 = this.MosterTipsDlg.transform.FindChild("Group/Describe/Desc").GetComponent<UILabel>();
                        label3.text = _config.description;
                        this.MosterTipsDlg.transform.FindChild("Group/Border").GetComponent<UISprite>().height = 0x73 + label3.height;
                        this.MosterTipsDlg.SetActive(true);
                    }
                }
            }
        }
        else
        {
            this.MosterTipsDlg.SetActive(false);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        this.ShowRushBtn(false);
        base.OnSerialization(pers);
    }

    [DebuggerHidden]
    private IEnumerator OnStartSmash()
    {
        return new <OnStartSmash>c__Iterator6E { <>f__this = this };
    }

    public static bool PhyIsEnough(int Cost)
    {
        if (Cost <= ActorData.getInstance().PhyForce)
        {
            return true;
        }
        if ((ActorData.getInstance().UserInfo.phyforce_buy_times < ActorData.getInstance().UserInfo.max_phy_buy_times) || (ActorData.getInstance().UserInfo.max_phy_buy_times == -1))
        {
            if (<>f__am$cache19 == null)
            {
                <>f__am$cache19 = delegate (GUIEntity obj) {
                    MessageBox box = (MessageBox) obj;
                    if (<>f__am$cache1B == null)
                    {
                        <>f__am$cache1B = box => SocketMgr.Instance.RequestRuneStonePurchase(RunestonePurchaseType.E_RPT_PhyForce, 0);
                    }
                    if (<>f__am$cache1C == null)
                    {
                        <>f__am$cache1C = delegate (GameObject go) {
                        };
                    }
                    box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x39a), ActorData.getInstance().UserInfo.buy_phy_cost, 120), <>f__am$cache1B, <>f__am$cache1C, false);
                };
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache19, null);
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9ba3cf));
        }
        return false;
    }

    public void ResetPanel()
    {
        ActorData.getInstance().mCurrDupReturnPrePara = null;
        if (this.mIsDupEnter)
        {
            TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowTitleBar(false);
            }
        }
        else
        {
            TitleBar bar2 = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
            if (bar2 != null)
            {
                bar2.transform.FindChild("TopRight").gameObject.SetActive(true);
            }
        }
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
        if (this._OkCallBack != null)
        {
            this._OkCallBack(base.gameObject);
        }
        GUIMgr.Instance.PopGUIEntity();
    }

    public void SetCloseCallBack(UIEventListener.VoidDelegate _CallBack)
    {
        this._OkCallBack = _CallBack;
    }

    private void ShowBuyBtn(bool _show)
    {
        base.gameObject.transform.FindChild("Info/BuyBtn").gameObject.SetActive(_show);
    }

    private void ShowRushBtn(bool _show)
    {
        base.gameObject.transform.FindChild("Rush/RushTenBtn").gameObject.SetActive(_show);
        base.gameObject.transform.FindChild("Rush/RushBtn").gameObject.SetActive(_show);
        base.gameObject.transform.FindChild("Rush/Icon").gameObject.SetActive(_show);
    }

    private void ShowRushOutlandBtn(bool _show)
    {
        base.gameObject.transform.FindChild("Rush/IconOutland").gameObject.SetActive(_show);
        base.gameObject.transform.FindChild("Rush/OutlandRushBtn").gameObject.SetActive(_show);
    }

    private void StartFight()
    {
        if (this.CurDupType == DuplicateType.DupType_Outland)
        {
            if (this.mOutlandEntry < 0)
            {
                TipsDiag.SetText("外域层级错误!");
            }
            else
            {
                SocketMgr.Instance.RequestOutlandDataEnter(this.mOutlandEntry);
            }
        }
        else if (this.CurDupType == DuplicateType.DupType_Undertown)
        {
            if (this.DungeonsCount <= 0)
            {
                TipsDiag.SetText("挑战次数不足!");
            }
            else if (PhyIsEnough(this.m_nUnderTownCostPhysical))
            {
                GUIMgr.Instance.DoModelGUI("SelectHeroPanel", delegate (GUIEntity Sobj) {
                    SelectHeroPanel panel = Sobj.Achieve<SelectHeroPanel>();
                    panel.Depth = 800;
                    panel.mBattleType = BattleType.Dungeons;
                    panel.SetDungeonsData(this.mDungeonsActivityCfg, this.Difficult);
                    panel.SetZhuZhanStat(false);
                }, null);
            }
        }
        else if (this.CanAtk(1))
        {
            if (<>f__am$cache1A == null)
            {
                <>f__am$cache1A = delegate (GUIEntity Sobj) {
                    SelectHeroPanel panel = Sobj.Achieve<SelectHeroPanel>();
                    panel.Depth = 800;
                    panel.mBattleType = BattleType.Normal;
                    panel.SetDupMapState();
                    SocketMgr.Instance.RequestGetAssistUserList();
                    GuideSystem.FireEvent(GuideEvent.Fomation);
                    GuideSystem.FireEvent(GuideEvent.Start_EliteTrench_Fight);
                };
            }
            GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cache1A, base.gameObject);
        }
    }

    private void StartSmash()
    {
        this.isSmashing = true;
        base.StartCoroutine(this.OnStartSmash());
    }

    private void UdpateDropItemData(List<int> TypeList, List<int> EntryList)
    {
        int num = 0;
        this.ClearDropBuffer();
        foreach (int num2 in EntryList)
        {
            GameObject gameObject = base.gameObject.transform.FindChild("OutPut/" + (num + 1)).gameObject;
            GameObject item = UnityEngine.Object.Instantiate(this.DupDropObj) as GameObject;
            item.transform.parent = gameObject.transform;
            item.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
            item.transform.localPosition = Vector3.zero;
            UITexture component = item.transform.FindChild("Icon").GetComponent<UITexture>();
            UISprite sprite = item.transform.FindChild("frame").GetComponent<UISprite>();
            UILabel label = item.transform.FindChild("Label").GetComponent<UILabel>();
            GameObject obj4 = item.transform.FindChild("Patch").gameObject;
            item.transform.FindChild("Tips").gameObject.gameObject.SetActive(XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(num2, 1).Count > 0);
            this.CreateDropBuffer.Add(item);
            item_config data = ConfigMgr.getInstance().getByEntry<item_config>(EntryList[num]);
            if (data == null)
            {
                Debug.LogWarning("Cfg is Null!");
                break;
            }
            component.mainTexture = BundleMgr.Instance.CreateItemIcon(data.icon);
            CommonFunc.SetEquipQualityBorder(sprite, data.quality, false);
            label.text = string.Empty;
            if (data.type == 3)
            {
                component.width = 90;
                component.height = 90;
            }
            else
            {
                component.width = 0x4a;
                component.height = 0x4a;
            }
            if ((data.type == 3) || (data.type == 2))
            {
                obj4.SetActive(true);
            }
            else
            {
                obj4.SetActive(false);
            }
            GUIDataHolder.setData(item, data);
            UIEventListener listener1 = UIEventListener.Get(item);
            listener1.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener1.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
            num++;
        }
    }

    public void UpdateAckCount()
    {
        if (this.CurDupType == DuplicateType.DupType_Elite)
        {
            List<FastBuf.TrenchData> list = new List<FastBuf.TrenchData>();
            ActorData.getInstance().TrenchEliteDataDict.TryGetValue(ActorData.getInstance().CurDupEntry, out list);
            trench_elite_config trenchEliteCfg = this.TrenchEliteCfg;
            int atkCountFromList = CommonFunc.GetAtkCountFromList(list, trenchEliteCfg.entry);
            base.gameObject.transform.FindChild("Info/Count/Val").GetComponent<UILabel>().text = atkCountFromList.ToString();
            this.UpdateRushBtnLabel(atkCountFromList);
            this.RushCount = atkCountFromList;
            if (this.RushCount <= 0)
            {
                this.ShowBuyBtn(true);
            }
            else
            {
                this.ShowBuyBtn(false);
            }
        }
    }

    public void UpdateBattleComplete()
    {
        if (ActorData.getInstance().CurDupType == DuplicateType.DupType_Normal)
        {
            trench_normal_config _config = ConfigMgr.getInstance().getByEntry<trench_normal_config>(ActorData.getInstance().CurTrenchEntry);
            this.UpdateNormalMapInfo(_config);
        }
        else if (ActorData.getInstance().CurDupType == DuplicateType.DupType_Elite)
        {
            trench_elite_config _config2 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(ActorData.getInstance().CurTrenchEntry);
            this.UpdateEliteMapInfo(_config2);
        }
    }

    public void UpdateCount(int count)
    {
        base.gameObject.transform.FindChild("Info/Count/Val").GetComponent<UILabel>().text = count.ToString();
        this.RushCount = count;
        this.UpdateRushBtnLabel(count);
        this.ShowBuyBtn(false);
    }

    public void UpdateData(int _dupEntry, int _trenchEntry, DuplicateType _type)
    {
        if (_type == DuplicateType.DupType_Normal)
        {
            trench_normal_config _config = ConfigMgr.getInstance().getByEntry<trench_normal_config>(_trenchEntry);
            this.UpdateNormalMapInfo(_config);
        }
        else if (_type == DuplicateType.DupType_Elite)
        {
            trench_elite_config _config2 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(_trenchEntry);
            this.UpdateEliteMapInfo(_config2);
        }
    }

    public void UpdateDungeonsData(dungeons_activity_config _cfg, int _Difficult, int _count)
    {
        this.HideGrade();
        base.gameObject.transform.FindChild("Rush").gameObject.SetActive(false);
        this.CurDupType = DuplicateType.DupType_Undertown;
        ActorData.getInstance().mCurrDupReturnPrePara = null;
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config != null)
        {
            List<int> configEntry = CommonFunc.GetConfigEntry(_cfg.cost_phy_force);
            int num = _config.dungeons_times - this.GetTimes(_cfg.entry);
            if (num < 0)
            {
                Debug.Log("DungeonsCount is " + num);
                num = 0;
            }
            this.DungeonsCount = num;
            base.gameObject.transform.FindChild("TitleVal").GetComponent<UILabel>().text = _cfg.name;
            base.gameObject.transform.FindChild("Info/Label").GetComponent<UILabel>().text = _cfg.description;
            base.gameObject.transform.FindChild("Info/Count/Val").GetComponent<UILabel>().text = num.ToString();
            base.gameObject.transform.FindChild("Info/Physical/Val").GetComponent<UILabel>().text = configEntry[0].ToString();
            ActorData.getInstance().m_nDungeonCostPhysical = configEntry[0];
            this.m_nUnderTownCostPhysical = configEntry[0];
            this.mDungeonsActivityCfg = _cfg;
            this.Difficult = _Difficult;
            if (this.Difficult == 0)
            {
                this.UpdateMonsterInfo(_cfg.easy_team_desc);
            }
            else if (this.Difficult == 1)
            {
                this.UpdateMonsterInfo(_cfg.normal_team_desc);
            }
            else if (this.Difficult == 2)
            {
                this.UpdateMonsterInfo(_cfg.hard_team_desc);
            }
            this.DungeonsOutput(_cfg, _Difficult);
        }
    }

    public void UpdateEliteMapInfo(trench_elite_config _cfg)
    {
        this.HideGrade();
        this.CurDupType = DuplicateType.DupType_Elite;
        trench_elite_config _config = _cfg;
        this.TrenchEliteCfg = _cfg;
        List<FastBuf.TrenchData> list = new List<FastBuf.TrenchData>();
        ActorData.getInstance().TrenchEliteDataDict.TryGetValue(ActorData.getInstance().CurDupEntry, out list);
        int atkCountFromList = CommonFunc.GetAtkCountFromList(list, _cfg.entry);
        base.gameObject.transform.FindChild("TitleVal").GetComponent<UILabel>().text = _config.name;
        base.gameObject.transform.FindChild("Info/Label").GetComponent<UILabel>().text = _config.desc;
        base.gameObject.transform.FindChild("Info/Count/Val").GetComponent<UILabel>().text = atkCountFromList.ToString();
        base.gameObject.transform.FindChild("Info/Physical/Val").GetComponent<UILabel>().text = _config.cost_phy_force.ToString();
        ActorData.getInstance().m_nEliteDupCostPhysical = _config.cost_phy_force;
        this.TrenchEntry = _config.entry;
        this.UpdateGrade();
        this.UpdateMonsterInfo(_cfg.monster_desc);
        this.DupOutput(_config);
        this.UpdateRushBtnLabel(atkCountFromList);
        this.RushCount = atkCountFromList;
        if (this.RushCount <= 0)
        {
            this.ShowBuyBtn(true);
        }
        else
        {
            this.ShowBuyBtn(false);
        }
    }

    public void UpdateGrade()
    {
        List<FastBuf.TrenchData> list = new List<FastBuf.TrenchData>();
        int gradeFromList = 0;
        if (this.CurDupType == DuplicateType.DupType_Normal)
        {
            ActorData.getInstance().TrenchNormalDataDict.TryGetValue(ActorData.getInstance().CurDupEntry, out list);
            gradeFromList = CommonFunc.GetGradeFromList(list, this.TrenchEntry);
        }
        else if (this.CurDupType == DuplicateType.DupType_Elite)
        {
            ActorData.getInstance().TrenchEliteDataDict.TryGetValue(ActorData.getInstance().CurDupEntry, out list);
            gradeFromList = CommonFunc.GetGradeFromList(list, this.TrenchEntry);
        }
        for (int i = 0; i <= gradeFromList; i++)
        {
            if (i != 0)
            {
                if (i > 3)
                {
                    return;
                }
                string name = "Info/Star/" + i;
                base.gameObject.transform.FindChild(name).gameObject.SetActive(true);
            }
        }
        this.mGrade = gradeFromList;
        if (gradeFromList == 3)
        {
            if (ActorData.getInstance().GetDupSmashTimes() > 0)
            {
                this.ShowBtn = true;
                this.ShowRushBtn(true);
                this.ShowRushOutlandBtn(false);
            }
        }
        else
        {
            this.ShowBtn = false;
            this.ShowRushBtn(false);
            this.ShowRushOutlandBtn(false);
        }
        this.UpdateRushRemain();
    }

    private void UpdateMonsterInfo(string _monsterInfo)
    {
        List<int> configEntry = CommonFunc.GetConfigEntry(_monsterInfo);
        this.UpdateMonsterInfoFromEntryList(configEntry);
    }

    private void UpdateMonsterInfoFromEntryList(List<int> EntryList)
    {
        GameObject gameObject = base.gameObject.transform.FindChild("Army").gameObject;
        if (this.CurDupType == DuplicateType.DupType_Outland)
        {
            gameObject.transform.FindChild("Label").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e54);
        }
        int num = 1;
        this.ClearMonsterBuffer();
        foreach (int num2 in EntryList)
        {
            monster_config data = ConfigMgr.getInstance().getByEntry<monster_config>(num2);
            if (data == null)
            {
                Debug.LogWarning("MonsterCfg Is Null! " + num2);
            }
            else
            {
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(data.card_entry);
                if (_config2 == null)
                {
                    Debug.LogWarning("CardCfg Is Null! " + data.card_entry);
                }
                else if (num > 4)
                {
                    Debug.LogWarning("Monster Desc Is Too More");
                }
                else
                {
                    gameObject.transform.FindChild(num + "/Sprite").gameObject.SetActive(true);
                    gameObject.transform.FindChild(num + "/frame").gameObject.SetActive(false);
                    GameObject obj3 = gameObject.transform.FindChild(num.ToString()).gameObject;
                    GameObject item = UnityEngine.Object.Instantiate(this.HeroIconObj) as GameObject;
                    item.transform.parent = obj3.transform;
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one;
                    this.CreateMonsterBuffer.Add(item);
                    if (this.CurDupType == DuplicateType.DupType_Outland)
                    {
                        string[] strArray = new string[] { "Ui_Fuben_Icon_wmboss", "Ui_Fuben_Icon_mmboss", "Ui_Fuben_Icon_boss", "Ui_Fuben_Icon_boss" };
                        UISprite sprite = gameObject.transform.FindChild(num + "/bossMark").GetComponent<UISprite>();
                        sprite.gameObject.SetActive(true);
                        sprite.spriteName = strArray[this.mOutlandType];
                    }
                    else
                    {
                        gameObject.transform.FindChild(num + "/bossMark").gameObject.SetActive(false);
                        item.transform.FindChild("IsBoss").gameObject.SetActive(data.type > 1);
                    }
                    UISprite component = item.transform.FindChild("frame").GetComponent<UISprite>();
                    UITexture texture = item.transform.FindChild("Icon").GetComponent<UITexture>();
                    CommonFunc.SetQualityBorder(component, data.quality);
                    texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                    GUIDataHolder.setData(item, data);
                    UIEventListener.Get(item).onPress = new UIEventListener.BoolDelegate(this.OnPressMonsterItem);
                    num++;
                }
            }
        }
    }

    public void UpdateNormalMapInfo(trench_normal_config _cfg)
    {
        this.HideGrade();
        this.CurDupType = DuplicateType.DupType_Normal;
        trench_normal_config _config = _cfg;
        base.gameObject.transform.FindChild("TitleVal").GetComponent<UILabel>().text = _config.name;
        base.gameObject.transform.FindChild("Info/Label").GetComponent<UILabel>().text = _config.desc;
        base.gameObject.transform.FindChild("Info/Count/Val").GetComponent<UILabel>().text = "无限";
        base.gameObject.transform.FindChild("Info/Physical/Val").GetComponent<UILabel>().text = _config.cost_phy_force.ToString();
        ActorData.getInstance().m_nNormalDupCostPhysical = _config.cost_phy_force;
        this.TrenchEntry = _config.entry;
        this.UpdateGrade();
        this.UpdateMonsterInfo(_config.monster_desc);
        this.DupOutput(_config);
        this.UpdateRushBtnLabel(10);
        this.RushCount = 10;
        this.ShowBuyBtn(false);
    }

    public void UpdateOutlandMapInfo(S2C_OutlandCreateMapReq res)
    {
        ActorData.getInstance().CurDupType = DuplicateType.DupType_Outland;
        this.CurDupType = DuplicateType.DupType_Outland;
        this.mOutlandEntry = res.entry;
        if (this.mOutlandEntry >= 0)
        {
            this.HideGrade();
            outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(res.entry);
            outland_map_type_config _config2 = ConfigMgr.getInstance().getByEntry<outland_map_type_config>(_config.outland_type);
            this.mOutlandType = _config2.entry;
            base.gameObject.transform.FindChild("Title1").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e53);
            base.gameObject.transform.FindChild("TitleVal").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x4e2a), _config2.name, _config.layer);
            base.gameObject.transform.FindChild("Info/Count").gameObject.SetActive(false);
            this.mOutlandCostPhysical = _config.cost_phy_force;
            base.gameObject.transform.FindChild("Info/Physical/Val").GetComponent<UILabel>().text = this.mOutlandCostPhysical.ToString();
            OutlandFloorPass pass = null;
            foreach (OutlandFloorPass pass2 in res.data.activity_data.floorData.floorInfo)
            {
                if (pass2.entry == res.entry)
                {
                    pass = pass2;
                    break;
                }
            }
            if (((pass != null) && pass.is_pass) && (pass.remain > 0))
            {
                base.gameObject.transform.FindChild("Rush").gameObject.SetActive(true);
                this.ShowRushBtn(false);
                base.gameObject.transform.FindChild("Rush/IconOutland").gameObject.SetActive(true);
                UIButton component = base.gameObject.transform.FindChild("Rush/OutlandRushBtn").GetComponent<UIButton>();
                component.gameObject.SetActive(true);
                this.mOutlandFloorCost = ActorData.getInstance().outlandFloorList[_config.layer - 1].cost_num;
                base.gameObject.transform.FindChild("Rush/IconOutland/Val").GetComponent<UILabel>().text = this.mOutlandFloorCost.ToString();
                GUIDataHolder.setData(component.gameObject, res.entry);
                UIEventListener.Get(component.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRushOutland);
            }
            else
            {
                base.gameObject.transform.FindChild("Rush").gameObject.SetActive(false);
            }
            List<int> entryList = new List<int>();
            foreach (OutlandBossInfo info in res.data.activity_data.bossInfo)
            {
                if (info.entry == res.entry)
                {
                    outland_event_config _config3 = ConfigMgr.getInstance().getByEntry<outland_event_config>(info.eventID);
                    if (_config3 != null)
                    {
                        monster_config _config4 = ConfigMgr.getInstance().getByEntry<monster_config>(StrParser.ParseDecInt(_config3.model, -1));
                        entryList.Add(_config4.entry);
                    }
                }
            }
            this.UpdateMonsterInfoFromEntryList(entryList);
            List<int> list2 = new List<int>();
            for (int i = 0; (i < res.goods.itemList.Count) && (i < 5); i++)
            {
                list2.Add(res.goods.itemList[i].item_entry);
            }
            this.UdpateDropItemData(null, list2);
        }
    }

    private void UpdateRushBtnLabel(int _count)
    {
        UILabel component = base.gameObject.transform.FindChild("Rush/RushTenBtn/Label").GetComponent<UILabel>();
        if (this.CurDupType == DuplicateType.DupType_Normal)
        {
            component.text = "扫荡10次";
        }
        else if (this.CurDupType == DuplicateType.DupType_Elite)
        {
            component.text = string.Format("扫荡{0}次", _count);
        }
    }

    public void UpdateRushRemain()
    {
        if (this.mGrade < 3)
        {
            base.gameObject.transform.FindChild("Rush").gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.transform.FindChild("Rush").gameObject.SetActive(true);
            this.ShowRushOutlandBtn(false);
            base.gameObject.transform.FindChild("Rush/Icon/Val").GetComponent<UILabel>().text = ActorData.getInstance().GetDupSmashTimes().ToString();
            if (ActorData.getInstance().GetDupSmashTimes() > 0)
            {
                base.gameObject.transform.FindChild("Rush/RushTenBtn").gameObject.SetActive(true);
                base.gameObject.transform.FindChild("Rush/RushBtn").gameObject.SetActive(true);
                base.gameObject.transform.FindChild("Rush/BuyRushCnt").gameObject.SetActive(false);
                base.gameObject.transform.FindChild("Rush/Icon").gameObject.SetActive(true);
            }
            else
            {
                base.gameObject.transform.FindChild("Rush/RushTenBtn").gameObject.SetActive(false);
                base.gameObject.transform.FindChild("Rush/RushBtn").gameObject.SetActive(false);
                base.gameObject.transform.FindChild("Rush/BuyRushCnt").gameObject.SetActive(true);
                base.gameObject.transform.FindChild("Rush/Icon").gameObject.SetActive(false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <BuyTimes>c__AnonStorey1BC
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__295(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__2A1(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <BuyTimes>c__AnonStorey1BD
    {
        private static UIEventListener.VoidDelegate <>f__am$cache4;
        internal DupLevInfoPanel <>f__this;
        internal buy_cost_config BuyCfg;
        internal FastBuf.TrenchData data;
        internal int MAX_count;

        internal void <>m__296(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate (GameObject go) {
                };
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0xa652b2), this.BuyCfg.buy_elite_cost_stone, this.MAX_count, this.data.buy_times + 1), go => SocketMgr.Instance.RequestDuplicateBuyTimes(this.<>f__this.GetDupData(), this.MAX_count), <>f__am$cache4, false);
        }

        internal void <>m__29F(GameObject go)
        {
            SocketMgr.Instance.RequestDuplicateBuyTimes(this.<>f__this.GetDupData(), this.MAX_count);
        }

        private static void <>m__2A0(GameObject go)
        {
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickRushOutland>c__AnonStorey1BB
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__292(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__29E(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickSmashBuy>c__AnonStorey1BE
    {
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        private static UIEventListener.VoidDelegate <>f__am$cache3;
        internal int Cost;
        internal int Count;

        internal void <>m__29B(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = go => SocketMgr.Instance.RequestDuplicateBuySmashTimes();
            }
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (GameObject go) {
                };
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x4b1), this.Cost, this.Count), <>f__am$cache2, <>f__am$cache3, false);
        }

        private static void <>m__2A2(GameObject go)
        {
            SocketMgr.Instance.RequestDuplicateBuySmashTimes();
        }

        private static void <>m__2A3(GameObject go)
        {
        }
    }

    [CompilerGenerated]
    private sealed class <OnStartSmash>c__Iterator6E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal DupLevInfoPanel <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSeconds(0.4f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.isSmashing = false;
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

