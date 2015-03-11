using FastBuf;
using fastJSON;
using HutongGames.PlayMaker.Actions;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class MainUI : GUIEntity
{
    private List<Vector3> _Building = new List<Vector3>();
    public GameObject _BuildingGroup;
    public GameObject[] _BuildingIcon;
    private int _chatClickNum;
    public UISprite _ChatIconSprite;
    public GameObject _ChatOperateGuildMsgTips;
    public GameObject _ChatOperateTips;
    private Transform _ChatPanel;
    private Transform _CommonChatGroup;
    private Transform _GuildChatGroup;
    private bool _isPlay;
    public GameObject _SingleChatItem;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache49;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4B;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4C;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4D;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4E;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4F;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache50;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache51;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache52;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache53;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache54;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache55;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache56;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache57;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache58;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache59;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$mapD;
    public GameObject ActiveTipsObj;
    public System.Action actSelectBuilding;
    private bool b_HaveNewActive__;
    public GameObject BoxTipsObj;
    private string[] buildingStr = new string[] { "Ui_Main_youxiang", "Ui_Main_bfwz", "Ui_Main_dixiacheng", "Ui_Main_jingjichang", "Ui_Main_jiuguan", "Ui_Main_shilianta", "Ui_Main_wuqidian", "Ui_Main_qianghua", "Ui_Main_fuben", "Ui_Main_gonghui", "Ui_Main_shjn", "Ui_Main_tree", "Ui_Main_zghs" };
    private bool cancelClickModel;
    private GameObject effectObj;
    public GameObject goActive;
    private GuildMsgState guildMsgState;
    private bool isSendGuildMsg;
    public UILabel labelAd;
    private float lastUpdateTime;
    public GameObject layerLockObject;
    public bool lockSwippy;
    private int m_currChatidx;
    private float mActual;
    private float mAdMsgLoopInterval = 10f;
    private Transform mArenaNewTips;
    private int MaxBuildingCount = 13;
    private Vector2 maxCameraInnerSize = new Vector2(10f, -22f);
    private Vector2 maxCameraOuterSize = new Vector2(11f, -24f);
    public UILabel mBoradTextlbl;
    public UISprite mBroadMsgBg;
    private float mBroadMsgLoopInterval = 10f;
    private bool mCardDrag;
    private List<GameObject> mChatItemList = new List<GameObject>();
    private int mCurrCardEntry;
    private long mCurrGuildId = -1L;
    private long mCurrPlayerId = -1L;
    private GameObject mCurrTouchGo;
    private bool mDargChatBox;
    private Transform mDetainsDartNewTips;
    private List<GameObject> mGuildMsgItemList = new List<GameObject>();
    private bool mInitEnd;
    private int mInitStart = (TimeMgr.Instance.ServerStampTime + 1);
    private bool mIsLeftMove;
    private bool mIsPopChatPanel = true;
    private bool mIsSendingAdMsg = true;
    private bool mIsSendingBroadMsg = true;
    private Transform mLifeSkillNewTips;
    private Transform mMailNewTips;
    private Vector2 mMomentum = Vector2.zero;
    private float momentumAmount = 35f;
    private bool mPressed;
    private Transform mPubNewTips;
    private GameObject mRole3DRole;
    private float mRt;
    private Transform mShopNewTips;
    private int mShowGmBtnTime;
    private Vector2 mStartPos;
    private int mStartShowTextTime;
    private float mTimeDelta;
    private float mTimeStart;
    private const int mXiaLaBaMinUpdateTime = 120;
    public UIButton RankButton;
    private IEnumerator refresher;
    private Vector2 scale = new Vector2(1.8f, 0f);
    private GameObject SignInTipsObj;
    public UISprite spriteADBg;
    private Vector2 startPos;
    private List<GuildMsgItem> tempMsgItems = new List<GuildMsgItem>();
    public GameObject TileTipsObj;

    [DebuggerHidden]
    private IEnumerator ADAuto(string msg)
    {
        return new <ADAuto>c__Iterator92 { msg = msg, <$>msg = msg, <>f__this = this };
    }

    private void AddChatMsg()
    {
        Transform transform = this._ChatPanel.FindChild("Group/List");
        CommonFunc.DeleteChildItem(transform);
        int num = 0;
        List<GameObject> list = new List<GameObject>();
        for (int i = ActorData.getInstance().mChatList.Count - 1; i >= 0; i--)
        {
            GameObject item = UnityEngine.Object.Instantiate(this._SingleChatItem) as GameObject;
            item.transform.parent = transform;
            item.transform.localPosition = new Vector3(0f, (float) num, -0.1f);
            item.transform.localScale = new Vector3(1f, 1f, 1f);
            UILabel component = item.transform.FindChild("Item/Label").GetComponent<UILabel>();
            UISprite sprite = item.transform.FindChild("Item/Border").GetComponent<UISprite>();
            FastBuf.Broadcast data = ActorData.getInstance().mChatList[i];
            string[] textArray1 = new string[] { "[0069c7]", data.userName, ":", GameConstant.DefaultTextColor, data.content };
            component.text = string.Concat(textArray1);
            sprite.height = component.height + 30;
            num -= sprite.height;
            if (data.userId > 0L)
            {
                Transform transform2 = item.transform.FindChild("Item");
                GUIDataHolder.setData(transform2.gameObject, data);
                UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickChatItemBtn);
                UIEventListener.Get(transform2.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressBox);
            }
            list.Add(item);
        }
        for (int j = list.Count - 1; j >= 0; j--)
        {
            this.mChatItemList.Add(list[j]);
        }
        this.mIsSendingBroadMsg = false;
        this.mIsSendingAdMsg = false;
    }

    [DebuggerHidden]
    public IEnumerator AddGuildMsgForTimes()
    {
        return new <AddGuildMsgForTimes>c__Iterator93 { <>f__this = this };
    }

    private void AdPanel()
    {
        if (!XSingleton<AdManager>.Singleton._isAd)
        {
            if (<>f__am$cache49 == null)
            {
                <>f__am$cache49 = delegate (GUIEntity obj) {
                    AdPicutrePanel panel = (AdPicutrePanel) obj;
                    panel.Depth = 0x41a;
                    panel.SetPicture();
                    XSingleton<AdManager>.Singleton._isAd = true;
                };
            }
            GUIMgr.Instance.DoModelGUI("AdPicutrePanel", <>f__am$cache49, null);
        }
    }

    public void BeginListen()
    {
        if (this.refresher == null)
        {
            this.refresher = this.AddGuildMsgForTimes();
        }
        else
        {
            base.StopCoroutine(this.refresher);
            this.refresher = this.AddGuildMsgForTimes();
        }
        base.StartCoroutine(this.refresher);
    }

    [DebuggerHidden]
    private IEnumerator broadMsgProcess(string msg)
    {
        return new <broadMsgProcess>c__Iterator91 { msg = msg, <$>msg = msg, <>f__this = this };
    }

    private void ChangeModelMaterial(GameObject obj, string shaderName)
    {
        MaterialFSM component = obj.GetComponent<MaterialFSM>();
        if (component == null)
        {
            component = obj.AddComponent<MaterialFSM>();
        }
        component.SetMaterial(shaderName);
    }

    public void CheckArenaNewTips()
    {
        if (this.mArenaNewTips != null)
        {
            this.mArenaNewTips.gameObject.SetActive(ActorData.getInstance().HaveNewArenaLog);
        }
    }

    public void CheckDetainsDart()
    {
        if (this.mDetainsDartNewTips != null)
        {
            this.mDetainsDartNewTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
        }
    }

    private void CheckEffect()
    {
        if (this.effectObj != null)
        {
            if (!SettingMgr.mInstance.IsEffectEnable)
            {
                this.effectObj.SetActive(false);
            }
            else
            {
                this.effectObj.SetActive(true);
            }
        }
    }

    public void CheckFreeRecruit()
    {
        if (this.mPubNewTips != null)
        {
            this.mPubNewTips.gameObject.SetActive(GameDataMgr.Instance.FreeRecruitFlag);
        }
    }

    private void CheckGuildNotice()
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<guildbattle_time_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                guildbattle_time_config current = (guildbattle_time_config) enumerator.Current;
                int hour = TimeMgr.Instance.ServerDateTime.Hour;
                int minute = TimeMgr.Instance.ServerDateTime.Minute;
                int second = TimeMgr.Instance.ServerDateTime.Second;
                int num4 = 0;
                if (TimeMgr.Instance.ServerDateTime.DayOfWeek == current.open_weekday)
                {
                    int guildBattleNoticeInt = SettingMgr.mInstance.GetGuildBattleNoticeInt();
                    if (((hour == 0x11) && (minute >= 30)) && (hour < 0x12))
                    {
                        DateTime time = new DateTime(TimeMgr.Instance.ServerDateTime.Year, TimeMgr.Instance.ServerDateTime.Month, TimeMgr.Instance.ServerDateTime.Day, 0x11, 30, 0);
                        if (guildBattleNoticeInt < TimeMgr.Instance.ConvertToTimeStamp(time))
                        {
                            num4 = 0x4e47;
                            SettingMgr.mInstance.SetGuildBattleNoticeInt(TimeMgr.Instance.ServerStampTime);
                        }
                    }
                    else if (((hour == 0x12) && (minute >= 0)) && ((hour == 0x12) && (minute <= 1)))
                    {
                        DateTime time2 = new DateTime(TimeMgr.Instance.ServerDateTime.Year, TimeMgr.Instance.ServerDateTime.Month, TimeMgr.Instance.ServerDateTime.Day, 0x12, 0, 0);
                        if (guildBattleNoticeInt < TimeMgr.Instance.ConvertToTimeStamp(time2))
                        {
                            num4 = 0x4e48;
                            SettingMgr.mInstance.SetGuildBattleNoticeInt(TimeMgr.Instance.ServerStampTime);
                        }
                    }
                    else if (((hour == 0x16) && (minute >= 30)) && (hour < 0x17))
                    {
                        DateTime time3 = new DateTime(TimeMgr.Instance.ServerDateTime.Year, TimeMgr.Instance.ServerDateTime.Month, TimeMgr.Instance.ServerDateTime.Day, 0x16, 30, 0);
                        if (guildBattleNoticeInt < TimeMgr.Instance.ConvertToTimeStamp(time3))
                        {
                            num4 = 0x4e49;
                            SettingMgr.mInstance.SetGuildBattleNoticeInt(TimeMgr.Instance.ServerStampTime);
                        }
                    }
                    else if (((hour == 0x17) && (minute >= 0)) && ((hour == 0x17) && (minute <= 1)))
                    {
                        DateTime time4 = new DateTime(TimeMgr.Instance.ServerDateTime.Year, TimeMgr.Instance.ServerDateTime.Month, TimeMgr.Instance.ServerDateTime.Day, 0x17, 0, 0);
                        if (guildBattleNoticeInt < TimeMgr.Instance.ConvertToTimeStamp(time4))
                        {
                            num4 = 0x4e4a;
                            SettingMgr.mInstance.SetGuildBattleNoticeInt(TimeMgr.Instance.ServerStampTime);
                        }
                    }
                    if (num4 != 0)
                    {
                        FastBuf.Broadcast bc = new FastBuf.Broadcast {
                            userId = -1L,
                            userName = "[ff0000]" + ConfigMgr.getInstance().GetWord(0x4e4b) + "[ffffff]",
                            content = ConfigMgr.getInstance().GetWord(num4)
                        };
                        ActorData.getInstance().AddBroadCast(bc);
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
    }

    public void CheckLifeSkill()
    {
        if (this.mLifeSkillNewTips != null)
        {
            this.mLifeSkillNewTips.gameObject.SetActive(XSingleton<LifeSkillManager>.Singleton.HaveCollect);
        }
    }

    public void CheckNewMail()
    {
        if (this.mMailNewTips != null)
        {
            this.mMailNewTips.gameObject.SetActive(ActorData.getInstance().HaveNewMail());
        }
    }

    public void CheckOpenLevelText()
    {
        for (int i = 0; i < this.MaxBuildingCount; i++)
        {
            UITexture component = this._BuildingIcon[i].transform.GetComponent<UITexture>();
            component.mainTexture = BundleMgr.Instance.CreateBuildingText(this.buildingStr[i]);
            nguiTextureGrey.doChangeEnableGrey(component, this.FunIsOpen(i));
            if (i == 11)
            {
                component.enabled = SoulBox.FuncShowable();
            }
        }
    }

    public void CheckShopNewStat()
    {
        if (this.mShopNewTips != null)
        {
            this.mShopNewTips.gameObject.SetActive(ActorData.getInstance().CheckShopTipsStatBool);
        }
    }

    private bool checkTouchEnable()
    {
        return (null == this.HoverdObject);
    }

    private void CheckUpdate()
    {
        this.CheckEffect();
        if ((this.mShowGmBtnTime != 0) && (TimeMgr.Instance.ServerStampTime > this.mShowGmBtnTime))
        {
            this.mShowGmBtnTime = 0;
        }
        if (!this.mInitEnd)
        {
            this.mInitEnd = TimeMgr.Instance.ServerStampTime >= this.mInitStart;
        }
        else
        {
            if (ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().sign_in)
            {
                if (ActorData.getInstance().RegistrationReward >= 1)
                {
                    this.SignInTipsObj.SetActive(false);
                }
                else
                {
                    this.SignInTipsObj.SetActive(true);
                }
            }
            if (GameDataMgr.Instance.ActiveIsNew || GameDataMgr.Instance.ActiveStage)
            {
                if (this.goActive != null)
                {
                    if (this.goActive.activeSelf)
                    {
                        if (this.ActiveTipsObj != null)
                        {
                            this.ActiveTipsObj.SetActive(true);
                        }
                    }
                    else if (this.ActiveTipsObj != null)
                    {
                        this.ActiveTipsObj.SetActive(false);
                    }
                }
            }
            else if (this.ActiveTipsObj != null)
            {
                this.ActiveTipsObj.SetActive(false);
            }
            if ((Camera.main != null) && Camera.main.active)
            {
                for (int i = 0; i < this.MaxBuildingCount; i++)
                {
                    Vector3 vector = Camera.main.WorldToViewportPoint(this._Building[i]);
                    if (Camera.main != null)
                    {
                        int activeHeight = UIRoot.list[0].activeHeight;
                        vector.x = ((vector.x - 0.5f) * (((float) Screen.width) / ((float) Screen.height))) * activeHeight;
                        vector.y = (vector.y - 0.5f) * activeHeight;
                        vector.z = 0f;
                        this._BuildingIcon[i].transform.localPosition = vector;
                        if (!this._BuildingIcon[i].activeSelf)
                        {
                            this._BuildingIcon[i].gameObject.SetActive(true);
                        }
                    }
                }
            }
            int num3 = (ActorData.getInstance().nBoxGoldKeys + ActorData.getInstance().nBoxSliverKeys) + ActorData.getInstance().nBoxCopperKeys;
            this.BoxTipsObj.SetActive(num3 > 0);
            this.TileTipsObj.SetActive(ActorData.getInstance().mHaveNewTitle);
        }
    }

    public void ClearAll3DRole()
    {
        if (this.mRole3DRole != null)
        {
            UnityEngine.Object.DestroyObject(this.mRole3DRole);
        }
    }

    private void ClickArena()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Arena);
        SoundManager.mInstance.PlaySFX("sound_build_z_3");
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().arena_ladder)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().arena_ladder));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_ArenaLadder))
        {
            GUIMgr.Instance.PushGUIEntity<ArenaPortal>(null);
        }
    }

    private void ClickDiaoXiang()
    {
        if (XSingleton<GameDetainsDartMgr>.Singleton.isTest)
        {
            if (<>f__am$cache59 == null)
            {
                <>f__am$cache59 = ui => (ui as DetainsDartMainUIPanel).SetDataInfo(false, null);
            }
            GUIMgr.Instance.PushGUIEntity<DetainsDartMainUIPanel>(<>f__am$cache59);
        }
        else if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().convoy_lv)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x186d8), CommonFunc.LevelLimitCfg().convoy_lv));
        }
        else
        {
            XSingleton<GameDetainsDartMgr>.Singleton.ReqDetaisDartInfo();
        }
    }

    public void ClickDungeons()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Dungeons);
        SoundManager.mInstance.PlaySFX("sound_build_z_8");
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().dungeons)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().dungeons));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_Dungeons))
        {
            if (<>f__am$cache52 == null)
            {
                <>f__am$cache52 = delegate (GUIEntity obj) {
                };
            }
            GUIMgr.Instance.PushGUIEntity("DungeonsPanel", <>f__am$cache52);
        }
    }

    private void ClickDup()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Duplicate) || GuideSystem.MatchEvent(GuideEvent.Duplicate_Elite))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_battle_select_building, null);
        }
        SoundManager.mInstance.PlaySFX("sound_build_z_2");
        if (<>f__am$cache55 == null)
        {
            <>f__am$cache55 = obj => ActorData.getInstance().mCurrDupReturnPrePara = null;
        }
        GUIMgr.Instance.PushGUIEntity("DupMap", <>f__am$cache55);
    }

    private void ClickEquipLevUp()
    {
        SoundManager.mInstance.PlaySFX("sound_build_z_9");
        TipsDiag.SetText("此功能已暂时屏蔽!");
    }

    private void ClickGuild()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Guild);
        SoundManager.mInstance.PlaySFX("sound_build_z_10");
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().guild)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().guild));
        }
        else
        {
            SocketMgr.Instance.RequestGuildData(true, null);
        }
    }

    private void ClickLinghunshu()
    {
        this._isPlay = true;
        SoundManager.mInstance.PlaySFX("soultree");
        ObjectManager.CreateTempObj("EffectPrefabs/boom_star", GameObject.Find("GameObject/cj_xzjm2/linghunshu/shu").transform.position, 3f);
        GameObject obj2 = GameObject.Find("GameObject/cj_xzjm2/linghunshu");
        if (obj2 != null)
        {
            <ClickLinghunshu>c__AnonStorey21B storeyb = new <ClickLinghunshu>c__AnonStorey21B {
                <>f__this = this,
                anim = obj2.transform.GetComponent<Animation>()
            };
            AnimationClip clip = storeyb.anim.GetClip("dianji");
            if (null != clip)
            {
                storeyb.anim.Play(clip.name);
                GUIMgr.Instance.Lock();
            }
            this.DelayCallBack(2f, new System.Action(storeyb.<>m__44C));
        }
    }

    private void ClickMail()
    {
        SoundManager.mInstance.PlaySFX("sound_build_z_4");
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().mail)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().mail));
        }
        else
        {
            GUIMgr.Instance.PushGUIEntity<MailListPanel>(null);
        }
    }

    public void ClickOutland()
    {
        <ClickOutland>c__AnonStorey21A storeya = new <ClickOutland>c__AnonStorey21A {
            <>f__this = this
        };
        this.FunctionBuildingResponse(GuideEvent.Function_Outland);
        if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_Outland))
        {
            this._isPlay = true;
            GameObject obj2 = GameObject.Find("GameObject/cj_xzjm2/waiyu");
            storeya.anim = obj2.transform.GetComponent<Animation>();
            storeya.freeClip = storeya.anim.GetClip("hua_free");
            if (null != storeya.freeClip)
            {
                GUIMgr.Instance.Lock();
                storeya.anim.Play(storeya.freeClip.name, PlayMode.StopSameLayer);
            }
            this.DelayCallBack(1.2f, new System.Action(storeya.<>m__447));
        }
    }

    private void ClickSignIn()
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().sign_in)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().sign_in));
        }
        else
        {
            if (<>f__am$cache56 == null)
            {
                <>f__am$cache56 = obj => SignInPanel panel = (SignInPanel) obj;
            }
            GUIMgr.Instance.PushGUIEntity("SignInPanel", <>f__am$cache56);
        }
    }

    private void ClickTiJiangPu()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Shop);
        SoundManager.mInstance.PlaySFX("sound_build_z_7");
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().shop)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().shop));
        }
        else
        {
            GUIMgr.Instance.PushGUIEntity<ShopTabEntity>(null);
        }
    }

    private void ClickTower()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Tower);
        SoundManager.mInstance.PlaySFX("sound_build_z_11");
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().void_tower)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().void_tower));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_VoidTower))
        {
            GUIMgr.Instance.PushGUIEntity<TowerPanel>(null);
        }
    }

    public void ClickWorldBoss()
    {
        SoundManager.mInstance.PlaySFX("sound_build_z_1");
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().world_boss)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().world_boss));
        }
        else
        {
            SocketMgr.Instance.RequestWorldBoss(0);
        }
    }

    private void ClickWorldCup()
    {
        SoundManager.mInstance.PlaySFX("sound_build_z_11");
    }

    public void ClickYuanZheng()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_Expedition);
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().flamebattle)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().flamebattle));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_FlameBattle))
        {
            YuanZhengPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<YuanZhengPanel>();
            if ((gUIEntity != null) && gUIEntity.Hidden)
            {
                if (<>f__am$cache53 == null)
                {
                    <>f__am$cache53 = obj => SocketMgr.Instance.RequestFlameBattleInfo();
                }
                GUIMgr.Instance.PushGUIEntity("YuanZhengPanel", <>f__am$cache53);
            }
            else
            {
                if (<>f__am$cache54 == null)
                {
                    <>f__am$cache54 = delegate (GUIEntity obj) {
                    };
                }
                GUIMgr.Instance.PushGUIEntity("YuanZhengPanel", <>f__am$cache54);
            }
        }
    }

    private void ClickZhaoMu()
    {
        <ClickZhaoMu>c__AnonStorey219 storey = new <ClickZhaoMu>c__AnonStorey219 {
            <>f__this = this
        };
        SoundManager.mInstance.PlaySFX("sound_build_z_6");
        this._isPlay = true;
        GameObject obj2 = GameObject.Find("GameObject/cj_xzjm2/jiuguan/men");
        storey.anim = obj2.transform.GetComponent<Animation>();
        AnimationClip clip = storey.anim.GetClip("kai");
        if (clip != null)
        {
            GUIMgr.Instance.Lock();
            storey.anim.Play(clip.name, PlayMode.StopSameLayer);
        }
        ObjectManager.CreateTempObj("EffectPrefabs/jiuguang_light", obj2.transform.position, 1f);
        this.DelayCallBack(1f, new System.Action(storey.<>m__43F));
    }

    public void Create3DRole(int cardEntry)
    {
        Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) cardEntry);
        if (cardByEntry != null)
        {
            this.ClearAll3DRole();
            GameObject obj2 = CardPlayer.CreateCardPlayerWithEquip((int) cardByEntry.cardInfo.entry, cardByEntry.equipInfo, CardPlayerStateType.Normal, cardByEntry.cardInfo.quality);
            if (obj2 != null)
            {
                Transform transform = GameObject.Find("juese").transform;
                Vector3 localScale = obj2.transform.localScale;
                obj2.transform.parent = transform;
                obj2.transform.localScale = localScale;
                obj2.transform.position = transform.position;
                obj2.transform.rotation = transform.rotation;
                obj2.name = "3DRole";
                if (obj2.transform.GetComponent<BoxCollider>() == null)
                {
                    BoxCollider collider = transform.gameObject.AddComponent<BoxCollider>();
                    collider.center = new Vector3(0f, 0.6f, 0f);
                    collider.size = new Vector3(1.3f, 1.3f, 1.3f);
                }
                this.mCurrCardEntry = cardEntry;
                obj2.GetComponent<AnimFSM>().PlayStandAnim();
            }
            this.mRole3DRole = obj2;
        }
    }

    private void EnterGuildList()
    {
        if (<>f__am$cache57 == null)
        {
            <>f__am$cache57 = obj => SocketMgr.Instance.RequestGetGuildList(0, false);
        }
        GUIMgr.Instance.PushGUIEntity("GuildListPanel", <>f__am$cache57);
    }

    private void EnterMyGuild()
    {
        if (<>f__am$cache58 == null)
        {
            <>f__am$cache58 = delegate (GUIEntity obj) {
            };
        }
        GUIMgr.Instance.PushGUIEntity("GuildPanel", <>f__am$cache58);
    }

    public GameObject FindBuildingIconObjectByKey(string key)
    {
        int index = this.FindBuildingIndexByKey(key);
        if (index == -1)
        {
            return null;
        }
        if (index >= this._BuildingIcon.Length)
        {
            return null;
        }
        return this._BuildingIcon[index];
    }

    private int FindBuildingIndexByKey(string key)
    {
        int length = this.buildingStr.Length;
        for (int i = 0; i != length; i++)
        {
            if (key == this.buildingStr[i])
            {
                return i;
            }
        }
        return -1;
    }

    private void FunctionBuildingResponse(GuideEvent _event)
    {
        if (GuideSystem.MatchEvent(_event))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Function.tag_function_select_building, null);
        }
    }

    private bool FunIsOpen(int idx)
    {
        switch (idx)
        {
            case 0:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().mail);

            case 1:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().flamebattle);

            case 2:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().dungeons);

            case 3:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().arena_ladder);

            case 4:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().pub);

            case 5:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().void_tower);

            case 6:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().shop);

            case 7:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().outland_beginner);

            case 8:
                return false;

            case 9:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().guild);

            case 10:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().lifeskill);

            case 11:
                return !SoulBox.FuncShowable();

            case 12:
                return (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().convoy_lv);
        }
        return false;
    }

    [DebuggerHidden]
    private IEnumerator GetGuildMsgSync()
    {
        return new <GetGuildMsgSync>c__Iterator94 { <>f__this = this };
    }

    private void InsertMsgByLists(List<GuildMsgItem> items)
    {
        if (items.Count > 0)
        {
            Transform listMsgPanel = this._ChatPanel.FindChild("GuildGroup/List");
            UIScrollBar component = this._ChatPanel.FindChild("GuildGroup/ScrollBar").GetComponent<UIScrollBar>();
            UIScrollView view = listMsgPanel.GetComponent<UIScrollView>();
            foreach (GuildMsgItem item in items)
            {
                this.InsertMsgChat(item, listMsgPanel, component, view);
            }
        }
    }

    public void InsertMsgChat(GuildMsgItem gmItem, Transform listMsgPanel, UIScrollBar uiScrollBar, UIScrollView view)
    {
        if (gmItem != null)
        {
            GameObject item = UnityEngine.Object.Instantiate(this._SingleChatItem) as GameObject;
            item.transform.parent = listMsgPanel;
            item.transform.localPosition = new Vector3(0f, (float) ActorData.getInstance().OffsetY, -0.1f);
            item.transform.localScale = new Vector3(1f, 1f, 1f);
            UILabel component = item.transform.FindChild("Item/Label").GetComponent<UILabel>();
            UISprite sprite = item.transform.FindChild("Item/Border").GetComponent<UISprite>();
            string[] textArray1 = new string[] { "[0069c7]", gmItem.name, ":", GameConstant.DefaultTextColor, gmItem.msg };
            component.text = string.Concat(textArray1);
            sprite.height = component.height + 30;
            ActorData data1 = ActorData.getInstance();
            data1.OffsetY -= sprite.height;
            Transform transform = item.transform.FindChild("Item");
            GUIDataHolder.setData(transform.gameObject, gmItem);
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickGuildMsgItemBtn);
            UIEventListener.Get(transform.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressBox);
            if (this.mGuildMsgItemList.Count > 0x31)
            {
                GameObject obj3 = this.mGuildMsgItemList[0];
                if (this._ChatOperateGuildMsgTips.transform.parent == obj3.transform)
                {
                    this._ChatOperateGuildMsgTips.transform.parent = this._ChatPanel.transform;
                    this._ChatOperateGuildMsgTips.gameObject.SetActive(false);
                }
                this.mGuildMsgItemList.Remove(obj3);
                UnityEngine.Object.Destroy(obj3);
            }
            this.mGuildMsgItemList.Add(item);
            uiScrollBar.value = 1f;
            view.InvalidateBounds();
            view.UpdatePosition();
        }
    }

    public void InsetChat(FastBuf.Broadcast bc)
    {
        if (bc != null)
        {
            string[] textArray1 = new string[] { "[0069c7]", bc.userName, ":", GameConstant.DefaultTextColor, bc.content };
            string str = string.Concat(textArray1);
            Transform transform = this._ChatPanel.FindChild("Group/List");
            GameObject item = UnityEngine.Object.Instantiate(this._SingleChatItem) as GameObject;
            item.transform.parent = transform;
            item.transform.localPosition = new Vector3(0f, 0f, -0.1f);
            item.transform.localScale = new Vector3(1f, 1f, 1f);
            UILabel component = item.transform.FindChild("Item/Label").GetComponent<UILabel>();
            UISprite sprite = item.transform.FindChild("Item/Border").GetComponent<UISprite>();
            component.text = str;
            sprite.height = component.height + 30;
            if (bc.userId > 0L)
            {
                Transform transform2 = item.transform.FindChild("Item");
                GUIDataHolder.setData(transform2.gameObject, bc);
                UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickChatItemBtn);
                UIEventListener.Get(transform2.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressBox);
            }
            if (this.mChatItemList.Count > 0x31)
            {
                GameObject obj3 = this.mChatItemList[0];
                if (this._ChatOperateTips.transform.parent == obj3.transform)
                {
                    this._ChatOperateTips.transform.parent = this._ChatPanel.transform;
                    this._ChatOperateTips.gameObject.SetActive(false);
                }
                this.mChatItemList.Remove(obj3);
                UnityEngine.Object.Destroy(obj3);
            }
            this.ResetChatItemPostion(sprite.height);
            this.mChatItemList.Add(item);
        }
    }

    private bool MapOverBoardInner(float _x)
    {
        if ((Camera.main == null) || !Camera.main.active)
        {
            return false;
        }
        if (((Camera.main.transform.localPosition.x - _x) >= this.maxCameraInnerSize.y) && ((Camera.main.transform.localPosition.x - _x) <= this.maxCameraInnerSize.x))
        {
            return false;
        }
        return true;
    }

    private bool MapOverBoardOuter(float _x)
    {
        if ((Camera.main == null) || !Camera.main.active)
        {
            return false;
        }
        if (((Camera.main.transform.localPosition.x - _x) >= this.maxCameraOuterSize.y) && ((Camera.main.transform.localPosition.x - _x) <= this.maxCameraOuterSize.x))
        {
            return false;
        }
        return true;
    }

    private void On_Swipe(Gesture gesture)
    {
        if (((((Camera.main != null) && Camera.main.active) && !this.lockSwippy) && this.checkTouchEnable()) && !this.mDargChatBox)
        {
            if (this == null)
            {
                Debug.LogWarning("SubscribeEvent is Too More!");
            }
            else if (base.gameObject == null)
            {
                Debug.LogWarning("SubscribeEvent is Too More!");
            }
            else
            {
                Vector2 vector = (Vector2) (Vector2.Scale(gesture.position - this.startPos, -this.scale) / 100f);
                this.startPos = gesture.position;
                if (vector.magnitude >= 0.0001f)
                {
                    if (!this.MapOverBoardOuter(vector.x))
                    {
                        Transform transform = Camera.main.transform;
                        transform.localPosition -= vector;
                        this.mMomentum = Vector2.Lerp(this.mMomentum, this.mMomentum + ((Vector2) (vector * (0.01f * this.momentumAmount))), 0.67f);
                        this.cancelClickModel = true;
                    }
                    else
                    {
                        this.mMomentum = Vector2.zero;
                    }
                }
            }
        }
    }

    private void On_SwipeEnd(Gesture gesture)
    {
        if (!this.lockSwippy && this.checkTouchEnable())
        {
            this.mPressed = false;
            this.mShowGmBtnTime = 0;
        }
    }

    private void On_SwipeStart(Gesture gesture)
    {
        if (((Camera.main != null) && Camera.main.active) && !this.lockSwippy)
        {
            this.mShowGmBtnTime = TimeMgr.Instance.ServerStampTime + 5;
            if (this.checkTouchEnable())
            {
                this.mMomentum = Vector2.zero;
                this.mPressed = true;
                this.startPos = gesture.position;
                SpringPosition component = Camera.main.GetComponent<SpringPosition>();
                if (component != null)
                {
                    component.enabled = false;
                }
            }
        }
    }

    private void On_TouchDown(Gesture gesture)
    {
        if (this.checkTouchEnable() && ((this.mStartShowTextTime > 0) && (TimeMgr.Instance.ServerStampTime >= this.mStartShowTextTime)))
        {
            this.mStartShowTextTime = 0;
        }
    }

    private void On_TouchStart(Gesture gesture)
    {
        if ((((Camera.main != null) && Camera.main.enabled) && this.checkTouchEnable()) && this.mInitEnd)
        {
            this.mStartShowTextTime = TimeMgr.Instance.ServerStampTime + 1;
            if ((gesture.fingerIndex == 0) && (Camera.main != null))
            {
                RaycastHit hit;
                this.cancelClickModel = false;
                if (Physics.Raycast(Camera.main.ScreenPointToRay((Vector3) gesture.position), out hit, 100f) && (this.HoverdObject == null))
                {
                    this.mCurrTouchGo = hit.collider.gameObject;
                    if ((this.mCurrTouchGo != null) && (this.mCurrTouchGo.name != "juese"))
                    {
                        this.ChangeModelMaterial(hit.collider.gameObject, "outline");
                        this.mStartShowTextTime = TimeMgr.Instance.ServerStampTime + 1;
                    }
                }
            }
        }
    }

    private void On_TouchUp(Gesture gesture)
    {
        this.mDargChatBox = false;
        if ((((Camera.main != null) && Camera.main.enabled) && this.checkTouchEnable()) && ((gesture.fingerIndex == 0) && (Camera.main != null)))
        {
            RaycastHit hit;
            this.mStartShowTextTime = 0;
            if (this.mCurrTouchGo != null)
            {
                this.ResetModelMaterial(this.mCurrTouchGo);
            }
            if ((Physics.Raycast(Camera.main.ScreenPointToRay((Vector3) gesture.position), out hit, 100f) && (this.HoverdObject == null)) && ((hit.collider.gameObject == this.mCurrTouchGo) && !this.cancelClickModel))
            {
                if (this.mCurrTouchGo.name != "juese")
                {
                    this.SelectModel(hit.collider.gameObject);
                }
                else if (this.mRole3DRole != null)
                {
                    this.mRole3DRole.GetComponent<AnimFSM>().PlayAnim("rest", 1f, 0f, false);
                }
            }
            this.mCurrTouchGo = null;
        }
    }

    private void OnClickAddFriendBtn(GameObject go)
    {
        if (this.mCurrPlayerId == -1L)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896ba));
            this._ChatOperateTips.gameObject.SetActive(false);
            this._ChatOperateGuildMsgTips.gameObject.SetActive(false);
        }
        else
        {
            if (this.mCurrPlayerId == ActorData.getInstance().SessionInfo.userid)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a2));
            }
            else if (ActorData.getInstance().FriendList.Find(e => e.userInfo.id == this.mCurrPlayerId) != null)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a3));
            }
            else
            {
                SocketMgr.Instance.RequestAddFriend(this.mCurrPlayerId);
            }
            this._ChatOperateTips.gameObject.SetActive(false);
            this._ChatOperateGuildMsgTips.gameObject.SetActive(false);
        }
    }

    private void OnClickAddGuildBtn(GameObject go)
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().guild)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().guild));
        }
        else if (this.mCurrGuildId == -1L)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652c7));
        }
        else if ((ActorData.getInstance().mGuildData != null) && (this.mCurrGuildId == ActorData.getInstance().mGuildData.id))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652c8));
        }
        else if ((ActorData.getInstance().mGuildData != null) && (ActorData.getInstance().mGuildData.id != -1L))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652c9));
        }
        else
        {
            SocketMgr.Instance.RequestGuildApply(this.mCurrGuildId);
        }
        this._ChatOperateTips.gameObject.SetActive(false);
    }

    private void OnClickChatBox(GameObject go)
    {
        if (this._ChatOperateTips != null)
        {
            this._ChatOperateTips.SetActive(false);
        }
        if (this._ChatOperateGuildMsgTips != null)
        {
            this._ChatOperateGuildMsgTips.SetActive(false);
        }
    }

    private void OnClickChatBtn(GameObject go)
    {
        this._chatClickNum++;
        TweenPosition.Begin(this._ChatPanel.gameObject, 0.3f, !this.mIsPopChatPanel ? Vector3.zero : new Vector3(480f, 0f, 0f)).method = UITweener.Method.EaseIn;
        this.mIsPopChatPanel = !this.mIsPopChatPanel;
        if (!this.mIsPopChatPanel)
        {
            this._ChatPanel.transform.FindChild("Btn/Icon").gameObject.SetActive(false);
            this._ChatPanel.transform.FindChild("Btn/Back").gameObject.SetActive(true);
            CommonFunc.ResetClippingPanel(this._ChatPanel.transform.FindChild("Group/List"));
            this.StartGetGuildMsg();
            this.guildMsgState = GuildMsgState.show;
        }
        else
        {
            this._ChatPanel.transform.FindChild("Btn/Icon").gameObject.SetActive(true);
            this._ChatPanel.transform.FindChild("Btn/Back").gameObject.SetActive(false);
            this.StartGetGuildMsg();
            this.guildMsgState = GuildMsgState.disPlay;
        }
    }

    private void OnClickChatItemBtn(GameObject go)
    {
        if (this._ChatOperateTips != null)
        {
            if (this._ChatOperateTips.gameObject.activeSelf && (this._ChatOperateTips.transform.parent == go.transform.parent))
            {
                this._ChatOperateTips.gameObject.SetActive(false);
                this.mCurrPlayerId = -1L;
            }
            else
            {
                object obj2 = GUIDataHolder.getData(go);
                if (obj2 != null)
                {
                    FastBuf.Broadcast broadcast = obj2 as FastBuf.Broadcast;
                    if (broadcast != null)
                    {
                        this.mCurrPlayerId = broadcast.userId;
                        this._ChatOperateTips.transform.FindChild("Group/Name").GetComponent<UILabel>().text = broadcast.userName;
                        this._ChatOperateTips.transform.FindChild("Group/GuildName").GetComponent<UILabel>().text = (broadcast.guild_id <= 0L) ? string.Empty : broadcast.guildName;
                        this.mCurrGuildId = broadcast.guild_id;
                        this._ChatOperateTips.transform.FindChild("Group/AddGuild").GetComponent<UIButton>().isEnabled = broadcast.guild_id > 0L;
                        this._ChatOperateTips.transform.parent = go.transform.parent;
                        this._ChatOperateTips.transform.localPosition = Vector3.zero;
                        this._ChatOperateTips.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnClickCommonTab(GameObject go)
    {
        this._CommonChatGroup.gameObject.SetActive(true);
        this._GuildChatGroup.gameObject.SetActive(false);
    }

    private void OnClickGuildMsgBtn()
    {
        if (TimeMgr.Instance.ServerStampTime > (ActorData.getInstance().mask_guildMsg_time + ActorData.getInstance().GuildMsgNoTime))
        {
            if ((ActorData.getInstance().mGuildData != null) && (ActorData.getInstance().mGuildData.id != -1L))
            {
                if (<>f__am$cache50 == null)
                {
                    <>f__am$cache50 = delegate (GUIEntity gui) {
                        XiaoLaBaPanel panel = (XiaoLaBaPanel) gui;
                        panel.Depth = 400;
                        panel.SetCostStoneActive(false);
                        panel.isGuildMsg = true;
                        panel.SetTitle("公会发言");
                    };
                }
                GUIMgr.Instance.DoModelGUI("XiaoLaBaPanel", <>f__am$cache50, null);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186aa));
            }
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186ab));
        }
    }

    private void OnClickGuildMsgItemBtn(GameObject go)
    {
        if (this._ChatOperateGuildMsgTips != null)
        {
            if (this._ChatOperateGuildMsgTips.gameObject.activeSelf && (this._ChatOperateGuildMsgTips.transform.parent == go.transform.parent))
            {
                this._ChatOperateGuildMsgTips.gameObject.SetActive(false);
                this.mCurrPlayerId = -1L;
            }
            else
            {
                object obj2 = GUIDataHolder.getData(go);
                if (obj2 != null)
                {
                    GuildMsgItem item = obj2 as GuildMsgItem;
                    if (item != null)
                    {
                        this.mCurrPlayerId = StrParser.ParseInt64(item.uid, -1L);
                        this._ChatOperateGuildMsgTips.transform.FindChild("Group/Name").GetComponent<UILabel>().text = item.name;
                        this._ChatOperateGuildMsgTips.transform.parent = go.transform.parent;
                        this._ChatOperateGuildMsgTips.transform.localPosition = Vector3.zero;
                        this._ChatOperateGuildMsgTips.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnClickGuildTab(GameObject go)
    {
        this._CommonChatGroup.gameObject.SetActive(false);
        this._GuildChatGroup.gameObject.SetActive(true);
    }

    private void OnClickJiaoHu()
    {
        string str = ServerInfo.getInstance().Community_url;
        Debug.Log("OnClickJiaoHu " + str);
        if (!string.IsNullOrEmpty(str))
        {
            PlatformInterface.mInstance.PlatformOpenWebView(str);
        }
    }

    private void OnClickNameBtn(GameObject go, bool isPressed)
    {
        base.transform.FindChild("TopLeft/NameTips").gameObject.SetActive(isPressed);
    }

    private void OnClickOpenChat()
    {
        if ((ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().broadcast_lv) && (ActorData.getInstance().UserInfo.vip_level.level < (CommonFunc.LevelLimitCfg().broadcast_viplv + 1)))
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x186ad), CommonFunc.LevelLimitCfg().broadcast_lv, CommonFunc.LevelLimitCfg().broadcast_viplv + 1));
        }
        else if (ActorData.getInstance().UserInfo.mask_chat_time > TimeMgr.Instance.ServerStampTime)
        {
            if (<>f__am$cache4E == null)
            {
                <>f__am$cache4E = e => ((MessageBox) e).SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x586), ActorData.getInstance().UserInfo.mask_chat_reason, TimeMgr.Instance.GetTimeStr(ActorData.getInstance().UserInfo.mask_chat_len), TimeMgr.Instance.GetFrozenTime(ActorData.getInstance().UserInfo.mask_chat_time)), null, null, true);
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache4E, null);
        }
        else
        {
            if (<>f__am$cache4F == null)
            {
                <>f__am$cache4F = delegate (GUIEntity gui) {
                    XiaoLaBaPanel panel = (XiaoLaBaPanel) gui;
                    panel.Depth = 400;
                };
            }
            GUIMgr.Instance.DoModelGUI("XiaoLaBaPanel", <>f__am$cache4F, null);
        }
    }

    private void OnClickRankButton(GameObject go)
    {
        AllRankInOnePanel.Open(EN_RankListType.EN_RANKLIST_LEVEL, true);
    }

    private void OnClickShengHuo()
    {
        this.FunctionBuildingResponse(GuideEvent.Function_LifeSkill);
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().lifeskill)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().lifeskill));
        }
        else
        {
            SoundManager.mInstance.PlaySFX("shenghuo");
            GUIMgr.Instance.PushGUIEntity("RichActivityPanel", null);
        }
    }

    public override void OnDestroy()
    {
        this.UnsubscribeEvent();
        this.ClearAll3DRole();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvent();
    }

    public override void OnInitialize()
    {
        this.UpdateUserBaseData();
        if (ActorData.getInstance() != null)
        {
            this.Create3DRole(ActorData.getInstance().UserInfo.headEntry);
        }
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/youxiang/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/yuanzheng/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/dixiacheng/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/jingjichang/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/jiuguan/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/shilianta/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/tiejiangpu/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/waiyu/Tip").transform.position);
        this._Building.Add(GameObject.Find("fuben/fuben_feiting").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/gonghui/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/shenghuo/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/linghunshu/Tip").transform.position);
        this._Building.Add(GameObject.Find("GameObject/cj_xzjm2/diaoxiang/Tip").transform.position);
        this.SignInTipsObj = base.gameObject.transform.FindChild("TopLeft/QianDao/Tips").gameObject;
        this.BoxTipsObj = base.gameObject.transform.FindChild("TopLeft/OpenBox/Tips").gameObject;
        this.TileTipsObj = base.gameObject.transform.FindChild("TopLeft/PlayerBtn/Tips").gameObject;
        this.mMailNewTips = this._BuildingIcon[0].transform.FindChild("new");
        this.mPubNewTips = this._BuildingIcon[4].transform.FindChild("new");
        this.mShopNewTips = this._BuildingIcon[6].transform.FindChild("new");
        this.mArenaNewTips = this._BuildingIcon[3].transform.FindChild("new");
        this.mLifeSkillNewTips = this._BuildingIcon[10].transform.FindChild("new");
        if (this._BuildingIcon.Length >= 13)
        {
            this.mDetainsDartNewTips = this._BuildingIcon[12].transform.FindChild("new");
        }
        this._ChatPanel = base.transform.FindChild("Left/ChatPanel");
        UIEventListener.Get(base.transform.FindChild("Left/ChatPanel/Btn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickChatBtn);
        UIEventListener.Get(base.transform.FindChild("Left/ChatPanel/Box").gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressBox);
        Transform transform = this._ChatPanel.transform.FindChild("CommonTab");
        Transform transform2 = this._ChatPanel.transform.FindChild("GuildTab");
        this._CommonChatGroup = this._ChatPanel.transform.FindChild("Group");
        this._GuildChatGroup = this._ChatPanel.transform.FindChild("GuildGroup");
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCommonTab);
        UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickGuildTab);
        EventDelegate.Add(this.RankButton.onClick, (EventDelegate.Callback) (() => this.OnClickRankButton(this.RankButton.gameObject)));
        this.effectObj = GameObject.Find("cj_zjm_texiao");
        this.OnRegister();
        float num = 1f;
        if ((((float) Screen.width) / ((float) Screen.height)) <= 1.5f)
        {
            this.maxCameraOuterSize.x += num;
            this.maxCameraOuterSize.y -= num;
            this.maxCameraInnerSize.x += num;
            this.maxCameraInnerSize.y -= num;
        }
        this.CheckOpenLevelText();
        if (GameDefine.getInstance().IsThirdPlatform())
        {
            UIEventListener.Get(base.transform.FindChild("TopLeft/Name").gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickNameBtn);
            base.transform.FindChild("TopLeft/NameTips/Label").GetComponent<UILabel>().text = GameDefine.getInstance().TencentLoginNickName;
        }
        this.CheckEffect();
        if (XSingleton<AdManager>.Singleton.AdPics.Count > 0)
        {
            this.AdPanel();
        }
        this.AddChatMsg();
        UIEventListener.Get(this._ChatOperateTips.transform.FindChild("Group/AddFriend").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAddFriendBtn);
        UIEventListener.Get(this._ChatPanel.transform.FindChild("Box").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickChatBox);
        UIEventListener.Get(this._ChatOperateTips.transform.FindChild("Group/AddGuild").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAddGuildBtn);
        UIEventListener.Get(this._ChatOperateGuildMsgTips.transform.FindChild("Group/AddFriend").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAddFriendBtn);
        ActorData.getInstance().OffsetY = 0;
        this.StartGetGuildMsg();
        this.guildMsgState = GuildMsgState.disPlay;
        this.InsertMsgByLists(ActorData.getInstance().guildMsgItems);
        this.SetCommunityBtnStat();
        base.transform.FindChild("TopLeft/HongBao").gameObject.SetActive(ActorData.getInstance().UserInfo.redpackage_enable);
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        this.CheckUpdate();
        this.CheckNewMail();
        this.CheckFreeRecruit();
        this.CheckShopNewStat();
        this.CheckArenaNewTips();
        this.CheckLifeSkill();
        this.CheckDetainsDart();
        if (((Camera.main != null) && Camera.main.active) && !this.mDargChatBox)
        {
            float deltaTime = this.UpdateRealTimeDelta();
            if (!this.mPressed)
            {
                if (this.mMomentum.magnitude > 1E-07f)
                {
                    Vector3 vector = (Vector3) NGUIMath.SpringDampen(ref this.mMomentum, 0.1f, deltaTime);
                    if (!this.MapOverBoardInner(vector.x))
                    {
                        Transform transform = Camera.main.transform;
                        transform.localPosition -= vector;
                    }
                    else
                    {
                        this.mMomentum = Vector2.zero;
                    }
                }
                if (this.MapOverBoardInner(0f))
                {
                    Vector3 pos = new Vector3(this.maxCameraInnerSize.x, -0.4f, 30f);
                    if (Camera.main.transform.localPosition.x < 0f)
                    {
                        pos = new Vector3(this.maxCameraInnerSize.y, -0.4f, 30f);
                    }
                    SpringPosition position = SpringPosition.Begin(Camera.main.gameObject, pos, 10f);
                    position.ignoreTimeScale = true;
                    position.worldSpace = false;
                }
            }
            NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
        }
    }

    private void OnPressBox(GameObject go, bool isPress)
    {
        this.mDargChatBox = isPress;
    }

    private void OnRegister()
    {
        EasyTouch.On_Swipe += new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeStart += new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_SwipeEnd += new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_TouchStart += new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchDown += new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchUp += new EasyTouch.TouchUpHandler(this.On_TouchUp);
    }

    private void OnShowGM()
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        ObjectManager.Instance.Tick();
        if ((ActorData.getInstance().BroadcastList.Count > 0) && !this.mIsSendingBroadMsg)
        {
            string str = ActorData.getInstance().PeekBroadMsg();
            if (str != string.Empty)
            {
                base.StartCoroutine("broadMsgProcess", str);
            }
        }
        if ((ActorData.getInstance().AdAutoInfoList.Count > 0) && !this.mIsSendingAdMsg)
        {
            string adMsg = ActorData.getInstance().GetAdMsg();
            if (adMsg != string.Empty)
            {
                base.StartCoroutine("ADAuto", adMsg);
            }
        }
        if (((this.lastUpdateTime < Time.realtimeSinceStartup) && SocketMgr.NetConnectIsOk) && (ActorData.getInstance().BroadcastList.Count == 0))
        {
            this.lastUpdateTime = Time.realtimeSinceStartup + 120f;
            Debug.Log(this.lastUpdateTime + "---->lasttiem!");
            SocketMgr.Instance.RequestBroadcastList();
        }
        this.CheckNewMail();
    }

    private void OpenBox()
    {
        if (<>f__am$cache4C == null)
        {
            <>f__am$cache4C = obj => SocketMgr.Instance.RequestOpenBoxReq();
        }
        GUIMgr.Instance.PushGUIEntity("BoxPanel", <>f__am$cache4C);
    }

    private void OpenChongZhi()
    {
        if (<>f__am$cache4B == null)
        {
            <>f__am$cache4B = obj => VipCardPanel panel = (VipCardPanel) obj;
        }
        GUIMgr.Instance.PushGUIEntity("VipCardPanel", <>f__am$cache4B);
    }

    private void OpenPlayerInfoPanel()
    {
        if (GuideSystem.MatchEvent(GuideEvent.HelpMe))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_HelpMe.tag_helpme_press_portal, null);
        }
        if (<>f__am$cache51 == null)
        {
            <>f__am$cache51 = delegate (GUIEntity obj) {
                PlayerInfoPanel panel = (PlayerInfoPanel) obj;
                panel.Depth = 380;
            };
        }
        GUIMgr.Instance.DoModelGUI("PlayerInfoPanel", <>f__am$cache51, null);
    }

    private void OpneHongBao()
    {
        if (<>f__am$cache4D == null)
        {
            <>f__am$cache4D = obj => SocketMgr.Instance.RequestRedPackageRecord();
        }
        GUIMgr.Instance.PushGUIEntity("HongBaoPanel", <>f__am$cache4D);
    }

    [DebuggerHidden]
    public IEnumerator PlayPopIcon(bool isPop)
    {
        return new <PlayPopIcon>c__Iterator90 { isPop = isPop, <$>isPop = isPop, <>f__this = this };
    }

    private void ResetChatItemPostion(int OffsetY)
    {
        foreach (GameObject obj2 in this.mChatItemList)
        {
            if (obj2 != null)
            {
                if (this._ChatPanel.transform.FindChild("Group").gameObject.activeSelf)
                {
                    TweenPosition.Begin(obj2, 0.2f, new Vector3(obj2.transform.localPosition.x, obj2.transform.localPosition.y - OffsetY, obj2.transform.localPosition.z));
                }
                else
                {
                    obj2.transform.localPosition = new Vector3(obj2.transform.localPosition.x, obj2.transform.localPosition.y - OffsetY, obj2.transform.localPosition.z);
                }
            }
        }
    }

    private void ResetModelMaterial(GameObject obj)
    {
        MaterialFSM component = obj.GetComponent<MaterialFSM>();
        if (component != null)
        {
            component.ResetMaterial();
        }
    }

    internal void ResetRefreshTime()
    {
        if (!this.isSendGuildMsg)
        {
            base.StartCoroutine(this.GetGuildMsgSync());
        }
    }

    private void SelectModel(GameObject obj)
    {
        while (this._isPlay)
        {
            Debug.Log("return******************************");
            return;
        }
        string name = obj.name;
        if (name != null)
        {
            int num;
            if (<>f__switch$mapD == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(0x10);
                dictionary.Add("shilianta", 0);
                dictionary.Add("yuanzheng", 1);
                dictionary.Add("shenghuo", 2);
                dictionary.Add("waiyu", 3);
                dictionary.Add("jiuguan", 4);
                dictionary.Add("dixiacheng", 5);
                dictionary.Add("youxiang", 6);
                dictionary.Add("tiejiangpu", 7);
                dictionary.Add("BOSS", 8);
                dictionary.Add("shijiebei", 9);
                dictionary.Add("fuben", 10);
                dictionary.Add("zhuangbeishengji", 11);
                dictionary.Add("jingjichang", 12);
                dictionary.Add("gonghui", 13);
                dictionary.Add("linghunshu", 14);
                dictionary.Add("diaoxiang", 15);
                <>f__switch$mapD = dictionary;
            }
            if (<>f__switch$mapD.TryGetValue(name, out num))
            {
                switch (num)
                {
                    case 0:
                        this.ClickTower();
                        break;

                    case 1:
                        this.ClickYuanZheng();
                        break;

                    case 2:
                        this.OnClickShengHuo();
                        break;

                    case 3:
                        this.ClickOutland();
                        break;

                    case 4:
                        this.ClickZhaoMu();
                        break;

                    case 5:
                        this.ClickDungeons();
                        break;

                    case 6:
                        this.ClickMail();
                        break;

                    case 7:
                        this.ClickTiJiangPu();
                        break;

                    case 8:
                        this.ClickWorldBoss();
                        break;

                    case 9:
                        this.ClickWorldCup();
                        break;

                    case 10:
                        this.ClickDup();
                        break;

                    case 11:
                        this.ClickEquipLevUp();
                        break;

                    case 12:
                        this.ClickArena();
                        break;

                    case 13:
                        this.ClickGuild();
                        break;

                    case 14:
                        this.ClickLinghunshu();
                        break;

                    case 15:
                        this.ClickDiaoXiang();
                        break;
                }
            }
        }
        Debug.Log(obj.name);
        if (this.actSelectBuilding != null)
        {
            this.actSelectBuilding();
        }
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (null != activityGUIEntity)
        {
            activityGUIEntity.HideFunBar();
        }
    }

    private void SetCommunityBtnStat()
    {
        Transform transform = base.transform.FindChild("TopLeft/XiaoLaBa");
        if (GameDefine.getInstance().isReleaseServer)
        {
            transform.gameObject.SetActive(true);
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }

    private void StartGetGuildMsg()
    {
        if ((ActorData.getInstance().mGuildData != null) && (ActorData.getInstance().mGuildData.id != -1L))
        {
            this.BeginListen();
        }
    }

    private void TestBattle()
    {
        List<long> cardGUIDs = new List<long>();
        for (int i = 0; i < 6; i++)
        {
            if (i < ActorData.getInstance().CardList.Count)
            {
                cardGUIDs.Add(ActorData.getInstance().CardList[i].card_id);
            }
        }
        SocketMgr.Instance.RequestEnterDup(0, 0, DuplicateType.DupType_Normal, cardGUIDs, -1L);
    }

    private void TweenChatMsg(bool isHaveNewMsg)
    {
        TweenAlpha component = this._ChatIconSprite.GetComponent<TweenAlpha>();
        Debug.Log("TweenAlpha " + this._chatClickNum);
        if ((this._chatClickNum % 2) == 0)
        {
            Debug.Log(this._chatClickNum);
            if (!component.enabled)
            {
                Debug.Log("isHaveNewMsg " + isHaveNewMsg);
                component.enabled = isHaveNewMsg;
            }
        }
        else
        {
            component.enabled = false;
            component.value = 1f;
        }
    }

    public void UnsubscribeEvent()
    {
        EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeStart -= new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_SwipeEnd -= new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_TouchStart -= new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.On_TouchUp);
    }

    public void UpdateGuild()
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().guild)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().guild));
        }
        else if (ActorData.getInstance().mUserGuildMemberData == null)
        {
            this.EnterGuildList();
        }
        else if (ActorData.getInstance().mUserGuildMemberData.guild_id <= 0L)
        {
            this.EnterGuildList();
        }
        else
        {
            this.EnterMyGuild();
        }
    }

    public void UpdateHead()
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(ActorData.getInstance().UserInfo.headEntry);
        if (_config != null)
        {
            base.transform.FindChild("TopLeft/PlayerBtn/HeroIcon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
    }

    public void UpdateHeadFrame()
    {
        UISprite component = base.transform.FindChild("TopLeft/PlayerBtn/FrameBg").GetComponent<UISprite>();
        UISprite sprite2 = base.transform.FindChild("TopLeft/PlayerBtn/TagBg").GetComponent<UISprite>();
        CommonFunc.SetPlayerHeadFrame(component, sprite2, ActorData.getInstance().UserInfo.headFrameEntry);
    }

    public void UpdateLevel()
    {
        base.transform.FindChild("TopLeft/level/Label").GetComponent<UILabel>().text = ActorData.getInstance().Level.ToString();
        this.CheckOpenLevelText();
    }

    public void UpdateName()
    {
        base.transform.FindChild("TopLeft/Name/Label").GetComponent<UILabel>().text = ActorData.getInstance().UserInfo.name;
    }

    private float UpdateRealTimeDelta()
    {
        this.mRt = Time.realtimeSinceStartup;
        float b = this.mRt - this.mTimeStart;
        this.mActual += Mathf.Max(0f, b);
        this.mTimeDelta = 0.001f * Mathf.Round(this.mActual * 1000f);
        this.mActual -= this.mTimeDelta;
        if (this.mTimeDelta > 1f)
        {
            this.mTimeDelta = 1f;
        }
        this.mTimeStart = this.mRt;
        return this.mTimeDelta;
    }

    public void UpdateTencentVip()
    {
    }

    public void UpdateUserBaseData()
    {
        this.UpdateName();
        this.UpdateLevel();
        this.UpdateHead();
        this.UpdateVip();
        this.UpdateTencentVip();
        this.UpdateHeadFrame();
    }

    public void UpdateVip()
    {
        Transform transform = base.transform.FindChild("TopLeft/Vip");
        UISprite component = transform.transform.FindChild("lv").GetComponent<UISprite>();
        if (ActorData.getInstance().UserInfo.vip_level.level < 1)
        {
            component.spriteName = "Ui_Main_Icon_1";
        }
        else
        {
            component.spriteName = "Ui_Main_Icon_" + ActorData.getInstance().UserInfo.vip_level.level;
        }
        component.MakePixelPerfect();
        component.height = (int) (component.height * 0.8f);
        component.width = (int) (component.width * 0.8f);
        transform.gameObject.SetActive(true);
        this.CheckOpenLevelText();
    }

    public bool bNewOrCompleteActive
    {
        get
        {
            return this.b_HaveNewActive__;
        }
        set
        {
            this.b_HaveNewActive__ = value;
        }
    }

    private GameObject HoverdObject
    {
        get
        {
            GameObject hoveredObject = UICamera.hoveredObject;
            if (null == hoveredObject)
            {
                return null;
            }
            if (hoveredObject == this.layerLockObject)
            {
                return null;
            }
            return hoveredObject;
        }
    }

    [CompilerGenerated]
    private sealed class <ADAuto>c__Iterator92 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>msg;
        internal MainUI <>f__this;
        internal float <flyinTime>__3;
        internal bool <moveOver>__5;
        internal int <rightPos>__1;
        internal Vector2 <textSize>__0;
        internal float <textStartMovePos>__4;
        internal float <zDepth>__2;
        internal string msg;

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
                    this.<>f__this.mIsSendingAdMsg = true;
                    this.<>f__this.spriteADBg.transform.localPosition = new Vector3(1500f, 0f, 0f);
                    TweenPosition.Begin(this.<>f__this.spriteADBg.gameObject, 1f, Vector3.zero).method = UITweener.Method.EaseInOut;
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    goto Label_03AA;

                case 1:
                    this.<>f__this.labelAd.UpdateNGUIText();
                    this.<textSize>__0 = NGUIText.CalculatePrintedSize(this.msg);
                    this.<rightPos>__1 = Screen.width * 2;
                    this.<zDepth>__2 = -1f;
                    this.<>f__this.labelAd.transform.localPosition = new Vector3((float) this.<rightPos>__1, 0f, this.<zDepth>__2);
                    this.<>f__this.labelAd.text = this.msg;
                    this.<flyinTime>__3 = 1f;
                    TweenPosition.Begin(this.<>f__this.labelAd.gameObject, this.<flyinTime>__3, new Vector3(-296f, 0f, this.<zDepth>__2)).method = UITweener.Method.EaseInOut;
                    this.$current = new WaitForSeconds(this.<flyinTime>__3 - 0.05f);
                    this.$PC = 2;
                    goto Label_03AA;

                case 2:
                    this.$current = new WaitForSeconds(0.05f);
                    this.$PC = 3;
                    goto Label_03AA;

                case 3:
                    this.$current = new WaitForSeconds(0.05f);
                    this.$PC = 4;
                    goto Label_03AA;

                case 4:
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 5;
                    goto Label_03AA;

                case 5:
                {
                    this.<textStartMovePos>__4 = this.<>f__this.labelAd.transform.localPosition.x;
                    object[] args = new object[] { "position", new Vector3((float) ((-this.<rightPos>__1 * 2) - 500), 0f, this.<zDepth>__2), "islocal", true, "time", 20, "easetype", iTween.EaseType.linear };
                    iTween.MoveTo(this.<>f__this.labelAd.gameObject, iTween.Hash(args));
                    this.<moveOver>__5 = false;
                    break;
                }
                case 6:
                    break;

                case 7:
                    this.<>f__this.mIsSendingAdMsg = false;
                    this.$current = null;
                    this.$PC = 8;
                    goto Label_03AA;

                case 8:
                    this.$PC = -1;
                    goto Label_03A8;

                default:
                    goto Label_03A8;
            }
            if (!this.<moveOver>__5)
            {
                if ((this.<textStartMovePos>__4 - this.<>f__this.labelAd.transform.localPosition.x) > (this.<textSize>__0.x + 250f))
                {
                    this.<moveOver>__5 = true;
                }
                this.$current = new WaitForSeconds(0.1f);
                this.$PC = 6;
            }
            else
            {
                iTween.Stop(this.<>f__this.labelAd.gameObject);
                this.<>f__this.labelAd.text = string.Empty;
                TweenPosition.Begin(this.<>f__this.spriteADBg.gameObject, 0.5f, new Vector3(-1500f, 0f, 0f)).method = UITweener.Method.Linear;
                this.$current = new WaitForSeconds(this.<>f__this.mAdMsgLoopInterval);
                this.$PC = 7;
            }
            goto Label_03AA;
        Label_03A8:
            return false;
        Label_03AA:
            return true;
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
    private sealed class <AddGuildMsgForTimes>c__Iterator93 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MainUI <>f__this;
        internal float <refresTime>__1;
        internal IEnumerator <sender>__0;

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
                    break;

                case 1:
                    goto Label_004C;

                case 2:
                    goto Label_0075;

                case 3:
                    break;
                    this.$PC = -1;
                    goto Label_0101;

                default:
                    goto Label_0101;
            }
            if (this.<>f__this.isSendGuildMsg)
            {
                this.$current = null;
                this.$PC = 1;
                goto Label_0103;
            }
        Label_004C:
            this.<sender>__0 = this.<>f__this.GetGuildMsgSync();
        Label_0075:
            while (this.<sender>__0.MoveNext())
            {
                this.$current = null;
                this.$PC = 2;
                goto Label_0103;
            }
            this.<refresTime>__1 = 60f;
            switch (this.<>f__this.guildMsgState)
            {
                case MainUI.GuildMsgState.disPlay:
                    this.<refresTime>__1 = ActorData.getInstance()._displayGetMsgTime;
                    break;

                case MainUI.GuildMsgState.show:
                    this.<refresTime>__1 = ActorData.getInstance()._showGetMsgTime;
                    break;
            }
            this.$current = new WaitForSeconds(this.<refresTime>__1);
            this.$PC = 3;
            goto Label_0103;
        Label_0101:
            return false;
        Label_0103:
            return true;
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
    private sealed class <broadMsgProcess>c__Iterator91 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>msg;
        internal MainUI <>f__this;
        internal float <flyinTime>__3;
        internal bool <moveOver>__5;
        internal int <rightPos>__1;
        internal Vector2 <textSize>__0;
        internal float <textStartMovePos>__4;
        internal float <zDepth>__2;
        internal string msg;

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
                    this.<>f__this.mIsSendingBroadMsg = true;
                    this.<>f__this.mBroadMsgBg.transform.localPosition = new Vector3(1500f, 0f, 0f);
                    TweenPosition.Begin(this.<>f__this.mBroadMsgBg.gameObject, 1f, Vector3.zero).method = UITweener.Method.EaseInOut;
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    goto Label_03D5;

                case 1:
                    this.<>f__this.mBoradTextlbl.UpdateNGUIText();
                    this.<textSize>__0 = NGUIText.CalculatePrintedSize(this.msg);
                    this.<rightPos>__1 = Screen.width * 2;
                    this.<zDepth>__2 = -1f;
                    this.<>f__this.mBoradTextlbl.transform.localPosition = new Vector3((float) this.<rightPos>__1, 0f, this.<zDepth>__2);
                    this.<>f__this.mBoradTextlbl.text = this.msg;
                    this.<flyinTime>__3 = 1f;
                    this.<>f__this.InsetChat(ActorData.getInstance().mChatList[ActorData.getInstance().mChatList.Count - 1]);
                    TweenPosition.Begin(this.<>f__this.mBoradTextlbl.gameObject, this.<flyinTime>__3, new Vector3(-296f, 0f, this.<zDepth>__2)).method = UITweener.Method.EaseInOut;
                    this.$current = new WaitForSeconds(this.<flyinTime>__3 - 0.05f);
                    this.$PC = 2;
                    goto Label_03D5;

                case 2:
                    this.$current = new WaitForSeconds(0.05f);
                    this.$PC = 3;
                    goto Label_03D5;

                case 3:
                    this.$current = new WaitForSeconds(0.05f);
                    this.$PC = 4;
                    goto Label_03D5;

                case 4:
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 5;
                    goto Label_03D5;

                case 5:
                {
                    this.<textStartMovePos>__4 = this.<>f__this.mBoradTextlbl.transform.localPosition.x;
                    object[] args = new object[] { "position", new Vector3((float) ((-this.<rightPos>__1 * 2) - 500), 0f, this.<zDepth>__2), "islocal", true, "time", 20, "easetype", iTween.EaseType.linear };
                    iTween.MoveTo(this.<>f__this.mBoradTextlbl.gameObject, iTween.Hash(args));
                    this.<moveOver>__5 = false;
                    break;
                }
                case 6:
                    break;

                case 7:
                    this.<>f__this.mIsSendingBroadMsg = false;
                    this.$current = null;
                    this.$PC = 8;
                    goto Label_03D5;

                case 8:
                    this.$PC = -1;
                    goto Label_03D3;

                default:
                    goto Label_03D3;
            }
            if (!this.<moveOver>__5)
            {
                if ((this.<textStartMovePos>__4 - this.<>f__this.mBoradTextlbl.transform.localPosition.x) > (this.<textSize>__0.x + 250f))
                {
                    this.<moveOver>__5 = true;
                }
                this.$current = new WaitForSeconds(0.1f);
                this.$PC = 6;
            }
            else
            {
                iTween.Stop(this.<>f__this.mBoradTextlbl.gameObject);
                this.<>f__this.mBoradTextlbl.text = string.Empty;
                TweenPosition.Begin(this.<>f__this.mBroadMsgBg.gameObject, 0.5f, new Vector3(-1500f, 0f, 0f)).method = UITweener.Method.Linear;
                this.$current = new WaitForSeconds(this.<>f__this.mBroadMsgLoopInterval);
                this.$PC = 7;
            }
            goto Label_03D5;
        Label_03D3:
            return false;
        Label_03D5:
            return true;
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
    private sealed class <ClickLinghunshu>c__AnonStorey21B
    {
        internal MainUI <>f__this;
        internal Animation anim;

        internal void <>m__44C()
        {
            GUIMgr.Instance.UnLock();
            this.<>f__this._isPlay = false;
            if (SoulBox.FuncShowable())
            {
                GUIMgr.Instance.DoModelGUI<SoulBox>(null, null);
            }
            AnimationClip clip = this.anim.GetClip("daiji");
            this.anim.Play(clip.name);
        }
    }

    [CompilerGenerated]
    private sealed class <ClickOutland>c__AnonStorey21A
    {
        internal MainUI <>f__this;
        internal Animation anim;
        internal AnimationClip freeClip;

        internal void <>m__447()
        {
            GUIMgr.Instance.UnLock();
            this.<>f__this._isPlay = false;
            ActorData.getInstance().isPreOutlandFight = false;
            if (ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().outland_beginner)
            {
                ActorData.getInstance().bOpenOutlandTitleInfo = true;
                SocketMgr.Instance.RequestOutlandsData(delegate {
                    this.freeClip = this.anim.GetClip("hua_daiji");
                    this.anim.Play(this.freeClip.name, PlayMode.StopSameLayer);
                }, null);
            }
            else
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().outland_beginner));
                this.<>f__this.DelayCallBack(0.2f, delegate {
                    this.freeClip = this.anim.GetClip("hua_daiji");
                    this.anim.Play(this.freeClip.name, PlayMode.StopSameLayer);
                });
            }
        }

        internal void <>m__44F()
        {
            this.freeClip = this.anim.GetClip("hua_daiji");
            this.anim.Play(this.freeClip.name, PlayMode.StopSameLayer);
        }

        internal void <>m__450()
        {
            this.freeClip = this.anim.GetClip("hua_daiji");
            this.anim.Play(this.freeClip.name, PlayMode.StopSameLayer);
        }
    }

    [CompilerGenerated]
    private sealed class <ClickZhaoMu>c__AnonStorey219
    {
        internal MainUI <>f__this;
        internal Animation anim;

        internal void <>m__43F()
        {
            GUIMgr.Instance.UnLock();
            this.<>f__this._isPlay = false;
            AnimationClip clip = this.anim.GetClip("guan");
            this.anim.Play(clip.name, PlayMode.StopSameLayer);
            if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().pub)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().pub));
            }
            else
            {
                if (GuideSystem.MatchEvent(GuideEvent.GoldRecruit) || GuideSystem.MatchEvent(GuideEvent.StoneRecruit))
                {
                    GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Recruit.tag_recruit_select_building, null);
                }
                GUIMgr.Instance.PushGUIEntity("RecruitPanel", null);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <GetGuildMsgSync>c__Iterator94 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<object>.Enumerator <$s_768>__9;
        internal MainUI <>f__this;
        internal object <chatMsgArray>__7;
        internal List<object> <chatMsgArrays>__8;
        internal GuildMsgItem <gItem>__13;
        internal GuildMsgItem <gmItem>__12;
        internal Dictionary<string, object> <infoData>__11;
        internal bool <isHaveNewMsg>__6;
        internal Dictionary<string, object> <jsondata>__3;
        internal object <msgArray>__10;
        internal Dictionary<string, object> <msgData>__5;
        internal string <strResult>__2;
        internal bool <valid>__4;
        internal WWW <www>__1;
        internal WWWForm <wwwFrom>__0;

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
                    this.<>f__this.isSendGuildMsg = true;
                    if ((ActorData.getInstance().mGuildData != null) && (ActorData.getInstance().mGuildData.id != -1L))
                    {
                        Debug.Log("guilde get msg guildid " + ActorData.getInstance().mGuildData.id);
                        this.<wwwFrom>__0 = new WWWForm();
                        this.<wwwFrom>__0.AddField("KEY", ActorData.getInstance().guildMsgKey.ToString());
                        this.<wwwFrom>__0.AddField("USERID", ActorData.getInstance().SessionInfo.userid.ToString());
                        this.<wwwFrom>__0.AddField("APP", "Chat");
                        this.<wwwFrom>__0.AddField("ACT", "getChat");
                        this.<wwwFrom>__0.AddField("MAINKEY", ServerInfo.lastGameServerId.ToString() + "|" + ActorData.getInstance().mGuildData.id.ToString());
                        this.<wwwFrom>__0.AddField("T", Time.time.ToString());
                        this.<www>__1 = new WWW(ServerInfo.getInstance().Chat_Url, this.<wwwFrom>__0);
                        break;
                    }
                    this.<>f__this.StopCoroutine(this.<>f__this.refresher);
                    goto Label_050E;

                case 1:
                    break;

                default:
                    goto Label_050E;
            }
            while (!this.<www>__1.isDone)
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (string.IsNullOrEmpty(this.<www>__1.error))
            {
                this.<strResult>__2 = this.<www>__1.text;
                Debug.Log("guild get msg result " + this.<strResult>__2);
                if (!string.IsNullOrEmpty(this.<strResult>__2))
                {
                    this.<jsondata>__3 = JSON.Instance.ToObject<Dictionary<string, object>>(this.<strResult>__2);
                    if (this.<jsondata>__3 != null)
                    {
                        this.<>f__this.tempMsgItems.Clear();
                        if (this.<jsondata>__3.ContainsKey("valid"))
                        {
                            this.<valid>__4 = StrParser.ParseBool(this.<jsondata>__3["valid"].ToString(), false);
                            if (this.<valid>__4)
                            {
                                this.<msgData>__5 = (Dictionary<string, object>) this.<jsondata>__3["data"];
                                ActorData.getInstance().guildMsgKey = StrParser.ParseDecInt(this.<msgData>__5["key"].ToString(), 0);
                                this.<isHaveNewMsg>__6 = StrParser.ParseBool(this.<msgData>__5["isHaveNewMsg"].ToString(), false);
                                this.<>f__this.TweenChatMsg(this.<isHaveNewMsg>__6);
                                if (this.<msgData>__5.ContainsKey("dTime"))
                                {
                                    ActorData.getInstance()._displayGetMsgTime = StrParser.ParseFloat(this.<msgData>__5["dTime"].ToString(), 60f);
                                }
                                if (this.<msgData>__5.ContainsKey("sTime"))
                                {
                                    ActorData.getInstance()._showGetMsgTime = StrParser.ParseFloat(this.<msgData>__5["sTime"].ToString(), 10f);
                                }
                                this.<chatMsgArray>__7 = this.<msgData>__5["chatMsgArray"];
                                if (this.<chatMsgArray>__7 != null)
                                {
                                    this.<chatMsgArrays>__8 = (List<object>) this.<chatMsgArray>__7;
                                    Debug.Log(this.<chatMsgArrays>__8.Count);
                                    this.<$s_768>__9 = this.<chatMsgArrays>__8.GetEnumerator();
                                    try
                                    {
                                        while (this.<$s_768>__9.MoveNext())
                                        {
                                            this.<msgArray>__10 = this.<$s_768>__9.Current;
                                            Debug.Log("add");
                                            this.<infoData>__11 = (Dictionary<string, object>) this.<msgArray>__10;
                                            this.<gmItem>__12 = new GuildMsgItem();
                                            this.<gmItem>__12.name = this.<infoData>__11["name"].ToString();
                                            this.<gmItem>__12.uid = this.<infoData>__11["uid"].ToString();
                                            this.<gmItem>__12.msg = this.<infoData>__11["msg"].ToString();
                                            this.<>f__this.tempMsgItems.Add(this.<gmItem>__12);
                                            ActorData.getInstance().guildMsgItems.Add(this.<gmItem>__12);
                                            if (ActorData.getInstance().guildMsgItems.Count > 0x31)
                                            {
                                                this.<gItem>__13 = ActorData.getInstance().guildMsgItems[0];
                                                if (this.<gItem>__13 != null)
                                                {
                                                    ActorData.getInstance().guildMsgItems.Remove(this.<gItem>__13);
                                                }
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        this.<$s_768>__9.Dispose();
                                    }
                                }
                            }
                        }
                        this.<>f__this.isSendGuildMsg = false;
                    }
                }
                this.<>f__this.InsertMsgByLists(this.<>f__this.tempMsgItems);
                this.$PC = -1;
            }
        Label_050E:
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
    private sealed class <PlayPopIcon>c__Iterator90 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool <$>isPop;
        internal MainUI <>f__this;
        internal int <i>__0;
        internal bool isPop;

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
                    if (this.<>f__this.mInitEnd)
                    {
                        this.<>f__this._BuildingGroup.SetActive(this.isPop);
                        this.<i>__0 = 0;
                        while (this.<i>__0 < this.<>f__this._BuildingIcon.Length)
                        {
                            if (this.isPop)
                            {
                                this.<>f__this._BuildingIcon[this.<i>__0].transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                                this.<>f__this._BuildingIcon[this.<i>__0].SetActive(true);
                                TweenScale.Begin(this.<>f__this._BuildingIcon[this.<i>__0], 0.1f, new Vector3(1f, 1f, 1f));
                                TweenAlpha.Begin(this.<>f__this._BuildingIcon[this.<i>__0], 0.1f, 1f);
                            }
                            else
                            {
                                TweenScale.Begin(this.<>f__this._BuildingIcon[this.<i>__0], 0.1f, new Vector3(0.001f, 0.001f, 0.001f));
                                TweenAlpha.Begin(this.<>f__this._BuildingIcon[this.<i>__0], 0.1f, 0f);
                                this.<>f__this._BuildingIcon[this.<i>__0].SetActive(false);
                            }
                            this.<i>__0++;
                        }
                        this.$current = null;
                        this.$PC = 1;
                        return true;
                    }
                    break;

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

    private enum GuildMsgState
    {
        disPlay,
        show
    }
}

