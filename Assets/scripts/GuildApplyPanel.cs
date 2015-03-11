using FastBuf;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildApplyPanel : GUIEntity
{
    public UIGrid gridroot;
    public GameObject GuildApplyObj;
    private const int LineCount = 2;

    private void OnClickAgreeBtn(GameObject obj)
    {
        GuildApplication application = (GuildApplication) GUIDataHolder.getData(obj);
        SocketMgr.Instance.RequestGuildApplicationProcess(application.user_data.id, true);
    }

    private void OnClickRejectBtn(GameObject obj)
    {
        GuildApplication application = (GuildApplication) GUIDataHolder.getData(obj);
        SocketMgr.Instance.RequestGuildApplicationProcess(application.user_data.id, false);
    }

    public override void OnInitialize()
    {
        this.UpdateMemberCount();
        this.UpdateGuildLev();
        base.OnInitialize();
    }

    public void UpdateData(List<GuildApplication> _listData)
    {
        int num = 0;
        CommonFunc.DeleteChildItem(this.gridroot.transform);
        List<GuildApplication> list = _listData;
        foreach (GuildApplication application in list)
        {
            GameObject obj3 = UnityEngine.Object.Instantiate(this.GuildApplyObj) as GameObject;
            obj3.transform.parent = this.gridroot.transform;
            obj3.transform.localPosition = new Vector3((float) (-170 + (num * 350)), 0f, 0f);
            obj3.transform.localScale = Vector3.one;
            this.UpdateItemData(obj3, application);
            num++;
        }
        this.gridroot.repositionNow = true;
    }

    private void UpdateGuildLev()
    {
        base.gameObject.transform.FindChild("Lev/val").GetComponent<UILabel>().text = (ActorData.getInstance().mGuildData.tech.hall_level + 1).ToString();
    }

    private void UpdateItemData(GameObject _obj, GuildApplication _data)
    {
        GuildCommonFunc.UpdateGuildMemberData2(_obj, _data.user_data);
        UIButton component = _obj.transform.FindChild("BtnGroup/Button1").GetComponent<UIButton>();
        UIButton button2 = _obj.transform.FindChild("BtnGroup/Button2").GetComponent<UIButton>();
        GUIDataHolder.setData(component.gameObject, _data);
        GUIDataHolder.setData(button2.gameObject, _data);
        UIEventListener.Get(component.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAgreeBtn);
        UIEventListener.Get(button2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRejectBtn);
    }

    public void UpdateMemberCount()
    {
        guild_hall_config _config = ConfigMgr.getInstance().getByEntry<guild_hall_config>((int) ActorData.getInstance().mGuildData.tech.hall_level);
        if (_config != null)
        {
            base.gameObject.transform.FindChild("MemberCnt/Cnt").GetComponent<UILabel>().text = ActorData.getInstance().mGuildMemberData.member.Count + "/" + _config.member_limit;
        }
    }
}

