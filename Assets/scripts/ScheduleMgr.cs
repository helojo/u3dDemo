using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleMgr : MonoBehaviour
{
    private Queue<int> _delList = new Queue<int>();
    private IDictionary<int, TimeCallBackEntry> eventList = new Dictionary<int, TimeCallBackEntry>();
    public static ScheduleMgr Instance;
    private int NextId;

    private void Awake()
    {
        Instance = this;
    }

    private void cancel(int id)
    {
        this.eventList.Remove(id);
    }

    public static void Cancel(int id)
    {
        if (Instance != null)
        {
            Instance.cancel(id);
        }
    }

    public void Clear()
    {
        this.eventList.Clear();
        base.StopAllCoroutines();
    }

    private int generateNextId()
    {
        return this.NextId++;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void RunWork(IEnumerator worker)
    {
        base.StartCoroutine(worker);
    }

    private int schedule(float timeDelay, System.Action cb)
    {
        int num = this.generateNextId();
        this.eventList[num] = new TimeCallBackEntry(Time.realtimeSinceStartup + timeDelay, cb);
        return num;
    }

    [Obsolete("Use monobehaviore DelayCallBack ")]
    public static int Schedule(float timeDelay, System.Action cb)
    {
        if (Instance != null)
        {
            return Instance.schedule(timeDelay, cb);
        }
        return 0;
    }

    private void Start()
    {
    }

    private void Update()
    {
        while (this._delList.Count > 0)
        {
            this.eventList.Remove(this._delList.Dequeue());
        }
        IEnumerator<KeyValuePair<int, TimeCallBackEntry>> enumerator = this.eventList.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, TimeCallBackEntry> current = enumerator.Current;
                if (current.Value.expiredTime <= Time.realtimeSinceStartup)
                {
                    if (current.Value.cb != null)
                    {
                        current.Value.cb();
                    }
                    this._delList.Enqueue(current.Key);
                    return;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    private class TimeCallBackEntry
    {
        public System.Action cb;
        public float expiredTime;

        public TimeCallBackEntry(float expired, System.Action cbEvent)
        {
            this.expiredTime = expired;
            this.cb = cbEvent;
        }
    }
}

