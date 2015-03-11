using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class GuidBattleDamageBarRankPanel : GUIPanelEntity
{
    private bool bForTest = true;
    private int curDupId__;
    private int curMaxDamage;
    private SelState curSelState;
    public List<DamageBarItemInfo> DupDamegaBarInfoList = new List<DamageBarItemInfo>();
    protected UITableManager<UIAutoGenItem<GridDamageItemTemplate, GridDamageItemModel>> TableGridDamage = new UITableManager<UIAutoGenItem<GridDamageItemTemplate, GridDamageItemModel>>();
    public List<DamageBarItemInfo> TrenchDamageBarInfoList = new List<DamageBarItemInfo>();

    private void CalculateBarValue()
    {
    }

    private int CalculateMaxDamage(List<DamageBarItemInfo> hoItemInfoList)
    {
        for (int i = 0; i < hoItemInfoList.Count; i++)
        {
            for (int j = 1; j < hoItemInfoList.Count; j++)
            {
                if (hoItemInfoList[i].damage > hoItemInfoList[j].damage)
                {
                    int damage = hoItemInfoList[i].damage;
                    hoItemInfoList[i].damage = hoItemInfoList[j].damage;
                    hoItemInfoList[j].damage = damage;
                }
            }
        }
        return 0;
    }

    private void ClickToggleLeftForReq()
    {
        SocketMgr.Instance.RequestC2S_GuildDuplicateDamageRank(this.curDupId__);
    }

    private void ClickToggleRightForReq()
    {
        SocketMgr.Instance.RequestC2S_GuildTrenchDamageRank(this.curDupId__);
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.Left.OnUIMouseClick(u => this.ClickToggleLeftForReq());
        this.Right.OnUIMouseClick(u => this.ClickToggleRightForReq());
        this.curSelState = SelState.left;
        this.SetSelState();
        this.SetHandOutItemInfo(this.DupDamegaBarInfoList);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.GoToggleInfo = base.FindChild<Transform>("GoToggleInfo");
        this.ToggleLeft = base.FindChild<UIToggle>("ToggleLeft");
        this.ToggleRight = base.FindChild<UIToggle>("ToggleRight");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.GridDamage = base.FindChild<UIGrid>("GridDamage");
        this.Left = base.FindChild<Transform>("Left");
        this.SpriteLeftUp = base.FindChild<UISprite>("SpriteLeftUp");
        this.SpriteLeftDown = base.FindChild<UISprite>("SpriteLeftDown");
        this.Right = base.FindChild<Transform>("Right");
        this.SpriteRightUp = base.FindChild<UISprite>("SpriteRightUp");
        this.SpriteRightDown = base.FindChild<UISprite>("SpriteRightDown");
        this.LabelHaveNoData = base.FindChild<UILabel>("LabelHaveNoData");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.TableGridDamage.InitFromGrid(this.GridDamage);
    }

    private void InitUiGridData(int itemCnt)
    {
        this.TableGridDamage.Count = itemCnt;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnSetToggleLeft()
    {
        this.curSelState = SelState.left;
        this.ToggleLeft.value = true;
        this.SetSelState();
        this.SetHandOutItemInfo(this.DupDamegaBarInfoList);
    }

    private void OnSetToggleRight()
    {
        this.curSelState = SelState.right;
        this.ToggleRight.value = true;
        this.SetSelState();
        this.SetHandOutItemInfo(this.TrenchDamageBarInfoList);
    }

    public void ReceiveDupDamageRank(S2C_GuildDuplicateDamageRank DupDamageData, bool bForTest = false)
    {
        if (!bForTest)
        {
            this.curDupId__ = DupDamageData.duplicateId;
            this.DupDamegaBarInfoList.Clear();
            for (int i = 0; i < DupDamageData.rankList.Count; i++)
            {
                DamageBarItemInfo item = new DamageBarItemInfo {
                    playerName = DupDamageData.rankList[i].name,
                    playerLv = DupDamageData.rankList[i].lvl,
                    rankId = i + 1,
                    damage = (int) DupDamageData.rankList[i].damage,
                    cardMainEntry = DupDamageData.rankList[i].headEntry,
                    cardQuality = 0,
                    cardStarlv = 0
                };
                this.DupDamegaBarInfoList.Add(item);
            }
        }
        else
        {
            this.DupDamegaBarInfoList.Clear();
            for (int j = 0; j < 5; j++)
            {
                DamageBarItemInfo info2 = new DamageBarItemInfo {
                    playerName = ConfigMgr.getInstance().GetWord(0x186bd) + j,
                    playerLv = j,
                    rankId = j + 1,
                    damage = 0x186a0 - (j * 0x2710),
                    cardMainEntry = j,
                    cardQuality = j,
                    cardStarlv = j + 1
                };
                this.DupDamegaBarInfoList.Add(info2);
            }
        }
        this.OnSetToggleLeft();
    }

    public void ReceiveTrenchDamageRank(S2C_GuildTrenchDamageRank TrenchDamageData, bool bForTest = false)
    {
        if (!bForTest)
        {
            this.curDupId__ = TrenchDamageData.duplicateId;
            this.TrenchDamageBarInfoList.Clear();
            for (int i = 0; i < TrenchDamageData.rankList.Count; i++)
            {
                DamageBarItemInfo item = new DamageBarItemInfo {
                    playerName = TrenchDamageData.rankList[i].name,
                    playerLv = TrenchDamageData.rankList[i].lvl,
                    rankId = i + 1,
                    damage = (int) TrenchDamageData.rankList[i].damage,
                    cardMainEntry = TrenchDamageData.rankList[i].headEntry,
                    cardQuality = 0,
                    cardStarlv = 0
                };
                this.TrenchDamageBarInfoList.Add(item);
            }
        }
        else
        {
            this.DupDamegaBarInfoList.Clear();
            for (int j = 0; j < 5; j++)
            {
                DamageBarItemInfo info2 = new DamageBarItemInfo {
                    playerName = ConfigMgr.getInstance().GetWord(0x186bd) + j,
                    playerLv = j,
                    rankId = j + 1,
                    damage = 0x3e8 - (j * 80),
                    cardMainEntry = j,
                    cardQuality = j,
                    cardStarlv = j + 1
                };
                this.TrenchDamageBarInfoList.Add(info2);
            }
        }
        this.OnSetToggleRight();
    }

    public void SetHandOutItemInfo(List<DamageBarItemInfo> hoItemInfoList)
    {
        this.InitUiGridData(hoItemInfoList.Count);
        if (hoItemInfoList.Count <= 0)
        {
            this.LabelHaveNoData.gameObject.SetActive(true);
            this.LabelHaveNoData.text = ConfigMgr.getInstance().GetWord(0x186c3);
        }
        else
        {
            this.LabelHaveNoData.gameObject.SetActive(false);
            int count = hoItemInfoList.Count;
            for (int i = 0; i < count; i++)
            {
                string str;
                switch (i)
                {
                    case 0:
                        this.curMaxDamage = hoItemInfoList[0].damage;
                        break;

                    case 0:
                    case 1:
                    case 2:
                    {
                        this.TableGridDamage[i].Model.Template.SpriteRankId.gameObject.SetActive(true);
                        this.TableGridDamage[i].Model.Template.LabelRankId.gameObject.SetActive(false);
                        int rankId = hoItemInfoList[i].rankId;
                        this.TableGridDamage[i].Model.Template.SpriteRankId.spriteName = "Ui_Guildwar_Icon_" + rankId;
                        goto Label_0169;
                    }
                }
                this.TableGridDamage[i].Model.Template.SpriteRankId.gameObject.SetActive(false);
                this.TableGridDamage[i].Model.Template.LabelRankId.gameObject.SetActive(true);
            Label_0169:
                str = string.Empty;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(hoItemInfoList[i].cardMainEntry);
                this.TableGridDamage[i].Model.Template.LabelPlayerName.text = hoItemInfoList[i].playerName;
                this.TableGridDamage[i].Model.Template.LabelPlayerLv.text = "Lv." + hoItemInfoList[i].playerLv;
                this.TableGridDamage[i].Model.Template.TexturHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                this.TableGridDamage[i].Model.Template.SpriteQuality.spriteName = "Ui_Hero_Frame_" + (hoItemInfoList[i].cardQuality + 1);
                this.TableGridDamage[i].Model.StarCnt = hoItemInfoList[i].cardStarlv;
                this.TableGridDamage[i].Model.Template.LabelRankId.text = hoItemInfoList[i].rankId.ToString();
                this.TableGridDamage[i].Model.Template.LabelDamage.text = hoItemInfoList[i].damage.ToString();
                float num5 = 0f;
                if (this.curMaxDamage != 0)
                {
                    num5 = ((float) hoItemInfoList[i].damage) / ((float) this.curMaxDamage);
                }
                else
                {
                    num5 = 0f;
                }
                this.TableGridDamage[i].Model.Template.BarDamage.value = num5;
            }
        }
    }

    private void SetSelState()
    {
        if (this.curSelState == SelState.left)
        {
            this.SpriteLeftDown.gameObject.SetActive(true);
            this.SpriteLeftUp.gameObject.SetActive(false);
            this.SpriteRightDown.gameObject.SetActive(false);
            this.SpriteRightUp.gameObject.SetActive(true);
        }
        else if (this.curSelState == SelState.right)
        {
            this.SpriteLeftDown.gameObject.SetActive(false);
            this.SpriteLeftUp.gameObject.SetActive(true);
            this.SpriteRightDown.gameObject.SetActive(true);
            this.SpriteRightUp.gameObject.SetActive(false);
        }
    }

    protected UIWidget AutoClose { get; set; }

    protected UIButton Btn_Close { get; set; }

    public int CurDupId
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

    protected Transform GoToggleInfo { get; set; }

    protected UIGrid GridDamage { get; set; }

    protected UILabel LabelHaveNoData { get; set; }

    protected Transform Left { get; set; }

    protected Transform Right { get; set; }

    protected UISprite SpriteLeftDown { get; set; }

    protected UISprite SpriteLeftUp { get; set; }

    protected UISprite SpriteRightDown { get; set; }

    protected UISprite SpriteRightUp { get; set; }

    protected UIToggle ToggleLeft { get; set; }

    protected UIToggle ToggleRight { get; set; }

    public class DamageBarItemInfo
    {
        public int cardMainEntry;
        public int cardQuality;
        public int cardStarlv;
        public int damage;
        public int playerLv;
        public string playerName;
        public int rankId;
    }

    public class GridDamageItemModel : TableItemModel<GuidBattleDamageBarRankPanel.GridDamageItemTemplate>
    {
        private UITableManager<UIStarItem> StarTable = new UITableManager<UIStarItem>();

        public override void Init(GuidBattleDamageBarRankPanel.GridDamageItemTemplate template, UITableItem item)
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
            this.BarDamage = base.FindChild<UISlider>("BarDamage");
            this.LabelDamage = base.FindChild<UILabel>("LabelDamage");
            this.GoHeadInInfo = base.FindChild<Transform>("GoHeadInInfo");
            this.SpriteHeadBg = base.FindChild<UISprite>("SpriteHeadBg");
            this.TexturHeadIcon = base.FindChild<UITexture>("TexturHeadIcon");
            this.SpriteQuality = base.FindChild<UISprite>("SpriteQuality");
            this.GoStarInfo = base.FindChild<Transform>("GoStarInfo");
            this.GridStar = base.FindChild<UIGrid>("GridStar");
        }

        public UISlider BarDamage { get; private set; }

        public UIDragScrollView DamageRankItem { get; private set; }

        public Transform GoHeadInInfo { get; private set; }

        public Transform GoPlayerInfo { get; private set; }

        public Transform GoRankIdInfo { get; private set; }

        public Transform GoStarInfo { get; private set; }

        public UIGrid GridStar { get; private set; }

        public UILabel LabelDamage { get; private set; }

        public UILabel LabelPlayerLv { get; private set; }

        public UILabel LabelPlayerName { get; private set; }

        public UILabel LabelRankId { get; private set; }

        public UISprite SpriteHeadBg { get; private set; }

        public UISprite SpriteQuality { get; private set; }

        public UISprite SpriteRankId { get; private set; }

        public UITexture TexturHeadIcon { get; private set; }
    }

    private enum SelState
    {
        left,
        right
    }
}

