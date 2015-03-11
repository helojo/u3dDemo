using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AllRankInOneListItem : MonoBehaviour
{
    private readonly List<GameObject> _cardGameObjects = new List<GameObject>(8);
    private int _theRankingValue;
    [CompilerGenerated]
    private static Comparison<RandListCardInfo> <>f__am$cache10;
    public UIGrid CardGrid;
    public GameObject CardTeamItem;
    public UILabel GuildName;
    public GameObject LevelNameAndValue;
    public UILabel LevelValue;
    public UILabel RankCapacityName;
    public UILabel RankCapacityValue;
    public UISprite RankingIcon;
    public UILabel RankingValue;
    internal EN_RankListType RankType;
    public UISprite RoleFrame;
    public UISprite RoleFrameIcon;
    public UITexture RoleHeadIcon;
    public UILabel RoleName;

    internal void Init(EN_RankListType rankType, RankListPlayerInfo info)
    {
        this.RoleName.text = info.name;
        this.GuildName.text = !string.IsNullOrEmpty(info.guildname) ? info.guildname : ConfigMgr.getInstance().GetWord(6);
        this.LevelValue.text = info.level + string.Empty;
        CommonFunc.SetPlayerHeadFrame(this.RoleFrame, this.RoleFrameIcon, info.head_frame_entry);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.headEntry);
        if (_config != null)
        {
            this.RoleHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            for (int i = 0; i < this._cardGameObjects.Count; i++)
            {
                GameObject obj2 = this._cardGameObjects[i];
                if (null != obj2)
                {
                    obj2.gameObject.SetActive(rankType != EN_RankListType.EN_RANKLIST_LOL);
                }
            }
            if ((info.order < 10) && (rankType != EN_RankListType.EN_RANKLIST_LOL))
            {
                List<RandListCardInfo> cardinfo = info.cardinfo;
                if (<>f__am$cache10 == null)
                {
                    <>f__am$cache10 = delegate (RandListCardInfo card1, RandListCardInfo card2) {
                        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(card1.entry);
                        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(card2.entry);
                        int num = (_config == null) ? -1 : _config.card_position;
                        int num2 = (_config2 == null) ? -1 : _config2.card_position;
                        return num - num2;
                    };
                }
                cardinfo.Sort(<>f__am$cache10);
                for (int j = this._cardGameObjects.Count; j < cardinfo.Count; j++)
                {
                    GameObject obj3 = UnityEngine.Object.Instantiate(this.CardTeamItem) as GameObject;
                    if (null != obj3)
                    {
                        this._cardGameObjects.Add(obj3);
                    }
                }
                for (int k = cardinfo.Count - 1; k >= 0; k--)
                {
                    RandListCardInfo info2 = cardinfo[k];
                    GameObject obj4 = this._cardGameObjects[k];
                    if (!obj4.activeSelf)
                    {
                        obj4.SetActive(true);
                    }
                    CardTeamItem component = obj4.GetComponent<CardTeamItem>();
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(info2.entry);
                    component.InitRankItemData(_config2.image, info2.quality, info2.level, info2.starLv, _config2.class_type);
                    obj4.transform.parent = this.CardGrid.transform;
                    obj4.transform.localPosition = Vector3.zero;
                    obj4.transform.localScale = Vector3.one;
                }
                for (int m = cardinfo.Count; m < this._cardGameObjects.Count; m++)
                {
                    if (null != this._cardGameObjects[m])
                    {
                        this._cardGameObjects[m].SetActive(false);
                    }
                }
                this.CardGrid.hideInactive = true;
                this.CardGrid.Reposition();
                this.CardGrid.transform.localPosition = new Vector3(320f, -28f, 0f);
            }
            this.RankCapacityValue.text = info.all_power + string.Empty;
            this.TheRankingValue = info.order + 1;
            AllRankInOnePanel.SetRankCapacity(rankType, this.LevelNameAndValue, this.RankCapacityName, this.RankCapacityValue, info);
        }
    }

    internal int TheRankingValue
    {
        get
        {
            return this._theRankingValue;
        }
        set
        {
            this._theRankingValue = value;
            this.RankingIcon.spriteName = string.Empty;
            if (this._theRankingValue <= 0)
            {
                this.RankingValue.text = string.Empty;
            }
            else
            {
                AllRankInOnePanel.SetRankValue(this.RankingIcon, this.RankingValue, this._theRankingValue);
            }
        }
    }
}

