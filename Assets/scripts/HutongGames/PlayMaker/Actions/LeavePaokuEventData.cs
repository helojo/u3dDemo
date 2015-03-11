namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Leave PaokuEvent Data"), ActionCategory("MTD")]
    public class LeavePaokuEventData : FsmStateAction
    {
        public override void OnEnter()
        {
            if (ActorData.getInstance().isLeavePaoku)
            {
                SocketMgr.Instance.RequestGuildData(true, () => base.Finish());
            }
            else
            {
                GUIMgr.Instance.PushGUIEntity("GuildPanel", delegate (GUIEntity objs) {
                    GUIMgr.Instance.PushGUIEntity("PaokuPanel", null);
                    base.Finish();
                });
            }
        }

        public override void OnUpdate()
        {
        }
    }
}

