using FastBuf;
using System;
using UnityEngine;

public class GuildMemberPanel : GUIEntity
{
    private GuildMember CurMember;
    private GameObject CurObj;

    private void ClickExpelMember()
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

    private void ClickGuildTransfer()
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

    private void ClickSendMail()
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

    public override void OnInitialize()
    {
        if (ActorData.getInstance().mUserGuildMemberData.position == 1)
        {
            base.gameObject.transform.FindChild("NormalOp").gameObject.SetActive(false);
            base.gameObject.transform.FindChild("ChairmanOP").gameObject.SetActive(true);
            this.CurObj = base.gameObject.transform.FindChild("ChairmanOP").gameObject;
        }
        else
        {
            base.gameObject.transform.FindChild("NormalOp").gameObject.SetActive(true);
            base.gameObject.transform.FindChild("ChairmanOP").gameObject.SetActive(false);
            this.CurObj = base.gameObject.transform.FindChild("NormalOp").gameObject;
        }
    }

    public void UpdateData(GuildMember _data)
    {
        this.CurMember = _data;
        GuildCommonFunc.UpdateGuildMemberData(this.CurObj.transform.FindChild("GuildMemberItem").gameObject, _data.userInfo);
    }
}

