using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RankingListPanel : GUIEntity
{
    public GameObject _SingleRankingItem;
    public GameObject _SingleRankingTopItem;
    [CompilerGenerated]
    private static Comparison<ArenaLadderEnemy> <>f__am$cache2;

    private void OnClickArenaItemBtn(GameObject go)
    {
        <OnClickArenaItemBtn>c__AnonStorey172 storey = new <OnClickArenaItemBtn>c__AnonStorey172();
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storey.info = obj2 as ArenaLadderEnemy;
            if (storey.info != null)
            {
                GUIMgr.Instance.DoModelGUI("TargetInfoPanel", new Action<GUIEntity>(storey.<>m__19C), null);
            }
        }
    }

    private void OnClickItemBtn(GameObject go)
    {
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        this.ResetClipViewport();
    }

    private void ResetClipViewport()
    {
        GUIMgr.Instance.ListRoot.gameObject.SetActive(true);
        Transform top = base.transform.FindChild("ListTopLeft");
        Transform bottom = base.transform.FindChild("ListBottomRight");
        Transform bounds = base.transform.FindChild("List");
        GUIMgr.Instance.ResetListViewpot(top, bottom, bounds, 0f);
    }

    private void SetArenaRankingInfo(GameObject obj, ArenaLadderEnemy info)
    {
        this.SetBaseInfo(obj, info.order, info.level, info.name, info.head_entry);
        UISprite component = obj.transform.FindChild("Item/RankIcon").GetComponent<UISprite>();
        UILabel label = obj.transform.FindChild("Item/Rank").GetComponent<UILabel>();
        UISprite frame = obj.transform.FindChild("Item/Head/QualityBorder").GetComponent<UISprite>();
        UISprite sprite3 = obj.transform.FindChild("Item/Head/QualityBorder/QIcon").GetComponent<UISprite>();
        CommonFunc.SetPlayerHeadFrame(frame, sprite3, info.head_frame_entry);
        if (info.order < 4)
        {
            component.spriteName = "Ui_Guildwar_Icon_" + info.order;
            component.gameObject.SetActive(true);
        }
        else
        {
            component.gameObject.SetActive(false);
            label.text = info.order.ToString();
        }
    }

    private void SetBaseInfo(GameObject obj, int _rank, int level, string name, int headEntry)
    {
        int num = _rank + 1;
        bool flag = (num % 2) == 0;
        UISprite component = obj.transform.FindChild("Item/Border").GetComponent<UISprite>();
        obj.transform.FindChild("Item/Level").GetComponent<UILabel>().text = "Lv." + level;
        obj.transform.FindChild("Item/Name").GetComponent<UILabel>().text = name;
        UITexture texture = obj.transform.FindChild("Item/Head/Icon").GetComponent<UITexture>();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(headEntry);
        if (_config != null)
        {
            texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
    }

    private void SetRankingInfo(GameObject obj, LeagueOpponent info)
    {
        this.SetBaseInfo(obj, info.rank, info.userInfo.level, info.userInfo.name, info.userInfo.head_entry);
        UILabel component = obj.transform.FindChild("Item/Rank").GetComponent<UILabel>();
        component.text = info.rank.ToString();
        component.color = ((info.rank % 2) == 0) ? ((Color) new Color32(0xfc, 210, 170, 0xff)) : ((Color) new Color32(120, 0x5c, 0x44, 0xff));
    }

    private void SetTopRankingInfo(GameObject obj, LeagueOpponent info)
    {
        this.SetBaseInfo(obj, info.rank, info.userInfo.level, info.userInfo.name, info.userInfo.head_entry);
        UISprite component = obj.transform.FindChild("Item/RankIcon").GetComponent<UISprite>();
        component.spriteName = "Ui_Guildwar_Icon_" + info.rank;
        component.MakePixelPerfect();
    }

    public void ShowArenaRanking(List<ArenaLadderEnemy> enemys)
    {
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = (e1, e2) => e1.order - e2.order;
        }
        enemys.Sort(<>f__am$cache2);
        Transform transform = base.transform.FindChild("List");
        int num = 0;
        for (int i = 0; i < enemys.Count; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleRankingTopItem) as GameObject;
            obj2.transform.parent = transform;
            obj2.transform.localPosition = new Vector3(0f, (float) num, -0.1f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            this.SetArenaRankingInfo(obj2, enemys[i]);
            num -= 120;
            Transform transform2 = obj2.transform.FindChild("Item");
            UIDragCamera component = transform2.GetComponent<UIDragCamera>();
            if (null != component)
            {
                component.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
            }
            GUIDataHolder.setData(transform2.gameObject, enemys[i]);
            UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickArenaItemBtn);
            obj2.transform.FindChild("Item/self").gameObject.SetActive(enemys[i].targetId == ActorData.getInstance().SessionInfo.userid);
        }
    }

    private int SortByLevel(LeagueOpponent Obj1, LeagueOpponent Obj2)
    {
        return (Obj1.rank - Obj2.rank);
    }

    public void UpdateData(List<LeagueOpponent> _userList)
    {
        _userList.Sort(new Comparison<LeagueOpponent>(this.SortByLevel));
        Debug.Log("UserList.cout" + _userList.Count);
        Transform transform = base.transform.FindChild("List");
        int num = 0;
        for (int i = 0; i < _userList.Count; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleRankingTopItem) as GameObject;
            obj2.transform.parent = transform;
            obj2.transform.localPosition = new Vector3(0f, (float) num, -0.1f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            if (i < 3)
            {
                this.SetTopRankingInfo(obj2, _userList[i]);
                num -= 120;
            }
            else
            {
                this.SetRankingInfo(obj2, _userList[i]);
                num -= 120;
            }
            Transform transform2 = obj2.transform.FindChild("Item");
            UIDragCamera component = transform2.GetComponent<UIDragCamera>();
            if (null != component)
            {
                component.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
            }
            GUIDataHolder.setData(transform2.gameObject, _userList[i]);
            UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickItemBtn);
            obj2.transform.FindChild("Item/self").gameObject.SetActive(_userList[i].userInfo.id == ActorData.getInstance().SessionInfo.userid);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickArenaItemBtn>c__AnonStorey172
    {
        internal ArenaLadderEnemy info;

        internal void <>m__19C(GUIEntity obj)
        {
            ((TargetInfoPanel) obj).UpdateData(this.info);
        }
    }
}

