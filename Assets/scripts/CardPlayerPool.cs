using System;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayerPool
{
    private static CardPlayerPool _instance;
    private Dictionary<int, Queue<ObjSlot>> objDict = new Dictionary<int, Queue<ObjSlot>>();
    public static float RemainTime = 10f;
    private GameObject rootObj;

    public void ClearPool()
    {
        foreach (KeyValuePair<int, Queue<ObjSlot>> pair in this.objDict)
        {
            foreach (ObjSlot slot in pair.Value)
            {
                ObjectManager.DestoryObj(slot.obj);
            }
        }
        this.objDict.Clear();
    }

    private void Init()
    {
        this.rootObj = new GameObject("CardModelPool");
        UnityEngine.Object.DontDestroyOnLoad(this.rootObj);
    }

    public static CardPlayerPool Instance()
    {
        if (_instance == null)
        {
            _instance = new CardPlayerPool();
            _instance.Init();
        }
        return _instance;
    }

    public GameObject PopObj(int cardID)
    {
        Queue<ObjSlot> queue;
        if (!this.objDict.TryGetValue(cardID, out queue))
        {
            return null;
        }
        if (queue.Count <= 0)
        {
            return null;
        }
        GameObject obj2 = queue.Dequeue().obj;
        if (obj2 == null)
        {
            return null;
        }
        obj2.SetActive(true);
        obj2.SendMessage("ResetCardPlayer");
        return obj2;
    }

    public void PushObj(GameObject _obj)
    {
        if ((_obj != null) && (this.rootObj != null))
        {
            CardPlayer component = _obj.GetComponent<CardPlayer>();
            if ((component == null) || (_obj.transform.localScale != Vector3.one))
            {
                UnityEngine.Object.DestroyObject(_obj);
            }
            else
            {
                Queue<ObjSlot> queue;
                _obj.transform.parent = this.rootObj.transform;
                UnityEngine.Object.DontDestroyOnLoad(_obj);
                _obj.SetActive(false);
                int cardID = component.cardID;
                ObjSlot item = new ObjSlot {
                    startTime = Time.time,
                    obj = _obj
                };
                if (this.objDict.TryGetValue(cardID, out queue))
                {
                    queue.Enqueue(item);
                }
                else
                {
                    Queue<ObjSlot> queue2 = new Queue<ObjSlot>();
                    queue2.Enqueue(item);
                    this.objDict.Add(cardID, queue2);
                }
            }
        }
    }

    public void UpdatePool()
    {
        foreach (KeyValuePair<int, Queue<ObjSlot>> pair in this.objDict)
        {
            Queue<ObjSlot> queue = pair.Value;
            while (queue.Count > 0)
            {
                ObjSlot slot = queue.Peek();
                if ((slot.startTime + RemainTime) >= Time.time)
                {
                    break;
                }
                ObjectManager.DestoryObj(slot.obj);
                queue.Dequeue();
            }
        }
    }

    private class ObjSlot
    {
        public GameObject obj;
        public float startTime;
    }
}

