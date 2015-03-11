using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScenePosManagerImplBase_normalGame : ScenePosManagerImplBase
{
    private List<SceneMonsterPos> monsterPosList = new List<SceneMonsterPos>();

    public override Vector3 GetMonsterInitPos(int monsterIndex)
    {
        if (base.battleGameData.GetCurScenePhase() < this.monsterPosList.Count)
        {
            return this.monsterPosList[base.battleGameData.GetCurScenePhase()].GetPos(monsterIndex);
        }
        return this.monsterPosList[0].GetPos(monsterIndex);
    }

    public override Quaternion GetMonsterInitRot(int monsterIndex)
    {
        if (base.battleGameData.GetCurScenePhase() < this.monsterPosList.Count)
        {
            return this.monsterPosList[base.battleGameData.GetCurScenePhase()].transform.rotation;
        }
        return this.monsterPosList[0].transform.rotation;
    }

    public override Vector3 GetSceneFighterDirByPhase()
    {
        List<Vector3> scenePathByPhase = this.GetScenePathByPhase();
        Vector3 vector = scenePathByPhase[scenePathByPhase.Count - 1] - scenePathByPhase[scenePathByPhase.Count - 2];
        vector.Normalize();
        return vector;
    }

    public override Vector3 GetSceneFighterEndPosByPhase(int fighterIndex)
    {
        return this.GetSceneFighterPosByPhase(fighterIndex, -1);
    }

    private Vector3 GetSceneFighterPosByPhase(int fighterIndex, int posIndex)
    {
        List<Vector3> teamPos = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().teamPos;
        if (fighterIndex >= teamPos.Count)
        {
            return Vector3.zero;
        }
        Vector3 v = teamPos[fighterIndex];
        List<Vector3> scenePathByPhase = this.GetScenePathByPhase();
        posIndex = (posIndex >= 0) ? posIndex : (scenePathByPhase.Count - 1);
        return Matrix4x4.TRS(scenePathByPhase[posIndex], Quaternion.LookRotation(-this.GetSceneFighterDirByPhase()), Vector3.one).MultiplyPoint(v);
    }

    public override Vector3 GetSceneFighterStartPosByPhase(int fighterIndex)
    {
        return this.GetSceneFighterPosByPhase(fighterIndex, 0);
    }

    public override List<Vector3> GetScenePathByPhase()
    {
        BattleSceneOnePath[] playerMovePaths = BattleScenePath.Instance().playerMovePaths;
        int curScenePhase = base.battleGameData.GetCurScenePhase();
        if ((curScenePhase < playerMovePaths.Length) && (curScenePhase >= 0))
        {
            return playerMovePaths[curScenePhase].nodes;
        }
        return playerMovePaths[0].nodes;
    }

    public override void InitSceneInfo()
    {
        this.monsterPosList.Clear();
        for (int i = 0; i < BattleGlobal.PhaseMaxNumber; i++)
        {
            GameObject obj2 = GameObject.Find(BattleGlobal.MonsterPosSceneObjName + i.ToString());
            if (obj2 != null)
            {
                this.monsterPosList.Add(obj2.GetComponent<SceneMonsterPos>());
            }
        }
        this.sceneNamedPosManager = null;
        GameObject obj3 = GameObject.Find("BattleGroup");
        if (obj3 != null)
        {
            this.sceneNamedPosManager = obj3.GetComponent<SceneNamedPos>();
        }
        else
        {
            this.sceneNamedPosManager = new SceneNamedPos();
        }
    }

    private SceneNamedPos sceneNamedPosManager { get; set; }
}

