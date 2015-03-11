using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("MTD/SceneMonsterPos")]
public class SceneMonsterPos : MonoBehaviour
{
    public bool isAutoUpdatePath = true;
    public bool isAutoUpdatePathNext = true;
    public PathSlot[] paths = new PathSlot[SlotNumber - 1];
    public static int PlayerPosSlotIndex = 6;
    public static int SlotNumber = 7;
    [HideInInspector]
    public Vector3[] slots = new Vector3[SlotNumber];
    [HideInInspector]
    public Vector3[] slots_WorldPos = new Vector3[SlotNumber];
    private Matrix4x4 translationLocalToWorld;

    private void Awake()
    {
        this.UpdateMatrix();
    }

    public void ClearPath()
    {
        for (int i = 0; i < this.paths.Length; i++)
        {
            PathSlot slot = this.paths[i];
            slot.nodes.Clear();
        }
    }

    private void DrawTeamPosDebug(Vector3 pos)
    {
    }

    public List<Vector3> GetPath(int index)
    {
        if (((index < this.paths.Length) && (this.paths[index] != null)) && (this.paths[index].nodes.Count > 0))
        {
            <GetPath>c__AnonStorey158 storey = new <GetPath>c__AnonStorey158 {
                <>f__this = this,
                result = new List<Vector3>()
            };
            this.paths[index].nodes.ForEach(new Action<Vector3>(storey.<>m__12D));
            storey.result.Add(this.GetPos(index));
            return storey.result;
        }
        return null;
    }

    public Vector3 GetPos(int index)
    {
        return this.slots_WorldPos[index];
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < (SlotNumber - 1); i++)
        {
            List<Vector3> path = this.GetPath(i);
            if ((path != null) && (path.Count > 0))
            {
                iTween.DrawPathGizmos(path.ToArray(), Color.green);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < SlotNumber; i++)
        {
            Gizmos.DrawWireCube(this.GetPos(i), Vector3.one);
        }
    }

    public void SetDefaultPath()
    {
        Matrix4x4 matrixx = new Matrix4x4();
        matrixx.SetTRS(Vector3.zero, base.gameObject.transform.rotation, Vector3.one);
        for (int i = 0; i < this.paths.Length; i++)
        {
            PathSlot slot = this.paths[i];
            slot.nodes.Clear();
            Vector3 v = new Vector3(0f, 0f, -1f);
            v = matrixx.MultiplyVector(v);
            Vector3 vector2 = this.GetPos(i) + ((Vector3) (v * 5f));
            slot.nodes.Add(vector2 - base.transform.position);
        }
    }

    public void SetWorldPos(int index, Vector3 worldPos)
    {
        this.slots[index] = this.translationLocalToWorld.inverse.MultiplyPoint(worldPos);
    }

    public void UpdateMatrix()
    {
        this.translationLocalToWorld.SetTRS(base.gameObject.transform.position, base.gameObject.transform.rotation, Vector3.one);
        for (int i = 0; i < SlotNumber; i++)
        {
            this.slots_WorldPos[i] = this.translationLocalToWorld.MultiplyPoint(this.slots[i]);
        }
    }

    public void UpdatePlayerMovePath()
    {
        if (this.isAutoUpdatePath)
        {
            int phase = -1;
            for (int i = 0; i < BattleGlobal.PhaseMaxNumber; i++)
            {
                if (base.gameObject.name.EndsWith(i.ToString()))
                {
                    phase = i;
                    break;
                }
            }
            if ((phase >= 0) && (BattleScenePath.Instance() != null))
            {
                BattleScenePath.Instance().SetPlayerMovePathEndPos(phase, this.GetPos(PlayerPosSlotIndex));
                if (this.isAutoUpdatePathNext)
                {
                    int num3 = phase + 1;
                    if (num3 < BattleGlobal.PhaseMaxNumber)
                    {
                        BattleScenePath.Instance().SetPlayerMovePathBeginPos(num3, this.GetPos(PlayerPosSlotIndex));
                    }
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <GetPath>c__AnonStorey158
    {
        internal SceneMonsterPos <>f__this;
        internal List<Vector3> result;

        internal void <>m__12D(Vector3 obj)
        {
            this.result.Add(obj + this.<>f__this.transform.position);
        }
    }

    [Serializable]
    public class PathSlot
    {
        public List<Vector3> nodes = new List<Vector3>();
    }
}

