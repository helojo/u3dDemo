using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Toolbox;
using UnityEngine;

public class GoldTreePanel : GUIEntity
{
    private List<ResultItme> _messages = new List<ResultItme>();
    [CompilerGenerated]
    private static System.Action <>f__am$cache17;
    private UIButton bt_once;
    private UIButton bt_ten;
    private UILabel coin;
    private UILabel gold;
    private UILabel HongBaoTip;
    private S2C_ShakeGoldTree LastResult;
    private Friend mCurrFriendInfo;
    private GuildMember mCurrGuildMemberInfo;
    private SocialUser mCurrQQFriendInfo;
    private int mCurrSelectIndex;
    private UILabel message;
    private List<GuildMember> mGuildMemberList;
    private List<Friend> mHongBaoFriendList;
    private HongBaoType mHongBaoType;
    private List<SocialUser> mQQFriendList;
    public int OpenType = 1;
    private UIScrollView ScrollView;
    private UILabel tipButtom;
    private UILabel tipTimes;
    private UILabel title;
    private UITexture treeSprite;
    private TweenPosition tween;

    private Friend GetNextFriendInfo()
    {
        if (this.mHongBaoFriendList != null)
        {
            this.mCurrSelectIndex++;
            if (this.mCurrSelectIndex >= this.mHongBaoFriendList.Count)
            {
                this.mCurrSelectIndex = 0;
            }
            for (int i = this.mCurrSelectIndex; i < this.mHongBaoFriendList.Count; i++)
            {
                if (!this.mHongBaoFriendList[i].redpackage_isdraw && (this.mHongBaoFriendList[i].redpackage_num > 0))
                {
                    return this.mHongBaoFriendList[i];
                }
            }
            for (int j = 0; j < this.mCurrSelectIndex; j++)
            {
                if (!this.mHongBaoFriendList[j].redpackage_isdraw && (this.mHongBaoFriendList[j].redpackage_num > 0))
                {
                    return this.mHongBaoFriendList[j];
                }
            }
        }
        return null;
    }

    private GuildMember GetNextGuildMemberInfo()
    {
        if (this.mGuildMemberList != null)
        {
            this.mCurrSelectIndex++;
            if (this.mCurrSelectIndex >= this.mGuildMemberList.Count)
            {
                this.mCurrSelectIndex = 0;
            }
            for (int i = this.mCurrSelectIndex; i < this.mGuildMemberList.Count; i++)
            {
                if (!this.mGuildMemberList[i].userInfo.redpackage_isdraw && (this.mGuildMemberList[i].userInfo.redpackage_num > 0))
                {
                    return this.mGuildMemberList[i];
                }
            }
            for (int j = 0; j < this.mCurrSelectIndex; j++)
            {
                if (!this.mGuildMemberList[j].userInfo.redpackage_isdraw && (this.mGuildMemberList[j].userInfo.redpackage_num > 0))
                {
                    return this.mGuildMemberList[j];
                }
            }
        }
        return null;
    }

    private SocialUser GetNextQQFriendInfo()
    {
        this.mCurrSelectIndex++;
        if (this.mQQFriendList != null)
        {
            if (this.mCurrSelectIndex >= this.mQQFriendList.Count)
            {
                this.mCurrSelectIndex = 0;
            }
            if (this.mQQFriendList.Count > 0)
            {
                return this.mQQFriendList[this.mCurrSelectIndex];
            }
        }
        return null;
    }

    private bool IsEnoughShakeCount(int count, out bool stone)
    {
        int num = ActorData.getInstance().UserInfo.cur_shake_count;
        stone = false;
        int num2 = 0;
        for (int i = num; i < (num + count); i++)
        {
            num2 += this.StoneCost(i);
        }
        if (ActorData.getInstance().Stone < num2)
        {
            stone = true;
            return false;
        }
        int vipType = ActorData.getInstance().VipType;
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(vipType);
        if (_config == null)
        {
            return false;
        }
        if ((_config.shake_gold_count - num) < count)
        {
            return false;
        }
        return true;
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.CheckMultiCameraStatus();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MainUI activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MainUI>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.lockSwippy = false;
            if (GoldTreeScene.Current != null)
            {
                GoldTreeScene.Current.Actived(false);
            }
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        GoldTreeScene.Current.SetHongBaoScene(false);
        this.treeSprite = base.transform.FindChild<UITexture>("treeSprite");
        this.bt_once = base.transform.FindChild<UIButton>("bt_once");
        this.bt_once.OnUIMouseClick(delegate (object o) {
            bool stone = false;
            if (this.IsEnoughShakeCount(1, out stone))
            {
                SocketMgr.Instance.RequestShakeGoldTree(1);
            }
            else
            {
                this.ShowError(stone);
            }
        });
        this.bt_ten = base.transform.FindChild<UIButton>("bt_ten");
        this.bt_ten.OnUIMouseClick(delegate (object o) {
            bool stone = false;
            if (this.IsEnoughShakeCount(10, out stone))
            {
                if (this.LastResult != null)
                {
                    if (<>f__am$cache17 == null)
                    {
                        <>f__am$cache17 = () => SocketMgr.Instance.RequestShakeGoldTree(10);
                    }
                    GoldTreeConfrim.Show(string.Format("{0}", this.LastResult.nextTenCastStone), string.Format("{0}", this.LastResult.nextTenGainGold), <>f__am$cache17, null);
                }
            }
            else
            {
                this.ShowError(stone);
            }
        });
        this.tween = base.transform.FindChild<TweenPosition>("ShowPanel");
        this.coin = base.transform.FindChild<UILabel>("lb_CoinValue");
        this.gold = base.transform.FindChild<UILabel>("lb_GoldValue");
        this.message = base.transform.FindChild<UILabel>("Message");
        this.title = base.transform.FindChild<UILabel>("TopLabel");
        this.tipTimes = base.transform.FindChild<UILabel>("tipTimes");
        this.HongBaoTip = base.transform.FindChild<UILabel>("HongBaoTip");
        this.tipButtom = base.transform.FindChild<UILabel>("tipButtom");
        this.ScrollView = base.transform.FindChild<UIScrollView>("Scroll View");
        this.title.text = ConfigMgr.getInstance().GetWord(0x2bc0);
        this.bt_once.Text(ConfigMgr.getInstance().GetWord(0x2bc1));
        this.bt_ten.Text(ConfigMgr.getInstance().GetWord(0x2bc2));
        this.tipButtom.text = ConfigMgr.getInstance().GetWord(0x2bc3);
        this.UpdateShakeTimes();
        if (GoldTreeScene.Current != null)
        {
            GoldTreeScene.Current.Actived(true);
        }
        SocketMgr.Instance.RequestShakeGoldTree(-1);
        MainUI activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MainUI>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.lockSwippy = true;
        }
    }

    public override void OnReset()
    {
        base.OnReset();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (GoldTreeScene.Current != null)
        {
            RenderTexture currentTexture = GoldTreeScene.Current.CurrentTexture;
            if (currentTexture != null)
            {
                this.treeSprite.mainTexture = currentTexture;
            }
        }
        if (this.OpenType == 1)
        {
            int vipType = ActorData.getInstance().VipType;
            int num2 = ActorData.getInstance().UserInfo.cur_shake_count;
            vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(vipType);
            if ((_config != null) && ((_config.shake_gold_count - num2) < 10))
            {
                this.bt_ten.SetState(UIButtonColor.State.Disabled, true);
            }
        }
    }

    public void PickOver(long friendid, int type)
    {
        switch (((HongBaoType) type))
        {
            case HongBaoType.GameFriend:
                this.mCurrFriendInfo.redpackage_num = 0;
                this.UpdateHongBaoFriendInfo(this.mCurrFriendInfo);
                break;

            case HongBaoType.GuildFriend:
                this.mCurrGuildMemberInfo.userInfo.redpackage_num = 0;
                this.UpdateHongBaoGuildFriendInfo(this.mCurrGuildMemberInfo);
                break;
        }
        this.SetHongBaoFriendInfo(friendid);
    }

    [DebuggerHidden]
    private IEnumerator RunMessageShow()
    {
        return new <RunMessageShow>c__Iterator7F { <>f__this = this };
    }

    private void SetCurrSelectIndex()
    {
        switch (this.mHongBaoType)
        {
            case HongBaoType.GameFriend:
                if ((this.mHongBaoFriendList != null) && (this.mCurrFriendInfo != null))
                {
                    for (int i = 0; i < this.mHongBaoFriendList.Count; i++)
                    {
                        if (this.mHongBaoFriendList[i].userInfo.id == this.mCurrFriendInfo.userInfo.id)
                        {
                            this.mCurrSelectIndex = i;
                            break;
                        }
                    }
                    break;
                }
                this.mCurrSelectIndex = 0;
                return;

            case HongBaoType.QQFriend:
                if ((this.mQQFriendList != null) && (this.mCurrQQFriendInfo != null))
                {
                    for (int j = 0; j < this.mQQFriendList.Count; j++)
                    {
                        if (this.mQQFriendList[j].QQUser.userInfo.id == this.mCurrQQFriendInfo.QQUser.userInfo.id)
                        {
                            this.mCurrSelectIndex = j;
                            break;
                        }
                    }
                    break;
                }
                this.mCurrSelectIndex = 0;
                return;

            case HongBaoType.GuildFriend:
                if ((this.mGuildMemberList != null) && (this.mCurrGuildMemberInfo != null))
                {
                    for (int k = 0; k < this.mGuildMemberList.Count; k++)
                    {
                        if (this.mGuildMemberList[k].userInfo.id == this.mCurrGuildMemberInfo.userInfo.id)
                        {
                            this.mCurrSelectIndex = k;
                            break;
                        }
                    }
                    break;
                }
                this.mCurrSelectIndex = 0;
                return;
        }
    }

    public void SetGuildHongBaoStat(GuildMember _CurrGuildMember, List<GuildMember> _GuildMemberList)
    {
        this.mHongBaoType = HongBaoType.GuildFriend;
        GoldTreeScene.Current.SetHongBaoScene(true);
        this.mGuildMemberList = _GuildMemberList;
        this.SetHongCommStat();
        this.UpdateHongBaoGuildFriendInfo(_CurrGuildMember);
        this.SetCurrSelectIndex();
        this.bt_once.OnUIMouseClick(delegate (object o) {
            if (this.mCurrGuildMemberInfo != null)
            {
                if (!this.mCurrGuildMemberInfo.userInfo.redpackage_isdraw && (this.mCurrGuildMemberInfo.userInfo.redpackage_num > 0))
                {
                    SocketMgr.Instance.RequestDrawRedPackage(this.mCurrGuildMemberInfo.userInfo.id, this.mCurrGuildMemberInfo.userInfo.name, string.Empty, 2, ActorData.getInstance().UserInfo.name);
                }
                else if (this.mCurrGuildMemberInfo.userInfo.redpackage_num < 1)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x887));
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x883));
                }
            }
        });
        this.bt_ten.OnUIMouseClick(delegate (object o) {
            if ((this.mGuildMemberList == null) || ((this.mGuildMemberList != null) && (this.mGuildMemberList.Count == 1)))
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x887));
            }
            else
            {
                GuildMember nextGuildMemberInfo = this.GetNextGuildMemberInfo();
                if (nextGuildMemberInfo != null)
                {
                    this.UpdateHongBaoGuildFriendInfo(nextGuildMemberInfo);
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x887));
                }
            }
        });
    }

    public void SetHongBaoFriendInfo(long friendid)
    {
        <SetHongBaoFriendInfo>c__AnonStorey1FF storeyff = new <SetHongBaoFriendInfo>c__AnonStorey1FF {
            friendid = friendid
        };
        switch (this.mHongBaoType)
        {
            case HongBaoType.GameFriend:
                if (this.mHongBaoFriendList != null)
                {
                    Friend friend = this.mHongBaoFriendList.Find(new Predicate<Friend>(storeyff.<>m__347));
                    if (friend != null)
                    {
                        Friend friend2 = ActorData.getInstance().FriendList.Find(new Predicate<Friend>(storeyff.<>m__348));
                        if (friend2 != null)
                        {
                            friend.redpackage_isdraw = friend2.redpackage_isdraw;
                            friend.redpackage_num = friend2.redpackage_num;
                        }
                        if (friend.redpackage_num < 0)
                        {
                            friend.redpackage_num = 0;
                        }
                    }
                }
                break;

            case HongBaoType.GuildFriend:
                if (this.mGuildMemberList != null)
                {
                    GuildMember member = this.mGuildMemberList.Find(new Predicate<GuildMember>(storeyff.<>m__349));
                    if (member != null)
                    {
                        GuildMember member2 = ActorData.getInstance().mGuildMemberData.member.Find(new Predicate<GuildMember>(storeyff.<>m__34A));
                        if (member2 != null)
                        {
                            member.userInfo.redpackage_isdraw = member2.userInfo.redpackage_isdraw;
                            member.userInfo.redpackage_num = member2.userInfo.redpackage_num;
                        }
                        if (member.userInfo.redpackage_num < 0)
                        {
                            member.userInfo.redpackage_num = 0;
                        }
                    }
                }
                break;
        }
    }

    public void SetHongBaoStat(Friend _currFriend, List<Friend> _FriendList)
    {
        GoldTreeScene.Current.SetHongBaoScene(true);
        this.mHongBaoType = HongBaoType.GameFriend;
        this.mHongBaoFriendList = _FriendList;
        this.SetHongCommStat();
        this.UpdateHongBaoFriendInfo(_currFriend);
        this.SetCurrSelectIndex();
        this.bt_once.OnUIMouseClick(delegate (object o) {
            if (this.mCurrFriendInfo != null)
            {
                if (!this.mCurrFriendInfo.redpackage_isdraw && (this.mCurrFriendInfo.redpackage_num > 0))
                {
                    SocketMgr.Instance.RequestDrawRedPackage(this.mCurrFriendInfo.userInfo.id, this.mCurrFriendInfo.userInfo.name, string.Empty, 1, ActorData.getInstance().UserInfo.name);
                }
                else if (this.mCurrFriendInfo.redpackage_num < 1)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x887));
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x883));
                }
            }
        });
        this.bt_ten.OnUIMouseClick(delegate (object o) {
            if ((this.mHongBaoFriendList == null) || ((this.mHongBaoFriendList != null) && (this.mHongBaoFriendList.Count == 1)))
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x887));
            }
            else
            {
                Friend nextFriendInfo = this.GetNextFriendInfo();
                if (nextFriendInfo != null)
                {
                    this.UpdateHongBaoFriendInfo(nextFriendInfo);
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x887));
                }
            }
        });
    }

    private void SetHongCommStat()
    {
        this.tipTimes.gameObject.SetActive(false);
        base.transform.FindChild("Midd").gameObject.SetActive(false);
        base.transform.FindChild("TopLabel").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x87e);
        this.bt_once.transform.FindChild("Label").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x880);
        this.bt_ten.transform.FindChild("Label").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x881);
    }

    public void SetQQHongBaoStat(SocialUser _currQQFriend, List<SocialUser> _QQFriendList)
    {
        this.mHongBaoType = HongBaoType.QQFriend;
        GoldTreeScene.Current.SetHongBaoScene(true);
        this.mQQFriendList = _QQFriendList;
        this.SetHongCommStat();
        this.UpdateHongBaoQQFriendInfo(_currQQFriend);
        this.SetCurrSelectIndex();
        this.bt_once.OnUIMouseClick(delegate (object o) {
            if (this.mCurrQQFriendInfo != null)
            {
                if (!this.mCurrQQFriendInfo.QQUser.redpackage_isdraw && (this.mCurrQQFriendInfo.QQUser.redpackage_num > 0))
                {
                    SocketMgr.Instance.RequestDrawRedPackage(this.mCurrQQFriendInfo.QQUser.userInfo.id, this.mCurrQQFriendInfo.QQUser.userInfo.name, this.mCurrQQFriendInfo.QQUser.open_id, 0, ActorData.getInstance().UserInfo.name);
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x883));
                }
            }
        });
        this.bt_ten.OnUIMouseClick(delegate (object o) {
            SocialUser nextQQFriendInfo = this.GetNextQQFriendInfo();
            this.UpdateHongBaoQQFriendInfo(nextQQFriendInfo);
        });
    }

    internal void ShowError(bool stone)
    {
        if (stone)
        {
            <ShowError>c__AnonStorey1FD storeyfd = new <ShowError>c__AnonStorey1FD {
                <>f__this = this,
                title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
            };
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyfd.<>m__343), null);
        }
        else
        {
            int vipType = ActorData.getInstance().VipType;
            ArrayList list = ConfigMgr.getInstance().getList<vip_config>();
            int entry = 0;
            IEnumerator enumerator = list.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    vip_config _config = current as vip_config;
                    if (_config.entry > entry)
                    {
                        entry = _config.entry;
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
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord((vipType < entry) ? 0x2bc7 : 0x2bc8));
        }
    }

    public void ShowShakeResult(S2C_ShakeGoldTree r)
    {
        List<GoldTreeShakeRes> shakeRes = r.shakeRes;
        this.LastResult = r;
        this.UpdateShakeTimes();
        if (shakeRes.Count > 0)
        {
            this.tween.gameObject.SetActive(true);
            this.tween.Play(true);
        }
        foreach (GoldTreeShakeRes res in shakeRes)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format(ConfigMgr.getInstance().GetWord(0x2bc5), res.cost_stone, res.single_gold * res.crit_multiple));
            if (res.crit_multiple > 1)
            {
                builder.Append("  " + string.Format(ConfigMgr.getInstance().GetWord(0x2bc6), res.crit_multiple));
            }
            ResultItme item = new ResultItme {
                M = res.crit_multiple,
                Message = builder.ToString()
            };
            this._messages.Add(item);
        }
        base.StopAllCoroutines();
        base.StartCoroutine(this.RunMessageShow());
        if (GoldTreeScene.Current != null)
        {
            if (shakeRes.Count == 1)
            {
                GoldTreeScene.Current.DoShakeOnce();
            }
            else if (shakeRes.Count > 1)
            {
                GoldTreeScene.Current.DoShakeTen();
            }
        }
        this.gold.text = string.Format("{0:0}", this.LastResult.nextGainGold);
        this.coin.text = string.Format("{0:0}", this.LastResult.nextCastStone);
        Daily activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<Daily>();
        if ((null != activityGUIEntity) && activityGUIEntity.mIsExistShakeGoldQuest)
        {
            SocketMgr.Instance.RequestDailyQuest();
            activityGUIEntity.mIsExistShakeGoldQuest = false;
        }
    }

    private int StoneCost(int nShakeCount)
    {
        int num = 0;
        if (nShakeCount < GameConstValues.GOLD_TREE_SHAKE_LIMIT_0)
        {
            return GameConstValues.GOLD_TREE_SHAKE_LIMIT_COST_0;
        }
        if (nShakeCount <= GameConstValues.GOLD_TREE_SHAKE_LIMIT_1)
        {
            return GameConstValues.GOLD_TREE_SHAKE_LIMIT_COST_1;
        }
        if (nShakeCount <= GameConstValues.GOLD_TREE_SHAKE_LIMIT_2)
        {
            return GameConstValues.GOLD_TREE_SHAKE_LIMIT_COST_2;
        }
        if (nShakeCount <= GameConstValues.GOLD_TREE_SHAKE_LIMIT_3)
        {
            return GameConstValues.GOLD_TREE_SHAKE_LIMIT_COST_3;
        }
        if (nShakeCount <= GameConstValues.GOLD_TREE_SHAKE_LIMIT_4)
        {
            return GameConstValues.GOLD_TREE_SHAKE_LIMIT_COST_4;
        }
        if (nShakeCount <= GameConstValues.GOLD_TREE_SHAKE_LIMIT_5)
        {
            return GameConstValues.GOLD_TREE_SHAKE_LIMIT_COST_5;
        }
        if (nShakeCount <= GameConstValues.GOLD_TREE_SHAKE_LIMIT_6)
        {
            num = GameConstValues.GOLD_TREE_SHAKE_LIMIT_COST_6;
        }
        return num;
    }

    public void UpdateData(S2C_DrawRedPackage res)
    {
        GoldTreeScene.Current.PlayHongBaoTx();
        switch (this.mHongBaoType)
        {
            case HongBaoType.GameFriend:
                if ((this.mCurrFriendInfo != null) && (this.mCurrFriendInfo.userInfo.id == res.friendid))
                {
                    this.mCurrFriendInfo.redpackage_isdraw = true;
                    this.mCurrFriendInfo.redpackage_num = res.friend_redpackage_num;
                    this.UpdateHongBaoFriendInfo(this.mCurrFriendInfo);
                    if (!this.tween.gameObject.activeSelf)
                    {
                        this.tween.gameObject.SetActive(true);
                        this.tween.Play(true);
                    }
                    this.message.text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x884), GameConstant.DefaultTextRedColor + this.mCurrFriendInfo.userInfo.name + GameConstant.DefaultTextColor, string.Format(ConfigMgr.getInstance().GetWord(0xa652a4), GameConstant.DefaultTextRedColor + res.incStone));
                }
                this.SetHongBaoFriendInfo(res.friendid);
                break;

            case HongBaoType.QQFriend:
                if ((this.mCurrQQFriendInfo != null) && (this.mCurrQQFriendInfo.QQUser.userInfo.id == res.friendid))
                {
                    this.mCurrQQFriendInfo.QQUser.redpackage_isdraw = true;
                    this.mCurrQQFriendInfo.QQUser.redpackage_num = res.friend_redpackage_num;
                    this.UpdateHongBaoQQFriendInfo(this.mCurrQQFriendInfo);
                    if (!this.tween.gameObject.activeSelf)
                    {
                        this.tween.gameObject.SetActive(true);
                        this.tween.Play(true);
                    }
                    this.message.text = string.Format(ConfigMgr.getInstance().GetWord(0x884), this.mCurrQQFriendInfo.QQUser.userInfo.name, string.Format(ConfigMgr.getInstance().GetWord(0xa652a4), res.incStone));
                }
                this.SetHongBaoFriendInfo(res.friendid);
                break;

            case HongBaoType.GuildFriend:
                if ((this.mCurrGuildMemberInfo != null) && (this.mCurrGuildMemberInfo.userInfo.id == res.friendid))
                {
                    this.mCurrGuildMemberInfo.userInfo.redpackage_isdraw = true;
                    this.mCurrGuildMemberInfo.userInfo.redpackage_num = res.friend_redpackage_num;
                    this.UpdateHongBaoGuildFriendInfo(this.mCurrGuildMemberInfo);
                    if (!this.tween.gameObject.activeSelf)
                    {
                        this.tween.gameObject.SetActive(true);
                        this.tween.Play(true);
                    }
                    this.message.text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x884), GameConstant.DefaultTextRedColor + this.mCurrGuildMemberInfo.userInfo.name + GameConstant.DefaultTextColor, string.Format(ConfigMgr.getInstance().GetWord(0xa652a4), GameConstant.DefaultTextRedColor + res.incStone));
                }
                this.SetHongBaoFriendInfo(res.friendid);
                break;
        }
    }

    private void UpdateFriendInfo(long friendid, int incStone)
    {
        <UpdateFriendInfo>c__AnonStorey1FE storeyfe = new <UpdateFriendInfo>c__AnonStorey1FE {
            friendid = friendid
        };
        if (this.mHongBaoFriendList != null)
        {
            Friend friend = this.mHongBaoFriendList.Find(new Predicate<Friend>(storeyfe.<>m__346));
            if ((friend != null) && (friend.redpackage_num != incStone))
            {
                friend.redpackage_isdraw = true;
                friend.redpackage_num = incStone;
            }
        }
    }

    private void UpdateHongBaoFriendInfo(Friend _currFriend)
    {
        this.mCurrFriendInfo = _currFriend;
        if (_currFriend != null)
        {
            this.HongBaoTip.text = string.Format(ConfigMgr.getInstance().GetWord(0x87f), _currFriend.redpackage_num);
            if (this.mCurrFriendInfo.redpackage_isdraw)
            {
                this.tipButtom.text = string.Format(ConfigMgr.getInstance().GetWord(0x889), _currFriend.userInfo.name) + "(" + ConfigMgr.getInstance().GetWord(0x882) + ")";
            }
            else if (this.mCurrFriendInfo.redpackage_num < 1)
            {
                this.tipButtom.text = string.Format(ConfigMgr.getInstance().GetWord(0x88a), _currFriend.userInfo.name);
            }
            else
            {
                this.tipButtom.text = string.Format(ConfigMgr.getInstance().GetWord(0x889), _currFriend.userInfo.name);
            }
            this.HongBaoTip.gameObject.SetActive(true);
        }
    }

    private void UpdateHongBaoGuildFriendInfo(GuildMember _CurrGuildMember)
    {
        this.mCurrGuildMemberInfo = _CurrGuildMember;
        if (this.mCurrGuildMemberInfo != null)
        {
            this.HongBaoTip.text = string.Format(ConfigMgr.getInstance().GetWord(0x87f), this.mCurrGuildMemberInfo.userInfo.redpackage_num);
            if (this.mCurrGuildMemberInfo.userInfo.redpackage_isdraw)
            {
                this.tipButtom.text = string.Format(ConfigMgr.getInstance().GetWord(0x889), this.mCurrGuildMemberInfo.userInfo.name) + "(" + ConfigMgr.getInstance().GetWord(0x882) + ")";
            }
            else if (this.mCurrGuildMemberInfo.userInfo.redpackage_num < 1)
            {
                this.tipButtom.text = string.Format(ConfigMgr.getInstance().GetWord(0x88a), this.mCurrGuildMemberInfo.userInfo.name);
            }
            else
            {
                this.tipButtom.text = string.Format(ConfigMgr.getInstance().GetWord(0x889), this.mCurrGuildMemberInfo.userInfo.name);
            }
            this.HongBaoTip.gameObject.SetActive(true);
        }
    }

    private void UpdateHongBaoQQFriendInfo(SocialUser _currQQFriend)
    {
        this.mCurrQQFriendInfo = _currQQFriend;
        if (this.mCurrQQFriendInfo != null)
        {
            this.HongBaoTip.text = string.Format(ConfigMgr.getInstance().GetWord(0x87f), _currQQFriend.QQUser.redpackage_num);
            if (this.mCurrQQFriendInfo.QQUser.redpackage_isdraw)
            {
                this.tipButtom.text = string.Format(ConfigMgr.getInstance().GetWord(0x889), _currQQFriend.QQUser.userInfo.name) + "(" + ConfigMgr.getInstance().GetWord(0x882) + ")";
            }
            else
            {
                this.tipButtom.text = string.Format(ConfigMgr.getInstance().GetWord(0x889), _currQQFriend.QQUser.userInfo.name);
            }
            this.HongBaoTip.gameObject.SetActive(true);
        }
    }

    private void UpdateShakeTimes()
    {
        int vipType = ActorData.getInstance().VipType;
        int num2 = ActorData.getInstance().UserInfo.cur_shake_count;
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(vipType);
        this.tipTimes.text = string.Format(ConfigMgr.getInstance().GetWord(0x2bc4), _config.shake_gold_count - num2, _config.shake_gold_count);
        int num3 = this.StoneCost(num2 + 1);
        int num4 = GameConstValues.GOLD_TREE_GOLD_BASE + ((ActorData.getInstance().Level - 1) * GameConstValues.GOLD_TREE_GOLD_GROW);
        float num5 = 1f + (num2 * (((float) GameConstValues.GOLD_TREE_GOLD_GROW_RATE) / 10000f));
        int num6 = Mathf.CeilToInt(num4 * num5);
        this.gold.text = string.Format("{0:0}", num6);
        this.coin.text = string.Format("{0:0}", num3);
    }

    [CompilerGenerated]
    private sealed class <RunMessageShow>c__Iterator7F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal LinkedList<string>.Enumerator <$s_661>__3;
        internal GoldTreePanel <>f__this;
        internal string <i>__4;
        internal LinkedList<string> <list>__0;
        internal GoldTreePanel.ResultItme <m>__1;
        internal string <sb>__2;

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
                    this.<>f__this.ScrollView.ResetPosition();
                    this.<list>__0 = new LinkedList<string>();
                    break;

                case 1:
                    break;

                default:
                    goto Label_0160;
            }
            if (this.<>f__this._messages.Count > 0)
            {
                this.<m>__1 = this.<>f__this._messages[0];
                this.<list>__0.AddLast(this.<m>__1.Message);
                this.<sb>__2 = string.Empty;
                this.<$s_661>__3 = this.<list>__0.GetEnumerator();
                try
                {
                    while (this.<$s_661>__3.MoveNext())
                    {
                        this.<i>__4 = this.<$s_661>__3.Current;
                        this.<sb>__2 = this.<i>__4 + '\n' + this.<sb>__2;
                    }
                }
                finally
                {
                    this.<$s_661>__3.Dispose();
                }
                this.<>f__this.message.text = this.<sb>__2;
                this.<>f__this._messages.RemoveAt(0);
                this.$current = new WaitForSeconds((this.<m>__1.M <= 1) ? 0.2f : 0.8f);
                this.$PC = 1;
                return true;
            }
            this.$PC = -1;
        Label_0160:
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

    [CompilerGenerated]
    private sealed class <SetHongBaoFriendInfo>c__AnonStorey1FF
    {
        internal long friendid;

        internal bool <>m__347(Friend e)
        {
            return (e.userInfo.id == this.friendid);
        }

        internal bool <>m__348(Friend e)
        {
            return (e.userInfo.id == this.friendid);
        }

        internal bool <>m__349(GuildMember e)
        {
            return (e.userInfo.id == this.friendid);
        }

        internal bool <>m__34A(GuildMember e)
        {
            return (e.userInfo.id == this.friendid);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowError>c__AnonStorey1FD
    {
        internal GoldTreePanel <>f__this;
        internal string title;

        internal void <>m__343(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            e.Achieve<MessageBox>().SetDialog(this.title, delegate (GameObject _go) {
                GUIMgr.Instance.ExitModelGUI(this.<>f__this);
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }, null, false);
        }

        internal void <>m__350(GameObject _go)
        {
            GUIMgr.Instance.ExitModelGUI(this.<>f__this);
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateFriendInfo>c__AnonStorey1FE
    {
        internal long friendid;

        internal bool <>m__346(Friend e)
        {
            return (e.userInfo.id == this.friendid);
        }
    }

    public enum HongBaoType
    {
        GameFriend,
        QQFriend,
        GuildFriend
    }

    private class ResultItme
    {
        public int M { get; set; }

        public string Message { get; set; }
    }
}

