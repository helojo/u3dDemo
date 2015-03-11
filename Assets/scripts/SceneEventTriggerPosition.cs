using System;
using UnityEngine;

public class SceneEventTriggerPosition : SceneEventTriggerBase
{
    public override void Check(GameObject target)
    {
        if ((target != null) && (Vector3.Distance(target.transform.position, base.ownerControl.gameObject.transform.position) < base.superData.radius))
        {
            base.OnBaseCheckTrigger();
        }
    }
}

