using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_ScenePosManager : BattleCom_Base
{
    public Vector3 GetMonsterInitPos(int monsterIndex)
    {
        return this.impl.GetMonsterInitPos(monsterIndex);
    }

    public Quaternion GetMonsterInitRot(int monsterIndex)
    {
        return this.impl.GetMonsterInitRot(monsterIndex);
    }

    public Vector3 GetSceneFighterDirByPhase()
    {
        return this.impl.GetSceneFighterDirByPhase();
    }

    public Vector3 GetSceneFighterEndPosByPhase(int fighterIndex)
    {
        return this.impl.GetSceneFighterEndPosByPhase(fighterIndex);
    }

    public Vector3 GetSceneFighterStartPosByPhase(int fighterIndex)
    {
        return this.impl.GetSceneFighterStartPosByPhase(fighterIndex);
    }

    public List<Vector3> GetScenePathByPhase()
    {
        return this.impl.GetScenePathByPhase();
    }

    public void InitSceneInfo()
    {
        this.impl.InitSceneInfo();
    }

    public override void OnCreateInit()
    {
        if (base.battleGameData.gameType == BattleGameType.Normal)
        {
            this.impl = new ScenePosManagerImplBase_normalGame();
        }
        else if (base.battleGameData.gameType == BattleGameType.Grid)
        {
            this.impl = new BattleCom_ScenePosImplGrid();
        }
        else if (base.battleGameData.gameType == BattleGameType.Shower)
        {
            this.impl = new ScenePosManagerImplBase_ShowerGame();
        }
        this.impl.battleGameData = base.battleGameData;
    }

    public ScenePosManagerImplBase impl { get; set; }
}

