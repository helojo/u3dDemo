using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class DetainsDartBattleRecordPanel : GUIPanelEntity
{
    private int curPage;
    private List<long> curPageItemsList = new List<long>();
    private int pageAllCnt;
    private Dictionary<int, List<long>> PageIdToEnermyListDic = new Dictionary<int, List<long>>();
    private Dictionary<int, List<ConvoyEnemyInfo>> pageIdToEnermyListInfoDic = new Dictionary<int, List<ConvoyEnemyInfo>>();
    protected UITableManager<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>> TableGridRank = new UITableManager<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>>();

    private void GetPageToItemsInfo(List<ConvoyEnemyInfo> rankInfodataList)
    {
        this.SetPageInfo();
    }

    public override void Initialize()
    {
        base.Initialize();
        XSingleton<GameDetainsDartMgr>.Singleton.curPageEnermyInfo.Clear();
        for (int i = 0; i < 10; i++)
        {
            GameDetainsDartMgr.EnermyItemData item = new GameDetainsDartMgr.EnermyItemData {
                mainCardEntry = 10 + i,
                mainCardQuality = i,
                playerLv = i,
                playerName = "仇人Name" + i,
                cards = new List<CardInfo>()
            };
            for (int j = 0; j < 5; j++)
            {
                CardInfo info = new CardInfo {
                    entry = (uint) (i + j),
                    level = 10 + j,
                    quality = j,
                    starLv = j
                };
                item.cards.Add(info);
            }
            XSingleton<GameDetainsDartMgr>.Singleton.curPageEnermyInfo.Add(item);
        }
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.Btn_Pre.OnUIMouseClick(u => this.PrePage());
        this.Btn_Next.OnUIMouseClick(u => this.NextPage());
        this.SetPageInfo();
        SocketMgr.Instance.RequestC2S_GetConvoyEnemyList();
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
        this.LabelBossHaveNoData = base.FindChild<UILabel>("LabelBossHaveNoData");
        this.TableGridRank.InitFromGrid(this.GridRank);
    }

    private void NextPage()
    {
        Debug.LogWarning("NextPage!");
    }

    private void OnClickBattleBack(ConvoyEnemyInfo enemyInfo)
    {
        <OnClickBattleBack>c__AnonStorey1B1 storeyb = new <OnClickBattleBack>c__AnonStorey1B1 {
            enemyInfo = enemyInfo
        };
        Debug.LogWarning("OnClickBattleBack : __" + storeyb.enemyInfo.name);
        XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo = storeyb.enemyInfo;
        GUIMgr.Instance.DoModelGUI("TargetInfoPanel", new Action<GUIEntity>(storeyb.<>m__242), null);
    }

    private void OnClickBattleBack(GameDetainsDartMgr.EnermyItemData EnermyInfo)
    {
        <OnClickBattleBack>c__AnonStorey1B0 storeyb = new <OnClickBattleBack>c__AnonStorey1B0 {
            EnermyInfo = EnermyInfo
        };
        Debug.LogWarning("OnClickBattleBack : __" + storeyb.EnermyInfo.mainCardEntry);
        GUIMgr.Instance.DoModelGUI("TargetInfoPanel", new Action<GUIEntity>(storeyb.<>m__241), null);
    }

    private void PrePage()
    {
        Debug.LogWarning("PrePage!");
    }

    public void SetEnermyInfo(List<ConvoyEnemyInfo> rankInfodataList)
    {
        Debug.LogWarning("SetEnermyInfo!");
        this.GridRank.cellWidth = 600f;
        this.GridRank.cellHeight = 130f;
        this.TableGridRank.Count = rankInfodataList.Count;
        for (int i = 0; i < this.TableGridRank.Count; i++)
        {
            <SetEnermyInfo>c__AnonStorey1AF storeyaf = new <SetEnermyInfo>c__AnonStorey1AF {
                <>f__this = this
            };
            int num2 = i;
            storeyaf.xxEnermyInfo = rankInfodataList[i];
            this.TableGridRank[i].Model.Template.Btn_BattleBack.OnUIMouseClick(new Action<object>(storeyaf.<>m__240));
            string flagName = string.Empty;
            convoy_flag_config _config = ConfigMgr.getInstance().getByEntry<convoy_flag_config>(rankInfodataList[i].flagId);
            if (_config != null)
            {
                flagName = _config.flagName;
            }
            this.TableGridRank[i].Model.Template.LabelBattleRecordTips_1.text = string.Format("你第{0}队护送的{1}队伍被偷袭了", rankInfodataList[i].convoyIndex + 1, flagName);
            this.TableGridRank[i].Model.Template.LabelBattleRecordTips_2.text = string.Format("损失了---金币：{0}   勇气点：{1}", rankInfodataList[i].gold, rankInfodataList[i].arenaLadderScore);
            this.TableGridRank[i].Model.Template.LabelPlayerName.text = rankInfodataList[i].name.ToString();
            this.TableGridRank[i].Model.Template.LabelPlayerLv.text = "Lv." + rankInfodataList[i].lvl.ToString();
            CommonFunc.SetPlayerHeadFrame(this.TableGridRank[i].Model.Template.SpriteQuality, this.TableGridRank[i].Model.Template.QIcon, rankInfodataList[i].head_frame_entry);
            card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(rankInfodataList[i].headEntry);
            if (_config2 != null)
            {
                this.TableGridRank[i].Model.Template.TexturHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
            }
            else
            {
                Debug.Log("SetRankInfo ::MainEntry is out of card_config!!!");
            }
        }
    }

    public void SetEnermyInfo(List<GameDetainsDartMgr.EnermyItemData> rankInfodataList)
    {
        Debug.LogWarning("SetEnermyInfo!");
        this.GridRank.cellWidth = 600f;
        this.GridRank.cellHeight = 130f;
        if (rankInfodataList.Count > XSingleton<GameDetainsDartMgr>.Singleton.onePageMaxFriendCnt)
        {
            this.TableGridRank.Count = XSingleton<GameDetainsDartMgr>.Singleton.onePageMaxFriendCnt;
        }
        else
        {
            this.TableGridRank.Count = rankInfodataList.Count;
        }
        for (int i = 0; i < this.TableGridRank.Count; i++)
        {
            <SetEnermyInfo>c__AnonStorey1AE storeyae = new <SetEnermyInfo>c__AnonStorey1AE {
                <>f__this = this
            };
            int num2 = i;
            storeyae.xxEnermyInfo = rankInfodataList[i];
            this.TableGridRank[i].Model.Template.Btn_BattleBack.OnUIMouseClick(new Action<object>(storeyae.<>m__23F));
            this.TableGridRank[i].Model.Template.LabelBattleRecordTips_1.text = "在你护送旗帜的途中偷袭了你的队伍";
            this.TableGridRank[i].Model.Template.LabelBattleRecordTips_2.text = "损失了---金币：99999   勇气点：15";
            this.TableGridRank[i].Model.Template.LabelPlayerName.text = rankInfodataList[i].playerName.ToString();
            this.TableGridRank[i].Model.Template.LabelPlayerLv.text = "Lv." + rankInfodataList[i].playerLv.ToString();
            this.TableGridRank[i].Model.Template.SpriteQuality.name = "Ui_Hero_Frame_" + (rankInfodataList[i].mainCardQuality + 1);
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(rankInfodataList[i].mainCardEntry);
            if (_config != null)
            {
                this.TableGridRank[i].Model.Template.TexturHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            }
            else
            {
                Debug.Log("SetRankInfo ::MainEntry is out of card_config!!!");
            }
        }
    }

    private void SetPageInfo()
    {
        this.pageAllCnt = 1;
        this.lb_pagenum.text = string.Format("{0}/{1}", this.curPage + 1, this.pageAllCnt);
    }

    protected UIWidget AutoClose { get; set; }

    protected UIButton Btn_Close { get; set; }

    protected UIButton Btn_Next { get; set; }

    protected UIButton Btn_Pre { get; set; }

    protected UIGrid GridRank { get; set; }

    protected UILabel LabelBossHaveNoData { get; set; }

    protected UILabel LabelDupTitle { get; set; }

    protected UILabel lb_pagenum { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    [CompilerGenerated]
    private sealed class <OnClickBattleBack>c__AnonStorey1B0
    {
        internal GameDetainsDartMgr.EnermyItemData EnermyInfo;

        internal void <>m__241(GUIEntity obj)
        {
            ((TargetInfoPanel) obj).UpdateDetainsDartData(this.EnermyInfo);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickBattleBack>c__AnonStorey1B1
    {
        internal ConvoyEnemyInfo enemyInfo;

        internal void <>m__242(GUIEntity obj)
        {
            ((TargetInfoPanel) obj).UpdateDetainsDartData(this.enemyInfo);
        }
    }

    [CompilerGenerated]
    private sealed class <SetEnermyInfo>c__AnonStorey1AE
    {
        internal DetainsDartBattleRecordPanel <>f__this;
        internal GameDetainsDartMgr.EnermyItemData xxEnermyInfo;

        internal void <>m__23F(object u)
        {
            this.<>f__this.OnClickBattleBack(this.xxEnermyInfo);
        }
    }

    [CompilerGenerated]
    private sealed class <SetEnermyInfo>c__AnonStorey1AF
    {
        internal DetainsDartBattleRecordPanel <>f__this;
        internal ConvoyEnemyInfo xxEnermyInfo;

        internal void <>m__240(object u)
        {
            this.<>f__this.OnClickBattleBack(this.xxEnermyInfo);
        }
    }

    public class GridRankItemModel : TableItemModel<DetainsDartBattleRecordPanel.GridRankItemTemplate>
    {
        public override void Init(DetainsDartBattleRecordPanel.GridRankItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
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
            this.QIcon = base.FindChild<UISprite>("QIcon");
            this.GoPlayerInfo = base.FindChild<Transform>("GoPlayerInfo");
            this.LabeKillTime = base.FindChild<UILabel>("LabeKillTime");
            this.LabelKillTimeTiitle = base.FindChild<UILabel>("LabelKillTimeTiitle");
            this.LabelPlayerName = base.FindChild<UILabel>("LabelPlayerName");
            this.LabelBattleRecordTips_1 = base.FindChild<UILabel>("LabelBattleRecordTips_1");
            this.LabelPlayerLv = base.FindChild<UILabel>("LabelPlayerLv");
            this.LabelBattleRecordTips_2 = base.FindChild<UILabel>("LabelBattleRecordTips_2");
            this.Table = base.FindChild<UIGrid>("Table");
            this.SpriteSmQuality = base.FindChild<UISprite>("SpriteSmQuality");
            this.TexturSmHeadIcon = base.FindChild<UITexture>("TexturSmHeadIcon");
            this.SpriteSmHeadBg = base.FindChild<UISprite>("SpriteSmHeadBg");
            this.LabelCardLv = base.FindChild<UILabel>("LabelCardLv");
            this.GoStartInfo = base.FindChild<UIGrid>("GoStartInfo");
            this.Btn_BattleBack = base.FindChild<UIButton>("Btn_BattleBack");
        }

        public UIButton Btn_BattleBack { get; private set; }

        public Transform GoHeadInInfo { get; private set; }

        public Transform GoPlayerInfo { get; private set; }

        public UIGrid GoStartInfo { get; private set; }

        public UILabel LabeKillTime { get; private set; }

        public UILabel LabelBattleRecordTips_1 { get; private set; }

        public UILabel LabelBattleRecordTips_2 { get; private set; }

        public UILabel LabelCardLv { get; private set; }

        public UILabel LabelKillTimeTiitle { get; private set; }

        public UILabel LabelPlayerLv { get; private set; }

        public UILabel LabelPlayerName { get; private set; }

        public UISprite QIcon { get; private set; }

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

