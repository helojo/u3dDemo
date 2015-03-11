using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldCupPanel : GUIEntity
{
    public GameObject _ApplyPanel;
    public GameObject _BaoMingBtn;
    public UILabel _BaoMingBtnLabel;
    public GameObject _BaoMingLabel;
    public GameObject _GamePanel;
    public UILabel _GameTimeLb;
    public UILabel _GameTimeTips;
    public GameObject _PickRewardBtn;
    public GameObject _PlayerGroup;
    public GameObject _PreRewardGroup;
    public GameObject _RankListGroup;
    public GameObject _RewardBtn;
    public GameObject _RewardPanel;
    public GameObject _RewardRank;
    public UILabel _RewardTimeTips;
    public GameObject _SinglePreRwardItem;
    public GameObject _SingleRankItem;
    public UILabel _TimeLabelTips;
    public UILabel _WaitTips;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache23;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache24;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache25;
    private Dictionary<int, LeagueMatchData> LeagueMatchDict = new Dictionary<int, LeagueMatchData>();
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private bool mAlreadyReqFinalInfo;
    private int mCurrEndTime;
    private int mCurrGameOverTime;
    private int mDailyRewardHour = 0x13;
    private int mDailyRewardMinite = 30;
    private bool mGameTick;
    private bool mIsSetGameInfoOver;
    private bool mIsStart;
    public bool mLockPkBtnEvent;
    private bool mRequestedPlayerListOk;
    private bool mRequestingState;
    private bool mRewardCooldown;
    private LeagueMatchData mSelectLeagueMatch;

    public void GetLeagueHistorySuccess()
    {
    }

    private LeagueMatchData GetLeagueMatch()
    {
        bool flag = ActorData.getInstance().CurrJoinLeague != -1;
        foreach (LeagueMatchData data in this.LeagueMatchDict.Values)
        {
            league_match_config lmc = data.lmc;
            if (!flag)
            {
                if (data.state == LeagueState.LeagueState_Apply)
                {
                    return data;
                }
            }
            else if (ActorData.getInstance().CurrJoinLeague == data.entry)
            {
                return data;
            }
        }
        return this.GetSoonOpenLeagueMatch();
    }

    private string GetMatchTime(int _time)
    {
        if ((_time / 0x15180) > 0)
        {
            return ((_time / 0x15180) + "天");
        }
        return ((_time / 0xe10) + "小时");
    }

    private LeagueMatchData GetSoonOpenLeagueMatch()
    {
        int num = 0;
        LeagueMatchData data = null;
        foreach (LeagueMatchData data2 in this.LeagueMatchDict.Values)
        {
            league_match_config lmc = data2.lmc;
            int startTime = CommonFunc.GetStartTime(lmc.start_time);
            int num3 = Mathf.FloorToInt(((float) (TimeMgr.Instance.ServerStampTime - startTime)) / ((float) lmc.repeated_period));
            if (num3 < 0)
            {
                num3 = 0;
            }
            else
            {
                startTime += lmc.repeated_period * (num3 + 1);
            }
            if (num == 0)
            {
                num = startTime;
                data = data2;
            }
            else if (num > startTime)
            {
                num = startTime;
                data = data2;
            }
        }
        return data;
    }

    private void InitLeagueData()
    {
        this.LeagueMatchDict.Clear();
        IEnumerator enumerator = ConfigMgr.getInstance().getList<league_match_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                league_match_config current = (league_match_config) enumerator.Current;
                if (current.is_league && (current.faction == -1))
                {
                    LeagueMatchData data = new LeagueMatchData {
                        entry = current.entry,
                        lmc = current,
                        state = LeagueState.LeagueState_None
                    };
                    this.LeagueMatchDict.Add(current.entry, data);
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
    }

    public bool InLeagueMatchState()
    {
        LeagueMatchData leagueMatch = this.GetLeagueMatch();
        if ((leagueMatch.state != LeagueState.LeagueState_Pre_Match) && (leagueMatch.state != LeagueState.LeagueState_Final_Match))
        {
            return false;
        }
        return true;
    }

    public void JoinLeagueSucess()
    {
        this.UpdateData();
    }

    public void LeagueApplySuccess()
    {
        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x27e6));
        if (this._BaoMingBtn.active)
        {
            this._BaoMingBtn.gameObject.SetActive(false);
        }
        if (!this._BaoMingLabel.active)
        {
            this._BaoMingLabel.gameObject.SetActive(true);
        }
    }

    private void OnClickBaoMingBtn(GameObject go)
    {
        if ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward != -1))
        {
            SocketMgr.Instance.RequestPickLeagueReward(LeagueRewardType.LeagueRewardType_Pre_Match);
        }
        else if ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward != -1))
        {
            SocketMgr.Instance.RequestPickLeagueReward(LeagueRewardType.LeagueRewardType_World);
        }
        else if ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.dailyReward != -1))
        {
            SocketMgr.Instance.RequestPickLeagueReward(LeagueRewardType.LeagueRewardType_Daily);
        }
        else if (this.mSelectLeagueMatch != null)
        {
            league_match_config lmc = this.mSelectLeagueMatch.lmc;
            int startTime = CommonFunc.GetStartTime(lmc.start_time);
            int num2 = Mathf.FloorToInt(((float) (TimeMgr.Instance.ServerStampTime - startTime)) / ((float) lmc.repeated_period));
            if (num2 < 0)
            {
                num2 = 0;
            }
            startTime += lmc.repeated_period * num2;
            int num3 = 0;
            if ((ActorData.getInstance().JoinLeagueInfo.leagueEntry != this.mSelectLeagueMatch.entry) && (TimeMgr.Instance.ServerStampTime > (startTime + lmc.apply_duration)))
            {
                num3 = 1;
            }
            int num4 = (lmc.repeated_period * num3) + startTime;
            int num5 = ((lmc.repeated_period * num3) + startTime) + lmc.apply_duration;
            if (TimeMgr.Instance.ServerStampTime < num4)
            {
                bool flag;
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x27e2), TimeMgr.Instance.GetArenaTime(out flag, num4)));
            }
            else if (ActorData.getInstance().JoinLeagueInfo.leagueEntry == this.mSelectLeagueMatch.entry)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x27e5));
            }
            else
            {
                SocketMgr.Instance.RequestLeagueApply(this.mSelectLeagueMatch.entry);
            }
        }
    }

    private void OnClickChangeTeam()
    {
        if (<>f__am$cache24 == null)
        {
            <>f__am$cache24 = delegate (GUIEntity entity) {
                SelectHeroPanel panel = (SelectHeroPanel) entity;
                panel.mBattleType = BattleType.WorldCupDefense;
                panel.SetButtonState(BattleType.WorldCupDefense);
            };
        }
        GUIMgr.Instance.PushGUIEntity("SelectHeroPanel", <>f__am$cache24);
    }

    private void OnClickIcon(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            LeagueOpponent opponent = obj2 as LeagueOpponent;
            if (opponent != null)
            {
                ActorData.getInstance().IsOnlyShowTargetTeam = true;
                SocketMgr.Instance.RequestGetLeagueOpponentFormation(ActorData.getInstance().JoinLeagueInfo.leagueEntry, ActorData.getInstance().JoinLeagueInfo.groupId, opponent.userInfo.id);
            }
        }
    }

    private void OnClickLingQuBtn(GameObject go)
    {
        if ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.dailyReward != -1))
        {
            SocketMgr.Instance.RequestPickLeagueReward(LeagueRewardType.LeagueRewardType_Daily);
        }
    }

    private void OnClickLog()
    {
        SocketMgr.Instance.RequestLeagueHistory();
    }

    private void OnClickPaiHangBang()
    {
        if ((ActorData.getInstance().JoinLeagueInfo.leagueEntry != -1) && (ActorData.getInstance().JoinLeagueInfo.groupId != -1))
        {
            if (<>f__am$cache25 == null)
            {
                <>f__am$cache25 = entity => SocketMgr.Instance.RequestGetLeagueRankList(ActorData.getInstance().JoinLeagueInfo.leagueEntry, ActorData.getInstance().JoinLeagueInfo.groupId);
            }
            GUIMgr.Instance.DoModelGUI("RankingListPanel", <>f__am$cache25, base.gameObject);
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2815));
        }
    }

    private void OnClickPickRewardBtn(GameObject go)
    {
        if ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward != -1))
        {
            SocketMgr.Instance.RequestPickLeagueReward(LeagueRewardType.LeagueRewardType_Pre_Match);
        }
        else if ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward != -1))
        {
            SocketMgr.Instance.RequestPickLeagueReward(LeagueRewardType.LeagueRewardType_World);
        }
    }

    private void OnClickPkBtn(GameObject go)
    {
        <OnClickPkBtn>c__AnonStorey176 storey = new <OnClickPkBtn>c__AnonStorey176 {
            go = go,
            <>f__this = this
        };
        if (!this.mLockPkBtnEvent)
        {
            if (((ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward != -1) || (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward != -1)) || (ActorData.getInstance().CurrLeagueReward.rewardInfo.dailyReward != -1))
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x27f1));
            }
            else if (ActorData.getInstance().RemainPk == 0)
            {
                if (ActorData.getInstance().RemainBuyPk == 0)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2802));
                }
                else
                {
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__1A5), base.gameObject);
                }
            }
            else
            {
                object obj2 = GUIDataHolder.getData(storey.go);
                if (obj2 != null)
                {
                    LeagueOpponent opponent = obj2 as LeagueOpponent;
                    if (opponent != null)
                    {
                        ActorData.getInstance().IsCostStone = false;
                        ActorData.getInstance().LeagueOpponentInfo = opponent;
                        ActorData.getInstance().IsOnlyShowTargetTeam = false;
                        ActorData.getInstance().mCurrWorldCupEndTime = this.mCurrEndTime;
                        SocketMgr.Instance.RequestGetLeagueOpponentFormation(ActorData.getInstance().JoinLeagueInfo.leagueEntry, ActorData.getInstance().JoinLeagueInfo.groupId, opponent.userInfo.id);
                        this.mLockPkBtnEvent = true;
                    }
                }
            }
        }
    }

    private void OnClickPreRewardItemBtn(GameObject go, bool isPressed)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int id = (int) obj2;
            if (isPressed)
            {
                <OnClickPreRewardItemBtn>c__AnonStorey175 storey = new <OnClickPreRewardItemBtn>c__AnonStorey175();
                if (GUIMgr.Instance.GetGUIEntity<InfoDlag>() != null)
                {
                    GUIMgr.Instance.ExitModelGUI("InfoDlag");
                }
                else
                {
                    treasure_config _config = ConfigMgr.getInstance().getByEntry<treasure_config>(id);
                    if (_config != null)
                    {
                        drop_group_config _config2 = ConfigMgr.getInstance().getByEntry<drop_group_config>(_config.drop_id);
                        if (_config2 != null)
                        {
                            char[] separator = new char[] { '|' };
                            string[] strArray = _config2.drop_pool_entry.Split(separator);
                            storey.desc = string.Empty;
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                if (strArray[i] != string.Empty)
                                {
                                    int num3 = int.Parse(strArray[i]);
                                    drop_pool_config _config3 = ConfigMgr.getInstance().getByEntry<drop_pool_config>(num3);
                                    if (_config3 != null)
                                    {
                                        char[] chArray2 = new char[] { '|' };
                                        string[] strArray2 = _config3.drop_item_entry.Split(chArray2);
                                        for (int j = 0; j < strArray2.Length; j++)
                                        {
                                            if (strArray2[j] != string.Empty)
                                            {
                                                item_config _config4 = ConfigMgr.getInstance().getByEntry<item_config>(int.Parse(strArray2[j]));
                                                if (_config4 != null)
                                                {
                                                    storey.desc = storey.desc + _config4.name + "    ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            GUIMgr.Instance.DoModelGUI("InfoDlag", new Action<GUIEntity>(storey.<>m__1A3), null);
                        }
                    }
                }
            }
            else
            {
                GUIMgr.Instance.ExitModelGUI("InfoDlag");
            }
        }
    }

    private void OnClickRewardItemBtn(GameObject go, bool isPressed)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int id = (int) obj2;
            UILabel component = go.transform.FindChild("Count").GetComponent<UILabel>();
            if (isPressed)
            {
                <OnClickRewardItemBtn>c__AnonStorey174 storey = new <OnClickRewardItemBtn>c__AnonStorey174();
                if (GUIMgr.Instance.GetGUIEntity<InfoDlag>() != null)
                {
                    GUIMgr.Instance.ExitModelGUI("InfoDlag");
                }
                else
                {
                    storey.tc = ConfigMgr.getInstance().getByEntry<treasure_config>(id);
                    if (storey.tc != null)
                    {
                        GUIMgr.Instance.DoModelGUI("InfoDlag", new Action<GUIEntity>(storey.<>m__1A1), null);
                    }
                }
            }
            else
            {
                GUIMgr.Instance.ExitModelGUI("InfoDlag");
            }
        }
    }

    private void OnClickShuoMingBtn(GameObject go)
    {
        if (<>f__am$cache23 == null)
        {
            <>f__am$cache23 = delegate (GUIEntity obj) {
                WorldCupRulePanel panel = (WorldCupRulePanel) obj;
                panel.Depth = 800;
                panel.ShowWorldCup(true);
            };
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cache23, null);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
        this.mLockPkBtnEvent = false;
        SocketMgr.Instance.RequestGetLeagueReward();
        this.UpdateTeamInfo();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.InitLeagueData();
        SocketMgr.Instance.RequestGetLeagueReward();
        UIEventListener.Get(base.transform.FindChild("ApplyPanel/BaoMingBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickBaoMingBtn);
        Debug.Log(this.mDailyRewardHour + ":" + this.mDailyRewardMinite);
        this.UpdateTeamInfo();
        UIEventListener.Get(base.transform.FindChild("ApplyPanel/ShuoMingBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShuoMingBtn);
        object[] objArray1 = new object[] { this.mDailyRewardHour, ":", this.mDailyRewardMinite, ConfigMgr.getInstance().GetWord(0x27f6) };
        base.transform.FindChild("GamePanel/RankInfo/MeiRiJiangLiLabel").GetComponent<UILabel>().text = string.Concat(objArray1);
        this.SetRewardItemInfo();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                if (this.mRewardCooldown)
                {
                    this.UpdateRewardPanelState();
                }
                if (this.mGameTick)
                {
                    this.UpdateMatchState();
                    this.SetMatchState(this.GetLeagueMatch());
                    Debug.Log(TimeMgr.Instance.ServerDateTime.Hour + "::::");
                    if (((!this._PickRewardBtn.active && (ActorData.getInstance().CurrLeagueReward != null)) && ((ActorData.getInstance().CurrLeagueReward.rewardInfo.dailyReward < 0) && (TimeMgr.Instance.ServerDateTime.Hour == this.mDailyRewardHour))) && (TimeMgr.Instance.ServerDateTime.Minute == (this.mDailyRewardMinite + 1)))
                    {
                        SocketMgr.Instance.RequestGetLeagueReward();
                    }
                }
            }
        }
    }

    public void PickLeagueRewardSuccess()
    {
        this.UpdateData();
    }

    private void RequestPlayerList(int leagueEntry, int groupId)
    {
        if (!this.mRequestedPlayerListOk)
        {
            this.mRequestedPlayerListOk = true;
            SocketMgr.Instance.RequestGetLeagueOpponentList(leagueEntry, groupId);
        }
    }

    private void SetMatchState(LeagueMatchData data)
    {
        bool flag;
        this.mSelectLeagueMatch = data;
        if (data == null)
        {
            return;
        }
        this.SetMatchTimeInfo(data);
        league_match_config lmc = data.lmc;
        int applyStart = data.applyStart;
        if ((ActorData.getInstance().JoinLeagueInfo.leagueEntry == -1) && (TimeMgr.Instance.ServerStampTime > data.applyEnd))
        {
            this._TimeLabelTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x27e2), TimeMgr.Instance.GetArenaTime(out flag, applyStart + data.lmc.repeated_period));
            if (this._BaoMingBtn.active)
            {
                this._BaoMingBtn.gameObject.SetActive(false);
            }
            return;
        }
        switch (data.state)
        {
            case LeagueState.LeagueState_None:
                if (TimeMgr.Instance.ServerStampTime >= applyStart)
                {
                    this._TimeLabelTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x27e2), TimeMgr.Instance.GetArenaTime(out flag, applyStart + data.lmc.repeated_period));
                    break;
                }
                this._TimeLabelTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x27e2), TimeMgr.Instance.GetArenaTime(out flag, applyStart));
                break;

            case LeagueState.LeagueState_Apply:
                this._TimeLabelTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x27db), TimeMgr.Instance.GetArenaTime(out flag, applyStart + lmc.apply_duration));
                if (ActorData.getInstance().CurrJoinLeague != data.entry)
                {
                    this._BaoMingBtnLabel.text = ConfigMgr.getInstance().GetWord(0x27fa);
                    if (!this._BaoMingBtn.active)
                    {
                        this._BaoMingBtn.gameObject.SetActive(true);
                    }
                }
                else
                {
                    this._BaoMingLabel.gameObject.SetActive(true);
                    this._BaoMingBtn.gameObject.SetActive(false);
                    this._BaoMingBtnLabel.text = ConfigMgr.getInstance().GetWord(0x27fb);
                }
                this.ShowApplyPanel(true);
                goto Label_0812;

            case LeagueState.LeagueState_Group:
                this._TimeLabelTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x27dc), TimeMgr.Instance.GetArenaTime(out flag, (applyStart + lmc.apply_duration) + lmc.regroup_duration));
                if (ActorData.getInstance().CurrJoinLeague != data.entry)
                {
                    this._BaoMingBtnLabel.text = ConfigMgr.getInstance().GetWord(0x27fa);
                }
                else
                {
                    this._BaoMingLabel.gameObject.SetActive(true);
                    this._BaoMingBtn.gameObject.SetActive(false);
                    this._BaoMingBtnLabel.text = ConfigMgr.getInstance().GetWord(0x27fb);
                }
                this.ShowApplyPanel(true);
                goto Label_0812;

            case LeagueState.LeagueState_Pre_Match:
                if ((ActorData.getInstance().CurrJoinLeague != -1) && (data.entry == ActorData.getInstance().JoinLeagueInfo.leagueEntry))
                {
                    if (ActorData.getInstance().CurrJoinGroupId != -1)
                    {
                        this.RequestPlayerList(data.entry, ActorData.getInstance().CurrJoinGroupId);
                    }
                    else if (!this.mRequestingState)
                    {
                        this.mRequestingState = true;
                        SocketMgr.Instance.RequestGetJoinLeague();
                        this._TimeLabelTips.text = ConfigMgr.getInstance().GetWord(0x27ea);
                    }
                    this._GameTimeTips.text = TimeMgr.Instance.GetArenaTime(out flag, data.preMatchEnd);
                    this.mCurrEndTime = data.preMatchEnd;
                    this._GameTimeLb.text = ConfigMgr.getInstance().GetWord(0x2811);
                }
                this.ShowApplyPanel(false);
                goto Label_0812;

            case LeagueState.LeagueState_Pre_Match_Finish:
                this._GameTimeTips.text = TimeMgr.Instance.GetArenaTime(out flag, data.PreMatchFinishEnd);
                this._GameTimeLb.text = ConfigMgr.getInstance().GetWord(0x2813);
                this._RewardTimeTips.text = TimeMgr.Instance.GetRemainTime(data.PreMatchFinishEnd);
                if (!this._PreRewardGroup.active)
                {
                    this.SetPreRewardInfoGroup();
                    this._PreRewardGroup.transform.FindChild("Tips").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x27e8);
                    this._PreRewardGroup.gameObject.SetActive(true);
                }
                if (!this._RewardPanel.active)
                {
                    this._RewardPanel.SetActive(true);
                }
                if (this._ApplyPanel.active)
                {
                    this._ApplyPanel.gameObject.SetActive(false);
                }
                if (this._GamePanel.active)
                {
                    this._GamePanel.gameObject.SetActive(false);
                }
                this._WaitTips.gameObject.SetActive(false);
                this._RewardBtn.gameObject.SetActive(false);
                if ((ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward < 0) && (TimeMgr.Instance.ServerStampTime > (data.preMatchEnd + 0xe10)))
                {
                    SocketMgr.Instance.RequestGetLeagueReward();
                }
                goto Label_0812;

            case LeagueState.LeagueState_Final_Match:
                if ((this.mAlreadyReqFinalInfo || (data.entry != ActorData.getInstance().CurrJoinLeague)) || (ActorData.getInstance().CurrJoinGroupId == -1))
                {
                    if (((ActorData.getInstance().JoinLeagueInfo != null) && (data.entry == ActorData.getInstance().CurrJoinLeague)) && (ActorData.getInstance().CurrJoinGroupId != -1))
                    {
                        this._GameTimeTips.text = TimeMgr.Instance.GetArenaTime(out flag, data.finalMatchEnd);
                        this.mCurrEndTime = data.finalMatchEnd;
                        this._GameTimeLb.text = ConfigMgr.getInstance().GetWord(0x2812);
                        this.ShowApplyPanel(false);
                        this.RequestPlayerList(data.entry, ActorData.getInstance().CurrJoinGroupId);
                    }
                    else
                    {
                        this.ShowApplyPanel(true);
                        this._TimeLabelTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x27e2), TimeMgr.Instance.GetArenaTime(out flag, data.applyStart + data.lmc.repeated_period));
                    }
                }
                else if (!this.mRequestingState)
                {
                    this.mRequestingState = true;
                    SocketMgr.Instance.RequestGetLeagueReward();
                    SocketMgr.Instance.RequestGetJoinLeague();
                    this.mAlreadyReqFinalInfo = true;
                }
                this.ShowApplyPanel(false);
                goto Label_0812;

            case LeagueState.LeagueState_Final_Match_Finish:
                this._GameTimeTips.text = TimeMgr.Instance.GetArenaTime(out flag, data.finalMatchFinishEnd);
                this._GameTimeLb.text = ConfigMgr.getInstance().GetWord(0x2814);
                this._RewardTimeTips.text = TimeMgr.Instance.GetRemainTime(data.finalMatchFinishEnd);
                if (this._ApplyPanel.active)
                {
                    this._ApplyPanel.gameObject.SetActive(false);
                }
                if (this._GamePanel.active)
                {
                    this._GamePanel.gameObject.SetActive(false);
                }
                if (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward > 0)
                {
                    this._RewardPanel.transform.FindChild("Rank/Label").GetComponent<UILabel>().text = (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward + 1).ToString();
                }
                else
                {
                    this._RewardPanel.transform.FindChild("Rank/Label").GetComponent<UILabel>().text = string.Empty;
                }
                if (!this._RankListGroup.active)
                {
                    this._RankListGroup.SetActive(true);
                }
                if (!this._RewardPanel.active)
                {
                    this._RewardPanel.SetActive(true);
                }
                this._WaitTips.gameObject.SetActive(true);
                if ((ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward < 0) && (TimeMgr.Instance.ServerStampTime > (data.finalMatchEnd + 0x1c20)))
                {
                    SocketMgr.Instance.RequestGetLeagueReward();
                }
                goto Label_0812;

            default:
                goto Label_0812;
        }
        this.ShowApplyPanel(true);
    Label_0812:
        if (((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward != -1)) || ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward != -1)))
        {
            if (!this._RewardPanel.active)
            {
                this.SetRewardPanel();
                this._ApplyPanel.SetActive(false);
                this._GamePanel.SetActive(false);
                this._RewardPanel.SetActive(true);
            }
        }
        else if ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.dailyReward != -1))
        {
            this._TimeLabelTips.text = ConfigMgr.getInstance().GetWord(0x27f6);
            this._BaoMingBtnLabel.text = ConfigMgr.getInstance().GetWord(0x27e4);
            if (!this._BaoMingBtn.active)
            {
                this._BaoMingBtn.gameObject.SetActive(true);
            }
            if (!this._PickRewardBtn.active)
            {
                this._PickRewardBtn.gameObject.SetActive(true);
            }
        }
        else if (this._PickRewardBtn.active)
        {
            this._PickRewardBtn.gameObject.SetActive(false);
        }
    }

    private void SetMatchTimeInfo(LeagueMatchData data)
    {
        if (data != null)
        {
            league_match_config lmc = data.lmc;
            int startTime = CommonFunc.GetStartTime(lmc.start_time);
            int num2 = Mathf.FloorToInt(((float) (TimeMgr.Instance.ServerStampTime - startTime)) / ((float) lmc.repeated_period));
            if (num2 < 0)
            {
                num2 = 0;
            }
            startTime += lmc.repeated_period * num2;
            int num3 = 0;
            if (((ActorData.getInstance().JoinLeagueInfo.leagueEntry != lmc.entry) && (TimeMgr.Instance.ServerStampTime > (startTime + lmc.apply_duration))) || ((data.state == LeagueState.LeagueState_None) && (TimeMgr.Instance.ServerStampTime > data.finalMatchFinishEnd)))
            {
                num3 = 1;
            }
            int num4 = (lmc.repeated_period * num3) + startTime;
            int num5 = ((lmc.repeated_period * num3) + startTime) + lmc.apply_duration;
            int num6 = (((lmc.repeated_period * num3) + startTime) + lmc.apply_duration) + lmc.regroup_duration;
            int num7 = ((((lmc.repeated_period * num3) + startTime) + lmc.apply_duration) + lmc.regroup_duration) + lmc.pre_match_duration;
            int num8 = (((((lmc.repeated_period * num3) + startTime) + lmc.apply_duration) + lmc.regroup_duration) + lmc.pre_match_duration) + lmc.pre_match_finish_duration;
            int num9 = ((((((lmc.repeated_period * num3) + startTime) + lmc.apply_duration) + lmc.regroup_duration) + lmc.pre_match_duration) + lmc.pre_match_finish_duration) + lmc.final_match_duration;
            int num10 = (((((((lmc.repeated_period * num3) + startTime) + lmc.apply_duration) + lmc.regroup_duration) + lmc.pre_match_duration) + lmc.pre_match_finish_duration) + lmc.final_match_duration) + lmc.final_match_finish_duration;
            this.mCurrGameOverTime = num10;
            DateTime time = TimeMgr.Instance.ConvertToDateTime((long) num4);
            DateTime time2 = TimeMgr.Instance.ConvertToDateTime((long) num5);
            DateTime time3 = TimeMgr.Instance.ConvertToDateTime((long) num6);
            DateTime time4 = TimeMgr.Instance.ConvertToDateTime((long) num7);
            DateTime time5 = TimeMgr.Instance.ConvertToDateTime((long) num8);
            DateTime time6 = TimeMgr.Instance.ConvertToDateTime((long) num9);
            DateTime time7 = TimeMgr.Instance.ConvertToDateTime((long) num10);
            string[] textArray1 = new string[] { string.Format(ConfigMgr.getInstance().GetWord(160), time.Month, time.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time.Hour, time.Minute), " - 持续：", this.GetMatchTime(num5 - num4) };
            base.transform.FindChild("ApplyPanel/Start/Time").GetComponent<UILabel>().text = string.Concat(textArray1);
            string[] textArray2 = new string[] { string.Format(ConfigMgr.getInstance().GetWord(160), time3.Month, time3.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time3.Hour, time3.Minute), " - 持续：", this.GetMatchTime(num7 - num6) };
            base.transform.FindChild("ApplyPanel/Pre/Time").GetComponent<UILabel>().text = string.Concat(textArray2);
            string[] textArray3 = new string[] { string.Format(ConfigMgr.getInstance().GetWord(160), time5.Month, time5.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time5.Hour, time5.Minute), " - 持续：", this.GetMatchTime(num9 - num8) };
            base.transform.FindChild("ApplyPanel/End/Time").GetComponent<UILabel>().text = string.Concat(textArray3);
            string[] textArray4 = new string[] { string.Format(ConfigMgr.getInstance().GetWord(160), time4.Month, time4.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time4.Hour, time4.Minute), " - ", string.Format(ConfigMgr.getInstance().GetWord(160), time5.Month, time5.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time5.Hour, time5.Minute) };
            this._PreRewardGroup.transform.FindChild("PreEnd/Time").GetComponent<UILabel>().text = string.Concat(textArray4);
            string[] textArray5 = new string[] { string.Format(ConfigMgr.getInstance().GetWord(160), time5.Month, time5.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time5.Hour, time5.Minute), " - ", string.Format(ConfigMgr.getInstance().GetWord(160), time6.Month, time6.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time6.Hour, time6.Minute) };
            this._PreRewardGroup.transform.FindChild("FinalStart/Time").GetComponent<UILabel>().text = string.Concat(textArray5);
            string[] textArray6 = new string[] { string.Format(ConfigMgr.getInstance().GetWord(160), time6.Month, time6.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time6.Hour, time6.Minute), " - ", string.Format(ConfigMgr.getInstance().GetWord(160), time7.Month, time7.Day), " ", string.Format(ConfigMgr.getInstance().GetWord(110), time7.Hour, time7.Minute) };
            this._PreRewardGroup.transform.FindChild("FinalEnd/Time").GetComponent<UILabel>().text = string.Concat(textArray6);
        }
    }

    private void SetPreRewardInfoGroup()
    {
        this._PreRewardGroup.transform.FindChild("Tips").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord((ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward != 1) ? 0x281a : 0x2819);
        arena_reward_config arc = ConfigMgr.getInstance().getByEntry<arena_reward_config>(1);
        arena_reward_config _config2 = ConfigMgr.getInstance().getByEntry<arena_reward_config>(2);
        if ((arc != null) && (_config2 != null))
        {
            Transform transform = this._PreRewardGroup.transform.FindChild("List/Reward1");
            this.SetRewardGird(transform, arc);
            Transform transform2 = this._PreRewardGroup.transform.FindChild("List/Reward2");
            this.SetRewardGird(transform2, _config2);
        }
    }

    private void SetRankingListGroup()
    {
        if (ActorData.getInstance().CurrLeagueReward != null)
        {
            Transform transform = base.transform.FindChild("RewardPanel/RankListGroup/List");
            CommonFunc.DeleteChildItem(transform);
            float num = 34.43f;
            string str = "[ffffff]";
            for (int i = 0; i < ActorData.getInstance().CurrLeagueReward.rankList.rankList.Count; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleRankItem) as GameObject;
                obj2.transform.parent = transform.transform;
                obj2.transform.localPosition = new Vector3(0f, -i * num, -0.1f);
                obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                switch (i)
                {
                    case 0:
                        str = "[f75df7]";
                        break;

                    case 1:
                        str = "[01d1eb]";
                        break;

                    case 2:
                        str = "[c4f301]";
                        break;

                    default:
                        str = "[ffffff]";
                        break;
                }
                obj2.transform.FindChild("Rank").GetComponent<UILabel>().text = str + string.Format(ConfigMgr.getInstance().GetWord(0x61), ConfigMgr.getInstance().GetWord(0x2847 + i));
                obj2.transform.FindChild("Name").GetComponent<UILabel>().text = str + ActorData.getInstance().CurrLeagueReward.rankList.rankList[i].name;
                obj2.transform.FindChild("Level").GetComponent<UILabel>().text = str + "LV" + ActorData.getInstance().CurrLeagueReward.rankList.rankList[i].level;
                obj2.transform.FindChild("Border").gameObject.SetActive((i % 2) != 0);
                obj2.transform.FindChild("self").gameObject.SetActive(ActorData.getInstance().CurrLeagueReward.rankList.rankList[i].userId == ActorData.getInstance().SessionInfo.userid);
            }
        }
    }

    private void SetRewardGird(Transform obj, arena_reward_config arc)
    {
        obj.FindChild("Gold").GetComponent<UILabel>().text = arc.gold.ToString();
        obj.FindChild("Stone").GetComponent<UILabel>().text = arc.stone.ToString();
        Transform transform = obj.FindChild("Item1");
        Transform transform2 = obj.FindChild("Item2");
        if (arc.treasure_entry_1 > -1)
        {
            treasure_config _config = ConfigMgr.getInstance().getByEntry<treasure_config>(arc.treasure_entry_1);
            UITexture component = transform.FindChild("Icon").GetComponent<UITexture>();
            transform.FindChild("Count").GetComponent<UILabel>().text = arc.treasure_num_1.ToString();
        }
        transform.gameObject.SetActive(arc.treasure_entry_1 > 0);
        if (arc.treasure_entry_2 > -1)
        {
            treasure_config _config2 = ConfigMgr.getInstance().getByEntry<treasure_config>(arc.treasure_entry_2);
            transform2.FindChild("Count").GetComponent<UILabel>().text = arc.treasure_num_2.ToString();
        }
        transform.gameObject.SetActive(arc.treasure_entry_2 > 0);
        GUIDataHolder.setData(transform.gameObject, arc.treasure_entry_1);
        GUIDataHolder.setData(transform2.gameObject, arc.treasure_entry_2);
        UIEventListener.Get(transform.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickRewardItemBtn);
        UIEventListener.Get(transform2.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickRewardItemBtn);
    }

    private void SetRewardItemInfo()
    {
        league_world_reward_config _config = ConfigMgr.getInstance().getByEntry<league_world_reward_config>(0);
        if (_config != null)
        {
            for (int i = 0; i < 4; i++)
            {
                Transform transform = this._ApplyPanel.transform.FindChild("Reward/Item" + (i + 1));
                UILabel component = transform.transform.FindChild("Count").GetComponent<UILabel>();
                switch (i)
                {
                    case 0:
                        GUIDataHolder.setData(transform.gameObject, _config.treasure_entry1);
                        component.text = _config.treasure_count1.ToString();
                        break;

                    case 1:
                        GUIDataHolder.setData(transform.gameObject, _config.treasure_entry2);
                        component.text = _config.treasure_count2.ToString();
                        break;

                    case 2:
                        GUIDataHolder.setData(transform.gameObject, _config.treasure_entry3);
                        component.text = _config.treasure_count3.ToString();
                        break;

                    case 3:
                        GUIDataHolder.setData(transform.gameObject, _config.treasure_entry4);
                        component.text = _config.treasure_count4.ToString();
                        break;
                }
                UIEventListener.Get(transform.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickRewardItemBtn);
            }
        }
    }

    private void SetRewardPanel()
    {
        if (ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward != -1)
        {
            this._RewardPanel.transform.FindChild("WaitTips").gameObject.SetActive(false);
            this._RewardPanel.transform.FindChild("Rank").gameObject.SetActive(false);
            this._RankListGroup.SetActive(false);
            if (TimeMgr.Instance.ServerStampTime > this.mCurrGameOverTime)
            {
                this._PreRewardGroup.transform.FindChild("GameOver").gameObject.SetActive(true);
                this._PreRewardGroup.transform.FindChild("PreEnd").gameObject.SetActive(false);
                this._PreRewardGroup.transform.FindChild("FinalStart").gameObject.SetActive(false);
                this._PreRewardGroup.transform.FindChild("FinalEnd").gameObject.SetActive(false);
            }
            else
            {
                if (TimeMgr.Instance.ServerStampTime < ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchRewardCd)
                {
                }
                this._PreRewardGroup.transform.FindChild("GameOver").gameObject.SetActive(false);
                this._PreRewardGroup.transform.FindChild("PreEnd").gameObject.SetActive(true);
                this._PreRewardGroup.transform.FindChild("FinalStart").gameObject.SetActive(true);
                this._PreRewardGroup.transform.FindChild("FinalEnd").gameObject.SetActive(true);
            }
            this.SetPreRewardInfoGroup();
            this._PreRewardGroup.SetActive(true);
        }
        else
        {
            this._PreRewardGroup.SetActive(false);
            if (TimeMgr.Instance.ServerStampTime > ActorData.getInstance().CurrLeagueReward.rewardInfo.finalMatchRewardCd)
            {
                this._RewardPanel.transform.FindChild("WaitTips").gameObject.SetActive(false);
                this._RewardPanel.transform.FindChild("Rank/Label").GetComponent<UILabel>().text = (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward + 1).ToString();
                this._RewardPanel.transform.FindChild("Rank").gameObject.SetActive(true);
            }
            else
            {
                this._RewardPanel.transform.FindChild("Rank").gameObject.SetActive(false);
                this._RewardPanel.transform.FindChild("WaitTips").gameObject.SetActive(true);
                this._RewardPanel.transform.FindChild("WaitTips").GetComponent<UILabel>().text = "等待发放奖励...";
            }
            this.SetRankingListGroup();
            this._RankListGroup.SetActive(true);
        }
    }

    private unsafe void SetTargetInfo(Transform obj, LeagueOpponent _info)
    {
        if (_info != null)
        {
            obj.FindChild("Name").GetComponent<UILabel>().text = _info.userInfo.name;
            obj.FindChild("Rank").GetComponent<UILabel>().text = _info.rank.ToString();
            obj.FindChild("FightPower").GetComponent<UILabel>().text = _info.teamPower.ToString();
            obj.FindChild("Level").GetComponent<UILabel>().text = _info.userInfo.level.ToString();
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_info.userInfo.head_entry);
            if (_config != null)
            {
                UITexture component = obj.FindChild("Icon").GetComponent<UITexture>();
                component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                GUIDataHolder.setData(component.gameObject, _info);
                UIEventListener.Get(component.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickIcon);
                obj.FindChild("QualityBorder").GetComponent<UISprite>().color = *((Color*) &(GameConstant.ConstQuantityColor[_config.quality]));
                Transform transform = obj.FindChild("PkBtn");
                GUIDataHolder.setData(transform.gameObject, _info);
                UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPkBtn);
            }
        }
    }

    private void SetTeamInfo(Transform obj, Card _card)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                obj.FindChild("Level").GetComponent<UILabel>().text = _card.cardInfo.level.ToString();
                CommonFunc.SetQualityColor(obj.FindChild("QualityBorder").GetComponent<UISprite>(), _card.cardInfo.quality);
                obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                for (int i = 0; i < 5; i++)
                {
                    UISprite component = obj.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                    component.gameObject.SetActive(i < _card.cardInfo.starLv);
                    component.transform.localPosition = new Vector3((float) (i * 0x17), 0f, 0f);
                }
                Transform transform = obj.transform.FindChild("Star");
                transform.localPosition = new Vector3(-6.8f - ((_card.cardInfo.starLv - 1) * 12.5f), transform.localPosition.y, 0f);
            }
        }
    }

    public void SetTeamPower(int _teamPower)
    {
        base.transform.FindChild("Team/FightPower").GetComponent<UILabel>().text = _teamPower.ToString();
    }

    public void SetTeamPower(List<Card> cardList)
    {
        base.transform.FindChild("Team/FightPower").GetComponent<UILabel>().text = ActorData.getInstance().GetTeamPowerByCardList(cardList).ToString();
    }

    private void ShowApplyPanel(bool isShow)
    {
        if (isShow)
        {
            if (!this._ApplyPanel.active)
            {
                this._ApplyPanel.gameObject.SetActive(true);
            }
            if (this._GamePanel.active)
            {
                this._GamePanel.gameObject.SetActive(false);
            }
        }
        else
        {
            if (this._ApplyPanel.active)
            {
                this._ApplyPanel.gameObject.SetActive(false);
            }
            if (!this._GamePanel.active)
            {
                this._GamePanel.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateData()
    {
        if (ActorData.getInstance().JoinLeagueInfo != null)
        {
            this.UpdateMatchState();
            LeagueMatchData leagueMatch = this.GetLeagueMatch();
            this.SetMatchState(leagueMatch);
            if ((ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward != -1) || (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward != -1))
            {
                this.SetRewardPanel();
                UIEventListener.Get(this._RewardBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPickRewardBtn);
                this._ApplyPanel.SetActive(false);
                this._GamePanel.SetActive(false);
                this._RewardPanel.SetActive(true);
                this.mRewardCooldown = true;
                this.mGameTick = false;
            }
            else
            {
                this._RewardPanel.gameObject.SetActive(false);
                if (ActorData.getInstance().JoinLeagueInfo.leagueEntry > 0)
                {
                    base.transform.FindChild("GamePanel/RankInfo/Rank").GetComponent<UILabel>().text = ActorData.getInstance().JoinLeagueInfo.rank.ToString();
                    base.transform.FindChild("GamePanel/RankInfo/Count").GetComponent<UILabel>().text = ActorData.getInstance().JoinLeagueInfo.remainPk + "/" + ActorData.getInstance().MaxWorldCupCount;
                    UILabel component = this._GamePanel.transform.FindChild("RoomNum").GetComponent<UILabel>();
                    component.text = ActorData.getInstance().JoinLeagueInfo.groupId.ToString();
                    if ((ActorData.getInstance().JoinLeagueInfo.groupId == 0) || (leagueMatch.state == LeagueState.LeagueState_Final_Match))
                    {
                        component.gameObject.SetActive(false);
                    }
                    else
                    {
                        component.gameObject.SetActive(true);
                    }
                    if ((ActorData.getInstance().CurrLeagueReward != null) && (ActorData.getInstance().CurrLeagueReward.rewardInfo.dailyLeagueEntry > 0))
                    {
                        this._PickRewardBtn.gameObject.SetActive(true);
                    }
                    else
                    {
                        this._PickRewardBtn.gameObject.SetActive(false);
                    }
                    UIEventListener.Get(this._PickRewardBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLingQuBtn);
                }
                else if (ActorData.getInstance().JoinLeagueInfo.leagueEntry == this.mSelectLeagueMatch.entry)
                {
                    this._BaoMingBtn.SetActive(false);
                    this._BaoMingLabel.SetActive(true);
                }
                Debug.Log("::::::::::" + leagueMatch.state);
                switch (leagueMatch.state)
                {
                    case LeagueState.LeagueState_None:
                    case LeagueState.LeagueState_Apply:
                    case LeagueState.LeagueState_Group:
                        this._ApplyPanel.SetActive(true);
                        this._GamePanel.SetActive(false);
                        break;

                    case LeagueState.LeagueState_Pre_Match:
                    case LeagueState.LeagueState_Final_Match:
                        if (ActorData.getInstance().JoinLeagueInfo.leagueEntry != leagueMatch.entry)
                        {
                            this._ApplyPanel.SetActive(true);
                            this._GamePanel.SetActive(false);
                            break;
                        }
                        this._ApplyPanel.SetActive(false);
                        this._GamePanel.SetActive(true);
                        break;

                    case LeagueState.LeagueState_Pre_Match_Finish:
                        if (ActorData.getInstance().JoinLeagueInfo.leagueEntry != leagueMatch.entry)
                        {
                            this._ApplyPanel.SetActive(true);
                            this._GamePanel.SetActive(false);
                            break;
                        }
                        this._ApplyPanel.SetActive(false);
                        this._GamePanel.SetActive(false);
                        this._RankListGroup.SetActive(false);
                        this.SetPreRewardInfoGroup();
                        this._PreRewardGroup.transform.FindChild("Tips").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x27e8);
                        this._PreRewardGroup.gameObject.SetActive(true);
                        this._RewardPanel.SetActive(true);
                        break;

                    case LeagueState.LeagueState_Final_Match_Finish:
                        if (ActorData.getInstance().JoinLeagueInfo.leagueEntry != leagueMatch.entry)
                        {
                            this._ApplyPanel.SetActive(true);
                            this._GamePanel.SetActive(false);
                            break;
                        }
                        this._ApplyPanel.SetActive(false);
                        this._GamePanel.SetActive(false);
                        this._PreRewardGroup.gameObject.SetActive(false);
                        this._RankListGroup.SetActive(true);
                        this._RewardPanel.SetActive(true);
                        break;

                    default:
                        this._ApplyPanel.SetActive(false);
                        this._GamePanel.SetActive(false);
                        break;
                }
                this.mGameTick = true;
                this.mRewardCooldown = false;
            }
            this.mIsStart = true;
        }
    }

    private void UpdateMatchState()
    {
        foreach (LeagueMatchData data in this.LeagueMatchDict.Values)
        {
            league_match_config lmc = data.lmc;
            int startTime = CommonFunc.GetStartTime(lmc.start_time);
            int num2 = Mathf.FloorToInt(((float) (TimeMgr.Instance.ServerStampTime - startTime)) / ((float) lmc.repeated_period));
            if (num2 < 0)
            {
                num2 = 0;
            }
            startTime += lmc.repeated_period * num2;
            int num3 = startTime + lmc.apply_duration;
            int num4 = num3 + lmc.regroup_duration;
            int num5 = num4 + lmc.pre_match_duration;
            int num6 = num5 + lmc.pre_match_finish_duration;
            int num7 = num6 + lmc.final_match_duration;
            int num8 = num7 + lmc.final_match_finish_duration;
            int num9 = startTime + lmc.repeated_period;
            data.applyStart = startTime;
            data.applyEnd = num3;
            data.groupEnd = num4;
            data.preMatchEnd = num5;
            data.PreMatchFinishEnd = num6;
            data.finalMatchEnd = num7;
            data.finalMatchFinishEnd = num8;
            if ((TimeMgr.Instance.ServerStampTime >= startTime) && (TimeMgr.Instance.ServerStampTime < num3))
            {
                data.state = LeagueState.LeagueState_Apply;
            }
            else if ((TimeMgr.Instance.ServerStampTime >= num3) && (TimeMgr.Instance.ServerStampTime < num4))
            {
                data.state = LeagueState.LeagueState_Group;
            }
            else if ((TimeMgr.Instance.ServerStampTime >= num4) && (TimeMgr.Instance.ServerStampTime < num5))
            {
                data.state = LeagueState.LeagueState_Pre_Match;
            }
            else if ((TimeMgr.Instance.ServerStampTime >= num5) && (TimeMgr.Instance.ServerStampTime < num6))
            {
                data.state = LeagueState.LeagueState_Pre_Match_Finish;
            }
            else if ((TimeMgr.Instance.ServerStampTime >= num6) && (TimeMgr.Instance.ServerStampTime < num7))
            {
                data.state = LeagueState.LeagueState_Final_Match;
            }
            else if ((TimeMgr.Instance.ServerStampTime >= num7) && (TimeMgr.Instance.ServerStampTime < num8))
            {
                data.state = LeagueState.LeagueState_Final_Match_Finish;
            }
            else if ((TimeMgr.Instance.ServerStampTime >= num8) && (TimeMgr.Instance.ServerStampTime < num9))
            {
                data.state = LeagueState.LeagueState_None;
            }
            else
            {
                data.state = LeagueState.LeagueState_None;
            }
        }
    }

    public void UpdatePlayerList(List<LeagueOpponent> opponentList)
    {
        int num = 0;
        for (int i = 0; i < 3; i++)
        {
            Transform transform = base.transform.FindChild("GamePanel/Target/" + (i + 1));
            if ((num < opponentList.Count) && (opponentList[num].userInfo.id > 0L))
            {
                this.SetTargetInfo(transform, opponentList[num]);
                num++;
                transform.gameObject.SetActive(true);
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
        this._PlayerGroup.gameObject.SetActive(num > 0);
    }

    private void UpdateRewardPanelState()
    {
        if (ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchReward != -1)
        {
            if (TimeMgr.Instance.ServerStampTime < ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchRewardCd)
            {
                this._RewardTimeTips.text = TimeMgr.Instance.GetRemainTime(ActorData.getInstance().CurrLeagueReward.rewardInfo.preMatchRewardCd);
                if (this._RewardBtn.active)
                {
                    this._RewardBtn.SetActive(false);
                }
            }
            else
            {
                this._RewardTimeTips.text = string.Empty;
                if (!this._RewardBtn.active)
                {
                    this._RewardBtn.SetActive(true);
                }
                if (this._WaitTips.active)
                {
                    this._WaitTips.gameObject.SetActive(false);
                }
            }
        }
        else if (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward != -1)
        {
            if (TimeMgr.Instance.ServerStampTime < ActorData.getInstance().CurrLeagueReward.rewardInfo.finalMatchRewardCd)
            {
                this._RewardTimeTips.text = TimeMgr.Instance.GetRemainTime(ActorData.getInstance().CurrLeagueReward.rewardInfo.finalMatchRewardCd);
                if (this._RewardBtn.active)
                {
                    this._RewardBtn.SetActive(false);
                }
            }
            else
            {
                this._RewardTimeTips.text = string.Empty;
                if (!this._RewardBtn.active)
                {
                    this._RewardBtn.SetActive(true);
                }
                if (!this._RewardRank.active)
                {
                    this._RewardRank.SetActive(true);
                }
                if (this._WaitTips.active)
                {
                    this._WaitTips.gameObject.SetActive(false);
                }
                this.mRewardCooldown = false;
                this._RewardPanel.transform.FindChild("Rank/Label").GetComponent<UILabel>().text = (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward + 1).ToString();
                if (ActorData.getInstance().CurrLeagueReward.rewardInfo.worldReward < 0)
                {
                    SocketMgr.Instance.RequestGetLeagueReward();
                }
            }
        }
    }

    public void UpdateTeamInfo()
    {
        if (ActorData.getInstance().WorldCupFormation != null)
        {
            List<Card> cardList = new List<Card>();
            foreach (long num in ActorData.getInstance().WorldCupFormation.card_id)
            {
                if (num != -1L)
                {
                    Card cardByID = ActorData.getInstance().GetCardByID(num);
                    if (cardByID != null)
                    {
                        cardList.Add(cardByID);
                    }
                }
            }
            cardList.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
            int num2 = 0;
            for (int i = 0; i < 5; i++)
            {
                Transform transform = base.transform.FindChild("Team/Pos" + (i + 1));
                if (num2 < cardList.Count)
                {
                    this.SetTeamInfo(transform, cardList[num2]);
                    num2++;
                    transform.gameObject.SetActive(true);
                }
                else
                {
                    transform.gameObject.SetActive(false);
                }
            }
            this.SetTeamPower(cardList);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickPkBtn>c__AnonStorey176
    {
        internal WorldCupPanel <>f__this;
        internal GameObject go;

        internal void <>m__1A5(GUIEntity obj)
        {
            ((MessageBox) obj).SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x27ff), 50, ActorData.getInstance().RemainBuyPk), delegate (GameObject box) {
                if (ActorData.getInstance().Stone < 50)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2800));
                }
                else
                {
                    object obj2 = GUIDataHolder.getData(this.go);
                    if (obj2 != null)
                    {
                        LeagueOpponent opponent = obj2 as LeagueOpponent;
                        if (opponent != null)
                        {
                            ActorData.getInstance().IsOnlyShowTargetTeam = false;
                            ActorData.getInstance().mCurrWorldCupEndTime = this.<>f__this.mCurrEndTime;
                            SocketMgr.Instance.RequestGetLeagueOpponentFormation(ActorData.getInstance().JoinLeagueInfo.leagueEntry, ActorData.getInstance().JoinLeagueInfo.groupId, opponent.userInfo.id);
                            ActorData.getInstance().IsCostStone = true;
                            this.<>f__this.mLockPkBtnEvent = true;
                        }
                    }
                }
            }, null, false);
        }

        internal void <>m__1A6(GameObject box)
        {
            if (ActorData.getInstance().Stone < 50)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2800));
            }
            else
            {
                object obj2 = GUIDataHolder.getData(this.go);
                if (obj2 != null)
                {
                    LeagueOpponent opponent = obj2 as LeagueOpponent;
                    if (opponent != null)
                    {
                        ActorData.getInstance().IsOnlyShowTargetTeam = false;
                        ActorData.getInstance().mCurrWorldCupEndTime = this.<>f__this.mCurrEndTime;
                        SocketMgr.Instance.RequestGetLeagueOpponentFormation(ActorData.getInstance().JoinLeagueInfo.leagueEntry, ActorData.getInstance().JoinLeagueInfo.groupId, opponent.userInfo.id);
                        ActorData.getInstance().IsCostStone = true;
                        this.<>f__this.mLockPkBtnEvent = true;
                    }
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickPreRewardItemBtn>c__AnonStorey175
    {
        internal string desc;

        internal void <>m__1A3(GUIEntity entity)
        {
            ((InfoDlag) entity).SetInfoCenterContent(this.desc);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickRewardItemBtn>c__AnonStorey174
    {
        internal treasure_config tc;

        internal void <>m__1A1(GUIEntity entity)
        {
            InfoDlag dlag = (InfoDlag) entity;
            string str = "[ff0000]" + this.tc.treasure_name;
            dlag.SetInfoCenterContent(str);
        }
    }

    private class LeagueMatchData
    {
        public int applyEnd;
        public int applyStart;
        public int entry;
        public int finalMatchEnd;
        public int finalMatchFinishEnd;
        public int groupEnd;
        public league_match_config lmc;
        public int preMatchEnd;
        public int PreMatchFinishEnd;
        public LeagueState state;
    }
}

