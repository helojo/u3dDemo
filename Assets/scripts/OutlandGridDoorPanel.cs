using System;
using UnityEngine;

public class OutlandGridDoorPanel : GUIEntity
{
    private Transform LeaveEnterBtn;
    private Transform RewardEnterBtn;

    private void MessageboxCanel()
    {
    }

    private void MessageboxOk(GameObject go)
    {
        SocketMgr.Instance.RequestOutlandEnterNextFloorReq(BattleState.GetInstance().CurGame.battleGameData.gridGameData.entry, BattleState.GetInstance().CurGame.battleGameData.gridGameData.map_entry, BattleState.GetInstance().CurGame.battleGameData.point);
        BattleState.GetInstance().CurGame.battleGameData.OnMsgLeave();
        GUIMgr.Instance.ExitModelGUI(base.name);
        BattleStaticEntry.ExitBattle();
        GameStateMgr.Instance.ChangeState("EXIT_OUTLAND_GRID_EVENT");
    }

    private void OnClickLeaveEnterBtn(GameObject go)
    {
    }

    private void OnClickRewardEnterBtn(GameObject go)
    {
        SocketMgr.Instance.RequestOutlandGetRewardReq(BattleState.GetInstance().CurGame.battleGameData.gridGameData.entry, BattleState.GetInstance().CurGame.battleGameData.gridGameData.map_entry, BattleState.GetInstance().CurGame.battleGameData.point);
    }

    public override void OnInitialize()
    {
        this.RewardEnterBtn = base.transform.FindChild("Reward/Enter");
        this.LeaveEnterBtn = base.transform.FindChild("Leave/Enter");
        UIEventListener.Get(this.RewardEnterBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRewardEnterBtn);
        UIEventListener.Get(this.LeaveEnterBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLeaveEnterBtn);
    }
}

