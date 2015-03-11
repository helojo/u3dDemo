using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class ScenePosManagerImplBase
{
    protected ScenePosManagerImplBase()
    {
    }

    public abstract Vector3 GetMonsterInitPos(int monsterIndex);
    public abstract Quaternion GetMonsterInitRot(int monsterIndex);
    public abstract Vector3 GetSceneFighterDirByPhase();
    public abstract Vector3 GetSceneFighterEndPosByPhase(int fighterIndex);
    public abstract Vector3 GetSceneFighterStartPosByPhase(int fighterIndex);
    public abstract List<Vector3> GetScenePathByPhase();
    public abstract void InitSceneInfo();

    public BattleData battleGameData { get; set; }
}

