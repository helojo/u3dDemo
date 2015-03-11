using FastBuf;
using System;
using UnityEngine;

public class GuildMemberCtrl : MonoBehaviour
{
    private GuildMember CurMember;
    private GameObject mShowObj;

    private void ClickExpelMember(GameObject Cobj)
    {
        if (this.CurMember.userInfo.id == ActorData.getInstance().SessionInfo.userid)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa6528b));
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0xa6528c), box => SocketMgr.Instance.RequestGuildExpelMember(this.CurMember.userInfo.id), null, false), base.gameObject);
        }
    }

    private void ClickGuildTransfer(GameObject Cobj)
    {
        if (this.CurMember.userInfo.id == ActorData.getInstance().SessionInfo.userid)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa6528b));
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0xa6528d), box => SocketMgr.Instance.RequestGuildTransfer(this.CurMember.userInfo.id), null, false), base.gameObject);
        }
    }

    private void Close(GameObject obj)
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }

    private void OnDestroy()
    {
        if (this.mShowObj != null)
        {
            this.mShowObj.SetActive(true);
        }
    }

    private void SendEMail(GameObject Cobj)
    {
        if (this.CurMember.userInfo.id == ActorData.getInstance().SessionInfo.userid)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa6528b));
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MailPanel", delegate (GUIEntity obj) {
                MailPanel panel = (MailPanel) obj;
                panel.Depth = 400;
                BriefUser user = new BriefUser {
                    id = this.CurMember.userInfo.id,
                    name = this.CurMember.userInfo.name,
                    level = this.CurMember.userInfo.level,
                    faction = this.CurMember.userInfo.faction,
                    lastOnlineTime = this.CurMember.userInfo.lastOnlineTime,
                    titleEntry = this.CurMember.userInfo.titleEntry,
                    head_entry = this.CurMember.userInfo.head_entry
                };
                panel.SetMailSendTo(user);
            }, null);
        }
    }

    private void Start()
    {
        GameObject gameObject = base.gameObject.transform.FindChild("Button1").gameObject;
        GameObject go = base.gameObject.transform.FindChild("Button2").gameObject;
        GameObject obj4 = base.gameObject.transform.FindChild("Button3").gameObject;
        GameObject obj5 = base.gameObject.transform.FindChild("ButtonClose").gameObject;
        if (ActorData.getInstance().mUserGuildMemberData.position == 1)
        {
            go.SetActive(true);
            obj4.SetActive(true);
        }
        else
        {
            go.SetActive(false);
            obj4.SetActive(false);
        }
        UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.SendEMail);
        UIEventListener.Get(go).onClick = new UIEventListener.VoidDelegate(this.ClickGuildTransfer);
        UIEventListener.Get(obj4).onClick = new UIEventListener.VoidDelegate(this.ClickExpelMember);
        UIEventListener.Get(obj5).onClick = new UIEventListener.VoidDelegate(this.Close);
    }

    public void UpdateData(GuildMember _data, GameObject _obj)
    {
        this.mShowObj = _obj;
        this.CurMember = _data;
    }
}

