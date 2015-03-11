using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class GuildDupEndBattle : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache13;
    public const string BossDeath = "Ui_GonghuiFb_Bg_jsboss";
    private int dupId__;
    public const string Finish = "Ui_GonghuiFb_Bg_zdwc";
    protected UITableManager<UIAutoGenItem<BossGridItemTemplate, BossGridItemModel>> TableBossGrid = new UITableManager<UIAutoGenItem<BossGridItemTemplate, BossGridItemModel>>();
    protected UITableManager<UIAutoGenItem<ItemGridItemTemplate, ItemGridItemModel>> TableItemGrid = new UITableManager<UIAutoGenItem<ItemGridItemTemplate, ItemGridItemModel>>();

    public override void Initialize()
    {
        base.Initialize();
        this.TableBossGrid.Count = 2;
        this.TableItemGrid.Count = 5;
        if (<>f__am$cache13 == null)
        {
            <>f__am$cache13 = u => GameStateMgr.Instance.ChangeState("GUILDDUP_END_EVENT");
        }
        this.Close.OnUIMouseClick(<>f__am$cache13);
        this.bt_apply.OnUIMouseClick(u => this.ReqRewardItem());
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.Close = base.FindChild<UIButton>("Close");
        this.bt_apply = base.FindChild<UIButton>("bt_apply");
        this.BossGrid = base.FindChild<UIGrid>("BossGrid");
        this.lb_damageLabel = base.FindChild<UILabel>("lb_damageLabel");
        this.lb_damage = base.FindChild<UILabel>("lb_damage");
        this.lb_allDamageLabel = base.FindChild<UILabel>("lb_allDamageLabel");
        this.lb_allDamage = base.FindChild<UILabel>("lb_allDamage");
        this.s_rate = base.FindChild<UISlider>("s_rate");
        this.lb_dupStageRate = base.FindChild<UILabel>("lb_dupStageRate");
        this.lb_goldLabel = base.FindChild<UILabel>("lb_goldLabel");
        this.lb_gold = base.FindChild<UILabel>("lb_gold");
        this.ItemGrid = base.FindChild<UIGrid>("ItemGrid");
        this.lb_Rank = base.FindChild<UILabel>("lb_Rank");
        this.lb_RankCount = base.FindChild<UILabel>("lb_RankCount");
        this.s_title = base.FindChild<UISprite>("s_title");
        this.TableBossGrid.InitFromGrid(this.BossGrid);
        this.TableItemGrid.InitFromGrid(this.ItemGrid);
    }

    private void ReqRewardItem()
    {
        List<int> list;
        int key = 0;
        Debug.LogWarning("GameGuildMgr.Singleton.PageIdToItemsDic:" + XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic.Count);
        if (XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic.TryGetValue(key, out list))
        {
            XSingleton<GameGuildMgr>.Singleton.CurReqPageItemIDList = list;
            Debug.LogWarning("GameGuildMgr.Singleton.CurReqDupId:_" + XSingleton<GameGuildMgr>.Singleton.CurReqDupId);
            foreach (int num2 in list)
            {
                Debug.LogWarning(string.Concat(new object[] { "CurReqPageItemIDList__:", num2, "______", list.Count }));
            }
            SocketMgr.Instance.RequestC2S_GuildDupItemQueueInfo(list, XSingleton<GameGuildMgr>.Singleton.CurReqDupId);
        }
    }

    internal void ShowBattleResult(S2C_GuildDupCombatEnd result)
    {
        this.dupId__ = result.guildDupId;
        this.lb_gold.text = string.Format("{0:N0}", result.addGold);
        this.Monster = XSingleton<GameGuildMgr>.Singleton.GetMonster(result.guildDupTrenchId);
        guilddup_trench_config _config = ConfigMgr.getInstance().getByEntry<guilddup_trench_config>(result.guildDupTrenchId);
        this.lb_allDamage.text = string.Format("{0:N0}", result.total_damage);
        this.lb_damage.text = string.Format("{0:N0}", result.cur_damage);
        this.lb_RankCount.text = string.Format("{0:0}", result.rank);
        double num = ((double) result.curDupTrenchInfo.damage_amount) / ((double) result.curDupTrenchInfo.total_hp_amount);
        this.lb_dupStageRate.text = string.Format("{0:0.00}%", num * 100.0);
        this.s_rate.value = (float) num;
        this.s_title.spriteName = !result.pass ? "Ui_GonghuiFb_Bg_zdwc" : "Ui_GonghuiFb_Bg_jsboss";
        this.bt_apply.ActiveSelfObject(result.pass);
        this.TableItemGrid.Count = result.dropItemList.itemList.Count;
        int num2 = 0;
        IEnumerator<UIAutoGenItem<ItemGridItemTemplate, ItemGridItemModel>> enumerator = this.TableItemGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<ItemGridItemTemplate, ItemGridItemModel> current = enumerator.Current;
                if (result.dropItemList.itemList.Count > num2)
                {
                    ItemProperty property = result.dropItemList.itemList[num2];
                    current.Model.DropItem = property;
                }
                else
                {
                    current.Model.DropItem = null;
                }
                num2++;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        this.TableBossGrid.Count = this.Monster.Count;
        num2 = 0;
        IEnumerator<UIAutoGenItem<BossGridItemTemplate, BossGridItemModel>> enumerator2 = this.TableBossGrid.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                UIAutoGenItem<BossGridItemTemplate, BossGridItemModel> item2 = enumerator2.Current;
                item2.Model.Config = this.Monster[num2];
                num2++;
            }
        }
        finally
        {
            if (enumerator2 == null)
            {
            }
            enumerator2.Dispose();
        }
    }

    protected UIGrid BossGrid { get; set; }

    protected UIButton bt_apply { get; set; }

    protected UIButton Close { get; set; }

    protected UIGrid ItemGrid { get; set; }

    protected UILabel lb_allDamage { get; set; }

    protected UILabel lb_allDamageLabel { get; set; }

    protected UILabel lb_damage { get; set; }

    protected UILabel lb_damageLabel { get; set; }

    protected UILabel lb_dupStageRate { get; set; }

    protected UILabel lb_gold { get; set; }

    protected UILabel lb_goldLabel { get; set; }

    protected UILabel lb_Rank { get; set; }

    protected UILabel lb_RankCount { get; set; }

    public List<monster_config> Monster { get; private set; }

    protected UISlider s_rate { get; set; }

    protected UISprite s_title { get; set; }

    public class BossGridItemModel : TableItemModel<GuildDupEndBattle.BossGridItemTemplate>
    {
        private monster_config _config;

        public override void Init(GuildDupEndBattle.BossGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }

        public monster_config Config
        {
            get
            {
                return this._config;
            }
            set
            {
                this._config = value;
                base.Template.Grid.ActiveSelfObject(false);
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(value.card_entry);
                if (_config != null)
                {
                    base.Template.bossName.text = _config.name;
                    CommonFunc.SetQualityBorder(base.Template.QualityBorder, value.quality);
                    base.Template.Icon1.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                }
            }
        }
    }

    public class BossGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.Grid = base.FindChild<UIGrid>("Grid");
            this.Icon1 = base.FindChild<UITexture>("Icon1");
            this.HeroBackground = base.FindChild<UISprite>("HeroBackground");
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.bossName = base.FindChild<UILabel>("bossName");
        }

        public UILabel bossName { get; private set; }

        public UIGrid Grid { get; private set; }

        public UISprite HeroBackground { get; private set; }

        public UITexture Icon1 { get; private set; }

        public UISprite QualityBorder { get; private set; }
    }

    public class ItemGridItemModel : TableItemModel<GuildDupEndBattle.ItemGridItemTemplate>
    {
        private ItemProperty di;

        public override void Init(GuildDupEndBattle.ItemGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            item.Root.OnUIMousePressOver(delegate (object u) {
                if ((this.IItem != null) && (GUITipManager.Current != null))
                {
                    GUITipManager.Current.DrawItemTip(base.Item.Root.GetInstanceID(), GUITipManager.Current.OffsetPos(base.Item.Root.transform), new Vector3(150f, 120f, 0f), this.IItem.entry);
                }
            });
        }

        public ItemProperty DropItem
        {
            get
            {
                return this.di;
            }
            set
            {
                base.Template.lb_num.text = string.Empty;
                this.di = value;
                if (value != null)
                {
                    this.IItem = ConfigMgr.getInstance().getByEntry<item_config>(value.item_entry);
                    if (this.IItem != null)
                    {
                        base.Template.Emptyframe.gameObject.SetActive(false);
                        base.Template.ItemBgSprite.gameObject.SetActive(false);
                        base.Template.EquipBroder.ActiveSelfObject(true);
                        base.Template.Texture.mainTexture = BundleMgr.Instance.CreateItemIcon(this.IItem.icon);
                        CommonFunc.SetEquipQualityBorder(base.Template.EquipBroder, this.IItem.quality, false);
                        base.Template.IsPart.ActiveSelfObject(this.IItem.type == 2);
                        base.Template.lb_num.text = string.Format("{0}", this.di.item_param);
                    }
                }
                else
                {
                    base.Template.Emptyframe.gameObject.SetActive(true);
                    base.Template.ItemBgSprite.gameObject.SetActive(false);
                    base.Template.Texture.mainTexture = null;
                    base.Template.EquipBroder.ActiveSelfObject(false);
                    base.Template.IsPart.ActiveSelfObject(false);
                }
            }
        }

        public item_config IItem { get; private set; }
    }

    public class ItemGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.ItemBgSprite = base.FindChild<UISprite>("ItemBgSprite");
            this.Emptyframe = base.FindChild<UISprite>("Emptyframe");
            this.Texture = base.FindChild<UITexture>("Texture");
            this.EquipBroder = base.FindChild<UISprite>("EquipBroder");
            this.IsPart = base.FindChild<UISprite>("IsPart");
            this.lb_num = base.FindChild<UILabel>("lb_num");
        }

        public UISprite Emptyframe { get; private set; }

        public UISprite EquipBroder { get; private set; }

        public UISprite IsPart { get; private set; }

        public UISprite ItemBgSprite { get; private set; }

        public UILabel lb_num { get; private set; }

        public UITexture Texture { get; private set; }
    }
}

