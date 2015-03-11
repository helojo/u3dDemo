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

internal class LifeSkillSelectFriendPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cacheC;
    [CompilerGenerated]
    private static Comparison<Friend> <>f__am$cacheD;
    private PageIndex<List<Friend>> FriendPageIndex;
    protected UITableManager<UIAutoGenItem<FriendGridItemTemplate, FriendGridItemModel>> TableFriendGrid = new UITableManager<UIAutoGenItem<FriendGridItemTemplate, FriendGridItemModel>>();

    public override void Initialize()
    {
        base.Initialize();
        this.TableFriendGrid.Cache = false;
        if (<>f__am$cacheC == null)
        {
            <>f__am$cacheC = u => GUIMgr.Instance.PopGUIEntity();
        }
        this.Close.OnUIMouseClick(<>f__am$cacheC);
        this.bt_next.OnUIMouseClick(delegate (object s) {
            if (this.FriendPageIndex != null)
            {
                base.StartCoroutine(this.ShowPage(this.FriendPageIndex.NextPage));
            }
        });
        this.bt_prv.OnUIMouseClick(delegate (object s) {
            if (this.FriendPageIndex != null)
            {
                base.StartCoroutine(this.ShowPage(this.FriendPageIndex.PrePage));
            }
        });
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.Close = base.FindChild<UIButton>("Close");
        this.FriendGroup = base.FindChild<Transform>("FriendGroup");
        this.lst_view = base.FindChild<UIPanel>("lst_view");
        this.FriendGrid = base.FindChild<UIGrid>("FriendGrid");
        this.lb_empty = base.FindChild<UILabel>("lb_empty");
        this.bt_prv = base.FindChild<UIButton>("bt_prv");
        this.lb_pagenum = base.FindChild<UILabel>("lb_pagenum");
        this.bt_next = base.FindChild<UIButton>("bt_next");
        this.L_Title = base.FindChild<UILabel>("L_Title");
        this.TableFriendGrid.InitFromGrid(this.FriendGrid);
    }

    private void OnItemWorkClick(FriendGridItemModel obj)
    {
        <OnItemWorkClick>c__AnonStorey212 storey = new <OnItemWorkClick>c__AnonStorey212 {
            obj = obj,
            <>f__this = this
        };
        GUIMgr.Instance.PopGUIEntity();
        GUIMgr.Instance.PushGUIEntity<LifeSkillBeginPanel>(new Action<GUIEntity>(storey.<>m__40D));
    }

    [DebuggerHidden]
    private IEnumerator ShowPage(int page)
    {
        return new <ShowPage>c__Iterator85 { page = page, <$>page = page, <>f__this = this };
    }

    public void ShowSelect(NewLifeSkillType type)
    {
        this.Type = type;
        SocketMgr.Instance.RequestC2S_NewLifeSkillGetFriend(this.Type, NewFriendType.EN_USER_FRIEND_GAME);
    }

    internal void UpdateFriendList(S2C_NewLifeSkillGetFriend result)
    {
        XSingleton<LifeSkillManager>.Singleton.UpdateRemain(result.remain);
        this.UpdateFriendList(result.friendList, true);
    }

    internal void UpdateFriendList(List<Friend> list, bool reset = true)
    {
        List<Friend> list2 = list.ToList<Friend>();
        if (<>f__am$cacheD == null)
        {
            <>f__am$cacheD = delegate (Friend l, Friend r) {
                if (l.newlifeskill_employ_time > r.newlifeskill_employ_time)
                {
                    return 1;
                }
                if (l.newlifeskill_employ_time == r.newlifeskill_employ_time)
                {
                    return 0;
                }
                return -1;
            };
        }
        list2.Sort(<>f__am$cacheD);
        if (reset)
        {
            this.FriendPageIndex = new PageIndex<List<Friend>>(list2);
            base.StartCoroutine(this.ShowPage(1));
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

    protected UIButton bt_next { get; set; }

    protected UIButton bt_prv { get; set; }

    protected UIButton Close { get; set; }

    protected UIGrid FriendGrid { get; set; }

    protected Transform FriendGroup { get; set; }

    protected UILabel L_Title { get; set; }

    protected UILabel lb_empty { get; set; }

    protected UILabel lb_pagenum { get; set; }

    protected UIPanel lst_view { get; set; }

    public NewLifeSkillType Type { get; set; }

    [CompilerGenerated]
    private sealed class <OnItemWorkClick>c__AnonStorey212
    {
        internal LifeSkillSelectFriendPanel <>f__this;
        internal LifeSkillSelectFriendPanel.FriendGridItemModel obj;

        internal void <>m__40D(GUIEntity u)
        {
            (u as LifeSkillBeginPanel).ShowLifeSkillEnter(NewSkillCellType.CELL_TYPE4, this.<>f__this.Type, this.obj.Friend.userInfo.id, this.obj.Friend.userInfo.head_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowPage>c__Iterator85 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>page;
        internal LifeSkillSelectFriendPanel <>f__this;
        internal int <i>__1;
        internal int <index>__0;
        internal UIAutoGenItem<LifeSkillSelectFriendPanel.FriendGridItemTemplate, LifeSkillSelectFriendPanel.FriendGridItemModel> <item>__2;
        internal UIScrollView <scroll>__3;
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
                    if (this.<>f__this.FriendPageIndex != null)
                    {
                        this.<>f__this.FriendPageIndex.CurrentPage = this.page;
                        this.<>f__this.TableFriendGrid.Count = this.<>f__this.FriendPageIndex.PageItemCount;
                        this.<index>__0 = 0;
                        this.<i>__1 = this.<>f__this.FriendPageIndex.Start;
                        while (this.<i>__1 < this.<>f__this.FriendPageIndex.End)
                        {
                            this.<item>__2 = this.<>f__this.TableFriendGrid[this.<index>__0];
                            this.<item>__2.Model.Friend = this.<>f__this.FriendPageIndex.List[this.<i>__1];
                            this.<item>__2.Model.OnItemWorkClick = new Action<LifeSkillSelectFriendPanel.FriendGridItemModel>(this.<>f__this.OnItemWorkClick);
                            this.<index>__0++;
                            this.<i>__1++;
                        }
                        this.<>f__this.lb_pagenum.text = string.Format("{0}/{1}", this.<>f__this.FriendPageIndex.CurrentPage, this.<>f__this.FriendPageIndex.PageCount);
                        this.<>f__this.lb_empty.enabled = this.<>f__this.TableFriendGrid.Count == 0;
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 1;
                        return true;
                    }
                    break;

                case 1:
                    this.<>f__this.TableFriendGrid.RepositionLayout();
                    this.<scroll>__3 = this.<>f__this.lst_view.GetComponent<UIScrollView>();
                    if (this.<scroll>__3 != null)
                    {
                        this.<scroll>__3.ResetPosition();
                        this.$PC = -1;
                        break;
                    }
                    break;
            }
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

    public class FriendGridItemModel : TableItemModel<LifeSkillSelectFriendPanel.FriendGridItemTemplate>
    {
        private FastBuf.Friend _friend;
        public Action<LifeSkillSelectFriendPanel.FriendGridItemModel> OnItemWorkClick;
        private UITableManager<StartItem> startTable = new UITableManager<StartItem>();

        public override void Init(LifeSkillSelectFriendPanel.FriendGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.startTable.InitFromGrid(template.StarGrid);
            template.bt_work.OnUIMouseClick(delegate (object u) {
                if (this.OnItemWorkClick != null)
                {
                    this.OnItemWorkClick(this);
                }
            });
        }

        private SelectState _State { get; set; }

        public FastBuf.Friend Friend
        {
            get
            {
                return this._friend;
            }
            set
            {
                this._friend = value;
                base.Template.Name.text = value.userInfo.name;
                base.Template.Level.text = string.Format("LV.{0}", value.userInfo.level);
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(this._friend.userInfo.head_entry);
                if (_config != null)
                {
                    CommonFunc.SetPlayerHeadFrame(base.Template.QualityBorder, base.Template.QIcon, value.userInfo.head_frame_entry);
                    base.Template.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    this.startTable.Count = 0;
                    this.State = (SelectState) value.newlifeskill_employ_time;
                }
            }
        }

        public SelectState State
        {
            get
            {
                return this._State;
            }
            set
            {
                this._State = value;
                base.Template.bt_work.gameObject.SetActive(this._State == SelectState.Empty);
                switch (value)
                {
                    case SelectState.Empty:
                        base.Template.lb_state.text = string.Empty;
                        break;

                    case SelectState.Working:
                        base.Template.lb_state.text = ConfigMgr.getInstance().GetWord(0x2c5e);
                        break;

                    case SelectState.HadWork:
                        base.Template.lb_state.text = ConfigMgr.getInstance().GetWord(0x2c5f);
                        break;
                }
            }
        }

        public NewLifeSkillType Type { get; set; }

        public enum SelectState
        {
            Empty,
            Working,
            HadWork
        }

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
            this.StarGrid = base.FindChild<UIGrid>("StarGrid");
            this.bt_work = base.FindChild<UIButton>("bt_work");
            this.lb_state = base.FindChild<UILabel>("lb_state");
        }

        public UIButton bt_work { get; private set; }

        public UITexture Icon { get; private set; }

        public UILabel lb_state { get; private set; }

        public UILabel Level { get; private set; }

        public UILabel Name { get; private set; }

        public UISprite QIcon { get; private set; }

        public UISprite QualityBorder { get; private set; }

        public UIGrid StarGrid { get; private set; }
    }
}

