using System;
using UnityEngine;

[Serializable]
public class SceneEventMoveNode
{
    public bool isSetPosMode;
    public bool isWalkMode = true;
    public Vector3 pos;
    public float speed = 10f;
}

