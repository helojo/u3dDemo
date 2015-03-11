using FastBuf;
using Newbie;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Daily : GUIEntity
{
    private UIGrid _grid;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache7;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache8;
    [CompilerGenerated]
    private static Predicate<UniversialRewardConfig> <>f__am$cache9;
    public bool commit_lock;
    public GameObject DailyItemPrefab;
    public Dictionary<int, dlgt_jump> jump_table = new Dictionary<int, dlgt_jump>();
    private List<UniversialRewardConfig> m_config_list;
    public bool mIsExistShakeGoldQuest;
    private bool mIsHaveCanDrawQuest;
    private const int MONTH_CARD_INDEX = 5;

    private bool ButtonStatus(UniversialRewardConfig data_config, daily_quest_config cfg)
    {
        switch (data_config.flag)
        {
            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_FINISH:
            {
                dlgt_jump _jump = null;
                return this.jump_table.TryGetValue(cfg.type, out _jump);
            }
            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW:
                return true;

            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_HAVE_DRAW:
                return false;
        }
        return false;
    }

    public bool CheckQuestExistsByType(int type)
    {
        for (int i = 0; i != this.m_config_list.Count; i++)
        {
            UniversialRewardConfig config = this.m_config_list[i];
            if (config != null)
            {
                daily_quest_config _config = ConfigMgr.getInstance().getByEntry<daily_quest_config>(config.entry);
                if ((_config != null) && ((_config.type == type) && !this.Finished(config)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private int CompareFunc(UniversialRewardConfig l, UniversialRewardConfig r)
    {
        int num = (l.flag != UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW) ? 0 : 1;
        int num2 = (r.flag != UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW) ? 0 : 1;
        if (num != num2)
        {
            return (num2 - num);
        }
        if (l.entry == 5)
        {
            return 1;
        }
        if (r.entry == 5)
        {
            return -1;
        }
        return (l.entry - r.entry);
    }

    private void FillRewardIconCtrl(UniversialRewardConfig config, daily_quest_config cfg, UITexture tex_icon, UISprite img_quality, GameObject obj_chip)
    {
        if (config.rewardConfig.Count > 0)
        {
            int num;
            obj_chip.SetActive(false);
            FastBuf.SingleUniversialRewardConfig config2 = config.rewardConfig[0];
            AffixType affixType = config2.affixType;
            switch (affixType)
            {
                case AffixType.AffixType_Card:
                {
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(config2.parameter1);
                    if (_config != null)
                    {
                        tex_icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                        CommonFunc.SetQualityBorder(img_quality, _config.quality);
                        num = 0x65;
                        tex_icon.height = num;
                        tex_icon.width = num;
                        return;
                    }
                    return;
                }
                case AffixType.AffixType_Gold:
                    tex_icon.mainTexture = BundleMgr.Instance.CreateTextureObject("GUI/Texture/Ui_Main_Icon_coin");
                    CommonFunc.SetEquipQualityBorder(img_quality, 0, false);
                    num = 90;
                    tex_icon.height = num;
                    tex_icon.width = num;
                    return;

                case AffixType.AffixType_Stone:
                    tex_icon.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
                    CommonFunc.SetEquipQualityBorder(img_quality, 0, false);
                    num = 0x7d;
                    tex_icon.height = num;
                    tex_icon.width = num;
                    return;

                case AffixType.AffixType_Item:
                {
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(config2.parameter1);
                    if (_config2 != null)
                    {
                        if (_config2.type == 3)
                        {
                            tex_icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.icon);
                            obj_chip.SetActive(true);
                        }
                        else
                        {
                            tex_icon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                        }
                        CommonFunc.SetEquipQualityBorder(img_quality, _config2.quality, false);
                        if (_config2.type == 3)
                        {
                            num = 0x62;
                            tex_icon.height = num;
                            tex_icon.width = num;
                        }
                        if (_config2.type == 2)
                        {
                            num = 0x52;
                            tex_icon.height = num;
                            tex_icon.width = num;
                        }
                        if (_config2.type == 3)
                        {
                            num = 0x62;
                            tex_icon.height = num;
                            tex_icon.width = num;
                        }
                        if (_config2.type == 4)
                        {
                            img_quality.width = 0x70;
                            img_quality.height = 110;
                            img_quality.transform.localPosition = new Vector3(0f, -1f, 0f);
                        }
                        return;
                    }
                    return;
                }
                case AffixType.AffixType_Contribute:
                    tex_icon.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Gonghui_Icon_ghjx");
                    CommonFunc.SetEquipQualityBorder(img_quality, 0, false);
                    num = 0x55;
                    tex_icon.height = num;
                    tex_icon.width = num;
                    return;
            }
            if (affixType == AffixType.AffixType_PhyForce)
            {
                tex_icon.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Phyforce");
                CommonFunc.SetEquipQualityBorder(img_quality, 0, false);
                num = 0x7d;
                tex_icon.height = num;
                tex_icon.width = num;
            }
        }
    }

    public bool Finished(UniversialRewardConfig data_config)
    {
        return ((data_config.flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_HAVE_DRAW) || (UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW == data_config.flag));
    }

    private string GenAllRewardDescript(UniversialRewardConfig data_config, daily_quest_config cfg)
    {
        string str = string.Empty;
        foreach (FastBuf.SingleUniversialRewardConfig config in data_config.rewardConfig)
        {
            string str2 = this.GenRewardDescript(config, cfg);
            if (!string.IsNullOrEmpty(str))
            {
                str = str + ",";
            }
            str = str + str2;
        }
        if (cfg.reward_exp <= 0)
        {
            return str;
        }
        if (!string.IsNullOrEmpty(str))
        {
            str = str + ",";
        }
        return (str + ConfigMgr.getInstance().GetWord(800) + "X" + cfg.reward_exp.ToString());
    }

    private string GenButtonStatus(UniversialRewardConfig data_config)
    {
        string word = string.Empty;
        if (data_config.flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW)
        {
            word = ConfigMgr.getInstance().GetWord(280);
            this.mIsHaveCanDrawQuest = true;
            return word;
        }
        return ConfigMgr.getInstance().GetWord(0x119);
    }

    private string GenDailyDescript(UniversialRewardConfig data_config, daily_quest_config cfg)
    {
        if (cfg.type == 1)
        {
            if (data_config.rewardConfig.Count <= 0)
            {
                return data_config.describe;
            }
            FastBuf.SingleUniversialRewardConfig config = data_config.rewardConfig[0];
            return string.Format(ConfigMgr.getInstance().GetWord(860), data_config.parameter + 1, config.parameter2);
        }
        return data_config.describe;
    }

    private string GenProgress(UniversialRewardConfig data_config, daily_quest_config cfg)
    {
        string str = string.Empty;
        if (cfg.parameters <= 0)
        {
            return str;
        }
        switch (data_config.flag)
        {
            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW:
            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_HAVE_DRAW:
                return ConfigMgr.getInstance().GetWord(840);
        }
        string[] textArray1 = new string[] { "(", data_config.parameter.ToString(), "/", Mathf.Max(0, cfg.parameters).ToString(), ")" };
        return string.Concat(textArray1);
    }

    private string GenRewardDescript(FastBuf.SingleUniversialRewardConfig data_config, daily_quest_config cfg)
    {
        string name = string.Empty;
        Debug.Log(data_config.affixType.ToString());
        AffixType affixType = data_config.affixType;
        switch (affixType)
        {
            case AffixType.AffixType_Card:
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(data_config.parameter1);
                if (_config != null)
                {
                    name = _config.name;
                }
                goto Label_010B;
            }
            case AffixType.AffixType_Equip:
                break;

            case AffixType.AffixType_Gold:
                name = ConfigMgr.getInstance().GetWord(0x89);
                goto Label_010B;

            case AffixType.AffixType_Stone:
                name = ConfigMgr.getInstance().GetWord(0x31b);
                goto Label_010B;

            case AffixType.AffixType_PhyForce:
                name = ConfigMgr.getInstance().GetWord(0x7a);
                goto Label_010B;

            default:
                switch (affixType)
                {
                    case AffixType.AffixType_Item:
                        break;

                    case AffixType.AffixType_ArenaLadderScore:
                        goto Label_010B;

                    case AffixType.AffixType_Contribute:
                        name = ConfigMgr.getInstance().GetWord(0x4e46);
                        goto Label_010B;

                    default:
                        goto Label_010B;
                }
                break;
        }
        item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(data_config.parameter1);
        if (_config2 != null)
        {
            name = _config2.name;
        }
    Label_010B:
        if (data_config.parameter2 > 0)
        {
            name = name + "X" + data_config.parameter2.ToString();
        }
        return name;
    }

    private bool GenValidStatus(UniversialRewardConfig data_config, daily_quest_config cfg)
    {
        if (cfg.type != 0)
        {
            return false;
        }
        return (UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW != data_config.flag);
    }

    private void JumpFtr_Areana()
    {
        ArenaLadderPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ArenaLadderPanel>();
        if (null != gUIEntity)
        {
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("ArenaLadderPanel");
        }
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = go => SocketMgr.Instance.RequestArenaLadderInfo();
        }
        GUIMgr.Instance.PushGUIEntity("ArenaLadderPanel", <>f__am$cache7);
    }

    private void JumpFtr_Bag()
    {
        GUIMgr.Instance.PushGUIEntity("BagPanel", null);
    }

    private void JumpFtr_Dungeons()
    {
        DungeonsPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<DungeonsPanel>();
        if (null != gUIEntity)
        {
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("DungeonsPanel");
        }
        GUIMgr.Instance.PushGUIEntity("DungeonsPanel", null);
    }

    private void JumpFtr_EquipLvUp()
    {
        GUIMgr.Instance.PushGUIEntity("HeroPanel", null);
    }

    private void JumpFtr_Friend()
    {
        GUIMgr.Instance.PushGUIEntity("FriendPanel", null);
    }

    private void JumpFtr_FriendAssist()
    {
        this.JumpFtr_NormalDup();
    }

    private void JumpFtr_GivePhyforce()
    {
        TitleBar instance = TitleBar.Instance;
        if (null != instance)
        {
            instance.OpenFriend(null);
        }
    }

    private void JumpFtr_Gold()
    {
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = delegate (GUIEntity s) {
                GoldTreePanel panel = s as GoldTreePanel;
                panel.Depth = 400;
                panel.OpenType = 1;
            };
        }
        GUIMgr.Instance.DoModelGUI("GoldTreePanel", <>f__am$cache8, null);
    }

    private void JumpFtr_Guild()
    {
        this.JumpFtr_GuildDup();
    }

    private void JumpFtr_GuildDup()
    {
        GuildPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
        if (null != gUIEntity)
        {
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("GuildPanel");
        }
        MainUI activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MainUI>();
        if (null != activityGUIEntity)
        {
            activityGUIEntity.UpdateGuild();
        }
    }

    private void JumpFtr_HeroDup()
    {
        DupMap gUIEntity = GUIMgr.Instance.GetGUIEntity<DupMap>();
        if (null != gUIEntity)
        {
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("DupMap");
        }
        GUIMgr.Instance.PushGUIEntity("DupMap", null);
    }

    private void JumpFtr_NormalDup()
    {
        DupMap gUIEntity = GUIMgr.Instance.GetGUIEntity<DupMap>();
        if (null != gUIEntity)
        {
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("DupMap");
        }
        GUIMgr.Instance.PushGUIEntity("DupMap", null);
    }

    private void JumpFtr_Outland()
    {
        ActorData.getInstance().isPreOutlandFight = false;
        if (ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().outland_beginner)
        {
            ActorData.getInstance().bOpenOutlandTitleInfo = true;
            SocketMgr.Instance.RequestOutlandsData(null, null);
        }
        else
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().outland_beginner));
        }
    }

    private void JumpFtr_Recruit()
    {
        RecruitPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<RecruitPanel>();
        if (null != gUIEntity)
        {
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("RecruitPanel");
        }
        GUIMgr.Instance.PushGUIEntity("RecruitPanel", null);
    }

    private void JumpFtr_SkillLvUp()
    {
        GUIMgr.Instance.PushGUIEntity("HeroPanel", null);
    }

    private void JumpFtr_Tower()
    {
        TowerPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<TowerPanel>();
        if (null != gUIEntity)
        {
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("TowerPanel");
        }
        GUIMgr.Instance.PushGUIEntity("TowerPanel", null);
    }

    private void JumpFtr_Vip()
    {
        GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
    }

    private void JumpFtr_Yuanzheng()
    {
        YuanZhengPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<YuanZhengPanel>();
        if (null != gUIEntity)
        {
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("YuanZhengPanel");
        }
        GUIMgr.Instance.PushGUIEntity("YuanZhengPanel", null);
    }

    public void OnCommit(UniversialReward reward)
    {
    }

    private void OnCommitEvent(GameObject go)
    {
        if (GuideSystem.MatchEvent(GuideEvent.Duplicate_Daily))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Daily.tag_daily_press_commit_button, go);
        }
        if (!this.commit_lock)
        {
            DailyBuffer component = go.GetComponent<DailyBuffer>();
            SocketMgr.Instance.CommitDailyQuest(component.config.entry);
            this.commit_lock = true;
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.ShowTitleBar(true);
        }
        SocketMgr.Instance.RequestDailyQuest();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.jump_table.Add(4, new dlgt_jump(this.JumpFtr_Recruit));
        this.jump_table.Add(5, new dlgt_jump(this.JumpFtr_NormalDup));
        this.jump_table.Add(6, new dlgt_jump(this.JumpFtr_HeroDup));
        this.jump_table.Add(7, new dlgt_jump(this.JumpFtr_SkillLvUp));
        this.jump_table.Add(8, new dlgt_jump(this.JumpFtr_EquipLvUp));
        this.jump_table.Add(9, new dlgt_jump(this.JumpFtr_Dungeons));
        this.jump_table.Add(10, new dlgt_jump(this.JumpFtr_Areana));
        this.jump_table.Add(11, new dlgt_jump(this.JumpFtr_Tower));
        this.jump_table.Add(13, new dlgt_jump(this.JumpFtr_Yuanzheng));
        this.jump_table.Add(14, new dlgt_jump(this.JumpFtr_Outland));
        this.jump_table.Add(0x10, new dlgt_jump(this.JumpFtr_Vip));
        this.jump_table.Add(0x11, new dlgt_jump(this.JumpFtr_Gold));
        this.jump_table.Add(0x12, new dlgt_jump(this.JumpFtr_GivePhyforce));
        this.jump_table.Add(0x13, new dlgt_jump(this.JumpFtr_FriendAssist));
        this.jump_table.Add(20, new dlgt_jump(this.JumpFtr_GuildDup));
        this.jump_table.Add(0x15, new dlgt_jump(this.JumpFtr_Guild));
        this.jump_table.Add(0x16, new dlgt_jump(this.JumpFtr_Bag));
    }

    private void OnJumpEvent(GameObject go)
    {
        DailyBuffer component = go.GetComponent<DailyBuffer>();
        dlgt_jump _jump = null;
        if (this.jump_table.TryGetValue(component.config.type, out _jump))
        {
            _jump();
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
        if (this.mIsHaveCanDrawQuest)
        {
            this.mIsHaveCanDrawQuest = false;
            CommonFunc.ResetClippingPanel(base.transform.FindChild("List"));
        }
    }

    public void Refresh(List<UniversialRewardConfig> config_list)
    {
        CommonFunc.DeleteChildItem(this.itemGrid.transform);
        if (<>f__am$cache9 == null)
        {
            <>f__am$cache9 = e => UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_START != e.flag;
        }
        config_list = config_list.FindAll(<>f__am$cache9);
        config_list.Sort(new Comparison<UniversialRewardConfig>(this.CompareFunc));
        this.m_config_list = config_list;
        int count = config_list.Count;
        for (int i = 0; i != count; i++)
        {
            UniversialRewardConfig config = config_list[i];
            if (config != null)
            {
                daily_quest_config cfg = ConfigMgr.getInstance().getByEntry<daily_quest_config>(config.entry);
                if ((cfg != null) && (cfg.user_lv_limit <= ActorData.getInstance().Level))
                {
                    GameObject obj2 = UnityEngine.Object.Instantiate(this.DailyItemPrefab) as GameObject;
                    obj2.transform.parent = this.itemGrid.transform;
                    obj2.transform.localPosition = Vector3.zero;
                    obj2.transform.localScale = Vector3.one;
                    obj2.transform.localRotation = Quaternion.identity;
                    UILabel component = obj2.transform.FindChild("name").GetComponent<UILabel>();
                    UILabel label2 = obj2.transform.FindChild("descript").GetComponent<UILabel>();
                    UILabel label3 = obj2.transform.FindChild("award").GetComponent<UILabel>();
                    UILabel label4 = obj2.transform.FindChild("progress").GetComponent<UILabel>();
                    UILabel label5 = obj2.transform.FindChild("unfinished").GetComponent<UILabel>();
                    UIButton btn = obj2.transform.FindChild("btn").GetComponent<UIButton>();
                    UILabel label6 = btn.transform.FindChild("Label").GetComponent<UILabel>();
                    UISprite sprite = obj2.transform.FindChild("bg").GetComponent<UISprite>();
                    UITexture texture = obj2.transform.FindChild("line").GetComponent<UITexture>();
                    UISprite sprite2 = obj2.transform.FindChild("Award/qulity").GetComponent<UISprite>();
                    UITexture texture2 = obj2.transform.FindChild("Award/texture").GetComponent<UITexture>();
                    UISprite sprite3 = obj2.transform.FindChild("TopBorder").GetComponent<UISprite>();
                    GameObject gameObject = obj2.transform.FindChild("Award/chip").gameObject;
                    UILabel label7 = obj2.transform.FindChild("day").GetComponent<UILabel>();
                    if (cfg.type == 0x11)
                    {
                        this.mIsExistShakeGoldQuest = true;
                    }
                    component.text = cfg.name;
                    label2.text = this.GenDailyDescript(config, cfg);
                    label3.text = this.GenAllRewardDescript(config, cfg);
                    label4.text = this.GenProgress(config, cfg);
                    label6.text = this.GenButtonStatus(config);
                    label7.text = string.Empty;
                    if (cfg.type == 0x10)
                    {
                        if (this.Finished(config))
                        {
                            label7.text = ConfigMgr.getInstance().GetWord(0x2d5) + string.Format(ConfigMgr.getInstance().GetWord(0x65), config.parameter2);
                        }
                        else
                        {
                            label6.text = ConfigMgr.getInstance().GetWord(0x4e59);
                        }
                    }
                    label5.gameObject.SetActive(this.GenValidStatus(config, cfg));
                    btn.gameObject.SetActive(this.ButtonStatus(config, cfg));
                    sprite.spriteName = !this.Finished(config) ? "Ui_Heroinfo_Bg_05" : "Ui_Chengjiu_Bg_00";
                    if (cfg.entry == 5)
                    {
                        sprite3.color = !this.ButtonStatus(config, cfg) ? ((Color) new Color32(0xdb, 0xd1, 0xbd, 0xff)) : ((Color) new Color32(0xed, 230, 0xd5, 0xff));
                    }
                    else
                    {
                        sprite3.color = !this.Finished(config) ? ((Color) new Color32(0xdb, 0xd1, 0xbd, 0xff)) : ((Color) new Color32(0xed, 230, 0xd5, 0xff));
                    }
                    texture.color = !this.Finished(config) ? ((Color) new Color32(0x65, 0x58, 0x4e, 0xff)) : ((Color) new Color32(70, 0x33, 0x25, 0xff));
                    label6.effectColor = (Color) new Color32(0x58, 0x58, 0x58, 0xff);
                    this.FillRewardIconCtrl(config, cfg, texture2, sprite2, gameObject);
                    this.RegisterButtonEvent(config, cfg, btn);
                }
            }
        }
        this.itemGrid.Reposition();
        if (this.mIsHaveCanDrawQuest)
        {
            CommonFunc.ResetClippingPanel(base.transform.FindChild("List"));
        }
        this.RequestNewbieGeneration(GuideEvent.Duplicate_Daily);
    }

    private void RegisterButtonEvent(UniversialRewardConfig config, daily_quest_config cfg, UIButton btn)
    {
        switch (config.flag)
        {
            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_FINISH:
                UIEventListener.Get(btn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnJumpEvent);
                break;

            case UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW:
                UIEventListener.Get(btn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnCommitEvent);
                break;
        }
        DailyBuffer buffer = btn.gameObject.AddComponent<DailyBuffer>();
        buffer.config = cfg;
        buffer.daily_config = config;
    }

    private void RequestNewbieGeneration(GuideEvent _event)
    {
        if (GuideSystem.MatchEvent(_event))
        {
            int childCount = this.itemGrid.transform.childCount;
            for (int i = 0; i != childCount; i++)
            {
                Transform child = this.itemGrid.transform.GetChild(i);
                if (null != child)
                {
                    Transform transform2 = child.FindChild("btn");
                    if (null != transform2)
                    {
                        DailyBuffer component = transform2.GetComponent<DailyBuffer>();
                        if ((null != component) && (component.daily_config.flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW))
                        {
                            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Daily.tag_daily_press_commit_button, transform2.gameObject);
                            return;
                        }
                    }
                }
            }
            GuideSystem.ActivedGuide.RequestCancel();
        }
    }

    private UIGrid itemGrid
    {
        get
        {
            if (null == this._grid)
            {
                this._grid = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
            }
            return this._grid;
        }
    }

    public delegate void dlgt_jump();
}

