using FastBuf;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class DupMap : GUIEntity
{
    private Transform _EliteTabBoard;
    private Transform _NormalTabBoard;
    public GameObject _SingleDupmapItem;
    private GameObject _Tips;
    public GameObject _TurnLeftBtn;
    public GameObject _TurnRightBtn;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache21;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache22;
    [CompilerGenerated]
    private static Comparison<duplicate_star_reward_config> <>f__am$cache23;
    public System.Action actReturn;
    private string[] DupChapterList = new string[] { "Ui_Fuben_Label_1", "Ui_Fuben_Label_2", "Ui_Fuben_Label_3", "Ui_Fuben_Label_4", "Ui_Fuben_Label_5", "Ui_Fuben_Label_6", "Ui_Fuben_Label_7", "Ui_Fuben_Label_8", "Ui_Fuben_Label_9", "Ui_Fuben_Label_10", "Ui_Fuben_Label_11", "Ui_Fuben_Label_12", "Ui_Fuben_Label_13", "Ui_Fuben_Label_14", "Ui_Fuben_Label_15", "Ui_Fuben_Label_16" };
    private float[] DupChapterPos = new float[] { 0f, 0f, 0f, 0.1234552f, 0.1234552f, 0.2494685f, 0.3700059f, 0.1513269f, 0.4006974f, 0.5925257f, 0.7520034f, 1f, 1f, 1f, 1f, 1f };
    private string[] DupFontList = new string[] { "Ui_Fuben_Label_ah", "Ui_Fuben_Label_sk", "Ui_Fuben_Label_yy", "Ui_Fuben_Label_zz", "Ui_Fuben_Label_xs", "Ui_Fuben_Label_gd", "Ui_Fuben_Label_atm", "Ui_Fuben_Label_zuer", "Ui_Fuben_Label_mld", "Ui_Fuben_Label_eyun", "Ui_Fuben_Label_sm", "Ui_Fuben_Label_heishi", "Ui_Fuben_Label_heixia", "Ui_Fuben_Label_stsm", "Ui_Fuben_Label_tongling", "Ui_Fuben_Label_heishang" };
    private int[] DupFontListNew = new int[] { 0x9d2b28, 0x9d2b29, 0x9d2b2a, 0x9d2b2b, 0x9d2b2c, 0x9d2b2d, 0x9d2b2e, 0x9d2b2f, 0x9d2b30, 0x9d2b31, 0x9d2b32, 0x9d2b33, 0x9d2b34, 0x9d2b35, 0x9d2b36, 0x9d2b37 };
    public GameObject DupItem;
    public List<GameObject> DupList = new List<GameObject>();
    private List<GameObject> DupObjBuffer = new List<GameObject>();
    private UIGrid grid;
    private string[] GuildDupList = new string[] { "Ui_Gonghuifb_Label_01xedk", "Ui_Gonghuifb_Label_02mwzz", "Ui_Gonghuifb_Label_03xrgd", "Ui_Gonghuifb_Label_04cmft", "Ui_Gonghuifb_Label_05grly", "Ui_Gonghuifb_Label_06wmzz" };
    private List<GameObject> GuildDupMap = new List<GameObject>();
    public List<Transform> guildMap = new List<Transform>();
    private bool m_bFirstShow;
    public List<GameObject> maskList = new List<GameObject>();
    private GameObject mCurClickDupObj;
    private duplicate_config mCurDupCfg;
    private DuplicateType mCurDupType;
    private int mCurrDupPageIdx = -1;
    private Dictionary<int, duplicate_config> mCurrOpenDupCfgDict = new Dictionary<int, duplicate_config>();
    private GameObject OpenNewDupObj;
    private ResetListPos ResetPanel;
    public GameObject ScaleMapRoot;
    private int sch_id = -1;
    public List<UIToggle> TabCheckList = new List<UIToggle>();
    public GameObject TrenchItem;
    private List<GameObject> TrenchObjBuffer = new List<GameObject>();
    private GameObject TrenchPart;

    private void CallBackPrePanel(System.Action action_finished)
    {
        <CallBackPrePanel>c__AnonStorey1BF storeybf = new <CallBackPrePanel>c__AnonStorey1BF {
            action_finished = action_finished
        };
        Debug.Log("111CallBackPrePanel11");
        storeybf.pre_laod = false;
        if (ActorData.getInstance().mCurrDupReturnPrePara != null)
        {
            switch (ActorData.getInstance().mCurrDupReturnPrePara.enterDuptype)
            {
                case EnterDupType.From_EquipBreak:
                    LoadingPerfab.BeginTransition();
                    ScheduleMgr.Schedule(0.3f, new System.Action(storeybf.<>m__2A8));
                    break;

                case EnterDupType.From_HeroInfoPanel:
                    LoadingPerfab.BeginTransition();
                    ScheduleMgr.Schedule(0.3f, new System.Action(storeybf.<>m__2A7));
                    break;

                case EnterDupType.From_HeroPanel:
                    LoadingPerfab.BeginTransition();
                    ScheduleMgr.Schedule(0.3f, new System.Action(storeybf.<>m__2A6));
                    break;
            }
        }
        if (!storeybf.pre_laod && (storeybf.action_finished != null))
        {
            storeybf.action_finished();
        }
    }

    private void ClickEliteTab()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Trench_Elite) && (this.mCurDupType != DuplicateType.DupType_Elite))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_select_elitetrench_tab, null);
        }
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().elite_dup)
        {
            this.TabCheckList[0].value = true;
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().elite_dup));
            this._NormalTabBoard.gameObject.SetActive(true);
            this._EliteTabBoard.gameObject.SetActive(false);
        }
        else
        {
            this._NormalTabBoard.gameObject.SetActive(false);
            this._EliteTabBoard.gameObject.SetActive(true);
            List<int> configEntry = CommonFunc.GetConfigEntry(this.mCurDupCfg.normal_entry);
            int num = configEntry[configEntry.Count - 1];
            if (ActorData.getInstance().NormalProgress <= num)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x19f));
            }
            else if (this.mCurDupCfg.entry > 0)
            {
                duplicate_config _config = ConfigMgr.getInstance().getByEntry<duplicate_config>(this.mCurDupCfg.entry - 1);
                if (_config != null)
                {
                    List<int> list2 = CommonFunc.GetConfigEntry(_config.elite_entry);
                    int num2 = list2[list2.Count - 1];
                    if (ActorData.getInstance().EliteProgress <= num2)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x1a0));
                    }
                }
            }
            this.mCurDupType = DuplicateType.DupType_Elite;
            this.UpdateTrenchData(this.mCurDupCfg);
            this.UpdateLevelCountLabel(true);
            if (GuideSystem.MatchEvent(GuideEvent.Trench_Elite) && !Utility.GenerateDupMapEliteTrenchNode())
            {
                GuideSystem.ActivedGuide.RequestCancel();
                Utility.NewbiestUnlock();
            }
        }
    }

    private void ClickNormalTab()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Trench_Elite))
        {
            GuideSystem.ActivedGuide.RequestCancel();
            Utility.NewbiestUnlock();
            Utility.EnforceClear();
        }
        this.mCurDupType = DuplicateType.DupType_Normal;
        this.UpdateTrenchData(this.mCurDupCfg);
        this.UpdateLevelCountLabel(true);
        this._NormalTabBoard.gameObject.SetActive(true);
        this._EliteTabBoard.gameObject.SetActive(false);
    }

    private void ClickReturn()
    {
        this._Tips.SetActive(false);
        ActorData.getInstance().DupListVal = 0f;
        this.EnableDragMap(true);
        this.TrenchPart.SetActive(false);
        this._TurnLeftBtn.gameObject.SetActive(false);
        this._TurnRightBtn.gameObject.SetActive(false);
        this.ShowArrow(true);
        Debug.Log("Click 1 " + this.m_bFirstShow);
        this.m_bFirstShow = false;
        Debug.Log("Click 2 " + this.m_bFirstShow);
        this.ShowTitleBar();
        if (this.actReturn != null)
        {
            this.actReturn();
        }
        this.actReturn = null;
        if (GuideSystem.MatchEvent(GuideEvent.Trench) || GuideSystem.MatchEvent(GuideEvent.Trench_Elite))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
    }

    private void ClosePanel()
    {
        ActorData.getInstance().OpenNewDup = false;
        if (GuideSystem.MatchEvent(GuideEvent.Trench) || GuideSystem.MatchEvent(GuideEvent.Trench_Elite))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        this.CallBackPrePanel(delegate {
            GUIMgr.Instance.CloseGUIEntity(base.entity_id);
            BundleMgr.Instance.ClearCache();
        });
    }

    [DebuggerHidden]
    private IEnumerator CreateDupData(GameObject obj, duplicate_config cfg, bool _OnlyShow)
    {
        return new <CreateDupData>c__Iterator70 { obj = obj, cfg = cfg, _OnlyShow = _OnlyShow, <$>obj = obj, <$>cfg = cfg, <$>_OnlyShow = _OnlyShow, <>f__this = this };
    }

    private void CreateGuildDupItem(Transform pos, guilddup_config cfg, bool _OnlyShow)
    {
        GameObject item = UnityEngine.Object.Instantiate(this.DupItem) as GameObject;
        item.transform.parent = pos;
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = Vector3.zero;
        this.GuildDupMap.Add(item);
        UITexture com = pos.FindChild<UITexture>("Texture");
        if (com != null)
        {
            com.ActiveSelfObject(true);
        }
        UILabel label = item.transform.FindChild<UILabel>("Label");
        if (label != null)
        {
            label.text = cfg.name;
        }
        UISprite sprite = item.transform.FindChild<UISprite>("Sprite");
        UISprite sprite2 = item.transform.FindChild<UISprite>("Chapter");
        if (sprite != null)
        {
            sprite.ActiveSelfObject(false);
        }
        if (sprite2 != null)
        {
            sprite2.ActiveSelfObject(false);
        }
        if (cfg.entry < this.GuildDupMap.Count)
        {
            UISprite sprite3 = item.transform.FindChild<UISprite>("GuildSprite");
            sprite3.spriteName = this.GuildDupList[cfg.entry];
            sprite3.MakePixelPerfect();
            UISprite sprite4 = item.transform.FindChild<UISprite>("GuildChapter1");
            sprite4.spriteName = this.DupChapterList[cfg.entry];
            sprite4.MakePixelPerfect();
        }
        item.transform.FindChild<UILabel>("Tips").text = string.Empty;
        UILabel label3 = item.transform.FindChild<UILabel>("GuildTips");
        int num = -1;
        GuildData mGuildData = ActorData.getInstance().mGuildData;
        if (mGuildData != null)
        {
            num = (int) mGuildData.tech.hall_level;
        }
        label3.ActiveSelfObject(num < cfg.guild_unlock_lv);
        if (num < cfg.guild_unlock_lv)
        {
            item.transform.OnUIMouseClick(u => this.OnGuildMap(u as guilddup_config)).userState = cfg;
            label3.text = string.Format(XSingleton<ConfigMgr>.Singleton[0x2c86], cfg.guild_unlock_lv);
        }
        else if (ActorData.getInstance().Level >= cfg.unlock_lv)
        {
            item.transform.OnUIMouseClick(u => this.OnGuildMap(u as guilddup_config)).userState = !_OnlyShow ? cfg : null;
        }
        else
        {
            item.transform.OnUIMouseClick(u => this.OnGuildMap(u as guilddup_config)).userState = cfg;
        }
        label3.text = string.Empty;
    }

    private List<RewardItem> CreateRewardItemListByItem(List<Item> _ItemList, int _gold, int _stone)
    {
        List<RewardItem> list = new List<RewardItem>();
        if (_ItemList != null)
        {
            foreach (Item item in _ItemList)
            {
                if (item.entry >= 0)
                {
                    RewardItem item2 = new RewardItem {
                        type = ItemType.ItemType_Item,
                        Data = item
                    };
                    list.Add(item2);
                }
            }
        }
        if (_gold > 0)
        {
            RewardItem item3 = new RewardItem {
                type = ItemType.ItemType_Gold,
                Data = _gold
            };
            list.Add(item3);
        }
        if (_stone > 0)
        {
            RewardItem item4 = new RewardItem {
                type = ItemType.ItemType_Stone,
                Data = _stone
            };
            list.Add(item4);
        }
        return list;
    }

    private void CreateTrenchIcon(GameObject obj, string Stricon)
    {
        obj.transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(Stricon);
    }

    private DupPassState DupState(duplicate_config _cfg)
    {
        List<int> configEntry = new List<int>();
        int normalProgress = 0;
        DupPassState unPass = DupPassState.UnPass;
        configEntry = CommonFunc.GetConfigEntry(_cfg.normal_entry);
        normalProgress = ActorData.getInstance().NormalProgress;
        foreach (int num2 in configEntry)
        {
            if (num2 == normalProgress)
            {
                return DupPassState.Passing;
            }
            if (normalProgress > num2)
            {
                return DupPassState.Pass;
            }
        }
        return unPass;
    }

    private void EnableDragMap(bool _enable)
    {
        UIScrollView component = base.gameObject.transform.FindChild("Map").GetComponent<UIScrollView>();
        if (_enable)
        {
            component.customMovement = new Vector2(2f, 0f);
        }
        else
        {
            component.customMovement = new Vector2(0f, 0f);
        }
    }

    private int GetAtkCountFromList(List<FastBuf.TrenchData> _List, int _Entry)
    {
        if (_List != null)
        {
            foreach (FastBuf.TrenchData data in _List)
            {
                if (data.entry == _Entry)
                {
                    return data.remain;
                }
            }
        }
        return 0;
    }

    private int GetCfgInDictIdx(duplicate_config dc)
    {
        ArrayList list = ConfigMgr.getInstance().getList<duplicate_config>();
        foreach (KeyValuePair<int, duplicate_config> pair in this.mCurrOpenDupCfgDict)
        {
            if (pair.Value == dc)
            {
                if (pair.Key == 0)
                {
                    this._TurnLeftBtn.SetActive(false);
                }
                else
                {
                    this._TurnLeftBtn.SetActive(true);
                }
                if (pair.Key == (list.Count - 1))
                {
                    this._TurnRightBtn.SetActive(false);
                }
                else
                {
                    this._TurnRightBtn.SetActive(true);
                }
                return pair.Key;
            }
        }
        return 0;
    }

    private duplicate_config GetDupCfg(bool isNext)
    {
        if (isNext)
        {
            ArrayList list = ConfigMgr.getInstance().getList<duplicate_config>();
            duplicate_config _config = (duplicate_config) list[this.mCurrDupPageIdx];
            if (this.mCurDupType == DuplicateType.DupType_Normal)
            {
                List<int> configEntry = CommonFunc.GetConfigEntry(_config.normal_entry);
                int num = configEntry[configEntry.Count - 1];
                if (ActorData.getInstance().NormalProgress <= num)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x19f));
                    return null;
                }
                duplicate_config _config2 = (duplicate_config) list[this.mCurrDupPageIdx + 1];
                if ((_config2 != null) && (ActorData.getInstance().Level < _config2.unlock_lv))
                {
                    TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x840), _config2.unlock_lv));
                    return null;
                }
            }
            else if (this.mCurDupType == DuplicateType.DupType_Elite)
            {
                List<int> list3 = CommonFunc.GetConfigEntry(_config.elite_entry);
                int num2 = list3[list3.Count - 1];
                if (ActorData.getInstance().EliteProgress <= num2)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x83f));
                    return null;
                }
                duplicate_config _config3 = (duplicate_config) list[this.mCurrDupPageIdx + 1];
                if ((_config3 != null) && (ActorData.getInstance().Level < _config3.unlock_lv))
                {
                    TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x840), _config3.unlock_lv));
                    return null;
                }
            }
            this.mCurrDupPageIdx++;
            this._TurnRightBtn.gameObject.SetActive(this.mCurrDupPageIdx < (list.Count - 1));
            if (!this._TurnLeftBtn.active)
            {
                this._TurnLeftBtn.SetActive(true);
            }
        }
        else
        {
            this.mCurrDupPageIdx--;
            if (this.mCurrDupPageIdx < 0)
            {
                this.mCurrDupPageIdx = 0;
            }
            duplicate_config _config4 = ConfigMgr.getInstance().getByEntry<duplicate_config>(this.mCurrDupPageIdx);
            if (_config4 != null)
            {
                int num3 = CommonFunc.GetConfigEntry(_config4.elite_entry)[0];
                if ((ActorData.getInstance().EliteProgress < num3) && (this.mCurDupType == DuplicateType.DupType_Elite))
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x1a0));
                }
            }
            this._TurnLeftBtn.gameObject.SetActive(this.mCurrDupPageIdx != 0);
            if (!this._TurnRightBtn.active)
            {
                this._TurnRightBtn.SetActive(true);
            }
        }
        return this.mCurrOpenDupCfgDict[this.mCurrDupPageIdx];
    }

    private void GetDupRewardInfo(out int starMaxNum, out int starCurNum, out List<int> starHasList, out List<int> starList)
    {
        starMaxNum = 0;
        starCurNum = 0;
        List<FastBuf.TrenchData> list = null;
        if (this.mCurDupType == DuplicateType.DupType_Normal)
        {
            ActorData.getInstance().TrenchNormalDataDict.TryGetValue(this.mCurDupCfg.entry, out list);
        }
        else if (this.mCurDupType == DuplicateType.DupType_Elite)
        {
            ActorData.getInstance().TrenchEliteDataDict.TryGetValue(this.mCurDupCfg.entry, out list);
        }
        if (list != null)
        {
            foreach (FastBuf.TrenchData data in list)
            {
                if ((data != null) && (data.grade >= 0))
                {
                    starCurNum += data.grade;
                }
            }
        }
        starHasList = new List<int>();
        DuplicateRewardInfo info = ActorData.getInstance().DupRewardInfo.Find(obj => obj.duplicateEntry == this.mCurDupCfg.entry);
        if (info != null)
        {
            if (this.mCurDupType == DuplicateType.DupType_Normal)
            {
                starHasList = info.normalStar;
            }
            else if (this.mCurDupType == DuplicateType.DupType_Elite)
            {
                starHasList = info.eliteStar;
            }
        }
        starList = new List<int>();
        ArrayList list2 = ConfigMgr.getInstance().getList<duplicate_star_reward_config>();
        int num = 0;
        IEnumerator enumerator = list2.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                duplicate_star_reward_config current = (duplicate_star_reward_config) enumerator.Current;
                if (num >= 3)
                {
                    goto Label_01BC;
                }
                if ((current.duplicate_entry == this.mCurDupCfg.entry) && (current.duplicate_type == this.mCurDupType))
                {
                    starList.Add(current.pick_limit);
                    starMaxNum = Math.Max(starMaxNum, current.pick_limit);
                    num++;
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
    Label_01BC:
        starList.Sort();
    }

    private GameObject GetGameObjectByData(duplicate_config _cfg)
    {
        foreach (GameObject obj2 in this.DupList)
        {
            duplicate_config _config = (duplicate_config) GUIDataHolder.getData(obj2);
            if ((_config != null) && (_config.entry == _cfg.entry))
            {
                return obj2;
            }
        }
        return this.DupList[0];
    }

    private void GetMapScrollMapVal()
    {
        UIScrollBar component = base.gameObject.transform.FindChild("Scroll Bar").GetComponent<UIScrollBar>();
        ActorData.getInstance().DupMapVal = component.value;
    }

    private bool GetNewOpenDup(int SrcDupEntry, out string DupName)
    {
        duplicate_config _config = ConfigMgr.getInstance().getByEntry<duplicate_config>(SrcDupEntry + 1);
        if (_config == null)
        {
            DupName = string.Empty;
            return false;
        }
        if (this.mCurDupType == DuplicateType.DupType_Normal)
        {
            foreach (int num in CommonFunc.GetConfigEntry(_config.normal_entry))
            {
                if (ActorData.getInstance().NormalProgress == num)
                {
                    DupName = ConfigMgr.getInstance().GetWord(0xa344ec) + _config.name;
                    return true;
                }
            }
        }
        else if (this.mCurDupType == DuplicateType.DupType_Elite)
        {
            foreach (int num2 in CommonFunc.GetConfigEntry(_config.elite_entry))
            {
                if (ActorData.getInstance().EliteProgress == num2)
                {
                    DupName = ConfigMgr.getInstance().GetWord(0xa344ed) + _config.name;
                    return true;
                }
            }
        }
        DupName = string.Empty;
        return true;
    }

    private void GetScrollListVal()
    {
        UIScrollBar component = base.gameObject.transform.FindChild("TrenchPart/LevelPart/Scroll Bar").GetComponent<UIScrollBar>();
        ActorData.getInstance().DupListVal = component.value;
    }

    public override void GUIStart()
    {
    }

    private bool LevelAllIsPass(duplicate_config _cfgData)
    {
        bool flag = true;
        List<int> list = new List<int>();
        DuplicateType mCurDupType = this.mCurDupType;
        if (mCurDupType != DuplicateType.DupType_Normal)
        {
            if (mCurDupType != DuplicateType.DupType_Elite)
            {
                return flag;
            }
        }
        else
        {
            foreach (int num in CommonFunc.GetConfigEntry(_cfgData.normal_entry))
            {
                if (ActorData.getInstance().NormalProgress <= num)
                {
                    return false;
                }
            }
            return flag;
        }
        foreach (int num2 in CommonFunc.GetConfigEntry(_cfgData.elite_entry))
        {
            if (ActorData.getInstance().EliteProgress <= num2)
            {
                return false;
            }
        }
        return flag;
    }

    public void LookDuplicateReward(int entry, int mCurDupType, int index)
    {
        <LookDuplicateReward>c__AnonStorey1C4 storeyc = new <LookDuplicateReward>c__AnonStorey1C4 {
            mCurDupType = mCurDupType,
            entry = entry
        };
        List<duplicate_star_reward_config> list2 = ConfigMgr.getInstance().getList<duplicate_star_reward_config>().Cast<duplicate_star_reward_config>().Where<duplicate_star_reward_config>(new Func<duplicate_star_reward_config, bool>(storeyc.<>m__2B3)).ToList<duplicate_star_reward_config>();
        if (<>f__am$cache23 == null)
        {
            <>f__am$cache23 = (d1, d2) => d1.pick_limit - d2.pick_limit;
        }
        list2.Sort(<>f__am$cache23);
        if (index < list2.Count)
        {
            <LookDuplicateReward>c__AnonStorey1C5 storeyc2 = new <LookDuplicateReward>c__AnonStorey1C5();
            duplicate_star_reward_config _config = list2[index];
            if (_config != null)
            {
                UILabel component = base.transform.FindChild("TrenchMap/Tips/TGroup/LbNumStar").gameObject.GetComponent<UILabel>();
                if (component != null)
                {
                    component.text = _config.pick_limit.ToString();
                }
                S2C_PickDuplicateReward reward = new S2C_PickDuplicateReward {
                    gold = (uint) _config.reward_gold,
                    stone = (uint) _config.reward_stone
                };
                storeyc2.tempItem1 = new Item();
                storeyc2.tempItem1.entry = _config.item_1;
                storeyc2.tempItem1.diff = _config.item_num_1;
                if (storeyc2.tempItem1.entry != -1)
                {
                    Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeyc2.<>m__2B5));
                    storeyc2.tempItem1.num = (item != null) ? item.num : 0;
                }
                reward.itemList.Add(storeyc2.tempItem1);
                storeyc2.tempItem2 = new Item();
                storeyc2.tempItem2.entry = _config.item_2;
                storeyc2.tempItem2.diff = _config.item_num_2;
                if (storeyc2.tempItem2.entry != -1)
                {
                    Item item2 = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeyc2.<>m__2B6));
                    storeyc2.tempItem2.num = (item2 != null) ? item2.num : 0;
                }
                reward.itemList.Add(storeyc2.tempItem2);
                storeyc2.tempItem3 = new Item();
                storeyc2.tempItem3.entry = _config.item_3;
                storeyc2.tempItem3.diff = _config.item_num_3;
                if (storeyc2.tempItem3.entry != -1)
                {
                    Item item3 = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeyc2.<>m__2B7));
                    storeyc2.tempItem3.num = (item3 != null) ? item3.num : 0;
                }
                reward.itemList.Add(storeyc2.tempItem3);
                List<RewardItem> itemList = this.CreateRewardItemListByItem(reward.itemList, (int) reward.gold, (int) reward.stone);
                this.UpdateData(itemList);
            }
        }
    }

    private void MoveMapAfterBattle()
    {
        if (this.OpenNewDupObj != null)
        {
            if (this.OpenNewDupObj.transform.parent.name == "Map1")
            {
                ActorData.getInstance().DupMapVal = 0.10128f;
            }
            else if (this.OpenNewDupObj.transform.parent.name == "Map2")
            {
                ActorData.getInstance().DupMapVal = 0.5534245f;
            }
            else if (this.OpenNewDupObj.transform.parent.name == "Map3")
            {
                ActorData.getInstance().DupMapVal = 1f;
            }
            this.ScrollMap();
        }
    }

    private void OnBoxClickBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        int num = (obj2 == null) ? -1 : ((int) obj2);
        if (num >= 1)
        {
            this.OnClickStarBox(num - 1);
        }
    }

    private void OnBoxPressBtn(GameObject go, bool isPress)
    {
        if (isPress)
        {
            if (this._Tips != null)
            {
                this._Tips.SetActive(true);
                object obj2 = GUIDataHolder.getData(go);
                int num = (obj2 == null) ? -1 : ((int) obj2);
                if (num >= 1)
                {
                    this.LookDuplicateReward(this.mCurDupCfg.entry, (int) this.mCurDupType, num - 1);
                }
            }
        }
        else
        {
            this._Tips.SetActive(false);
        }
    }

    private void OnClickCopperBox()
    {
        this.OnClickStarBox(0);
    }

    private void OnClickDup(GameObject obj)
    {
        duplicate_config dc = (duplicate_config) GUIDataHolder.getData(obj);
        if (dc == null)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x83f));
        }
        else if (ActorData.getInstance().Level < dc.unlock_lv)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9ba3c4), dc.unlock_lv));
        }
        else
        {
            if (GuideSystem.MatchEvent(GuideEvent.Trench) || GuideSystem.MatchEvent(GuideEvent.Trench_Elite))
            {
                GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_battle_select_duplicate, obj);
            }
            this.mCurClickDupObj = obj.transform.parent.gameObject;
            this.ShowTrenchPart(dc);
        }
    }

    private void OnClickEliteTrench(GameObject obj)
    {
        <OnClickEliteTrench>c__AnonStorey1C3 storeyc = new <OnClickEliteTrench>c__AnonStorey1C3();
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().elite_dup)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().elite_dup));
        }
        else
        {
            storeyc.cfg = (trench_elite_config) GUIDataHolder.getData(obj);
            ActorData.getInstance().CurTrenchEntry = storeyc.cfg.entry;
            ActorData.getInstance().CurDupType = DuplicateType.DupType_Elite;
            this.GetScrollListVal();
            this.GetMapScrollMapVal();
            if (GuideSystem.MatchEvent(GuideEvent.Trench_Elite))
            {
                GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_battle_select_trench, obj);
            }
            GUIMgr.Instance.PushGUIEntity("DupLevInfoPanel", new Action<GUIEntity>(storeyc.<>m__2B1));
        }
    }

    private void OnClickGoldBox()
    {
        this.OnClickStarBox(2);
    }

    private void OnClickNormalTrench(GameObject obj)
    {
        <OnClickNormalTrench>c__AnonStorey1C2 storeyc = new <OnClickNormalTrench>c__AnonStorey1C2 {
            cfg = (trench_normal_config) GUIDataHolder.getData(obj)
        };
        ActorData.getInstance().CurTrenchEntry = storeyc.cfg.entry;
        ActorData.getInstance().CurDupType = DuplicateType.DupType_Normal;
        this.GetMapScrollMapVal();
        this.GetScrollListVal();
        if (GuideSystem.MatchEvent(GuideEvent.Trench))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_battle_select_trench, obj);
        }
        GUIMgr.Instance.PushGUIEntity("DupLevInfoPanel", new Action<GUIEntity>(storeyc.<>m__2B0));
    }

    private void OnClickSilverBox()
    {
        this.OnClickStarBox(1);
    }

    private void OnClickStarBox(int index)
    {
        int num;
        int num2;
        List<int> list;
        List<int> list2;
        this.GetDupRewardInfo(out num2, out num, out list, out list2);
        if (((index < list2.Count) && (num >= list2[index])) && !list.Contains(list2[index]))
        {
            SocketMgr.Instance.RequestDupReward(this.mCurDupCfg.entry, (int) this.mCurDupType, list2[index]);
        }
    }

    private void OnClickTurnLeft()
    {
        this.ShowTrenchPart(this.GetDupCfg(false));
    }

    private void OnClickTurnRight()
    {
        this.ShowTrenchPart(this.GetDupCfg(true));
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(true);
            activityGUIEntity.transform.FindChild("TopRight").gameObject.SetActive(true);
        }
        EventCenter.Instance.DoEvent(EventCenter.EventType.UIShow_Dupmap, null);
        this.UpdateMapData();
        this.DelayCallBack(0.1f, delegate {
            Transform transform = base.transform.FindChild("TrenchMap");
            if ((null != transform) && !transform.gameObject.activeSelf)
            {
                GuideSystem.FireEvent(GuideEvent.Trench);
                GuideSystem.FireEvent(GuideEvent.Trench_Elite);
            }
            if (GuideSystem.MatchEvent(GuideEvent.Trench) || GuideSystem.MatchEvent(GuideEvent.Trench_Elite))
            {
                List<GameObject> vari = new List<GameObject> {
                    base.transform.FindChild("Map/Grid/Map1/0/Texture").gameObject,
                    base.transform.FindChild("Map/Grid/Map1/0/DupItem(Clone)").gameObject
                };
                GuideSystem.ActivedGuide.RequestMultiGeneration(GuideRegister_Battle.tag_battle_select_duplicate, vari);
            }
        });
        if (this.OpenNewDupObj != null)
        {
            ActorData.getInstance().DupMapVal = this.DupChapterPos[this.mCurrOpenDupCfgDict.Count - 1];
        }
        this.ScrollMap();
        this.ResetMapRange();
    }

    private void OnGuildMap(guilddup_config dup)
    {
        <OnGuildMap>c__AnonStorey1C1 storeyc = new <OnGuildMap>c__AnonStorey1C1();
        if (dup != null)
        {
            storeyc.dupState = XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(dup.entry);
            if (storeyc.dupState != null)
            {
                GUIMgr.Instance.PushGUIEntity<GuildDupTrenchMap>(new Action<GUIEntity>(storeyc.<>m__2AF));
            }
        }
    }

    public override void OnInitialize()
    {
        this._NormalTabBoard = base.transform.FindChild("TrenchMap/NormalTabBoard");
        this._EliteTabBoard = base.transform.FindChild("TrenchMap/EliteTabBoard");
        this._Tips = base.transform.FindChild("TrenchMap/Tips").gameObject;
        this.grid = base.transform.FindChild("TrenchMap/Tips/List/Grid").GetComponent<UIGrid>();
        if (GUIMgr.Instance.Root.activeWidth > 0x3e8)
        {
            base.transform.FindChild("Left/Btn").GetComponent<BoxCollider>().size = new Vector3(86f, 450f, 0f);
            base.transform.FindChild("Right/Btn").GetComponent<BoxCollider>().size = new Vector3(86f, 450f, 0f);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        if (GuideSystem.MatchEvent(GuideEvent.Trench) || GuideSystem.MatchEvent(GuideEvent.Trench_Elite))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        GUIMgr.Instance.DockTitleBar();
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(false);
        }
    }

    [DebuggerHidden]
    private IEnumerator PlayDupAnim()
    {
        return new <PlayDupAnim>c__Iterator6F { <>f__this = this };
    }

    private void PlayNewOpenAnim(GameObject Obj)
    {
        Vector3 pos = new Vector3(0f, -15f, 0f);
        TweenPosition position = TweenPosition.Begin(Obj, 0.5f, pos);
        position.method = UITweener.Method.Linear;
        position.style = UITweener.Style.PingPong;
        position.from = Vector3.zero;
    }

    private void PopFriendShip()
    {
        if ((ActorData.getInstance().mFriendReward != null) && (ActorData.getInstance().mFriendReward.userInfo.id > 0L))
        {
            <PopFriendShip>c__AnonStorey1C0 storeyc = new <PopFriendShip>c__AnonStorey1C0 {
                have = false
            };
            XSingleton<SocialFriend>.Singleton.Each(new SocialFriend.EachCondtion(storeyc.<>m__2AA));
            if (!storeyc.have)
            {
                if (<>f__am$cache22 == null)
                {
                    <>f__am$cache22 = obj => obj.Achieve<FriendResultPanel>().UpdateData(ActorData.getInstance().mFriendReward);
                }
                GUIMgr.Instance.DoModelGUI<FriendResultPanel>(<>f__am$cache22, base.gameObject);
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator PopMapAnim()
    {
        return new <PopMapAnim>c__Iterator71 { <>f__this = this };
    }

    private void PopTrenchPart(duplicate_config _cfg)
    {
        this.mCurrDupPageIdx = this.GetCfgInDictIdx(_cfg);
        this.EnableDragMap(false);
        this.ShowArrow(false);
        this.TrenchPart.SetActive(true);
        TweenAlpha.Begin(this.TrenchPart.transform.FindChild("Board/mask").gameObject, 0.3f, 0.78f);
        EventCenter.Instance.DoEvent(EventCenter.EventType.UIShow_Trench, null);
        if (!this.m_bFirstShow)
        {
            this.mCurDupType = DuplicateType.DupType_Normal;
            this.UpdateToggleState();
            this.m_bFirstShow = true;
        }
        this.UpdateTrenchData(_cfg);
        this.UpdateLevelCountLabel(true);
        if (GuideSystem.MatchEvent(GuideEvent.Trench))
        {
            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Battle.tag_battle_select_trench, base.transform.FindChild("TrenchMap/MapPos/0/1").gameObject);
        }
        if (GuideSystem.MatchEvent(GuideEvent.Trench_Elite) && !Utility.GenerateDupMapEliteTab())
        {
            Utility.NewbiestUnlock();
            GuideSystem.ActivedGuide.RequestCancel();
        }
    }

    public static void RefreshDupRewardUI()
    {
        DupMap component = GameObject.Find("UI Root/Camera/DupMap").GetComponent<DupMap>();
        if (component != null)
        {
            component.UpdateDupRewardInfo();
        }
    }

    public void RequestBoxGeneration()
    {
    }

    private void ResetListPos()
    {
        this.ResetPanel.PanelReset();
    }

    private void ResetMap()
    {
        TweenScale.Begin(this.ScaleMapRoot, 0.4f, new Vector3(1f, 1f, 1f));
        TweenPosition.Begin(this.ScaleMapRoot, 0.4f, Vector3.one);
    }

    private void ResetMapRange()
    {
        UIPanel component = base.gameObject.transform.FindChild("Map").GetComponent<UIPanel>();
        int activeHeight = GUIMgr.Instance.Root.activeHeight;
        float width = (((float) Screen.width) / ((float) Screen.height)) * activeHeight;
        component.SetRect(0f, 0f, width, (float) activeHeight);
    }

    public void ResetMapScroll()
    {
        ActorData.getInstance().DupMapVal = 0f;
        this.ScrollMap();
        this.ScrollList();
        Transform transform = base.transform.FindChild("Map");
        if (null != transform)
        {
            UIScrollView component = transform.GetComponent<UIScrollView>();
            if (null != component)
            {
                component.ResetPosition();
            }
        }
    }

    private void ScaleMap()
    {
        Transform transform = this.mCurClickDupObj.transform.parent.FindChild("Target").transform;
        float x = this.mCurClickDupObj.transform.parent.transform.localPosition.x;
        float num2 = ((-(this.mCurClickDupObj.transform.localPosition.x - transform.localPosition.x) * 1.5f) - (0.5f * x)) + 30f;
        float y = -(this.mCurClickDupObj.transform.localPosition.y - transform.localPosition.y) * 1.5f;
        TweenPosition.Begin(this.ScaleMapRoot, 0.4f, new Vector3(num2, y, 0f));
        TweenScale.Begin(this.ScaleMapRoot, 0.4f, new Vector3(1.5f, 1.5f, 1f));
    }

    private void ScrollList()
    {
        base.gameObject.transform.FindChild("TrenchPart/LevelPart/Scroll Bar").GetComponent<UIScrollBar>().value = ActorData.getInstance().DupListVal;
    }

    private void ScrollMap()
    {
        base.gameObject.transform.FindChild("Scroll Bar").GetComponent<UIScrollBar>().value = ActorData.getInstance().DupMapVal;
    }

    private void SetCenterCtrl()
    {
        this.ScaleMapRoot.GetComponent<UICenterOnChild>().EnableRecenter = false;
    }

    private unsafe void SetItemInfo(Transform obj, RewardItem _Itemdata)
    {
        if (_Itemdata != null)
        {
            UITexture component = obj.FindChild("Icon").GetComponent<UITexture>();
            UISprite sprite = obj.FindChild("QualityBorder").GetComponent<UISprite>();
            UISprite sprite2 = obj.FindChild("Border").GetComponent<UISprite>();
            UILabel label = obj.FindChild("Name").GetComponent<UILabel>();
            UILabel label2 = obj.FindChild("Count").GetComponent<UILabel>();
            GameObject gameObject = obj.FindChild("sprite").gameObject;
            ItemType type = _Itemdata.type;
            switch (type)
            {
                case ItemType.ItemType_Gold:
                    label.text = ConfigMgr.getInstance().GetWord(0x89);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Gold");
                    sprite.color = *((Color*) &(GameConstant.ConstQuantityColor[0]));
                    label2.text = _Itemdata.Data.ToString();
                    break;

                case ItemType.ItemType_Stone:
                    label.text = ConfigMgr.getInstance().GetWord(0x31b);
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
                    sprite.gameObject.SetActive(true);
                    label2.text = _Itemdata.Data.ToString();
                    break;

                default:
                    if (type == ItemType.ItemType_Item)
                    {
                        Item data = _Itemdata.Data as Item;
                        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(data.entry);
                        if (_config != null)
                        {
                            component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                            sprite.color = *((Color*) &(GameConstant.ConstQuantityColor[_config.quality]));
                            label.text = _config.name;
                            label2.text = data.diff.ToString();
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
                    break;
            }
        }
    }

    private void ShowArrow(bool _show)
    {
    }

    private void ShowNextDup(int index)
    {
        if (index <= (this.DupList.Count - 1))
        {
            GameObject go = this.DupList[index];
            duplicate_config data = ConfigMgr.getInstance().getByEntry<duplicate_config>(index);
            GUIDataHolder.setData(go, data);
            base.StartCoroutine(this.CreateDupData(go, data, true));
        }
    }

    private void ShowTitleBar()
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.transform.FindChild("TopRight").gameObject.SetActive(true);
        }
    }

    private void ShowTrenchPart(duplicate_config dc)
    {
        if (dc != null)
        {
            this.mCurDupCfg = dc;
            ActorData.getInstance().CurDupEntry = dc.entry;
            SocketMgr.Instance.RequestGetDuplicateRemain(this.mCurDupCfg.entry, DuplicateType.DupType_Normal);
            SocketMgr.Instance.RequestGetDuplicateRemain(this.mCurDupCfg.entry, DuplicateType.DupType_Elite);
            this.PopTrenchPart(dc);
        }
    }

    private bool TrenchIsOpen(int _entry)
    {
        if (this.mCurDupType == DuplicateType.DupType_Normal)
        {
            return (ActorData.getInstance().NormalProgress >= _entry);
        }
        if (this.mCurDupType != DuplicateType.DupType_Elite)
        {
            return false;
        }
        return (ActorData.getInstance().EliteProgress >= _entry);
    }

    private void UpdateBattleComplete()
    {
        duplicate_config _config = ConfigMgr.getInstance().getByEntry<duplicate_config>(ActorData.getInstance().CurDupEntry);
        this.mCurDupCfg = _config;
        GameObject gameObjectByData = this.GetGameObjectByData(_config);
        this.mCurClickDupObj = gameObjectByData;
        this.mCurDupType = ActorData.getInstance().CurDupType;
        if (this.LevelAllIsPass(_config) && ActorData.getInstance().OpenNewDup)
        {
            string str;
            base.StartCoroutine(this.PlayDupAnim());
            if (this.GetNewOpenDup(_config.entry, out str))
            {
                TipsDiag.SetText(str);
            }
            ActorData.getInstance().DupListVal = 0f;
        }
        else
        {
            this.UpdateToggleState();
            this.PopFriendShip();
            this.m_bFirstShow = true;
            this.PopTrenchPart(_config);
        }
    }

    private void UpdateBattleReplayComplete()
    {
        this.UpdateBattleComplete();
        if (<>f__am$cache21 == null)
        {
            <>f__am$cache21 = delegate (GUIEntity guiE) {
                DupLevInfoPanel panel = guiE.Achieve<DupLevInfoPanel>();
                panel.mIsDupEnter = true;
                panel.UpdateBattleComplete();
            };
        }
        GUIMgr.Instance.PushGUIEntity("DupLevInfoPanel", <>f__am$cache21);
    }

    private void UpdateData(List<RewardItem> itemList)
    {
        if (this.grid != null)
        {
            CommonFunc.DeleteChildItem(this.grid.transform);
            Debug.Log("===========>" + itemList.Count);
            if (itemList.Count == 0)
            {
                return;
            }
            foreach (RewardItem item in itemList)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleDupmapItem) as GameObject;
                obj2.transform.parent = this.grid.transform;
                obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                this.SetItemInfo(obj2.transform, item);
            }
        }
        this.grid.repositionNow = true;
    }

    public void UpdateDupRewardInfo()
    {
        if (this.mCurDupCfg != null)
        {
            int num;
            int num2;
            List<int> list;
            List<int> list2;
            this.GetDupRewardInfo(out num2, out num, out list, out list2);
            UISlider component = base.gameObject.transform.FindChild("TrenchMap/LoadingSlider").GetComponent<UISlider>();
            component.value = ((float) num) / ((float) num2);
            for (int i = 0; i < 3; i++)
            {
                if (i < list2.Count)
                {
                    int item = list2[i];
                    int num5 = i + 1;
                    Transform transform = component.gameObject.transform.FindChild("Box" + num5.ToString());
                    UITexture texture = transform.FindChild("Texture").gameObject.GetComponent<UITexture>();
                    GUIDataHolder.setData(transform.gameObject, i + 1);
                    UIEventListener.Get(transform.gameObject).onClick = null;
                    UIEventListener.Get(transform.gameObject).onPress = null;
                    Vector3 localPosition = transform.localPosition;
                    localPosition.x = ((((float) item) / ((float) num2)) * 652f) + 10f;
                    transform.localPosition = localPosition;
                    if (list.Contains(item))
                    {
                        transform.transform.FindChild("Texture").gameObject.SetActive(false);
                        transform.transform.FindChild("TextureOpen").gameObject.SetActive(true);
                        transform.transform.FindChild("baoxiao_fuben").gameObject.SetActive(false);
                    }
                    else
                    {
                        transform.transform.FindChild("Texture").gameObject.SetActive(true);
                        transform.transform.FindChild("TextureOpen").gameObject.SetActive(false);
                        transform.transform.FindChild("baoxiao_fuben").gameObject.SetActive(num >= item);
                        if (num >= item)
                        {
                            Debug.Log((i + 1) + "   add  OnBoxClickBtn");
                            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnBoxClickBtn);
                            UIEventListener.Get(transform.gameObject).onPress = null;
                        }
                        else
                        {
                            Debug.Log((i + 1) + "add  OnBoxPressBtn");
                            UIEventListener.Get(transform.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnBoxPressBtn);
                        }
                    }
                }
            }
        }
    }

    private void UpdateGrade(GameObject obj, int _grade)
    {
        for (int i = 0; i <= _grade; i++)
        {
            if (i != 0)
            {
                if (i > 3)
                {
                    return;
                }
                string name = "Star/" + i;
                obj.transform.FindChild(name).gameObject.SetActive(true);
            }
        }
    }

    public void UpdateLevelCountLabel(bool _PlayAnim)
    {
        List<FastBuf.TrenchData> list = new List<FastBuf.TrenchData>();
        foreach (GameObject obj2 in this.TrenchObjBuffer)
        {
            UILabel component = obj2.transform.FindChild("Count").GetComponent<UILabel>();
            switch (this.mCurDupType)
            {
                case DuplicateType.DupType_Normal:
                {
                    ActorData.getInstance().TrenchNormalDataDict.TryGetValue(this.mCurDupCfg.entry, out list);
                    trench_normal_config _config = (trench_normal_config) GUIDataHolder.getData(obj2);
                    component.text = string.Empty;
                    this.UpdateGrade(obj2, CommonFunc.GetGradeFromList(list, _config.entry));
                    if (((ActorData.getInstance().NormalProgress == _config.entry) && (CommonFunc.GetGradeFromList(list, _config.entry) <= 0)) && _PlayAnim)
                    {
                        this.PlayNewOpenAnim(obj2);
                    }
                    break;
                }
                case DuplicateType.DupType_Elite:
                {
                    ActorData.getInstance().TrenchEliteDataDict.TryGetValue(this.mCurDupCfg.entry, out list);
                    trench_elite_config _config2 = (trench_elite_config) GUIDataHolder.getData(obj2);
                    this.UpdateGrade(obj2, CommonFunc.GetGradeFromList(list, _config2.entry));
                    if (((ActorData.getInstance().EliteProgress == _config2.entry) && (CommonFunc.GetGradeFromList(list, _config2.entry) <= 0)) && _PlayAnim)
                    {
                        this.PlayNewOpenAnim(obj2);
                    }
                    if (!this.TrenchIsOpen(_config2.entry))
                    {
                        component.text = string.Empty;
                    }
                    else
                    {
                        int atkCountFromList = this.GetAtkCountFromList(list, _config2.entry);
                        if (atkCountFromList > 0)
                        {
                            component.text = string.Format(ConfigMgr.getInstance().GetWord(0xa037cd), atkCountFromList);
                        }
                        else
                        {
                            component.text = string.Empty;
                        }
                    }
                    break;
                }
            }
        }
    }

    private void UpdateMapData()
    {
        this.mCurrOpenDupCfgDict.Clear();
        this.TrenchPart = base.gameObject.transform.FindChild("TrenchMap").gameObject;
        this.ResetPanel = base.gameObject.transform.FindChild("TrenchPart/LevelPart/Scroll View").GetComponent<ResetListPos>();
        ArrayList list = ConfigMgr.getInstance().getList<duplicate_config>();
        int key = 0;
        foreach (GameObject obj2 in this.DupObjBuffer)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        this.DupObjBuffer.Clear();
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                duplicate_config current = (duplicate_config) enumerator.Current;
                if (this.DupState(current) != DupPassState.UnPass)
                {
                    GameObject go = this.DupList[current.entry];
                    if (go == null)
                    {
                        Debug.LogWarning("Dup Entry in Editor Is Error! " + current.entry);
                    }
                    else
                    {
                        GUIDataHolder.setData(go, current);
                        this.mCurrOpenDupCfgDict.Add(key, current);
                        base.StartCoroutine(this.CreateDupData(go, current, false));
                        key++;
                        if (current.entry > 5)
                        {
                            this.maskList[0].SetActive(false);
                            this.maskList[1].SetActive(false);
                        }
                        if (current.entry > 10)
                        {
                            this.maskList[2].SetActive(false);
                            this.maskList[3].SetActive(false);
                        }
                        if (ActorData.getInstance().OpenNewDup && (this.DupState(current) == DupPassState.Passing))
                        {
                            this.OpenNewDupObj = go;
                        }
                    }
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
        foreach (GameObject obj4 in this.GuildDupMap)
        {
            UnityEngine.Object.Destroy(obj4);
        }
        this.GuildDupMap.Clear();
        foreach (guilddup_config _config2 in ConfigMgr.getInstance().getListResult<guilddup_config>())
        {
            GuildDupStatusInfo dupStateByEntry = XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(_config2.entry);
            if ((((dupStateByEntry != null) && ((dupStateByEntry.status != GuildDupStatusEnum.GuildDupStatus_Invalid) && (dupStateByEntry.status != GuildDupStatusEnum.GuildDupStatus_Close))) && ((dupStateByEntry.status != GuildDupStatusEnum.GuildDupStatus_Can_Open) && (_config2.entry >= 0))) && (_config2.entry < this.guildMap.Count))
            {
                this.CreateGuildDupItem(this.guildMap[_config2.entry], _config2, false);
            }
        }
        this.ShowNextDup(key);
        this.SetCenterCtrl();
    }

    private void UpdateToggleState()
    {
        if (this.mCurDupType == DuplicateType.DupType_Normal)
        {
            this.TabCheckList[0].value = true;
            this.TabCheckList[1].value = false;
        }
        else if (this.mCurDupType == DuplicateType.DupType_Elite)
        {
            this.TabCheckList[0].value = false;
            this.TabCheckList[1].value = true;
        }
    }

    private void UpdateTrenchData(duplicate_config _cfg)
    {
        UITexture component = this.TrenchPart.transform.FindChild("Map").GetComponent<UITexture>();
        GameObject gameObject = null;
        UILabel label = this.TrenchPart.transform.FindChild("Title/Label").GetComponent<UILabel>();
        int num = this.DupFontListNew[_cfg.entry];
        label.text = ConfigMgr.getInstance().GetWord(num);
        component.mainTexture = BundleMgr.Instance.CreateTrenchMapIcon(_cfg.background);
        int lastEntry = 0;
        List<int> configEntry = new List<int>();
        switch (this.mCurDupType)
        {
            case DuplicateType.DupType_Normal:
                gameObject = this.TrenchPart.transform.FindChild("MapPos/" + _cfg.entry).gameObject;
                configEntry = CommonFunc.GetConfigEntry(_cfg.normal_entry);
                break;

            case DuplicateType.DupType_Elite:
                gameObject = this.TrenchPart.transform.FindChild("EliteMapPos/" + _cfg.entry).gameObject;
                configEntry = CommonFunc.GetConfigEntry(_cfg.elite_entry);
                break;
        }
        if (gameObject == null)
        {
            Debug.LogWarning("_cfg Entry is error" + _cfg.entry);
        }
        else
        {
            lastEntry = configEntry[configEntry.Count - 1];
            foreach (GameObject obj3 in this.TrenchObjBuffer)
            {
                UnityEngine.Object.Destroy(obj3);
            }
            this.TrenchObjBuffer.Clear();
            int num3 = 0;
            foreach (int num4 in configEntry)
            {
                if (!this.TrenchIsOpen(num4))
                {
                    continue;
                }
                switch (this.mCurDupType)
                {
                    case DuplicateType.DupType_Normal:
                    {
                        int num6 = num3 + 1;
                        GameObject obj4 = gameObject.transform.FindChild(num6.ToString()).gameObject;
                        GameObject item = UnityEngine.Object.Instantiate(this.TrenchItem) as GameObject;
                        item.transform.parent = obj4.transform;
                        item.transform.localScale = Vector3.one;
                        item.transform.localPosition = Vector3.zero;
                        this.TrenchObjBuffer.Add(item);
                        trench_normal_config data = ConfigMgr.getInstance().getByEntry<trench_normal_config>(num4);
                        GUIDataHolder.setData(item, data);
                        UIEventListener listener1 = UIEventListener.Get(item);
                        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickNormalTrench));
                        if (ActorData.getInstance().NormalProgress == num4)
                        {
                            this.UpdateTrenchInfo(item, data, true, lastEntry);
                        }
                        else
                        {
                            this.UpdateTrenchInfo(item, data, false, lastEntry);
                        }
                        break;
                    }
                    case DuplicateType.DupType_Elite:
                    {
                        int num5 = num3 + 1;
                        if (num5 > 12)
                        {
                            num5 = 12;
                        }
                        GameObject obj6 = gameObject.transform.FindChild(num5.ToString()).gameObject;
                        GameObject obj7 = UnityEngine.Object.Instantiate(this.TrenchItem) as GameObject;
                        obj7.transform.parent = obj6.transform;
                        obj7.transform.localScale = Vector3.one;
                        obj7.transform.localPosition = Vector3.zero;
                        this.TrenchObjBuffer.Add(obj7);
                        trench_elite_config _config2 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(num4);
                        GUIDataHolder.setData(obj7, _config2);
                        UIEventListener listener2 = UIEventListener.Get(obj7);
                        listener2.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener2.onClick, new UIEventListener.VoidDelegate(this.OnClickEliteTrench));
                        if (ActorData.getInstance().EliteProgress == num4)
                        {
                            this.UpdateTrenchInfo(obj7, _config2, true, lastEntry);
                        }
                        else
                        {
                            this.UpdateTrenchInfo(obj7, _config2, false, lastEntry);
                        }
                        break;
                    }
                }
                num3++;
            }
            this.UpdateDupRewardInfo();
        }
    }

    private void UpdateTrenchInfo(GameObject _obj, object _data, bool NewOpen, int LastEntry)
    {
        UILabel component = _obj.transform.FindChild("Name/Label").GetComponent<UILabel>();
        GameObject gameObject = _obj.transform.FindChild("Normal").gameObject;
        GameObject obj3 = _obj.transform.FindChild("Boss").gameObject;
        Transform transform = _obj.transform.FindChild("Star");
        switch (this.mCurDupType)
        {
            case DuplicateType.DupType_Normal:
            {
                trench_normal_config _config = (trench_normal_config) _data;
                if (_config.entry == LastEntry)
                {
                    obj3.SetActive(true);
                    gameObject.SetActive(false);
                    this.CreateTrenchIcon(obj3, _config.monster_picture);
                    transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, 88.92f, 0f);
                }
                else
                {
                    obj3.SetActive(false);
                    gameObject.SetActive(true);
                    this.CreateTrenchIcon(gameObject, _config.monster_picture);
                }
                break;
            }
            case DuplicateType.DupType_Elite:
            {
                trench_elite_config _config2 = (trench_elite_config) _data;
                if (_config2.entry == LastEntry)
                {
                    obj3.SetActive(true);
                    gameObject.SetActive(false);
                    this.CreateTrenchIcon(obj3, _config2.monster_picture);
                    transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, 88.92f, 0f);
                }
                else
                {
                    obj3.SetActive(false);
                    gameObject.SetActive(true);
                    this.CreateTrenchIcon(gameObject, _config2.monster_picture);
                }
                break;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <CallBackPrePanel>c__AnonStorey1BF
    {
        private static Action<GUIEntity> <>f__am$cache2;
        private static Action<GUIEntity> <>f__am$cache3;
        private static Action<GUIEntity> <>f__am$cache4;
        private static Action<GUIEntity> <>f__am$cache5;
        private static Action<GUIEntity> <>f__am$cache6;
        private static Action<GUIEntity> <>f__am$cache7;
        private static System.Action <>f__am$cache8;
        private static System.Action <>f__am$cache9;
        private static Action<GUIEntity> <>f__am$cacheA;
        private static System.Action <>f__am$cacheB;
        internal System.Action action_finished;
        internal bool pre_laod;

        internal void <>m__2A6()
        {
            if (this.action_finished != null)
            {
                this.action_finished();
            }
            this.pre_laod = true;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (GUIEntity entity) {
                    if (<>f__am$cache5 == null)
                    {
                        <>f__am$cache5 = delegate (GUIEntity obj) {
                            ((DetailsPanel) obj).InitList(ActorData.getInstance().mCurrDupReturnPrePara.heroPanelPartEntry);
                            if (<>f__am$cache8 == null)
                            {
                                <>f__am$cache8 = () => LoadingPerfab.EndTransition();
                            }
                            ScheduleMgr.Schedule(0.3f, <>f__am$cache8);
                        };
                    }
                    GUIMgr.Instance.PushGUIEntity("DetailsPanel", <>f__am$cache5);
                };
            }
            GUIMgr.Instance.PushGUIEntity("HeroPanel", <>f__am$cache2);
        }

        internal void <>m__2A7()
        {
            if (this.action_finished != null)
            {
                this.action_finished();
            }
            this.pre_laod = true;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (GUIEntity entity) {
                    if (<>f__am$cache6 == null)
                    {
                        <>f__am$cache6 = delegate (GUIEntity obj) {
                            HeroInfoPanel panel = (HeroInfoPanel) obj;
                            if (ActorData.getInstance().mCurrDupReturnPrePara.mIsCardPartInfo)
                            {
                                int entry = (int) ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo.cardInfo.entry;
                                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(entry);
                                if (_config != null)
                                {
                                    card_ex_config cardExCfg = CommonFunc.GetCardExCfg(entry, _config.evolve_lv);
                                    if (cardExCfg != null)
                                    {
                                        panel.ShowCardPartInfo(cardExCfg);
                                    }
                                }
                            }
                            else
                            {
                                SocketMgr.Instance.RequestGetSkillPoint();
                                panel.InitCardInfo(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo);
                                panel.SetCurrShowCardList(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoShowCardList);
                            }
                            if (<>f__am$cache9 == null)
                            {
                                <>f__am$cache9 = () => LoadingPerfab.EndTransition();
                            }
                            ScheduleMgr.Schedule(0.3f, <>f__am$cache9);
                        };
                    }
                    GUIMgr.Instance.PushGUIEntity("HeroInfoPanel", <>f__am$cache6);
                };
            }
            GUIMgr.Instance.PushGUIEntity("HeroPanel", <>f__am$cache3);
        }

        internal void <>m__2A8()
        {
            if (this.action_finished != null)
            {
                this.action_finished();
            }
            this.pre_laod = true;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate (GUIEntity entity) {
                    HeroPanel panel = (HeroPanel) entity;
                    if (<>f__am$cache7 == null)
                    {
                        <>f__am$cache7 = delegate (GUIEntity obj) {
                            SocketMgr.Instance.RequestGetSkillPoint();
                            HeroInfoPanel panel = (HeroInfoPanel) obj;
                            panel.InitCardInfo(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo);
                            panel.SetCurrShowCardList(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoShowCardList);
                            if (<>f__am$cacheA == null)
                            {
                                <>f__am$cacheA = delegate (GUIEntity beObj) {
                                    ((BreakEquipPanel) beObj).ResetDupJumpStat(ActorData.getInstance().mCurrDupReturnPrePara);
                                    if (<>f__am$cacheB == null)
                                    {
                                        <>f__am$cacheB = () => LoadingPerfab.EndTransition();
                                    }
                                    ScheduleMgr.Schedule(0.3f, <>f__am$cacheB);
                                };
                            }
                            GUIMgr.Instance.PushGUIEntity("BreakEquipPanel", <>f__am$cacheA);
                        };
                    }
                    GUIMgr.Instance.PushGUIEntity("HeroInfoPanel", <>f__am$cache7);
                };
            }
            GUIMgr.Instance.PushGUIEntity("HeroPanel", <>f__am$cache4);
        }

        private static void <>m__2B8(GUIEntity entity)
        {
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = delegate (GUIEntity obj) {
                    ((DetailsPanel) obj).InitList(ActorData.getInstance().mCurrDupReturnPrePara.heroPanelPartEntry);
                    if (<>f__am$cache8 == null)
                    {
                        <>f__am$cache8 = () => LoadingPerfab.EndTransition();
                    }
                    ScheduleMgr.Schedule(0.3f, <>f__am$cache8);
                };
            }
            GUIMgr.Instance.PushGUIEntity("DetailsPanel", <>f__am$cache5);
        }

        private static void <>m__2B9(GUIEntity entity)
        {
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = delegate (GUIEntity obj) {
                    HeroInfoPanel panel = (HeroInfoPanel) obj;
                    if (ActorData.getInstance().mCurrDupReturnPrePara.mIsCardPartInfo)
                    {
                        int id = (int) ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo.cardInfo.entry;
                        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(id);
                        if (_config != null)
                        {
                            card_ex_config cardExCfg = CommonFunc.GetCardExCfg(id, _config.evolve_lv);
                            if (cardExCfg != null)
                            {
                                panel.ShowCardPartInfo(cardExCfg);
                            }
                        }
                    }
                    else
                    {
                        SocketMgr.Instance.RequestGetSkillPoint();
                        panel.InitCardInfo(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo);
                        panel.SetCurrShowCardList(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoShowCardList);
                    }
                    if (<>f__am$cache9 == null)
                    {
                        <>f__am$cache9 = () => LoadingPerfab.EndTransition();
                    }
                    ScheduleMgr.Schedule(0.3f, <>f__am$cache9);
                };
            }
            GUIMgr.Instance.PushGUIEntity("HeroInfoPanel", <>f__am$cache6);
        }

        private static void <>m__2BA(GUIEntity entity)
        {
            HeroPanel panel = (HeroPanel) entity;
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = delegate (GUIEntity obj) {
                    SocketMgr.Instance.RequestGetSkillPoint();
                    HeroInfoPanel panel = (HeroInfoPanel) obj;
                    panel.InitCardInfo(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo);
                    panel.SetCurrShowCardList(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoShowCardList);
                    if (<>f__am$cacheA == null)
                    {
                        <>f__am$cacheA = delegate (GUIEntity beObj) {
                            ((BreakEquipPanel) beObj).ResetDupJumpStat(ActorData.getInstance().mCurrDupReturnPrePara);
                            if (<>f__am$cacheB == null)
                            {
                                <>f__am$cacheB = () => LoadingPerfab.EndTransition();
                            }
                            ScheduleMgr.Schedule(0.3f, <>f__am$cacheB);
                        };
                    }
                    GUIMgr.Instance.PushGUIEntity("BreakEquipPanel", <>f__am$cacheA);
                };
            }
            GUIMgr.Instance.PushGUIEntity("HeroInfoPanel", <>f__am$cache7);
        }

        private static void <>m__2BB(GUIEntity obj)
        {
            ((DetailsPanel) obj).InitList(ActorData.getInstance().mCurrDupReturnPrePara.heroPanelPartEntry);
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = () => LoadingPerfab.EndTransition();
            }
            ScheduleMgr.Schedule(0.3f, <>f__am$cache8);
        }

        private static void <>m__2BC(GUIEntity obj)
        {
            HeroInfoPanel panel = (HeroInfoPanel) obj;
            if (ActorData.getInstance().mCurrDupReturnPrePara.mIsCardPartInfo)
            {
                int entry = (int) ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo.cardInfo.entry;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(entry);
                if (_config != null)
                {
                    card_ex_config cardExCfg = CommonFunc.GetCardExCfg(entry, _config.evolve_lv);
                    if (cardExCfg != null)
                    {
                        panel.ShowCardPartInfo(cardExCfg);
                    }
                }
            }
            else
            {
                SocketMgr.Instance.RequestGetSkillPoint();
                panel.InitCardInfo(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo);
                panel.SetCurrShowCardList(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoShowCardList);
            }
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = () => LoadingPerfab.EndTransition();
            }
            ScheduleMgr.Schedule(0.3f, <>f__am$cache9);
        }

        private static void <>m__2BD(GUIEntity obj)
        {
            SocketMgr.Instance.RequestGetSkillPoint();
            HeroInfoPanel panel = (HeroInfoPanel) obj;
            panel.InitCardInfo(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo);
            panel.SetCurrShowCardList(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoShowCardList);
            if (<>f__am$cacheA == null)
            {
                <>f__am$cacheA = delegate (GUIEntity beObj) {
                    ((BreakEquipPanel) beObj).ResetDupJumpStat(ActorData.getInstance().mCurrDupReturnPrePara);
                    if (<>f__am$cacheB == null)
                    {
                        <>f__am$cacheB = () => LoadingPerfab.EndTransition();
                    }
                    ScheduleMgr.Schedule(0.3f, <>f__am$cacheB);
                };
            }
            GUIMgr.Instance.PushGUIEntity("BreakEquipPanel", <>f__am$cacheA);
        }

        private static void <>m__2BE()
        {
            LoadingPerfab.EndTransition();
        }

        private static void <>m__2BF()
        {
            LoadingPerfab.EndTransition();
        }

        private static void <>m__2C0(GUIEntity beObj)
        {
            ((BreakEquipPanel) beObj).ResetDupJumpStat(ActorData.getInstance().mCurrDupReturnPrePara);
            if (<>f__am$cacheB == null)
            {
                <>f__am$cacheB = () => LoadingPerfab.EndTransition();
            }
            ScheduleMgr.Schedule(0.3f, <>f__am$cacheB);
        }

        private static void <>m__2C1()
        {
            LoadingPerfab.EndTransition();
        }
    }

    [CompilerGenerated]
    private sealed class <CreateDupData>c__Iterator70 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool _OnlyShow;
        internal bool <$>_OnlyShow;
        internal duplicate_config <$>cfg;
        internal GameObject <$>obj;
        internal DupMap <>f__this;
        internal UISprite <guildRoot>__4;
        internal UILabel <lvLock>__5;
        internal UISprite <MapChapter>__3;
        internal UISprite <MapFont>__2;
        internal Transform <MapImage>__1;
        internal GameObject <newObj>__0;
        internal duplicate_config cfg;
        internal GameObject obj;

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
                {
                    this.<newObj>__0 = UnityEngine.Object.Instantiate(this.<>f__this.DupItem) as GameObject;
                    this.<newObj>__0.transform.parent = this.obj.transform;
                    this.<newObj>__0.transform.localScale = Vector3.one;
                    this.<newObj>__0.transform.localPosition = Vector3.zero;
                    this.<>f__this.DupObjBuffer.Add(this.<newObj>__0);
                    this.<MapImage>__1 = this.obj.transform.FindChild("Texture");
                    if (this.<MapImage>__1 != null)
                    {
                        this.<MapImage>__1.gameObject.SetActive(true);
                    }
                    this.<newObj>__0.transform.FindChild("Label").GetComponent<UILabel>().text = this.cfg.name;
                    if (this.cfg.entry < this.<>f__this.DupFontList.Length)
                    {
                        this.<MapFont>__2 = this.<newObj>__0.transform.FindChild("Sprite").GetComponent<UISprite>();
                        this.<MapFont>__2.spriteName = this.<>f__this.DupFontList[this.cfg.entry];
                        this.<MapFont>__2.MakePixelPerfect();
                        this.<MapChapter>__3 = this.<newObj>__0.transform.FindChild("Chapter").GetComponent<UISprite>();
                        this.<MapChapter>__3.spriteName = this.<>f__this.DupChapterList[this.cfg.entry];
                        this.<MapChapter>__3.MakePixelPerfect();
                    }
                    else
                    {
                        Debug.LogWarning("Not Find Font ,Entry is " + this.cfg.entry);
                    }
                    this.<guildRoot>__4 = this.<newObj>__0.transform.FindChild<UISprite>("GuildSprite");
                    if (this.<guildRoot>__4 != null)
                    {
                        this.<guildRoot>__4.ActiveSelfObject(false);
                    }
                    this.<lvLock>__5 = this.<newObj>__0.transform.FindChild("Tips").GetComponent<UILabel>();
                    if (ActorData.getInstance().Level < this.cfg.unlock_lv)
                    {
                        GUIDataHolder.setData(this.<newObj>__0, this.cfg);
                        UIEventListener listener2 = UIEventListener.Get(this.<newObj>__0);
                        listener2.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener2.onClick, new UIEventListener.VoidDelegate(this.<>f__this.OnClickDup));
                        this.<lvLock>__5.text = string.Format(ConfigMgr.getInstance().GetWord(0x9ba3c4), this.cfg.unlock_lv);
                        break;
                    }
                    if (!this._OnlyShow)
                    {
                        GUIDataHolder.setData(this.<newObj>__0, this.cfg);
                        this.<lvLock>__5.text = string.Empty;
                    }
                    else
                    {
                        this.<lvLock>__5.text = ConfigMgr.getInstance().GetWord(0x83f);
                    }
                    UIEventListener listener1 = UIEventListener.Get(this.<newObj>__0);
                    listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.<>f__this.OnClickDup));
                    break;
                }
                case 1:
                    this.$PC = -1;
                    goto Label_0357;

                default:
                    goto Label_0357;
            }
            this.$current = null;
            this.$PC = 1;
            return true;
        Label_0357:
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

    [CompilerGenerated]
    private sealed class <LookDuplicateReward>c__AnonStorey1C4
    {
        internal int entry;
        internal int mCurDupType;

        internal bool <>m__2B3(duplicate_star_reward_config d)
        {
            return ((d.duplicate_type == this.mCurDupType) && (d.duplicate_entry == this.entry));
        }
    }

    [CompilerGenerated]
    private sealed class <LookDuplicateReward>c__AnonStorey1C5
    {
        internal Item tempItem1;
        internal Item tempItem2;
        internal Item tempItem3;

        internal bool <>m__2B5(Item i)
        {
            return (i.entry == this.tempItem1.entry);
        }

        internal bool <>m__2B6(Item i)
        {
            return (i.entry == this.tempItem2.entry);
        }

        internal bool <>m__2B7(Item i)
        {
            return (i.entry == this.tempItem3.entry);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickEliteTrench>c__AnonStorey1C3
    {
        internal trench_elite_config cfg;

        internal void <>m__2B1(GUIEntity guiE)
        {
            DupLevInfoPanel panel = guiE.Achieve<DupLevInfoPanel>();
            panel.mIsDupEnter = true;
            panel.UpdateEliteMapInfo(this.cfg);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickNormalTrench>c__AnonStorey1C2
    {
        private static Action<GUIEntity> <>f__am$cache1;
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        internal trench_normal_config cfg;

        internal void <>m__2B0(GUIEntity guiE)
        {
            DupLevInfoPanel panel = guiE.Achieve<DupLevInfoPanel>();
            panel.mIsDupEnter = true;
            panel.OpenTypeIsPush = true;
            panel.UpdateNormalMapInfo(this.cfg);
            if (ActorData.getInstance().UserInfo.zero_profile_time > TimeMgr.Instance.ServerStampTime)
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate (GUIEntity e) {
                        MessageBox box = (MessageBox) e;
                        if (<>f__am$cache2 == null)
                        {
                            <>f__am$cache2 = delegate (GameObject go) {
                            };
                        }
                        box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x585), ActorData.getInstance().UserInfo.zero_profile_reason, TimeMgr.Instance.GetTimeStr(ActorData.getInstance().UserInfo.zero_profile_len), TimeMgr.Instance.GetFrozenTime(ActorData.getInstance().UserInfo.zero_profile_time)), <>f__am$cache2, null, true);
                    };
                }
                GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache1, null);
            }
        }

        private static void <>m__2C2(GUIEntity e)
        {
            MessageBox box = (MessageBox) e;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (GameObject go) {
                };
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x585), ActorData.getInstance().UserInfo.zero_profile_reason, TimeMgr.Instance.GetTimeStr(ActorData.getInstance().UserInfo.zero_profile_len), TimeMgr.Instance.GetFrozenTime(ActorData.getInstance().UserInfo.zero_profile_time)), <>f__am$cache2, null, true);
        }

        private static void <>m__2C3(GameObject go)
        {
        }
    }

    [CompilerGenerated]
    private sealed class <OnGuildMap>c__AnonStorey1C1
    {
        internal GuildDupStatusInfo dupState;

        internal void <>m__2AF(GUIEntity u)
        {
            (u as GuildDupTrenchMap).ShowTrenchMap(this.dupState);
        }
    }

    [CompilerGenerated]
    private sealed class <PlayDupAnim>c__Iterator6F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal DupMap <>f__this;
        internal UITexture <tex>__0;

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
                    if (this.<>f__this.OpenNewDupObj != null)
                    {
                        this.<tex>__0 = this.<>f__this.OpenNewDupObj.transform.FindChild("Texture").GetComponent<UITexture>();
                        this.<tex>__0.alpha = 0.1f;
                        this.<tex>__0.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        this.$current = new WaitForSeconds(1.3f);
                        this.$PC = 1;
                        return true;
                    }
                    break;

                case 1:
                    this.<>f__this.OpenNewDupObj.SetActive(true);
                    TweenAlpha.Begin(this.<tex>__0.gameObject, 0.5f, 1f);
                    TweenScale.Begin(this.<tex>__0.gameObject, 0.5f, Vector3.one);
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

    [CompilerGenerated]
    private sealed class <PopFriendShip>c__AnonStorey1C0
    {
        internal bool have;

        internal bool <>m__2AA(SocialUser t)
        {
            if (t.QQUser.userInfo.id == ActorData.getInstance().mFriendReward.userInfo.id)
            {
                this.have = true;
                return true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <PopMapAnim>c__Iterator71 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal DupMap <>f__this;
        internal GameObject <obj>__0;

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
                    this.<obj>__0 = this.<>f__this.TrenchPart.transform.FindChild("PopMap").gameObject;
                    this.<obj>__0.SetActive(true);
                    TweenScale.Begin(this.<obj>__0, 0.3f, Vector3.one);
                    this.$current = null;
                    this.$PC = 1;
                    return true;

                case 1:
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

    private enum DupPassState
    {
        Pass,
        UnPass,
        Passing
    }

    private class RewardItem
    {
        public object Data;
        public ItemType type;
    }
}

