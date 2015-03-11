using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class PushNotiTXPanel : GUIPanelEntity
{
    private long beyond = -1L;

    public string GetDupName(int noraml, int elite)
    {
        trench_normal_config _config3;
        string name;
        string str = string.Empty;
        ArrayList list = ConfigMgr.getInstance().getList<duplicate_config>();
        if (elite <= 0)
        {
            if (noraml <= 0)
            {
                return string.Empty;
            }
            _config3 = ConfigMgr.getInstance().getByEntry<trench_normal_config>(noraml);
            name = string.Empty;
            if (_config3 == null)
            {
                return str;
            }
            IEnumerator enumerator2 = list.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    object current = enumerator2.Current;
                    duplicate_config _config4 = current as duplicate_config;
                    char[] separator = new char[] { '|' };
                    if (_config4.normal_entry.Split(separator).Contains<string>(_config3.entry.ToString()))
                    {
                        name = _config4.name;
                        goto Label_0171;
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
            goto Label_0171;
        }
        trench_elite_config _config = ConfigMgr.getInstance().getByEntry<trench_elite_config>(elite);
        string str2 = string.Empty;
        if (_config == null)
        {
            return str;
        }
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object obj2 = enumerator.Current;
                duplicate_config _config2 = obj2 as duplicate_config;
                char[] chArray1 = new char[] { '|' };
                if (_config2.elite_entry.Split(chArray1).Contains<string>(_config.entry.ToString()))
                {
                    str2 = _config2.name;
                    goto Label_00B3;
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
    Label_00B3:
        return string.Format("精英-{0}-{1}", str2, _config.name);
    Label_0171:
        return string.Format("普通-{0}-{1}", name, _config3.name);
    }

    public override void Initialize()
    {
        base.Initialize();
        this.bt_cancel.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.bt_send.OnUIMouseClick(delegate (object u) {
            if (this.beyond > 0L)
            {
                SocketMgr.Instance.RequestC2S_QQFriendsInGame_Beyond_Share(this.beyond);
            }
            GUIMgr.Instance.ExitModelGUI(this);
        });
        this.lb_mtWord.text = ConfigMgr.getInstance().GetWord(0x2c47);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.lb_up = base.FindChild<UILabel>("lb_up");
        this.lb_dwon = base.FindChild<UILabel>("lb_dwon");
        this.UpIcon = base.FindChild<UITexture>("UpIcon");
        this.upName = base.FindChild<UILabel>("upName");
        this.upLevel = base.FindChild<UILabel>("upLevel");
        this.dwonIcon = base.FindChild<UITexture>("dwonIcon");
        this.dwonName = base.FindChild<UILabel>("dwonName");
        this.dwonLevel = base.FindChild<UILabel>("dwonLevel");
        this.bt_cancel = base.FindChild<UIButton>("bt_cancel");
        this.bt_send = base.FindChild<UIButton>("bt_send");
        this.lb_mtWord = base.FindChild<UILabel>("lb_mtWord");
    }

    [DebuggerHidden]
    private IEnumerator LoadPic(string url, UITexture texture)
    {
        return new <LoadPic>c__Iterator9C { url = url, texture = texture, <$>url = url, <$>texture = texture };
    }

    internal void Show(int noraml, int elite, int newRank)
    {
        this.lb_up.text = newRank.ToString();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(ActorData.getInstance().UserInfo.headEntry);
        if (_config != null)
        {
            this.UpIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        this.upName.text = ActorData.getInstance().UserInfo.name;
        List<SocialUser> users = XSingleton<SocialFriend>.Singleton.Users;
        if (users.Count > newRank)
        {
            SocialUser user = users[newRank];
            this.beyond = user.QQUser.userInfo.id;
            this.lb_dwon.text = (newRank + 1).ToString();
            if (user.Plat != null)
            {
                this.dwonName.text = user.Plat.SocialName;
                this.dwonIcon.StartCoroutine(this.LoadPic(user.Plat.SamillIconID, this.dwonIcon));
            }
            if (user.UserInfo != null)
            {
                this.dwonLevel.text = this.GetDupName(user.UserInfo.Normal, user.UserInfo.Elite);
            }
        }
        this.upLevel.text = this.GetDupName(noraml, elite);
    }

    protected UIButton bt_cancel { get; set; }

    protected UIButton bt_send { get; set; }

    protected UITexture dwonIcon { get; set; }

    protected UILabel dwonLevel { get; set; }

    protected UILabel dwonName { get; set; }

    protected UILabel lb_dwon { get; set; }

    protected UILabel lb_mtWord { get; set; }

    protected UILabel lb_up { get; set; }

    protected UITexture UpIcon { get; set; }

    protected UILabel upLevel { get; set; }

    protected UILabel upName { get; set; }

    [CompilerGenerated]
    private sealed class <LoadPic>c__Iterator9C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal UITexture <$>texture;
        internal string <$>url;
        internal WWW <ww>__0;
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
                    goto Label_00A2;
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
                    this.texture.mainTexture = this.<ww>__0.texture;
                }
                this.$PC = -1;
            }
        Label_00A2:
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

