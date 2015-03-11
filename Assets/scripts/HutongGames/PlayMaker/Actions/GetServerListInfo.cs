namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Tooltip("Get ServerList Info"), ActionCategory("MTD")]
    public class GetServerListInfo : FsmStateAction
    {
        public FsmEvent failedEvent;
        public FsmEvent succeedEvent;

        public override void OnEnter()
        {
            WaitPanelHelper.ShowWaitPanel("GetServerListInfo");
            List<string> list = StrParser.ParseStringList(GameDefine.getInstance().url_ac_locojoy_com, ":");
            string host = CommonFunc.CheckHostIP(list[0], GameDefine.getInstance().url_webServer_ips);
            if (list.Count > 1)
            {
                host = host + ":" + list[1];
            }
            ServerInfo.getInstance().StartDownload(host, GameDefine.getInstance().gameId, GameDefine.getInstance().clientVersion, GameDefine.getInstance().clientChannel, delegate {
                WaitPanelHelper.HideWaitPanel("GetServerListInfo");
                PayMgr.Instance.Init();
                base.Fsm.Event(this.succeedEvent);
            }, delegate {
                WaitPanelHelper.HideWaitPanel("GetServerListInfo");
                GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(13), go => this.OnEnter(), null, true), null);
                base.Fsm.Event(this.failedEvent);
            });
        }

        public override void Reset()
        {
            this.failedEvent = null;
            this.succeedEvent = null;
        }
    }
}

