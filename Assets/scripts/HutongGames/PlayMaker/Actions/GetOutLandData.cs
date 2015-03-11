namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Get outland data"), ActionCategory("MTD")]
    public class GetOutLandData : FsmStateAction
    {
        public override void OnEnter()
        {
            ActorData.getInstance().bOpenOutlandTitleInfo = true;
            SocketMgr.Instance.RequestOutlandsData(null, () => base.Finish());
        }

        public override void OnUpdate()
        {
        }
    }
}

