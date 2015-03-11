using System;

[Serializable]
public class SceneEventProcesserSuperData
{
    public string AnimName;
    public bool comonEnable;
    public bool IsAllPlayer;
    public SceneEventProcesserLoopType loopType;
    public float maxRandomWaitTime;
    public float minRandomWaitTime;
    public string ModelName;
    public SceneEventMoveNode MoveNode;
    public string NewSceneObjName;
    public string SceneObjName;
    public int talkWordID;
    public float timeOfDuration;
    public SceneEventProcesserType type;
}

