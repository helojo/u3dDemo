using FastBuf;
using System;
using UnityEngine;

public class BattleCom_PhaseManager : BattleCom_Base
{
    public static bool G_moveEnable = true;
    private BattleCom_PhaseManagerImplBase impl;

    public void BeginBattleFinished(bool isWin, bool isTimeOut)
    {
        this.impl.BeginBattleFinished(isWin, isTimeOut);
    }

    public void BeginPhaseFinishing()
    {
        this.impl.BeginPhaseFinishing();
    }

    public void BeginPhaseStarting()
    {
        this.impl.BeginPhaseStarting();
    }

    public void BeginShowBattleResult(S2C_DuplicateEndReq res)
    {
        this.impl.BeginShowBattleResult(res);
    }

    public void BlockScene()
    {
        this.impl.BlockScene();
    }

    public Vector3 ChangePosByBlock(Vector3 srcPos, Vector3 destPos)
    {
        return this.impl.ChangePosByBlock(srcPos, destPos);
    }

    public override void OnCreateInit()
    {
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgLeave = (System.Action) Delegate.Combine(battleGameData.OnMsgLeave, new System.Action(this.OnMsgLeave));
        if (base.battleGameData.gameType == BattleGameType.Normal)
        {
            this.impl = new BattleCom_PhaseNormal();
        }
        else if (base.battleGameData.gameType == BattleGameType.Grid)
        {
            this.impl = new BattleCom_PhaseGrid();
        }
        this.impl.owner = this;
        this.impl.battleGameData = base.battleGameData;
    }

    private void OnMsgLeave()
    {
        base.StopAllCoroutines();
    }

    public void UnblockScene()
    {
        this.impl.UnblockScene();
    }
}

