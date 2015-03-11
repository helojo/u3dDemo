using System;
using UnityEngine;

public class SceneEventTriggerRandomTime : SceneEventTriggerBase
{
    private float lastTriggerTime;

    public override void Check(GameObject target)
    {
        if ((this.lastTriggerTime + base.superData.triggerIntervalTime) < Time.time)
        {
            this.lastTriggerTime = Time.time;
            base.OnBaseCheckTrigger();
        }
    }
}

