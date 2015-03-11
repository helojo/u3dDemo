using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class SceneEventTriggerBase
{
    protected SceneEventTriggerBase()
    {
    }

    public abstract void Check(GameObject target);
    public static SceneEventTriggerBase CreateTrigger(SceneEventTriggerSuperData data, SceneEventControler owner)
    {
        SceneEventTriggerBase base2 = null;
        SceneEventTriggerType type = data.type;
        if (type == SceneEventTriggerType.Position)
        {
            base2 = new SceneEventTriggerPosition();
        }
        else if (type == SceneEventTriggerType.RandomTime)
        {
            base2 = new SceneEventTriggerRandomTime();
        }
        else
        {
            Debug.LogError("CreateTrigger Error ");
        }
        if (base2 != null)
        {
            base2.enable = true;
            base2.superData = data;
            base2.ownerControl = owner;
            base2.DoAdd();
        }
        return base2;
    }

    public void DoAdd()
    {
        SceneEventTriggerManager.Instance().AddTrigger(this);
    }

    public void DoRemove()
    {
        SceneEventTriggerManager.Instance().RemoveTrigger(this);
    }

    public void OnBaseCheckTrigger()
    {
    }

    public void OnTrigger()
    {
        this.enable = false;
        this.ownerControl.OnTrigger();
    }

    public bool enable { get; set; }

    public SceneEventControler ownerControl { get; set; }

    public SceneEventTriggerSuperData superData { get; set; }
}

