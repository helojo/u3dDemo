using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RealTimeBufferInfo : MonoBehaviour
{
    public string avatarModelName;
    public List<SkillEffectInfo> effectInfos = new List<SkillEffectInfo>();
    public float modelScale = 1f;
    public string sound;
}

