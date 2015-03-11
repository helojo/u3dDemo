using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class LifeSkillFriendAndRewardPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache1B;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache1C;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache1D;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache1E;
    public TabTypes current;
    private PageIndex<List<Friend>> FriendPageIndex;
    private PageIndex<List<NewSkillLifeFriendReward>> RewardPageIndex;
    protected UITableManager<UIAutoGenItem<FriendGridItemTemplate, FriendGridItemModel>> TableFriendGrid = new UITableManager<UIAutoGenItem<FriendGridItemTemplate, FriendGridItemModel>>();
    protected UITableManager<UIAutoGenItem<RewardGridItemTemplate, RewardGridItemModel>> TableRewardGrid = new UITableManager<UIAutoGenItem<RewardGridItemTemplate, RewardGridItemModel>>();

    public override void Initialize()
    {
        base.Initialize();
        this.TableFriendGrid.Cache = false;
        this.TableRewardGrid.Cache = false;
        if (<>f__am$cache1B == null)
        {
            <>f__am$cache1B = u => GUIMgr.Instance.PopGUIEntity();
        }
        this.Close.OnUIMouseClick(<>f__am$cache1B);
        if (<>f__am$cache1C == null)
        {
            <>f__am$cache1C = u => SocketMgr.Instance.RequestC2S_NewLifeRecvFriendReward(-1L, -1, true, NewFriendType.EN_USER_FRIEND_GAME);
        }
        this.bt_AllAgreeBtn.OnUIMouseClick(<>f__am$cache1C);
        if (<>f__am$cache1D == null)
        {
            <>f__am$cache1D = u => SocketMgr.Instance.RequestC2S_NewLifeSendAndReceiveTimes(NewSkillLifeFriendOpt.Friend_Life_Hand_Recv, -1L, true);
        }
        this.bt_friendAllAgreeBtn.OnUIMouseClick(<>f__am$cache1D);
        this.to_Friend.OnUIMouseClick(u => this.ShowTap(TabTypes.FRIEND));
        this.to_Reward.OnUIMouseClick(u => this.ShowTap(TabTypes.REWARD));
        this.bt_next.OnUIMouseClick(delegate (object s) {
            int nextPage = 1;
            switch (this.current)
            {
                case TabTypes.REWARD:
                    nextPage = this.RewardPageIndex.NextPage;
                    break;

                case TabTypes.FRIEND:
                    nextPage = this.FriendPageIndex.NextPage;
                    break;
            }
            base.StartCoroutine(this.ShowPage(nextPage));
        });
        this.bt_prv.OnUIMouseClick(delegate (object s) {
            int prePage = 1;
            switch (this.current)
            {
                case TabTypes.REWARD:
                    prePage = this.RewardPageIndex.PrePage;
                    break;

                case TabTypes.FRIEND:
                    prePage = this.FriendPageIndex.PrePage;
                    break;
            }
            base.StartCoroutine(this.ShowPage(prePage));
        });
        this.lb_Times.text = ConfigMgr.getInstance().GetWord(0x2c66);
        this.YaoQingBtn.gameObject.SetActive(false);
        if (<>f__am$cache1E == null)
        {
            <>f__am$cache1E = delegate (object u) {
                string title = ConfigMgr.getInstance().GetWord(0x4e38);
                string word = ConfigMgr.getInstance().GetWord(0x4e39);
                string url = (GameDefine.getInstance().GetTencentType() != TencentType.QQ) ? GameDefine.getInstance().GetShareURL() : GameDefine.getInstance().GetShareURL();
                SharePanel.G_ShareFriend(title, word, url, "GUI/Share/Ui_Share_Icon_yqhy");
                GUIMgr.Instance.ExitModelGUI<PushNotifyPanel>();
            };
        }
        this.YaoQingBtn.OnUIMouseClick(<>f__am$cache1E);
        this.bt_friendAllAgreeBtn.Text(XSingleton<ConfigMgr>.Singleton[0x2c8a]);
        this.bt_AllAgreeBtn.Text(XSingleton<ConfigMgr>.Singleton[0x2c8b]);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.to_Friend = base.FindChild<UIToggle>("to_Friend");
        this.to_Reward = base.FindChild<UIToggle>("to_Reward");
        this.RewardGroup = base.FindChild<Transform>("RewardGroup");
        this.bt_AllAgreeBtn = base.FindChild<UIButton>("bt_AllAgreeBtn");
        this.lst_friend = base.FindChild<UIPanel>("lst_friend");
        this.RewardGrid = base.FindChild<UIGrid>("RewardGrid");
        this.lb_reward_empty = base.FindChild<UILabel>("lb_reward_empty");
        this.lb_maxRewardTimes = base.FindChild<UILabel>("lb_maxRewardTimes");
        this.L_Title = base.FindChild<UILabel>("L_Title");
        this.Close = base.FindChild<UIButton>("Close");
        this.FriendGroup = base.FindChild<Transform>("FriendGroup");
        this.bt_friendAllAgreeBtn = base.FindChild<UIButton>("bt_friendAllAgreeBtn");
        this.lb_Times = base.FindChild<UILabel>("lb_Times");
        this.YaoQingBtn = base.FindChild<UIButton>("YaoQingBtn");
        this.Count = base.FindChild<UILabel>("Count");
        this.lst_reward = base.FindChild<UIPanel>("lst_reward");
        this.FriendGrid = base.FindChild<UIGrid>("FriendGrid");
        this.lb_friend_empty = base.FindChild<UILabel>("lb_friend_empty");
        this.bt_prv = base.FindChild<UIButton>("bt_prv");
        this.lb_pagenum = base.FindChild<UILabel>("lb_pagenum");
        this.bt_next = base.FindChild<UIButton>("bt_next");
        this.TableRewardGrid.InitFromGrid(this.RewardGrid);
        this.TableFriendGrid.InitFromGrid(this.FriendGrid);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
    }

    private void OnItemClick(FriendGridItemModel obj)
    {
        Friend friend = obj.Friend;
        switch (friend.newlifeskill_phy_state)
        {
            case 0:
            case 6:
                SocketMgr.Instance.RequestC2S_NewLifeSendAndReceiveTimes(NewSkillLifeFriendOpt.Friend_Life_Hand_Give, friend.userInfo.id, false);
                break;

            case 2:
            case 3:
                SocketMgr.Instance.RequestC2S_NewLifeSendAndReceiveTimes(NewSkillLifeFriendOpt.Friend_Life_Hand_Recv, friend.userInfo.id, false);
                break;
        }
    }

    private void OnItemWorkClick(FriendGridItemModel obj)
    {
    }

    private void OnRewardBtClick(RewardGridItemModel obj)
    {
        SocketMgr.Instance.RequestC2S_NewLifeRecvFriendReward(obj.Reward.userInfo.id, obj.Reward.reward_index, false, NewFriendType.EN_USER_FRIEND_GAME);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.current == TabTypes.REWARD)
        {
            IEnumerator<UIAutoGenItem<RewardGridItemTemplate, RewardGridItemModel>> enumerator = this.TableRewardGrid.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UIAutoGenItem<RewardGridItemTemplate, RewardGridItemModel> current = enumerator.Current;
                    current.Model.Update();
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
    }

    [DebuggerHidden]
    private IEnumerator ShowPage(int page)
    {
        return new <ShowPage>c__Iterator84 { page = page, <$>page = page, <>f__this = this };
    }

    public void ShowTap(TabTypes tab)
    {
        this.to_Friend.value = tab == TabTypes.FRIEND;
        this.to_Reward.value = tab == TabTypes.REWARD;
        this.FriendGroup.gameObject.SetActive(tab == TabTypes.FRIEND);
        this.RewardGroup.gameObject.SetActive(tab == TabTypes.REWARD);
        if (this.current != tab)
        {
            this.current = tab;
            switch (tab)
            {
                case TabTypes.REWARD:
                    SocketMgr.Instance.RequestC2S_NewLifeFriendReward();
                    this.L_Title.text = ConfigMgr.getInstance().GetWord(0x2c56);
                    break;

                case TabTypes.FRIEND:
                    SocketMgr.Instance.RequestC2S_NewLifeSkillGetFriend(this.Type, NewFriendType.EN_USER_FRIEND_GAME);
                    this.L_Title.text = ConfigMgr.getInstance().GetWord(0x2c55);
                    break;
            }
        }
    }

    private int SortValue(NewSkillFriendState state)
    {
        int num = -1;
        switch (state)
        {
            case NewSkillFriendState.Friend_Life_Hand_State_None:
                return 1;

            case NewSkillFriendState.Friend_Life_Hand_State_Gived:
                return 0;

            case NewSkillFriendState.Friend_Life_Hand_State_RecvAndGive:
                return 10;

            case NewSkillFriendState.Friend_Life_Hand_State_Recv:
                return 11;

            case ((NewSkillFriendState) 4):
            case ((NewSkillFriendState) 5):
                return num;

            case NewSkillFriendState.Friend_Life_Hand_State_CanGiveNotRecv:
                return 9;

            case NewSkillFriendState.Friend_Life_Hand_State_Each_Other:
                return 0;
        }
        return num;
    }

    internal void UpdateFriendList(S2C_NewLifeSkillGetFriend result)
    {
        XSingleton<LifeSkillManager>.Singleton.UpdateRemain(result.remain);
        this.UpdateFriendList(result.friendList, true);
        this.UpdateLimit(result.limit_max - result.recved_num, result.limit_max);
    }

    internal void UpdateFriendList(List<Friend> list, bool reset = true)
    {
        list.Sort(delegate (Friend l, Friend r) {
            NewSkillFriendState state = (NewSkillFriendState) l.newlifeskill_phy_state;
            NewSkillFriendState state2 = (NewSkillFriendState) r.newlifeskill_phy_state;
            int num = this.SortValue(state);
            int num2 = this.SortValue(state2);
            if (num > num2)
            {
                return -1;
            }
            if (num == num2)
            {
                return 0;
            }
            return 1;
        });
        int page = (this.FriendPageIndex != null) ? this.FriendPageIndex.CurrentPage : 1;
        this.FriendPageIndex = new PageIndex<List<Friend>>(list);
        this.FriendPageIndex.CurrentPage = page;
        if (reset)
        {
            base.StartCoroutine(this.ShowPage(page));
        }
        else
        {
            IEnumerator<UIAutoGenItem<FriendGridItemTemplate, FriendGridItemModel>> enumerator = this.TableFriendGrid.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UIAutoGenItem<FriendGridItemTemplate, FriendGridItemModel> current = enumerator.Current;
                    foreach (Friend friend in list)
                    {
                        if (current.Model.Friend.userInfo.id == friend.userInfo.id)
                        {
                            current.Model.Friend = friend;
                        }
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
        }
    }

    public void UpdateLimit(int num, int max)
    {
        this.Count.text = string.Format("{0}/{1}", num, max);
    }

    internal void UpdateRewardList(S2C_NewLifeFriendReward result)
    {
        this.UpdateRewardList(result.reward, true);
    }

    internal void UpdateRewardList(List<NewSkillLifeFriendReward> list, bool reset = true)
    {
        int page = (this.RewardPageIndex != null) ? this.RewardPageIndex.CurrentPage : 1;
        this.RewardPageIndex = new PageIndex<List<NewSkillLifeFriendReward>>(list);
        if (reset)
        {
            base.StartCoroutine(this.ShowPage(page));
        }
        else
        {
            IEnumerator<UIAutoGenItem<RewardGridItemTemplate, RewardGridItemModel>> enumerator = this.TableRewardGrid.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UIAutoGenItem<RewardGridItemTemplate, RewardGridItemModel> current = enumerator.Current;
                    foreach (NewSkillLifeFriendReward reward in list)
                    {
                        if (current.Model.Reward.userInfo.id == reward.userInfo.id)
                        {
                            current.Model.Reward = reward;
                        }
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
        }
    }

    protected UIButton bt_AllAgreeBtn { get; set; }

    protected UIButton bt_friendAllAgreeBtn { get; set; }

    protected UIButton bt_next { get; set; }

    protected UIButton bt_prv { get; set; }

    protected UIButton Close { get; set; }

    protected UILabel Count { get; set; }

    protected UIGrid FriendGrid { get; set; }

    protected Transform FriendGroup { get; set; }

    protected UILabel L_Title { get; set; }

    protected UILabel lb_friend_empty { get; set; }

    protected UILabel lb_maxRewardTimes { get; set; }

    protected UILabel lb_pagenum { get; set; }

    protected UILabel lb_reward_empty { get; set; }

    protected UILabel lb_Times { get; set; }

    protected UIPanel lst_friend { get; set; }

    protected UIPanel lst_reward { get; set; }

    protected UIGrid RewardGrid { get; set; }

    protected Transform RewardGroup { get; set; }

    protected UIToggle to_Friend { get; set; }

    protected UIToggle to_Reward { get; set; }

    public NewLifeSkillType Type { get; set; }

    protected UIButton YaoQingBtn { get; set; }

    [CompilerGenerated]
    private sealed class <ShowPage>c__Iterator84 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>page;
        internal LifeSkillFriendAndRewardPanel <>f__this;
        internal int <i>__1;
        internal int <i>__3;
        internal int <index>__0;
        internal UIAutoGenItem<LifeSkillFriendAndRewardPanel.FriendGridItemTemplate, LifeSkillFriendAndRewardPanel.FriendGridItemModel> <item>__2;
        internal UIAutoGenItem<LifeSkillFriendAndRewardPanel.RewardGridItemTemplate, LifeSkillFriendAndRewardPanel.RewardGridItemModel> <item>__4;
        internal int page;

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
                    this.<index>__0 = 0;
                    switch (this.<>f__this.current)
                    {
                        case LifeSkillFriendAndRewardPanel.TabTypes.REWARD:
                            if (this.<>f__this.RewardPageIndex == null)
                            {
                                goto Label_03D3;
                            }
                            this.<>f__this.RewardPageIndex.CurrentPage = this.page;
                            this.<>f__this.TableRewardGrid.Count = this.<>f__this.RewardPageIndex.PageItemCount;
                            this.<index>__0 = 0;
                            this.<i>__3 = this.<>f__this.RewardPageIndex.Start;
                            while (this.<i>__3 < this.<>f__this.RewardPageIndex.End)
                            {
                                this.<item>__4 = this.<>f__this.TableRewardGrid[this.<index>__0];
                                this.<item>__4.Model.Reward = this.<>f__this.RewardPageIndex.List[this.<i>__3];
                                this.<item>__4.Model.OnItemBtClick = new Action<LifeSkillFriendAndRewardPanel.RewardGridItemModel>(this.<>f__this.OnRewardBtClick);
                                this.<index>__0++;
                                this.<i>__3++;
                            }
                            this.<>f__this.lb_pagenum.text = string.Format("{0}/{1}", this.<>f__this.RewardPageIndex.CurrentPage, this.<>f__this.RewardPageIndex.PageCount);
                            this.<>f__this.lb_reward_empty.gameObject.SetActive(this.<>f__this.TableRewardGrid.Count == 0);
                            this.$current = null;
                            this.$PC = 2;
                            goto Label_03D5;

                        case LifeSkillFriendAndRewardPanel.TabTypes.FRIEND:
                            if (this.<>f__this.FriendPageIndex == null)
                            {
                                goto Label_03D3;
                            }
                            this.<>f__this.FriendPageIndex.CurrentPage = this.page;
                            this.<>f__this.TableFriendGrid.Count = this.<>f__this.FriendPageIndex.PageItemCount;
                            this.<index>__0 = 0;
                            this.<i>__1 = this.<>f__this.FriendPageIndex.Start;
                            while (this.<i>__1 < this.<>f__this.FriendPageIndex.End)
                            {
                                this.<item>__2 = this.<>f__this.TableFriendGrid[this.<index>__0];
                                this.<item>__2.Model.Friend = this.<>f__this.FriendPageIndex.List[this.<i>__1];
                                this.<item>__2.Model.OnItemClick = new Action<LifeSkillFriendAndRewardPanel.FriendGridItemModel>(this.<>f__this.OnItemClick);
                                this.<item>__2.Model.OnItemWorkClick = new Action<LifeSkillFriendAndRewardPanel.FriendGridItemModel>(this.<>f__this.OnItemWorkClick);
                                this.<index>__0++;
                                this.<i>__1++;
                            }
                            this.<>f__this.lb_friend_empty.gameObject.SetActive(this.<>f__this.TableFriendGrid.Count == 0);
                            this.<>f__this.lb_pagenum.text = string.Format("{0}/{1}", this.<>f__this.FriendPageIndex.CurrentPage, this.<>f__this.FriendPageIndex.PageCount);
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_03D5;
                    }
                    break;

                case 1:
                    this.<>f__this.TableFriendGrid.RepositionLayout();
                    this.<>f__this.lst_reward.ResetClip();
                    break;

                case 2:
                    this.<>f__this.TableRewardGrid.RepositionLayout();
                    this.<>f__this.lst_friend.ResetClip();
                    break;

                default:
                    goto Label_03D3;
            }
            this.$PC = -1;
        Label_03D3:
            return false;
        Label_03D5:
            return true;
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

    public class FriendGridItemModel : TableItemModel<LifeSkillFriendAndRewardPanel.FriendGridItemTemplate>
    {
        private FastBuf.Friend _friend;
        public Action<LifeSkillFriendAndRewardPanel.FriendGridItemModel> OnItemClick;
        public Action<LifeSkillFriendAndRewardPanel.FriendGridItemModel> OnItemWorkClick;
        private UITableManager<StartItem> startTable = new UITableManager<StartItem>();

        public override void Init(LifeSkillFriendAndRewardPanel.FriendGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.startTable.InitFromGrid(template.StarGrid);
            base.Template.GiveBtn.OnUIMouseClick(delegate (object u) {
                if (this.OnItemClick != null)
                {
                    this.OnItemClick(this);
                }
            });
            template.bt_work.OnUIMouseClick(delegate (object u) {
                if (this.OnItemWorkClick != null)
                {
                    this.OnItemWorkClick(this);
                }
            });
        }

        public FastBuf.Friend Friend
        {
            get
            {
                return this._friend;
            }
            set
            {
                base.Template.bt_work.gameObject.SetActive(false);
                base.Template.GiveBtn.Disable(false);
                this._friend = value;
                base.Template.Name.text = value.userInfo.name;
                base.Template.Level.text = string.Format("LV.{0}", value.userInfo.level);
                CommonFunc.SetPlayerHeadFrame(base.Template.QualityBorder, base.Template.QIcon, value.userInfo.head_frame_entry);
                switch (this._friend.newlifeskill_phy_state)
                {
                    case 0:
                        base.Template.GiveBtn.Text(ConfigMgr.getInstance().GetWord(0x2c50));
                        break;

                    case 1:
                        base.Template.GiveBtn.Text(ConfigMgr.getInstance().GetWord(0x2c52));
                        base.Template.GiveBtn.Disable(true);
                        break;

                    case 2:
                        base.Template.GiveBtn.Text(ConfigMgr.getInstance().GetWord(0x2c54));
                        break;

                    case 3:
                        base.Template.GiveBtn.Text(ConfigMgr.getInstance().GetWord(0x2c53));
                        break;

                    case 6:
                        base.Template.GiveBtn.Text(ConfigMgr.getInstance().GetWord(0x2c50));
                        break;

                    case 7:
                        base.Template.GiveBtn.Text(ConfigMgr.getInstance().GetWord(0x2c51));
                        base.Template.GiveBtn.Disable(true);
                        break;

                    default:
                        base.Template.GiveBtn.Disable(true);
                        Debug.Log("StateError:" + this._friend.newlifeskill_phy_state);
                        base.Template.GiveBtn.Text(ConfigMgr.getInstance().GetWord(0x2c51));
                        break;
                }
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(this._friend.userInfo.head_entry);
                if (_config != null)
                {
                    base.Template.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    this.startTable.Count = 0;
                }
            }
        }

        public NewLifeSkillType Type { get; set; }

        public class StartItem : UITableItem
        {
            public override void OnCreate()
            {
            }
        }
    }

    public class FriendGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.QIcon = base.FindChild<UISprite>("QIcon");
            this.Name = base.FindChild<UILabel>("Name");
            this.Level = base.FindChild<UILabel>("Level");
            this.Icon = base.FindChild<UITexture>("Icon");
            this.GiveBtn = base.FindChild<UIButton>("GiveBtn");
            this.StarGrid = base.FindChild<UIGrid>("StarGrid");
            this.bt_work = base.FindChild<UIButton>("bt_work");
        }

        public UIButton bt_work { get; private set; }

        public UIButton GiveBtn { get; private set; }

        public UITexture Icon { get; private set; }

        public UILabel Level { get; private set; }

        public UILabel Name { get; private set; }

        public UISprite QIcon { get; private set; }

        public UISprite QualityBorder { get; private set; }

        public UIGrid StarGrid { get; private set; }
    }

    public class RewardGridItemModel : TableItemModel<LifeSkillFriendAndRewardPanel.RewardGridItemTemplate>
    {
        private NewSkillLifeFriendReward _Reward;
        public Action<LifeSkillFriendAndRewardPanel.RewardGridItemModel> OnItemBtClick;
        private UITableManager<StartItem> startTable = new UITableManager<StartItem>();

        public override void Init(LifeSkillFriendAndRewardPanel.RewardGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.startTable.InitFromGrid(template.StarGrid);
            template.PickRewardBtn.OnUIMouseClick(delegate (object u) {
                if (this.OnItemBtClick != null)
                {
                    this.OnItemBtClick(this);
                }
            });
        }

        public void Update()
        {
            if (this.Reward != null)
            {
                DateTime time = TimeMgr.Instance.ConvertToDateTime((long) this._Reward.rece_time);
                TimeSpan span = (TimeSpan) (time - TimeMgr.Instance.ServerDateTime);
                if (span.TotalSeconds > 0.0)
                {
                    base.Template.lb_rewardTime.text = string.Format(XSingleton<ConfigMgr>.Singleton[0x2c65], Math.Floor(span.TotalHours), span.Minutes, span.Seconds);
                }
                else
                {
                    base.Template.lb_rewardTime.text = string.Format(XSingleton<ConfigMgr>.Singleton[0x2c87], time);
                }
            }
        }

        public NewSkillLifeFriendReward Reward
        {
            get
            {
                return this._Reward;
            }
            set
            {
                this._Reward = value;
                DateTime time = TimeMgr.Instance.ConvertToDateTime((long) value.rece_time);
                bool flag = (value.add_gold > 0) || (value.itemid >= 0);
                bool flag2 = time < TimeMgr.Instance.ServerDateTime;
                base.Template.PickRewardBtn.Disable(!(flag2 && flag));
                base.Template.Name.text = value.userInfo.name;
                base.Template.Level.text = string.Format("LV.{0}", value.userInfo.level);
                CommonFunc.SetPlayerHeadFrame(base.Template.QualityBorder, base.Template.QIcon, value.userInfo.head_frame_entry);
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(value.userInfo.head_entry);
                if (_config != null)
                {
                    base.Template.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    this.startTable.Count = 0;
                }
            }
        }

        public class StartItem : UITableItem
        {
            public override void OnCreate()
            {
            }
        }
    }

    public class RewardGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
            this.QIcon = base.FindChild<UISprite>("QIcon");
            this.Name = base.FindChild<UILabel>("Name");
            this.Level = base.FindChild<UILabel>("Level");
            this.Icon = base.FindChild<UITexture>("Icon");
            this.PickRewardBtn = base.FindChild<UIButton>("PickRewardBtn");
            this.StarGrid = base.FindChild<UIGrid>("StarGrid");
            this.lb_rewardTime = base.FindChild<UILabel>("lb_rewardTime");
        }

        public UITexture Icon { get; private set; }

        public UILabel lb_rewardTime { get; private set; }

        public UILabel Level { get; private set; }

        public UILabel Name { get; private set; }

        public UIButton PickRewardBtn { get; private set; }

        public UISprite QIcon { get; private set; }

        public UISprite QualityBorder { get; private set; }

        public UIGrid StarGrid { get; private set; }
    }

    public enum TabTypes
    {
        None,
        REWARD,
        FRIEND
    }
}

