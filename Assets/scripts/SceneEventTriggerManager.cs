using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneEventTriggerManager
{
    private static SceneEventTriggerManager _instance = new SceneEventTriggerManager();
    private List<SceneEventTriggerBase> triggerList = new List<SceneEventTriggerBase>();

    public void AddTrigger(SceneEventTriggerBase trigger)
    {
        this.triggerList.Add(trigger);
    }

    public void CheckTargetPosition(GameObject target)
    {
        <CheckTargetPosition>c__AnonStorey157 storey = new <CheckTargetPosition>c__AnonStorey157 {
            target = target
        };
        this.triggerList.ForEach(new Action<SceneEventTriggerBase>(storey.<>m__12C));
    }

    public static SceneEventTriggerManager Instance()
    {
        return _instance;
    }

    public void RemoveTrigger(SceneEventTriggerBase trigger)
    {
        this.triggerList.Remove(trigger);
    }

    [CompilerGenerated]
    private sealed class <CheckTargetPosition>c__AnonStorey157
    {
        internal GameObject target;

        internal void <>m__12C(SceneEventTriggerBase trigger)
        {
            if ((trigger != null) && trigger.enable)
            {
                trigger.Check(this.target);
            }
        }
    }
}

