using System;

[Serializable]
public class SceneEventTriggerSuperData
{
    public string battleIDText = string.Empty;
    public float radius = 1f;
    public int random = 100;
    public float triggerIntervalTime;
    public SceneEventTriggerType type;
}

