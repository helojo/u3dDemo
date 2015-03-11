using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class PaoKuSelectCardPanel : GUIPanelEntity
{
    private guildrun_character_config _guildrunCharacter;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache13;
    [CompilerGenerated]
    private static Func<Card, bool> <>f__am$cache14;
    private List<guildrun_character_config> listsTemp = new List<guildrun_character_config>();
    protected UITableManager<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>> TableMapGrid = new UITableManager<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>>();

    private void Exit()
    {
        GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x186d3), new UIEventListener.VoidDelegate(this.ExitMessageboxOk), null, false), null);
    }

    private void ExitMessageboxOk(GameObject go)
    {
        ActorData.getInstance().isLeavePaoku = true;
        Time.timeScale = 1f;
        ParkourManager._instance.DestoryParkourAsset();
        GameStateMgr.Instance.ChangeState("EXIT_PARKOUR_EVENT");
    }

    private string GetPaokuSaveName()
    {
        object[] objArray1 = new object[] { "PAOKU", TimeMgr.Instance.ServerDateTime.ToString("yyyy-MM-dd"), ActorData.getInstance().SessionInfo.userid, ServerInfo.lastGameServerId.ToString() };
        return string.Concat(objArray1);
    }

    private void InitData()
    {
        ArrayList source = ConfigMgr.getInstance().getList<guildrun_character_config>();
        if (source != null)
        {
            this.listsTemp = source.Cast<guildrun_character_config>().ToList<guildrun_character_config>();
            if (this.listsTemp.Count > 0)
            {
                this.TableMapGrid.Cache = false;
                this.TableMapGrid.Count = this.listsTemp.Count;
                int num = 0;
                foreach (guildrun_character_config _config in this.listsTemp)
                {
                    guildrun_character_config _config2 = _config;
                    UIAutoGenItem<MapGridItemTemplate, MapGridItemModel> item = this.TableMapGrid[num];
                    item.Model.Config = _config2;
                    item.Model.index = num;
                    num++;
                    item.Model.OnClick = new Action<MapGridItemModel>(this.OnItemClick);
                    if (_config2.for_card_entry == 0x13)
                    {
                        if (<>f__am$cache14 == null)
                        {
                            <>f__am$cache14 = c => c.cardInfo.entry == 0x13;
                        }
                        if (ActorData.getInstance().CardList.Any<Card>(<>f__am$cache14))
                        {
                            item.Model.OnClick = new Action<MapGridItemModel>(this.OnItemClick);
                        }
                        else
                        {
                            item.Model.OnClick = null;
                        }
                    }
                }
                this.GuildrunCharacterConfig = this.listsTemp[0];
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        if (<>f__am$cache13 == null)
        {
            <>f__am$cache13 = delegate (object u) {
                if (ActorData.getInstance().paokuMapEntry != -1)
                {
                    GUIMgr.Instance.CloseUniqueGUIEntity("PaoKuSelectCardPanel");
                    GUIMgr.Instance.OpenUniqueGUIEntity("PaokuInPanel", null);
                    ParkourManager._instance.GameStart = true;
                    ParkourManager._instance.StartCoroutine(ParkourManager._instance.cCtrl.GameStart("PK_StartCamera"));
                }
            };
        }
        this.btn_go.OnUIMouseClick(<>f__am$cache13);
        UIEventListener listener1 = UIEventListener.Get(this.btn_closed.gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, go => this.Exit());
        this.tt_anim_1.OnUIMouseClick(delegate (object u) {
            this.tt_anim_1.gameObject.SetActive(false);
            this.tt_anim_2.gameObject.SetActive(true);
            this.guild_group.gameObject.SetActive(false);
        });
        this.tt_anim_2.OnUIMouseClick(delegate (object u) {
            this.tt_anim_1.gameObject.SetActive(false);
            this.tt_anim_2.gameObject.SetActive(false);
            this.guild_group.gameObject.SetActive(true);
        });
        this.guild_group.OnUIMouseClick(delegate (object u) {
            this.tt_anim_1.gameObject.SetActive(false);
            this.tt_anim_2.gameObject.SetActive(false);
            this.guild_group.gameObject.SetActive(false);
            SettingMgr.mInstance.SetCommonString(this.GetPaokuSaveName().ToUpper(), "PARKOUNEW");
        });
        if (string.IsNullOrEmpty(SettingMgr.mInstance.GetCommonString(this.GetPaokuSaveName().ToUpper())))
        {
            this.anim_group.gameObject.SetActive(true);
            this.tt_anim_1.gameObject.SetActive(true);
            this.tt_anim_1.mainTexture = BundleMgr.Instance.CreatePaokuIcon("paokou_anim_1");
            this.tt_anim_2.mainTexture = BundleMgr.Instance.CreatePaokuIcon("paokou_anim_2");
        }
        else
        {
            this.anim_group.gameObject.SetActive(false);
        }
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.anim_group = base.FindChild<UIPanel>("anim_group");
        this.tt_anim_1 = base.FindChild<UITexture>("tt_anim_1");
        this.tt_anim_2 = base.FindChild<UITexture>("tt_anim_2");
        this.guild_group = base.FindChild<Transform>("guild_group");
        this.btn_closed = base.FindChild<UIButton>("btn_closed");
        this.lb_guild_chapter = base.FindChild<UILabel>("lb_guild_chapter");
        this.List = base.FindChild<UIPanel>("List");
        this.MapGrid = base.FindChild<UIGrid>("MapGrid");
        this.tt_sk = base.FindChild<UITexture>("tt_sk");
        this.slider_Foreground = base.FindChild<UISprite>("slider_Foreground");
        this.lb_name = base.FindChild<UILabel>("lb_name");
        this.lb_hp_value = base.FindChild<UILabel>("lb_hp_value");
        this.lb_sk_name = base.FindChild<UILabel>("lb_sk_name");
        this.lb_cd_time = base.FindChild<UILabel>("lb_cd_time");
        this.lb_sk_desc = base.FindChild<UILabel>("lb_sk_desc");
        this.btn_go = base.FindChild<UIButton>("btn_go");
        this.TableMapGrid.InitFromGrid(this.MapGrid);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        this.InitData();
    }

    private void OnItemClick(MapGridItemModel model)
    {
        <OnItemClick>c__AnonStorey20F storeyf = new <OnItemClick>c__AnonStorey20F {
            model = model
        };
        if ((storeyf.model != null) && (storeyf.model.Config != null))
        {
            guildrun_character_config _config = this.listsTemp.Find(new Predicate<guildrun_character_config>(storeyf.<>m__3D2));
            if (_config != null)
            {
                this.GuildrunCharacterConfig = _config;
            }
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
    }

    protected UIPanel anim_group { get; set; }

    protected UIButton btn_closed { get; set; }

    protected UIButton btn_go { get; set; }

    protected Transform guild_group { get; set; }

    public guildrun_character_config GuildrunCharacterConfig
    {
        get
        {
            return this._guildrunCharacter;
        }
        set
        {
            this._guildrunCharacter = value;
            this.lb_name.text = "【" + value.character_id + "】";
            this.lb_hp_value.text = value.character_hp.ToString();
            if (GameConstValues.GUILD_PAOKU_HP_MAX_LEVEL != 0)
            {
                Debug.Log(value.character_hp + "   " + GameConstValues.GUILD_PAOKU_HP_MAX_LEVEL);
                this.slider_Foreground.fillAmount = ((float) value.character_hp) / ((float) GameConstValues.GUILD_PAOKU_HP_MAX_LEVEL);
            }
            this.tt_sk.mainTexture = BundleMgr.Instance.CreateSkillIcon(value.skill_icon);
            this.lb_sk_name.text = value.skill_name.ToLower();
            this.lb_cd_time.text = value.skillcd_time + "秒";
            this.lb_sk_desc.text = value.skill_describe.ToString();
            ActorData.getInstance().paokuCardEntry = value.entry;
            IEnumerator<UIAutoGenItem<MapGridItemTemplate, MapGridItemModel>> enumerator = this.TableMapGrid.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UIAutoGenItem<MapGridItemTemplate, MapGridItemModel> current = enumerator.Current;
                    if (value.for_card_entry == current.Model.Config.for_card_entry)
                    {
                        current.Template.sp_Border_hight.gameObject.SetActive(true);
                        current.Template.sp_dxj_QualityBorder.gameObject.SetActive(false);
                    }
                    else
                    {
                        current.Template.sp_Border_hight.gameObject.SetActive(false);
                        current.Template.sp_dxj_QualityBorder.gameObject.SetActive(true);
                    }
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            ParkourManager._instance.SelectCharacter(value.entry);
        }
    }

    protected UILabel lb_cd_time { get; set; }

    protected UILabel lb_guild_chapter { get; set; }

    protected UILabel lb_hp_value { get; set; }

    protected UILabel lb_name { get; set; }

    protected UILabel lb_sk_desc { get; set; }

    protected UILabel lb_sk_name { get; set; }

    protected UIPanel List { get; set; }

    protected UIGrid MapGrid { get; set; }

    protected UISprite slider_Foreground { get; set; }

    protected UITexture tt_anim_1 { get; set; }

    protected UITexture tt_anim_2 { get; set; }

    protected UITexture tt_sk { get; set; }

    [CompilerGenerated]
    private sealed class <OnItemClick>c__AnonStorey20F
    {
        internal PaoKuSelectCardPanel.MapGridItemModel model;

        internal bool <>m__3D2(guildrun_character_config p)
        {
            return (p.entry == this.model.Config.entry);
        }
    }

    public class MapGridItemModel : TableItemModel<PaoKuSelectCardPanel.MapGridItemTemplate>
    {
        private guildrun_character_config _config;
        [CompilerGenerated]
        private static Func<Card, bool> <>f__am$cache3;
        public int index;
        public Action<PaoKuSelectCardPanel.MapGridItemModel> OnClick;

        public override void Init(PaoKuSelectCardPanel.MapGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            UIEventListener listener1 = UIEventListener.Get(base.Template.tt_dxj.gameObject);
            listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, delegate (GameObject go) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
        }

        public guildrun_character_config Config
        {
            get
            {
                return this._config;
            }
            set
            {
                this._config = value;
                base.Template.tt_dxj.mainTexture = BundleMgr.Instance.CreateHeadIcon(value.character_icon);
                base.Template.sp_dxj_QualityBorder.gameObject.SetActive(true);
                base.Template.sp_Border_hight.gameObject.SetActive(false);
                if (value.for_card_entry == 0x13)
                {
                    if (<>f__am$cache3 == null)
                    {
                        <>f__am$cache3 = c => c.cardInfo.entry == 0x13;
                    }
                    if (ActorData.getInstance().CardList.Any<Card>(<>f__am$cache3))
                    {
                        nguiTextureGrey.doChangeEnableGrey(base.Template.tt_dxj, false);
                    }
                    else
                    {
                        nguiTextureGrey.doChangeEnableGrey(base.Template.tt_dxj, true);
                    }
                }
            }
        }
    }

    public class MapGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.Map = base.FindChild<UIDragScrollView>("Map");
            this.sp_dxj_QualityBorder = base.FindChild<UISprite>("sp_dxj_QualityBorder");
            this.tt_dxj = base.FindChild<UITexture>("tt_dxj");
            this.sp_Border_hight = base.FindChild<UISprite>("sp_Border_hight");
        }

        public UIDragScrollView Map { get; private set; }

        public UISprite sp_Border_hight { get; private set; }

        public UISprite sp_dxj_QualityBorder { get; private set; }

        public UITexture tt_dxj { get; private set; }
    }
}

