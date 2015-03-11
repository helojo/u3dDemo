using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class GuildPanel : GUIEntity
{
    public UIButton _bindQQBtn;
    public UILabel _bindQQTxt;
    public UILabel _qqGroupNameTxt;
    public UILabel _qqGroupTxt;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache10;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache11;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache12;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheE;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheF;
    public List<GameObject> FuncList = new List<GameObject>();
    public bool m_bClickBind;
    public bool m_bClickJoin;
    public bool m_bClickUnBind;
    public bool m_bSend;
    private int m_nFlag = -1;
    public UITableManager<MemberItem> MemberTable = new UITableManager<MemberItem>();
    private GuildData mGuildData;
    private GuildMemberData mGuildMemberData;
    private UserGuildMemberData mUserGuildData;

    public static void BindQQGroupCallback()
    {
        GuildPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuildPanel>();
        if ((activityGUIEntity != null) && (activityGUIEntity.m_bClickBind || activityGUIEntity.m_bClickJoin))
        {
            Debug.Log("------------------BindQQGroupCallback");
            SocketMgr.Instance.RequestGuildBindQQInfo();
        }
    }

    public void CheckGuildApplyStat()
    {
        base.transform.FindChild("MgrBtn/New").gameObject.SetActive(ActorData.getInstance().GuildApplyList.Count > 0);
    }

    private void ClickBindQQ()
    {
        if (!this.m_bSend)
        {
            Debug.Log("ClickBindQQ -- " + this.m_nFlag);
            if (this.m_nFlag == 0)
            {
                PlatformInterface.mInstance.PlatformBindQQGroup(ActorData.getInstance().mGuildData.id + string.Empty, ActorData.getInstance().mGuildData.name);
                this.m_bClickBind = true;
            }
            else if (this.m_nFlag == 1)
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", e => ((MessageBox) e).SetDialog(ConfigMgr.getInstance().GetWord(0x4e63), delegate (GameObject go) {
                    SocketMgr.Instance.RequestGuildUnBindQQGroup();
                    this.m_bClickUnBind = true;
                }, null, false), null);
            }
            else if (this.m_nFlag == 2)
            {
                SocketMgr.Instance.RequestGuildJoinQQGroup();
                this.m_bClickJoin = true;
            }
            this.m_bSend = true;
        }
    }

    private void ClickContribution()
    {
        if (this.mUserGuildData.contribute_times >= 1)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa65290));
        }
        else
        {
            if (<>f__am$cache11 == null)
            {
                <>f__am$cache11 = delegate (GUIEntity obj) {
                    GuildContributePanel panel = (GuildContributePanel) obj;
                    panel.Depth = 400;
                };
            }
            GUIMgr.Instance.DoModelGUI("GuildContributePanel", <>f__am$cache11, base.gameObject);
        }
    }

    private void ClickExistBtn()
    {
        if (<>f__am$cache10 == null)
        {
            <>f__am$cache10 = delegate (GUIEntity obj) {
                if (<>f__am$cache12 == null)
                {
                    <>f__am$cache12 = go => SocketMgr.Instance.RequestGuildQuit();
                }
                obj.Achieve<MessageBox>().SetDialog(ConfigMgr.getInstance().GetWord(0xa6528f), <>f__am$cache12, null, false);
            };
        }
        GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache10, base.gameObject);
    }

    private void ClickMgrBtn()
    {
        this.OpenGuildMgrPanel();
    }

    public void ClosePanel()
    {
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
    }

    private List<GuildMember> GetHaveHongBaoMember()
    {
        List<GuildMember> list = new List<GuildMember>();
        if (ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_guild_friend_can_draw)
        {
            foreach (GuildMember member in ActorData.getInstance().mGuildMemberData.member)
            {
                if (!member.userInfo.redpackage_isdraw && (member.userInfo.redpackage_num > 0))
                {
                    list.Add(member);
                }
            }
        }
        return list;
    }

    public void GetNewData()
    {
        this.mGuildData = ActorData.getInstance().mGuildData;
        this.mUserGuildData = ActorData.getInstance().mUserGuildMemberData;
        this.mGuildMemberData = ActorData.getInstance().mGuildMemberData;
    }

    private void OnClickBattle(GameObject obj)
    {
        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xcc));
    }

    private void OnClickDup(GameObject go)
    {
        if (<>f__am$cacheE == null)
        {
            <>f__am$cacheE = u => GuildDupList list = u as GuildDupList;
        }
        GUIMgr.Instance.PushGUIEntity<GuildDupList>(<>f__am$cacheE);
    }

    private void OnClickHall(GameObject obj)
    {
        GUIMgr.Instance.PushGUIEntity("GuildHallPanel", null);
    }

    private void OnClickMebmer(MemberItem item)
    {
        <OnClickMebmer>c__AnonStorey203 storey = new <OnClickMebmer>c__AnonStorey203 {
            item = item,
            <>f__this = this
        };
        if (storey.item != null)
        {
            GUIMgr.Instance.DoModelGUI<GuildMemberCtrlDlag>(new Action<GUIEntity>(storey.<>m__374), base.gameObject);
        }
    }

    private void OnClickPaoKu(GameObject obj)
    {
        GUIMgr.Instance.PushGUIEntity("PaokuPanel", null);
    }

    private void OnClickRanking(GameObject obj)
    {
        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xcc));
    }

    private void OnClickShop(GameObject obj)
    {
        GUIMgr.Instance.PushGUIEntity("GuildShopPanel", null);
    }

    private void OnClickTech(GameObject obj)
    {
        GUIMgr.Instance.PushGUIEntity("GuildTechPanel", null);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        CommonFunc.ResetClippingPanel(base.transform.FindChild("List"));
        this.UpdateData();
        this.UpdateGuildLevel();
        this.UpdateGuildMember();
        GUIMgr.Instance.FloatTitleBar();
        if ((ActorData.getInstance().mUserGuildMemberData != null) && (ActorData.getInstance().mUserGuildMemberData.position == 1))
        {
            SocketMgr.Instance.RequestGuildApplication();
        }
        if (GameDefine.getInstance().GetTencentType() == TencentType.QQ)
        {
            this._bindQQBtn.gameObject.SetActive(false);
            this._qqGroupNameTxt.gameObject.SetActive(false);
            this._qqGroupTxt.gameObject.SetActive(false);
        }
        if (GameDefine.getInstance().GetTencentType() == TencentType.QQ)
        {
            SocketMgr.Instance.RequestGuildBindQQInfo();
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        UIGrid component = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        this.MemberTable.InitFromGrid(component);
        this.MemberTable.InitFromGrid(component);
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
        this.m_bClickBind = false;
        this.m_bClickUnBind = false;
        this.m_bClickJoin = false;
        this.m_bSend = false;
    }

    public void OpenGuildMgrPanel()
    {
        if (<>f__am$cacheF == null)
        {
            <>f__am$cacheF = delegate (GUIEntity obj) {
                GuildMgrPanel panel = (GuildMgrPanel) obj;
                panel.Depth = 400;
                panel.Init();
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildMgrPanel", <>f__am$cacheF, base.gameObject);
    }

    private void RequestGuildApply()
    {
        SocketMgr.Instance.RequestGuildApplication();
    }

    private void ResetClipViewport()
    {
        GUIMgr.Instance.ListRoot.gameObject.SetActive(true);
        Transform top = base.transform.FindChild("ListTopLeft");
        Transform bottom = base.transform.FindChild("ListBottomRight");
        Transform bounds = base.transform.FindChild("List");
        GUIMgr.Instance.ResetListViewpot(top, bottom, bounds, 0f);
    }

    private void UpdataFuncList()
    {
        CommonFunc.ResetClippingPanel(base.transform.FindChild("FuncList"));
        this.FuncList[1].transform.FindChild("Label").GetComponent<UILabel>().text = "Lv " + (this.mGuildData.tech.shop_level + 1);
        this.FuncList[0].transform.FindChild("Label").GetComponent<UILabel>().text = "Lv " + (this.mGuildData.tech.warmill_level + 1);
        UIEventListener.Get(this.FuncList[0]).onClick = new UIEventListener.VoidDelegate(this.OnClickTech);
        UIEventListener.Get(this.FuncList[1]).onClick = new UIEventListener.VoidDelegate(this.OnClickShop);
        UIEventListener.Get(this.FuncList[2]).onClick = new UIEventListener.VoidDelegate(this.OnClickDup);
        UIEventListener.Get(this.FuncList[3]).onClick = new UIEventListener.VoidDelegate(this.OnClickPaoKu);
        UIEventListener.Get(this.FuncList[4]).onClick = new UIEventListener.VoidDelegate(this.OnClickBattle);
        UIEventListener.Get(this.FuncList[5]).onClick = new UIEventListener.VoidDelegate(this.OnClickRanking);
    }

    public void UpdateContribution()
    {
        base.gameObject.transform.FindChild("Contribution/Val").GetComponent<UILabel>().text = this.mGuildData.cur_contribution.ToString();
        base.gameObject.transform.FindChild("Contribution/PlayerCon/Val").GetComponent<UILabel>().text = ActorData.getInstance().mUserGuildMemberData.contribution_of_day + "/" + GameConstValues.MAX_GUILD_CONTRIBUTION_OF_DAY;
    }

    public void UpdateData()
    {
        this.GetNewData();
        if (this.mUserGuildData.position == 1)
        {
            base.gameObject.transform.FindChild("MgrBtn").gameObject.SetActive(true);
            base.gameObject.transform.FindChild("ExistBtn").gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.transform.FindChild("MgrBtn").gameObject.SetActive(false);
            base.gameObject.transform.FindChild("ExistBtn").gameObject.SetActive(true);
        }
        this.UpdateMemberCount();
        this.UpdateNameAndID();
        this.UpdateNotice();
        this.UpdateContribution();
        this.UpdataFuncList();
    }

    public void UpdateFuncListLev()
    {
        this.FuncList[1].transform.FindChild("Label").GetComponent<UILabel>().text = "Lv " + (this.mGuildData.tech.shop_level + 1);
        this.FuncList[0].transform.FindChild("Label").GetComponent<UILabel>().text = "Lv " + (this.mGuildData.tech.warmill_level + 1);
    }

    public void UpdateGuildBindQQInfo(string strOpenId, string strQQGroupName, bool bMember)
    {
        this.m_bSend = false;
        bool flag = (ActorData.getInstance().mUserGuildMemberData != null) && (ActorData.getInstance().mUserGuildMemberData.position == 1);
        Debug.Log(string.Concat(new object[] { " UpdateGuildBindQQInfo------ ", strOpenId, ", ", strQQGroupName, ", ", bMember }));
        if (strOpenId == string.Empty)
        {
            this._bindQQBtn.gameObject.SetActive(flag);
            this._bindQQTxt.text = ConfigMgr.getInstance().GetWord(0x4e5a);
            this._qqGroupNameTxt.text = ConfigMgr.getInstance().GetWord(0x4e5d);
            this.m_nFlag = !flag ? 4 : 0;
        }
        else
        {
            if (flag)
            {
                this._bindQQTxt.text = ConfigMgr.getInstance().GetWord(0x4e5b);
                this.m_nFlag = 1;
                this._bindQQBtn.gameObject.SetActive(true);
            }
            else if (!bMember)
            {
                this._bindQQTxt.text = ConfigMgr.getInstance().GetWord(0x4e5c);
                this.m_nFlag = 2;
                this._bindQQBtn.gameObject.SetActive(true);
            }
            else
            {
                this.m_nFlag = 3;
                this._bindQQBtn.gameObject.SetActive(false);
                this._qqGroupTxt.gameObject.SetActive(true);
            }
            this._qqGroupNameTxt.text = strQQGroupName;
            if (strQQGroupName == string.Empty)
            {
                this._qqGroupNameTxt.text = ConfigMgr.getInstance().GetWord(0x4e5d);
            }
            if (strQQGroupName.Length > 6)
            {
                this._qqGroupNameTxt.text = strQQGroupName.Substring(0, 5) + "..";
            }
        }
        if ((this.m_bClickBind && flag) && (strOpenId != string.Empty))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e5e));
            this.m_bClickBind = false;
        }
        else if ((this.m_bClickJoin && bMember) && !flag)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e60));
            this.m_bClickJoin = false;
        }
        this._qqGroupNameTxt.gameObject.SetActive(true);
    }

    public void UpdateGuildLevel()
    {
        if (ActorData.getInstance().mGuildData != null)
        {
            base.transform.FindChild("GuildLevel/val").GetComponent<UILabel>().text = (ActorData.getInstance().mGuildData.tech.hall_level + 1).ToString();
        }
        else
        {
            base.transform.FindChild("GuildLevel/val").GetComponent<UILabel>().text = string.Empty;
        }
    }

    public void UpdateGuildMember()
    {
        this.ResetClipViewport();
        this.UpdateMemberCount();
        this.MemberTable.Count = ActorData.getInstance().mGuildMemberData.member.Count;
        for (int i = 0; i < this.MemberTable.Count; i++)
        {
            MemberItem item = this.MemberTable[i];
            item.ItemData = ActorData.getInstance().mGuildMemberData.member[i];
            item.OnClick = new Action<MemberItem>(this.OnClickMebmer);
        }
    }

    public void UpdateGuildUnBindQQInfo()
    {
        this._bindQQBtn.gameObject.SetActive(true);
        this._bindQQTxt.text = ConfigMgr.getInstance().GetWord(0x4e5a);
        this._qqGroupNameTxt.gameObject.SetActive(true);
        this._qqGroupNameTxt.text = ConfigMgr.getInstance().GetWord(0x4e5d);
        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e5f));
        this.m_bClickUnBind = false;
        this.m_nFlag = 0;
        this.m_bSend = false;
    }

    public void UpdateMemberCount()
    {
        guild_hall_config _config = ConfigMgr.getInstance().getByEntry<guild_hall_config>((int) this.mGuildData.tech.hall_level);
        if (_config != null)
        {
            base.gameObject.transform.FindChild("MemberCnt/Cnt").GetComponent<UILabel>().text = this.mGuildMemberData.member.Count + "/" + _config.member_limit;
        }
    }

    public void UpdateMemberInfo(long memberId)
    {
        <UpdateMemberInfo>c__AnonStorey204 storey = new <UpdateMemberInfo>c__AnonStorey204 {
            memberId = memberId
        };
        IEnumerator<MemberItem> enumerator = this.MemberTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                MemberItem current = enumerator.Current;
                if (current.ItemData.userInfo.id == storey.memberId)
                {
                    GuildMember member = ActorData.getInstance().mGuildMemberData.member.Find(new Predicate<GuildMember>(storey.<>m__37A));
                    if (member != null)
                    {
                        current.ItemData = member;
                    }
                    return;
                }
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

    private void UpdateNameAndID()
    {
        base.gameObject.transform.FindChild("Title/Name").GetComponent<UILabel>().text = this.mGuildData.name;
        base.gameObject.transform.FindChild("ID/val").GetComponent<UILabel>().text = this.mGuildData.id.ToString();
    }

    public void UpdateNotice()
    {
        base.gameObject.transform.FindChild("Notice/Label").GetComponent<UILabel>().text = this.mGuildData.notice;
    }

    [CompilerGenerated]
    private sealed class <OnClickMebmer>c__AnonStorey203
    {
        internal GuildPanel <>f__this;
        internal GuildPanel.MemberItem item;

        internal void <>m__374(GUIEntity obj)
        {
            ((GuildMemberCtrlDlag) obj).UpdateData(this.item.ItemData, this.<>f__this.GetHaveHongBaoMember());
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateMemberInfo>c__AnonStorey204
    {
        internal long memberId;

        internal bool <>m__37A(GuildMember t)
        {
            return (t.userInfo.id == this.memberId);
        }
    }

    public class MemberItem : UITableItem
    {
        private GuildMember _item;
        public UIDragCamera Drag;
        private UILabel GuildJob;
        private UISprite HongBao;
        private UITexture Icon;
        private UILabel LastOnlineTime;
        private UILabel Level;
        private UILabel Name;
        public Action<GuildPanel.MemberItem> OnClick;
        private UISprite Qframe;
        private UISprite QIconSprite;
        private UILabel SingleContribute;

        public override void OnCreate()
        {
            this.Icon = base.Root.FindChild<UITexture>("Icon");
            this.GuildJob = base.Root.FindChild<UILabel>("GuildJob");
            this.LastOnlineTime = base.Root.FindChild<UILabel>("LastOnlineTime");
            this.Name = base.Root.FindChild<UILabel>("Name");
            this.Level = base.Root.FindChild<UILabel>("Level");
            this.Qframe = base.Root.FindChild<UISprite>("Qframe");
            this.QIconSprite = base.Root.FindChild<UISprite>("QIconSprite");
            this.SingleContribute = base.Root.FindChild<UILabel>("Contribute");
            this.HongBao = base.Root.FindChild<UISprite>("HongBao");
            base.Root.OnUIMouseClick(delegate (object u) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
        }

        public GuildMember ItemData
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(value.userInfo.head_entry);
                if (_config != null)
                {
                    this.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                }
                CommonFunc.SetPlayerHeadFrame(this.Qframe, this.QIconSprite, value.userInfo.head_frame_entry);
                if ((ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_guild_friend_can_draw) && (ActorData.getInstance().SessionInfo.userid != value.userInfo.id))
                {
                    this.HongBao.gameObject.SetActive(!value.userInfo.redpackage_isdraw && (value.userInfo.redpackage_num > 0));
                }
                else
                {
                    this.HongBao.gameObject.SetActive(false);
                }
                this.LastOnlineTime.text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(value.userInfo.lastOnlineTime));
                if (value.position == 1)
                {
                    this.GuildJob.text = ConfigMgr.getInstance().GetWord(0xa652bb);
                }
                else
                {
                    this.GuildJob.text = ConfigMgr.getInstance().GetWord(0xa652bc);
                }
                this.Name.text = value.userInfo.name;
                this.Level.text = "LV." + value.userInfo.level;
                this.SingleContribute.text = value.userInfo.contribution_of_day.ToString();
            }
        }
    }
}

