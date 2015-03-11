using System;
using UnityEngine;

[AddComponentMenu("MTD/BattleScenePath")]
public class BattleScenePath : MonoBehaviour
{
    private static BattleScenePath _Instance;
    public bool isShowPath = true;
    public BattleSceneOnePath[] playerMovePaths = new BattleSceneOnePath[BattleGlobal.PhaseMaxNumber];

    private void Awake()
    {
        _Instance = this;
    }

    public Vector3[] GetPlayerMovePathByPhase(int phase)
    {
        return this.playerMovePaths[phase].nodes.ToArray();
    }

    public static BattleScenePath Instance()
    {
        if (_Instance == null)
        {
            Debug.LogWarning("BattleScenePath is null , so use find");
            GameObject obj2 = GameObject.Find("BattleGroup");
            if (obj2 != null)
            {
                _Instance = obj2.GetComponent<BattleScenePath>();
            }
        }
        return _Instance;
    }

    private void OnDestroy()
    {
        _Instance = null;
    }

    private void OnDrawGizmos()
    {
        if (this.isShowPath)
        {
            foreach (BattleSceneOnePath path in this.playerMovePaths)
            {
                if (path.nodes.Count > 1)
                {
                    iTween.DrawPathGizmos(path.nodes.ToArray(), Color.blue);
                    if (path.exitNodes.Count > 1)
                    {
                        iTween.DrawPathGizmos(path.exitNodes.ToArray(), Color.red);
                    }
                    GameObject parent = Resources.Load(BattleGlobal.DefaultTeamPosName) as GameObject;
                    if (parent != null)
                    {
                        Matrix4x4 matrixx = new Matrix4x4();
                        matrixx.SetTRS(path.nodes[0], Quaternion.LookRotation(path.nodes[1] - path.nodes[0]), Vector3.one);
                        GameObject obj3 = BattleGlobalFunc.FindChildObjectByName(parent, "pos_0");
                        if (obj3 != null)
                        {
                            Gizmos.color = Color.red;
                            Vector3 position = obj3.transform.position;
                            Gizmos.DrawWireCube(matrixx.MultiplyPoint(position), Vector3.one);
                        }
                    }
                }
            }
        }
    }

    public void SetPlayerMovePathBeginPos(int phase, Vector3 pos)
    {
        if ((phase < this.playerMovePaths.Length) && (this.playerMovePaths[phase] != null))
        {
            this.playerMovePaths[phase].nodes[0] = pos;
        }
    }

    public void SetPlayerMovePathEndPos(int phase, Vector3 pos)
    {
        this.playerMovePaths[phase].nodes[this.playerMovePaths[phase].nodes.Count - 1] = pos;
    }
}

