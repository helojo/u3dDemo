using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class GuidBattleBossHandOutPanel : GUIPanelEntity
{
    private bool bForTest = true;
    private int clickReqCnt;
    private static int curDupId__;
    private int curPage;
    private List<int> curPageItemsList = new List<int>();
    private int curReqItemId;
    public int curReqItemIndex;
    private ReqPageState curreqPageState = ReqPageState.reqNone;
    private Dictionary<int, List<HandOutItemInfo>> DupIdToBaseItemInfoListDic = new Dictionary<int, List<HandOutItemInfo>>();
    private Dictionary<int, List<HandOutItemInfo>> DupIdToIServerItemInfoListDic = new Dictionary<int, List<HandOutItemInfo>>();
    private bool haveClick;
    private List<int> ItemIds = new List<int>();
    private int manualTimes__ = 1;
    private int pageAllCnt;
    private Dictionary<int, List<int>> PageIdToItemsDic = new Dictionary<int, List<int>>();
    private const int perPageItemCnt = 10;
    private bool reqLock;
    private List<HandOutItemInfo> ServerItemInfoList = new List<HandOutItemInfo>();
    protected UITableManager<UIAutoGenItem<GridHandTableItemTemplate, GridHandTableItemModel>> TableGridHandTable = new UITableManager<UIAutoGenItem<GridHandTableItemTemplate, GridHandTableItemModel>>();
    private List<HandOutItemInfo> xxTestHoItemInfoList = new List<HandOutItemInfo>();

    private void ClickDetail(int index, int itemId = 0, int quality = 0, int leftCnt = 0)
    {
        <ClickDetail>c__AnonStorey209 storey = new <ClickDetail>c__AnonStorey209 {
            itemId = itemId,
            quality = quality,
            leftCnt = leftCnt,
            <>f__this = this,
            xxPlayerQueueList = new List<GuidBattleItemHandQueuePanel.HandPlayerQueueInfo>()
        };
        HandOutItemInfo info = this.JudgeItemIdIsInSeverItemsData(this.ServerItemInfoList, storey.itemId);
        if (info != null)
        {
            foreach (HandOutItemInfo info2 in this.xxTestHoItemInfoList)
            {
                if (info2.entry == storey.itemId)
                {
                    int num = 0;
                    foreach (GuildDupDistributeUserInfo info3 in info.reqPlayerInfoList)
                    {
                        num++;
                        GuidBattleItemHandQueuePanel.HandPlayerQueueInfo item = new GuidBattleItemHandQueuePanel.HandPlayerQueueInfo {
                            playerLv = info3.level,
                            playerName = info3.name,
                            queueId = num,
                            cardMainEntry = info3.headEntry,
                            userId = info3.userId
                        };
                        storey.xxPlayerQueueList.Add(item);
                    }
                    break;
                }
            }
        }
        GUIMgr.Instance.DoModelGUI<GuidBattleItemHandQueuePanel>(new Action<GUIEntity>(storey.<>m__393), null);
    }

    private void ClickReq(int itemId, int index = 0)
    {
        this.curReqItemId = this.xxTestHoItemInfoList[index].entry;
        this.curReqItemIndex = index;
        SocketMgr.Instance.RequestC2S_ReqGuildDupItemDistribute(DupId, itemId);
    }

    private void GetDupOutItems(int id)
    {
        this.ItemIds.Clear();
        string distributeItemList = string.Empty;
        guilddup_config _config = ConfigMgr.getInstance().getByEntry<guilddup_config>(id);
        if (_config != null)
        {
            distributeItemList = _config.distributeItemList;
        }
        char[] separator = new char[] { '|' };
        foreach (string str2 in distributeItemList.Split(separator))
        {
            this.ItemIds.Add(int.Parse(str2));
        }
        this.GetPageToItemsInfo();
        this.InitConfigDataForList();
    }

    private void GetPageToItemsInfo()
    {
        List<int> list2;
        int key = 0;
        for (int i = 0; i < this.ItemIds.Count; i++)
        {
            key = i / 10;
            int num3 = i % 10;
            if (!this.PageIdToItemsDic.ContainsKey(key))
            {
                List<int> list = new List<int>();
                this.PageIdToItemsDic.Add(key, list);
                if (!this.PageIdToItemsDic[key].Contains(this.ItemIds[i]))
                {
                    this.PageIdToItemsDic[key].Add(this.ItemIds[i]);
                }
                else
                {
                    Debug.Log("ItemIds have same ItemId from config！！！");
                }
            }
            else if (!this.PageIdToItemsDic[key].Contains(this.ItemIds[i]))
            {
                this.PageIdToItemsDic[key].Add(this.ItemIds[i]);
            }
            else
            {
                Debug.Log("ItemIds have same ItemId from config！！！");
            }
        }
        if (this.PageIdToItemsDic.TryGetValue(this.curPage, out list2))
        {
            this.curPageItemsList = list2;
            XSingleton<GameGuildMgr>.Singleton.CurReqPageItemIDList = list2;
        }
        else
        {
            Debug.Log("ItemsList of curPage is Error！！！+++:::" + this.curPage);
        }
        this.pageAllCnt = this.PageIdToItemsDic.Count;
        this.SetPageInfo();
    }

    private void InitConfigDataForList()
    {
        this.xxTestHoItemInfoList.Clear();
        for (int i = 0; i < this.curPageItemsList.Count; i++)
        {
            HandOutItemInfo item = new HandOutItemInfo {
                entry = this.curPageItemsList[i]
            };
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.curPageItemsList[i]);
            if (_config != null)
            {
                item.quality = _config.quality;
            }
            else
            {
                Debug.Log("Error!!!! item_config have not this itemId _: " + this.curPageItemsList[i]);
            }
            item.reqRankId = -1;
            item.leftCnt = 0;
            item.reqPlayerAllCnt = 0;
            List<GuildDupDistributeUserInfo> list = new List<GuildDupDistributeUserInfo>();
            for (int j = 0; j < 0; j++)
            {
                GuildDupDistributeUserInfo info2 = new GuildDupDistributeUserInfo {
                    headEntry = (ushort) (j + 2),
                    level = (byte) j,
                    name = "j +" + j,
                    userId = j
                };
                list.Add(info2);
            }
            item.reqPlayerInfoList = list;
            this.xxTestHoItemInfoList.Add(item);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.SetHandOutItemInfo(this.xxTestHoItemInfoList);
        this.bt_next.OnUIMouseClick(u => this.TuenToNextPage());
        this.bt_prv.OnUIMouseClick(u => this.TurnToPrePage());
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.LabelMainTips = base.FindChild<UILabel>("LabelMainTips");
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridHandTable = base.FindChild<UIGrid>("GridHandTable");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.LabelHandOutTitle = base.FindChild<UILabel>("LabelHandOutTitle");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.bt_prv = base.FindChild<UIButton>("bt_prv");
        this.lb_pagenum = base.FindChild<UILabel>("lb_pagenum");
        this.bt_next = base.FindChild<UIButton>("bt_next");
        this.TableGridHandTable.InitFromGrid(this.GridHandTable);
    }

    private void InitUiGridData(int itemCnt)
    {
        this.TableGridHandTable.Cache = false;
        this.TableGridHandTable.Count = itemCnt;
        if (itemCnt <= 0)
        {
            this.LabelMainTips.text = ConfigMgr.getInstance().GetWord(0x186b7);
        }
        else
        {
            this.LabelMainTips.text = ConfigMgr.getInstance().GetWord(0x186b8);
        }
    }

    private HandOutItemInfo JudgeItemIdIsInSeverItemsData(List<HandOutItemInfo> hoItemInfoList, int itemId)
    {
        HandOutItemInfo info = new HandOutItemInfo();
        bool flag = false;
        foreach (HandOutItemInfo info2 in hoItemInfoList)
        {
            if (info2.entry == itemId)
            {
                info = info2;
                flag = true;
                break;
            }
            flag = false;
        }
        if (!flag)
        {
            info = null;
        }
        return info;
    }

    public void ReceiveBossDamageRank(S2C_GuildDupItemQueueInfo dupItemQueueData, bool bForTest = false)
    {
        if (!bForTest)
        {
            this.manualTimes__ = dupItemQueueData.manualTimes;
            curDupId__ = XSingleton<GameGuildMgr>.Singleton.CurReqDupId;
            if (this.curreqPageState == ReqPageState.reqPre)
            {
                this.curPage--;
            }
            else if (this.curreqPageState == ReqPageState.reqNext)
            {
                this.curPage++;
            }
            else if (this.curreqPageState == ReqPageState.reqNone)
            {
            }
            this.SetPageInfo();
            this.SetScrollListInitPos(this.ScrollViewList);
            this.ServerItemInfoList.Clear();
            List<HandOutItemInfo> list = new List<HandOutItemInfo>();
            this.GetDupOutItems(DupId);
            foreach (GuildDupDistributeQueueInfo info in dupItemQueueData.queue)
            {
                HandOutItemInfo item = new HandOutItemInfo {
                    entry = info.itemId
                };
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.itemId);
                if (_config != null)
                {
                    item.quality = _config.quality;
                }
                else
                {
                    item.quality = 0;
                    Debug.Log("Cur Item-Config is Error!  :::" + info.itemId);
                }
                item.leftCnt = info.itemCount;
                item.reqPlayerAllCnt = info.users.Count;
                item.reqPlayerInfoList = info.users;
                int num = 0;
                foreach (GuildDupDistributeUserInfo info3 in info.users)
                {
                    num++;
                    item.reqRankId = num;
                    if (info3.userId == ActorData.getInstance().SessionInfo.userid)
                    {
                        item.isInQueue = true;
                        break;
                    }
                    item.isInQueue = false;
                }
                this.ServerItemInfoList.Add(item);
            }
        }
        this.SetHandOutItemInfo(this.xxTestHoItemInfoList);
    }

    public void ReceiveBossDamageRank(S2C_ReqGuildDupItemDistribute dupItemQueueData, bool bForTest = false)
    {
        this.curreqPageState = ReqPageState.reqNone;
        SocketMgr.Instance.RequestC2S_GuildDupItemQueueInfo(this.curPageItemsList, curDupId__);
    }

    private void SetHandOutItemInfo(List<HandOutItemInfo> hoItemInfoList)
    {
        this.InitUiGridData(hoItemInfoList.Count);
        int count = hoItemInfoList.Count;
        for (int i = 0; i < count; i++)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(hoItemInfoList[i].entry);
            this.TableGridHandTable[i].Model.Template.LabelItemName.text = _config.name;
            this.TableGridHandTable[i].Model.Template.TextureItemIcon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            CommonFunc.SetEquipQualityBorder(this.TableGridHandTable[i].Model.Template.SpriteItemQulity, hoItemInfoList[i].quality, false);
            this.TableGridHandTable[i].Model.Entry = _config.entry;
            int entry = hoItemInfoList[i].entry;
            HandOutItemInfo info = this.JudgeItemIdIsInSeverItemsData(this.ServerItemInfoList, entry);
            string str2 = "[00ff00]";
            if (this.JudgeItemIdIsInSeverItemsData(this.ServerItemInfoList, hoItemInfoList[i].entry) != null)
            {
                <SetHandOutItemInfo>c__AnonStorey207 storey = new <SetHandOutItemInfo>c__AnonStorey207 {
                    <>f__this = this
                };
                if (info.isInQueue)
                {
                    str2 = "[00ff00]";
                }
                else if (info.leftCnt > 0)
                {
                    str2 = "[840100]";
                }
                else
                {
                    str2 = "[4c3629]";
                }
                if (info.isInQueue)
                {
                    string format = str2 + ConfigMgr.getInstance().GetWord(0x186b9);
                    this.TableGridHandTable[i].Model.Template.LabelReqDetail.text = string.Format(format, info.reqRankId);
                    this.TableGridHandTable[i].Model.Template.SpriteReqState.spriteName = "Ui_Gonghuifb_Icon_ysq";
                }
                else
                {
                    string str4 = str2 + ConfigMgr.getInstance().GetWord(0x186ba);
                    this.TableGridHandTable[i].Model.Template.LabelReqDetail.text = string.Format(str4, info.leftCnt, info.reqPlayerAllCnt);
                    this.TableGridHandTable[i].Model.Template.SpriteReqState.spriteName = "Ui_Gonghuifb_Icon_wsq";
                }
                storey.xxIndex = i;
                storey.xxItemId = info.entry;
                storey.xxItemQuality = info.quality;
                storey.xxItemCnt = info.leftCnt;
                if (info.reqPlayerInfoList.Count > 0)
                {
                    this.TableGridHandTable[i].Model.Template.Btn_Detail.gameObject.SetActive(true);
                }
                else
                {
                    this.TableGridHandTable[i].Model.Template.Btn_Detail.gameObject.SetActive(false);
                }
                this.TableGridHandTable[i].Model.Template.Btn_Detail.OnUIMouseClick(new Action<object>(storey.<>m__38F));
                this.TableGridHandTable[i].Model.Template.SpriteReqState.OnUIMouseClick(new Action<object>(storey.<>m__390));
            }
            else
            {
                <SetHandOutItemInfo>c__AnonStorey208 storey2 = new <SetHandOutItemInfo>c__AnonStorey208 {
                    <>f__this = this
                };
                str2 = "[4c3629]";
                string str5 = str2 + ConfigMgr.getInstance().GetWord(0x186ba);
                this.TableGridHandTable[i].Model.Template.LabelReqDetail.text = string.Format(str5, hoItemInfoList[i].leftCnt, hoItemInfoList[i].reqPlayerAllCnt);
                this.TableGridHandTable[i].Model.Template.SpriteReqState.spriteName = "Ui_Gonghuifb_Icon_wsq";
                storey2.xxIndex = i;
                storey2.xxItemId = hoItemInfoList[storey2.xxIndex].entry;
                storey2.xxItemQuality = hoItemInfoList[storey2.xxIndex].quality;
                storey2.xxItemCnt = hoItemInfoList[storey2.xxIndex].leftCnt;
                if (hoItemInfoList[i].reqPlayerInfoList.Count > 0)
                {
                    this.TableGridHandTable[i].Model.Template.Btn_Detail.gameObject.SetActive(true);
                }
                else
                {
                    this.TableGridHandTable[i].Model.Template.Btn_Detail.gameObject.SetActive(false);
                }
                this.TableGridHandTable[i].Model.Template.Btn_Detail.OnUIMouseClick(new Action<object>(storey2.<>m__391));
                this.TableGridHandTable[i].Model.Template.SpriteReqState.OnUIMouseClick(new Action<object>(storey2.<>m__392));
            }
            this.reqLock = false;
        }
    }

    private void SetPageInfo()
    {
        this.pageAllCnt = this.PageIdToItemsDic.Count;
        this.lb_pagenum.text = string.Format("{0}/{1}", this.curPage + 1, this.pageAllCnt);
    }

    public void SetScrollListInitPos(UIPanel scrList)
    {
        if (scrList != null)
        {
            if (scrList != null)
            {
                scrList.clipOffset = new Vector2(scrList.clipOffset.x, 0f);
                scrList.gameObject.transform.localPosition = new Vector3(scrList.gameObject.transform.localPosition.x, 0f, scrList.transform.localPosition.z);
            }
            SpringPanel component = scrList.GetComponent<SpringPanel>();
            if (component != null)
            {
                component.enabled = true;
                component.target = new Vector3(component.target.x, 0f, component.target.z);
                component.enabled = false;
            }
        }
    }

    private void TuenToNextPage()
    {
        List<int> list;
        if (this.PageIdToItemsDic.TryGetValue(this.curPage + 1, out list))
        {
            this.curPageItemsList = list;
            this.curreqPageState = ReqPageState.reqNext;
            SocketMgr.Instance.RequestC2S_GuildDupItemQueueInfo(this.curPageItemsList, DupId);
        }
        else
        {
            Debug.Log("Did not have this page____:" + this.curPage);
        }
    }

    private void TurnToPrePage()
    {
        List<int> list;
        if (this.PageIdToItemsDic.TryGetValue(this.curPage - 1, out list))
        {
            this.curPageItemsList = list;
            this.curreqPageState = ReqPageState.reqPre;
            SocketMgr.Instance.RequestC2S_GuildDupItemQueueInfo(this.curPageItemsList, DupId);
        }
        else
        {
            Debug.Log("Did not have this page____:" + this.curPage);
        }
    }

    protected UIWidget AutoClose { get; set; }

    protected UIButton bt_next { get; set; }

    protected UIButton bt_prv { get; set; }

    protected UIButton Btn_Close { get; set; }

    public static int DupId
    {
        get
        {
            return curDupId__;
        }
        set
        {
            curDupId__ = value;
        }
    }

    protected UIGrid GridHandTable { get; set; }

    protected UILabel LabelHandOutTitle { get; set; }

    protected UILabel LabelMainTips { get; set; }

    protected UILabel lb_pagenum { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    [CompilerGenerated]
    private sealed class <ClickDetail>c__AnonStorey209
    {
        private static Predicate<GuildMember> <>f__am$cache5;
        internal GuidBattleBossHandOutPanel <>f__this;
        internal int itemId;
        internal int leftCnt;
        internal int quality;
        internal List<GuidBattleItemHandQueuePanel.HandPlayerQueueInfo> xxPlayerQueueList;

        internal void <>m__393(GUIEntity entity)
        {
            GuidBattleItemHandQueuePanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleItemHandQueuePanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.curItemEntry = this.itemId;
                activityGUIEntity.curItemQuality = this.quality;
                activityGUIEntity.curItemLeftCnt = this.leftCnt;
                activityGUIEntity.dupId = GuidBattleBossHandOutPanel.DupId;
                bool isGuidMaster = false;
                GuildMemberData mGuildMemberData = ActorData.getInstance().mGuildMemberData;
                if (mGuildMemberData != null)
                {
                    if (<>f__am$cache5 == null)
                    {
                        <>f__am$cache5 = t => t.userInfo.id == ActorData.getInstance().SessionInfo.userid;
                    }
                    GuildMember member = mGuildMemberData.member.Find(<>f__am$cache5);
                    if (member != null)
                    {
                        if (member.position == 1)
                        {
                            isGuidMaster = true;
                        }
                        else
                        {
                            isGuidMaster = false;
                        }
                    }
                }
                activityGUIEntity.SetHandOutItemInfo(this.xxPlayerQueueList, this.<>f__this.manualTimes__, false, isGuidMaster);
            }
        }

        private static bool <>m__394(GuildMember t)
        {
            return (t.userInfo.id == ActorData.getInstance().SessionInfo.userid);
        }
    }

    [CompilerGenerated]
    private sealed class <SetHandOutItemInfo>c__AnonStorey207
    {
        internal GuidBattleBossHandOutPanel <>f__this;
        internal int xxIndex;
        internal int xxItemCnt;
        internal int xxItemId;
        internal int xxItemQuality;

        internal void <>m__38F(object u)
        {
            this.<>f__this.ClickDetail(this.xxIndex, this.xxItemId, this.xxItemQuality, this.xxItemCnt);
        }

        internal void <>m__390(object u)
        {
            this.<>f__this.ClickReq(this.xxItemId, this.xxIndex);
        }
    }

    [CompilerGenerated]
    private sealed class <SetHandOutItemInfo>c__AnonStorey208
    {
        internal GuidBattleBossHandOutPanel <>f__this;
        internal int xxIndex;
        internal int xxItemCnt;
        internal int xxItemId;
        internal int xxItemQuality;

        internal void <>m__391(object u)
        {
            this.<>f__this.ClickDetail(this.xxIndex, this.xxItemId, this.xxItemQuality, this.xxItemCnt);
        }

        internal void <>m__392(object u)
        {
            this.<>f__this.ClickReq(this.xxItemId, this.xxIndex);
        }
    }

    public class GridHandTableItemModel : TableItemModel<GuidBattleBossHandOutPanel.GridHandTableItemTemplate>
    {
        public override void Init(GuidBattleBossHandOutPanel.GridHandTableItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.Entry = -1;
            base.Template.SpriteItemQulity.OnUIMousePressOver(delegate (object u) {
                if ((this.Entry >= 0) && (GUITipManager.Current != null))
                {
                    GUITipManager.Current.DrawItemTip(base.Item.Root.GetInstanceID(), GUITipManager.Current.OffsetPos(base.Template.SpriteItemQulity.transform), new Vector3(230f, 20f, 0f), this.Entry);
                }
            });
        }

        public int Entry { get; set; }
    }

    public class GridHandTableItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.ReqBossItem = base.FindChild<UIDragScrollView>("ReqBossItem");
            this.SpriteItemQulity = base.FindChild<UISprite>("SpriteItemQulity");
            this.TextureItemIcon = base.FindChild<UITexture>("TextureItemIcon");
            this.LabelItemName = base.FindChild<UILabel>("LabelItemName");
            this.LabelReqDetail = base.FindChild<UILabel>("LabelReqDetail");
            this.Btn_Detail = base.FindChild<UIPlaySound>("Btn_Detail");
            this.Btn_Req = base.FindChild<Transform>("Btn_Req");
            this.SpriteReqState = base.FindChild<UISprite>("SpriteReqState");
        }

        public UIPlaySound Btn_Detail { get; private set; }

        public Transform Btn_Req { get; private set; }

        public UILabel LabelItemName { get; private set; }

        public UILabel LabelReqDetail { get; private set; }

        public UIDragScrollView ReqBossItem { get; private set; }

        public UISprite SpriteItemQulity { get; private set; }

        public UISprite SpriteReqState { get; private set; }

        public UITexture TextureItemIcon { get; private set; }
    }

    public class HandOutItemInfo
    {
        public int entry;
        public bool isInQueue;
        public int leftCnt;
        public int quality;
        public int reqPlayerAllCnt;
        public List<GuildDupDistributeUserInfo> reqPlayerInfoList;
        public int reqRankId;
    }

    private enum ReqPageState
    {
        reqPre,
        reqNext,
        reqNone
    }
}

