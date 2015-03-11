using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Toolbox;
using UnityEngine;

public class AddFriendPanel : GUIEntity
{
    public Transform bottom_right;
    public UITableManager<FriendTableItem> FriendTable = new UITableManager<FriendTableItem>();
    private ListType mListType;
    public Transform top_left;

    public void AgreeFriendSuccessd()
    {
        this.InitFriendReqList();
    }

    private void ExitPanel()
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    private void InitFriendReqList()
    {
        this.mListType = ListType.REQ_FRIEND;
        base.StartCoroutine(this.UpdateList(ActorData.getInstance().FriendReqList));
        this.SetCurrShowType();
    }

    private void InitGuiControlEvent()
    {
        base.transform.FindChild("Sign/ID").GetComponent<UILabel>().text = ActorData.getInstance().SessionInfo.userid.ToString();
        UIEventListener.Get(base.transform.FindChild("AllAgreeBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAllAgreeBtn);
        UIEventListener.Get(base.transform.FindChild("AllRefuseBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAllRefuseBtn);
        UIEventListener.Get(base.transform.FindChild("SearchBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSearchBtn);
    }

    private void OnClickAddBtn(FriendTableItem item)
    {
        BriefUser itemData = item.ItemData;
        SocketMgr.Instance.RequestAddFriend(itemData.id);
    }

    private void OnClickAgreeBtn(FriendTableItem item)
    {
        BriefUser itemData = item.ItemData;
        SocketMgr.Instance.RequestAgreeFriend(itemData.id);
        GUIMgr.Instance.PopGUIEntity();
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
            GUIMgr.Instance.PopGUIEntity();
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b1) + "0");
        }
    }

    private void OnClickAllRefuseBtn(GameObject go)
    {
        if (ActorData.getInstance().FriendReqList.Count < 1)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b1) + "0");
        }
        else
        {
            SocketMgr.Instance.RequestRefuseFriend(-1L, true);
        }
    }

    private void OnClickItemBtn(FriendTableItem item)
    {
        <OnClickItemBtn>c__AnonStorey1DE storeyde = new <OnClickItemBtn>c__AnonStorey1DE {
            <>f__this = this,
            info = item.ItemData
        };
        GUIMgr.Instance.DoModelGUI("FriendInfoPanel", new Action<GUIEntity>(storeyde.<>m__2F9), null);
        SoundManager.mInstance.PlaySFX("sound_ui_t_8");
    }

    private void OnClickItemBtn(GameObject go)
    {
        <OnClickItemBtn>c__AnonStorey1DF storeydf = new <OnClickItemBtn>c__AnonStorey1DF {
            go = go,
            <>f__this = this
        };
        GUIMgr.Instance.DoModelGUI("FriendInfoPanel", new Action<GUIEntity>(storeydf.<>m__2FA), null);
    }

    private void OnClickSearchBtn(GameObject obj)
    {
        if (ActorData.getInstance().SessionInfo != null)
        {
            UIInput component = base.gameObject.transform.FindChild("InputId").GetComponent<UIInput>();
            if (component.text.Trim().Length == 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x989695));
            }
            else
            {
                Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
                if (regex.Match(component.text.Trim()).Success)
                {
                    long id = long.Parse(component.text);
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
                    SocketMgr.Instance.RequestSearchFriend(false, 0L, component.text);
                }
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        this.ResetClipViewport();
        this.InitFriendReqList();
        GUIMgr.Instance.FloatTitleBar();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        UIGrid component = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        this.FriendTable.InitFromGrid(component);
        this.FriendTable.Count = 0;
        this.InitGuiControlEvent();
    }

    public void RefuseFriendSuccessd()
    {
        this.InitFriendReqList();
    }

    public void ReqAddFriendSucceed()
    {
        Debug.Log("ReqAddFriendSucceed");
    }

    private void ResetClipViewport()
    {
        GUIMgr.Instance.ResetListViewpot(this.top_left, this.bottom_right, base.transform.FindChild("List"), 0f);
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
                obj.FindChild("Level").GetComponent<UILabel>().text = _data.level.ToString();
                obj.FindChild("LastLoginTime").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(_data.lastOnlineTime));
                obj.FindChild("Camp").GetComponent<UISprite>().spriteName = (_data.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
                GUIDataHolder.setData(obj.gameObject, _data);
                UIEventListener.Get(obj.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickItemBtn);
            }
        }
    }

    private void SetCurrShowType()
    {
        Transform transform = base.transform.FindChild("AllAgreeBtn");
        Transform transform2 = base.transform.FindChild("AllRefuseBtn");
        Transform transform3 = base.transform.FindChild("ShowAllBtn");
        UILabel component = base.transform.FindChild("CountTips").GetComponent<UILabel>();
        switch (this.mListType)
        {
            case ListType.REQ_FRIEND:
                component.text = ConfigMgr.getInstance().GetWord(0x9896b1);
                transform.gameObject.SetActive(true);
                transform2.gameObject.SetActive(true);
                transform3.gameObject.SetActive(false);
                break;

            case ListType.SEARCH_FRIEND:
                component.text = ConfigMgr.getInstance().GetWord(0x9896b0);
                transform.gameObject.SetActive(false);
                transform2.gameObject.SetActive(false);
                transform3.gameObject.SetActive(true);
                break;
        }
    }

    public void ShowSearchedFriend(List<BriefUser> _userList)
    {
        if (_userList.Count > 0)
        {
            this.mListType = ListType.SEARCH_FRIEND;
            base.StartCoroutine(this.UpdateList(_userList));
            this.SetCurrShowType();
        }
    }

    [DebuggerHidden]
    public IEnumerator UpdateList(List<BriefUser> _userList)
    {
        return new <UpdateList>c__Iterator79 { _userList = _userList, <$>_userList = _userList, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1DE
    {
        internal AddFriendPanel <>f__this;
        internal BriefUser info;

        internal void <>m__2F9(GUIEntity obj)
        {
            FriendInfoPanel panel = (FriendInfoPanel) obj;
            switch (this.<>f__this.mListType)
            {
                case AddFriendPanel.ListType.REQ_FRIEND:
                    panel.SetReqFriendInfo(this.info);
                    break;

                case AddFriendPanel.ListType.SEARCH_FRIEND:
                    panel.SetSearchFriendInfo(this.info);
                    break;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1DF
    {
        internal AddFriendPanel <>f__this;
        internal GameObject go;

        internal void <>m__2FA(GUIEntity obj)
        {
            FriendInfoPanel panel = (FriendInfoPanel) obj;
            object obj2 = GUIDataHolder.getData(this.go);
            if (obj2 != null)
            {
                switch (this.<>f__this.mListType)
                {
                    case AddFriendPanel.ListType.REQ_FRIEND:
                        panel.SetReqFriendInfo((BriefUser) obj2);
                        break;

                    case AddFriendPanel.ListType.SEARCH_FRIEND:
                        panel.SetSearchFriendInfo((BriefUser) obj2);
                        break;
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateList>c__Iterator79 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<BriefUser> _userList;
        internal List<BriefUser> <$>_userList;
        internal AddFriendPanel <>f__this;
        internal UILabel <FriendCount>__2;
        internal int <i>__0;
        internal AddFriendPanel.FriendTableItem <tItem>__1;

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
                    this.<>f__this.ResetClipViewport();
                    this.<>f__this.FriendTable.Count = this._userList.Count;
                    Debug.Log(this._userList.Count);
                    this.<i>__0 = 0;
                    goto Label_0183;

                case 1:
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_01F6;

                default:
                    goto Label_01F6;
            }
        Label_0175:
            this.<i>__0++;
        Label_0183:
            if (this.<i>__0 < this._userList.Count)
            {
                this.<tItem>__1 = this.<>f__this.FriendTable[this.<i>__0];
                this.<tItem>__1.ItemData = this._userList[this.<i>__0];
                this.<tItem>__1.OnClick = new Action<AddFriendPanel.FriendTableItem>(this.<>f__this.OnClickItemBtn);
                this.<tItem>__1.OnClickAdd = new Action<AddFriendPanel.FriendTableItem>(this.<>f__this.OnClickAddBtn);
                this.<tItem>__1.OnClickAgree = new Action<AddFriendPanel.FriendTableItem>(this.<>f__this.OnClickAgreeBtn);
                this.<tItem>__1.SetType(this.<>f__this.mListType);
                if ((this.<>f__this.FriendTable.Count >= 3) && (this.<tItem>__1.Drag != null))
                {
                    this.<tItem>__1.Drag.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
                }
                if ((this.<i>__0 % 10) == 0)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01F8;
                }
                goto Label_0175;
            }
            this.<FriendCount>__2 = this.<>f__this.transform.FindChild("RequestCount").GetComponent<UILabel>();
            this.<FriendCount>__2.text = this.<>f__this.FriendTable.Count.ToString();
            this.$current = null;
            this.$PC = 2;
            goto Label_01F8;
        Label_01F6:
            return false;
        Label_01F8:
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

    public class FriendTableItem : UITableItem
    {
        private BriefUser _BriefUser;
        private Transform AddBtn;
        private Transform AgreeBtn;
        private UISprite Camp;
        public UIDragCamera Drag;
        private Transform GetTiLiBtn;
        private UILabel GivePhyForce;
        private UITexture Icon;
        private Transform Item1;
        private UISprite JobIcon;
        private UILabel LastLoginTime;
        private UILabel Level;
        private UILabel Name;
        public Action<AddFriendPanel.FriendTableItem> OnClick;
        public Action<AddFriendPanel.FriendTableItem> OnClickAdd;
        public Action<AddFriendPanel.FriendTableItem> OnClickAgree;
        private UISprite QIcon;
        private UISprite QualityBorder;

        public override void OnCreate()
        {
            this.Icon = base.Root.FindChild<UITexture>("Icon");
            this.LastLoginTime = base.Root.FindChild<UILabel>("LastLoginTime");
            Transform ui = base.Root.FindChild<Transform>("Item1");
            this.QualityBorder = base.Root.FindChild<UISprite>("QualityBorder");
            this.QIcon = base.Root.FindChild<UISprite>("QIcon");
            this.Name = base.Root.FindChild<UILabel>("Name");
            this.Level = base.Root.FindChild<UILabel>("Level");
            this.Camp = base.Root.FindChild<UISprite>("Camp");
            this.GetTiLiBtn = base.Root.FindChild<Transform>("TiLi");
            this.GivePhyForce = base.Root.FindChild<UILabel>("GivePhyForce");
            this.AddBtn = base.Root.FindChild<Transform>("AddBtn");
            this.AgreeBtn = base.Root.FindChild<Transform>("AgreeBtn");
            this.AddBtn.OnUIMouseClick(delegate (object u) {
                if (this.OnClickAdd != null)
                {
                    this.OnClickAdd(this);
                }
            });
            this.AgreeBtn.OnUIMouseClick(delegate (object u) {
                if (this.OnClickAgree != null)
                {
                    this.OnClickAgree(this);
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

        public void SetType(AddFriendPanel.ListType type)
        {
            this.AddBtn.gameObject.SetActive(type == AddFriendPanel.ListType.SEARCH_FRIEND);
            this.AgreeBtn.gameObject.SetActive(type == AddFriendPanel.ListType.REQ_FRIEND);
        }

        public BriefUser ItemData
        {
            get
            {
                return this._BriefUser;
            }
            set
            {
                this._BriefUser = value;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(value.head_entry);
                if (_config != null)
                {
                    this.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    this.Name.text = value.name;
                    this.Level.text = "Lv." + value.level;
                    CommonFunc.SetPlayerHeadFrame(this.QualityBorder, this.QIcon, value.head_frame_entry);
                    this.LastLoginTime.text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(value.lastOnlineTime));
                    this.Camp.spriteName = (value.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
                }
            }
        }
    }

    public enum ListType
    {
        REQ_FRIEND,
        SEARCH_FRIEND
    }
}

