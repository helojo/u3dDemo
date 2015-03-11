using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class DetainsDartInviteFriendPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cacheD;
    private int curPage;
    private List<long> curPageItemsList = new List<long>();
    private int pageAllCnt;
    private Dictionary<int, List<long>> PageIdToFriendListDic = new Dictionary<int, List<long>>();
    protected UITableManager<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>> TableGridRank = new UITableManager<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>>();

    private void GetPageToItemsInfo()
    {
        List<long> list3;
        List<int> list = new List<int>();
        foreach (Friend friend in ActorData.getInstance().FriendList)
        {
            list.Add((int) friend.userInfo.id);
        }
        int key = 0;
        int onePageMaxFriendCnt = XSingleton<GameDetainsDartMgr>.Singleton.onePageMaxFriendCnt;
        for (int i = 0; i < list.Count; i++)
        {
            if (onePageMaxFriendCnt == 0)
            {
                Debug.Log("PageCnt is Error！ PageCnt：" + onePageMaxFriendCnt);
                return;
            }
            key = i / onePageMaxFriendCnt;
            int num4 = i % onePageMaxFriendCnt;
            if (!this.PageIdToFriendListDic.ContainsKey(key))
            {
                List<long> list2 = new List<long>();
                this.PageIdToFriendListDic.Add(key, list2);
                if (!this.PageIdToFriendListDic[key].Contains((long) list[i]))
                {
                    this.PageIdToFriendListDic[key].Add((long) list[i]);
                }
                else
                {
                    Debug.Log("ItemIds have same ItemId from config！！！");
                }
            }
            else if (!this.PageIdToFriendListDic[key].Contains((long) list[i]))
            {
                this.PageIdToFriendListDic[key].Add((long) list[i]);
            }
            else
            {
                Debug.Log("ItemIds have same ItemId from config！！！");
            }
        }
        if (this.PageIdToFriendListDic.TryGetValue(this.curPage, out list3))
        {
            this.curPageItemsList = list3;
            XSingleton<GameDetainsDartMgr>.Singleton.CurReqPageFriendIdList = list3;
        }
        else
        {
            Debug.Log("ItemsList of curPage is Error！！！+++:::" + this.curPage);
        }
        this.pageAllCnt = this.PageIdToFriendListDic.Count;
        this.SetPageInfo();
    }

    public override void Initialize()
    {
        base.Initialize();
        XSingleton<GameDetainsDartMgr>.Singleton.curPageFriendInfo.Clear();
        for (int i = 0; i < 10; i++)
        {
            GameDetainsDartMgr.FriendItemData item = new GameDetainsDartMgr.FriendItemData {
                mainCardEntry = i,
                mainCardQuality = i,
                playerLv = i,
                playerName = "Name" + i,
                cards = new List<CardInfo>()
            };
            for (int j = 0; j < 5; j++)
            {
                CardInfo info = new CardInfo {
                    entry = (uint) j,
                    level = 10 + j,
                    quality = j,
                    starLv = j
                };
                item.cards.Add(info);
            }
            XSingleton<GameDetainsDartMgr>.Singleton.curPageFriendInfo.Add(item);
        }
        if (<>f__am$cacheD == null)
        {
            <>f__am$cacheD = u => GUIMgr.Instance.PopGUIEntity();
        }
        this.Btn_Close.OnUIMouseClick(<>f__am$cacheD);
        this.Btn_Pre.OnUIMouseClick(u => this.PrePage());
        this.Btn_Next.OnUIMouseClick(u => this.NextPage());
        this.SetFriendInfo(XSingleton<GameDetainsDartMgr>.Singleton.curPageFriendInfo, true);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.Btn_Pre = base.FindChild<UIButton>("Btn_Pre");
        this.lb_pagenum = base.FindChild<UILabel>("lb_pagenum");
        this.Btn_Next = base.FindChild<UIButton>("Btn_Next");
        this.LabelDupTitle = base.FindChild<UILabel>("LabelDupTitle");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridRank = base.FindChild<UIGrid>("GridRank");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.TableGridRank.InitFromGrid(this.GridRank);
    }

    private void NextPage()
    {
        Debug.LogWarning("NextPage!");
    }

    private void OnClickAskHelpForBattle(int mainCardEntry, int quality)
    {
        Debug.LogWarning("OnClickAskHelpForBattle : __" + mainCardEntry);
        DetainsDartSelFlagAndTeamPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<DetainsDartSelFlagAndTeamPanel>();
        if (gUIEntity != null)
        {
            gUIEntity.SetInviteFriendDataInfo(mainCardEntry, quality);
            GUIMgr.Instance.PopGUIEntity();
        }
    }

    private void OnClickAskHelpForInvite(int mainCardEntry, int quality)
    {
        Debug.LogWarning("OnClickAskHelpForInvite : __" + mainCardEntry);
        DetainsDartSelFlagAndTeamPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<DetainsDartSelFlagAndTeamPanel>();
        if (gUIEntity != null)
        {
            gUIEntity.SetInviteFriendDataInfo(mainCardEntry, quality);
            GUIMgr.Instance.PopGUIEntity();
        }
    }

    private void PrePage()
    {
        Debug.LogWarning("PrePage!");
    }

    public void SetFriendInfo(List<GameDetainsDartMgr.FriendItemData> rankInfodataList, bool isForEscort = true)
    {
        <SetFriendInfo>c__AnonStorey1B2 storeyb = new <SetFriendInfo>c__AnonStorey1B2 {
            rankInfodataList = rankInfodataList,
            <>f__this = this
        };
        Debug.LogWarning("SetFriendInfo!");
        this.GridRank.cellWidth = 600f;
        this.GridRank.cellHeight = 130f;
        this.TableGridRank.Count = storeyb.rankInfodataList.Count;
        for (int i = 0; i < this.TableGridRank.Count; i++)
        {
            <SetFriendInfo>c__AnonStorey1B3 storeyb2 = new <SetFriendInfo>c__AnonStorey1B3 {
                <>f__ref$434 = storeyb,
                <>f__this = this,
                indexTemp = i
            };
            if (isForEscort)
            {
                this.TableGridRank[i].Model.Template.Btn_ForHelp.OnUIMouseClick(new Action<object>(storeyb2.<>m__246));
            }
            else
            {
                this.TableGridRank[i].Model.Template.Btn_ForHelp.OnUIMouseClick(new Action<object>(storeyb2.<>m__247));
            }
            this.TableGridRank[i].Model.Template.LabelBattleNum.text = "战斗力：" + 0x2710;
            this.TableGridRank[i].Model.Template.LabelPlayerName.text = storeyb.rankInfodataList[i].playerName.ToString();
            this.TableGridRank[i].Model.Template.LabelGuidLv.text = "Lv." + storeyb.rankInfodataList[i].playerLv.ToString();
            this.TableGridRank[i].Model.Template.SpriteQuality.name = "Ui_Hero_Frame_" + (storeyb.rankInfodataList[i].mainCardQuality + 1);
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(storeyb.rankInfodataList[i].mainCardEntry);
            if (_config != null)
            {
                this.TableGridRank[i].Model.Template.TexturHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            }
            else
            {
                Debug.Log("SetRankInfo ::MainEntry is out of card_config!!!");
            }
            this.TableGridRank[i].Model.TeamCardCnt = storeyb.rankInfodataList[i].cards.Count;
            for (int j = 0; j < storeyb.rankInfodataList[i].cards.Count; j++)
            {
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) storeyb.rankInfodataList[i].cards[j].entry);
                this.TableGridRank[i].Model.TeamCardTable[j].TexturSmHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                this.TableGridRank[i].Model.TeamCardTable[j].SpriteSmQuality.spriteName = "Ui_Hero_Frame_" + (storeyb.rankInfodataList[i].cards[j].quality + 1);
                this.TableGridRank[i].Model.TeamCardTable[j].LabelCardLv.text = storeyb.rankInfodataList[i].cards[j].level.ToString();
                this.TableGridRank[i].Model.TeamCardTable[j].startCnt = storeyb.rankInfodataList[i].cards[j].starLv;
            }
        }
    }

    private void SetPageInfo()
    {
        this.pageAllCnt = this.PageIdToFriendListDic.Count;
        this.lb_pagenum.text = string.Format("{0}/{1}", this.curPage + 1, this.pageAllCnt);
    }

    protected UIWidget AutoClose { get; set; }

    protected UIButton Btn_Close { get; set; }

    protected UIButton Btn_Next { get; set; }

    protected UIButton Btn_Pre { get; set; }

    protected UIGrid GridRank { get; set; }

    protected UILabel LabelDupTitle { get; set; }

    protected UILabel lb_pagenum { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    [CompilerGenerated]
    private sealed class <SetFriendInfo>c__AnonStorey1B2
    {
        internal DetainsDartInviteFriendPanel <>f__this;
        internal List<GameDetainsDartMgr.FriendItemData> rankInfodataList;
    }

    [CompilerGenerated]
    private sealed class <SetFriendInfo>c__AnonStorey1B3
    {
        internal DetainsDartInviteFriendPanel.<SetFriendInfo>c__AnonStorey1B2 <>f__ref$434;
        internal DetainsDartInviteFriendPanel <>f__this;
        internal int indexTemp;

        internal void <>m__246(object u)
        {
            this.<>f__this.OnClickAskHelpForInvite(this.<>f__ref$434.rankInfodataList[this.indexTemp].mainCardEntry, this.<>f__ref$434.rankInfodataList[this.indexTemp].mainCardQuality);
        }

        internal void <>m__247(object u)
        {
            this.<>f__this.OnClickAskHelpForBattle(this.<>f__ref$434.rankInfodataList[this.indexTemp].mainCardEntry, this.<>f__ref$434.rankInfodataList[this.indexTemp].mainCardQuality);
        }
    }

    public class GridRankItemModel : TableItemModel<DetainsDartInviteFriendPanel.GridRankItemTemplate>
    {
        public UITableManager<UITeamCardItem> TeamCardTable = new UITableManager<UITeamCardItem>();

        public override void Init(DetainsDartInviteFriendPanel.GridRankItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.TeamCardTable.InitFromGrid(base.Template.Table.GetComponent<UIGrid>());
        }

        public int TeamCardCnt
        {
            get
            {
                return this.TeamCardTable.Count;
            }
            set
            {
                this.TeamCardTable.Count = value;
                base.Template.Table.transform.localPosition = new Vector3(0f, -35f, 0f);
                Vector3 localPosition = base.Template.Table.transform.localPosition;
            }
        }

        public class UITeamCardItem : UITableItem
        {
            private UITableManager<UIStartItem> StartTabel = new UITableManager<UIStartItem>();

            public override void OnCreate()
            {
                this.TexturSmHeadIcon = base.FindChild<UITexture>("TexturSmHeadIcon");
                this.SpriteSmHeadBg = base.FindChild<UISprite>("SpriteSmHeadBg");
                this.SpriteSmQuality = base.FindChild<UISprite>("SpriteSmQuality");
                this.LabelCardLv = base.FindChild<UILabel>("LabelCardLv");
                this.GoStartInfo = base.FindChild<UIGrid>("GoStartInfo");
                this.StartTabel.InitFromGrid(this.GoStartInfo);
            }

            public UIGrid GoStartInfo { get; private set; }

            public UILabel LabelCardLv { get; private set; }

            public UISprite SpriteSmHeadBg { get; private set; }

            public UISprite SpriteSmQuality { get; private set; }

            public int startCnt
            {
                get
                {
                    return this.StartTabel.Count;
                }
                set
                {
                    this.StartTabel.Count = value;
                    Vector3 localPosition = this.GoStartInfo.transform.localPosition;
                    this.GoStartInfo.transform.localPosition = new Vector3((float) (15 + (value * -5)), -18f, localPosition.z);
                }
            }

            public UITexture TexturSmHeadIcon { get; private set; }

            public class UIStartItem : UITableItem
            {
                public override void OnCreate()
                {
                }
            }
        }
    }

    public class GridRankItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.TimeRankItem = base.FindChild<UIDragScrollView>("TimeRankItem");
            this.GoHeadInInfo = base.FindChild<Transform>("GoHeadInInfo");
            this.SpriteHeadBg = base.FindChild<UISprite>("SpriteHeadBg");
            this.TexturHeadIcon = base.FindChild<UITexture>("TexturHeadIcon");
            this.SpriteQuality = base.FindChild<UISprite>("SpriteQuality");
            this.GoPlayerInfo = base.FindChild<Transform>("GoPlayerInfo");
            this.LabeKillTime = base.FindChild<UILabel>("LabeKillTime");
            this.LabelKillTimeTiitle = base.FindChild<UILabel>("LabelKillTimeTiitle");
            this.LabelPlayerName = base.FindChild<UILabel>("LabelPlayerName");
            this.LabelBattleNum = base.FindChild<UILabel>("LabelBattleNum");
            this.LabelGuidLv = base.FindChild<UILabel>("LabelGuidLv");
            this.GoPlayCardsInfo = base.FindChild<Transform>("GoPlayCardsInfo");
            this.Table = base.FindChild<UIGrid>("Table");
            this.SpriteSmQuality = base.FindChild<UISprite>("SpriteSmQuality");
            this.TexturSmHeadIcon = base.FindChild<UITexture>("TexturSmHeadIcon");
            this.SpriteSmHeadBg = base.FindChild<UISprite>("SpriteSmHeadBg");
            this.LabelCardLv = base.FindChild<UILabel>("LabelCardLv");
            this.GoStartInfo = base.FindChild<UIGrid>("GoStartInfo");
            this.LabelBossHaveNoData = base.FindChild<UILabel>("LabelBossHaveNoData");
            this.Btn_ForHelp = base.FindChild<UIButton>("Btn_ForHelp");
        }

        public UIButton Btn_ForHelp { get; private set; }

        public Transform GoHeadInInfo { get; private set; }

        public Transform GoPlayCardsInfo { get; private set; }

        public Transform GoPlayerInfo { get; private set; }

        public UIGrid GoStartInfo { get; private set; }

        public UILabel LabeKillTime { get; private set; }

        public UILabel LabelBattleNum { get; private set; }

        public UILabel LabelBossHaveNoData { get; private set; }

        public UILabel LabelCardLv { get; private set; }

        public UILabel LabelGuidLv { get; private set; }

        public UILabel LabelKillTimeTiitle { get; private set; }

        public UILabel LabelPlayerName { get; private set; }

        public UISprite SpriteHeadBg { get; private set; }

        public UISprite SpriteQuality { get; private set; }

        public UISprite SpriteSmHeadBg { get; private set; }

        public UISprite SpriteSmQuality { get; private set; }

        public UIGrid Table { get; private set; }

        public UITexture TexturHeadIcon { get; private set; }

        public UITexture TexturSmHeadIcon { get; private set; }

        public UIDragScrollView TimeRankItem { get; private set; }
    }
}

