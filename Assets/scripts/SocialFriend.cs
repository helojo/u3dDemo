using FastBuf;
using fastJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Toolbox;
using UnityEngine;

public class SocialFriend : XSingleton<SocialFriend>
{
    private string _key = "MT-d----------FF-dd";
    private string _locojoy_url = "http://121.41.46.244/mt2_base/index";
    private SocialState _s;
    [CompilerGenerated]
    private static Func<trench_elite_config, int> <>f__am$cache10;
    [CompilerGenerated]
    private static Comparison<SocialUser> <>f__am$cacheE;
    [CompilerGenerated]
    private static Func<trench_normal_config, int> <>f__am$cacheF;
    private bool isStartedReuqest;
    private List<SocialPlatformFriend> PlatformFriends;
    private long sessionUserID;
    private Dictionary<string, SocialUser> SocialUsers = new Dictionary<string, SocialUser>();
    private List<SocialUserInfo> SocialUserTrench;

    public SocialFriend()
    {
        this.Message = string.Empty;
        this.SocialFriends = new List<QQFriendUser>();
        this.SocialUserTrench = new List<SocialUserInfo>();
        this.PlatformFriends = new List<SocialPlatformFriend>();
    }

    public void AddOwner(bool next = false, DuplicateType type = 0)
    {
        int eliteProgress = ActorData.getInstance().EliteProgress;
        int normalProgress = ActorData.getInstance().NormalProgress;
        if (next)
        {
            switch (type)
            {
                case DuplicateType.DupType_Normal:
                    normalProgress = this.Next(normalProgress, type);
                    break;

                case DuplicateType.DupType_Elite:
                    eliteProgress = this.Next(eliteProgress, type);
                    break;
            }
        }
        Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) ActorData.getInstance().HeadEntry);
        SocialUser user2 = new SocialUser();
        QQFriendUser user3 = new QQFriendUser {
            open_id = this.Key
        };
        BriefUser user4 = new BriefUser {
            leaderInfo = cardByEntry,
            head_entry = ActorData.getInstance().HeadEntry,
            lastOnlineTime = TimeMgr.Instance.ServerStampTime,
            id = -1L,
            name = ActorData.getInstance().UserInfo.name,
            titleEntry = -1,
            level = ActorData.getInstance().Level,
            signature = string.Empty
        };
        user3.userInfo = user4;
        user2.QQUser = user3;
        SocialPlatformFriend friend = new SocialPlatformFriend {
            SocialName = GameDefine.getInstance().TencentLoginNickName,
            OpenID = this.Key,
            SamillIconID = GameDefine.getInstance().TencentLoginSelfPic
        };
        user2.Plat = friend;
        user2.Owner = true;
        SocialUserInfo info = new SocialUserInfo {
            Elite = eliteProgress,
            Normal = normalProgress,
            Hero = -1,
            Level = ActorData.getInstance().Level,
            UserID = -1L
        };
        user2.UserInfo = info;
        SocialUser user = user2;
        if (this.SocialUsers.ContainsKey(user.QQUser.open_id))
        {
            this.SocialUsers.Remove(user.QQUser.open_id);
        }
        this.SocialUsers.Add(user.QQUser.open_id, user);
        this.Owner = user;
    }

    private void callBack(Texture texture)
    {
        this.OwnerHead = texture;
    }

    private void CallBack(bool success, string content, string error)
    {
        if (success)
        {
            Dictionary<string, object> dictionary = JSON.Instance.ToObject<Dictionary<string, object>>(content);
            string str = dictionary["errno"].ToString();
            if (str != "0")
            {
                this.Message = "Web Server Response Error Code:" + str;
                this.State = SocialState.Failure;
            }
            else
            {
                object obj2 = dictionary["data"];
                List<object> list = (List<object>) obj2;
                List<SocialUserInfo> list2 = new List<SocialUserInfo>(0x10);
                for (int i = 0; i < list.Count; i++)
                {
                    Dictionary<string, object> dictionary2 = (Dictionary<string, object>) list[i];
                    char[] separator = new char[] { ':' };
                    string[] strArray = dictionary2["userid"].ToString().Split(separator);
                    long result = 0L;
                    if (long.TryParse(strArray[1], out result))
                    {
                    }
                    int num3 = StrParser.ParseDecInt(dictionary2["level"].ToString());
                    int num4 = StrParser.ParseDecInt(dictionary2["normal_trench"].ToString());
                    int num5 = StrParser.ParseDecInt(dictionary2["elite_trench"].ToString());
                    int num6 = StrParser.ParseDecInt(dictionary2["hero_trench"].ToString());
                    int num7 = StrParser.ParseDecInt(dictionary2["qq_vip_lv"].ToString());
                    SocialUserInfo item = new SocialUserInfo {
                        Elite = num5,
                        Hero = num6,
                        Level = num3,
                        Normal = num4,
                        UserID = result,
                        VipLevel = num7
                    };
                    list2.Add(item);
                }
                this.SocialUserTrench = list2;
                this.State = SocialState.Ready;
                this.RequestComplete();
            }
        }
        else
        {
            this.State = SocialState.Failure;
            this.Message = "Send Request To Web Server Failure Error:" + error;
        }
    }

    internal void CompletePlatformRequest(List<SocialPlatformFriend> list)
    {
        this.isStartedReuqest = false;
        this.PlatformFriends = list;
        this.RequestWeb();
    }

    public bool Contains(long userID)
    {
        <Contains>c__AnonStorey28C storeyc = new <Contains>c__AnonStorey28C {
            userID = userID,
            have = false
        };
        this.Each(new EachCondtion(storeyc.<>m__5EB));
        return storeyc.have;
    }

    public void Each(EachCondtion condtion)
    {
        foreach (KeyValuePair<string, SocialUser> pair in this.SocialUsers)
        {
            if (!pair.Value.Owner && condtion(pair.Value))
            {
                break;
            }
        }
    }

    public int GetPowerByUserID(long userID)
    {
        if (this.State == SocialState.Ready)
        {
            foreach (KeyValuePair<string, SocialUser> pair in this.SocialUsers)
            {
                if ((pair.Value.QQUser != null) && (pair.Value.QQUser.userInfo.id == userID))
                {
                    return pair.Value.QQUser.power;
                }
            }
        }
        return 0;
    }

    public void Init()
    {
        this.sessionUserID = (int) ActorData.getInstance().SessionInfo.userid;
        this._locojoy_url = GameDefine.getInstance().GetPlayerInfoURL();
        this.State = SocialState.NoStarted;
        this.isStartedReuqest = false;
        this.SocialFriends.Clear();
        this.SocialUserTrench.Clear();
        this.PlatformFriends.Clear();
        this.SocialUsers.Clear();
        this.HadInited = false;
    }

    [DebuggerHidden]
    private IEnumerator Loading(string url, Action<Texture> callBack)
    {
        return new <Loading>c__IteratorB0 { url = url, callBack = callBack, <$>url = url, <$>callBack = callBack };
    }

    public int Next(int entry, DuplicateType type)
    {
        List<int> list = new List<int>();
        if (type == DuplicateType.DupType_Normal)
        {
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = t => t.entry;
            }
            list = XSingleton<ConfigMgr>.Singleton.getListResult<trench_normal_config>().Select<trench_normal_config, int>(<>f__am$cacheF).ToList<int>();
        }
        if (type == DuplicateType.DupType_Elite)
        {
            if (<>f__am$cache10 == null)
            {
                <>f__am$cache10 = t => t.entry;
            }
            list = XSingleton<ConfigMgr>.Singleton.getListResult<trench_elite_config>().Select<trench_elite_config, int>(<>f__am$cache10).ToList<int>();
        }
        for (int i = 0; i < (list.Count - 1); i++)
        {
            if (list[i] == entry)
            {
                return list[i + 1];
            }
        }
        if (list.Count > 0)
        {
            return list[list.Count - 1];
        }
        return -1;
    }

    private void RequestComplete()
    {
        foreach (QQFriendUser user in this.SocialFriends)
        {
            if (!this.SocialUsers.ContainsKey(user.open_id))
            {
                SocialUser user2 = new SocialUser {
                    QQUser = user
                };
                this.SocialUsers.Add(user.open_id, user2);
            }
        }
        foreach (SocialUserInfo info in this.SocialUserTrench)
        {
            foreach (KeyValuePair<string, SocialUser> pair in this.SocialUsers)
            {
                if (pair.Value.QQUser.userInfo.id == info.UserID)
                {
                    pair.Value.UserInfo = info;
                }
            }
        }
        foreach (SocialPlatformFriend friend in this.PlatformFriends)
        {
            SocialUser user3;
            if (this.SocialUsers.TryGetValue(friend.OpenID, out user3))
            {
                user3.Plat = friend;
                user3.QQUser.userInfo.name = friend.SocialName;
            }
        }
        this.Message = string.Format("获得好友数量:{0}", this.SocialUsers.Count);
        this.AddOwner(false, DuplicateType.DupType_Normal);
    }

    public void RequestGameServerForGetFriendList()
    {
        if (!this.isStartedReuqest)
        {
            ScheduleMgr.Instance.RunWork(this.Loading(GameDefine.getInstance().TencentLoginSelfPic, new Action<Texture>(this.callBack)));
            this.isStartedReuqest = true;
            this.State = SocialState.RequestingServer;
            SocketMgr.Instance.RequestGameQQFriends();
        }
    }

    internal void RequestPlatformFaiulre(string message)
    {
        this.State = SocialState.Failure;
        this.Message = "request Platform Failure(" + message + ")";
    }

    [DebuggerHidden]
    private IEnumerator RequestServer(string url, WWWForm postData, Action<bool, string, string> callBack)
    {
        return new <RequestServer>c__IteratorAF { url = url, postData = postData, callBack = callBack, <$>url = url, <$>postData = postData, <$>callBack = callBack };
    }

    internal void RequestServerError()
    {
        this.Message = "Game Server Responser Error";
        this.State = SocialState.Failure;
    }

    private void RequestWeb()
    {
        string str = string.Empty;
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < this.SocialFriends.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(",");
            }
            QQFriendUser user = this.SocialFriends[i];
            builder.Append(user.server_id);
            builder.Append(":");
            builder.Append(user.userInfo.id);
        }
        str = builder.ToString();
        string url = string.Format("{0}", this._locojoy_url);
        this.State = SocialState.RequestingWeb;
        WWWForm postData = new WWWForm();
        postData.AddField("userid", str);
        ScheduleMgr.Instance.RunWork(this.RequestServer(url, postData, new Action<bool, string, string>(this.CallBack)));
    }

    public void Update(List<QQFriendUser> socialFriend)
    {
        this.SocialFriends = socialFriend;
        if (!this.HadInited)
        {
            if (socialFriend.Count == 0)
            {
                this.State = SocialState.Failure;
                this.Message = "Server Response a empty list!!";
            }
            else
            {
                this.HadInited = true;
                this.State = SocialState.RequestingPlatform;
                PlatformInterface.mInstance.PlatformQueryFriend();
                Debug.Log("Server Response Success!!");
            }
        }
    }

    internal void UpdateCoolDownTime(long p)
    {
        <UpdateCoolDownTime>c__AnonStorey28D storeyd = new <UpdateCoolDownTime>c__AnonStorey28D {
            p = p
        };
        this.Each(new EachCondtion(storeyd.<>m__5EC));
    }

    internal void UpdateQQFriendGame(QQFriendUser_Fresh u)
    {
        foreach (KeyValuePair<string, SocialUser> pair in this.SocialUsers)
        {
            if (pair.Value.QQUser.userInfo.id == u.user_id)
            {
                pair.Value.QQUser.givePhyForceTime = u.givePhyForceTime;
                pair.Value.QQUser.acceptMinePhyForceTime = u.acceptMinePhyForceTime;
                pair.Value.QQUser.alreadyGivePhyForceToday = u.alreadyGivePhyForceToday;
                pair.Value.QQUser.canRecvLifeSkillAction = u.canRecvLifeSkillAction;
                pair.Value.QQUser.canSendLifeSkillAction = u.canSendLifeSkillAction;
                pair.Value.QQUser.cool_down_time = u.cool_down_time;
                pair.Value.QQUser.isGivePhyForceNow = u.isGivePhyForceNow;
            }
        }
    }

    public bool HadInited { get; private set; }

    private string Key
    {
        get
        {
            return this._key;
        }
    }

    public string Message { get; set; }

    public SocialUser Owner { get; private set; }

    public Texture OwnerHead { get; set; }

    private int OwnerLastRank { get; set; }

    public int OwnerRank
    {
        get
        {
            List<SocialUser> users = this.Users;
            int num = 1;
            foreach (SocialUser user in users)
            {
                if (user.QQUser.open_id == this.Key)
                {
                    return num;
                }
                num++;
            }
            return num;
        }
    }

    public List<QQFriendUser> ServerSocialUser
    {
        get
        {
            return this.SocialFriends;
        }
    }

    private List<QQFriendUser> SocialFriends { get; set; }

    public SocialState State
    {
        get
        {
            return this._s;
        }
        private set
        {
            this._s = value;
            Debug.Log("Social API:" + this._s.ToString());
        }
    }

    public List<SocialUser> Users
    {
        get
        {
            List<SocialUser> list = this.SocialUsers.Values.ToList<SocialUser>();
            if (<>f__am$cacheE == null)
            {
                <>f__am$cacheE = delegate (SocialUser l, SocialUser r) {
                    if (l.UserInfo != null)
                    {
                        if (r.UserInfo == null)
                        {
                            return -1;
                        }
                        if (l.UserInfo.Elite > 0)
                        {
                            if (l.UserInfo.Elite > r.UserInfo.Elite)
                            {
                                return -1;
                            }
                            if (l.UserInfo.Elite == r.UserInfo.Elite)
                            {
                                return 0;
                            }
                            return 1;
                        }
                        if (l.UserInfo.Normal > r.UserInfo.Normal)
                        {
                            return -1;
                        }
                        if (l.UserInfo.Normal == r.UserInfo.Normal)
                        {
                            return 0;
                        }
                    }
                    return 1;
                };
            }
            list.Sort(<>f__am$cacheE);
            int num = 1;
            foreach (SocialUser user in list)
            {
                user.Index = num;
                num++;
            }
            return list;
        }
    }

    [CompilerGenerated]
    private sealed class <Contains>c__AnonStorey28C
    {
        internal bool have;
        internal long userID;

        internal bool <>m__5EB(SocialUser t)
        {
            if (t.QQUser.userInfo.id == this.userID)
            {
                this.have = true;
                return true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <Loading>c__IteratorB0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Action<Texture> <$>callBack;
        internal string <$>url;
        internal WWW <ww>__0;
        internal Action<Texture> callBack;
        internal string url;

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
                    this.<ww>__0 = new WWW(this.url);
                    break;

                case 1:
                    break;

                default:
                    goto Label_009C;
            }
            if (!this.<ww>__0.isDone)
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (string.IsNullOrEmpty(this.<ww>__0.error))
            {
                if (this.callBack != null)
                {
                    this.callBack(this.<ww>__0.texture);
                }
                this.$PC = -1;
            }
        Label_009C:
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
    private sealed class <RequestServer>c__IteratorAF : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Action<bool, string, string> <$>callBack;
        internal WWWForm <$>postData;
        internal string <$>url;
        internal WWW <www>__0;
        internal Action<bool, string, string> callBack;
        internal WWWForm postData;
        internal string url;

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
                    this.<www>__0 = new WWW(this.url, this.postData);
                    break;

                case 1:
                    break;

                default:
                    goto Label_00CF;
            }
            if (!this.<www>__0.isDone)
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (!string.IsNullOrEmpty(this.<www>__0.error))
            {
                if (this.callBack != null)
                {
                    this.callBack(false, string.Empty, this.<www>__0.error);
                }
            }
            else if (this.callBack != null)
            {
                this.callBack(true, this.<www>__0.text, string.Empty);
            }
            this.$PC = -1;
        Label_00CF:
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
    private sealed class <UpdateCoolDownTime>c__AnonStorey28D
    {
        internal long p;

        internal bool <>m__5EC(SocialUser u)
        {
            if (u.QQUser.userInfo.id == this.p)
            {
                u.QQUser.cool_down_time = 0;
                return true;
            }
            return false;
        }
    }

    public delegate bool EachCondtion(SocialUser item);

    public enum SocialState
    {
        NoStarted,
        RequestingServer,
        RequestingPlatform,
        RequestingWeb,
        Failure,
        Ready
    }
}

