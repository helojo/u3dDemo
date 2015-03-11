using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleSceneOnePath
{
    public List<Vector3> exitNodes = new List<Vector3>();
    public List<Vector3> nodes = new List<Vector3> { Vector3.zero, Vector3.zero };

    public Vector3 GetBeginPos()
    {
        return this.nodes[0];
    }

    public Vector3[] GetExitPath()
    {
        return this.exitNodes.ToArray();
    }

    public Vector3[] GetPath()
    {
        return this.nodes.ToArray();
    }
}

