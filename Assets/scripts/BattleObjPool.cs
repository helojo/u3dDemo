using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleObjPool
{
    private int objQueueRemainNumber;
    private float objRemainTime;
    private Dictionary<string, Queue<ObjSlot>> objsDict;

    public BattleObjPool()
    {
        this.objRemainTime = 10f;
        this.objQueueRemainNumber = 1;
        this.objsDict = new Dictionary<string, Queue<ObjSlot>>();
    }

    public BattleObjPool(float _objRemianTime, int _objQueueRemainNumber) : this()
    {
        this.objRemainTime = _objRemianTime;
        this.objQueueRemainNumber = _objQueueRemainNumber;
    }

    public void Clear()
    {
        foreach (KeyValuePair<string, Queue<ObjSlot>> pair in this.objsDict)
        {
            foreach (ObjSlot slot in pair.Value)
            {
                UnityEngine.Object.DestroyObject(slot.obj);
            }
        }
        this.objsDict.Clear();
    }

    public GameObject PullObj(string name)
    {
        Queue<ObjSlot> queue;
        if (this.objsDict.TryGetValue(name, out queue) && (queue.Count > 0))
        {
            GameObject obj2 = queue.Dequeue().obj;
            obj2.SetActive(true);
            return obj2;
        }
        return null;
    }

    public void PushGameObj(string name, GameObject obj)
    {
        Queue<ObjSlot> queue;
        ObjSlot slot;
        if (!this.objsDict.TryGetValue(name, out queue))
        {
            queue = new Queue<ObjSlot>();
            this.objsDict.Add(name, queue);
        }
        slot.obj = obj;
        slot.lastUseTime = Time.time;
        Vector3 localScale = obj.transform.localScale;
        obj.transform.parent = null;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = localScale;
        obj.SetActive(false);
        if (obj.particleSystem != null)
        {
            obj.particleSystem.Clear(true);
        }
        queue.Enqueue(slot);
    }

    public void Update()
    {
        foreach (KeyValuePair<string, Queue<ObjSlot>> pair in this.objsDict)
        {
            Queue<ObjSlot> queue = pair.Value;
            while (queue.Count > this.objQueueRemainNumber)
            {
                ObjSlot slot = queue.Peek();
                if (Time.time <= (slot.lastUseTime + this.objRemainTime))
                {
                    break;
                }
                UnityEngine.Object.DestroyObject(queue.Dequeue().obj);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ObjSlot
    {
        public GameObject obj;
        public float lastUseTime;
    }
}

