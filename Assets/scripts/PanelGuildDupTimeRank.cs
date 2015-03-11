using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class PanelGuildDupTimeRank : GUIPanelEntity
{
    public List<guilddup_config> configs;
    private int curDupIndex;
    public string curDupName = string.Empty;
    private const int defaultRankCnt = 11;
    public static Dictionary<int, List<RankItemData>> DupIndexToRankInfoListDic = new Dictionary<int, List<RankItemData>>();
    private static int maxDupIndex = 100;
    private static int minDupIndex = 0;
    protected UITableManager<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>> TableGridRank = new UITableManager<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>>();

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.Bgtn_TurnRight.OnUIMouseClick(u => this.TurnRight());
        this.Bgtn_TurnLeft.OnUIMouseClick(u => this.TurnLeft());
        this.TurnToNextDupRank(DupIndexToRankInfoListDic.Count);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.LabelDupTitle = base.FindChild<UILabel>("LabelDupTitle");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.Bgtn_TurnRight = base.FindChild<UIButton>("Bgtn_TurnRight");
        this.Bgtn_TurnLeft = base.FindChild<UIButton>("Bgtn_TurnLeft");
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridRank = base.FindChild<UIGrid>("GridRank");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.TableGridRank.InitFromGrid(this.GridRank);
    }

    public void InitUiGridData(int rankCnt = 11)
    {
        if (rankCnt <= 1)
        {
            rankCnt = 1;
        }
        this.TableGridRank.Count = rankCnt;
        if (rankCnt == 1)
        {
            if (this.TableGridRank.Count == 1)
            {
                this.TableGridRank[0].Model.Template.GoGuidInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoHeadInInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoRankIdInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoTimeInfo.gameObject.SetActive(false);
                this.TableGridRank[0].Model.Template.GoTitleInfo.gameObject.SetActive(true);
                this.TableGridRank[0].Model.Template.LabelDupHaveNoData.gameObject.SetActive(true);
                this.TableGridRank[0].Model.Template.LabelDupHaveNoData.text = ConfigMgr.getInstance().GetWord(0x186c5);
                this.TableGridRank[0].Model.Template.LabelDupRankTitle.text = ConfigMgr.getInstance().GetWord(0x186c6);
            }
        }
        else
        {
            this.TableGridRank[0].Model.Template.GoGuidInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoHeadInInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoRankIdInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoTimeInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.GoTitleInfo.gameObject.SetActive(true);
            this.TableGridRank[0].Model.Template.LabelDupHaveNoData.gameObject.SetActive(false);
            this.TableGridRank[0].Model.Template.LabelDupRankTitle.text = ConfigMgr.getInstance().GetWord(0x186c6);
            float y = 0f;
            for (int i = 0; i < this.TableGridRank.Count; i++)
            {
                this.TableGridRank[i].Model.RankID = i;
                this.SetRankId(i);
                switch (i)
                {
                    case 0:
                    {
                        this.TableGridRank[i].Model.Template.LabelDupRankTitle.text = ConfigMgr.getInstance().GetWord(0x186c6);
                        this.TableGridRank[i].Model.Template.GoTitleInfo.gameObject.SetActive(true);
                        Vector3 localPosition = this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition;
                        this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition = new Vector3(localPosition.x, 0f, 0f);
                        this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size = new Vector3(695f, 160f, 0f);
                        this.TableGridRank[i].Model.Template.GoHeadInInfo.transform.localPosition = new Vector3(-330f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.GoGuidInfo.transform.localPosition = new Vector3(-100f, 0f, 0f);
                        this.TableGridRank[i].Model.Item.Root.transform.localPosition = new Vector3(localPosition.x, y, 0f);
                        y -= this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size.y;
                        this.TableGridRank[i].Model.Template.SpriteRankId.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.LabelDupHaveNoData.gameObject.SetActive(false);
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
                            this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size = new Vector3(695f, 160f, 0f);
                            this.TableGridRank[i].Model.Item.Root.transform.localPosition = new Vector3(vector2.x, y, 0f);
                            y -= 110f;
                        }
                        else
                        {
                            Vector3 vector3 = this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition;
                            this.TableGridRank[i].Model.Template.GoTitleInfo.transform.localPosition = new Vector3(vector3.x, 50f, 0f);
                            this.TableGridRank[i].Model.Template.GoTitleInfo.gameObject.SetActive(false);
                            this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size = new Vector3(695f, 110f, 0f);
                            this.TableGridRank[i].Model.Item.Root.transform.localPosition = new Vector3(vector3.x, y, 0f);
                            y -= this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size.y;
                        }
                        string str = "Ui_Guildwar_Icon_" + i;
                        this.TableGridRank[i].Model.Template.SpriteRankId.spriteName = str;
                        this.TableGridRank[i].Model.Template.SpriteRankId.gameObject.SetActive(true);
                        this.TableGridRank[i].Model.Template.LabelDupHaveNoData.gameObject.SetActive(false);
                        this.TableGridRank[i].Model.Template.GoHeadInInfo.transform.localPosition = new Vector3(-230f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.GoGuidInfo.transform.localPosition = new Vector3(0f, 0f, 0f);
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
                        this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size = new Vector3(695f, 110f, 0f);
                        this.TableGridRank[i].Model.Template.GoHeadInInfo.transform.localPosition = new Vector3(-230f, 0f, 0f);
                        this.TableGridRank[i].Model.Template.GoGuidInfo.transform.localPosition = new Vector3(0f, 0f, 0f);
                        this.TableGridRank[i].Model.Item.Root.transform.localPosition = new Vector3(vector4.x, y, 0f);
                        y -= this.TableGridRank[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>().size.y;
                        this.TableGridRank[i].Model.Template.LabelDupHaveNoData.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    public void ReceiveDupRank(S2C_GuildDuplicatePassRank dupPassRankData, bool bForTest = false)
    {
        if (!bForTest)
        {
            DupIndexToRankInfoListDic.Clear();
            List<RankItemData> list = new List<RankItemData>();
            int duplicateId = dupPassRankData.duplicateId;
            this.curDupIndex = dupPassRankData.duplicateId;
            int num2 = 0;
            foreach (GuildDuplicateRankInfo info in dupPassRankData.rankList)
            {
                RankItemData item = new RankItemData {
                    mainCardEntry = info.headEntry,
                    guidId = (int) info.guildId,
                    guidName = info.guildName,
                    guidMasterName = info.charimanName,
                    guidLv = info.lvl,
                    useTime = info.usedTime,
                    fastTime = info.timestamp,
                    rankId = num2
                };
                num2++;
                list.Add(item);
            }
            if (!DupIndexToRankInfoListDic.ContainsKey(duplicateId))
            {
                DupIndexToRankInfoListDic.Add(duplicateId, list);
            }
        }
        else
        {
            DupIndexToRankInfoListDic.Clear();
            List<RankItemData> list2 = new List<RankItemData>();
            int key = dupPassRankData.duplicateId;
            for (int i = 0; i < 11; i++)
            {
                RankItemData data2 = new RankItemData {
                    mainCardEntry = i * key,
                    guidId = i * key,
                    guidName = "guildName_" + (i * key),
                    guidMasterName = "charimanName_" + (i * key),
                    guidLv = i * key,
                    useTime = i * key,
                    rankId = i
                };
                long num5 = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.TimeStampZero);
                data2.fastTime = -28880 + ((i * 0xb48) * key);
                list2.Add(data2);
            }
            if (!DupIndexToRankInfoListDic.ContainsKey(key))
            {
                DupIndexToRankInfoListDic.Add(key, list2);
            }
        }
        this.TurnToNextDupRank(this.curDupIndex);
    }

    private void RePosition()
    {
        this.GridRank.enabled = true;
        this.GridRank.cellHeight = 80f;
        this.GridRank.repositionNow = true;
        this.GridRank.enabled = false;
    }

    private void ReqDupPassInfo(int dupId)
    {
        SocketMgr.Instance.RequestC2S_GuildDuplicatePassRank(dupId);
    }

    public void SetDupRankInfo(List<RankItemData> rankItemDataList, string dupName)
    {
        this.LabelDupTitle.text = dupName;
        if (rankItemDataList.Count > 0)
        {
            foreach (RankItemData data in rankItemDataList)
            {
                IEnumerator<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>> enumerator = this.TableGridRank.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        UIAutoGenItem<GridRankItemTemplate, GridRankItemModel> current = enumerator.Current;
                        this.SetRankInfo(data);
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
        else
        {
            this.InitUiGridData(1);
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

    public void SetRankInfo(RankItemData rankInfodata)
    {
        IEnumerator<UIAutoGenItem<GridRankItemTemplate, GridRankItemModel>> enumerator = this.TableGridRank.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<GridRankItemTemplate, GridRankItemModel> current = enumerator.Current;
                if (rankInfodata.rankId == current.Model.RankID)
                {
                    this.SetRankId(rankInfodata.rankId);
                    current.Model.Template.LabelGuidMasterName.text = ConfigMgr.getInstance().GetWord(0x186c7) + rankInfodata.guidMasterName;
                    current.Model.Template.LabelGuidLv.text = "Lv." + rankInfodata.guidLv.ToString();
                    current.Model.Template.LabelGuidName.text = rankInfodata.guidName;
                    current.Model.Template.SpriteQuality.name = "Ui_Hero_Frame_" + (rankInfodata.mainCardQuality + 1);
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(rankInfodata.mainCardEntry);
                    current.Model.Template.TexturHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    if (rankInfodata.rankId == 0)
                    {
                        current.Model.Template.LabelTimeTiitle.text = ConfigMgr.getInstance().GetWord(0x186c8);
                        DateTime time = TimeMgr.Instance.ConvertToDateTime((long) rankInfodata.fastTime);
                        object[] objArray1 = new object[] { time.Year, ConfigMgr.getInstance().GetWord(0x57c), time.Month, ConfigMgr.getInstance().GetWord(0x57d), time.Day, ConfigMgr.getInstance().GetWord(0x57e) };
                        current.Model.Template.LabelTime.text = string.Concat(objArray1);
                    }
                    else
                    {
                        current.Model.Template.LabelTimeTiitle.text = ConfigMgr.getInstance().GetWord(0x186c9);
                        TimeSpan span = TimeSpan.FromSeconds((double) rankInfodata.useTime);
                        object[] objArray2 = new object[] { Mathf.FloorToInt((float) span.TotalDays), ConfigMgr.getInstance().GetWord(0x579), Mathf.FloorToInt((float) span.Hours), ConfigMgr.getInstance().GetWord(0x57f), Mathf.FloorToInt((float) span.Minutes), ConfigMgr.getInstance().GetWord(0x580) };
                        current.Model.Template.LabelTime.text = string.Concat(objArray2);
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
        if (this.curDupIndex > (minDupIndex + 1))
        {
            this.curDupIndex--;
        }
        else
        {
            this.curDupIndex = minDupIndex;
        }
        if (XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.curDupIndex) != null)
        {
            if (((1 != 0) && (XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.curDupIndex).status == GuildDupStatusEnum.GuildDupStatus_Can_Open)) || ((XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.curDupIndex).status == GuildDupStatusEnum.GuildDupStatus_Open) || (XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.curDupIndex).status == GuildDupStatusEnum.GuildDupStatus_Pass)))
            {
                this.ReqDupPassInfo(this.curDupIndex);
            }
            else
            {
                this.curDupIndex++;
            }
        }
        else
        {
            this.curDupIndex++;
        }
    }

    private void TurnRight()
    {
        if (this.curDupIndex < maxDupIndex)
        {
            this.curDupIndex++;
        }
        else
        {
            this.curDupIndex = maxDupIndex;
        }
        if (XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.curDupIndex) != null)
        {
            if (((1 != 0) && (XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.curDupIndex).status == GuildDupStatusEnum.GuildDupStatus_Can_Open)) || ((XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.curDupIndex).status == GuildDupStatusEnum.GuildDupStatus_Open) || (XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.curDupIndex).status == GuildDupStatusEnum.GuildDupStatus_Pass)))
            {
                this.ReqDupPassInfo(this.curDupIndex);
            }
            else
            {
                this.curDupIndex--;
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186ca));
            }
        }
        else
        {
            this.curDupIndex--;
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186ca));
        }
    }

    private void TurnToNextDupRank(int index)
    {
        List<RankItemData> list;
        string dupName = string.Empty;
        int id = index;
        guilddup_config _config = ConfigMgr.getInstance().getByEntry<guilddup_config>(id);
        if (_config != null)
        {
            dupName = _config.name;
        }
        else
        {
            dupName = string.Empty;
        }
        if (DupIndexToRankInfoListDic.TryGetValue(index, out list))
        {
            this.InitUiGridData(list.Count);
            this.SetDupRankInfo(list, dupName);
        }
        else
        {
            this.LabelDupTitle.text = dupName;
            this.InitUiGridData(index);
        }
        UIScrollView component = this.ScrollViewList.gameObject.GetComponent<UIScrollView>();
        this.SetScrollListInitPos(component);
    }

    protected UIWidget AutoClose { get; set; }

    protected UIButton Bgtn_TurnLeft { get; set; }

    protected UIButton Bgtn_TurnRight { get; set; }

    protected UIButton Btn_Close { get; set; }

    protected UIGrid GridRank { get; set; }

    protected UILabel LabelDupTitle { get; set; }

    public static int MaxDupId
    {
        get
        {
            return maxDupIndex;
        }
        set
        {
            maxDupIndex = value;
        }
    }

    public static int MinDupId
    {
        get
        {
            return minDupIndex;
        }
        set
        {
            minDupIndex = value;
        }
    }

    protected UIPanel ScrollViewList { get; set; }

    public class GridRankItemModel : TableItemModel<PanelGuildDupTimeRank.GridRankItemTemplate>
    {
        public override void Init(PanelGuildDupTimeRank.GridRankItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }

        public int RankID { get; set; }
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
            this.GoGuidInfo = base.FindChild<Transform>("GoGuidInfo");
            this.LabelGuidName = base.FindChild<UILabel>("LabelGuidName");
            this.LabelGuidMasterName = base.FindChild<UILabel>("LabelGuidMasterName");
            this.LabelGuidLv = base.FindChild<UILabel>("LabelGuidLv");
            this.GoTimeInfo = base.FindChild<Transform>("GoTimeInfo");
            this.LabelTimeTiitle = base.FindChild<UILabel>("LabelTimeTiitle");
            this.LabelTime = base.FindChild<UILabel>("LabelTime");
            this.GoRankIdInfo = base.FindChild<Transform>("GoRankIdInfo");
            this.SpriteRankId = base.FindChild<UISprite>("SpriteRankId");
            this.LabelRankId = base.FindChild<UILabel>("LabelRankId");
            this.GoTitleInfo = base.FindChild<Transform>("GoTitleInfo");
            this.LabelDupRankTitle = base.FindChild<UILabel>("LabelDupRankTitle");
            this.LabelDupHaveNoData = base.FindChild<UILabel>("LabelDupHaveNoData");
        }

        public Transform GoGuidInfo { get; private set; }

        public Transform GoHeadInInfo { get; private set; }

        public Transform GoRankIdInfo { get; private set; }

        public Transform GoTimeInfo { get; private set; }

        public Transform GoTitleInfo { get; private set; }

        public UILabel LabelDupHaveNoData { get; private set; }

        public UILabel LabelDupRankTitle { get; private set; }

        public UILabel LabelGuidLv { get; private set; }

        public UILabel LabelGuidMasterName { get; private set; }

        public UILabel LabelGuidName { get; private set; }

        public UILabel LabelRankId { get; private set; }

        public UILabel LabelTime { get; private set; }

        public UILabel LabelTimeTiitle { get; private set; }

        public UISprite SpriteHeadBg { get; private set; }

        public UISprite SpriteQuality { get; private set; }

        public UISprite SpriteRankId { get; private set; }

        public UITexture TexturHeadIcon { get; private set; }

        public UIDragScrollView TimeRankItem { get; private set; }
    }

    public class RankItemData
    {
        public int fastTime;
        public int guidId;
        public int guidLv;
        public string guidMasterName;
        public string guidName;
        public int mainCardEntry;
        public int mainCardQuality;
        public int rankId;
        public int useTime;
    }
}

