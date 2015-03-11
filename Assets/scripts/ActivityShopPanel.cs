using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class ActivityShopPanel : MonoBehaviour
{
    private UIButton bt_fixed;
    private UIButton btn_refresh;
    public CalRefreshCost CalCost;
    public CalFixedCost CalFixed;
    public string FixedMessage = "{0}";
    private bool isFixed;
    private float last;
    private UILabel lb_time;
    private int mNextRefreshTime;
    public BuyItem OnBuy;
    public System.Action OnFixed;
    public System.Action OnRefresh;
    private ShowTimeType ShowType;
    public int StopTime;
    private UITableManager<TableShopItem> TableShop = new UITableManager<TableShopItem>();
    private float time;
    private UILabel title;
    private UILabel TopLabel;

    public void DoFixed()
    {
        <DoFixed>c__AnonStorey24C storeyc = new <DoFixed>c__AnonStorey24C {
            <>f__this = this,
            cost = 0
        };
        if (this.CalFixed != null)
        {
            storeyc.cost = this.CalFixed();
        }
        storeyc.titlestr = string.Format(this.FixedMessage, storeyc.cost);
        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyc.<>m__51C), base.gameObject);
    }

    public T FindChild<T>(string name) where T: Component
    {
        return base.transform.FindChild<T>(name);
    }

    public void LateUpdate()
    {
        if (this.last <= Time.time)
        {
            this.last = Time.time + 1f;
            this.ShowRefreshTime();
            if (TimeMgr.Instance.ServerStampTime > this.mNextRefreshTime)
            {
                this.SetNextRefleshTime();
                switch (this.Type)
                {
                    case ShopType.Goblin:
                        SocketMgr.Instance.Request_C2S_GetGoblinShopInfo();
                        Debug.Log("---Request_C2S_GetGoblinShopInfo---");
                        break;

                    case ShopType.Secret:
                        SocketMgr.Instance.Request_C2S_GetSecretShopInfo();
                        Debug.Log("---Request_C2S_GetSecretShopInfo---");
                        break;
                }
            }
            bool goblinFixed = false;
            switch (this.Type)
            {
                case ShopType.Goblin:
                    goblinFixed = ActorData.getInstance().goblinFixed;
                    break;

                case ShopType.Secret:
                    goblinFixed = ActorData.getInstance().secretFixed;
                    break;
            }
            if (goblinFixed)
            {
                this.ShowType = ShowTimeType.NextRefreshTime;
            }
            else if ((this.time + 5f) < Time.time)
            {
                this.time = Time.time;
                this.ShowType = (this.ShowType != ShowTimeType.NextRefreshTime) ? ShowTimeType.NextRefreshTime : ShowTimeType.ShowCloseTime;
            }
        }
    }

    private int NextGobulinShopRefreshTime()
    {
        int num = TimeMgr.Instance.ServerDateTime.Hour * 0xe10;
        ArrayList list = ConfigMgr.getInstance().getList<goblin_shop_time_config>();
        int count = list.Count;
        if (count < 1)
        {
            return -1;
        }
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                goblin_shop_time_config _config = current as goblin_shop_time_config;
                if (num < _config.day_time)
                {
                    return _config.day_time;
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
        return (list[count - 1] as goblin_shop_time_config).day_time;
    }

    private int NextScrectShopRefreshTime()
    {
        TimeSpan span = (TimeSpan) (TimeMgr.Instance.ServerDateTime - TimeMgr.Instance.ServerDateTime.Date);
        double totalSeconds = span.TotalSeconds;
        ArrayList list = ConfigMgr.getInstance().getList<secret_shop_time_config>();
        int count = list.Count;
        if (count < 1)
        {
            return -1;
        }
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                secret_shop_time_config _config = current as secret_shop_time_config;
                if (totalSeconds < _config.day_time)
                {
                    return _config.day_time;
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
        return (list[count - 1] as secret_shop_time_config).day_time;
    }

    public void OnClickItem(TableShopItem item)
    {
        <OnClickItem>c__AnonStorey24D storeyd = new <OnClickItem>c__AnonStorey24D {
            <>f__this = this,
            coinType = ShopCoinType.None
        };
        switch (this.Type)
        {
            case ShopType.Goblin:
                storeyd.coinType = (item.Item.CostType != 1) ? ShopCoinType.GoblinShopStone : ShopCoinType.GoblinShopGold;
                break;

            case ShopType.Secret:
                storeyd.coinType = (item.Item.CostType != 1) ? ShopCoinType.SecretShopStone : ShopCoinType.SecretShopGold;
                break;
        }
        if (storeyd.coinType != ShopCoinType.None)
        {
            storeyd.si = item.Item.Item;
            GUIMgr.Instance.DoModelGUI<BuyEnterPanel>(new Action<GUIEntity>(storeyd.<>m__51D), null);
        }
    }

    public void OnHidePage()
    {
    }

    public void OnInitializePage()
    {
        this.TopLabel = this.FindChild<UILabel>("TopLabel");
        this.bt_fixed = this.FindChild<UIButton>("bt_fixed");
        this.btn_refresh = this.FindChild<UIButton>("btn_refresh");
        this.bt_fixed.Text(ConfigMgr.getInstance().GetWord(0x2c39));
        this.lb_time = this.FindChild<UILabel>("lb_time");
        this.title = this.FindChild<UILabel>("title");
        this.btn_refresh.OnUIMouseClick(delegate (object u) {
            <OnInitializePage>c__AnonStorey24B storeyb = new <OnInitializePage>c__AnonStorey24B {
                <>f__this = this
            };
            int count = 0;
            storeyb.cost = 0;
            if (this.CalCost != null)
            {
                storeyb.cost = this.CalCost(out count);
            }
            string str = "\n(" + string.Format(ConfigMgr.getInstance().GetWord(330), count) + ")";
            storeyb.titlestr = string.Format(ConfigMgr.getInstance().GetWord(910), storeyb.cost) + str;
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyb.<>m__51E), base.gameObject);
        });
        this.bt_fixed.OnUIMouseClick(u => this.DoFixed());
        this.TableShop.InitFromGrid(this.FindChild<UIGrid>("Grid"));
        this.TableShop.Count = 0;
    }

    public void OnShow()
    {
        this.SetNextRefleshTime();
        this.bt_fixed.gameObject.SetActive(false);
    }

    public void OnShowPage()
    {
    }

    private void OpenVipCardPanel()
    {
        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2bc9));
    }

    private void SetNextRefleshTime()
    {
        List<int> list = new List<int>();
        switch (this.Type)
        {
            case ShopType.Goblin:
            {
                ArrayList list2 = ConfigMgr.getInstance().getList<goblin_shop_time_config>();
                for (int j = 0; j < list2.Count; j++)
                {
                    list.Add(((goblin_shop_time_config) list2[j]).day_time);
                }
                break;
            }
            case ShopType.Secret:
            {
                ArrayList list3 = ConfigMgr.getInstance().getList<secret_shop_time_config>();
                for (int k = 0; k < list3.Count; k++)
                {
                    list.Add(((secret_shop_time_config) list3[k]).day_time);
                }
                break;
            }
        }
        if (list.Count < 1)
        {
            list.Add(0x12750);
        }
        int num3 = (int) TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime.Date);
        for (int i = 0; i < list.Count; i++)
        {
            int num5 = list[i];
            if (TimeMgr.Instance.ServerStampTime < (num3 + num5))
            {
                this.mNextRefreshTime = num3 + num5;
                return;
            }
        }
        int num6 = list[0];
        this.mNextRefreshTime = (num3 + 0x15180) + num6;
    }

    public void ShowRefreshTime()
    {
        this.title.text = (this.ShowType != ShowTimeType.NextRefreshTime) ? "距离商店打烊:" : "下次自动刷新时间:";
        int num = 0;
        switch (this.Type)
        {
            case ShopType.Goblin:
            {
                ShowTimeType showType = this.ShowType;
                if (showType == ShowTimeType.NextRefreshTime)
                {
                    num = this.NextGobulinShopRefreshTime();
                }
                else if (showType == ShowTimeType.ShowCloseTime)
                {
                    int num2 = ActorData.getInstance().goblinShopOpenTime + ActorData.getInstance().goblinShopDuration;
                    TimeSpan span = (TimeSpan) (TimeMgr.Instance.ConvertToDateTime((long) num2) - TimeMgr.Instance.ServerDateTime);
                    if (span.TotalSeconds < 0.0)
                    {
                        span = TimeSpan.FromSeconds(0.0);
                    }
                    this.lb_time.text = string.Format("{0:00}:{1:00}:{2:00}", Math.Floor(span.TotalHours), span.Minutes, span.Seconds);
                    break;
                }
                break;
            }
            case ShopType.Secret:
                switch (this.ShowType)
                {
                    case ShowTimeType.NextRefreshTime:
                        num = this.NextScrectShopRefreshTime();
                        break;

                    case ShowTimeType.ShowCloseTime:
                    {
                        TimeSpan span2 = (TimeSpan) (TimeMgr.Instance.ConvertToDateTime((long) (ActorData.getInstance().secretShopOpenTime + ActorData.getInstance().secretShopDuration)) - TimeMgr.Instance.ServerDateTime);
                        if (span2.TotalSeconds < 0.0)
                        {
                            span2 = TimeSpan.FromSeconds(0.0);
                        }
                        this.lb_time.text = string.Format("{0:00}:{1:00}:{2:00}", Math.Floor(span2.TotalHours), span2.Minutes, span2.Seconds);
                        break;
                    }
                }
                break;
        }
        if (this.ShowType == ShowTimeType.NextRefreshTime)
        {
            TimeSpan span3 = (TimeSpan) (TimeMgr.Instance.ServerDateTime - TimeMgr.Instance.ServerDateTime.Date);
            this.lb_time.text = ((span3.TotalSeconds <= num) ? "今日" : "明日") + string.Format("{0:0}:00点", num / 0xe10);
        }
    }

    public void ShowShop(string title, List<ShopDataItem> items)
    {
        this.UpdateData(items);
    }

    internal void UpdateBuyResult(ShopBuyResult shopBuyResult)
    {
        IEnumerator<TableShopItem> enumerator = this.TableShop.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                TableShopItem current = enumerator.Current;
                if (current.Item.Item.entry == shopBuyResult.shopEntry)
                {
                    current.Item.Item.buyCount = shopBuyResult.buyCount;
                }
                current.Item = current.Item;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    public void UpdateData(List<ShopDataItem> items)
    {
        this.TableShop.Count = items.Count;
        for (int i = 0; i < items.Count; i++)
        {
            this.TableShop[i].Item = items[i];
            this.TableShop[i].Click = new Action<TableShopItem>(this.OnClickItem);
        }
    }

    public bool HideFixedBT
    {
        get
        {
            return this.isFixed;
        }
        set
        {
            this.isFixed = value;
            this.bt_fixed.gameObject.SetActive(!value);
        }
    }

    public ShopType Type { get; set; }

    [CompilerGenerated]
    private sealed class <DoFixed>c__AnonStorey24C
    {
        internal ActivityShopPanel <>f__this;
        internal int cost;
        internal string titlestr;

        internal void <>m__51C(GUIEntity e)
        {
            e.Achieve<MessageBox>().SetDialog(this.titlestr, delegate (GameObject _go) {
                if (ActorData.getInstance().UserInfo.stone < this.cost)
                {
                    this.<>f__this.OpenVipCardPanel();
                }
                else if (this.<>f__this.OnFixed != null)
                {
                    this.<>f__this.OnFixed();
                }
            }, null, false);
        }

        internal void <>m__51F(GameObject _go)
        {
            if (ActorData.getInstance().UserInfo.stone < this.cost)
            {
                this.<>f__this.OpenVipCardPanel();
            }
            else if (this.<>f__this.OnFixed != null)
            {
                this.<>f__this.OnFixed();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItem>c__AnonStorey24D
    {
        internal ActivityShopPanel <>f__this;
        internal ShopCoinType coinType;
        internal ShopItem si;

        internal void <>m__51D(GUIEntity guiE)
        {
            ((BuyEnterPanel) guiE).UpateData(this.coinType, this.si, delegate {
                if ((this.coinType == ShopCoinType.GoblinShopGold) || (this.coinType == ShopCoinType.SecretShopGold))
                {
                    if (ActorData.getInstance().Gold < this.si.cost)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                        return;
                    }
                }
                else if (((this.coinType == ShopCoinType.GoblinShopStone) || (this.coinType == ShopCoinType.SecretShopStone)) && (ActorData.getInstance().Stone < this.si.cost))
                {
                    <OnClickItem>c__AnonStorey24E storeye = new <OnClickItem>c__AnonStorey24E {
                        title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                    };
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeye.<>m__521), null);
                    return;
                }
                GUIMgr.Instance.ExitModelGUI<BuyEnterPanel>();
                if (this.<>f__this.OnBuy != null)
                {
                    this.<>f__this.OnBuy(this.si);
                }
            });
        }

        internal void <>m__520()
        {
            if ((this.coinType == ShopCoinType.GoblinShopGold) || (this.coinType == ShopCoinType.SecretShopGold))
            {
                if (ActorData.getInstance().Gold < this.si.cost)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                    return;
                }
            }
            else if (((this.coinType == ShopCoinType.GoblinShopStone) || (this.coinType == ShopCoinType.SecretShopStone)) && (ActorData.getInstance().Stone < this.si.cost))
            {
                <OnClickItem>c__AnonStorey24E storeye = new <OnClickItem>c__AnonStorey24E {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeye.<>m__521), null);
                return;
            }
            GUIMgr.Instance.ExitModelGUI<BuyEnterPanel>();
            if (this.<>f__this.OnBuy != null)
            {
                this.<>f__this.OnBuy(this.si);
            }
        }

        private sealed class <OnClickItem>c__AnonStorey24E
        {
            private static UIEventListener.VoidDelegate <>f__am$cache1;
            internal string title;

            internal void <>m__521(GUIEntity e)
            {
                MessageBox box = e as MessageBox;
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate (GameObject _go) {
                        GUIMgr.Instance.ExitModelGUIImmediate("BuyEnterPanel");
                        GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                    };
                }
                e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
            }

            private static void <>m__522(GameObject _go)
            {
                GUIMgr.Instance.ExitModelGUIImmediate("BuyEnterPanel");
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnInitializePage>c__AnonStorey24B
    {
        internal ActivityShopPanel <>f__this;
        internal int cost;
        internal string titlestr;

        internal void <>m__51E(GUIEntity e)
        {
            e.Achieve<MessageBox>().SetDialog(this.titlestr, delegate (GameObject _go) {
                if (ActorData.getInstance().UserInfo.stone < this.cost)
                {
                    this.<>f__this.OpenVipCardPanel();
                }
                else if (this.<>f__this.OnRefresh != null)
                {
                    this.<>f__this.OnRefresh();
                }
            }, null, false);
        }

        internal void <>m__523(GameObject _go)
        {
            if (ActorData.getInstance().UserInfo.stone < this.cost)
            {
                this.<>f__this.OpenVipCardPanel();
            }
            else if (this.<>f__this.OnRefresh != null)
            {
                this.<>f__this.OnRefresh();
            }
        }
    }

    public delegate bool BuyItem(ShopItem item);

    public delegate int CalFixedCost();

    public delegate int CalRefreshCost(out int count);

    public class ShopDataItem
    {
        public int CostType { get; set; }

        public int EntryId { get; set; }

        public ShopItem Item { get; set; }

        public int Limit { get; set; }
    }

    public enum ShopType
    {
        Goblin,
        Secret
    }

    private enum ShowTimeType
    {
        NextRefreshTime,
        ShowCloseTime
    }

    public class TableShopItem : UITableItem
    {
        private ActivityShopPanel.ShopDataItem _item;
        private Transform bg_f;
        private UIButton bt;
        private UISprite chip;
        public Action<ActivityShopPanel.TableShopItem> Click;
        private UILabel count;
        private UISprite frame;
        private UILabel name;
        private UISprite picon;
        private UILabel price;
        private UITexture texture;
        private Transform Tips;
        private Transform UpTips;

        public override void OnCreate()
        {
            this.name = base.FindChild<UILabel>("name");
            this.frame = base.FindChild<UISprite>("frame");
            this.price = base.FindChild<UILabel>("price");
            this.texture = base.FindChild<UITexture>("texture");
            this.picon = base.FindChild<UISprite>("picon");
            this.chip = base.FindChild<UISprite>("chip");
            this.count = base.FindChild<UILabel>("count");
            this.Tips = base.FindChild<Transform>("Tips");
            this.bt = base.Root.GetComponent<UIButton>();
            this.bg_f = base.Root.FindChild("bg_f");
            this.UpTips = base.Root.FindChild("UpTips");
            base.Root.OnUIMouseClick(delegate (object o) {
                if (this.bt.enabled && (this.Click != null))
                {
                    this.Click(this);
                }
            });
        }

        public ActivityShopPanel.ShopDataItem Item
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(value.EntryId);
                this.name.text = _config.name;
                this.price.text = value.Item.cost.ToString();
                this.count.text = value.Item.stackCount.ToString();
                this.picon.spriteName = (value.CostType != 1) ? "Item_Icon_Stone" : "Item_Icon_Gold";
                this.UpTips.gameObject.SetActive(XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(_config.entry, value.Item.stackCount).Count > 0);
                if (_config != null)
                {
                    if (_config.type == 3)
                    {
                        this.texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.icon);
                    }
                    else
                    {
                        this.texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                    }
                    CommonFunc.SetEquipQualityBorder(this.frame, _config.quality, false);
                    int num = (_config.type != 3) ? 0x6c : 0x7b;
                    this.texture.height = num;
                    this.texture.width = num;
                    this.frame.depth = (_config.type != 3) ? 30 : 11;
                    bool flag = (_config.type == 2) || (_config.type == 3);
                    this.chip.gameObject.SetActive(flag);
                }
                this.bt.enabled = true;
                if ((value.Limit - value.Item.buyCount) <= 0)
                {
                    this.bt.enabled = false;
                    nguiTextureGrey.doChangeEnableGrey(this.texture, true);
                    this.bg_f.gameObject.SetActive(true);
                    this.Tips.gameObject.SetActive(true);
                }
                else
                {
                    nguiTextureGrey.doChangeEnableGrey(this.texture, false);
                    this.bg_f.gameObject.SetActive(false);
                    this.Tips.gameObject.SetActive(false);
                }
            }
        }

        public string Name
        {
            get
            {
                return this.name.text;
            }
        }
    }
}

