using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GuildMemberCtrlDlag : GUIEntity
{
    private GuildMember CurMember;
    private List<GuildMember> mGuildMemberList;

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

    private void OnClickAddFriendBtn(GameObject go)
    {
        if (this.CurMember.userInfo.id == ActorData.getInstance().SessionInfo.userid)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a2));
        }
        else if (ActorData.getInstance().FriendList.Find(e => e.userInfo.id == this.CurMember.userInfo.id) != null)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a3));
        }
        else
        {
            SocketMgr.Instance.RequestAddFriend(this.CurMember.userInfo.id);
        }
    }

    private void OnClickPickHongBaoBtn(GameObject go)
    {
        if (this.CurMember != null)
        {
            GUIMgr.Instance.DoModelGUI("GoldTreePanel", delegate (GUIEntity s) {
                GoldTreePanel panel = s as GoldTreePanel;
                panel.Depth = 400;
                panel.OpenType = 4;
                panel.SetGuildHongBaoStat(this.CurMember, this.mGuildMemberList);
            }, null);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        Transform transform = base.transform.FindChild("CtrlGroup");
        GameObject gameObject = transform.transform.FindChild("Button1").gameObject;
        GameObject go = transform.transform.FindChild("Button2").gameObject;
        GameObject obj4 = transform.transform.FindChild("Button3").gameObject;
        GameObject obj5 = transform.transform.FindChild("Button4").gameObject;
        UISprite component = base.transform.FindChild("Background").GetComponent<UISprite>();
        if (ActorData.getInstance().mUserGuildMemberData.position == 1)
        {
            go.SetActive(true);
            obj5.SetActive(true);
        }
        else
        {
            go.SetActive(false);
            obj5.SetActive(false);
            component.height = 0x11c;
        }
        UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.SendEMail);
        UIEventListener.Get(go).onClick = new UIEventListener.VoidDelegate(this.ClickGuildTransfer);
        UIEventListener.Get(obj4).onClick = new UIEventListener.VoidDelegate(this.OnClickAddFriendBtn);
        UIEventListener.Get(obj5).onClick = new UIEventListener.VoidDelegate(this.ClickExpelMember);
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

    private void SetMemberInfo(GuildMember _data)
    {
        UITexture component = base.transform.FindChild("Head/Icon").GetComponent<UITexture>();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_data.userInfo.head_entry);
        if (_config != null)
        {
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        base.transform.FindChild("BaseInfo/LastOnlineTime").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(_data.userInfo.lastOnlineTime));
        UILabel label2 = base.transform.FindChild("BaseInfo/GuildJob").GetComponent<UILabel>();
        if (_data.position == 1)
        {
            label2.text = ConfigMgr.getInstance().GetWord(0xa652bb);
        }
        else
        {
            label2.text = ConfigMgr.getInstance().GetWord(0xa652bc);
        }
        UISprite frame = base.transform.FindChild("Head/Qframe").GetComponent<UISprite>();
        UISprite sprite2 = base.transform.FindChild("Head/Qframe/IconSprite").GetComponent<UISprite>();
        CommonFunc.SetPlayerHeadFrame(frame, sprite2, _data.userInfo.head_frame_entry);
        base.transform.FindChild("BaseInfo/Name").GetComponent<UILabel>().text = _data.userInfo.name;
        base.transform.FindChild("BaseInfo/Level").GetComponent<UILabel>().text = "LV." + _data.userInfo.level;
    }

    public void UpdateData(GuildMember _data, List<GuildMember> _GuildMemberList = null)
    {
        this.CurMember = _data;
        this.mGuildMemberList = _GuildMemberList;
        this.SetMemberInfo(_data);
        GameObject gameObject = base.transform.FindChild("PickHongBaoBtn").gameObject;
        if (((ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_guild_friend_can_draw) && (!_data.userInfo.redpackage_isdraw && (ActorData.getInstance().SessionInfo.userid != _data.userInfo.id))) && (_data.userInfo.redpackage_num > 0))
        {
            UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPickHongBaoBtn);
            gameObject.gameObject.SetActive(true);
        }
        else
        {
            gameObject.gameObject.SetActive(false);
        }
    }

    public void UpdateMemberInfo(long memberId)
    {
        if (this.CurMember.userInfo.id == memberId)
        {
            base.transform.FindChild("PickHongBaoBtn").gameObject.SetActive(false);
        }
    }
}

