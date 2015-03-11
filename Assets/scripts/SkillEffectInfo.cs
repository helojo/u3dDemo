using System;
using UnityEngine;

[Serializable]
public class SkillEffectInfo
{
    public SkillEffectAttachType attachType;
    public float delayDestoryOnFinish = 0.5f;
    public float delayTime;
    public float distance = 10f;
    public float durtTime = 1f;
    public GameObject effectPrefab;
    public HangPointType hangPoint;
    public bool isDestoryOnFinish;
    public bool isDestoryOnSkillFinish;
    public Vector3 offset = Vector3.zero;
}

