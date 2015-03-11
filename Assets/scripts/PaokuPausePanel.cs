using FastBuf;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class PaokuPausePanel : GUIPanelEntity
{
    private int expendCount = -1;
    private guildrun_expend_config geResetConfig = new guildrun_expend_config();

    private void Continue()
    {
        GUIMgr.Instance.ExitModelGUI(base.name);
        ParkourManager._instance.Pause(false);
    }

    private void Exit()
    {
        GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x186d3), new UIEventListener.VoidDelegate(this.ExitMessageboxOk), null, false), null);
    }

    private void ExitMessageboxOk(GameObject go)
    {
        ActorData.getInstance().isLeavePaoku = true;
        Time.timeScale = 1f;
        ParkourManager._instance.DestoryParkourAsset();
        GameStateMgr.Instance.ChangeState("EXIT_PARKOUR_EVENT");
    }

    public override void Initialize()
    {
        base.Initialize();
        ArrayList list = ConfigMgr.getInstance().getList<guildrun_expend_config>();
        if (list != null)
        {
            this.expendCount = list.Count;
        }
        this.geResetConfig = ConfigMgr.getInstance().getByEntry<guildrun_expend_config>((int) ActorData.getInstance().mUserGuildMemberData.parkour_reset_times);
        UIEventListener listener1 = UIEventListener.Get(this.ibutton_continue.gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, go => this.Continue());
        UIEventListener listener2 = UIEventListener.Get(this.ibutton_exit.gameObject);
        listener2.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener2.onClick, go => this.Exit());
        UIEventListener listener3 = UIEventListener.Get(this.ibutton_replay.gameObject);
        listener3.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener3.onClick, go => this.Replay());
        if ((ActorData.getInstance().mUserGuildMemberData.parkour_reset_times + 1) > this.expendCount)
        {
            this.ibutton_replay.Disable(true);
            this.lb_max.gameObject.SetActive(true);
            this.group_stone.gameObject.SetActive(false);
        }
        else
        {
            this.ibutton_replay.Disable(false);
            this.lb_max.gameObject.SetActive(false);
            this.group_stone.gameObject.SetActive(true);
        }
        if (this.geResetConfig != null)
        {
            this.replay_stone_count.text = this.geResetConfig.restart_expend.ToString();
        }
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.ibutton_continue = base.FindChild<UIButton>("ibutton_continue");
        this.ibutton_exit = base.FindChild<UIButton>("ibutton_exit");
        this.ibutton_replay = base.FindChild<UIButton>("ibutton_replay");
        this.lb_max = base.FindChild<UILabel>("lb_max");
        this.group_stone = base.FindChild<Transform>("group_stone");
        this.replay_stone_count = base.FindChild<UILabel>("replay_stone_count");
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

    protected Transform group_stone { get; set; }

    protected UIButton ibutton_continue { get; set; }

    protected UIButton ibutton_exit { get; set; }

    protected UIButton ibutton_replay { get; set; }

    protected UILabel lb_max { get; set; }

    protected UILabel replay_stone_count { get; set; }
}

