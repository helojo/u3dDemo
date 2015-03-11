using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class HongBaoPanel : GUIEntity
{
    private UILabel _NullTipsLabel;
    private UILabel _PageLabel;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheA;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheB;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheC;
    private Transform ChongZhiBtn;
    public UITableManager<FriendTableItem> FriendTable = new UITableManager<FriendTableItem>();
    private Transform JumpFriendBtn;
    private int MaxPageCount = 10;
    private int mCurrPage;
    private int mFriendSumPage;
    public bool mIsShowLingQuList = true;
    private S2C_RedPackageRecord mRecordData;

    private List<RedPackageRecord> GetRecordList()
    {
        this.JumpFriendBtn.gameObject.SetActive(!this.mIsShowLingQuList);
        this.ChongZhiBtn.gameObject.SetActive(this.mIsShowLingQuList);
        if (this.mRecordData != null)
        {
            List<RedPackageRecord> list = !this.mIsShowLingQuList ? this.mRecordData.drawRecord : this.mRecordData.beDrawRecord;
            if (list.Count < 1)
            {
                this._NullTipsLabel.text = !this.mIsShowLingQuList ? ConfigMgr.getInstance().GetWord(0x886) : ConfigMgr.getInstance().GetWord(0x885);
                this._NullTipsLabel.gameObject.SetActive(true);
                return list;
            }
            this._NullTipsLabel.gameObject.SetActive(false);
            return list;
        }
        this._NullTipsLabel.text = !this.mIsShowLingQuList ? ConfigMgr.getInstance().GetWord(0x886) : ConfigMgr.getInstance().GetWord(0x885);
        this._NullTipsLabel.gameObject.SetActive(true);
        return null;
    }

    private void InitGuiControlEvent()
    {
        UIEventListener.Get(base.transform.FindChild("Tab/LingQuBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLingQuBtn);
        UIEventListener.Get(base.transform.FindChild("Tab/FaFangBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickFaFangBtn);
        this.ChongZhiBtn = base.transform.FindChild("Group/ChongZhiBtn");
        UIEventListener.Get(this.ChongZhiBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickChongZhiBtn);
        this.JumpFriendBtn = base.transform.FindChild("Group/JumpFriendBtn");
        UIEventListener.Get(this.JumpFriendBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickJumpFriendBtn);
        UIEventListener.Get(base.transform.FindChild("Group/ShuoMingBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShuoMingBtn);
        this._PageLabel = base.transform.FindChild("Group/LabelPageCount/Label").GetComponent<UILabel>();
        this._NullTipsLabel = base.transform.FindChild("NullTipsLabel").GetComponent<UILabel>();
    }

    private void OnClickChongZhiBtn(GameObject go)
    {
        if (<>f__am$cacheC == null)
        {
            <>f__am$cacheC = obj => VipCardPanel panel = (VipCardPanel) obj;
        }
        GUIMgr.Instance.PushGUIEntity("VipCardPanel", <>f__am$cacheC);
    }

    private void OnClickFaFangBtn(GameObject go)
    {
        this.SetBtnStat(true);
    }

    private void OnClickJumpFriendBtn(GameObject go)
    {
        if (ActorData.getInstance().UserInfo.redpackage_game_friend_can_draw)
        {
            if (<>f__am$cacheB == null)
            {
                <>f__am$cacheB = delegate (GUIEntity obj) {
                    FriendPanel panel = (FriendPanel) obj;
                    ActorData.getInstance().FriendPkEndReturn = true;
                };
            }
            GUIMgr.Instance.PushGUIEntity("FriendPanel", <>f__am$cacheB);
        }
        else if (ActorData.getInstance().UserInfo.redpackage_guild_friend_can_draw)
        {
            if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().guild)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x8a4), CommonFunc.LevelLimitCfg().guild));
            }
            else
            {
                SocketMgr.Instance.RequestGuildData(true, null);
            }
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x8a5));
        }
    }

    private void OnClickLeft()
    {
        this.mCurrPage--;
        if (this.mCurrPage < 0)
        {
            this.mCurrPage = (this.mFriendSumPage > 0) ? (this.mFriendSumPage - 1) : 0;
        }
        this.UpdateList(this.mCurrPage);
    }

    private void OnClickLingQuBtn(GameObject go)
    {
        this.SetBtnStat(false);
    }

    private void OnClickRight()
    {
        this.mCurrPage++;
        if (this.mCurrPage >= this.mFriendSumPage)
        {
            this.mCurrPage = 0;
        }
        this.UpdateList(this.mCurrPage);
    }

    private void OnClickShuoMingBtn(GameObject go)
    {
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = obj => ((WorldCupRulePanel) obj).SetHongBaoRule();
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cacheA, base.gameObject);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
        this.ResetClipViewport();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        this.InitGuiControlEvent();
        UIGrid component = base.transform.FindChild("Group/List/Grid").GetComponent<UIGrid>();
        this.FriendTable.InitFromGrid(component);
        this.FriendTable.Count = 0;
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    private void ResetClipViewport()
    {
        Transform top = base.transform.FindChild("ListTopLeft");
        Transform bottom = base.transform.FindChild("ListBottomRight");
        GUIMgr.Instance.ResetListViewpot(top, bottom, base.transform.FindChild("Group/List"), 0f);
    }

    private void SetBtnStat(bool isLingQuList)
    {
        this.mIsShowLingQuList = isLingQuList;
        this.mCurrPage = 0;
        this.SetTipsLabel();
        this.UpdateList(this.mCurrPage);
    }

    private void SetTipsLabel()
    {
        UILabel component = base.transform.FindChild("Group/TipsLabel").GetComponent<UILabel>();
        if (this.mIsShowLingQuList)
        {
            int num = (this.mRecordData == null) ? 0 : this.mRecordData.beDrawRedPackageCount;
            if (num < 0)
            {
                num = 0;
            }
            component.text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x87c), GameConstant.DefaultTextRedColor + num + GameConstant.DefaultTextColor);
        }
        else
        {
            int num2 = (this.mRecordData == null) ? 0 : this.mRecordData.drawRedPackageCount;
            if (num2 < 0)
            {
                num2 = 0;
            }
            component.text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x87d), GameConstant.DefaultTextRedColor + num2 + GameConstant.DefaultTextColor);
        }
    }

    public void UpdateData(S2C_RedPackageRecord res)
    {
        this.mRecordData = res;
        this.SetTipsLabel();
        this.UpdateList(this.mCurrPage);
    }

    public void UpdateList(int page)
    {
        this.ResetClipViewport();
        List<RedPackageRecord> recordList = this.GetRecordList();
        if (recordList != null)
        {
            this.mFriendSumPage = (int) Math.Ceiling((double) (((float) recordList.Count) / ((float) this.MaxPageCount)));
            int maxPageCount = (page >= (this.mFriendSumPage - 1)) ? (recordList.Count % this.MaxPageCount) : this.MaxPageCount;
            if ((recordList.Count > 0) && ((recordList.Count % this.MaxPageCount) == 0))
            {
                maxPageCount = this.MaxPageCount;
            }
            this.FriendTable.Count = maxPageCount;
            for (int i = 0; i < maxPageCount; i++)
            {
                FriendTableItem item = this.FriendTable[i];
                int num3 = (page * this.MaxPageCount) + i;
                if (recordList.Count <= num3)
                {
                    Debug.LogWarning("Friend Index Out of Index");
                    break;
                }
                item.ItemData = recordList[num3];
                if (!this.mIsShowLingQuList)
                {
                    item.Name.text = string.Format(ConfigMgr.getInstance().GetWord(0x889), recordList[num3].name + GameConstant.DefaultTextRedColor);
                }
            }
            if (this.mFriendSumPage == 0)
            {
                this._PageLabel.text = "0/0";
            }
            else
            {
                this._PageLabel.text = (page + 1) + "/" + this.mFriendSumPage;
            }
        }
    }

    public class FriendTableItem : UITableItem
    {
        private RedPackageRecord _Friend;
        public UIDragCamera Drag;
        private UITexture Icon;
        private Transform Item1;
        private UILabel LastLoginTime;
        private UILabel Level;
        public UILabel Name;
        private UISprite QIcon;
        private UISprite QualityBorder;
        private UILabel StoneCount;
        private UILabel Tips;

        public override void OnCreate()
        {
            this.Icon = base.Root.FindChild<UITexture>("Icon");
            this.LastLoginTime = base.Root.FindChild<UILabel>("LastLoginTime");
            Transform transform = base.Root.FindChild<Transform>("Item1");
            this.QualityBorder = base.Root.FindChild<UISprite>("QualityBorder");
            this.Name = base.Root.FindChild<UILabel>("Name");
            this.Level = base.Root.FindChild<UILabel>("Level");
            this.StoneCount = base.Root.FindChild<UILabel>("StoneCount");
            this.Tips = base.Root.FindChild<UILabel>("Tips");
            this.QIcon = base.Root.FindChild<UISprite>("QIcon");
            this.Drag = transform.GetComponent<UIDragCamera>();
            this.Item1 = transform;
        }

        public RedPackageRecord ItemData
        {
            get
            {
                return this._Friend;
            }
            set
            {
                this._Friend = value;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(value.headEntry);
                if (_config != null)
                {
                    this.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    this.Name.text = value.name;
                    this.Level.text = string.Empty;
                    CommonFunc.SetPlayerHeadFrame(this.QualityBorder, this.QIcon, value.headFrameEntry);
                    this.LastLoginTime.text = TimeMgr.Instance.GetMailTime((int) value.time);
                    this.StoneCount.text = value.drawStone.ToString();
                    this.Tips.text = ConfigMgr.getInstance().GetWord(value.wishTextId);
                    this.Drag.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
                }
            }
        }
    }
}

