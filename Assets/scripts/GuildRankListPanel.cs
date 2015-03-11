using FastBuf;
using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildRankListPanel : GUIEntity
{
    public GameObject _SingleDamageItem;
    public GameObject _SingleMarsItem;
    public GameObject _SingleResourceItem;
    private List<GuildBattleDamageRankInfo> m_DamageList = new List<GuildBattleDamageRankInfo>();
    private List<GuildBattleResourceRankInfo> m_ResourceList = new List<GuildBattleResourceRankInfo>();
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private List<GuildBattleWinTimesRankInfo> m_WinTimesList = new List<GuildBattleWinTimesRankInfo>();
    private bool mIsStart;
    public int mStandType = -1;

    private void OnClosePanel()
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        SocketMgr.Instance.RequestGuildBattleDamageRank();
        base.transform.FindChild("Tab/Damage").GetComponent<UIToggle>().value = true;
        this.mStandType = 0;
        base.transform.FindChild("ListDamage").gameObject.SetActive(this.mStandType == 0);
        base.transform.FindChild("Mars_Bg").gameObject.SetActive(this.mStandType == 1);
        base.transform.FindChild("ListResource").gameObject.SetActive(this.mStandType == 2);
        CommonFunc.ResetClippingPanel(base.transform.FindChild("ListDamage"));
        CommonFunc.ResetClippingPanel(base.transform.FindChild("ListResource"));
        CommonFunc.ResetClippingPanel(base.transform.FindChild("Mars_Bg/List"));
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
            }
        }
    }

    private unsafe void SetDamageInfo(GameObject obj, GuildBattleDamageRankInfo info)
    {
        UILabel component = obj.transform.FindChild("Level").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("name").GetComponent<UILabel>();
        UILabel label3 = obj.transform.FindChild("num").GetComponent<UILabel>();
        component.text = "Lv " + info.lvl;
        label2.text = info.name;
        label3.text = string.Empty + info.damage;
        for (int i = 0; i < info.cards.Count; i++)
        {
            obj.transform.FindChild("frame" + (i + 1)).gameObject.SetActive(true);
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.cards[i].entry);
            obj.transform.FindChild("frame" + (i + 1)).GetComponent<UISprite>().color = *((Color*) &(GameConstant.ConstQuantityColor[_config.quality]));
            obj.transform.FindChild("frame" + (i + 1) + "/Label").GetComponent<UILabel>().text = string.Empty + info.cards[i].lvl;
            obj.transform.FindChild("frame" + (i + 1) + "/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            CommonFunc.SetQualityBorder(obj.transform.FindChild("frame" + (i + 1) + "/QualityBorder").GetComponent<UISprite>(), info.cards[i].quality);
            for (int j = 0; j < 5; j++)
            {
                object[] objArray1 = new object[] { "frame", i + 1, "/Star/", j + 1 };
                UISprite sprite2 = obj.transform.FindChild(string.Concat(objArray1)).GetComponent<UISprite>();
                sprite2.gameObject.SetActive(j < info.cards[i].starLv);
                sprite2.transform.localPosition = new Vector3((float) (j * 0x10), 0f, 0f);
            }
            Transform transform = obj.transform.FindChild("frame" + (i + 1) + "/Star");
            transform.localPosition = new Vector3(-10f - ((info.cards[i].starLv - 1) * 8f), transform.localPosition.y, 0f);
            transform.gameObject.SetActive(true);
        }
    }

    private void SetResourceInfo(GameObject obj, GuildBattleResourceRankInfo info)
    {
        UILabel component = obj.transform.FindChild("name").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("Level").GetComponent<UILabel>();
        UITexture texture = obj.transform.FindChild("IconBg/Icon").GetComponent<UITexture>();
        UILabel label3 = obj.transform.FindChild("res_num").GetComponent<UILabel>();
        UILabel label4 = obj.transform.FindChild("member").GetComponent<UILabel>();
        component.text = info.name;
        label2.text = "Lv " + (info.lvl + 1);
        label3.text = string.Empty + info.recource;
        label4.text = string.Empty + info.memberCount;
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.headEntry);
        if (_config != null)
        {
            texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
    }

    private void SetWinTimesInfo(GameObject obj, GuildBattleWinTimesRankInfo info)
    {
        UILabel component = obj.transform.FindChild("Level").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("Name").GetComponent<UILabel>();
        UILabel label3 = obj.transform.FindChild("resCount").GetComponent<UILabel>();
        label2.text = info.name;
        component.text = "Lv." + info.lvl;
        label3.text = string.Empty + info.winTimes;
    }

    public void ShowDamegeList()
    {
        float y = 0f;
        UIGrid component = base.transform.FindChild("ListDamage/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        base.transform.FindChild("desc").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x4e3d), ActorData.getInstance().mGuildData.name);
        base.transform.FindChild("no_rank").gameObject.SetActive(this.m_DamageList.Count == 0);
        for (int i = 0; i < this.m_DamageList.Count; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleDamageItem) as GameObject;
            obj2.transform.parent = component.transform;
            obj2.transform.localPosition = new Vector3(0f, y, -0.1f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            y -= component.cellHeight;
            UISprite sprite = obj2.transform.FindChild("RankIcon").GetComponent<UISprite>();
            UILabel label = obj2.transform.FindChild("Rank").GetComponent<UILabel>();
            label.gameObject.SetActive(i >= 3);
            sprite.gameObject.SetActive(i < 3);
            if (i < 3)
            {
                sprite.spriteName = "Ui_Guildwar_Icon_" + (i + 1);
                sprite.MakePixelPerfect();
            }
            else
            {
                label.text = (i + 1) + string.Empty;
            }
            this.SetDamageInfo(obj2, this.m_DamageList[i]);
        }
    }

    private void ShowRankList()
    {
        PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
        if (component != null)
        {
            FsmInt num = component.FsmVariables.FindFsmInt("standType");
            this.mStandType = num.Value;
            float y = 0f;
            base.transform.FindChild("ListDamage").gameObject.SetActive(this.mStandType == 0);
            base.transform.FindChild("Mars_Bg").gameObject.SetActive(this.mStandType == 1);
            base.transform.FindChild("ListResource").gameObject.SetActive(this.mStandType == 2);
            if (this.mStandType == 0)
            {
                UIGrid grid = base.transform.FindChild("ListDamage/Grid").GetComponent<UIGrid>();
                CommonFunc.DeleteChildItem(grid.transform);
                base.transform.FindChild("desc").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x4e3d), ActorData.getInstance().mGuildData.name);
                base.transform.FindChild("no_rank").gameObject.SetActive(this.m_DamageList.Count == 0);
                for (int i = 0; i < this.m_DamageList.Count; i++)
                {
                    GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleDamageItem) as GameObject;
                    obj2.transform.parent = grid.transform;
                    obj2.transform.localPosition = new Vector3(0f, y, -0.1f);
                    obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                    y -= grid.cellHeight;
                    UISprite sprite = obj2.transform.FindChild("RankIcon").GetComponent<UISprite>();
                    UILabel label = obj2.transform.FindChild("Rank").GetComponent<UILabel>();
                    label.gameObject.SetActive(i >= 3);
                    sprite.gameObject.SetActive(i < 3);
                    if (i < 3)
                    {
                        sprite.spriteName = "Ui_Guildwar_Icon_" + (i + 1);
                        sprite.MakePixelPerfect();
                    }
                    else
                    {
                        label.text = (i + 1) + string.Empty;
                    }
                    this.SetDamageInfo(obj2, this.m_DamageList[i]);
                }
            }
            else if (this.mStandType == 1)
            {
                UIGrid grid2 = base.transform.FindChild("Mars_Bg/List/Grid").GetComponent<UIGrid>();
                CommonFunc.DeleteChildItem(grid2.transform);
                base.transform.FindChild("desc").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x4e3f), ActorData.getInstance().mGuildData.name);
                base.transform.FindChild("no_rank").gameObject.SetActive(this.m_WinTimesList.Count == 0);
                for (int j = 0; j < this.m_WinTimesList.Count; j++)
                {
                    GameObject obj3 = UnityEngine.Object.Instantiate(this._SingleMarsItem) as GameObject;
                    obj3.transform.parent = grid2.transform;
                    obj3.transform.localPosition = new Vector3(0f, y, -0.1f);
                    obj3.transform.localScale = new Vector3(1f, 1f, 1f);
                    y -= grid2.cellHeight;
                    obj3.transform.FindChild("Rank").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x4e3b), j + 1);
                    this.SetWinTimesInfo(obj3, this.m_WinTimesList[j]);
                }
            }
            else if (this.mStandType == 2)
            {
                UIGrid grid3 = base.transform.FindChild("ListResource/Grid").GetComponent<UIGrid>();
                CommonFunc.DeleteChildItem(grid3.transform);
                base.transform.FindChild("desc").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e3e);
                base.transform.FindChild("no_rank").gameObject.SetActive(this.m_ResourceList.Count == 0);
                for (int k = 0; k < this.m_ResourceList.Count; k++)
                {
                    GameObject obj4 = UnityEngine.Object.Instantiate(this._SingleResourceItem) as GameObject;
                    obj4.transform.parent = grid3.transform;
                    obj4.transform.localPosition = new Vector3(0f, y, -0.1f);
                    obj4.transform.localScale = new Vector3(1f, 1f, 1f);
                    y -= grid3.cellHeight;
                    UISprite sprite2 = obj4.transform.FindChild("RankIcon").GetComponent<UISprite>();
                    UILabel label3 = obj4.transform.FindChild("Rank").GetComponent<UILabel>();
                    label3.gameObject.SetActive(k >= 3);
                    sprite2.gameObject.SetActive(k < 3);
                    if (k < 3)
                    {
                        sprite2.spriteName = "Ui_Guildwar_Icon_" + (k + 1);
                        sprite2.MakePixelPerfect();
                    }
                    else
                    {
                        label3.text = (k + 1) + string.Empty;
                    }
                    this.SetResourceInfo(obj4, this.m_ResourceList[k]);
                }
            }
        }
    }

    public void UpdateDamageData(GuildBattleDamageRankList info)
    {
        this.m_DamageList = info.rankList;
        SocketMgr.Instance.RequestGuildBattleMarsRank();
    }

    public void UpdateResourceData(GuildBattleResourceRankList info)
    {
        this.m_ResourceList = info.rankList;
        this.ShowDamegeList();
    }

    public void UpdateWinTimesData(GuildBattleWinTimesRankList info)
    {
        this.m_WinTimesList = info.rankList;
        SocketMgr.Instance.RequestGuildBattleResourceRank();
    }
}

