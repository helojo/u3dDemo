using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RealTimeSkillInfo : MonoBehaviour
{
    public List<RealTimeSkillActionInfo> actionInfoes = new List<RealTimeSkillActionInfo>();
    public float afterTime = -1f;
    public float BreakTime;
    public bool isNeedTurn = true;
    public bool isTeamDirection;
    public bool oneByOne;
    public float ShowTimeTime;
    public string testBufferName;
    public float time = -1f;
}

