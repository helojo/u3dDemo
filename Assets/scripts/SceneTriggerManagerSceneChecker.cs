using System;
using UnityEngine;

public class SceneTriggerManagerSceneChecker : MonoBehaviour
{
    private void FixedUpdate()
    {
        SceneEventTriggerManager.Instance().CheckTargetPosition(null);
    }

    private void Start()
    {
    }
}

