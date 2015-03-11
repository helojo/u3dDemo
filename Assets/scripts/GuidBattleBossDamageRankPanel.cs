using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class GuidBattleBossDamageRankPanel : GUIPanelEntity
{
    public static Dictionary<int, List<RankItemData>> BossIndexToRankInfoListDic = new Dictionary<int, List<RankItemData>>();
    private int curBossId__;
    private string curBossName__ = string.Empty;
    private const int defaultRankCnt__ = 11;
    private int dupId__;
    private int maxBossIndex__ = 100;
    private int maxOpenBossIndx = 5;
    private int minBossIndex__;
    protected UITableManager<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>> TableGridRank = new UITableManager<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>>();

    public GuildDupTrenchMap.TrenchData GetBossStateByEntry(int id)
    {
        GuildDupTrenchMap gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupTrenchMap>();
        if (gUIEntity != null)
        {
            foreach (GuildDupTrenchMap.TrenchData data in gUIEntity.Levels)
            {
                if (data.TrenchInfo == null)
                {
                    return null;
                }
                if (data.TrenchInfo.guildDupTrenchEntry == id)
                {
                    return data;
                }
            }
        }
        return null;
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.Btn_TurnRight.OnUIMouseClick(u => this.TurnRight());
        this.Btn_TurnLeft.OnUIMouseClick(u => this.TurnLeft());
        this.TurnToNextDupRank(this.curBossId__);
        this.InitTurnBtnShowState();
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.LabelDupTitle = base.FindChild<UILabel>("LabelDupTitle");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.Btn_TurnRight = base.FindChild<UIButton>("Btn_TurnRight");
        this.Btn_TurnLeft = base.FindChild<UIButton>("Btn_TurnLeft");
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridRank = base.FindChild<UIGrid>("GridRank");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.TableGridRank.InitFromGrid(this.GridRank);
    }

    private void InitScrollInfo()
    {
        UIScrollView component = this.ScrollViewList.gameObject.GetComponent<UIScrollView>();
        this.SetScrollListInitPos(component);
    }

    private void InitTurnBtnShowState()
    {
        if (this.curBossId__ <= this.minBossIndex__)
        {
            this.Btn_TurnLeft.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_TurnLeft.gameObject.SetActive(true);
        }
        if (this.curBossId__ >= this.maxBossIndex__)
        {
            this.Btn_TurnRight.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_TurnRight.gameObject.SetActive(true);
        }
    }

    public void InitUiGridData(int rankCnt = 11)
    {
        if ((rankCnt == 1) || (rankCnt == 0))
        {
            this.TableGridRank.Count = 1;
            if (this.TableGridRank.Count == 1)
            {
                this.TableGridRank[0].Model.Template.GoPlayCardsInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoHeadInInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoRankIdInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoPlayerInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoDamageInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoTitleInfo.gameObject.SetActive(true);
                this.TableGridRank[0].Model.Template.LabelBossHaveNoData.gameObject.SetActive(true);
                this.TableGridRank[0].Model.Template.LabelBossHaveNoData.text = ConfigMgr.getInstance().GetWord(0x186b4);
                this.TableGridRank[0].Model.Template.LabelDupRankTitle.text = ConfigMgr.getInstance().GetWord(0x186b5);
            }
        }
        else
        {
            this.TableGridRank.Count = rankCnt;
            this.TableGridRank[0].Model.Template.GoPlayCardsInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoHeadInInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoRankIdInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoPlayerInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoTitleInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoDamageInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.LabelBossHaveNoData.gameObject.SetActive(false);
            this.TableGridRank[0].Model.Template.LabelBossHaveNoData.text = ConfigMgr.getInstance().GetWord(0x186b4);
            this.TableGridRank[0].Model.Template.LabelDupRankTitle.text = ConfigMgr.getInstance().GetWord(0x186b5);
            bool flag = false;
            if (this.TableGridRank.Count > 4)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            float y = 0f;
            for (int i = 0; i < this.TableGridRank.Count; i++)
            {
                if (this.TableGridRank[i].Model.Template.TimeRankItem != null)
                {
                    this.TableGridRank[i].Model.Template.TimeRankItem.enabled = flag;
                }
                this.TableGridRank.Cache = false;
                this.TableGridRank[i].Model.RankID = i;
                this.SetRankId(i);
                switch (i)
                {
                    case 0:
                    {
                        this.TableGridRank[i].Model.Template.LabelDupRankTitle.text = ConfigMgr.getInstance().GetWord(0x186b5);
                        this.TableGridRank[i].Model.Template.GoTitleInfo.gameObject.SetActive(true);
                        Vector3 localPosition = this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition;
                        this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition = new Vector3(localPosition.x, 0f, 0f);
                        this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size = new Vector3(695f, 170f, 0f);
                        this.TableGridRank[i].Model.Item.Root.transform.localPosition = new Vector3(localPosition.x, y, 0f);
                        y -= this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size.y;
                        this.TableGridRank[i].Model.Template.SpriteRankId.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelRankId.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelBossHaveNoData.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.GoHeadInInfo.transform.localPosition = new Vector3(-310f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.GoPlayerInfo.transform.localPosition = new Vector3(-230f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.LabeKillTime.transform.localPosition = new Vector3(510f, -40f, 0f);
                        this.TableGridRank[i].Model.Template.LabelKillTimeTiitle.transform.localPosition = new Vector3(358f, -38f, 0f);
                        this.TableGridRank[i].Model.Template.LabeKillTime.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.LabelKillTimeTiitle.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.GoDamageInfo.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.GoPlayCardsInfo.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelGuidName.transform.localPosition = new Vector3(80f, -38f, 0f);
                        break;
                    }
                    case 1:
                    case 2:
                    case 3:
                    {
                        if (i == 1)
                        {
                            Vector3 vector2 = this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition;
                            this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition = new Vector3(vector2.x, 0f, 0f);
                            this.TableGridRank[i].Model.Template.GoTitleInfo.gameObject.SetActive(true);
                            this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size = new Vector3(695f, 170f, 0f);
                            this.TableGridRank[i].Model.Item.Root.transform.localPosition = new Vector3(vector2.x, y, 0f);
                            y -= 120f;
                        }
                        else
                        {
                            Vector3 vector3 = this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition;
                            this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition = new Vector3(vector3.x, 50f, 0f);
                            this.TableGridRank[i].Model.Template.GoTitleInfo.gameObject.SetActive(false);
                            this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size = new Vector3(695f, 120f, 0f);
                            this.TableGridRank[i].Model.Item.Root.transform.localPosition = new Vector3(vector3.x, y, 0f);
                            y -= this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size.y;
                        }
                        string str = "Ui_Guildwar_Icon_" + i;
                        this.TableGridRank[i].Model.Template.SpriteRankId.spriteName = str;
                        this.TableGridRank[i].Model.Template.SpriteRankId.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.LabelRankId.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelBossHaveNoData.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.GoHeadInInfo.transform.localPosition = new Vector3(-80f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.GoPlayerInfo.transform.localPosition = new Vector3(0f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.LabeKillTime.transform.localPosition = new Vector3(146f, -40f, 0f);
                        this.TableGridRank[i].Model.Template.LabelKillTimeTiitle.transform.localPosition = new Vector3(0f, -40f, 0f);
                        this.TableGridRank[i].Model.Template.LabeKillTime.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelKillTimeTiitle.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.GoDamageInfo.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.GoPlayCardsInfo.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.LabelGuidName.transform.localPosition = new Vector3(287f, 22f, 0f);
                        this.TableGridRank[i].Model.TeamCardCnt = i;
                        break;
                    }
                    default:
                    {
                        this.TableGridRank[i].Model.Template.SpriteRankId.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelRankId.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.LabelRankId.text = i.ToString();
                        this.TableGridRank[i].Model.Template.LabelGuidLv.text = i.ToString();
                        Vector3 vector4 = this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition;
                        this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition = new Vector3(vector4.x, 0f, 0f);
                        this.TableGridRank[i].Model.Template.GoTitleInfo.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size = new Vector3(695f, 120f, 0f);
                        this.TableGridRank[i].Model.Item.Root.transform.localPosition = new Vector3(vector4.x, y, 0f);
                        y -= this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size.y;
                        this.TableGridRank[i].Model.Template.LabelRankId.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.LabelRankId.text = i.ToString();
                        this.TableGridRank[i].Model.Template.LabelBossHaveNoData.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.GoHeadInInfo.transform.localPosition = new Vector3(-80f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.GoPlayerInfo.transform.localPosition = new Vector3(0f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.LabeKillTime.transform.localPosition = new Vector3(146f, -40f, 0f);
                        this.TableGridRank[i].Model.Template.LabelKillTimeTiitle.transform.localPosition = new Vector3(0f, -40f, 0f);
                        this.TableGridRank[i].Model.Template.LabeKillTime.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelKillTimeTiitle.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.GoDamageInfo.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.GoPlayCardsInfo.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelGuidName.transform.localPosition = new Vector3(287f, 22f, 0f);
                        break;
                    }
                }
            }
        }
    }

    public void ReceiveBossDamageRank(S2C_GuildWholeSvrTrenchDamageRank bossDamageData, int min, int max, bool bForTest = false)
    {
        if (!bForTest)
        {
            BossIndexToRankInfoListDic.Clear();
            List<RankItemData> list = new List<RankItemData>();
            this.dupId__ = bossDamageData.duplicateId;
            this.curBossId__ = bossDamageData.trenchId;
            int num = 0;
            foreach (GuildDupRankUserInfo info in bossDamageData.rankList)
            {
                RankItemData item = new RankItemData {
                    mainCardEntry = info.headEntry,
                    guidName = info.guildName,
                    playerName = info.name,
                    playerLv = info.lvl,
                    fastTime = info.timestamp,
                    damage = (int) info.damage,
                    userId = (int) info.userId,
                    killedTime = info.timestamp,
                    rankId = num
                };
                num++;
                List<cardInfo> list2 = new List<cardInfo>();
                foreach (CardSimpleInfo info2 in info.cards)
                {
                    cardInfo info3 = new cardInfo {
                        entry = info2.entry,
                        lvl = info2.lvl,
                        quality = info2.quality,
                        starLv = info2.starLv
                    };
                    list2.Add(info3);
                }
                item.cards = list2;
                list.Add(item);
            }
            if (!BossIndexToRankInfoListDic.ContainsKey(this.curBossId__))
            {
                BossIndexToRankInfoListDic.Add(this.curBossId__, list);
            }
        }
        else
        {
            BossIndexToRankInfoListDic.Clear();
            List<RankItemData> list3 = new List<RankItemData>();
            this.curBossId__ = bossDamageData.trenchId;
            this.dupId__ = bossDamageData.duplicateId;
            for (int i = 0; i < (this.curBossId__ + 1); i++)
            {
                RankItemData data2 = new RankItemData {
                    rankId = i,
                    mainCardEntry = i,
                    guidName = "guildName_" + i,
                    playerName = this.curBossId__ + "name_" + i,
                    playerLv = i
                };
                long num3 = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.TimeStampZero);
                data2.killedTime = -28880 + (i * 0xb48);
                data2.damage = 0x186a0 - i;
                data2.userId = i;
                List<cardInfo> list4 = new List<cardInfo>();
                for (int j = 0; j < 5; j++)
                {
                    cardInfo info4 = new cardInfo {
                        entry = j,
                        lvl = j,
                        quality = j,
                        starLv = j + 1
                    };
                    list4.Add(info4);
                }
                data2.cards = list4;
                list3.Add(data2);
            }
            if (!BossIndexToRankInfoListDic.ContainsKey(this.curBossId__))
            {
                BossIndexToRankInfoListDic.Add(this.curBossId__, list3);
            }
        }
        this.TurnToNextDupRank(this.curBossId__);
    }

    private void RePosition()
    {
        this.GridRank.enabled = true;
        this.GridRank.cellHeight = 80f;
        this.GridRank.repositionNow = true;
        this.GridRank.enabled = false;
    }

    private void ReqBossDamageInfo(int dupId, int bossId)
    {
        SocketMgr.Instance.RequestC2S_GuildWholeSvrTrenchDamageRank(dupId, bossId);
    }

    public void SetBossDamageRankInfo(List<RankItemData> rankItemDataList, string dupName)
    {
        bool flag = false;
        if (rankItemDataList.Count >= 4)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        this.LabelDupTitle.text = dupName;
        foreach (RankItemData data in rankItemDataList)
        {
            IEnumerator<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>> enumerator = this.TableGridRank.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UIAutoGenItem<GridRankItemTemplate, GridRankItemModel> current = enumerator.Current;
                    this.SetRankInfo(data, false);
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

    private void SetRankId(int rankId)
    {
        if (rankId <= this.TableGridRank.Count)
        {
            if (rankId <= 3)
            {
                this.TableGridRank[rankId].Model.Template.SpriteRankId.gameObject.SetActive(true);
                this.TableGridRank[rankId].Model.Template.LabelRankId.gameObject.SetActive(false);
                string str = "Ui_Guildwar_Icon_" + rankId;
                this.TableGridRank[rankId].Model.Template.SpriteRankId.spriteName = str;
            }
            else
            {
                this.TableGridRank[rankId].Model.Template.SpriteRankId.gameObject.SetActive(false);
                this.TableGridRank[rankId].Model.Template.LabelRankId.gameObject.SetActive(true);
                this.TableGridRank[rankId].Model.Template.LabelRankId.text = rankId.ToString();
            }
        }
    }

    public void SetRankInfo(RankItemData rankInfodata, bool canDrag = false)
    {
        IEnumerator<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>> enumerator = this.TableGridRank.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<GridRankItemTemplate, GridRankItemModel> current = enumerator.Current;
                if (rankInfodata.rankId == current.Model.RankID)
                {
                    if (rankInfodata.rankId == 0)
                    {
                        if (rankInfodata.userId == -1)
                        {
                            current.Model.Template.GoPlayCardsInfo.gameObject.SetActive(false);
                            current.Model.Template.GoHeadInInfo.gameObject.SetActive(false);
                            current.Model.Template.GoRankIdInfo.gameObject.SetActive(false);
                            current.Model.Template.GoPlayerInfo.gameObject.SetActive(false);
                            current.Model.Template.GoDamageInfo.gameObject.SetActive(false);
                            current.Model.Template.GoTitleInfo.gameObject.SetActive(true);
                            current.Model.Template.LabelBossHaveNoData.gameObject.SetActive(true);
                            current.Model.Template.LabelBossHaveNoData.text = ConfigMgr.getInstance().GetWord(0x186b4);
                            current.Model.Template.LabelDupRankTitle.text = ConfigMgr.getInstance().GetWord(0x186b5);
                        }
                        else
                        {
                            current.Model.Template.GoPlayCardsInfo.gameObject.SetActive(false);
                            current.Model.Template.GoHeadInInfo.gameObject.SetActive(true);
                            current.Model.Template.GoRankIdInfo.gameObject.SetActive(true);
                            current.Model.Template.GoPlayerInfo.gameObject.SetActive(true);
                            current.Model.Template.GoDamageInfo.gameObject.SetActive(false);
                            if (rankInfodata.rankId <= 1)
                            {
                                current.Model.Template.GoTitleInfo.gameObject.SetActive(true);
                            }
                            else
                            {
                                current.Model.Template.GoTitleInfo.gameObject.SetActive(false);
                            }
                            current.Model.Template.LabelBossHaveNoData.gameObject.SetActive(false);
                            current.Model.Template.LabelBossHaveNoData.text = ConfigMgr.getInstance().GetWord(0x186b4);
                            current.Model.Template.LabelDupRankTitle.text = ConfigMgr.getInstance().GetWord(0x186b5);
                        }
                    }
                    this.SetRankId(rankInfodata.rankId);
                    current.Model.Template.LabelGuidName.text = ConfigMgr.getInstance().GetWord(0x186b6) + rankInfodata.guidName;
                    current.Model.Template.LabelPlayerName.text = rankInfodata.playerName.ToString();
                    current.Model.Template.LabelGuidLv.text = "Lv." + rankInfodata.playerLv.ToString();
                    current.Model.Template.SpriteQuality.name = "Ui_Hero_Frame_" + (rankInfodata.mainCardQuality + 1);
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(rankInfodata.mainCardEntry);
                    if (_config != null)
                    {
                        current.Model.Template.TexturHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    }
                    else
                    {
                        Debug.Log("SetRankInfo ::MainEntry is out of card_config!!!");
                    }
                    DateTime time = TimeMgr.Instance.ConvertToDateTime((long) rankInfodata.killedTime);
                    object[] objArray1 = new object[] { time.Year, ConfigMgr.getInstance().GetWord(0x57c), time.Month, ConfigMgr.getInstance().GetWord(0x57d), time.Day, ConfigMgr.getInstance().GetWord(0x57e) };
                    current.Model.Template.LabeKillTime.text = string.Concat(objArray1);
                    current.Model.Template.LabelPerDamage.text = rankInfodata.damage.ToString();
                    current.Model.TeamCardCnt = rankInfodata.cards.Count;
                    for (int i = 0; i < rankInfodata.cards.Count; i++)
                    {
                        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(rankInfodata.cards[i].entry);
                        current.Model.TeamCardTable[i].TexturSmHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                        current.Model.TeamCardTable[i].SpriteSmQuality.spriteName = "Ui_Hero_Frame_" + (rankInfodata.cards[i].quality + 1);
                        current.Model.TeamCardTable[i].LabelCardLv.text = rankInfodata.cards[i].lvl.ToString();
                        current.Model.TeamCardTable[i].startCnt = rankInfodata.cards[i].starLv;
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

    public void SetScrollListInitPos(UIScrollView scrList)
    {
        if (scrList != null)
        {
            UIPanel component = scrList.gameObject.GetComponent<UIPanel>();
            if (component != null)
            {
                component.clipOffset = new Vector2(component.clipOffset.x, 0f);
                component.gameObject.transform.localPosition = new Vector3(component.gameObject.transform.localPosition.x, 0f, scrList.transform.localPosition.z);
            }
            SpringPanel panel2 = scrList.GetComponent<SpringPanel>();
            if (panel2 != null)
            {
                panel2.enabled = true;
                panel2.target = new Vector3(panel2.target.x, 0f, panel2.target.z);
                panel2.enabled = false;
            }
        }
    }

    private void TurnLeft()
    {
        int id = this.curBossId__ - 1;
        GuildDupTrenchMap.TrenchData bossStateByEntry = this.GetBossStateByEntry(id);
        if (((bossStateByEntry != null) && (bossStateByEntry.TrenchInfo != null)) && (((1 != 0) && (bossStateByEntry.TrenchInfo.status == GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Open)) || (bossStateByEntry.TrenchInfo.status == GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Pass)))
        {
            this.ReqBossDamageInfo(this.dupId__, id);
        }
    }

    private void TurnRight()
    {
        int id = this.curBossId__ + 1;
        GuildDupTrenchMap.TrenchData bossStateByEntry = this.GetBossStateByEntry(id);
        if (bossStateByEntry != null)
        {
            if (bossStateByEntry.TrenchInfo != null)
            {
                if (((1 != 0) && (bossStateByEntry.TrenchInfo.status == GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Open)) || (bossStateByEntry.TrenchInfo.status == GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Pass))
                {
                    this.ReqBossDamageInfo(this.dupId__, id);
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c7f));
                }
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c7f));
            }
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c7f));
        }
    }

    private void TurnToNextDupRank(int index)
    {
        List<RankItemData> list;
        this.InitTurnBtnShowState();
        string dupName = "xxDupName_" + index;
        guilddup_config _config = ConfigMgr.getInstance().getByEntry<guilddup_config>(this.dupId__);
        int id = this.curBossId__;
        dupName = ConfigMgr.getInstance().getByEntry<guilddup_trench_config>(id).name;
        if (BossIndexToRankInfoListDic.TryGetValue(index, out list))
        {
            this.InitUiGridData(list.Count);
            this.SetBossDamageRankInfo(list, dupName);
        }
        else
        {
            this.LabelDupTitle.text = dupName;
            this.InitUiGridData(index);
        }
        this.InitScrollInfo();
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

    protected UIButton Btn_TurnLeft { get; set; }

    protected UIButton Btn_TurnRight { get; set; }

    public int DupId
    {
        get
        {
            return this.dupId__;
        }
        set
        {
            this.dupId__ = value;
        }
    }

    protected UIGrid GridRank { get; set; }

    protected UILabel LabelDupTitle { get; set; }

    public int MaxBossId
    {
        get
        {
            return this.maxBossIndex__;
        }
        set
        {
            this.maxBossIndex__ = value;
        }
    }

    public int MinBossId
    {
        get
        {
            return this.minBossIndex__;
        }
        set
        {
            this.minBossIndex__ = value;
        }
    }

    protected UIPanel ScrollViewList { get; set; }

    public class cardInfo
    {
        public int entry;
        public int lvl;
        public int quality;
        public int starLv;
    }

    public class GridRankItemModel : TableItemModel<GuidBattleBossDamageRankPanel.GridRankItemTemplate>
    {
        public UITableManager<UITeamCardItem> TeamCardTable = new UITableManager<UITeamCardItem>();

        public override void Init(GuidBattleBossDamageRankPanel.GridRankItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.TeamCardTable.InitFromGrid(base.Template.Table.GetComponent<UIGrid>());
        }

        public int RankID { get; set; }

        public int TeamCardCnt
        {
            get
            {
                return this.TeamCardTable.Count;
            }
            set
            {
                this.TeamCardTable.Count = value;
                base.Template.Table.transform.localPosition = new Vector3(30f, -35f, 0f);
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
                    this.GoStartInfo.transform.localPosition = new Vector3((float) (15 + (value * -6)), -18f, localPosition.z);
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
            this.LabelGuidName = base.FindChild<UILabel>("LabelGuidName");
            this.LabelGuidLv = base.FindChild<UILabel>("LabelGuidLv");
            this.GoRankIdInfo = base.FindChild<Transform>("GoRankIdInfo");
            this.SpriteRankId = base.FindChild<UISprite>("SpriteRankId");
            this.LabelRankId = base.FindChild<UILabel>("LabelRankId");
            this.GoTitleInfo = base.FindChild<Transform>("GoTitleInfo");
            this.LabelDupRankTitle = base.FindChild<UILabel>("LabelDupRankTitle");
            this.GoDamageInfo = base.FindChild<Transform>("GoDamageInfo");
            this.LabelPerDamage = base.FindChild<UILabel>("LabelPerDamage");
            this.GoPlayCardsInfo = base.FindChild<Transform>("GoPlayCardsInfo");
            this.Table = base.FindChild<UIGrid>("Table");
            this.SpriteSmQuality = base.FindChild<UISprite>("SpriteSmQuality");
            this.TexturSmHeadIcon = base.FindChild<UITexture>("TexturSmHeadIcon");
            this.SpriteSmHeadBg = base.FindChild<UISprite>("SpriteSmHeadBg");
            this.LabelCardLv = base.FindChild<UILabel>("LabelCardLv");
            this.GoStartInfo = base.FindChild<UIGrid>("GoStartInfo");
            this.LabelBossHaveNoData = base.FindChild<UILabel>("LabelBossHaveNoData");
        }

        public Transform GoDamageInfo { get; private set; }

        public Transform GoHeadInInfo { get; private set; }

        public Transform GoPlayCardsInfo { get; private set; }

        public Transform GoPlayerInfo { get; private set; }

        public Transform GoRankIdInfo { get; private set; }

        public UIGrid GoStartInfo { get; private set; }

        public Transform GoTitleInfo { get; private set; }

        public UILabel LabeKillTime { get; private set; }

        public UILabel LabelBossHaveNoData { get; private set; }

        public UILabel LabelCardLv { get; private set; }

        public UILabel LabelDupRankTitle { get; private set; }

        public UILabel LabelGuidLv { get; private set; }

        public UILabel LabelGuidName { get; private set; }

        public UILabel LabelKillTimeTiitle { get; private set; }

        public UILabel LabelPerDamage { get; private set; }

        public UILabel LabelPlayerName { get; private set; }

        public UILabel LabelRankId { get; private set; }

        public UISprite SpriteHeadBg { get; private set; }

        public UISprite SpriteQuality { get; private set; }

        public UISprite SpriteRankId { get; private set; }

        public UISprite SpriteSmHeadBg { get; private set; }

        public UISprite SpriteSmQuality { get; private set; }

        public UIGrid Table { get; private set; }

        public UITexture TexturHeadIcon { get; private set; }

        public UITexture TexturSmHeadIcon { get; private set; }

        public UIDragScrollView TimeRankItem { get; private set; }
    }

    public class RankItemData
    {
        public List<GuidBattleBossDamageRankPanel.cardInfo> cards;
        public int damage;
        public int fastTime;
        public int guidId;
        public string guidName;
        public int killedTime;
        public int mainCardEntry;
        public int mainCardQuality;
        public int playerLv;
        public string playerName;
        public int rankId;
        public int userId;
        public int useTime;
    }
}

