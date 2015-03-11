using FastBuf;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class TitleBar : GUIEntity
{
    public UILabel _BuyCount;
    public UILabel _CooldownAllTime;
    public UILabel _CooldownTime;
    public GameObject _FunBg;
    public GameObject _FunBtn;
    public GameObject _FunBtnNew;
    public GameObject _FunGroup;
    public Transform _GoldTf;
    public UILabel _IntervalTime;
    public Transform _StoneTf;
    public Transform _TiLiTf;
    public UILabel _TiLiTimeLabel;
    public GameObject _TiLiTimePanel;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache17;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache18;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache19;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1A;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1B;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1C;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1D;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1E;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache1F;
    public System.Action actHiddenFunc;
    private bool createBarOK;
    private float lastShow;
    private UILabel lblGold;
    private UILabel lblStone;
    private UILabel lblTiLi;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private int mCurrDepth;
    private bool mIsStart = true;

    [DebuggerHidden]
    public IEnumerator ChangeNumberAmin(UILabel _num)
    {
        return new <ChangeNumberAmin>c__Iterator95 { _num = _num, <$>_num = _num };
    }

    public void CheckAchievement()
    {
        base.transform.FindChild("TopRight/FunPanel/Group/AchievementPanel/New").gameObject.SetActive(CommonFunc.CheckAchievementCompleted());
    }

    private void CheckBtnTips()
    {
        bool flag = ((CommonFunc.CheckCardCanLevUp() || this.GetFriendNewStat()) || CommonFunc.CheckAchievementCompleted()) || CommonFunc.CheckDailyCompleted();
        this._FunBtnNew.SetActive(flag);
    }

    public void CheckCardTips()
    {
        base.transform.FindChild("TopRight/FunPanel/Group/Hero/New").gameObject.SetActive(CommonFunc.CheckCardCanLevUp());
    }

    public void CheckFriendTips()
    {
        base.transform.FindChild("TopRight/FunPanel/Group/Friend/New").gameObject.SetActive(this.GetFriendNewStat());
    }

    public void CheckLiveness()
    {
        base.transform.FindChild("TopRight/FunPanel/Group/Task/New").gameObject.SetActive(CommonFunc.CheckDailyCompleted());
    }

    public void CheckTips()
    {
        this.CheckCardTips();
        this.CheckFriendTips();
        this.CheckAchievement();
        this.CheckLiveness();
    }

    private void FireNewbieCardBreakGuide()
    {
        HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
        if (null == activityGUIEntity)
        {
            GuideSystem.FireEvent(GuideEvent.CardBreak_Portal);
        }
        else if (activityGUIEntity.CanbeBreak() && GuideSystem.Skipable(GuideEvent.CardBreak_Portal))
        {
            GuideSystem.ActivedGuide.RequestCancel();
            GuideSystem.SkipToEvent(GuideEvent.CardBreak_Function, false);
            GuideSystem.FireEvent(GuideEvent.CardBreak_Function);
        }
    }

    private void FireNewbieMedicineGuide()
    {
        if (ActorData.getInstance().NormalProgress >= 10)
        {
            GuideSystem.FireEvent(GuideEvent.Medicine_Portal);
        }
    }

    private void FireNewbieMissionGuide()
    {
        if (ActorData.getInstance().NormalProgress >= 5)
        {
            GuideSystem.FireEvent(GuideEvent.EquipLevelUp_MissionPortal);
        }
    }

    private void FireNewbieSkillLevelupGuide()
    {
        if ((ActorData.getInstance().NormalProgress >= 7) && (null == GUIMgr.Instance.GetActivityGUIEntity<HeroPanel>()))
        {
            HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
            if (null == activityGUIEntity)
            {
                GuideSystem.FireEvent(GuideEvent.SkillLevelUp_Portal);
            }
            else if (activityGUIEntity.SkillCanbeLevelup() && GuideSystem.Skipable(GuideEvent.SkillLevelUp_Portal))
            {
                GuideSystem.ActivedGuide.RequestCancel();
                GuideSystem.SkipToEvent(GuideEvent.SkillLevelUp_Function, false);
                GuideSystem.FireEvent(GuideEvent.SkillLevelUp_Function);
            }
        }
    }

    private void FireNewbieStrengthenGuide()
    {
        if (null == GUIMgr.Instance.GetActivityGUIEntity<HeroPanel>())
        {
            HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
            if (null == activityGUIEntity)
            {
                GuideSystem.FireEvent(GuideEvent.Strengthen_Portal);
            }
            else if (activityGUIEntity.CanbeStrengthen() && GuideSystem.Skipable(GuideEvent.Strengthen_Portal))
            {
                GuideSystem.ActivedGuide.RequestCancel();
                GuideSystem.SkipToEvent(GuideEvent.Strengthen_Function, false);
                GuideSystem.FireEvent(GuideEvent.Strengthen_Function);
            }
        }
    }

    private bool GetFriendNewStat()
    {
        return (ActorData.getInstance().CurrHaveTiLiCanPick() || ((ActorData.getInstance().FriendList.Count < 200) && (ActorData.getInstance().FriendReqList.Count > 0)));
    }

    public void HideFunBar()
    {
        PlayMakerFSM component = this._FunBtn.GetComponent<PlayMakerFSM>();
        if (component != null)
        {
            component.FsmVariables.FindFsmInt("isOpen").Value = 0;
            this._FunGroup.transform.localPosition = new Vector3(0f, 800f, 0f);
            if (this.actHiddenFunc != null)
            {
                this.actHiddenFunc();
            }
            this.actHiddenFunc = null;
            Transform transform = base.transform.FindChild("TopRight/Group/Btn");
            transform.FindChild("Background").gameObject.SetActive(true);
            transform.FindChild("Checked").gameObject.SetActive(false);
        }
    }

    public bool IsFuncBarExpan()
    {
        return (this._FunGroup.transform.localPosition.y <= 50f);
    }

    public bool IsFuncBarShow()
    {
        return this._FunBtn.gameObject.activeSelf;
    }

    private void OnClickBuyPhysical()
    {
        if (ActorData.getInstance().IsSendPak)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x3e3));
        }
        else
        {
            HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ClearSkillEffect();
            }
            if (GUIMgr.Instance.GetGUIEntity<MessageBox>() == null)
            {
                if ((ActorData.getInstance().UserInfo.phyforce_buy_times < ActorData.getInstance().UserInfo.max_phy_buy_times) || (ActorData.getInstance().UserInfo.max_phy_buy_times == -1))
                {
                    if (<>f__am$cache18 == null)
                    {
                        <>f__am$cache18 = delegate (GUIEntity obj) {
                            MessageBox box = (MessageBox) obj;
                            string str = "\n(" + string.Format(ConfigMgr.getInstance().GetWord(0x14b), ActorData.getInstance().UserInfo.phyforce_buy_times) + ")";
                            if (<>f__am$cache1F == null)
                            {
                                <>f__am$cache1F = box => SocketMgr.Instance.RequestRuneStonePurchase(RunestonePurchaseType.E_RPT_PhyForce, 0);
                            }
                            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x39a), ActorData.getInstance().UserInfo.buy_phy_cost, 120) + str, <>f__am$cache1F, null, false);
                        };
                    }
                    GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache18, null);
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896ad));
                }
            }
        }
    }

    public void OnClickLittleHelper(GameObject go)
    {
        if (GuideSystem.MatchEvent(GuideEvent.Duplicate_Daily))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Daily.tag_daily_portal_press_button, null);
        }
        this.HideFunBar();
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().daily_misson)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().daily_misson));
        }
        else
        {
            GUIMgr.Instance.PushGUIEntity("Daily", null);
        }
    }

    private void OnClickTiLi(GameObject go, bool isPress)
    {
        if (isPress)
        {
            this.mIsStart = true;
            this._TiLiTimePanel.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            this._TiLiTimePanel.SetActive(true);
            TweenScale.Begin(this._TiLiTimePanel, 0.1f, Vector3.one).method = UITweener.Method.EaseInOut;
            TweenAlpha.Begin(this._TiLiTimePanel, 0.1f, 1f);
        }
        else
        {
            TweenScale.Begin(this._TiLiTimePanel, 0.1f, Vector3.zero).method = UITweener.Method.EaseInOut;
            TweenAlpha.Begin(this._TiLiTimePanel, 0.1f, 0f);
            this._TiLiTimePanel.SetActive(false);
            this.mIsStart = false;
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        this.createBarOK = true;
    }

    public void OnEnterAchievement(GameObject go)
    {
        if (GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_MissionPortal))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Mission.tag_mission_portal_press_button, null);
        }
        this.HideFunBar();
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().achievement)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().achievement));
        }
        else
        {
            GUIMgr.Instance.PushGUIEntity("AchievementPanel", null);
        }
    }

    public override void OnInitialize()
    {
        Transform transform = base.transform.FindChild("TopRight/FunPanel/Group");
        UIEventListener.Get(transform.FindChild("Hero").gameObject).onClick = new UIEventListener.VoidDelegate(this.OpenHeroPanel);
        UIEventListener.Get(transform.FindChild("Bag").gameObject).onClick = new UIEventListener.VoidDelegate(this.OpenBag);
        UIEventListener.Get(transform.FindChild("AchievementPanel").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnEnterAchievement);
        UIEventListener.Get(transform.FindChild("Fragment").gameObject).onClick = new UIEventListener.VoidDelegate(this.OpenFragmentBag);
        UIEventListener.Get(transform.FindChild("Friend").gameObject).onClick = new UIEventListener.VoidDelegate(this.OpenFriend);
        UIEventListener.Get(transform.FindChild("Task").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLittleHelper);
        this.UpdateStone(false);
        this.UpdateGold(false);
        this.UpdateTiLi(false);
        UIEventListener.Get(base.transform.FindChild("TopLeft/TiLi").gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickTiLi);
        if (<>f__am$cache17 == null)
        {
            <>f__am$cache17 = delegate (object o) {
                if (<>f__am$cache1E == null)
                {
                    <>f__am$cache1E = delegate (GUIEntity s) {
                        GoldTreePanel panel = s as GoldTreePanel;
                        panel.Depth = 400;
                        panel.OpenType = 1;
                    };
                }
                GUIMgr.Instance.DoModelGUI("GoldTreePanel", <>f__am$cache1E, null);
            };
        }
        base.transform.FindChild<UIButton>("PlusButtonGlod").OnUIMouseClick(<>f__am$cache17);
        this._IntervalTime.text = 5.ToString();
    }

    public void OnlyShowFunBtn(bool _show)
    {
        if (_show)
        {
            this.mCurrDepth = base.Depth;
            base.transform.FindChild("TopLeft").gameObject.SetActive(false);
        }
        else
        {
            base.Depth = this.mCurrDepth;
            base.transform.FindChild("TopLeft").gameObject.SetActive(true);
        }
    }

    private void OnOpenGroup()
    {
        if ((GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_Portal) || GuideSystem.MatchEvent(GuideEvent.CardBreak_Portal)) || ((GuideSystem.MatchEvent(GuideEvent.SkillLevelUp_Portal) || GuideSystem.MatchEvent(GuideEvent.CardCombine_Portal)) || GuideSystem.MatchEvent(GuideEvent.Strengthen_Portal)))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Hero.tag_hero_portal_press_floatbtn, null);
        }
        if (GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_MissionPortal))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Mission.tag_mission_press_floatbtn, null);
        }
        if (GuideSystem.MatchEvent(GuideEvent.Medicine_Portal))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Medicine.tag_medicine_portal_press_floatbtn, null);
        }
        if (GuideSystem.MatchEvent(GuideEvent.Duplicate_Daily))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Daily.tag_daily_press_floatbutton, null);
        }
    }

    private void OnOpenGroupTweenFinished()
    {
        this.RequestHeroGeneration();
        if (GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_MissionPortal))
        {
            Transform transform = base.transform.FindChild("TopRight/FunPanel/Group/AchievementPanel");
            if (null != transform)
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Mission.tag_mission_portal_press_button, transform.gameObject);
            }
        }
        if (GuideSystem.MatchEvent(GuideEvent.Medicine_Portal))
        {
            Transform transform2 = base.transform.FindChild("TopRight/FunPanel/Group/Bag");
            if (null != transform2)
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Medicine.tag_medicine_press_function_button, transform2.gameObject);
            }
        }
        if (GuideSystem.MatchEvent(GuideEvent.Duplicate_Daily))
        {
            Transform transform3 = base.transform.FindChild("TopRight/FunPanel/Group/Task");
            if (null != transform3)
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Daily.tag_daily_portal_press_button, transform3.gameObject);
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if ((this.lastShow + 1f) < Time.time)
        {
            this.lastShow = Time.time;
            int getNextCalForceTime = ActorData.getInstance().GetNextCalForceTime;
            int phyForce = ActorData.getInstance().PhyForce;
            int maxPhyForce = ActorData.getInstance().UserInfo.maxPhyForce;
            int num4 = Mathf.Clamp(getNextCalForceTime, 0, 300);
            TitleBar bar = this;
            if (bar != null)
            {
                if (phyForce >= maxPhyForce)
                {
                    bar.ShowCoolDownTime(false);
                }
                else
                {
                    bar.ShowCoolDownTime(true);
                    bar.SetForceCoolDownTime(string.Format("{0:D2}:{1:D2}", num4 / 60, num4 % 60));
                    int num5 = num4 + (((maxPhyForce - phyForce) - 1) * 300);
                    if (num5 > 0xe10)
                    {
                        bar.SetForceCoolDownAllTime(string.Format("{0:D2}:{1:D2}:{2:D2}", num5 / 0xe10, (num5 % 0xe10) / 60, num5 % 60));
                    }
                    else
                    {
                        bar.SetForceCoolDownAllTime(string.Format("{0:D2}:{1:D2}", num5 / 60, num5 % 60));
                    }
                }
            }
        }
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                DateTime serverDateTime = TimeMgr.Instance.ServerDateTime;
                this._TiLiTimeLabel.text = string.Format("{0:D2}:{1:D2}:{2:D2}", serverDateTime.Hour, serverDateTime.Minute, serverDateTime.Second);
                this.CheckBtnTips();
            }
        }
        if (this.createBarOK)
        {
            GuideSystem.FireEvent(GuideEvent.EquipLevelUp_Portal);
            GuideSystem.FireEvent(GuideEvent.CardCombine_Portal);
            this.FireNewbieMissionGuide();
            this.FireNewbieCardBreakGuide();
            this.FireNewbieSkillLevelupGuide();
            this.FireNewbieStrengthenGuide();
            this.FireNewbieMedicineGuide();
            GuideSystem.FireEvent(GuideEvent.Function_Guild);
            GuideSystem.FireEvent(GuideEvent.Function_Shop);
            GuideSystem.FireEvent(GuideEvent.Function_Dungeons);
            GuideSystem.FireEvent(GuideEvent.Function_Arena);
            GuideSystem.FireEvent(GuideEvent.Function_Outland);
            GuideSystem.FireEvent(GuideEvent.Function_LifeSkill);
            GuideSystem.FireEvent(GuideEvent.Function_Guild);
            GuideSystem.FireEvent(GuideEvent.Function_Expedition);
            GuideSystem.FireEvent(GuideEvent.Function_Tower);
            GuideSystem.FireEvent(GuideEvent.Duplicate_Elite);
            GuideSystem.FireEvent(GuideEvent.Duplicate_Daily);
        }
    }

    private void OpenBag(GameObject go)
    {
        if (GuideSystem.MatchEvent(GuideEvent.Medicine_Portal))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Medicine.tag_medicine_press_function_button, null);
        }
        this.HideFunBar();
        ActorData.getInstance().IsPopPanel = false;
        if (GUIMgr.Instance.GetActivityGUIEntity<DetailsPanel>() != null)
        {
            GUIMgr.Instance.PopGUIEntity();
        }
        BagPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<BagPanel>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.InitItemList(ShowItemType.All);
            activityGUIEntity.SelectDefaultCheckbox();
        }
        else
        {
            if (<>f__am$cache1B == null)
            {
                <>f__am$cache1B = obj => ((BagPanel) obj).SelectDefaultCheckbox();
            }
            GUIMgr.Instance.PushGUIEntity("BagPanel", <>f__am$cache1B);
        }
    }

    private void OpenChongZhi()
    {
        if (<>f__am$cache19 == null)
        {
            <>f__am$cache19 = obj => VipCardPanel panel = (VipCardPanel) obj;
        }
        GUIMgr.Instance.PushGUIEntity("VipCardPanel", <>f__am$cache19);
    }

    private void OpenFragmentBag(GameObject go)
    {
        this.HideFunBar();
        if (GUIMgr.Instance.GetActivityGUIEntity<DetailsPanel>() != null)
        {
            GUIMgr.Instance.PopGUIEntity();
        }
        ActorData.getInstance().IsPopPanel = false;
        FragmentBagPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FragmentBagPanel>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.InitItemList(ShowItemType.All);
            activityGUIEntity.SelectDefaultCheckbox();
        }
        else
        {
            if (<>f__am$cache1C == null)
            {
                <>f__am$cache1C = obj => ((FragmentBagPanel) obj).SelectDefaultCheckbox();
            }
            GUIMgr.Instance.PushGUIEntity("FragmentBagPanel", <>f__am$cache1C);
        }
    }

    public void OpenFriend(GameObject go)
    {
        this.HideFunBar();
        ActorData.getInstance().FriendPkEndReturn = false;
        if (<>f__am$cache1A == null)
        {
            <>f__am$cache1A = obj => FriendPanel panel = (FriendPanel) obj;
        }
        GUIMgr.Instance.PushGUIEntity("FriendPanel", <>f__am$cache1A);
    }

    private void OpenHeroPanel(GameObject go)
    {
        if ((GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_Portal) || GuideSystem.MatchEvent(GuideEvent.CardBreak_Portal)) || ((GuideSystem.MatchEvent(GuideEvent.SkillLevelUp_Portal) || GuideSystem.MatchEvent(GuideEvent.CardCombine_Portal)) || GuideSystem.MatchEvent(GuideEvent.Strengthen_Portal)))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Hero.tag_hero_portal_press_function_button, null);
        }
        this.HideFunBar();
        if (GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>() != null)
        {
            GUIMgr.Instance.PopGUIEntity();
        }
        else if (GUIMgr.Instance.GetActivityGUIEntity<BreakEquipPanel>() != null)
        {
            GUIMgr.Instance.PopGUIEntity();
            GUIMgr.Instance.PopGUIEntity();
        }
        if (<>f__am$cache1D == null)
        {
            <>f__am$cache1D = delegate (GUIEntity obj) {
                HeroPanel panel = (HeroPanel) obj;
                SocketMgr.Instance.RequestGetSkillPoint();
            };
        }
        GUIMgr.Instance.PushGUIEntity("HeroPanel", <>f__am$cache1D);
    }

    public void RequestBagGeneration()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Medicine_Portal))
        {
            Transform transform = base.transform.FindChild("TopRight/FunPanel/Group/Bag");
            if (null != transform)
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Medicine.tag_medicine_press_function_button, transform.gameObject);
            }
        }
    }

    public void RequestDailyGeneration()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Duplicate_Daily))
        {
            Transform transform = base.transform.FindChild("TopRight/FunPanel/Group/Task");
            if (null != transform)
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Daily.tag_daily_portal_press_button, transform.gameObject);
            }
        }
    }

    public void RequestHeroGeneration()
    {
        if ((GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_Portal) || GuideSystem.MatchEvent(GuideEvent.CardBreak_Portal)) || ((GuideSystem.MatchEvent(GuideEvent.SkillLevelUp_Portal) || GuideSystem.MatchEvent(GuideEvent.CardCombine_Portal)) || GuideSystem.MatchEvent(GuideEvent.Strengthen_Portal)))
        {
            Transform transform = base.transform.FindChild("TopRight/FunPanel/Group/Hero");
            if (null != transform)
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Hero.tag_hero_portal_press_function_button, transform.gameObject);
            }
        }
    }

    public void RequestMissionGeneration()
    {
        if (GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_MissionPortal))
        {
            Transform transform = base.transform.FindChild("TopRight/FunPanel/Group/AchievementPanel");
            if (null != transform)
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Mission.tag_mission_portal_press_button, transform.gameObject);
            }
        }
    }

    public void SetForceCoolDownAllTime(string txt)
    {
        this._CooldownAllTime.text = txt;
    }

    public void SetForceCoolDownTime(string txt)
    {
        this._CooldownTime.text = txt;
    }

    public void ShowCoolDownTime(bool enable)
    {
        if (!enable)
        {
            this._CooldownTime.text = "00:00";
            this._CooldownAllTime.text = "00:00";
        }
    }

    public void ShowFuncBar(bool _show)
    {
    }

    public void ShowTitleBar(bool _show)
    {
        base.transform.FindChild("TopRight").gameObject.SetActive(_show);
        base.transform.FindChild("TopLeft").gameObject.SetActive(_show);
    }

    public void UpdateGold(bool _isPlayAmin)
    {
        this.GoldLabel.text = (ActorData.getInstance().Gold > 0) ? ActorData.getInstance().Gold.ToString("#,###") : "0";
        if (_isPlayAmin)
        {
            base.StartCoroutine(this.ChangeNumberAmin(this.GoldLabel));
        }
    }

    public void UpdateStone(bool _isPlayAmin)
    {
        this.StoneLabel.text = (ActorData.getInstance().Stone > 0) ? ActorData.getInstance().Stone.ToString("#,###") : "0";
        if (_isPlayAmin)
        {
            base.StartCoroutine(this.ChangeNumberAmin(this.StoneLabel));
        }
    }

    public void UpdateTiLi(bool _isPlayAmin)
    {
        this.TiLiLabel.text = ActorData.getInstance().PhyForce + "/" + ActorData.getInstance().UserInfo.maxPhyForce;
        this._BuyCount.text = ActorData.getInstance().UserInfo.phyforce_buy_times.ToString();
        if (_isPlayAmin)
        {
            base.StartCoroutine(this.ChangeNumberAmin(this.TiLiLabel));
        }
    }

    private UILabel GoldLabel
    {
        get
        {
            if (null == this.lblGold)
            {
                this.lblGold = base.transform.FindChild("TopLeft/Gold/Num/Label").GetComponent<UILabel>();
            }
            return this.lblGold;
        }
    }

    public static TitleBar Instance
    {
        get
        {
            return (TitleBar) GUIMgr.Instance.GetGUIEntity("TitleBar");
        }
    }

    private UILabel StoneLabel
    {
        get
        {
            if (null == this.lblStone)
            {
                this.lblStone = base.transform.FindChild("TopLeft/Stone/Num/Label").GetComponent<UILabel>();
            }
            return this.lblStone;
        }
    }

    private UILabel TiLiLabel
    {
        get
        {
            if (null == this.lblTiLi)
            {
                this.lblTiLi = base.transform.FindChild("TopLeft/TiLi/Num/Label").GetComponent<UILabel>();
            }
            return this.lblTiLi;
        }
    }

    [CompilerGenerated]
    private sealed class <ChangeNumberAmin>c__Iterator95 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal UILabel _num;
        internal UILabel <$>_num;

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
                    object[] args = new object[] { "scale", new Vector3(1.5f, 1.5f, 1.5f), "time", 0.2f, "easetype", iTween.EaseType.spring };
                    iTween.ScaleTo(this._num.gameObject, iTween.Hash(args));
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    goto Label_013A;
                }
                case 1:
                {
                    object[] objArray2 = new object[] { "scale", Vector3.one, "time", 0.2f, "easetype", iTween.EaseType.spring };
                    iTween.ScaleTo(this._num.gameObject, iTween.Hash(objArray2));
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 2;
                    goto Label_013A;
                }
                case 2:
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_013A;

                case 3:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_013A:
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
}

