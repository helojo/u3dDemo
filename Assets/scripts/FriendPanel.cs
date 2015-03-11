using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Toolbox;
using UnityEngine;

public class FriendPanel : GUIEntity
{
    public GameObject _AddGroup;
    public UIDraggableCamera _DragCamera;
    public GameObject _FrinedGroup;
    public Transform _FrinedList_bottom_right;
    public Transform _FrinedList_top_left;
    private List<Friend> _gameFriends;
    public string _MsgText = "MT2太棒了！你也快来和我一起玩吧！";
    public UILabel _PageLabel;
    public GameObject _QQFriendGroup;
    private UILabel _RefreshListLabel;
    public GameObject _SearchGroup;
    public string _TipsText = "邀请你体验超爽游戏。";
    [CompilerGenerated]
    private static Action<object> <>f__am$cache24;
    [CompilerGenerated]
    private static Func<SocialUser, bool> <>f__am$cache25;
    [CompilerGenerated]
    private static Predicate<Friend> <>f__am$cache26;
    [CompilerGenerated]
    private static Comparison<Friend> <>f__am$cache27;
    public Transform bottom_right;
    private UIButton bt_receiveAll;
    public UITableManager<FriendTableItem> FriendTable = new UITableManager<FriendTableItem>();
    public GameObject InfoPanel;
    public Transform List_F;
    public Transform List_Q;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private int MaxPageCount = 10;
    private int mCurrPage;
    private bool mDirty;
    private int mFriendSumPage;
    private bool mIsStart;
    private ListType mListType = ListType.QQ_FRIENDLIST;
    private PageIndex<List<SocialUser>> PIQQFriend;
    public UILabel QFriendCount;
    private UILabel QQFriendCount;
    public UITableManager<QQFriendItem> QQFriendTable = new UITableManager<QQFriendItem>();
    public GameObject SingleFriendItem;
    public GameObject SingleQQFriendItem;
    public Transform top_left;

    private void AttachListDraggable(GameObject list_item)
    {
        UIDragCamera component = list_item.GetComponent<UIDragCamera>();
        if (null != component)
        {
            component.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
        }
    }

    private void Awake()
    {
    }

    private void CheckShareBtn()
    {
        Transform transform = base.transform.FindChild("QQFriendGroup/YaoQinBtn");
        if (transform != null)
        {
            transform.gameObject.SetActive(SharePanel.IsCanShare());
        }
    }

    private List<Friend> GetHaveHongBaoFriendList()
    {
        List<Friend> list = new List<Friend>();
        if (ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_game_friend_can_draw)
        {
            foreach (Friend friend in this.GameFriends)
            {
                if (!friend.redpackage_isdraw && (friend.redpackage_num > 0))
                {
                    list.Add(friend);
                }
            }
        }
        return list;
    }

    private List<SocialUser> GetSocialUserList()
    {
        List<SocialUser> list = new List<SocialUser>();
        if (ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_plat_friend_can_draw)
        {
            for (int i = this.PIQQFriend.Start; i < this.PIQQFriend.End; i++)
            {
                SocialUser item = this.PIQQFriend.List[i];
                if (!item.QQUser.redpackage_isdraw && (item.QQUser.redpackage_num > 0))
                {
                    list.Add(item);
                }
            }
        }
        return list;
    }

    private bool HaveQQTiLi()
    {
        <HaveQQTiLi>c__AnonStorey1E8 storeye = new <HaveQQTiLi>c__AnonStorey1E8 {
            have = false
        };
        XSingleton<SocialFriend>.Singleton.Each(new SocialFriend.EachCondtion(storeye.<>m__310));
        return storeye.have;
    }

    private bool HaveTiLi()
    {
        foreach (Friend friend in ActorData.getInstance().FriendList)
        {
            if (friend.isGivePhyForceNow)
            {
                return true;
            }
        }
        return false;
    }

    [DebuggerHidden]
    public IEnumerator InitFriendList(int page)
    {
        return new <InitFriendList>c__Iterator7B { page = page, <$>page = page, <>f__this = this };
    }

    private void InitGuicontrolEvent()
    {
        this._RefreshListLabel = this._FrinedGroup.transform.FindChild("RefreshBtn/Label").GetComponent<UILabel>();
        UIEventListener.Get(this._FrinedGroup.transform.FindChild("RefreshBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRefreshBtn);
        UIEventListener.Get(this._FrinedGroup.transform.FindChild("GetTiLiBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAllGetTiLi);
        UIEventListener.Get(this._FrinedGroup.transform.FindChild("UpdateBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickUpdateBtn);
        UIEventListener.Get(this._FrinedGroup.transform.FindChild("bt_SendAll").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOneKeySendAllBtn);
        UIEventListener.Get(this._QQFriendGroup.transform.FindChild("bt_SendAll").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickQQOneKeySendAllBtn);
        UIEventListener.Get(base.transform.FindChild("NewTab/QQFriend").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickQQFriendBtn);
        UIEventListener.Get(base.transform.FindChild("NewTab/MyFriend").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMyFriendBtn);
        UIEventListener.Get(this._FrinedGroup.transform.FindChild("SearchBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMySearchBtn);
        UIEventListener.Get(this._FrinedGroup.transform.FindChild("RequestBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMyRequestBtn);
        this.bt_next.OnUIMouseClick(delegate (object u) {
            if (this.PIQQFriend != null)
            {
                base.StartCoroutine(this.ShowQQFriendByPage(this.PIQQFriend.NextPage));
            }
        });
        this.bt_prv.OnUIMouseClick(delegate (object u) {
            if (this.PIQQFriend != null)
            {
                base.StartCoroutine(this.ShowQQFriendByPage(this.PIQQFriend.PrePage));
            }
        });
        this.UpdateSignature();
        this.SetFriendTips();
    }

    private void OnClickAddBtn(GameObject go)
    {
        this._AddGroup.gameObject.SetActive(true);
        this._SearchGroup.gameObject.SetActive(false);
        this._FrinedGroup.gameObject.SetActive(false);
        this.UpdateRequestList();
    }

    private void OnClickAllAgreeBtn(GameObject go)
    {
        if (ActorData.getInstance().FriendList.Count >= ActorData.getInstance().MaxFriendCount)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(970));
        }
        else if (ActorData.getInstance().FriendReqList.Count > 0)
        {
            SocketMgr.Instance.RequestAgreeAllFriend();
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b1) + "0");
        }
    }

    private void OnClickAllGetTiLi(GameObject obj)
    {
        if (!this.HaveTiLi())
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896ac));
        }
        else if (ActorData.getInstance().UserInfo.remainPhyForceAccept <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b3));
        }
        else if (ActorData.getInstance().FriendList.Count > 0)
        {
            SocketMgr.Instance.RequestAcceptAllFriendPhyForce();
        }
    }

    private void OnClickFriendBtn(GameObject go)
    {
        this._AddGroup.gameObject.SetActive(false);
        this._SearchGroup.gameObject.SetActive(false);
        this._FrinedGroup.gameObject.SetActive(true);
        this.mListType = ListType.FRIENDLIST;
        if (this.mDirty)
        {
            this.UpdateList();
        }
    }

    private void OnClickGetTiLiBtn(FriendTableItem item)
    {
        Friend itemData = item.ItemData;
        if (ActorData.getInstance().UserInfo.remainPhyForceAccept <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b3));
        }
        else
        {
            SocketMgr.Instance.RequestAcceptFriendPhyForce(itemData.userInfo.id);
        }
    }

    private void OnClickGetTiLiBtn(GameObject go)
    {
        if (ActorData.getInstance().UserInfo.remainPhyForceAccept <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b3));
        }
        else
        {
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                Friend friend = (Friend) obj2;
                if (friend != null)
                {
                    SocketMgr.Instance.RequestAcceptFriendPhyForce(friend.userInfo.id);
                }
            }
        }
    }

    private void OnClickItemBtn(FriendTableItem item)
    {
        <OnClickItemBtn>c__AnonStorey1E6 storeye = new <OnClickItemBtn>c__AnonStorey1E6 {
            <>f__this = this,
            info = item.ItemData
        };
        FriendInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<FriendInfoPanel>();
        if (gUIEntity != null)
        {
            gUIEntity.UpdateData(storeye.info, this.GetHaveHongBaoFriendList());
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("FriendInfoPanel", new Action<GUIEntity>(storeye.<>m__30E), base.gameObject);
        }
        SoundManager.mInstance.PlaySFX("sound_ui_t_8");
    }

    private void OnClickItemBtn(GameObject go)
    {
        <OnClickItemBtn>c__AnonStorey1E7 storeye = new <OnClickItemBtn>c__AnonStorey1E7 {
            go = go,
            <>f__this = this
        };
        GUIMgr.Instance.DoModelGUI("FriendInfoPanel", new Action<GUIEntity>(storeye.<>m__30F), base.gameObject);
    }

    private void OnClickLeft()
    {
        this.ResetFriendSumPage();
        this.mCurrPage--;
        if (this.mCurrPage < 0)
        {
            this.mCurrPage = (this.mFriendSumPage > 0) ? (this.mFriendSumPage - 1) : 0;
        }
        base.StopAllCoroutines();
        base.StartCoroutine(this.InitFriendList(this.mCurrPage));
    }

    private void OnClickMyFriendBtn(GameObject go)
    {
        this.mListType = ListType.FRIENDLIST;
        this._FrinedGroup.gameObject.SetActive(true);
        base.StartCoroutine(this.InitFriendList(this.mCurrPage));
        this._QQFriendGroup.gameObject.SetActive(false);
    }

    private void OnClickMyRequestBtn(GameObject go)
    {
        GUIMgr.Instance.PushGUIEntity("AddFriendPanel", null);
    }

    private void OnClickMySearchBtn(GameObject go)
    {
        GUIMgr.Instance.PushGUIEntity("AddFriendPanel", null);
    }

    private void OnClickOneKeySendAllBtn(GameObject go)
    {
        if (this.GameFriends.Count < 1)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x59a));
        }
        else
        {
            if (<>f__am$cache26 == null)
            {
                <>f__am$cache26 = e => !e.alreadyGivePhyForceToday;
            }
            if (this.GameFriends.Find(<>f__am$cache26) == null)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x599));
            }
            else
            {
                SocketMgr.Instance.RequestGiveFriendPhyForce(0L);
            }
        }
    }

    public void OnClickQQAllGetTiLi()
    {
        if (!this.HaveQQTiLi())
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896ac));
        }
        else if (ActorData.getInstance().UserInfo.remainPhyForceAccept <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b3));
        }
        else if (XSingleton<SocialFriend>.Singleton.ServerSocialUser.Count > 0)
        {
            SocketMgr.Instance.RequestC2S_AcceptAll_TX_FriendPhyForce();
        }
    }

    private void OnClickQQFriendBtn(GameObject go)
    {
        this.QQFriendCount.text = "0";
        this.ResetClipViewport(base.FindChild<Transform>("ListTopLeft"), base.FindChild<Transform>("ListBottomRight"), base.transform.FindChild("QQFriendGroup/List"));
        this.mListType = ListType.QQ_FRIENDLIST;
        this._FrinedGroup.gameObject.SetActive(false);
        this._QQFriendGroup.gameObject.SetActive(true);
        if (XSingleton<SocialFriend>.Singleton.State != SocialFriend.SocialState.Ready)
        {
            this.QQFriendTable.Count = 0;
        }
        else
        {
            if (<>f__am$cache25 == null)
            {
                <>f__am$cache25 = t => ((t.QQUser != null) && (t.QQUser.userInfo != null)) && (t.QQUser.userInfo.leaderInfo != null);
            }
            List<SocialUser> list = XSingleton<SocialFriend>.Singleton.Users.Where<SocialUser>(<>f__am$cache25).ToList<SocialUser>();
            this.QQFriendCount.text = list.Count + string.Empty;
            this.PIQQFriend = new PageIndex<List<SocialUser>>(list, 10);
            this.QQFriendTable.Cache = false;
            base.StartCoroutine(this.ShowQQFriendByPage(1));
        }
    }

    private void OnClickQQItem(QQFriendItem item)
    {
        <OnClickQQItem>c__AnonStorey1E5 storeye = new <OnClickQQItem>c__AnonStorey1E5 {
            item = item,
            <>f__this = this
        };
        GUIMgr.Instance.DoModelGUI<QQFriendInfoPanel>(new Action<GUIEntity>(storeye.<>m__30B), base.gameObject);
    }

    private void OnClickQQOneKeySendAllBtn(GameObject go)
    {
        if (this.QQFriendTable.Count > 0)
        {
            SocketMgr.Instance.RequestC2S_GiveAll_TX_FriendPhyForce();
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x59a));
        }
    }

    private void OnClickQQReceive(QQFriendItem obj)
    {
        SocketMgr.Instance.RequestC2S_Cross_AcceptFriendPhyForce(obj.User.QQUser.userInfo.id);
    }

    private void OnClickQQSend(QQFriendItem obj)
    {
        <OnClickQQSend>c__AnonStorey1E4 storeye = new <OnClickQQSend>c__AnonStorey1E4 {
            _user = obj.User
        };
        if (storeye._user != null)
        {
            SocketMgr.Instance.RequestC2S_Cross_GiveFriendPhyForce(storeye._user.QQUser.userInfo.id);
            GUIMgr.Instance.DoModelGUI<SendForceHello>(new Action<GUIEntity>(storeye.<>m__30A), null);
        }
    }

    private void OnClickRefreshBtn(GameObject obj)
    {
        if (this.mIsStart)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x25b));
        }
        else
        {
            ActorData.getInstance().NextRefreshFriendListTime = TimeMgr.Instance.ServerStampTime + 60;
            this.mIsStart = true;
            SocketMgr.Instance.RequestGetFriendList();
            SocketMgr.Instance.RequestGetFriendReqList();
        }
    }

    private void OnClickRight()
    {
        this.ResetFriendSumPage();
        this.mCurrPage++;
        if (this.mCurrPage >= this.mFriendSumPage)
        {
            this.mCurrPage = 0;
        }
        base.StopAllCoroutines();
        base.StartCoroutine(this.InitFriendList(this.mCurrPage));
    }

    private void OnClickSearchBtn(GameObject obj)
    {
        if (ActorData.getInstance().SessionInfo != null)
        {
            UIInput component = this._SearchGroup.transform.FindChild("InputId").GetComponent<UIInput>();
            if (component.value.Trim().Length == 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x989695));
            }
            else
            {
                Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
                if (regex.Match(component.value.Trim()).Success)
                {
                    long id = long.Parse(component.value);
                    if (id == ActorData.getInstance().SessionInfo.userid)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a2));
                    }
                    else if (ActorData.getInstance().TargetInFriendList(id))
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a3));
                    }
                    else
                    {
                        SocketMgr.Instance.RequestSearchFriend(true, id, string.Empty);
                    }
                }
                else
                {
                    SocketMgr.Instance.RequestSearchFriend(false, 0L, component.value);
                }
            }
        }
    }

    private void OnClickSearchTab(GameObject go)
    {
        this._AddGroup.gameObject.SetActive(false);
        this._SearchGroup.gameObject.SetActive(true);
        this._FrinedGroup.gameObject.SetActive(false);
    }

    private void OnClickUpdateBtn(GameObject go)
    {
        GUIMgr.Instance.DoModelGUI("SetSignatureDlag", null, base.gameObject);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.multiCamera = true;
        GUIMgr.Instance.FloatTitleBar();
        this.CheckShareBtn();
        if (ActorData.getInstance().FriendPkEndReturn)
        {
            base.transform.FindChild("NewTab/MyFriend").GetComponent<UIToggle>().value = true;
            this.OnClickMyFriendBtn(null);
            this.SetFriendTips();
            this.mIsStart = TimeMgr.Instance.ServerStampTime < ActorData.getInstance().NextRefreshFriendListTime;
            this.SetRefreshTimeLabel();
        }
        else if (this.mListType == ListType.FRIENDLIST)
        {
            this.OnClickMyFriendBtn(null);
            this.SetFriendTips();
            this.mIsStart = TimeMgr.Instance.ServerStampTime < ActorData.getInstance().NextRefreshFriendListTime;
            this.SetRefreshTimeLabel();
            this.SetFriends();
            this.InitFriendList(this.mCurrPage);
        }
        else
        {
            this.OnClickQQFriendBtn(null);
        }
    }

    private void OnGiveFriendTiLiClick(FriendTableItem item)
    {
        Friend itemData = item.ItemData;
        if (itemData.alreadyGivePhyForceToday)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x989687));
        }
        else
        {
            SocketMgr.Instance.RequestGiveFriendPhyForce(itemData.userInfo.id);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        this._TipsText = ConfigMgr.getInstance().GetWord(0x4e38);
        this._MsgText = ConfigMgr.getInstance().GetWord(0x4e39);
        UIGrid component = this._FrinedGroup.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        this.FriendTable.InitFromGrid(component);
        this.FriendTable.Count = 0;
        base.FindChild<UIButton>("YaoQinBtn").OnUIMouseClick(u => this.YaoQingHaoYou());
        this.QQFriendTable.InitFromGrid(base.FindChild<UIGrid>("QQGrid"));
        this.QQFriendTable.Count = 0;
        this.QQFriendCount = base.FindChild<UILabel>("QQFriendCount");
        this.bt_receiveAll = base.FindChild<UIButton>("bt_receiveAll");
        this.bt_prv = base.FindChild<UIButton>("bt_prv");
        this.lb_pagenum = base.FindChild<UILabel>("lb_pagenum");
        this.bt_next = base.FindChild<UIButton>("bt_next");
        if (<>f__am$cache24 == null)
        {
            <>f__am$cache24 = delegate (object s) {
                bool flag = false;
                foreach (QQFriendUser user in XSingleton<SocialFriend>.Singleton.ServerSocialUser)
                {
                    if (user.isGivePhyForceNow)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896ac));
                }
                else if (ActorData.getInstance().UserInfo.remainPhyForceAccept <= 0)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b3));
                }
                else
                {
                    SocketMgr.Instance.RequestC2S_AcceptAll_TX_FriendPhyForce();
                }
            };
        }
        this.bt_receiveAll.OnUIMouseClick(<>f__am$cache24);
        this.InitGuicontrolEvent();
        SocketMgr.Instance.RequestRefreshQQFriendInGame();
        this.lb_pagenum.text = string.Format("{0}/{1}", 0, 0);
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                this.SetRefreshTimeLabel();
            }
        }
    }

    public override void OnUpdateUIData()
    {
        base.OnUpdateUIData();
        if (this.mListType == ListType.QQ_FRIENDLIST)
        {
            IEnumerator<QQFriendItem> enumerator = this.QQFriendTable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.UpdateData();
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
    }

    public void OpreationSucess()
    {
        this.UpdateRequestList();
        this.SetFriendTips();
        this.mDirty = true;
    }

    private void RepositionListAnchor(Transform trList)
    {
        GUIMgr.Instance.ResetListViewpot(this.top_left, this.bottom_right, trList, 0f);
    }

    private void ResetClipViewport(Transform topLeft, Transform bottomRight, Transform root)
    {
        GUIMgr.Instance.ResetListViewpot(topLeft, bottomRight, root, 0f);
    }

    private void ResetFriendSumPage()
    {
        int count = this.GameFriends.Count;
        this.mFriendSumPage = (int) Math.Ceiling((double) (((float) count) / ((float) this.MaxPageCount)));
    }

    private unsafe void SetBriefUserInfo(Transform obj, BriefUser _data)
    {
        if (_data != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_data.head_entry);
            if (_config != null)
            {
                obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                obj.FindChild("Job/Icon").GetComponent<UISprite>().spriteName = GameConstant.CardJobIcon[_config.class_type];
                obj.FindChild("QualityBorder").GetComponent<UISprite>().color = *((Color*) &(GameConstant.ConstQuantityColor[_config.quality]));
                obj.FindChild("Name").GetComponent<UILabel>().text = _data.name;
                obj.FindChild("Level").GetComponent<UILabel>().text = "LV. " + _data.level.ToString();
                obj.FindChild("LastLoginTime").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(_data.lastOnlineTime));
                obj.FindChild("Camp").GetComponent<UISprite>().spriteName = (_data.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
                GUIDataHolder.setData(obj.gameObject, _data);
                UIEventListener.Get(obj.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickItemBtn);
            }
        }
    }

    private unsafe void SetFriendInfo(Transform obj, Friend _data)
    {
        if (_data != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_data.userInfo.head_entry);
            if (_config != null)
            {
                obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                obj.FindChild("Job/Icon").GetComponent<UISprite>().spriteName = GameConstant.CardJobIcon[_config.class_type];
                obj.FindChild("QualityBorder").GetComponent<UISprite>().color = *((Color*) &(GameConstant.ConstQuantityColor[_config.quality]));
                Transform transform = obj.FindChild("TiLi");
                transform.gameObject.SetActive(_data.isGivePhyForceNow);
                GUIDataHolder.setData(transform.gameObject, _data);
                UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickGetTiLiBtn);
                obj.FindChild("GivePhyForce").gameObject.SetActive(_data.alreadyGivePhyForceToday);
                obj.FindChild("Name").GetComponent<UILabel>().text = _data.userInfo.name;
                obj.FindChild("Level").GetComponent<UILabel>().text = "LV. " + _data.userInfo.level.ToString();
                obj.FindChild("LastLoginTime").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(_data.userInfo.lastOnlineTime));
                obj.FindChild("Camp").GetComponent<UISprite>().spriteName = (_data.userInfo.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
                GUIDataHolder.setData(obj.gameObject, _data);
                UIEventListener.Get(obj.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickItemBtn);
            }
        }
    }

    private void SetFriends()
    {
        List<Friend> list = ActorData.getInstance().FriendList.ToList<Friend>();
        if (<>f__am$cache27 == null)
        {
            <>f__am$cache27 = delegate (Friend l, Friend r) {
                if (ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_game_friend_can_draw)
                {
                    int num = (l.redpackage_isdraw || (l.redpackage_num <= 0)) ? 0 : 1;
                    int num2 = (r.redpackage_isdraw || (r.redpackage_num <= 0)) ? 0 : 1;
                    if (num != num2)
                    {
                        return num2 - num;
                    }
                }
                int num3 = !l.isGivePhyForceNow ? 0 : 1;
                int num4 = !r.isGivePhyForceNow ? 0 : 1;
                if (num4 != num3)
                {
                    return num4 - num3;
                }
                if (l.userInfo.lastOnlineTime != r.userInfo.lastOnlineTime)
                {
                    return r.userInfo.lastOnlineTime - l.userInfo.lastOnlineTime;
                }
                if (l.userInfo.level != r.userInfo.level)
                {
                    return r.userInfo.level - l.userInfo.level;
                }
                return 1;
            };
        }
        list.Sort(<>f__am$cache27);
        this._gameFriends = list;
    }

    public void SetFriendTips()
    {
        base.transform.FindChild("NewTab/MyFriend/New").gameObject.SetActive(((ActorData.getInstance().FriendReqList.Count > 0) && (ActorData.getInstance().FriendList.Count < 200)) || ActorData.getInstance().CurrHaveTiLiCanPick());
        base.transform.FindChild("FriendGroup/SearchBtn/New").gameObject.SetActive((ActorData.getInstance().FriendReqList.Count > 0) && (ActorData.getInstance().FriendList.Count < 200));
    }

    private void SetRefreshTimeLabel()
    {
        if (TimeMgr.Instance.ServerStampTime < ActorData.getInstance().NextRefreshFriendListTime)
        {
            this._RefreshListLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0x71), ActorData.getInstance().NextRefreshFriendListTime - TimeMgr.Instance.ServerStampTime);
        }
        else
        {
            this.mIsStart = false;
            this._RefreshListLabel.text = ConfigMgr.getInstance().GetWord(0x3e6);
        }
    }

    [DebuggerHidden]
    private IEnumerator ShowQQFriendByPage(int page)
    {
        return new <ShowQQFriendByPage>c__Iterator7A { page = page, <$>page = page, <>f__this = this };
    }

    public void ShowSearchedFriend(List<BriefUser> _userList)
    {
        if (_userList.Count > 0)
        {
            this.UpdateSearchList(_userList);
            this.mListType = ListType.SEARCH_FRIEND;
        }
    }

    internal void UpdateFriendByID(long userID)
    {
        <UpdateFriendByID>c__AnonStorey1E3 storeye = new <UpdateFriendByID>c__AnonStorey1E3 {
            userID = userID
        };
        IEnumerator<FriendTableItem> enumerator = this.FriendTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                FriendTableItem current = enumerator.Current;
                if (storeye.userID == 0)
                {
                    current.ItemData = current.ItemData;
                }
                else if (current.ItemData.userInfo.id == storeye.userID)
                {
                    Friend friend = ActorData.getInstance().FriendList.Find(new Predicate<Friend>(storeye.<>m__304));
                    if (friend != null)
                    {
                        current.ItemData = friend;
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

    public void UpdateList()
    {
        this.ResetFriendSumPage();
        base.StopAllCoroutines();
        this.SetFriends();
        if (this.mCurrPage > (this.mFriendSumPage - 1))
        {
            this.mCurrPage = this.mFriendSumPage - 1;
            if (this.mCurrPage < 0)
            {
                this.mCurrPage = 0;
            }
        }
        base.StartCoroutine(this.InitFriendList(this.mCurrPage));
        this.SetFriendTips();
    }

    public void UpdateQQFriendList()
    {
        if (!ActorData.getInstance().FriendPkEndReturn)
        {
            this.OnClickQQFriendBtn(null);
        }
    }

    public void UpdateRequestList()
    {
        CommonFunc.ResetClippingPanel(this._AddGroup.transform.FindChild("List"));
        UIGrid component = this._AddGroup.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        float y = 0f;
        for (int i = 0; i < ActorData.getInstance().FriendReqList.Count; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this.SingleFriendItem) as GameObject;
            obj2.transform.parent = component.transform;
            obj2.transform.localPosition = new Vector3(0f, y, -0.1f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            y -= component.cellHeight;
            Transform transform = obj2.transform.FindChild("Item1");
            this.SetBriefUserInfo(transform, ActorData.getInstance().FriendReqList[i]);
        }
        this._AddGroup.transform.FindChild("RequestCount").GetComponent<UILabel>().text = ActorData.getInstance().FriendReqList.Count.ToString();
    }

    private void UpdateSearchList(List<BriefUser> _userList)
    {
        CommonFunc.ResetClippingPanel(this._SearchGroup.transform.FindChild("List"));
        UIGrid component = this._SearchGroup.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        float y = 0f;
        for (int i = 0; i < _userList.Count; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this.SingleFriendItem) as GameObject;
            obj2.transform.parent = component.transform;
            obj2.transform.localPosition = new Vector3(0f, y, -0.1f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            y -= component.cellHeight;
            Transform transform = obj2.transform.FindChild("Item1");
            this.SetBriefUserInfo(transform, _userList[i]);
        }
    }

    public void UpdateSignature()
    {
        this._FrinedGroup.transform.FindChild("Sign/Label").GetComponent<UILabel>().text = ActorData.getInstance().UserInfo.signature;
    }

    public void YaoQingHaoYou()
    {
        this._TipsText = ConfigMgr.getInstance().GetWord(0x4e38);
        this._MsgText = ConfigMgr.getInstance().GetWord(0x4e39);
        Debug.Log("yao qing hao you");
        string url = (GameDefine.getInstance().GetTencentType() != TencentType.QQ) ? GameDefine.getInstance().GetShareURL() : GameDefine.getInstance().GetShareURL();
        SharePanel.G_ShareFriend(this._TipsText, this._MsgText, url, "GUI/Share/Ui_Share_Icon_yqhy");
        Debug.Log("OnClickQQFriend" + this._TipsText);
        GUIMgr.Instance.ExitModelGUI("PushNotifyPanel");
    }

    protected UIButton bt_next { get; set; }

    protected UIButton bt_prv { get; set; }

    private List<Friend> GameFriends
    {
        get
        {
            if (this._gameFriends == null)
            {
                this.SetFriends();
            }
            return this._gameFriends;
        }
    }

    protected UILabel lb_pagenum { get; set; }

    [CompilerGenerated]
    private sealed class <HaveQQTiLi>c__AnonStorey1E8
    {
        internal bool have;

        internal bool <>m__310(SocialUser t)
        {
            if (t.QQUser.isGivePhyForceNow)
            {
                this.have = true;
                return true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <InitFriendList>c__Iterator7B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>page;
        internal FriendPanel <>f__this;
        internal UILabel <FriendCount>__5;
        internal List<Friend> <friends>__0;
        internal int <i>__2;
        internal int <index>__4;
        internal int <pageFriendCount>__1;
        internal FriendPanel.FriendTableItem <tItem>__3;
        internal int page;

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
                    this.<>f__this.ResetClipViewport(this.<>f__this._FrinedList_top_left, this.<>f__this._FrinedList_bottom_right, this.<>f__this.transform.FindChild("FriendGroup/List"));
                    this.<>f__this.ResetFriendSumPage();
                    this.<friends>__0 = this.<>f__this.GameFriends;
                    this.<pageFriendCount>__1 = (this.page >= (this.<>f__this.mFriendSumPage - 1)) ? (this.<friends>__0.Count % this.<>f__this.MaxPageCount) : this.<>f__this.MaxPageCount;
                    if ((this.<friends>__0.Count > 0) && ((this.<friends>__0.Count % this.<>f__this.MaxPageCount) == 0))
                    {
                        this.<pageFriendCount>__1 = this.<>f__this.MaxPageCount;
                    }
                    this.<>f__this.FriendTable.Count = this.<pageFriendCount>__1;
                    this.<i>__2 = 0;
                    while (this.<i>__2 < this.<pageFriendCount>__1)
                    {
                        this.<tItem>__3 = this.<>f__this.FriendTable[this.<i>__2];
                        this.<index>__4 = (this.page * this.<>f__this.MaxPageCount) + this.<i>__2;
                        if (this.<friends>__0.Count <= this.<index>__4)
                        {
                            Debug.LogWarning("Friend Index Out of Index");
                            break;
                        }
                        this.<tItem>__3.ItemData = this.<friends>__0[this.<index>__4];
                        this.<tItem>__3.OnClick = new Action<FriendPanel.FriendTableItem>(this.<>f__this.OnClickItemBtn);
                        this.<tItem>__3.OnLingQuTiLiClick = new Action<FriendPanel.FriendTableItem>(this.<>f__this.OnClickGetTiLiBtn);
                        this.<tItem>__3.OnGiveFriendTiLiClick = new Action<FriendPanel.FriendTableItem>(this.<>f__this.OnGiveFriendTiLiClick);
                        if ((this.<>f__this.FriendTable.Count >= 3) && (this.<tItem>__3.Drag != null))
                        {
                            this.<tItem>__3.Drag.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
                        }
                        if ((this.<i>__2 % 10) == 0)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_0348;
                        }
                    Label_0253:
                        this.<i>__2++;
                    }
                    break;

                case 1:
                    goto Label_0253;

                case 2:
                    this.$PC = -1;
                    goto Label_0346;

                default:
                    goto Label_0346;
            }
            this.<FriendCount>__5 = this.<>f__this._FrinedGroup.transform.FindChild("FriendCount").GetComponent<UILabel>();
            this.<FriendCount>__5.text = this.<friends>__0.Count + "/" + ActorData.getInstance().MaxFriendCount;
            if (this.<>f__this.mFriendSumPage == 0)
            {
                this.<>f__this._PageLabel.text = "0/0";
            }
            else
            {
                this.<>f__this._PageLabel.text = (this.page + 1) + "/" + this.<>f__this.mFriendSumPage;
            }
            this.$current = null;
            this.$PC = 2;
            goto Label_0348;
        Label_0346:
            return false;
        Label_0348:
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
    private sealed class <OnClickItemBtn>c__AnonStorey1E6
    {
        internal FriendPanel <>f__this;
        internal Friend info;

        internal void <>m__30E(GUIEntity obj)
        {
            ((FriendInfoPanel) obj).UpdateData(this.info, this.<>f__this.GetHaveHongBaoFriendList());
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1E7
    {
        internal FriendPanel <>f__this;
        internal GameObject go;

        internal void <>m__30F(GUIEntity obj)
        {
            FriendInfoPanel panel = (FriendInfoPanel) obj;
            object obj2 = GUIDataHolder.getData(this.go);
            if (obj2 != null)
            {
                switch (this.<>f__this.mListType)
                {
                    case FriendPanel.ListType.FRIENDLIST:
                        panel.UpdateData((Friend) obj2, null);
                        break;

                    case FriendPanel.ListType.REQ_FRIEND:
                        panel.SetReqFriendInfo((BriefUser) obj2);
                        break;

                    case FriendPanel.ListType.SEARCH_FRIEND:
                        panel.SetSearchFriendInfo((BriefUser) obj2);
                        break;
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickQQItem>c__AnonStorey1E5
    {
        internal FriendPanel <>f__this;
        internal FriendPanel.QQFriendItem item;

        internal void <>m__30B(GUIEntity ui)
        {
            (ui as QQFriendInfoPanel).ShowUser(this.item.User, this.<>f__this.GetSocialUserList());
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickQQSend>c__AnonStorey1E4
    {
        internal SocialUser _user;

        internal void <>m__30A(GUIEntity ui)
        {
            SendForceHello hello = ui as SendForceHello;
            hello.Depth = 400;
            hello.Show(this._user);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowQQFriendByPage>c__Iterator7A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>page;
        internal FriendPanel <>f__this;
        internal SocialUser <data>__2;
        internal int <i>__1;
        internal int <index>__0;
        internal FriendPanel.QQFriendItem <item>__3;
        internal int page;

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
                    if (this.<>f__this.PIQQFriend != null)
                    {
                        this.<>f__this.PIQQFriend.CurrentPage = this.page;
                        this.<>f__this.QQFriendTable.Count = this.<>f__this.PIQQFriend.PageItemCount;
                        this.<index>__0 = 0;
                        this.<i>__1 = this.<>f__this.PIQQFriend.Start;
                        while (this.<i>__1 < this.<>f__this.PIQQFriend.End)
                        {
                            this.<data>__2 = this.<>f__this.PIQQFriend.List[this.<i>__1];
                            this.<item>__3 = this.<>f__this.QQFriendTable[this.<index>__0];
                            this.<item>__3.User = this.<data>__2;
                            this.<item>__3.Rank = this.<data>__2.Index;
                            this.<item>__3.OnReceive = new Action<FriendPanel.QQFriendItem>(this.<>f__this.OnClickQQReceive);
                            this.<item>__3.OnSend = new Action<FriendPanel.QQFriendItem>(this.<>f__this.OnClickQQSend);
                            if (this.<item>__3.Drag != null)
                            {
                                this.<item>__3.Drag.draggableCamera = (this.<>f__this.QQFriendTable.Count < 3) ? null : GUIMgr.Instance.ListDraggableCamera;
                            }
                            this.<index>__0++;
                            this.<i>__1++;
                        }
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 1;
                        return true;
                    }
                    break;

                case 1:
                    this.<>f__this.lb_pagenum.text = string.Format("{0}/{1}", this.<>f__this.PIQQFriend.CurrentPage, this.<>f__this.PIQQFriend.PageCount);
                    this.<>f__this.ResetClipViewport(this.<>f__this.FindChild<Transform>("ListTopLeft"), this.<>f__this.FindChild<Transform>("ListBottomRight"), this.<>f__this.transform.FindChild("QQFriendGroup/List"));
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
    private sealed class <UpdateFriendByID>c__AnonStorey1E3
    {
        internal long userID;

        internal bool <>m__304(Friend t)
        {
            return (t.userInfo.id == this.userID);
        }
    }

    public class FriendTableItem : UITableItem
    {
        private Friend _Friend;
        private UISprite Camp;
        public UIDragCamera Drag;
        private Transform GetTiLiBtn;
        private Transform GiveFiendTiLi;
        private Transform GivePhyForce;
        private UISprite HongBao;
        private UITexture Icon;
        private Transform Item1;
        private UISprite JobIcon;
        private UILabel LastLoginTime;
        private UILabel Level;
        private UILabel Name;
        private UISprite NewTips;
        public Action<FriendPanel.FriendTableItem> OnClick;
        public Action<FriendPanel.FriendTableItem> OnGiveFriendTiLiClick;
        public Action<FriendPanel.FriendTableItem> OnLingQuTiLiClick;
        private UISprite QIcon;
        private UISprite QualityBorder;

        public override void OnCreate()
        {
            this.Icon = base.Root.FindChild<UITexture>("Icon");
            this.LastLoginTime = base.Root.FindChild<UILabel>("LastLoginTime");
            Transform ui = base.Root.FindChild<Transform>("Item1");
            this.QualityBorder = base.Root.FindChild<UISprite>("QualityBorder");
            this.Name = base.Root.FindChild<UILabel>("Name");
            this.Level = base.Root.FindChild<UILabel>("Level");
            this.Camp = base.Root.FindChild<UISprite>("Camp");
            this.GetTiLiBtn = base.Root.FindChild<Transform>("TiLi");
            this.GiveFiendTiLi = base.Root.FindChild<Transform>("GiveFiendTiLi");
            this.GivePhyForce = base.Root.FindChild<Transform>("GivePhyForce");
            this.NewTips = base.Root.FindChild<UISprite>("New");
            this.QIcon = base.Root.FindChild<UISprite>("QIcon");
            this.HongBao = base.Root.FindChild<UISprite>("HongBao");
            this.GiveFiendTiLi.OnUIMouseClick(delegate (object u) {
                if (this.OnGiveFriendTiLiClick != null)
                {
                    this.OnGiveFriendTiLiClick(this);
                }
            });
            this.GetTiLiBtn.OnUIMouseClick(delegate (object u) {
                if (this.OnLingQuTiLiClick != null)
                {
                    this.OnLingQuTiLiClick(this);
                }
            });
            ui.OnUIMouseClick(delegate (object u) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
            this.Item1 = ui;
            this.Drag = ui.GetComponent<UIDragCamera>();
        }

        public Friend ItemData
        {
            get
            {
                return this._Friend;
            }
            set
            {
                this._Friend = value;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(value.userInfo.head_entry);
                if (_config != null)
                {
                    this.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    this.Name.text = value.userInfo.name;
                    this.Level.text = "LV. " + value.userInfo.level.ToString();
                    CommonFunc.SetPlayerHeadFrame(this.QualityBorder, this.QIcon, value.userInfo.head_frame_entry);
                    this.LastLoginTime.text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(value.userInfo.lastOnlineTime));
                    this.Camp.spriteName = (value.userInfo.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
                    this.GetTiLiBtn.gameObject.SetActive(value.isGivePhyForceNow);
                    this.GiveFiendTiLi.gameObject.SetActive(!value.alreadyGivePhyForceToday);
                    this.GivePhyForce.gameObject.SetActive(value.alreadyGivePhyForceToday);
                    this.NewTips.gameObject.SetActive(value.isGivePhyForceNow);
                    if ((ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_game_friend_can_draw) && (ActorData.getInstance().SessionInfo.userid != value.userInfo.id))
                    {
                        this.HongBao.gameObject.SetActive(!value.redpackage_isdraw && (value.redpackage_num > 0));
                    }
                    else
                    {
                        this.HongBao.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private enum ListType
    {
        FRIENDLIST,
        REQ_FRIEND,
        SEARCH_FRIEND,
        QQ_FRIENDLIST
    }

    public class QQFriendItem : UITableItem
    {
        private SocialUser _user;
        private UITexture BlueBroad;
        private UIButton bt_Receive;
        private UIButton bt_Send;
        public UIDragCamera Drag;
        private UILabel DupText;
        private Transform GivePhyForce;
        private UISprite HongBao;
        private UITexture Icon;
        private UITexture LagerIcon;
        private UILabel lb_rank;
        private UILabel Level;
        private UILabel Name;
        public Action<FriendPanel.QQFriendItem> OnClick;
        public Action<FriendPanel.QQFriendItem> OnReceive;
        public Action<FriendPanel.QQFriendItem> OnSend;
        private UISprite qqIcon;
        private UISprite QQVip;
        private UISprite QualityBorder;
        private int r = 0x3e8;
        private UISprite RankIcon;

        [DebuggerHidden]
        private IEnumerator LoadPic(string url, UITexture texture, UITexture gameIcon)
        {
            return new <LoadPic>c__Iterator7C { url = url, texture = texture, gameIcon = gameIcon, <$>url = url, <$>texture = texture, <$>gameIcon = gameIcon };
        }

        public override void OnCreate()
        {
            this.Icon = base.FindChild<UITexture>("Icon");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.QQVip = base.FindChild<UISprite>("QQVip");
            this.RankIcon = base.FindChild<UISprite>("RankIcon");
            this.HongBao = base.FindChild<UISprite>("HongBao");
            this.Name = base.FindChild<UILabel>("Name");
            this.Level = base.FindChild<UILabel>("Level");
            this.DupText = base.FindChild<UILabel>("DupText");
            this.Drag = base.Root.GetComponent<UIDragCamera>();
            this.bt_Receive = base.FindChild<UIButton>("bt_Receive");
            this.bt_Send = base.FindChild<UIButton>("bt_Send");
            this.BlueBroad = base.FindChild<UITexture>("Owner");
            this.LagerIcon = base.FindChild<UITexture>("LagerIcon");
            this.GivePhyForce = base.FindChild<Transform>("GivePhyForce");
            this.lb_rank = base.FindChild<UILabel>("lb_rank");
            this.bt_Receive.OnUIMouseClick(delegate (object u) {
                if (this.OnReceive != null)
                {
                    this.OnReceive(this);
                }
            });
            this.bt_Send.OnUIMouseClick(delegate (object u) {
                if (this.OnSend != null)
                {
                    this.OnSend(this);
                }
            });
            base.Root.OnUIMouseClick(delegate (object u) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
        }

        public void UpdateData()
        {
            this.User = this.User;
        }

        public int Rank
        {
            get
            {
                return this.r;
            }
            set
            {
                this.r = value;
                this.RankIcon.gameObject.SetActive(this.r <= 3);
                this.RankIcon.spriteName = string.Format("Ui_Worldcup_Icon_{0}", this.r);
                this.lb_rank.gameObject.SetActive(this.r > 3);
                this.lb_rank.text = string.Format("{0}", this.r);
            }
        }

        public SocialUser User
        {
            get
            {
                return this._user;
            }
            set
            {
                trench_normal_config _config4;
                string name;
                this._user = value;
                int id = 0;
                id = (int) value.QQUser.userInfo.leaderInfo.cardInfo.entry;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(id);
                if (_config == null)
                {
                    Debug.LogWarning("CardCfg Is Null! Entry is " + id);
                    return;
                }
                this.Name.text = (this.User.Plat == null) ? this.User.QQUser.userInfo.name : this.User.Plat.SocialName;
                this.Level.text = string.Format("LV. {0}", this.User.QQUser.userInfo.level);
                Texture texture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                this.Icon.mainTexture = texture;
                this.Icon.mainTexture = texture;
                CommonFunc.SetQualityBorder(this.QualityBorder, value.QQUser.userInfo.leaderInfo.cardInfo.quality);
                if (value.Plat != null)
                {
                    this.Icon.StartCoroutine(this.LoadPic(value.Plat.SamillIconID, this.LagerIcon, this.Icon));
                }
                this.HongBao.gameObject.SetActive(false);
                this.QQVip.gameObject.SetActive(false);
                this.bt_Receive.gameObject.SetActive(this.User.QQUser.isGivePhyForceNow);
                if (this.User.Owner)
                {
                    this.bt_Send.gameObject.SetActive(false);
                    this.GivePhyForce.gameObject.SetActive(false);
                }
                else
                {
                    this.bt_Send.gameObject.SetActive(!this.User.QQUser.alreadyGivePhyForceToday);
                    this.GivePhyForce.gameObject.SetActive(this.User.QQUser.alreadyGivePhyForceToday);
                }
                this.BlueBroad.gameObject.SetActive(this.User.Owner);
                Debug.Log(("User Info Exists:" + value.UserInfo) == null);
                if (value.UserInfo == null)
                {
                    this.DupText.text = string.Empty;
                    return;
                }
                ArrayList list = ConfigMgr.getInstance().getList<duplicate_config>();
                if (value.UserInfo.Elite <= 0)
                {
                    if (value.UserInfo.Normal <= 0)
                    {
                        this.DupText.text = string.Empty;
                        return;
                    }
                    _config4 = ConfigMgr.getInstance().getByEntry<trench_normal_config>(value.UserInfo.Normal);
                    name = string.Empty;
                    if (_config4 == null)
                    {
                        return;
                    }
                    IEnumerator enumerator2 = list.GetEnumerator();
                    try
                    {
                        while (enumerator2.MoveNext())
                        {
                            object current = enumerator2.Current;
                            duplicate_config _config5 = current as duplicate_config;
                            char[] separator = new char[] { '|' };
                            if (_config5.normal_entry.Split(separator).Contains<string>(_config4.entry.ToString()))
                            {
                                name = _config5.name;
                                goto Label_03E4;
                            }
                        }
                    }
                    finally
                    {
                        IDisposable disposable2 = enumerator2 as IDisposable;
                        if (disposable2 == null)
                        {
                        }
                        disposable2.Dispose();
                    }
                    goto Label_03E4;
                }
                trench_elite_config _config2 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(value.UserInfo.Elite);
                string str = string.Empty;
                if (_config2 == null)
                {
                    return;
                }
                IEnumerator enumerator = list.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object obj2 = enumerator.Current;
                        duplicate_config _config3 = obj2 as duplicate_config;
                        char[] chArray1 = new char[] { '|' };
                        if (_config3.elite_entry.Split(chArray1).Contains<string>(_config2.entry.ToString()))
                        {
                            str = _config3.name;
                            goto Label_0307;
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
            Label_0307:
                this.DupText.text = string.Format("精英-{0}-{1}", str, _config2.name);
                return;
            Label_03E4:
                this.DupText.text = string.Format("普通-{0}-{1}", name, _config4.name);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadPic>c__Iterator7C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal UITexture <$>gameIcon;
            internal UITexture <$>texture;
            internal string <$>url;
            internal WWW <ww>__0;
            internal UITexture gameIcon;
            internal UITexture texture;
            internal string url;

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
                        this.<ww>__0 = new WWW(this.url);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00BA;
                }
                if (!this.<ww>__0.isDone)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                if (string.IsNullOrEmpty(this.<ww>__0.error))
                {
                    if (this.texture != null)
                    {
                        this.gameIcon.enabled = false;
                        this.texture.enabled = true;
                        this.texture.mainTexture = this.<ww>__0.texture;
                    }
                    this.$PC = -1;
                }
            Label_00BA:
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
}

