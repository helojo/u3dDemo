using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuildHallPanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache7;
    public UIGrid gridroot;
    private GameObject GuildMebmerIns;
    public GameObject GuildMebmerObj;
    public GameObject GuildMemberCtrlObj;
    private const int LineCount = 2;
    private int mCurrPage;
    private int mPageCount;
    private const int PAGE_CAPACITY = 10;
    private GameObject PerRootObj;

    private void ClickLevUp()
    {
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = delegate (GUIEntity guiE) {
                GuildLevUpPanel panel = guiE.Achieve<GuildLevUpPanel>();
                panel.Depth = 400;
                panel.UpdateData(GuildFuncType.Hall);
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildLevUpPanel", <>f__am$cache7, null);
    }

    private void OnClickMebmer(GameObject obj)
    {
        GuildMember member = (GuildMember) GUIDataHolder.getData(obj);
        GameObject gameObject = obj.transform.FindChild("Root").gameObject;
        if (this.GuildMebmerIns == null)
        {
            this.GuildMebmerIns = UnityEngine.Object.Instantiate(this.GuildMemberCtrlObj) as GameObject;
        }
        if (this.PerRootObj != null)
        {
            this.PerRootObj.SetActive(true);
        }
        this.GuildMebmerIns.transform.parent = obj.transform;
        this.GuildMebmerIns.transform.localPosition = Vector3.zero;
        this.GuildMebmerIns.transform.localScale = Vector3.one;
        this.GuildMebmerIns.SetActive(true);
        GuildMemberCtrl component = this.GuildMebmerIns.GetComponent<GuildMemberCtrl>();
        if (component != null)
        {
            this.PerRootObj = gameObject;
            gameObject.SetActive(false);
            component.UpdateData(member, gameObject);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.UpdateHallLev();
        this.UpdateContribution();
        this.SetPageInfo(ActorData.getInstance().mGuildMemberData.member.Count);
        this.UpdateGuildMember();
        GUIMgr.Instance.FloatTitleBar();
    }

    private void OnPageDown()
    {
        this.mCurrPage++;
        if (this.mCurrPage >= this.mPageCount)
        {
            this.mCurrPage = this.mPageCount - 1;
        }
        else
        {
            this.UpdateGuildMember();
        }
    }

    private void OnPageUp()
    {
        this.mCurrPage--;
        if (this.mCurrPage < 0)
        {
            this.mCurrPage = 0;
        }
        else
        {
            this.UpdateGuildMember();
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    private void SetPageInfo(int nCount)
    {
        this.mPageCount = ((nCount + 10) - 1) / 10;
    }

    private void UpdateBuildLevUpLimit()
    {
        GameObject gameObject = base.gameObject.transform.FindChild("BuildLvUp/Button").gameObject;
        if (ActorData.getInstance().mUserGuildMemberData.position == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateContribution()
    {
        base.gameObject.transform.FindChild("BuildLvUp/Val").GetComponent<UILabel>().text = ActorData.getInstance().mGuildData.cur_contribution.ToString();
    }

    public void UpdateGuildMember()
    {
        this.UpdateMemberCnt();
        this.UpdateBuildLevUpLimit();
        CommonFunc.ResetClippingPanel(base.transform.FindChild("Scroll View"));
        int index = this.mCurrPage * 10;
        int count = Math.Min(10, ActorData.getInstance().mGuildMemberData.member.Count - index);
        if (this.mCurrPage == 0)
        {
            base.gameObject.transform.FindChild("ButtonLeft").gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.transform.FindChild("ButtonLeft").gameObject.SetActive(true);
        }
        if (this.mCurrPage == (this.mPageCount - 1))
        {
            base.gameObject.transform.FindChild("ButtonRight").gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.transform.FindChild("ButtonRight").gameObject.SetActive(true);
        }
        base.gameObject.transform.FindChild("LabelPageCount/Label").GetComponent<UILabel>().text = (this.mCurrPage + 1) + "/" + this.mPageCount;
        CommonFunc.DeleteChildItem(this.gridroot.transform);
        List<GuildMember> list = new List<GuildMember>();
        foreach (GuildMember member in ActorData.getInstance().mGuildMemberData.member.GetRange(index, count))
        {
            GameObject obj3 = UnityEngine.Object.Instantiate(this.GuildMebmerObj) as GameObject;
            obj3.transform.parent = this.gridroot.transform;
            obj3.transform.localPosition = Vector3.zero;
            obj3.transform.localScale = Vector3.one;
            this.UpdateItemData(obj3, member);
            index++;
        }
        this.gridroot.repositionNow = true;
    }

    public void UpdateHallLev()
    {
        base.gameObject.transform.FindChild("Lev/val").GetComponent<UILabel>().text = (ActorData.getInstance().mGuildData.tech.hall_level + 1).ToString();
    }

    private void UpdateItemData(GameObject _obj, GuildMember _data)
    {
        GuildCommonFunc.UpdateGuildMemberData(_obj, _data.userInfo);
        UILabel component = _obj.transform.FindChild("Root/Label").GetComponent<UILabel>();
        _obj.transform.FindChild("Root/Time").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(_data.userInfo.lastOnlineTime));
        if (_data.position == 1)
        {
            component.text = ConfigMgr.getInstance().GetWord(0xa652bb);
        }
        else
        {
            component.text = ConfigMgr.getInstance().GetWord(0xa652bc);
        }
        GUIDataHolder.setData(_obj.gameObject, _data);
        UIEventListener listener1 = UIEventListener.Get(_obj);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickMebmer));
    }

    public void UpdateMemberCnt()
    {
        guild_hall_config _config = ConfigMgr.getInstance().getByEntry<guild_hall_config>((int) ActorData.getInstance().mGuildData.tech.hall_level);
        base.gameObject.transform.FindChild("MemberCnt/Cnt").GetComponent<UILabel>().text = ActorData.getInstance().mGuildMemberData.member.Count + "/" + _config.member_limit;
    }
}

