using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventCenter : MonoBehaviour
{
    private EventHandler[] allEventHandler = new EventHandler[7];
    public static EventCenter Instance;

    public void DoEvent(EventType type, object arg)
    {
        try
        {
            if (this.allEventHandler[(int) type] != null)
            {
                this.allEventHandler[(int) type](arg);
            }
        }
        catch (Exception)
        {
        }
    }

    private void Start()
    {
        Instance = this;
    }

    public EventHandler this[EventType type]
    {
        get
        {
            return this.allEventHandler[(int) type];
        }
        set
        {
            this.allEventHandler[(int) type] = value;
        }
    }

    public delegate void EventHandler(object arg);

    public enum EventType
    {
        None,
        UIShow_Recruit,
        UIShow_Recruit_Result,
        UIShow_Dupmap,
        UIShow_Trench,
        UIShow_DupLevelInfo,
        BackTo_MainUI,
        Count
    }
}

