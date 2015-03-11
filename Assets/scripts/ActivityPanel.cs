using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;

public class ActivityPanel : GUIEntity
{
    private LifeSkillType _SkillType;
    private UIButton bt_try;
    public UISprite BufferAdd;
    private UISprite bufferType;
    private UILabel CollectLabel;
    public UISprite firendAdd;
    private UIGrid HeroGrid;
    public UITableManager<HeroItem> heroTable = new UITableManager<HeroItem>();
    public UITableManager<RewardItem> ItemTable = new UITableManager<RewardItem>();
    private float lastUpdateTime;
    private UILabel lb_buffer;
    private UILabel lb_buffer_times;
    private UILabel lb_friend_buffer;
    private UILabel lb_friend_times;
    private UILabel lb_produce;
    private UILabel lb_timeLeft;
    private UILabel lb_tryTimes;
    private DateTime? NextCollectTime;
    public System.Action OnClose;
    public Action<List<int>> OnSelectHero;
    public System.Action OnTry;
    private UITexture s_exp;
    private UISprite s_gold;
    private DateTime startTime;

    private int NeedVipLevel(int pos)
    {
        ArrayList list = ConfigMgr.getInstance().getList<vip_config>();
        for (int i = 0; i < list.Count; i++)
        {
            vip_config _config = list[i] as vip_config;
            if (_config.lifeskill_card_num >= (pos + 1))
            {
                return _config.entry;
            }
        }
        return 0;
    }

    public void OnHeroClick(HeroItem item)
    {
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.FindChild<UIButton>("bt_Close").OnUIMouseClick(delegate (object o) {
            if (this.OnClose != null)
            {
                this.OnClose();
            }
        });
        this.bt_try = base.FindChild<UIButton>("bt_try");
        this.bt_try.Text(ConfigMgr.getInstance().GetWord(0x2c2c));
        this.bt_try.OnUIMouseClick(delegate (object o) {
            if (this.OnTry != null)
            {
                this.OnTry();
            }
        });
        this.bufferType = base.FindChild<UISprite>("bufferType");
        this.lb_timeLeft = base.FindChild<UILabel>("lb_timeLeft");
        this.s_gold = base.FindChild<UISprite>("s_gold");
        this.lb_produce = base.FindChild<UILabel>("lb_produce");
        this.s_exp = base.FindChild<UITexture>("s_exp");
        this.lb_tryTimes = base.FindChild<UILabel>("lb_tryTimes");
        this.firendAdd = base.FindChild<UISprite>("firendAdd");
        this.BufferAdd = base.FindChild<UISprite>("BufferAdd");
        this.HeroGrid = base.FindChild<UIGrid>("HeroGrid");
        this.lb_buffer = base.FindChild<UILabel>("lb_buffer");
        this.lb_friend_buffer = base.FindChild<UILabel>("lb_friend_buffer");
        this.lb_friend_times = base.FindChild<UILabel>("lb_friend_times");
        this.lb_buffer_times = base.FindChild<UILabel>("lb_buffer_times");
        this.CollectLabel = base.FindChild<UILabel>("CollectLabel");
        this.heroTable.InitFromGrid(this.HeroGrid);
        this.heroTable.Count = 3;
        this.ItemTable.InitFromGrid(base.FindChild<UIGrid>("ShowItem"));
        this.ItemTable.Count = 0;
        for (int i = 0; i < this.heroTable.Count; i++)
        {
            HeroItem item = this.heroTable[i];
            item.Click = new Action<HeroItem>(this.OnHeroClick);
            item.SetCard(-1);
            item.SetVip(this.NeedVipLevel(i) + 1, ActorData.getInstance().VipType + 1);
        }
        this.firendAdd.gameObject.SetActive(false);
        this.BufferAdd.gameObject.SetActive(false);
        this.NextCollectTime = null;
        base.FindChild<Transform>("BottomLeft").gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
    }

    internal void ShowData(LifeSkillData data)
    {
        this.ShowEntry(data.cards.ToArray());
    }

    private void ShowEntry(int[] entry)
    {
        int pos = 0;
        IEnumerator<HeroItem> enumerator = this.heroTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                HeroItem current = enumerator.Current;
                current.SetCard(-1);
                current.SetVip(this.NeedVipLevel(pos) + 1, ActorData.getInstance().VipType + 1);
                pos++;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        for (int i = 0; (i < entry.Length) && (i < 3); i++)
        {
            this.heroTable[i].SetCard(entry[i]);
        }
    }

    public LifeSkillType SkillType
    {
        get
        {
            return this._SkillType;
        }
        set
        {
            this._SkillType = value;
            this.bufferType.spriteName = (value != LifeSkillType.LifeSkill_Fish) ? "Ui_Skill_Icon_ck" : "Ui_Skill_Icon_dy";
        }
    }

    public class HeroItem : UITableItem
    {
        private int _star = 1;
        public UITableManager<StarItem> _starTable = new UITableManager<StarItem>();
        private UIGrid _tableGrid;
        private UITexture Add;
        private bool canClick;
        public UISprite CardBg;
        public Action<ActivityPanel.HeroItem> Click;
        private UISprite HeroBackground;
        public UITexture Icon;
        private UISprite QualityBorder;
        private UILabel VLabel;
        private UISprite VSumal;
        private Transform VSumalRoot;

        public override void OnCreate()
        {
            base.Root.OnUIMouseClick(delegate (object o) {
                if (this.canClick && (this.Click != null))
                {
                    this.Click(this);
                }
            });
            this.Icon = base.Root.FindChild<UITexture>("Icon");
            this.CardBg = base.Root.FindChild<UISprite>("CardBg");
            this._tableGrid = base.Root.FindChild<UIGrid>("Grid");
            this._starTable.InitFromGrid(this._tableGrid);
            this.QualityBorder = base.Root.FindChild<UISprite>("QualityBorder");
            this.Add = base.FindChild<UITexture>("Add");
            this.VSumal = base.FindChild<UISprite>("VLevel");
            this.VLabel = base.FindChild<UILabel>("VLabel");
            this.VSumalRoot = base.FindChild<Transform>("VSumalRoot");
            this.HeroBackground = base.FindChild<UISprite>("HeroBackground");
        }

        public void SetCard(int entry)
        {
            if (entry < 0)
            {
                CommonFunc.SetQualityBorder(this.QualityBorder, 0);
                this.Star = 0;
                this.Icon.mainTexture = null;
                this.CardBg.gameObject.SetActive(true);
                this.HeroBackground.color = (Color) new Color32(0x27, 30, 30, 0x7d);
                this.Add.gameObject.SetActive(true);
            }
            else
            {
                this.Add.gameObject.SetActive(false);
                this.HeroBackground.color = (Color) new Color32(0x27, 30, 30, 0xff);
                this.CardBg.gameObject.SetActive(true);
                Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) entry);
                if (cardByEntry != null)
                {
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(entry);
                    if (_config != null)
                    {
                        CommonFunc.SetQualityBorder(this.QualityBorder, cardByEntry.cardInfo.quality);
                        this.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                        this.Star = cardByEntry.cardInfo.starLv;
                    }
                }
            }
        }

        public void SetVip(int v, int lvel)
        {
            this.VSumalRoot.gameObject.SetActive(lvel < v);
            this.VLabel.text = string.Format("开放", new object[0]);
            this.VSumal.spriteName = string.Format("Ui_Main_Icon_{0}", v);
            this.Add.gameObject.SetActive(lvel >= v);
            this.canClick = lvel >= v;
        }

        public int Star
        {
            get
            {
                return this._star;
            }
            set
            {
                this._star = value;
                this._starTable.Count = this._star;
                int num = 40;
                switch (this._star)
                {
                    case 1:
                        num = 40;
                        break;

                    case 2:
                        num = 0x21;
                        break;

                    case 3:
                        num = 0x15;
                        break;

                    case 4:
                        num = 10;
                        break;

                    case 5:
                        num = 0;
                        break;
                }
                this._tableGrid.transform.localPosition = new Vector3((float) num, this._tableGrid.transform.localPosition.y, this._tableGrid.transform.localPosition.z);
            }
        }

        public class StarItem : UITableItem
        {
            public override void OnCreate()
            {
            }
        }
    }

    public class RewardItem : UITableItem
    {
        private UILabel Count;
        private UITexture Icon;

        public override void OnCreate()
        {
            this.Icon = base.FindChild<UITexture>("Icon");
            this.Count = base.FindChild<UILabel>("Count");
        }

        public void SetItem(Item item)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
            if (_config != null)
            {
                this.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                this.Count.text = item.num.ToString();
            }
        }
    }
}

