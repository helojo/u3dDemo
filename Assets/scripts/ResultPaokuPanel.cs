using FastBuf;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class ResultPaokuPanel : GUIPanelEntity
{
    public GuildParkour _guildParkour;
    public static ResultPaokuPanel _instance;
    public GameObject _TipsDiag;
    private int expendCount = -1;
    private guildrun_config gdConfig;
    private guildrun_expend_config geResetConfig;
    private guildrun_expend_config geReviveConfig;

    private void Awake()
    {
        _instance = this;
    }

    public void InitData(S2C_ParkourEnd data)
    {
        if (data.star.Count == 3)
        {
            guildrun_config gdConfig = this.gdConfig;
            if (gdConfig != null)
            {
                if (data.star[0])
                {
                    this.star1.gameObject.SetActive(true);
                    this.star1Lose.gameObject.SetActive(false);
                    this.lb_coin_count1.text = gdConfig.escape_gold_award.ToString();
                    this.sp_win.gameObject.SetActive(true);
                    this.sp_faile.gameObject.SetActive(false);
                    this.Total.gameObject.SetActive(true);
                    this.btn_resurrection.gameObject.SetActive(false);
                    this.btn_replay.gameObject.SetActive(false);
                }
                else
                {
                    this.sp_win.gameObject.SetActive(false);
                    this.sp_faile.gameObject.SetActive(true);
                    this.star1.gameObject.SetActive(false);
                    this.star1Lose.gameObject.SetActive(true);
                    this.Total.gameObject.SetActive(false);
                    this.lb_coin_count1Lose.text = gdConfig.escape_gold_award.ToString();
                    this.btn_resurrection.gameObject.SetActive(true);
                    this.btn_replay.gameObject.SetActive(true);
                    guildrun_expend_config _config2 = ConfigMgr.getInstance().getByEntry<guildrun_expend_config>((int) data.user_data.parkour_reset_times);
                    guildrun_expend_config _config3 = ConfigMgr.getInstance().getByEntry<guildrun_expend_config>((int) data.user_data.parkour_revive_times);
                    if (_config2 != null)
                    {
                        this.replay_stone_count.text = _config2.restart_expend.ToString();
                    }
                    if (_config3 != null)
                    {
                        this.resurre_stone_count.text = _config3.resurrection_expend.ToString();
                    }
                }
                if (data.star[1])
                {
                    this.star2.gameObject.SetActive(true);
                    this.star2Lose.gameObject.SetActive(false);
                    this.lb_coin_count2.text = gdConfig.noharm_gold_award.ToString();
                }
                else
                {
                    this.star2.gameObject.SetActive(false);
                    this.star2Lose.gameObject.SetActive(true);
                    this.lb_coin_count2Lose.text = gdConfig.noharm_gold_award.ToString();
                }
                if (data.star[2])
                {
                    this.star3.gameObject.SetActive(true);
                    this.star3Lose.gameObject.SetActive(false);
                    this.lb_coin_count3.text = gdConfig.allchest_gold_award.ToString();
                }
                else
                {
                    this.star3.gameObject.SetActive(false);
                    this.star3Lose.gameObject.SetActive(true);
                    this.lb_coin_count3Lose.text = gdConfig.allchest_gold_award.ToString();
                }
                this.lb_coin_total.text = data.addGold.ToString();
            }
        }
    }

    public void InitDataByResult(GuildParkour gpParkour)
    {
        this._guildParkour = gpParkour;
        Debug.Log("paoku end box " + gpParkour.box);
        Debug.Log("paoku end coin " + gpParkour.coin);
        Debug.Log("paoku end time " + gpParkour.time);
        Debug.Log("paoku end trap_count " + gpParkour.trap_count);
        Debug.Log("paoku end result " + gpParkour.result);
        if (this._guildParkour != null)
        {
            ActorData.getInstance().isPaokuWin = this._guildParkour.result;
            if (this._guildParkour.result)
            {
                SocketMgr.Instance.RequestParkourEnd(gpParkour);
            }
            else if (this.gdConfig != null)
            {
                this.sp_win.gameObject.SetActive(false);
                this.sp_faile.gameObject.SetActive(true);
                this.Total.gameObject.SetActive(false);
                this.btn_resurrection.gameObject.SetActive(true);
                this.btn_replay.gameObject.SetActive(true);
                this.star1.gameObject.SetActive(false);
                this.star1Lose.gameObject.SetActive(true);
                this.lb_coin_count1Lose.text = this.gdConfig.escape_gold_award.ToString();
                this.star2.gameObject.SetActive(false);
                this.star2Lose.gameObject.SetActive(true);
                this.lb_coin_count2Lose.text = this.gdConfig.noharm_gold_award.ToString();
                this.star3.gameObject.SetActive(false);
                this.star3Lose.gameObject.SetActive(true);
                this.lb_coin_count3Lose.text = this.gdConfig.allchest_gold_award.ToString();
                if ((ActorData.getInstance().mUserGuildMemberData.parkour_reset_times + 1) > this.expendCount)
                {
                    this.btn_replay.Disable(true);
                    this.lb_max_replay.gameObject.SetActive(true);
                    this.group_stone_replay.gameObject.SetActive(false);
                }
                else
                {
                    this.btn_replay.Disable(false);
                    this.lb_max_replay.gameObject.SetActive(false);
                    this.group_stone_replay.gameObject.SetActive(true);
                }
                if ((ActorData.getInstance().mUserGuildMemberData.parkour_revive_times + 1) > this.expendCount)
                {
                    this.btn_resurrection.Disable(true);
                    this.lb_max_resurr.gameObject.SetActive(true);
                    this.group_stone_resurr.gameObject.SetActive(false);
                }
                else
                {
                    this.btn_resurrection.Disable(false);
                    this.lb_max_resurr.gameObject.SetActive(false);
                    this.group_stone_resurr.gameObject.SetActive(true);
                }
                if (this.geResetConfig != null)
                {
                    this.replay_stone_count.text = this.geResetConfig.restart_expend.ToString();
                }
                if (this.geReviveConfig != null)
                {
                    this.resurre_stone_count.text = this.geReviveConfig.resurrection_expend.ToString();
                }
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        UIEventListener listener1 = UIEventListener.Get(this.btn_return.gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, go => this.Return());
        ArrayList list = ConfigMgr.getInstance().getList<guildrun_expend_config>();
        if (list != null)
        {
            this.expendCount = list.Count;
        }
        this.geResetConfig = ConfigMgr.getInstance().getByEntry<guildrun_expend_config>((int) ActorData.getInstance().mUserGuildMemberData.parkour_reset_times);
        this.geReviveConfig = ConfigMgr.getInstance().getByEntry<guildrun_expend_config>((int) ActorData.getInstance().mUserGuildMemberData.parkour_revive_times);
        UIEventListener listener2 = UIEventListener.Get(this.btn_replay.gameObject);
        listener2.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener2.onClick, go => this.Replay());
        UIEventListener listener3 = UIEventListener.Get(this.btn_resurrection.gameObject);
        listener3.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener3.onClick, go => this.Resurre());
        this.gdConfig = ConfigMgr.getInstance().getByEntry<guildrun_config>(ActorData.getInstance().paokuMapEntry);
        Transform transform = base.transform.FindChild("star1");
        Transform transform2 = base.transform.FindChild("star2");
        Transform transform3 = base.transform.FindChild("star3");
        Transform transform4 = base.transform.FindChild("star1Lose");
        Transform transform5 = base.transform.FindChild("star2Lose");
        Transform transform6 = base.transform.FindChild("star3Lose");
        GUIDataHolder.setData(transform.gameObject, 1);
        GUIDataHolder.setData(transform2.gameObject, 2);
        GUIDataHolder.setData(transform3.gameObject, 3);
        GUIDataHolder.setData(transform4.gameObject, 1);
        GUIDataHolder.setData(transform5.gameObject, 2);
        GUIDataHolder.setData(transform6.gameObject, 3);
        UIEventListener.Get(transform.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressStart);
        UIEventListener.Get(transform2.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressStart);
        UIEventListener.Get(transform3.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressStart);
        UIEventListener.Get(transform4.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressStart);
        UIEventListener.Get(transform5.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressStart);
        UIEventListener.Get(transform6.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressStart);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.sp_win = base.FindChild<UISprite>("sp_win");
        this.btn_return = base.FindChild<UIButton>("btn_return");
        this.star1 = base.FindChild<Transform>("star1");
        this.lb_desc1 = base.FindChild<UILabel>("lb_desc1");
        this.sp_star1 = base.FindChild<UISprite>("sp_star1");
        this.sp_ok1 = base.FindChild<UISprite>("sp_ok1");
        this.sp_coin1 = base.FindChild<UISprite>("sp_coin1");
        this.lb_coin_count1 = base.FindChild<UILabel>("lb_coin_count1");
        this.star2 = base.FindChild<Transform>("star2");
        this.lb_desc2 = base.FindChild<UILabel>("lb_desc2");
        this.sp_star2 = base.FindChild<UISprite>("sp_star2");
        this.sp_ok2 = base.FindChild<UISprite>("sp_ok2");
        this.sp_coin2 = base.FindChild<UISprite>("sp_coin2");
        this.lb_coin_count2 = base.FindChild<UILabel>("lb_coin_count2");
        this.star3 = base.FindChild<Transform>("star3");
        this.lb_desc3 = base.FindChild<UILabel>("lb_desc3");
        this.sp_star3 = base.FindChild<UISprite>("sp_star3");
        this.sp_ok3 = base.FindChild<UISprite>("sp_ok3");
        this.sp_coin3 = base.FindChild<UISprite>("sp_coin3");
        this.lb_coin_count3 = base.FindChild<UILabel>("lb_coin_count3");
        this.Total = base.FindChild<Transform>("Total");
        this.lb_coin_total = base.FindChild<UILabel>("lb_coin_total");
        this.star1Lose = base.FindChild<Transform>("star1Lose");
        this.lb_desc1Lose = base.FindChild<UILabel>("lb_desc1Lose");
        this.sp_star1Lose = base.FindChild<UISprite>("sp_star1Lose");
        this.sp_coin1Lose = base.FindChild<UISprite>("sp_coin1Lose");
        this.lb_coin_count1Lose = base.FindChild<UILabel>("lb_coin_count1Lose");
        this.star2Lose = base.FindChild<Transform>("star2Lose");
        this.lb_desc2Lose = base.FindChild<UILabel>("lb_desc2Lose");
        this.sp_star2Lose = base.FindChild<UISprite>("sp_star2Lose");
        this.sp_coin2Lose = base.FindChild<UISprite>("sp_coin2Lose");
        this.lb_coin_count2Lose = base.FindChild<UILabel>("lb_coin_count2Lose");
        this.star3Lose = base.FindChild<Transform>("star3Lose");
        this.lb_desc3Lose = base.FindChild<UILabel>("lb_desc3Lose");
        this.sp_star3Lose = base.FindChild<UISprite>("sp_star3Lose");
        this.sp_coin3Lose = base.FindChild<UISprite>("sp_coin3Lose");
        this.lb_coin_count3Lose = base.FindChild<UILabel>("lb_coin_count3Lose");
        this.sp_faile = base.FindChild<UISprite>("sp_faile");
        this.btn_resurrection = base.FindChild<UIButton>("btn_resurrection");
        this.group_stone_resurr = base.FindChild<Transform>("group_stone_resurr");
        this.resurre_stone_count = base.FindChild<UILabel>("resurre_stone_count");
        this.lb_max_resurr = base.FindChild<UILabel>("lb_max_resurr");
        this.btn_replay = base.FindChild<UIButton>("btn_replay");
        this.group_stone_replay = base.FindChild<Transform>("group_stone_replay");
        this.replay_stone_count = base.FindChild<UILabel>("replay_stone_count");
        this.lb_max_replay = base.FindChild<UILabel>("lb_max_replay");
    }

    private void OnPressStart(GameObject go, bool isPress)
    {
        if (this._TipsDiag != null)
        {
            if (!isPress)
            {
                this._TipsDiag.gameObject.SetActive(false);
            }
            else
            {
                object obj2 = GUIDataHolder.getData(go);
                if (obj2 != null)
                {
                    int num = (int) obj2;
                    UILabel component = this._TipsDiag.transform.FindChild("Label").GetComponent<UILabel>();
                    switch (num)
                    {
                        case 1:
                            component.text = ConfigMgr.getInstance().GetWord(0x89d);
                            break;

                        case 2:
                            component.text = ConfigMgr.getInstance().GetWord(0x89e);
                            break;

                        case 3:
                            component.text = ConfigMgr.getInstance().GetWord(0x89f);
                            break;
                    }
                    this._TipsDiag.transform.parent = go.transform;
                    this._TipsDiag.transform.localPosition = Vector3.zero;
                    this._TipsDiag.gameObject.SetActive(true);
                }
            }
        }
    }

    private void Replay()
    {
        if (this.geResetConfig != null)
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x186d2), this.geResetConfig.restart_expend), new UIEventListener.VoidDelegate(this.ReplayMessageboxOk), null, false), null);
        }
    }

    private void ReplayMessageboxOk(GameObject go)
    {
        SocketMgr.Instance.RequestParkourReset(ActorData.getInstance().paokuMapEntry, ActorData.getInstance().paokuCardEntry);
    }

    private void Resurre()
    {
        if (this.geReviveConfig != null)
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x186d4), this.geReviveConfig.resurrection_expend), new UIEventListener.VoidDelegate(this.ResurreMessageboxOk), null, false), null);
        }
    }

    private void ResurreMessageboxOk(GameObject go)
    {
        SocketMgr.Instance.RequestParkourRevive(ActorData.getInstance().paokuMapEntry);
    }

    private void Return()
    {
        if (this._guildParkour != null)
        {
            if (this._guildParkour.result)
            {
                ActorData.getInstance().isLeavePaoku = true;
                Time.timeScale = 1f;
                ParkourManager._instance.DestoryParkourAsset();
                GameStateMgr.Instance.ChangeState("EXIT_PARKOUR_EVENT");
            }
            else
            {
                SocketMgr.Instance.RequestParkourEnd(this._guildParkour);
            }
        }
    }

    protected UIButton btn_replay { get; set; }

    protected UIButton btn_resurrection { get; set; }

    protected UIButton btn_return { get; set; }

    protected Transform group_stone_replay { get; set; }

    protected Transform group_stone_resurr { get; set; }

    protected UILabel lb_coin_count1 { get; set; }

    protected UILabel lb_coin_count1Lose { get; set; }

    protected UILabel lb_coin_count2 { get; set; }

    protected UILabel lb_coin_count2Lose { get; set; }

    protected UILabel lb_coin_count3 { get; set; }

    protected UILabel lb_coin_count3Lose { get; set; }

    protected UILabel lb_coin_total { get; set; }

    protected UILabel lb_desc1 { get; set; }

    protected UILabel lb_desc1Lose { get; set; }

    protected UILabel lb_desc2 { get; set; }

    protected UILabel lb_desc2Lose { get; set; }

    protected UILabel lb_desc3 { get; set; }

    protected UILabel lb_desc3Lose { get; set; }

    protected UILabel lb_max_replay { get; set; }

    protected UILabel lb_max_resurr { get; set; }

    protected UILabel replay_stone_count { get; set; }

    protected UILabel resurre_stone_count { get; set; }

    protected UISprite sp_coin1 { get; set; }

    protected UISprite sp_coin1Lose { get; set; }

    protected UISprite sp_coin2 { get; set; }

    protected UISprite sp_coin2Lose { get; set; }

    protected UISprite sp_coin3 { get; set; }

    protected UISprite sp_coin3Lose { get; set; }

    protected UISprite sp_faile { get; set; }

    protected UISprite sp_ok1 { get; set; }

    protected UISprite sp_ok2 { get; set; }

    protected UISprite sp_ok3 { get; set; }

    protected UISprite sp_star1 { get; set; }

    protected UISprite sp_star1Lose { get; set; }

    protected UISprite sp_star2 { get; set; }

    protected UISprite sp_star2Lose { get; set; }

    protected UISprite sp_star3 { get; set; }

    protected UISprite sp_star3Lose { get; set; }

    protected UISprite sp_win { get; set; }

    protected Transform star1 { get; set; }

    protected Transform star1Lose { get; set; }

    protected Transform star2 { get; set; }

    protected Transform star2Lose { get; set; }

    protected Transform star3 { get; set; }

    protected Transform star3Lose { get; set; }

    protected Transform Total { get; set; }
}

