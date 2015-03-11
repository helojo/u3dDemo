using FastBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class GuildDupTrenchMap : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache14;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache15;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache16;
    private FastBuf.guilddup_config guilddup_config;
    private List<int> ItemIds = new List<int>();
    public List<TrenchData> Levels = new List<TrenchData>();
    private guilddup_trench_config[] Maps;

    private void Clear()
    {
        foreach (TrenchData data in this.Levels)
        {
            UnityEngine.Object.Destroy(data.Target);
        }
        this.Levels.Clear();
    }

    private void GetDupOutItems(int id)
    {
        this.ItemIds.Clear();
        string distributeItemList = string.Empty;
        FastBuf.guilddup_config _config = ConfigMgr.getInstance().getByEntry<FastBuf.guilddup_config>(id);
        if (_config != null)
        {
            distributeItemList = _config.distributeItemList;
        }
        char[] separator = new char[] { '|' };
        string[] strArray = distributeItemList.Split(separator);
        foreach (string str2 in strArray)
        {
            this.ItemIds.Add(int.Parse(str2));
        }
        this.GetPageToItemsInfo();
    }

    private void GetPageToItemsInfo()
    {
        List<int> list2;
        int key = 0;
        XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic.Clear();
        for (int i = 0; i < this.ItemIds.Count; i++)
        {
            if (XSingleton<GameGuildMgr>.Singleton.perPageItemCnt == 0)
            {
                Debug.LogWarning("PageCnt is Error！ PageCnt：" + XSingleton<GameGuildMgr>.Singleton.perPageItemCnt);
                return;
            }
            key = i / XSingleton<GameGuildMgr>.Singleton.perPageItemCnt;
            int num3 = i % XSingleton<GameGuildMgr>.Singleton.perPageItemCnt;
            if (!XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic.ContainsKey(key))
            {
                List<int> list = new List<int>();
                XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic.Add(key, list);
                if (!XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic[key].Contains(this.ItemIds[i]))
                {
                    XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic[key].Add(this.ItemIds[i]);
                }
                else
                {
                    Debug.LogWarning("ItemIds have same ItemId from config！！！");
                }
            }
            else if (!XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic[key].Contains(this.ItemIds[i]))
            {
                XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic[key].Add(this.ItemIds[i]);
            }
            else
            {
                Debug.LogWarning("ItemIds have same ItemId from config！！！");
            }
        }
        int num4 = 0;
        if (XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic.TryGetValue(num4, out list2))
        {
            XSingleton<GameGuildMgr>.Singleton.CurReqPageItemIDList = list2;
        }
        else
        {
            Debug.LogWarning("ItemsList of curPage is Error！！！+++:::" + 0);
        }
        foreach (KeyValuePair<int, List<int>> pair in XSingleton<GameGuildMgr>.Singleton.PageIdToItemsDic)
        {
        }
    }

    private string GetTimeLimit()
    {
        TimeSpan span = TimeSpan.FromSeconds((double) (XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.guilddup_config.entry).end_time - TimeMgr.Instance.ServerStampTime));
        return (((span.TotalDays < 1.0) ? string.Empty : string.Format(ConfigMgr.getInstance().GetWord(0x2c7c), Mathf.FloorToInt((float) span.TotalDays))) + ((span.Hours <= 0) ? string.Empty : string.Format(ConfigMgr.getInstance().GetWord(0x2c7d), span.Hours)));
    }

    private void GoPage(int p)
    {
        List<FastBuf.guilddup_config> list = ConfigMgr.getInstance().getListResult<FastBuf.guilddup_config>();
        int index = list.IndexOf(this.guilddup_config);
        int num2 = 0;
        if (list.Count > 1)
        {
            num2 = index + p;
            if (num2 < 0)
            {
                num2 = list.Count - 1;
            }
            if (num2 >= list.Count)
            {
                num2 = 0;
            }
            FastBuf.guilddup_config _config = list[num2];
            GuildDupStatusInfo dupStateByEntry = XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(_config.entry);
            if (dupStateByEntry == null)
            {
                TipsDiag.SetText("副本尚未开启！");
            }
            else
            {
                switch ((dupStateByEntry.status + 1))
                {
                    case GuildDupStatusEnum.GuildDupStatus_Close:
                    case GuildDupStatusEnum.GuildDupStatus_Can_Open:
                    case GuildDupStatusEnum.GuildDupStatus_Open:
                        TipsDiag.SetText("副本尚未开启！");
                        return;
                }
                this.ShowTrenchMap(_config, dupStateByEntry);
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        if (<>f__am$cache14 == null)
        {
            <>f__am$cache14 = u => GUIMgr.Instance.PopGUIEntity();
        }
        this.Close.OnUIMouseClick(<>f__am$cache14);
        if (<>f__am$cache15 == null)
        {
            <>f__am$cache15 = delegate (object u) {
            };
        }
        this.bt_apply_reward.OnUIMouseClick(<>f__am$cache15);
        this.bt_reset.OnUIMouseClick(delegate (object u) {
            if (XSingleton<GameGuildMgr>.Singleton.IsLeader)
            {
                MessageBox.ShowMessageBox(string.Format(ConfigMgr.getInstance().GetWord(0x2c84), this.guilddup_config.reset_cost_dupenergy), o => SocketMgr.Instance.RequestC2S_ResetGuildDup(this.guilddup_config.entry), null, false);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c85));
            }
        });
        this.bt_rank.OnUIMouseClick(u => this.ReqPassTimeRank());
        this.bt_damage_rank.OnUIMouseClick(u => this.ReqDamageRank());
        this.bt_rule.OnUIMouseClick(u => this.ReqRuleInfo());
        this.bt_apply_reward.OnUIMouseClick(u => this.ReqRewardItem());
        this.bt_left.OnUIMouseClick(u => this.GoPage(1));
        this.bt_right.OnUIMouseClick(u => this.GoPage(-1));
        this.TrenchItem.gameObject.SetActive(false);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.bt_left = base.FindChild<UIButton>("bt_left");
        this.Close = base.FindChild<UIButton>("Close");
        this.bt_reset = base.FindChild<UIButton>("bt_reset");
        this.TrenchMap = base.FindChild<Transform>("TrenchMap");
        this.Map = base.FindChild<UITexture>("Map");
        this.bt_rule = base.FindChild<UIButton>("bt_rule");
        this.bt_rank = base.FindChild<UIButton>("bt_rank");
        this.MapPos = base.FindChild<Transform>("MapPos");
        this.lb_titleLabel = base.FindChild<UILabel>("lb_titleLabel");
        this.bt_damage_rank = base.FindChild<UIButton>("bt_damage_rank");
        this.bt_apply_reward = base.FindChild<UIButton>("bt_apply_reward");
        this.lb_limit_count = base.FindChild<UILabel>("lb_limit_count");
        this.lb_destription = base.FindChild<UILabel>("lb_destription");
        this.TrenchItem = base.FindChild<UIDragScrollView>("TrenchItem");
        this.bt_right = base.FindChild<UIButton>("bt_right");
        this.lb_engry = base.FindChild<UILabel>("lb_engry");
    }

    private void OnClickNormalTrench(TrenchData trench)
    {
        <OnClickNormalTrench>c__AnonStorey20E storeye = new <OnClickNormalTrench>c__AnonStorey20E {
            trench = trench,
            <>f__this = this
        };
        if (storeye.trench.TrenchInfo == null)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c7f));
        }
        else
        {
            switch ((storeye.trench.TrenchInfo.status + 1))
            {
                case GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Close:
                case GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Open:
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c7e));
                    break;

                case GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Pass:
                {
                    GuildDupStatusInfo dupStateByEntry = XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.guilddup_config.entry);
                    if (dupStateByEntry == null)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c81));
                        return;
                    }
                    if (dupStateByEntry.remain_times > 0)
                    {
                        if (ActorData.getInstance().Level < this.guilddup_config.unlock_lv)
                        {
                            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x2c82), this.guilddup_config.unlock_lv));
                            return;
                        }
                        GUIMgr.Instance.PushGUIEntity<GuildDupBeginBattle>(new Action<GUIEntity>(storeye.<>m__3C5));
                        break;
                    }
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c80));
                    return;
                }
                case (GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Pass | GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Open):
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c83));
                    break;
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
    }

    private void ReqDamageRank()
    {
        int entry = this.guilddup_config.entry;
        SocketMgr.Instance.RequestC2S_GuildDuplicateDamageRank(entry);
    }

    private void ReqPassTimeRank()
    {
        int entry = this.guilddup_config.entry;
        SocketMgr.Instance.RequestC2S_GuildDuplicatePassRank(entry);
    }

    private void ReqRewardItem()
    {
        XSingleton<GameGuildMgr>.Singleton.CurReqDupId = this.guilddup_config.entry;
        this.GetDupOutItems(XSingleton<GameGuildMgr>.Singleton.CurReqDupId);
        SocketMgr.Instance.RequestC2S_GuildDupItemQueueInfo(XSingleton<GameGuildMgr>.Singleton.CurReqPageItemIDList, XSingleton<GameGuildMgr>.Singleton.CurReqDupId);
    }

    private void ReqRuleInfo()
    {
        GuildBattleNewRulePanel panel = GUIMgr.Instance.GetGUIEntity<GuildBattleNewRulePanel>();
        if (panel != null)
        {
            panel.GetRuleInfo();
        }
        else
        {
            if (<>f__am$cache16 == null)
            {
                <>f__am$cache16 = delegate (GUIEntity u) {
                    GuildBattleNewRulePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildBattleNewRulePanel>();
                    if (gUIEntity != null)
                    {
                        gUIEntity.GetRuleInfo();
                    }
                };
            }
            GUIMgr.Instance.DoModelGUI<GuildBattleNewRulePanel>(<>f__am$cache16, null);
        }
    }

    public void SetTrenchMapData(FastBuf.guilddup_config guilddup_config, GuildDupStatusInfo state, S2C_GetGuildDupTrenchInfo result)
    {
        this.ShowTrenchMap(guilddup_config, state);
        this.ShowTrenchState(result);
    }

    public void ShowLast()
    {
        this.ShowTrenchMap(XSingleton<GameGuildMgr>.Singleton.LastGuildDup);
    }

    private void ShowState(List<GuildDupTrenchInfo> list)
    {
        this.lb_engry.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c7a), ActorData.getInstance().mGuildData.dupEnergy);
        GuildDupStatusInfo dupStateByEntry = XSingleton<GameGuildMgr>.Singleton.GetDupStateByEntry(this.guilddup_config.entry);
        this.lb_limit_count.text = string.Format("{0}", (dupStateByEntry != null) ? dupStateByEntry.remain_times : 0);
        this.lb_titleLabel.text = this.guilddup_config.name;
        this.lb_destription.text = string.Format(ConfigMgr.getInstance().GetWord(0x2c7b), this.GetTimeLimit());
        foreach (GuildDupTrenchInfo info2 in list)
        {
            foreach (TrenchData data in this.Levels)
            {
                if (info2.guildDupTrenchEntry == data.Config.entry)
                {
                    data.TrenchInfo = info2;
                    data.ShowState();
                    break;
                }
            }
        }
    }

    public void ShowTrenchMap(GuildDupStatusInfo guildDupStatusInfo)
    {
        FastBuf.guilddup_config _config = ConfigMgr.getInstance().getByEntry<FastBuf.guilddup_config>(guildDupStatusInfo.guildDupEntry);
        this.ShowTrenchMap(_config, guildDupStatusInfo);
        XSingleton<GameGuildMgr>.Singleton.UIOpenType = OpenType.FromTrench;
    }

    internal void ShowTrenchMap(FastBuf.guilddup_config guilddup_config, GuildDupStatusInfo state)
    {
        <ShowTrenchMap>c__AnonStorey20D storeyd = new <ShowTrenchMap>c__AnonStorey20D {
            <>f__this = this
        };
        XSingleton<GameGuildMgr>.Singleton.UIOpenType = OpenType.FromGuild;
        XSingleton<GameGuildMgr>.Singleton.CurReqDupId = guilddup_config.entry;
        this.GetDupOutItems(XSingleton<GameGuildMgr>.Singleton.CurReqDupId);
        SocketMgr.Instance.RequestC2S_GetGuildDupTrenchInfo(guilddup_config.entry);
        XSingleton<GameGuildMgr>.Singleton.LastGuildDup = state;
        this.Clear();
        this.guilddup_config = guilddup_config;
        storeyd.listEntrys = CommonFunc.GetConfigEntry(guilddup_config.trench_entry);
        guilddup_trench_config[] _configArray = ConfigMgr.getInstance().getListResult<guilddup_trench_config>().Where<guilddup_trench_config>(new Func<guilddup_trench_config, bool>(storeyd.<>m__3C3)).ToArray<guilddup_trench_config>();
        this.Maps = _configArray;
        this.Map.mainTexture = BundleMgr.Instance.CreateTrenchMapIcon(guilddup_config.background);
        Transform transform = this.MapPos.FindChild<Transform>(string.Format("{0}", guilddup_config.entry));
        int num = 0;
        foreach (guilddup_trench_config _config in _configArray)
        {
            TrenchData item = new TrenchData {
                Config = _config
            };
            int num3 = num + 1;
            GameObject gameObject = transform.FindChild(num3.ToString()).gameObject;
            GameObject go = UnityEngine.Object.Instantiate(this.TrenchItem.gameObject) as GameObject;
            go.SetActive(true);
            go.transform.parent = gameObject.transform;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            item.Target = go;
            this.Levels.Add(item);
            guilddup_trench_config data = _config;
            GUIDataHolder.setData(go, data);
            go.transform.OnUIMouseClick(new Action<object>(storeyd.<>m__3C4)).userState = item;
            num++;
            item.ShowState();
        }
    }

    internal void ShowTrenchState(S2C_GetGuildDupTrenchInfo result)
    {
        this.ShowState(result.guildTrenchInfo);
    }

    internal void ShowTrenchState(S2C_ResetGuildDup result)
    {
        this.ShowState(result.curDupTrenchInfo);
    }

    protected UIButton bt_apply_reward { get; set; }

    protected UIButton bt_damage_rank { get; set; }

    protected UIButton bt_left { get; set; }

    protected UIButton bt_rank { get; set; }

    protected UIButton bt_reset { get; set; }

    protected UIButton bt_right { get; set; }

    protected UIButton bt_rule { get; set; }

    protected UIButton Close { get; set; }

    protected UILabel lb_destription { get; set; }

    protected UILabel lb_engry { get; set; }

    protected UILabel lb_limit_count { get; set; }

    protected UILabel lb_titleLabel { get; set; }

    protected UITexture Map { get; set; }

    protected Transform MapPos { get; set; }

    protected UIDragScrollView TrenchItem { get; set; }

    protected Transform TrenchMap { get; set; }

    [CompilerGenerated]
    private sealed class <OnClickNormalTrench>c__AnonStorey20E
    {
        internal GuildDupTrenchMap <>f__this;
        internal GuildDupTrenchMap.TrenchData trench;

        internal void <>m__3C5(GUIEntity child)
        {
            (child as GuildDupBeginBattle).BeginBattle(this.<>f__this.guilddup_config, this.trench.Config, this.trench.TrenchInfo);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowTrenchMap>c__AnonStorey20D
    {
        internal GuildDupTrenchMap <>f__this;
        internal List<int> listEntrys;

        internal bool <>m__3C3(guilddup_trench_config t)
        {
            return this.listEntrys.Contains(t.entry);
        }

        internal void <>m__3C4(object u)
        {
            this.<>f__this.OnClickNormalTrench(u as GuildDupTrenchMap.TrenchData);
        }
    }

    public class HandOutItemInfo
    {
        public int entry;
        public bool isInQueue;
        public int leftCnt;
        public int quality;
        public int reqPlayerAllCnt;
        public List<GuildDupDistributeUserInfo> reqPlayerInfoList;
        public int reqRankId;
    }

    public class TrenchData
    {
        private T FindChild<T>(string name) where T: Component
        {
            return this.Target.transform.FindChild<T>(name);
        }

        public void ShowState()
        {
            UITexture texture = this.FindChild<UITexture>("bossTexture");
            UISprite com = this.FindChild<UISprite>("s_addBuffer");
            UISprite sprite2 = this.FindChild<UISprite>("s_close");
            UISprite sprite3 = this.FindChild<UISprite>("s_time");
            UITexture texture2 = this.FindChild<UITexture>("Icon");
            Transform transform = this.FindChild<Transform>("Boss2");
            UILabel label = this.FindChild<UILabel>("lb_bufferValue");
            UILabel label2 = this.FindChild<UILabel>("lb_time");
            UISlider slider = this.FindChild<UISlider>("s_rate");
            UITexture texture3 = this.FindChild<UITexture>("BeKilled");
            UILabel label3 = this.FindChild<UILabel>("lb_rate");
            UITexture texture4 = this.FindChild<UITexture>("bossNumTexture");
            texture.ColorState(ColorStates.TextureGrey);
            texture4.ColorState(ColorStates.TextureGrey);
            texture2.ColorState(ColorStates.TextureGrey);
            texture.ActiveSelfObject(false);
            texture4.ActiveSelfObject(false);
            sprite2.ActiveSelfObject(false);
            com.ActiveSelfObject(false);
            sprite3.ActiveSelfObject(false);
            slider.ActiveSelfObject(false);
            texture3.ActiveSelfObject(false);
            label3.text = string.Empty;
            if (this.Config != null)
            {
                List<monster_config> monster = XSingleton<GameGuildMgr>.Singleton.GetMonster(this.Config.entry);
                transform.ActiveSelfObject((monster != null) && (monster.Count > 1));
                if ((monster != null) && (monster.Count > 1))
                {
                    texture4.mainTexture = BundleMgr.Instance.CreateGuildDupIcon(string.Format("Ui_GonghuiFb_Icon_bossx{0}", monster.Count));
                }
                texture2.mainTexture = BundleMgr.Instance.CreateHeadIcon(this.Config.monster_picture);
                if (this.TrenchInfo == null)
                {
                    texture2.ColorState(ColorStates.Black);
                    sprite2.ActiveSelfObject(true);
                }
                else
                {
                    switch ((this.TrenchInfo.status + 1))
                    {
                        case GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Close:
                        case GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Open:
                            texture2.ColorState(ColorStates.TextureGrey);
                            sprite2.ActiveSelfObject(true);
                            break;

                        case GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Pass:
                        {
                            texture.ColorState(ColorStates.Normal);
                            texture4.ColorState(ColorStates.Normal);
                            texture2.ColorState(ColorStates.Normal);
                            texture.ActiveSelfObject(true);
                            texture4.ActiveSelfObject(true);
                            com.ActiveSelfObject(true);
                            slider.ActiveSelfObject(true);
                            float num = (float) (((double) this.TrenchInfo.damage_amount) / ((double) this.TrenchInfo.total_hp_amount));
                            slider.value = num;
                            label3.text = string.Format("{0:0}%", num * 100f);
                            label.text = string.Format("[00ff00]攻击+{0:0}%", this.TrenchInfo.buffCount);
                            TimeSpan span = TimeSpan.FromSeconds((double) (this.TrenchInfo.end_time - TimeMgr.Instance.ServerStampTime));
                            sprite3.ActiveSelfObject(span.TotalHours > 0.0);
                            string str = "[00ff00]";
                            if (Math.Floor(span.TotalDays) >= 1.0)
                            {
                                str = str + string.Format("{0:0}天", Math.Floor(span.TotalDays));
                            }
                            str = str + string.Format("{0:0}小时", span.Hours);
                            label2.text = str;
                            break;
                        }
                        case (GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Pass | GuildDupTrenchStatusEnum.GuildTrenchStatusEnum_Open):
                            texture3.ActiveSelfObject(true);
                            texture2.ColorState(ColorStates.Black);
                            break;
                    }
                }
            }
        }

        public guilddup_trench_config Config { get; set; }

        public GameObject Target { get; set; }

        public GuildDupTrenchInfo TrenchInfo { get; set; }
    }
}

