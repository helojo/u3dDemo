using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class FriendInfoPanel : GUIEntity
{
    private BriefUser mBriefUser;
    private List<Friend> mHongBaoFriendList;

    private void OnClickAddBtn(GameObject go)
    {
        if (this.mBriefUser != null)
        {
            SocketMgr.Instance.RequestAddFriend(this.mBriefUser.id);
        }
    }

    private void OnClickAgreeBtn(GameObject go)
    {
        if (this.mBriefUser != null)
        {
            SocketMgr.Instance.RequestAgreeFriend(this.mBriefUser.id);
        }
    }

    private void OnClickDelBtn(GameObject go)
    {
        <OnClickDelBtn>c__AnonStorey1E2 storeye = new <OnClickDelBtn>c__AnonStorey1E2();
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storeye.info = obj2 as Friend;
            if (storeye.info != null)
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeye.<>m__301), base.gameObject);
            }
        }
    }

    private void OnClickGetTiLiBtn(GameObject go)
    {
        if (ActorData.getInstance().UserInfo.remainPhyForceAccept <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b3));
        }
        else if (this.mBriefUser != null)
        {
            SocketMgr.Instance.RequestAcceptFriendPhyForce(this.mBriefUser.id);
            base.transform.FindChild("GetTiLiBtn").gameObject.SetActive(false);
        }
    }

    private void OnClickGiftTiLiBtn(GameObject go)
    {
        if (this.mBriefUser != null)
        {
            Friend friend = ActorData.getInstance().FriendList.Find(e => e.userInfo.id == this.mBriefUser.id);
            if (friend != null)
            {
                if (friend.alreadyGivePhyForceToday)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x989687));
                }
                else
                {
                    SocketMgr.Instance.RequestGiveFriendPhyForce(this.mBriefUser.id);
                }
            }
        }
    }

    private void OnClickHongBaoBtn(GameObject go)
    {
        <OnClickHongBaoBtn>c__AnonStorey1E1 storeye = new <OnClickHongBaoBtn>c__AnonStorey1E1 {
            <>f__this = this
        };
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storeye.info = obj2 as Friend;
            if (storeye.info != null)
            {
                GUIMgr.Instance.DoModelGUI("GoldTreePanel", new Action<GUIEntity>(storeye.<>m__2FF), null);
            }
        }
    }

    private void OnClickPkBtn(GameObject go)
    {
        if (this.mBriefUser != null)
        {
            SocketMgr.Instance.RequestGetFriendFormation(this.mBriefUser.id);
        }
    }

    private void OnClickRefuseBtn(GameObject go)
    {
        if (this.mBriefUser != null)
        {
            SocketMgr.Instance.RequestRefuseFriend(this.mBriefUser.id, false);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.transform.FindChild("DelBtn").gameObject.SetActive(false);
        base.transform.FindChild("GetTiLiBtn").gameObject.SetActive(false);
        base.transform.FindChild("GiftTiLiBtn").gameObject.SetActive(false);
        base.transform.FindChild("PkBtn").gameObject.SetActive(false);
        base.transform.FindChild("AddBtn").gameObject.SetActive(false);
        base.transform.FindChild("SendMailBtn").gameObject.SetActive(false);
        base.transform.FindChild("AgreeBtn").gameObject.SetActive(false);
        base.transform.FindChild("RefuseBtn").gameObject.SetActive(false);
    }

    public override void OnDestroy()
    {
        CommonFunc.ShowFuncList(true);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        CommonFunc.ShowFuncList(false);
    }

    public override void OnRelease()
    {
    }

    private void SendMail()
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().mail)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().mail));
        }
        else if (this.mBriefUser != null)
        {
            GUIMgr.Instance.DoModelGUI("MailPanel", delegate (GUIEntity obj) {
                MailPanel panel = (MailPanel) obj;
                panel.Depth = 400;
                panel.SetMailSendTo(this.mBriefUser);
            }, null);
        }
    }

    private void SetBriefUserInfo(BriefUser _data)
    {
        this.mBriefUser = _data;
        if (_data != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_data.head_entry);
            if (_config != null)
            {
                base.transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                user_title_config _config2 = ConfigMgr.getInstance().getByEntry<user_title_config>(_data.titleEntry);
                if (_config2 != null)
                {
                    base.transform.FindChild("ChenHao/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateTitleIcon(_config2.icon.ToString());
                }
                base.transform.FindChild("Job/Icon").GetComponent<UISprite>().spriteName = GameConstant.CardJobIcon[_config.class_type];
                UISprite component = base.transform.FindChild("QualityBorder").GetComponent<UISprite>();
                UISprite sprite3 = base.transform.FindChild("QualityBorder/QIcon").GetComponent<UISprite>();
                CommonFunc.SetPlayerHeadFrame(component, sprite3, _data.head_frame_entry);
                base.transform.FindChild("Sign/Name").GetComponent<UILabel>().text = _data.name;
                base.transform.FindChild("Sign/ID").GetComponent<UILabel>().text = _data.id.ToString();
                base.transform.FindChild("Level").GetComponent<UILabel>().text = _data.level.ToString();
                UILabel label4 = base.transform.FindChild("ChenHao/Label").GetComponent<UILabel>();
                user_title_config _config3 = ConfigMgr.getInstance().getByEntry<user_title_config>(_data.titleEntry);
                if (_config3 != null)
                {
                    label4.text = _config3.name;
                }
                base.transform.FindChild("QianMing/Label").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetMaskWord(_data.signature);
                base.transform.FindChild("Sign/Camp").GetComponent<UISprite>().spriteName = (_data.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
            }
        }
    }

    private void SetDelBtnEvent(Friend _data)
    {
        Transform transform = base.transform.FindChild("DelBtn");
        GUIDataHolder.setData(transform.gameObject, _data);
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickDelBtn);
    }

    public void SetReqFriendInfo(BriefUser _data)
    {
        this.SetBriefUserInfo(_data);
        base.transform.FindChild("DelBtn").gameObject.SetActive(false);
        base.transform.FindChild("GetTiLiBtn").gameObject.SetActive(false);
        base.transform.FindChild("GiftTiLiBtn").gameObject.SetActive(false);
        base.transform.FindChild("PkBtn").gameObject.SetActive(false);
        base.transform.FindChild("AddBtn").gameObject.SetActive(false);
        base.transform.FindChild("SendMailBtn").gameObject.SetActive(false);
        Transform transform = base.transform.FindChild("AgreeBtn");
        transform.gameObject.SetActive(true);
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAgreeBtn);
        Transform transform2 = base.transform.FindChild("RefuseBtn");
        transform2.gameObject.SetActive(true);
        UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRefuseBtn);
    }

    public void SetSearchFriendInfo(BriefUser _data)
    {
        this.SetBriefUserInfo(_data);
        base.transform.FindChild("DelBtn").gameObject.SetActive(false);
        base.transform.FindChild("GetTiLiBtn").gameObject.SetActive(false);
        base.transform.FindChild("GiftTiLiBtn").gameObject.SetActive(false);
        base.transform.FindChild("AgreeBtn").gameObject.SetActive(false);
        base.transform.FindChild("RefuseBtn").gameObject.SetActive(false);
        base.transform.FindChild("PkBtn").gameObject.SetActive(false);
        base.transform.FindChild("SendMailBtn").gameObject.SetActive(false);
        Transform transform = base.transform.FindChild("AddBtn");
        transform.transform.localPosition = new Vector3(15f, -134f, 0f);
        transform.gameObject.SetActive(true);
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAddBtn);
    }

    public void UpdateData(Friend _data, List<Friend> _friendList = null)
    {
        base.Depth = 380;
        this.SetBriefUserInfo(_data.userInfo);
        this.SetDelBtnEvent(_data);
        this.mHongBaoFriendList = _friendList;
        base.transform.FindChild("DelBtn").gameObject.SetActive(true);
        base.transform.FindChild("SendMailBtn").gameObject.SetActive(true);
        base.transform.FindChild("AddBtn").gameObject.SetActive(false);
        Transform transform = base.transform.FindChild("GiftTiLiBtn");
        transform.gameObject.SetActive(true);
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickGiftTiLiBtn);
        Transform transform2 = base.transform.FindChild("HongBaoBtn");
        if (((ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_game_friend_can_draw) && (!_data.redpackage_isdraw && (_data.redpackage_num > 0))) && (ActorData.getInstance().SessionInfo.userid != _data.userInfo.id))
        {
            transform2.gameObject.SetActive(true);
            GUIDataHolder.setData(transform2.gameObject, _data);
            UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickHongBaoBtn);
        }
        else
        {
            transform2.gameObject.SetActive(false);
        }
        Transform transform3 = base.transform.FindChild("PkBtn");
        transform3.gameObject.SetActive(true);
        UIEventListener.Get(transform3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPkBtn);
    }

    public void UpdateHongBaoFriendInfo(long friendid, bool redpackage_isdraw, int redpackage_num)
    {
        <UpdateHongBaoFriendInfo>c__AnonStorey1E0 storeye = new <UpdateHongBaoFriendInfo>c__AnonStorey1E0 {
            friendid = friendid
        };
        if (this.mHongBaoFriendList != null)
        {
            Friend friend = this.mHongBaoFriendList.Find(new Predicate<Friend>(storeye.<>m__2FE));
            if (friend != null)
            {
                friend.redpackage_isdraw = redpackage_isdraw;
                friend.redpackage_num = redpackage_num;
                if (friend.redpackage_num < 0)
                {
                    friend.redpackage_num = 0;
                }
                base.transform.FindChild("HongBaoBtn").gameObject.SetActive(false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickDelBtn>c__AnonStorey1E2
    {
        internal Friend info;

        internal void <>m__301(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            box.MultiLayered = true;
            box.SetDialog(ConfigMgr.getInstance().GetWord(0x98969d), box => SocketMgr.Instance.RequestDeleteFriend(this.info.userInfo.id), null, false);
        }

        internal void <>m__303(GameObject box)
        {
            SocketMgr.Instance.RequestDeleteFriend(this.info.userInfo.id);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickHongBaoBtn>c__AnonStorey1E1
    {
        internal FriendInfoPanel <>f__this;
        internal Friend info;

        internal void <>m__2FF(GUIEntity s)
        {
            GoldTreePanel panel = s as GoldTreePanel;
            panel.Depth = 400;
            panel.OpenType = 2;
            panel.SetHongBaoStat(this.info, this.<>f__this.mHongBaoFriendList);
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateHongBaoFriendInfo>c__AnonStorey1E0
    {
        internal long friendid;

        internal bool <>m__2FE(Friend e)
        {
            return (e.userInfo.id == this.friendid);
        }
    }
}

