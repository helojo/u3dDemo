using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class QQFriendInfoPanel : GUIEntity
{
    private SocialUser _user;
    private UIButton bt_pk;
    private UIButton bt_send;
    private UIButton Close;
    private UILabel DupText;
    private UITexture Icon;
    private List<SocialUser> mUserList;
    private UILabel Name;
    private UITexture QQHead;
    private UISprite QQVip;
    private UISprite QualityBorder;

    [DebuggerHidden]
    private IEnumerator LoadPic(string url, UITexture texture, UITexture gameIcon)
    {
        return new <LoadPic>c__Iterator7D { url = url, texture = texture, gameIcon = gameIcon, <$>url = url, <$>texture = texture, <$>gameIcon = gameIcon };
    }

    private void OnClickPickHongBao(GameObject go)
    {
        if (this._user != null)
        {
            GUIMgr.Instance.DoModelGUI("GoldTreePanel", delegate (GUIEntity s) {
                GoldTreePanel panel = s as GoldTreePanel;
                panel.Depth = 400;
                panel.OpenType = 3;
                panel.SetQQHongBaoStat(this._user, this.mUserList);
            }, null);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        this.bt_send.gameObject.SetActive(false);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.Icon = base.FindChild<UITexture>("Icon");
        this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
        this.QQVip = base.FindChild<UISprite>("QQVip");
        this.Name = base.FindChild<UILabel>("Name");
        this.DupText = base.FindChild<UILabel>("DupText");
        this.bt_send = base.FindChild<UIButton>("bt_send");
        this.bt_pk = base.FindChild<UIButton>("bt_pk");
        this.Close = base.FindChild<UIButton>("Close");
        this.QQHead = base.FindChild<UITexture>("QQHead");
        this.Close.OnUIMouseClick(s => GUIMgr.Instance.ExitModelGUI(this));
        this.bt_send.OnUIMouseClick(delegate (object u) {
            if (this._user != null)
            {
                SocketMgr.Instance.RequestC2S_Cross_GiveFriendPhyForce(this._user.QQUser.userInfo.id);
                GUIMgr.Instance.DoModelGUI<SendForceHello>(delegate (GUIEntity ui) {
                    SendForceHello hello = ui as SendForceHello;
                    hello.Depth = 400;
                    hello.Show(this._user);
                }, null);
            }
        });
    }

    public override void OnUpdateUIData()
    {
        base.OnUpdateUIData();
        this.ShowUser(this._user, null);
    }

    public void ShowUser(SocialUser user, List<SocialUser> userList = null)
    {
        trench_normal_config _config4;
        string name;
        this._user = user;
        this.mUserList = userList;
        int id = 0;
        id = (int) user.QQUser.userInfo.leaderInfo.cardInfo.entry;
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(id);
        if (_config == null)
        {
            Debug.LogWarning("CardCfg Is Null! Entry is " + id);
            return;
        }
        this.Name.text = (user.Plat == null) ? "QQ好友" : user.Plat.SocialName;
        this.Icon.enabled = true;
        this.QQHead.enabled = false;
        Texture texture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        this.Icon.mainTexture = texture;
        this.Icon.mainTexture = texture;
        CommonFunc.SetQualityBorder(this.QualityBorder, user.QQUser.userInfo.leaderInfo.cardInfo.quality);
        if (user.Plat != null)
        {
            this.QQHead.StartCoroutine(this.LoadPic(user.Plat.SamillIconID, this.QQHead, this.Icon));
        }
        this.QQVip.gameObject.SetActive(false);
        Transform transform = base.transform.FindChild("bt_PickHongBao");
        if ((ActorData.getInstance().UserInfo.redpackage_enable && ActorData.getInstance().UserInfo.redpackage_plat_friend_can_draw) && (!user.QQUser.redpackage_isdraw && (user.QQUser.redpackage_num > 0)))
        {
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPickHongBao);
            transform.gameObject.SetActive(true);
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
        if (user.Owner)
        {
            this.bt_pk.gameObject.SetActive(false);
            this.bt_send.gameObject.SetActive(false);
        }
        else
        {
            this.bt_send.gameObject.SetActive(!this._user.QQUser.alreadyGivePhyForceToday);
            this.bt_pk.gameObject.SetActive(false);
        }
        if (user.UserInfo == null)
        {
            this.DupText.text = string.Empty;
            return;
        }
        ArrayList list = ConfigMgr.getInstance().getList<duplicate_config>();
        if (user.UserInfo.Elite <= 0)
        {
            if (user.UserInfo.Normal <= 0)
            {
                this.DupText.text = string.Empty;
                return;
            }
            _config4 = ConfigMgr.getInstance().getByEntry<trench_normal_config>(user.UserInfo.Normal);
            name = string.Empty;
            if (_config4 == null)
            {
                return;
            }
            IEnumerator enumerator2 = list.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    object current = enumerator2.Current;
                    duplicate_config _config5 = current as duplicate_config;
                    char[] separator = new char[] { '|' };
                    if (_config5.normal_entry.Split(separator).Contains<string>(_config4.entry.ToString()))
                    {
                        name = _config5.name;
                        goto Label_03D4;
                    }
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 == null)
                {
                }
                disposable2.Dispose();
            }
            goto Label_03D4;
        }
        trench_elite_config _config2 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(user.UserInfo.Elite);
        string str = string.Empty;
        if (_config2 == null)
        {
            return;
        }
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object obj2 = enumerator.Current;
                duplicate_config _config3 = obj2 as duplicate_config;
                char[] chArray1 = new char[] { '|' };
                if (_config3.elite_entry.Split(chArray1).Contains<string>(_config2.entry.ToString()))
                {
                    str = _config3.name;
                    goto Label_02F6;
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    Label_02F6:
        this.DupText.text = string.Format("精英-{0}-{1}", str, _config2.name);
        return;
    Label_03D4:
        this.DupText.text = string.Format("普通-{0}-{1}", name, _config4.name);
    }

    [CompilerGenerated]
    private sealed class <LoadPic>c__Iterator7D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal UITexture <$>gameIcon;
        internal UITexture <$>texture;
        internal string <$>url;
        internal WWW <ww>__0;
        internal UITexture gameIcon;
        internal UITexture texture;
        internal string url;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<ww>__0 = new WWW(this.url);
                    break;

                case 1:
                    break;

                default:
                    goto Label_00BA;
            }
            if (!this.<ww>__0.isDone)
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (string.IsNullOrEmpty(this.<ww>__0.error))
            {
                if (this.texture != null)
                {
                    this.gameIcon.enabled = false;
                    this.texture.enabled = true;
                    this.texture.mainTexture = this.<ww>__0.texture;
                }
                this.$PC = -1;
            }
        Label_00BA:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

