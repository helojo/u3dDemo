using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class AllRankInOnePanel : GUIEntity
{
    private Camera _camera3D;
    private static EN_RankListType _currentRankType;
    private RankListPlayerInfo _myInfo;
    private readonly List<GameObject> _rankItemGameObjects = new List<GameObject>(50);
    private readonly List<RankListPlayerInfo> _tempList = new List<RankListPlayerInfo>(50);
    public UIButton BackButton;
    public UILabel GuildLabel;
    public UILabel GuildName;
    public GameObject InfoHeadGroup;
    public GameObject Item;
    public Transform ItemList;
    public Transform ListBottomRight;
    public Transform ListTopLeft;
    public UILabel RankCapacityName;
    public UILabel RankCapacityValue;
    public UISprite RankingIcon;
    public UILabel RankingValue;
    public UIGrid RankItemGrid;
    public UILabel RankLabel;
    public UILabel RankListTitle;
    public GameObject RankTabAll;
    public GameObject RankTabArena;
    public GameObject RankTabLevel;
    public GameObject RankTabLol;
    public GameObject RankTabTeam;
    public GameObject RankTabTower;
    public UISprite RoleFrame;
    public UISprite RoleFrameIcon;
    public UITexture RoleHeadIcon;
    public UILabel RoleLabel;
    public UILabel RoleLevel;
    public GameObject RoleLevelNameAndValue;
    public UILabel RoleName;
    public UILabel Title;

    private void AddTabButton(GameObject go, EN_RankListType rankType, string labelValue)
    {
        <AddTabButton>c__AnonStorey248 storey = new <AddTabButton>c__AnonStorey248 {
            rankType = rankType,
            <>f__this = this
        };
        go.transform.FindChild("Label").gameObject.GetComponent<UILabel>().text = labelValue;
        UIEventTrigger component = go.GetComponent<UIEventTrigger>();
        EventDelegate.Add(component.onPress, new EventDelegate.Callback(storey.<>m__514));
        EventDelegate.Add(component.onRelease, new EventDelegate.Callback(this.GetServerData));
    }

    private void ChangeTabBg(GameObject go, EN_RankListType rankType)
    {
        go.GetComponent<UISprite>().spriteName = (this.CurrentRankType != rankType) ? "Ui_Paihang_Bg_buttondown" : "Ui_Paihang_Bg_buttonup";
    }

    private void GetServerData()
    {
        SocketMgr.Instance.RequestAllRankInOneList((int) this.CurrentRankType);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        this.TabButtonInit();
        EventDelegate.Add(this.BackButton.onClick, delegate {
            if (null != this._camera3D)
            {
                this._camera3D.enabled = true;
                this._camera3D = null;
            }
            GUIMgr.Instance.ExitModelGUI(this);
        });
        this.GuildLabel.text = ConfigMgr.getInstance().GetWord(0x5a4) + ":";
        this.RoleLabel.text = ConfigMgr.getInstance().GetWord(0xbc0);
        this.RankLabel.text = ConfigMgr.getInstance().GetWord(0xbc1);
    }

    internal static void Open(EN_RankListType rankType, bool closeCamera3D = false)
    {
        <Open>c__AnonStorey24A storeya = new <Open>c__AnonStorey24A {
            rankType = rankType,
            closeCamera3D = closeCamera3D
        };
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().arena_ladder)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().arena_ladder));
        }
        else
        {
            GUIMgr.Instance.DoModelGUI<AllRankInOnePanel>(new Action<GUIEntity>(storeya.<>m__516), null);
        }
    }

    private void OpenItemInfo(GameObject go)
    {
        <OpenItemInfo>c__AnonStorey249 storey = new <OpenItemInfo>c__AnonStorey249 {
            <>f__this = this
        };
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storey.info = obj2 as RankListPlayerInfo;
            if (storey.info != null)
            {
                GUIMgr.Instance.DoModelGUI("TargetInfoPanel", new Action<GUIEntity>(storey.<>m__515), null);
            }
        }
    }

    private void ResetClipViewport()
    {
        GUIMgr.Instance.ListRoot.gameObject.SetActive(true);
        GUIMgr.Instance.ResetListViewpot(this.ListTopLeft, this.ListBottomRight, this.ItemList, 0f);
    }

    internal static void SetRankCapacity(EN_RankListType rankType, GameObject levelNameAndValue, UILabel rankCapacityName, UILabel rankCapacityValue, RankListPlayerInfo info)
    {
        if (!levelNameAndValue.activeSelf && (rankType != EN_RankListType.EN_RANKLIST_LEVEL))
        {
            levelNameAndValue.SetActive(true);
        }
        else if (levelNameAndValue.activeSelf && (rankType == EN_RankListType.EN_RANKLIST_LEVEL))
        {
            levelNameAndValue.SetActive(false);
        }
        switch (rankType)
        {
            case EN_RankListType.EN_RANKLIST_LEVEL:
                rankCapacityName.text = ConfigMgr.getInstance().GetWord(0xbb9);
                rankCapacityValue.text = info.level + string.Empty;
                break;

            case EN_RankListType.EN_RANKLIST_TOP_FIVE_CARD:
            case EN_RankListType.EN_RANKLIST_ALL_CARD:
            case EN_RankListType.EN_RANKLIST_AREAN:
            case EN_RankListType.EN_RANKLIST_LOL:
                rankCapacityName.text = ConfigMgr.getInstance().GetWord(0xbc3);
                if (rankType != EN_RankListType.EN_RANKLIST_ALL_CARD)
                {
                    rankCapacityValue.text = info.five_power + string.Empty;
                    break;
                }
                rankCapacityValue.text = info.all_power + string.Empty;
                break;

            case EN_RankListType.EN_RANKLIST_TOWER:
                rankCapacityName.text = ConfigMgr.getInstance().GetWord(0xbbc);
                rankCapacityValue.text = info.tower_num + string.Empty;
                break;
        }
    }

    internal static void SetRankValue(UISprite icon, UILabel valueLabel, int order)
    {
        if (!icon.gameObject.activeSelf)
        {
            icon.gameObject.SetActive(true);
        }
        if (!valueLabel.gameObject.activeSelf)
        {
            valueLabel.gameObject.SetActive(true);
        }
        valueLabel.text = (order > 3) ? (order + string.Empty) : string.Empty;
        switch (order)
        {
            case 0:
                icon.gameObject.SetActive(false);
                valueLabel.gameObject.SetActive(false);
                break;

            case 1:
                icon.spriteName = "Ui_Guildwar_Icon_1";
                break;

            case 2:
                icon.spriteName = "Ui_Guildwar_Icon_2";
                break;

            case 3:
                icon.spriteName = "Ui_Guildwar_Icon_3";
                break;

            default:
                icon.spriteName = string.Empty;
                break;
        }
    }

    private void ShowRoleInfo(bool show)
    {
        this.RoleName.gameObject.SetActive(show);
        this.GuildLabel.gameObject.SetActive(show);
        this.GuildName.gameObject.SetActive(show);
        this.RoleLevel.gameObject.SetActive(show);
        this.InfoHeadGroup.SetActive(show);
        this.RankingIcon.gameObject.SetActive(show);
        this.RankingValue.gameObject.SetActive(show);
        this.RoleLevelNameAndValue.gameObject.SetActive(show);
        this.RankCapacityName.gameObject.SetActive(show);
        this.RankCapacityValue.gameObject.SetActive(show);
    }

    private void TabButtonInit()
    {
        this.AddTabButton(this.RankTabLevel, EN_RankListType.EN_RANKLIST_LEVEL, ConfigMgr.getInstance().GetWord(0xbb9));
        this.AddTabButton(this.RankTabArena, EN_RankListType.EN_RANKLIST_AREAN, ConfigMgr.getInstance().GetWord(0xbba));
        this.AddTabButton(this.RankTabTower, EN_RankListType.EN_RANKLIST_TOWER, ConfigMgr.getInstance().GetWord(0x5a6));
        this.AddTabButton(this.RankTabAll, EN_RankListType.EN_RANKLIST_ALL_CARD, ConfigMgr.getInstance().GetWord(0xbbd));
        this.AddTabButton(this.RankTabTeam, EN_RankListType.EN_RANKLIST_TOP_FIVE_CARD, ConfigMgr.getInstance().GetWord(0xbbe));
        this.AddTabButton(this.RankTabLol, EN_RankListType.EN_RANKLIST_LOL, ConfigMgr.getInstance().GetWord(0xbc6));
    }

    private void TestInit()
    {
        if (this._myInfo == null)
        {
            this._myInfo = new RankListPlayerInfo();
            this._myInfo.headEntry = 1;
            this._myInfo.head_frame_entry = 3;
            this._myInfo.all_power = 0x1869f;
            this._myInfo.five_power = 0x1869e;
            this._myInfo.guildname = "大公会";
            this._myInfo.level = 0x3e7;
            this._myInfo.name = "充值三千万";
            this._myInfo.order = 0;
            this._myInfo.tower_num = 0x63;
            for (int i = 0; i < 50; i++)
            {
                RankListPlayerInfo item = new RankListPlayerInfo {
                    headEntry = 0,
                    all_power = 0x1b198,
                    five_power = 0x1388,
                    guildname = "公会" + i,
                    head_frame_entry = 2,
                    level = 50 - i,
                    name = "哀木涕 - " + i,
                    order = i,
                    tower_num = 0x63,
                    cardinfo = new List<RandListCardInfo>(5)
                };
                for (int j = 0; j < 5; j++)
                {
                    RandListCardInfo info2 = new RandListCardInfo {
                        entry = 1,
                        level = 0x37 - i,
                        starLv = j + 1,
                        quality = 4
                    };
                    item.cardinfo.Add(info2);
                }
                this._tempList.Add(item);
            }
            this.TestUpdate();
        }
    }

    private void TestUpdate()
    {
        this.UpdateData(this._myInfo, this._tempList);
    }

    internal void UpdateData(RankListPlayerInfo myInfo, List<RankListPlayerInfo> infos)
    {
        this.UpdateInfo(myInfo);
        if (infos != null)
        {
            bool flag = infos.Count > 3;
            for (int i = 0; i < infos.Count; i++)
            {
                GameObject obj2;
                if (this._rankItemGameObjects.Count > i)
                {
                    obj2 = this._rankItemGameObjects[i];
                }
                else
                {
                    obj2 = UnityEngine.Object.Instantiate(this.Item) as GameObject;
                    this._rankItemGameObjects.Add(obj2);
                }
                if (null != obj2)
                {
                    if (!obj2.activeSelf)
                    {
                        obj2.SetActive(true);
                    }
                    obj2.GetComponent<AllRankInOneListItem>().Init(this.CurrentRankType, infos[i]);
                    obj2.transform.parent = this.RankItemGrid.gameObject.transform;
                    obj2.transform.localPosition = Vector3.zero;
                    obj2.transform.localScale = Vector3.one;
                    obj2.GetComponent<UIDragCamera>().draggableCamera = !flag ? null : GUIMgr.Instance.ListDraggableCamera;
                    GUIDataHolder.setData(obj2, infos[i]);
                    UIEventListener.Get(obj2).onClick = new UIEventListener.VoidDelegate(this.OpenItemInfo);
                }
            }
            for (int j = infos.Count; j < this._rankItemGameObjects.Count; j++)
            {
                if ((null != this._rankItemGameObjects[j]) && this._rankItemGameObjects[j].activeSelf)
                {
                    this._rankItemGameObjects[j].SetActive(false);
                }
            }
            this.RankItemGrid.Reposition();
            this.RankItemGrid.transform.localPosition = Vector3.zero;
            this.ResetClipViewport();
        }
    }

    private void UpdateInfo(RankListPlayerInfo myInfo)
    {
        this.ShowRoleInfo(false);
        if (myInfo != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(myInfo.headEntry);
            if (_config != null)
            {
                this.RoleName.text = myInfo.name;
                this.GuildName.text = !string.IsNullOrEmpty(myInfo.guildname) ? myInfo.guildname : ConfigMgr.getInstance().GetWord(6);
                this.RoleLevel.text = myInfo.level + string.Empty;
                CommonFunc.SetPlayerHeadFrame(this.RoleFrame, this.RoleFrameIcon, myInfo.head_frame_entry);
                this.RoleHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                int order = (myInfo.order >= 0) ? (myInfo.order + 1) : 0;
                switch (this.CurrentRankType)
                {
                    case EN_RankListType.EN_RANKLIST_LEVEL:
                        this.Title.text = ConfigMgr.getInstance().GetWord(0xbb9) + ConfigMgr.getInstance().GetWord(0xbb8);
                        this.RankListTitle.text = ConfigMgr.getInstance().GetWord(0xbb9);
                        break;

                    case EN_RankListType.EN_RANKLIST_TOP_FIVE_CARD:
                        this.Title.text = ConfigMgr.getInstance().GetWord(0xbbe) + ConfigMgr.getInstance().GetWord(0xbb8);
                        this.RankListTitle.text = ConfigMgr.getInstance().GetWord(0xbbf) + ConfigMgr.getInstance().GetWord(0xbbe);
                        break;

                    case EN_RankListType.EN_RANKLIST_ALL_CARD:
                        this.Title.text = ConfigMgr.getInstance().GetWord(0xbbd) + ConfigMgr.getInstance().GetWord(0xbb8);
                        this.RankListTitle.text = ConfigMgr.getInstance().GetWord(0xbc2);
                        break;

                    case EN_RankListType.EN_RANKLIST_TOWER:
                        this.Title.text = ConfigMgr.getInstance().GetWord(0x5a6) + ConfigMgr.getInstance().GetWord(0xbb8);
                        this.RankListTitle.text = ConfigMgr.getInstance().GetWord(0x5a6) + ConfigMgr.getInstance().GetWord(0xbbb);
                        if (myInfo.tower_num == 0)
                        {
                            order = -1;
                        }
                        break;

                    case EN_RankListType.EN_RANKLIST_AREAN:
                        this.Title.text = ConfigMgr.getInstance().GetWord(0xbba) + ConfigMgr.getInstance().GetWord(0xbb8);
                        this.RankListTitle.text = ConfigMgr.getInstance().GetWord(0xbc2);
                        break;

                    case EN_RankListType.EN_RANKLIST_LOL:
                        this.Title.text = ConfigMgr.getInstance().GetWord(0xbc6) + ConfigMgr.getInstance().GetWord(0xbb8);
                        this.RankListTitle.text = ConfigMgr.getInstance().GetWord(0xbc2);
                        break;
                }
                this.ShowRoleInfo(true);
                SetRankValue(this.RankingIcon, this.RankingValue, order);
                SetRankCapacity(this.CurrentRankType, this.RoleLevelNameAndValue, this.RankCapacityName, this.RankCapacityValue, myInfo);
            }
        }
    }

    private EN_RankListType CurrentRankType
    {
        get
        {
            return _currentRankType;
        }
        set
        {
            _currentRankType = value;
            this.ChangeTabBg(this.RankTabLevel, EN_RankListType.EN_RANKLIST_LEVEL);
            this.ChangeTabBg(this.RankTabArena, EN_RankListType.EN_RANKLIST_AREAN);
            this.ChangeTabBg(this.RankTabTower, EN_RankListType.EN_RANKLIST_TOWER);
            this.ChangeTabBg(this.RankTabAll, EN_RankListType.EN_RANKLIST_ALL_CARD);
            this.ChangeTabBg(this.RankTabTeam, EN_RankListType.EN_RANKLIST_TOP_FIVE_CARD);
            this.ChangeTabBg(this.RankTabLol, EN_RankListType.EN_RANKLIST_LOL);
        }
    }

    [CompilerGenerated]
    private sealed class <AddTabButton>c__AnonStorey248
    {
        internal AllRankInOnePanel <>f__this;
        internal EN_RankListType rankType;

        internal void <>m__514()
        {
            this.<>f__this.CurrentRankType = this.rankType;
        }
    }

    [CompilerGenerated]
    private sealed class <Open>c__AnonStorey24A
    {
        internal bool closeCamera3D;
        internal EN_RankListType rankType;

        internal void <>m__516(GUIEntity entity)
        {
            AllRankInOnePanel panel = (AllRankInOnePanel) entity;
            panel.CurrentRankType = this.rankType;
            if (this.closeCamera3D)
            {
                panel._camera3D = GUIMgr.Instance.Camera3D;
                if (!panel._camera3D.enabled)
                {
                    panel._camera3D = null;
                }
                else
                {
                    panel._camera3D.enabled = false;
                }
            }
            panel.GetServerData();
        }
    }

    [CompilerGenerated]
    private sealed class <OpenItemInfo>c__AnonStorey249
    {
        internal AllRankInOnePanel <>f__this;
        internal RankListPlayerInfo info;

        internal void <>m__515(GUIEntity obj)
        {
            ((TargetInfoPanel) obj).UpdateDataForAllRank(this.<>f__this.CurrentRankType, this.info);
        }
    }
}

