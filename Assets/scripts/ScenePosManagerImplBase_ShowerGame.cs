using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenePosManagerImplBase_ShowerGame : ScenePosManagerImplBase
{
    public override Vector3 GetMonsterInitPos(int monsterIndex)
    {
        return Vector3.zero;
    }

    public override Quaternion GetMonsterInitRot(int monsterIndex)
    {
        return Quaternion.identity;
    }

    public override Vector3 GetSceneFighterDirByPhase()
    {
        Transform transform = base.battleGameData.battleComObject.gameObject.transform.FindChild("pos_" + 0);
        if (transform != null)
        {
            return transform.forward;
        }
        return Vector3.forward;
    }

    public override Vector3 GetSceneFighterEndPosByPhase(int fighterIndex)
    {
        return this.GetSceneFighterStartPosByPhase(fighterIndex);
    }

    public override Vector3 GetSceneFighterStartPosByPhase(int fighterIndex)
    {
        Transform transform = base.battleGameData.battleComObject.gameObject.transform.FindChild("pos_" + fighterIndex);
        if (transform != null)
        {
            return transform.position;
        }
        return Vector3.zero;
    }

    public override List<Vector3> GetScenePathByPhase()
    {
        return null;
    }

    public override void InitSceneInfo()
    {
    }
}

