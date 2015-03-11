using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class GuidBattleItemHandQueuePanel : GUIPanelEntity
{
    private bool bForTest = true;
    public bool canHand;
    public int curItemEntry;
    public long curItemHandToPlayerId;
    public int curItemLeftCnt;
    public int curItemQuality;
    public int dupId;
    private Dictionary<int, List<HandPlayerQueueInfo>> DupToItemsQueueListDic = new Dictionary<int, List<HandPlayerQueueInfo>>();
    private const int GuidMasterCanHandMaxCnt = 1;
    private int handChanceCnt__;
    protected UITableManager<UIAutoGenItem<GridHandTableItemTemplate, GridHandTableItemModel>> TableGridHandTable = new UITableManager<UIAutoGenItem<GridHandTableItemTemplate, GridHandTableItemModel>>();
    public List<HandPlayerQueueInfo> xxTestHandPlayerQueueInfoList = new List<HandPlayerQueueInfo>();

    private void ClickHand(int index, long playerID)
    {
        this.curItemHandToPlayerId = playerID;
        SocketMgr.Instance.RequestCC2S_GuildDupItemManualDistribute(this.dupId, this.curItemEntry, playerID);
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridHandTable = base.FindChild<UIGrid>("GridHandTable");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.LabelHandOutTitle = base.FindChild<UILabel>("LabelHandOutTitle");
        this.GoItemInfo = base.FindChild<Transform>("GoItemInfo");
        this.SpriteItemBg = base.FindChild<UISprite>("SpriteItemBg");
        this.SpriteItemQulity = base.FindChild<UISprite>("SpriteItemQulity");
        this.TextureItemIcon = base.FindChild<UITexture>("TextureItemIcon");
        this.LabelItemName = base.FindChild<UILabel>("LabelItemName");
        this.LabelReqDetail = base.FindChild<UILabel>("LabelReqDetail");
        this.LabelNum = base.FindChild<UILabel>("LabelNum");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.TableGridHandTable.InitFromGrid(this.GridHandTable);
    }

    private void InitUiGridData(int itemCnt)
    {
        this.TableGridHandTable.Cache = false;
        this.TableGridHandTable.Count = itemCnt;
    }

    public void SetBottomItemInfo(List<HandPlayerQueueInfo> hoItemInfoList, bool isHandOutOk)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.curItemEntry);
        if (_config != null)
        {
            this.TextureItemIcon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            CommonFunc.SetEquipQualityBorder(this.SpriteItemQulity, _config.quality, false);
        }
        else
        {
            Debug.LogWarning("!!!item_config have not this entry : " + this.curItemEntry);
        }
        int count = hoItemInfoList.Count;
        if (isHandOutOk)
        {
            this.curItemLeftCnt--;
            if (this.curItemLeftCnt < 0)
            {
                this.curItemLeftCnt = 0;
            }
            Debug.LogWarning("!!!ReqPlayerCnt : " + count);
            if (count < 0)
            {
                count = 0;
            }
        }
        this.LabelNum.text = this.curItemLeftCnt.ToString();
        this.LabelReqDetail.text = string.Format(ConfigMgr.getInstance().GetWord(0x186c4), count);
        this.LabelItemName.text = _config.name;
    }

    public void SetHandOutItemInfo(List<HandPlayerQueueInfo> hoItemInfoList, int canHandCnt, bool isHandOutOk = false, bool isGuidMaster = false)
    {
        this.xxTestHandPlayerQueueInfoList = hoItemInfoList;
        if (isHandOutOk)
        {
            List<HandPlayerQueueInfo> list = new List<HandPlayerQueueInfo>();
            int num = 0x3e8;
            for (int j = 0; j < this.xxTestHandPlayerQueueInfoList.Count; j++)
            {
                if (this.xxTestHandPlayerQueueInfoList[j].userId == this.curItemHandToPlayerId)
                {
                    num = j;
                }
                if (j > num)
                {
                    HandPlayerQueueInfo item = new HandPlayerQueueInfo {
                        playerName = this.xxTestHandPlayerQueueInfoList[j].playerName,
                        playerLv = this.xxTestHandPlayerQueueInfoList[j].playerLv,
                        cardMainEntry = this.xxTestHandPlayerQueueInfoList[j].cardMainEntry,
                        cardQuality = this.xxTestHandPlayerQueueInfoList[j].cardQuality,
                        cardStarlv = this.xxTestHandPlayerQueueInfoList[j].cardStarlv,
                        userId = this.xxTestHandPlayerQueueInfoList[j].userId,
                        queueId = this.xxTestHandPlayerQueueInfoList[j].queueId - 1
                    };
                    list.Add(item);
                }
                else if (j != num)
                {
                    list.Add(this.xxTestHandPlayerQueueInfoList[j]);
                }
            }
            this.xxTestHandPlayerQueueInfoList = list;
        }
        this.handChanceCnt__ = canHandCnt;
        this.InitUiGridData(this.xxTestHandPlayerQueueInfoList.Count);
        int count = this.xxTestHandPlayerQueueInfoList.Count;
        for (int i = 0; i < count; i++)
        {
            <SetHandOutItemInfo>c__AnonStorey20B storeyb = new <SetHandOutItemInfo>c__AnonStorey20B {
                <>f__this = this
            };
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(this.xxTestHandPlayerQueueInfoList[i].cardMainEntry);
            this.TableGridHandTable[i].Model.Template.LabelPlayerName.text = this.xxTestHandPlayerQueueInfoList[i].playerName;
            this.TableGridHandTable[i].Model.Template.LabelPlayerLv.text = "Lv." + this.xxTestHandPlayerQueueInfoList[i].playerLv;
            this.TableGridHandTable[i].Model.Template.TextureHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            this.TableGridHandTable[i].Model.Template.SpriteQuality.spriteName = "Ui_Hero_Frame_" + (this.xxTestHandPlayerQueueInfoList[i].cardQuality + 1);
            this.TableGridHandTable[i].Model.StarCnt = this.xxTestHandPlayerQueueInfoList[i].cardStarlv;
            storeyb.xxIndex = this.xxTestHandPlayerQueueInfoList[i].queueId;
            this.TableGridHandTable[i].Model.Template.LabelQueueId.text = storeyb.xxIndex.ToString();
            storeyb.xxIndex = i;
            storeyb.xxplayerId = this.xxTestHandPlayerQueueInfoList[i].userId;
            this.TableGridHandTable[i].Model.Template.Btn_Hand.OnUIMouseClick(new Action<object>(storeyb.<>m__3A5));
            if (isGuidMaster)
            {
                this.TableGridHandTable[i].Model.Template.Btn_Hand.gameObject.SetActive(true);
                if (this.handChanceCnt__ > 1)
                {
                    this.TableGridHandTable[i].Model.Template.Btn_Hand.gameObject.SetActive(false);
                }
                else if ((this.handChanceCnt__ > 0) && (this.handChanceCnt__ <= 1))
                {
                    this.TableGridHandTable[i].Model.Template.Btn_Hand.enabled = true;
                    this.TableGridHandTable[i].Model.Template.Btn_Hand.gameObject.SetActive(true);
                }
                else
                {
                    this.TableGridHandTable[i].Model.Template.Btn_Hand.enabled = false;
                    this.TableGridHandTable[i].Model.Template.Btn_Hand.gameObject.SetActive(true);
                }
            }
            else
            {
                this.TableGridHandTable[i].Model.Template.Btn_Hand.gameObject.SetActive(false);
            }
        }
        this.SetBottomItemInfo(this.xxTestHandPlayerQueueInfoList, isHandOutOk);
    }

    protected UIWidget AutoClose { get; set; }

    protected UIButton Btn_Close { get; set; }

    protected Transform GoItemInfo { get; set; }

    protected UIGrid GridHandTable { get; set; }

    public int HadnChangeCnt
    {
        get
        {
            return this.handChanceCnt__;
        }
        set
        {
            this.handChanceCnt__ = value;
        }
    }

    protected UILabel LabelHandOutTitle { get; set; }

    protected UILabel LabelItemName { get; set; }

    protected UILabel LabelNum { get; set; }

    protected UILabel LabelReqDetail { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    protected UISprite SpriteItemBg { get; set; }

    protected UISprite SpriteItemQulity { get; set; }

    protected UITexture TextureItemIcon { get; set; }

    [CompilerGenerated]
    private sealed class <SetHandOutItemInfo>c__AnonStorey20B
    {
        internal GuidBattleItemHandQueuePanel <>f__this;
        internal int xxIndex;
        internal long xxplayerId;

        internal void <>m__3A5(object u)
        {
            this.<>f__this.ClickHand(this.xxIndex, this.xxplayerId);
        }
    }

    public class GridHandTableItemModel : TableItemModel<GuidBattleItemHandQueuePanel.GridHandTableItemTemplate>
    {
        private UITableManager<UIStarItem> StarTable = new UITableManager<UIStarItem>();

        public override void Init(GuidBattleItemHandQueuePanel.GridHandTableItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.StarTable.InitFromGrid(base.Template.GridStar);
        }

        public int StarCnt
        {
            get
            {
                return this.StarTable.Count;
            }
            set
            {
                this.StarTable.Count = value;
                Vector3 localPosition = base.Template.GridStar.transform.localPosition;
                base.Template.GridStar.transform.localPosition = new Vector3((float) (5 + (value * -5)), localPosition.y, localPosition.z);
            }
        }

        public class UIStarItem : UITableItem
        {
            public override void OnCreate()
            {
            }
        }
    }

    public class GridHandTableItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.ReqQueueItem = base.FindChild<UIDragScrollView>("ReqQueueItem");
            this.LabelQueueId = base.FindChild<UILabel>("LabelQueueId");
            this.TextureHeadIcon = base.FindChild<UITexture>("TextureHeadIcon");
            this.SpriteQuality = base.FindChild<UISprite>("SpriteQuality");
            this.LabelPlayerName = base.FindChild<UILabel>("LabelPlayerName");
            this.LabelPlayerLv = base.FindChild<UILabel>("LabelPlayerLv");
            this.Btn_Hand = base.FindChild<UIButton>("Btn_Hand");
            this.GridStar = base.FindChild<UIGrid>("GridStar");
        }

        public UIButton Btn_Hand { get; private set; }

        public UIGrid GridStar { get; private set; }

        public UILabel LabelPlayerLv { get; private set; }

        public UILabel LabelPlayerName { get; private set; }

        public UILabel LabelQueueId { get; private set; }

        public UIDragScrollView ReqQueueItem { get; private set; }

        public UISprite SpriteQuality { get; private set; }

        public UITexture TextureHeadIcon { get; private set; }
    }

    public class HandPlayerQueueInfo
    {
        public int cardMainEntry;
        public int cardQuality;
        public int cardStarlv;
        public int playerLv;
        public string playerName;
        public int queueId;
        public long userId;
    }
}

