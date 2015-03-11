using System;
using System.Runtime.CompilerServices;
using Toolbox;

internal class SendForceHello : GUIPanelEntity
{
    private SocialUser _user;

    public override void Initialize()
    {
        base.Initialize();
        this.lb_mtWord.text = ConfigMgr.getInstance().GetWord(0x2c40);
        this.bt_cancel.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.bt_send.OnUIMouseClick(delegate (object u) {
            GUIMgr.Instance.ExitModelGUI(this);
            if (this._user != null)
            {
                SocketMgr.Instance.RequestC2S_QQFriendsInGame_Share(this._user.QQUser.userInfo.id);
            }
        });
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.bt_cancel = base.FindChild<UIButton>("bt_cancel");
        this.bt_send = base.FindChild<UIButton>("bt_send");
        this.lb_mtWord = base.FindChild<UILabel>("lb_mtWord");
        this.lb_message = base.FindChild<UILabel>("lb_message");
        this.Weixin = base.FindChild<UITexture>("Weixin");
        this.qq = base.FindChild<UITexture>("qq");
    }

    public void Show(SocialUser user)
    {
        this._user = user;
        TencentType tencentType = GameDefine.getInstance().GetTencentType();
        this.qq.gameObject.SetActive(tencentType == TencentType.QQ);
        this.Weixin.gameObject.SetActive(tencentType == TencentType.WEIXIN);
    }

    protected UIButton bt_cancel { get; set; }

    protected UIButton bt_send { get; set; }

    protected UILabel lb_message { get; set; }

    protected UILabel lb_mtWord { get; set; }

    protected UITexture qq { get; set; }

    protected UITexture Weixin { get; set; }
}

