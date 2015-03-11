using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class ShopTabEntity : GUIEntity
{
    [CompilerGenerated]
    private static Comparison<vip_config> <>f__am$cacheC;
    [CompilerGenerated]
    private static Comparison<vip_config> <>f__am$cacheD;
    private CourageShopPanel courage_shop;
    private Type Current;
    private ActivityShopPanel goblin_shop;
    private GameObject goblin_tips;
    private bool GoblinISOpen;
    private ActivityShopPanel secret_shop;
    private GameObject secret_tips;
    private bool SecretISOpen;
    private BoxCollider TCourage;
    private BoxCollider TGoblin;
    private BoxCollider TSecret;
    private Type type;

    public void CheckNewTipsStat()
    {
        base.FindChild<Transform>("GoblinNewTips").ActiveSelfObject(ActorData.getInstance().ShowGoblinShopTipsNew);
        base.FindChild<Transform>("SecretNewTips").ActiveSelfObject(ActorData.getInstance().ShowSecretShopTipsNew);
    }

    private void EnableGoblin(bool enable)
    {
        this.GoblinISOpen = enable;
        this.TGoblin.transform.GetComponent<ChangeSprite>().SetEnable(enable);
        foreach (UIToggle toggle in this.TGoblin.GetComponents<UIToggle>())
        {
            toggle.enabled = enable;
        }
    }

    private void EnableSecret(bool enable)
    {
        this.SecretISOpen = enable;
        this.TSecret.transform.GetComponent<ChangeSprite>().SetEnable(enable);
        foreach (UIToggle toggle in this.TSecret.GetComponents<UIToggle>())
        {
            toggle.enabled = enable;
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
        this.CouargeShop.OnShowPage();
        this.GoblinShop.OnShowPage();
        this.SecretShop.OnShowPage();
    }

    public override void OnInitialize()
    {
        this.CouargeShop.OnInitializePage();
        this.GoblinShop.OnInitializePage();
        this.SecretShop.OnInitializePage();
        this.CheckNewTipsStat();
        this.TCourage = base.FindChild<BoxCollider>("TCourage");
        this.TGoblin = base.FindChild<BoxCollider>("TGoblin");
        this.TSecret = base.FindChild<BoxCollider>("TSecret");
        this.TCourage.OnUIMouseClick(delegate (object s) {
            if (this.Current != Type.Courage)
            {
                this.Switch2CourageShop();
            }
        });
        this.TGoblin.OnUIMouseClick(delegate (object s) {
            if (this.Current != Type.Goblin)
            {
                if (!this.GoblinISOpen)
                {
                    vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
                    if (_config != null)
                    {
                        if (_config.can_fix_goblin_shop == 1)
                        {
                            <OnInitialize>c__AnonStorey263 storey = new <OnInitialize>c__AnonStorey263 {
                                cost = _config.fix_goblin_shop_stone
                            };
                            string format = ConfigMgr.getInstance().GetWord(0x2c3f);
                            storey.titlestr = string.Format(format, storey.cost);
                            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__573), base.gameObject);
                        }
                        else
                        {
                            ArrayList list = ConfigMgr.getInstance().getList<vip_config>();
                            List<vip_config> list2 = new List<vip_config>();
                            IEnumerator enumerator = list.GetEnumerator();
                            try
                            {
                                while (enumerator.MoveNext())
                                {
                                    object current = enumerator.Current;
                                    list2.Add(current as vip_config);
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
                            if (<>f__am$cacheC == null)
                            {
                                <>f__am$cacheC = delegate (vip_config l, vip_config r) {
                                    if (l.entry > r.entry)
                                    {
                                        return -1;
                                    }
                                    if (l.entry == r.entry)
                                    {
                                        return 0;
                                    }
                                    return 1;
                                };
                            }
                            list2.Sort(<>f__am$cacheC);
                            int num = 0;
                            foreach (vip_config _config2 in list2)
                            {
                                vip_config _config3 = _config2;
                                if (_config3.can_fix_goblin_shop == 1)
                                {
                                    num = _config3.entry + 1;
                                }
                            }
                            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x2cec), num));
                        }
                    }
                }
                else
                {
                    this.Switch2GoblinShop();
                }
            }
        });
        this.TSecret.OnUIMouseClick(delegate (object s) {
            if (this.Current != Type.Secret)
            {
                if (!this.SecretISOpen)
                {
                    vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
                    if (_config != null)
                    {
                        if (_config.can_fix_secret_shop == 1)
                        {
                            <OnInitialize>c__AnonStorey265 storey = new <OnInitialize>c__AnonStorey265 {
                                cost = _config.fix_secret_shop_stone
                            };
                            string word = ConfigMgr.getInstance().GetWord(0x2c3e);
                            storey.titlestr = string.Format(word, storey.cost);
                            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__575), base.gameObject);
                        }
                        else
                        {
                            ArrayList list = ConfigMgr.getInstance().getList<vip_config>();
                            List<vip_config> list2 = new List<vip_config>();
                            IEnumerator enumerator = list.GetEnumerator();
                            try
                            {
                                while (enumerator.MoveNext())
                                {
                                    object current = enumerator.Current;
                                    list2.Add(current as vip_config);
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
                            if (<>f__am$cacheD == null)
                            {
                                <>f__am$cacheD = delegate (vip_config l, vip_config r) {
                                    if (l.entry > r.entry)
                                    {
                                        return -1;
                                    }
                                    if (l.entry == r.entry)
                                    {
                                        return 0;
                                    }
                                    return 1;
                                };
                            }
                            list2.Sort(<>f__am$cacheD);
                            int num = 0;
                            foreach (vip_config _config2 in list2)
                            {
                                vip_config _config3 = _config2;
                                if (_config3.can_fix_secret_shop == 1)
                                {
                                    num = _config3.entry + 1;
                                }
                            }
                            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x2cec), num));
                        }
                    }
                }
                else
                {
                    this.Switch2SecretShop();
                }
            }
        });
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
    }

    public override void OnUpdate()
    {
        DateTime serverDateTime = TimeMgr.Instance.ServerDateTime;
        if (ActorData.getInstance().secretFixed)
        {
            this.EnableSecret(true);
        }
        else
        {
            DateTime time2 = TimeMgr.Instance.ConvertToDateTime((long) ActorData.getInstance().secretShopOpenTime);
            DateTime time3 = TimeMgr.Instance.ConvertToDateTime((long) (ActorData.getInstance().secretShopDuration + ActorData.getInstance().secretShopOpenTime));
            this.EnableSecret((time2 < serverDateTime) && (time3 > serverDateTime));
        }
        if (ActorData.getInstance().goblinFixed)
        {
            this.EnableGoblin(true);
        }
        else
        {
            DateTime time4 = TimeMgr.Instance.ConvertToDateTime((long) ActorData.getInstance().goblinShopOpenTime);
            DateTime time5 = TimeMgr.Instance.ConvertToDateTime((long) (ActorData.getInstance().goblinShopOpenTime + ActorData.getInstance().goblinShopDuration));
            this.EnableGoblin((serverDateTime < time5) && (serverDateTime > time4));
        }
    }

    public void Switch2CourageShop()
    {
        this.SwitchTabByType(Type.Courage);
    }

    public void Switch2GoblinShop()
    {
        this.SwitchTabByType(Type.Goblin);
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config != null)
        {
            this.goblin_shop.HideFixedBT = !ActorData.getInstance().goblinFixed ? (_config.can_fix_goblin_shop != 1) : true;
        }
    }

    public void Switch2SecretShop()
    {
        this.SwitchTabByType(Type.Secret);
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config != null)
        {
            this.secret_shop.HideFixedBT = !ActorData.getInstance().secretFixed ? (_config.can_fix_secret_shop != 1) : true;
        }
    }

    public void SwitchTabByType(Type type)
    {
        this.Current = type;
        switch (type)
        {
            case Type.Courage:
                this.CouargeShop.gameObject.SetActive(true);
                this.GoblinShop.gameObject.SetActive(false);
                this.SecretShop.gameObject.SetActive(false);
                this.CouargeShop.Refresh();
                break;

            case Type.Goblin:
                this.CouargeShop.gameObject.SetActive(false);
                this.GoblinShop.gameObject.SetActive(true);
                this.GoblinShop.OnShow();
                this.SecretShop.gameObject.SetActive(false);
                SocketMgr.Instance.Request_C2S_GetGoblinShopInfo();
                ActorData.getInstance().TodayOpenGoblinShopTips = true;
                this.CheckNewTipsStat();
                break;

            case Type.Secret:
                this.CouargeShop.gameObject.SetActive(false);
                this.GoblinShop.gameObject.SetActive(false);
                this.SecretShop.gameObject.SetActive(true);
                this.SecretShop.OnShow();
                SocketMgr.Instance.Request_C2S_GetSecretShopInfo();
                ActorData.getInstance().TodayOpenSecretShopTips = true;
                this.CheckNewTipsStat();
                break;
        }
    }

    public CourageShopPanel ActivedCourageShop
    {
        get
        {
            if (!this.CouargeShop.gameObject.activeSelf)
            {
                return null;
            }
            return this.CouargeShop;
        }
    }

    public ActivityShopPanel ActivedGoblinShop
    {
        get
        {
            if (!this.GoblinShop.gameObject.activeSelf)
            {
                return null;
            }
            return this.GoblinShop;
        }
    }

    public ActivityShopPanel ActivedSecretShop
    {
        get
        {
            if (!this.SecretShop.gameObject.activeSelf)
            {
                return null;
            }
            return this.SecretShop;
        }
    }

    private CourageShopPanel CouargeShop
    {
        get
        {
            if (null == this.courage_shop)
            {
                this.courage_shop = base.transform.FindChild("Courage").GetComponent<CourageShopPanel>();
            }
            return this.courage_shop;
        }
    }

    private ActivityShopPanel GoblinShop
    {
        get
        {
            if (null == this.goblin_shop)
            {
                this.goblin_shop = base.transform.FindChild("Goblin").GetComponent<ActivityShopPanel>();
            }
            return this.goblin_shop;
        }
    }

    private ActivityShopPanel SecretShop
    {
        get
        {
            if (null == this.secret_shop)
            {
                this.secret_shop = base.transform.FindChild("Secret").GetComponent<ActivityShopPanel>();
            }
            return this.secret_shop;
        }
    }

    [CompilerGenerated]
    private sealed class <OnInitialize>c__AnonStorey263
    {
        internal int cost;
        internal string titlestr;

        internal void <>m__573(GUIEntity e)
        {
            e.Achieve<MessageBox>().SetDialog(this.titlestr, delegate (GameObject _go) {
                if (ActorData.getInstance().Stone < this.cost)
                {
                    <OnInitialize>c__AnonStorey264 storey = new <OnInitialize>c__AnonStorey264 {
                        title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                    };
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__578), null);
                }
                else
                {
                    SocketMgr.Instance.Request_C2S_GoblinShopFix();
                }
            }, null, false);
        }

        internal void <>m__577(GameObject _go)
        {
            if (ActorData.getInstance().Stone < this.cost)
            {
                <OnInitialize>c__AnonStorey264 storey = new <OnInitialize>c__AnonStorey264 {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__578), null);
            }
            else
            {
                SocketMgr.Instance.Request_C2S_GoblinShopFix();
            }
        }

        private sealed class <OnInitialize>c__AnonStorey264
        {
            private static UIEventListener.VoidDelegate <>f__am$cache1;
            internal string title;

            internal void <>m__578(GUIEntity e1)
            {
                MessageBox box = e1 as MessageBox;
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = _go1 => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                }
                e1.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
            }

            private static void <>m__579(GameObject _go1)
            {
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnInitialize>c__AnonStorey265
    {
        internal int cost;
        internal string titlestr;

        internal void <>m__575(GUIEntity e)
        {
            e.Achieve<MessageBox>().SetDialog(this.titlestr, delegate (GameObject _go) {
                if (ActorData.getInstance().Stone < this.cost)
                {
                    <OnInitialize>c__AnonStorey266 storey = new <OnInitialize>c__AnonStorey266 {
                        title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                    };
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__57B), null);
                }
                else
                {
                    SocketMgr.Instance.Request_C2S_SecretShopFix();
                }
            }, null, false);
        }

        internal void <>m__57A(GameObject _go)
        {
            if (ActorData.getInstance().Stone < this.cost)
            {
                <OnInitialize>c__AnonStorey266 storey = new <OnInitialize>c__AnonStorey266 {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__57B), null);
            }
            else
            {
                SocketMgr.Instance.Request_C2S_SecretShopFix();
            }
        }

        private sealed class <OnInitialize>c__AnonStorey266
        {
            private static UIEventListener.VoidDelegate <>f__am$cache1;
            internal string title;

            internal void <>m__57B(GUIEntity e1)
            {
                MessageBox box = e1 as MessageBox;
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = _go1 => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                }
                e1.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
            }

            private static void <>m__57C(GameObject _go1)
            {
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
        }
    }

    public enum Type
    {
        Courage,
        Goblin,
        Secret
    }
}

