using FastBuf;
using System;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class RichActivityPanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache8;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache9;
    [CompilerGenerated]
    private static Action<object> <>f__am$cacheA;
    [CompilerGenerated]
    private static Action<object> <>f__am$cacheB;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheC;
    private Transform Fish;
    private float lastUpdateTime;
    private UILabel lb_fish_time;
    private UILabel lb_mining_time;
    private UILabel lb_tryTimes;
    private Transform Mining;
    private UISprite s_isOkFish;
    private UISprite s_isOkMine;

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
        SocketMgr.Instance.RequestC2S_NewLifeSkillMapSimpleData();
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = delegate (object s) {
                if (<>f__am$cacheC == null)
                {
                    <>f__am$cacheC = delegate (GUIEntity obj) {
                    };
                }
                GUIMgr.Instance.DoModelGUI<RichActivityRulePanel>(<>f__am$cacheC, null);
            };
        }
        base.FindChild<UIButton>("bt_rule").OnUIMouseClick(<>f__am$cacheB);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.Fish = base.transform.FindChild<Transform>("Fish");
        this.Mining = base.transform.FindChild<Transform>("Mining");
        base.transform.FindChild<UILabel>("TopLabel").text = ConfigMgr.getInstance().GetWord(0x2c24);
        base.transform.FindChild<UILabel>("lb_mining").text = ConfigMgr.getInstance().GetWord(0x2c26);
        this.s_isOkFish = base.FindChild<UISprite>("s_isOkFish");
        this.s_isOkMine = base.FindChild<UISprite>("s_isOkMine");
        base.transform.FindChild<UILabel>("lb_fish").text = ConfigMgr.getInstance().GetWord(0x2c25);
        bool flag = false;
        this.s_isOkFish.enabled = flag;
        this.s_isOkMine.enabled = flag;
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = o => GameStateMgr.Instance.ChangeState("LOAD_FISH_EVENT");
        }
        this.Fish.OnUIMouseClick(<>f__am$cache8);
        if (<>f__am$cache9 == null)
        {
            <>f__am$cache9 = o => GameStateMgr.Instance.ChangeState("LOAD_MINING_EVENT");
        }
        this.Mining.OnUIMouseClick(<>f__am$cache9);
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = o => GUIMgr.Instance.PopGUIEntity();
        }
        base.transform.FindChild<UIButton>("Close").OnUIMouseClick(<>f__am$cacheA);
        this.lb_tryTimes = base.FindChild<UILabel>("lb_tryTimes");
        this.lb_tryTimes.enabled = false;
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if ((this.lastUpdateTime + 1f) > Time.time)
        {
            this.lastUpdateTime = Time.time;
        }
        else
        {
            this.ShowActivityState();
        }
    }

    public void ShowActivityState()
    {
        this.s_isOkMine.enabled = XSingleton<LifeSkillManager>.Singleton.CanCollect(NewLifeSkillType.NEW_LIFE_SKILL_MINING);
        this.s_isOkFish.enabled = XSingleton<LifeSkillManager>.Singleton.CanCollect(NewLifeSkillType.NEW_LIFE_SKILL_FISHING);
    }
}

