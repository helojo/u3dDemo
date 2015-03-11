using FastBuf;
using HutongGames.PlayMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuildBattlePanel : GUIEntity
{
    public UIButton _guideBtn;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache8;
    [CompilerGenerated]
    private static Comparison<GuildBattleTargetInfo> <>f__am$cache9;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheA;
    private float m_interval = 1f;
    private bool m_IsStart;
    private List<GuildBattleTargetInfo> m_TargetInfoList = new List<GuildBattleTargetInfo>();
    private float m_times;
    private int mEndTime;
    public List<GameObject> TargetBtnList = new List<GameObject>();
    public List<GameObject> TargetInfoList = new List<GameObject>();

    private void BuyTimes()
    {
        <BuyTimes>c__AnonStorey201 storey = new <BuyTimes>c__AnonStorey201();
        int num = 20;
        int num2 = 4;
        int num3 = 30;
        int num4 = 3;
        int num5 = 3;
        int num6 = 50;
        storey.cost = (num + (((ActorData.getInstance().nCurAttackTimes + 1) - num2) * num3)) + ((((ActorData.getInstance().nCurAttackTimes + 1) - num4) / num5) * num6);
        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__354), base.gameObject);
    }

    private string GetGuildChairManIcon()
    {
        string str = string.Empty;
        List<GuildMember> list = new List<GuildMember>();
        foreach (GuildMember member in ActorData.getInstance().mGuildMemberData.member)
        {
            if (member.position == 1)
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(member.userInfo.head_entry);
                if (_config == null)
                {
                    Debug.LogWarning("Head entry is " + member.userInfo.head_entry);
                    return str;
                }
                return _config.image;
            }
        }
        return str;
    }

    private void GuideOnClick(GameObject go)
    {
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = delegate (GUIEntity obj) {
                GuildBattleRulePanel panel = (GuildBattleRulePanel) obj;
                panel.Depth = 800;
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildBattleRulePanel", <>f__am$cacheA, null);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
        if (ActorData.getInstance().m_entryFrom == 1)
        {
            SocketMgr.Instance.RequestGuildBattleInfo();
            ActorData.getInstance().m_entryFrom = 0;
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        if (this._guideBtn != null)
        {
            UIEventListener.Get(this._guideBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.GuideOnClick);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
        GuildPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
        if (null == gUIEntity)
        {
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = delegate (GUIEntity obj) {
                };
            }
            GUIMgr.Instance.PushGUIEntity("GuildPanel", <>f__am$cache8);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.m_IsStart)
        {
            this.m_times += Time.deltaTime;
            if (this.m_times >= this.m_interval)
            {
                long num = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
                base.gameObject.transform.FindChild("Top/time").GetComponent<UILabel>().text = TimeMgr.Instance.GetRemainTime(this.mEndTime);
                if (TimeMgr.Instance.GetRemainTime(this.mEndTime) == string.Empty)
                {
                    base.gameObject.transform.FindChild("Top/time").GetComponent<UILabel>().text = "00:00:00";
                    SocketMgr.Instance.RequestGuildBattleInfo();
                    this.m_IsStart = false;
                }
                this.m_times = 0f;
            }
        }
        int hour = TimeMgr.Instance.ServerDateTime.Hour;
        int minute = TimeMgr.Instance.ServerDateTime.Minute;
        int second = TimeMgr.Instance.ServerDateTime.Second;
        if ((((hour == 0x12) && (minute == 0)) && (second == 0)) || (((hour == 0x17) && (minute == 0)) && (second == 0)))
        {
            IEnumerator enumerator = ConfigMgr.getInstance().getList<guildbattle_time_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    guildbattle_time_config current = (guildbattle_time_config) enumerator.Current;
                    if (TimeMgr.Instance.ServerDateTime.DayOfWeek == current.open_weekday)
                    {
                        SocketMgr.Instance.RequestGuildBattleInfo();
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
    }

    private void OpenPlayerInfo()
    {
        <OpenPlayerInfo>c__AnonStorey200 storey = new <OpenPlayerInfo>c__AnonStorey200 {
            <>f__this = this
        };
        if (ActorData.getInstance().nCurAttackTimes == ActorData.getInstance().nTotalAttackTime)
        {
            this.BuyTimes();
        }
        else
        {
            PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
            if (component != null)
            {
                storey.type = component.FsmVariables.FindFsmInt("standType");
                GUIMgr.Instance.DoModelGUI("TargetTeamPanel", new Action<GUIEntity>(storey.<>m__353), base.gameObject);
            }
        }
    }

    public void UpdateBattleInfo(S2C_GuildBattleInfo res)
    {
        base.gameObject.transform.FindChild("left/bg2").gameObject.SetActive(res.battleStatus != 0);
        base.gameObject.transform.FindChild("right/bg1").gameObject.SetActive(res.battleStatus != 0);
        base.gameObject.transform.FindChild("right/bg2").gameObject.SetActive(res.battleStatus != 0);
        base.gameObject.transform.FindChild("start_time").gameObject.SetActive(res.battleStatus != 2);
        base.gameObject.transform.FindChild("map/mengban").gameObject.SetActive(res.battleStatus != 2);
        base.gameObject.transform.FindChild("map/vs").gameObject.SetActive(res.battleStatus == 0);
        base.gameObject.transform.FindChild("map/complete").gameObject.SetActive(res.battleStatus == 1);
        base.gameObject.transform.FindChild("remain_time").gameObject.SetActive(res.battleStatus == 2);
        base.gameObject.transform.FindChild("Top").gameObject.SetActive(res.battleStatus == 2);
        base.gameObject.transform.FindChild("left/result").gameObject.SetActive(res.battleStatus == 1);
        base.gameObject.transform.FindChild("right/result").gameObject.SetActive(res.battleStatus == 1);
        for (int i = 0; i < 5; i++)
        {
            this.TargetInfoList[i].SetActive(res.battleStatus == 2);
            this.TargetBtnList[i].SetActive(res.battleStatus == 2);
        }
        this.m_IsStart = res.battleStatus == 2;
        if (res.battleStatus == 0)
        {
            bool flag = false;
            base.gameObject.transform.FindChild("left/bg1/guild_name").GetComponent<UILabel>().text = ActorData.getInstance().mGuildData.name;
            base.gameObject.transform.FindChild("left/icon_bg/headicon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(this.GetGuildChairManIcon());
            IEnumerator enumerator = ConfigMgr.getInstance().getList<guildbattle_time_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    guildbattle_time_config current = (guildbattle_time_config) enumerator.Current;
                    if (TimeMgr.Instance.ServerDateTime.DayOfWeek == current.open_weekday)
                    {
                        int hour = TimeMgr.Instance.ServerDateTime.Hour;
                        int minute = TimeMgr.Instance.ServerDateTime.Minute;
                        int second = TimeMgr.Instance.ServerDateTime.Second;
                        if ((hour >= 0x12) && (hour <= 0x16))
                        {
                            flag = true;
                        }
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
            base.gameObject.transform.FindChild("never_fight").gameObject.SetActive(flag);
        }
        else if (res.battleStatus == 2)
        {
            base.gameObject.transform.FindChild("never_fight").gameObject.SetActive(false);
            this.m_TargetInfoList = res.enemys;
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = (t1, t2) => t1.resourceEntry - t2.resourceEntry;
            }
            this.m_TargetInfoList.Sort(<>f__am$cache9);
            base.gameObject.transform.FindChild("left/bg1/guild_name").GetComponent<UILabel>().text = ActorData.getInstance().mGuildData.name;
            base.gameObject.transform.FindChild("left/bg2/guild_point").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e3a) + " " + res.selfResource;
            base.gameObject.transform.FindChild("left/icon_bg/headicon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(this.GetGuildChairManIcon());
            base.gameObject.transform.FindChild("right/bg1/guild_name").GetComponent<UILabel>().text = res.targetGuildName;
            base.gameObject.transform.FindChild("right/bg2/guild_point").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e3a) + " " + res.targetResource;
            card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(res.targetHeadEntry);
            if (_config2 != null)
            {
                base.gameObject.transform.FindChild("right/icon_bg/headicon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
            }
            base.gameObject.transform.FindChild("right/icon_bg/headicon_no").gameObject.SetActive(false);
            ActorData.getInstance().nCurAttackTimes = res.curAttackTimes;
            ActorData.getInstance().nTotalAttackTime = res.totalAttackTimes;
            this.mEndTime = res.battleEndTime;
            if (this.m_IsStart)
            {
                long num5 = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
                base.gameObject.transform.FindChild("Top/time").GetComponent<UILabel>().text = TimeMgr.Instance.GetRemainTime(this.mEndTime);
            }
            this.UpdateTargetsInfo();
            this.UpdateGuildBattleTimesInfo();
        }
        else if (res.battleStatus == 1)
        {
            base.gameObject.transform.FindChild("never_fight").gameObject.SetActive(false);
            string str = string.Empty;
            string str2 = string.Empty;
            if (res.battleResult == GuildBattleResultType.GuildBattleResult_AllWin)
            {
                str = "Ui_Fuben_Icon_win";
                str2 = "Ui_Fuben_Icon_win";
            }
            else if (res.battleResult == GuildBattleResultType.GuildBattleResult_AllLose)
            {
                str = "Ui_Fuben_Icon_lose";
                str2 = "Ui_Fuben_Icon_lose";
            }
            else if (res.battleResult == GuildBattleResultType.GuildBattleResult_Win)
            {
                str = "Ui_Fuben_Icon_win";
                str2 = "Ui_Fuben_Icon_lose";
            }
            else
            {
                str = "Ui_Fuben_Icon_lose";
                str2 = "Ui_Fuben_Icon_win";
            }
            base.gameObject.transform.FindChild("left/result").GetComponent<UISprite>().spriteName = str;
            base.gameObject.transform.FindChild("right/result").GetComponent<UISprite>().spriteName = str2;
            base.gameObject.transform.FindChild("left/bg1/guild_name").GetComponent<UILabel>().text = ActorData.getInstance().mGuildData.name;
            base.gameObject.transform.FindChild("left/bg2/guild_point").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e3a) + " " + res.selfResource;
            base.gameObject.transform.FindChild("left/icon_bg/headicon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(this.GetGuildChairManIcon());
            base.gameObject.transform.FindChild("right/bg1/guild_name").GetComponent<UILabel>().text = res.targetGuildName;
            base.gameObject.transform.FindChild("right/bg2/guild_point").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e3a) + " " + res.targetResource;
            card_config _config3 = ConfigMgr.getInstance().getByEntry<card_config>(res.targetHeadEntry);
            if (_config3 != null)
            {
                base.gameObject.transform.FindChild("right/icon_bg/headicon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config3.image);
            }
            base.gameObject.transform.FindChild("right/icon_bg/headicon_no").gameObject.SetActive(false);
        }
    }

    public void UpdateGuildBattleTimesInfo()
    {
        int nCurAttackTimes = ActorData.getInstance().nCurAttackTimes;
        base.gameObject.transform.FindChild("LingQuBtn").gameObject.SetActive(nCurAttackTimes == ActorData.getInstance().nTotalAttackTime);
        base.gameObject.transform.FindChild("remain_time").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x4e40), ActorData.getInstance().nTotalAttackTime - ActorData.getInstance().nCurAttackTimes);
    }

    private void UpdateTargetsInfo()
    {
        foreach (GuildBattleTargetInfo info in this.m_TargetInfoList)
        {
            this.TargetInfoList[info.resourceEntry].transform.FindChild("bg1/count").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e3a) + " " + info.resourceCount;
        }
    }

    [CompilerGenerated]
    private sealed class <BuyTimes>c__AnonStorey201
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        internal int cost;

        internal void <>m__354(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = go => SocketMgr.Instance.RequestGuildBattleBuyTimes();
            }
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (GameObject go) {
                };
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x4e3c), this.cost), <>f__am$cache1, <>f__am$cache2, false);
        }

        private static void <>m__356(GameObject go)
        {
            SocketMgr.Instance.RequestGuildBattleBuyTimes();
        }

        private static void <>m__357(GameObject go)
        {
        }
    }

    [CompilerGenerated]
    private sealed class <OpenPlayerInfo>c__AnonStorey200
    {
        internal GuildBattlePanel <>f__this;
        internal FsmInt type;

        internal void <>m__353(GUIEntity obj)
        {
            ((TargetTeamPanel) obj).SetGuildBattleNodeInfo(this.<>f__this.m_TargetInfoList[this.type.Value]);
        }
    }
}

