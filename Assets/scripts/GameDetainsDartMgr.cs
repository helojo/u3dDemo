using FastBuf;
using System;
using System.Collections.Generic;
using Toolbox;

public class GameDetainsDartMgr : XSingleton<GameDetainsDartMgr>
{
    public int buyInterceptTimes = -1;
    public bool canIntercept;
    public EnermyItemData curBattleEnermyInfo;
    public DetainsDartState curDetainsDartState = DetainsDartState.None;
    public ConvoyEnemyInfo curEnermyInfo;
    public int curEscortCnt = -1;
    public int curEscortLeftTime = -1;
    public EscortState curEscortState = EscortState.None;
    public int CurFriendPageId;
    public int curInterceptLeftTime;
    public InterceptState curInterceptState;
    public ConvoyRobTarget curInterceptTargetInfo;
    public int curInterceptTimes = -1;
    public int curLeftRightPage = -1;
    public List<EnermyItemData> curPageEnermyInfo = new List<EnermyItemData>();
    public List<FriendItemData> curPageFriendInfo = new List<FriendItemData>();
    public List<ConvoyFriendFormationInfo> CurPageFriendInfo_new = new List<ConvoyFriendFormationInfo>();
    public int curRefreshFlagCnt = -1;
    public int curRefreshInterceptCnt = -1;
    public List<long> curReqEscortCardIdList = new List<long>();
    public ConvoyInfo curReqEscortInfo = new ConvoyInfo();
    public long curReqFriendId = -1L;
    public List<long> CurReqPageFriendIdList = new List<long>();
    public int curSelFlagIndex;
    public int curSelTeamIndex = -1;
    public List<ConvoyEnemyInfo> EnemyListInfo = new List<ConvoyEnemyInfo>();
    public List<ConvoyInfo> EscortListInfo = new List<ConvoyInfo>();
    public List<ConvoyRobTarget> InterceptListInfo = new List<ConvoyRobTarget>();
    public bool isHadEscortTeam;
    public bool isTest;
    public int maxBuyInterceptCnt = -1;
    public int maxEscortCnt;
    public int maxInterceptCnt;
    public int onePageMaxFriendCnt = 10;
    public Dictionary<int, List<long>> pageIdToPageFriendIdDic = new Dictionary<int, List<long>>();
    public Dictionary<int, List<int>> TeamIdToTeamCardsInfo = new Dictionary<int, List<int>>();

    public GameDetainsDartMgr()
    {
        this.TeamIdToTeamCardsInfo = new Dictionary<int, List<int>>();
    }

    private void Clear()
    {
        this.maxEscortCnt = 0;
        this.maxInterceptCnt = 0;
        this.curSelTeamIndex = -1;
        this.curLeftRightPage = -1;
        this.TeamIdToTeamCardsInfo.Clear();
        XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Clear();
        this.curEscortState = EscortState.None;
        this.curInterceptState = InterceptState.Battle;
        this.curDetainsDartState = DetainsDartState.None;
    }

    internal void ReqDetaisDartInfo()
    {
        this.Clear();
        if (!this.isTest)
        {
            SocketMgr.Instance.RequestC2S_GetConvoyInfo();
        }
    }

    internal void UpdateDetainsDartInfo(S2C_GetConvoyInfo message)
    {
    }

    public enum DetainsDartState
    {
        Escort,
        Intercept,
        None
    }

    public class EnermyItemData
    {
        public List<CardInfo> cards;
        public int loseCourageNum;
        public int loseGoldNum;
        public int mainCardEntry;
        public int mainCardQuality;
        public int playerLv;
        public string playerName;
    }

    public enum EscortState
    {
        Selet,
        Doing,
        Done,
        GettingReward,
        None
    }

    public class FriendItemData
    {
        public List<CardInfo> cards;
        public int mainCardEntry;
        public int mainCardQuality;
        public int playerLv;
        public string playerName;
    }

    public enum InterceptState
    {
        SeletPlayer,
        Battle
    }
}

