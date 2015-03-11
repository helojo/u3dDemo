using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class GuidBattleBossRicherRankPanel : GUIPanelEntity
{
    private bool bForTest = true;
    public bool canHand;
    private int curBossId__;
    private int curDupId__;
    protected UITableManager<UIAutoGenItem<GridDamageItemTemplate, GridDamageItemModel>> TableGridDamage = new UITableManager<UIAutoGenItem<GridDamageItemTemplate, GridDamageItemModel>>();
    public List<HelpGoldItemInfo> xxTestHelpGoldInfoList = new List<HelpGoldItemInfo>();

    private void ClickHelpGold()
    {
        GUIMgr.Instance.ExitModelGUI(this);
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.Btn_Help.OnUIMouseClick(u => this.ClickHelpGold());
        this.SetHandOutItemInfo(this.xxTestHelpGoldInfoList);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridDamage = base.FindChild<UIGrid>("GridDamage");
        this.LabelTltle = base.FindChild<UILabel>("LabelTltle");
        this.Btn_Help = base.FindChild<UIButton>("Btn_Help");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.TableGridDamage.InitFromGrid(this.GridDamage);
    }

    private void InitUiGridData(int itemCnt)
    {
        this.TableGridDamage.Cache = false;
        this.TableGridDamage.Count = itemCnt;
    }

    public void ReceiveGuildDupSupportRank(S2C_GuildDupSupportRank supportRankData, bool bForTest = false)
    {
        if (!bForTest)
        {
            this.xxTestHelpGoldInfoList.Clear();
            List<HelpGoldItemInfo> list = new List<HelpGoldItemInfo>();
            int num = 0;
            foreach (GuildDupRankUserInfo info in supportRankData.users)
            {
                num++;
                HelpGoldItemInfo item = new HelpGoldItemInfo {
                    playerName = info.name,
                    playerLv = info.lvl,
                    rankId = num,
                    helpGoldNum = (int) info.damage,
                    cardMainEntry = info.headEntry,
                    cardQuality = 0,
                    cardStarlv = 0
                };
                if (((int) info.damage) >= 1)
                {
                    this.xxTestHelpGoldInfoList.Add(item);
                }
            }
        }
        else
        {
            this.xxTestHelpGoldInfoList.Clear();
            for (int i = 0; i < 5; i++)
            {
                HelpGoldItemInfo info3 = new HelpGoldItemInfo {
                    playerName = ConfigMgr.getInstance().GetWord(0x186c2) + i,
                    playerLv = i,
                    rankId = i + 1,
                    helpGoldNum = 0x186a0 - (i * 0x2710),
                    cardMainEntry = i,
                    cardQuality = 0,
                    cardStarlv = 0
                };
                this.xxTestHelpGoldInfoList.Add(info3);
            }
        }
        this.SetHandOutItemInfo(this.xxTestHelpGoldInfoList);
    }

    public void SetHandOutItemInfo(List<HelpGoldItemInfo> hoItemInfoList)
    {
        this.InitUiGridData(hoItemInfoList.Count);
        int count = hoItemInfoList.Count;
        for (int i = 0; i < count; i++)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(hoItemInfoList[i].cardMainEntry);
            this.TableGridDamage[i].Model.Template.LabelPlayerName.text = hoItemInfoList[i].playerName;
            this.TableGridDamage[i].Model.Template.LabelPlayerLv.text = "Lv." + hoItemInfoList[i].playerLv;
            this.TableGridDamage[i].Model.Template.TexturHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            this.TableGridDamage[i].Model.Template.SpriteQuality.spriteName = "Ui_Hero_Frame_" + (hoItemInfoList[i].cardQuality + 1);
            this.TableGridDamage[i].Model.StarCnt = hoItemInfoList[i].cardStarlv;
            this.TableGridDamage[i].Model.Template.LabelRankId.text = hoItemInfoList[i].rankId.ToString();
            this.TableGridDamage[i].Model.Template.LabelGoldNum.text = hoItemInfoList[i].helpGoldNum.ToString();
        }
    }

    protected UIWidget AutoClose { get; set; }

    public int BossId
    {
        get
        {
            return this.curBossId__;
        }
        set
        {
            this.curBossId__ = value;
        }
    }

    protected UIButton Btn_Close { get; set; }

    protected UIButton Btn_Help { get; set; }

    public int DupId
    {
        get
        {
            return this.curDupId__;
        }
        set
        {
            this.curDupId__ = value;
        }
    }

    protected UIGrid GridDamage { get; set; }

    protected UILabel LabelTltle { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    public class GridDamageItemModel : TableItemModel<GuidBattleBossRicherRankPanel.GridDamageItemTemplate>
    {
        private UITableManager<UIStarItem> StarTable = new UITableManager<UIStarItem>();

        public override void Init(GuidBattleBossRicherRankPanel.GridDamageItemTemplate template, UITableItem item)
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
                base.Template.GridStar.transform.localPosition = new Vector3((float) (15 + (value * -9)), localPosition.y, localPosition.z);
            }
        }

        public class UIStarItem : UITableItem
        {
            public override void OnCreate()
            {
            }
        }
    }

    public class GridDamageItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.DamageRankItem = base.FindChild<UIDragScrollView>("DamageRankItem");
            this.GoRankIdInfo = base.FindChild<Transform>("GoRankIdInfo");
            this.SpriteRankId = base.FindChild<UISprite>("SpriteRankId");
            this.LabelRankId = base.FindChild<UILabel>("LabelRankId");
            this.GoPlayerInfo = base.FindChild<Transform>("GoPlayerInfo");
            this.LabelPlayerName = base.FindChild<UILabel>("LabelPlayerName");
            this.LabelPlayerLv = base.FindChild<UILabel>("LabelPlayerLv");
            this.SpriteGold = base.FindChild<UISprite>("SpriteGold");
            this.LabelGoldNum = base.FindChild<UILabel>("LabelGoldNum");
            this.LabelHelpTitle = base.FindChild<UILabel>("LabelHelpTitle");
            this.GoHeadInInfo = base.FindChild<Transform>("GoHeadInInfo");
            this.SpriteHeadBg = base.FindChild<UISprite>("SpriteHeadBg");
            this.TexturHeadIcon = base.FindChild<UITexture>("TexturHeadIcon");
            this.SpriteQuality = base.FindChild<UISprite>("SpriteQuality");
            this.GoStarInfo = base.FindChild<Transform>("GoStarInfo");
            this.GridStar = base.FindChild<UIGrid>("GridStar");
        }

        public UIDragScrollView DamageRankItem { get; private set; }

        public Transform GoHeadInInfo { get; private set; }

        public Transform GoPlayerInfo { get; private set; }

        public Transform GoRankIdInfo { get; private set; }

        public Transform GoStarInfo { get; private set; }

        public UIGrid GridStar { get; private set; }

        public UILabel LabelGoldNum { get; private set; }

        public UILabel LabelHelpTitle { get; private set; }

        public UILabel LabelPlayerLv { get; private set; }

        public UILabel LabelPlayerName { get; private set; }

        public UILabel LabelRankId { get; private set; }

        public UISprite SpriteGold { get; private set; }

        public UISprite SpriteHeadBg { get; private set; }

        public UISprite SpriteQuality { get; private set; }

        public UISprite SpriteRankId { get; private set; }

        public UITexture TexturHeadIcon { get; private set; }
    }

    public class HelpGoldItemInfo
    {
        public int cardMainEntry;
        public int cardQuality;
        public int cardStarlv;
        public int helpGoldNum;
        public int playerLv;
        public string playerName;
        public int rankId;
    }
}

