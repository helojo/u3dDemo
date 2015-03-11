using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleCom_ScenePosImplGrid : ScenePosManagerImplBase
{
    private GameObject battleCenterObj;

    public override Vector3 GetMonsterInitPos(int monsterIndex)
    {
        if (base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetMonsterFighters().Count > 1)
        {
            int num = monsterIndex + 1;
            return this.battleCenterObj.transform.FindChild("pos" + num.ToString()).position;
        }
        return this.battleCenterObj.transform.FindChild("pos0").position;
    }

    public override Quaternion GetMonsterInitRot(int monsterIndex)
    {
        return Quaternion.LookRotation(-this.battleCenterObj.transform.forward);
    }

    public override Vector3 GetSceneFighterDirByPhase()
    {
        return this.battleCenterObj.transform.forward;
    }

    public override Vector3 GetSceneFighterEndPosByPhase(int fighterIndex)
    {
        return this.GetSceneFighterStartPosByPhase(fighterIndex);
    }

    public override Vector3 GetSceneFighterStartPosByPhase(int fighterIndex)
    {
        List<Vector3> teamPos = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().teamPos;
        if (fighterIndex >= teamPos.Count)
        {
            return Vector3.zero;
        }
        Vector3 v = teamPos[fighterIndex];
        return Matrix4x4.TRS(this.battleCenterObj.transform.position, Quaternion.LookRotation(-this.GetSceneFighterDirByPhase()), Vector3.one).MultiplyPoint(v);
    }

    public override List<Vector3> GetScenePathByPhase()
    {
        return null;
    }

    public override void InitSceneInfo()
    {
        this.battleCenterObj = GameObject.Find("battle_center");
    }

    public void SetGridBattleType(bool isBoss)
    {
        if (isBoss)
        {
            this.battleCenterObj = GameObject.Find("battle_center_boss");
        }
        else
        {
            this.battleCenterObj = GameObject.Find("battle_center");
        }
    }
}

