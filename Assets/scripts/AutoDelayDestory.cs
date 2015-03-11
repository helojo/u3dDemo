using System;
using UnityEngine;

public class AutoDelayDestory : MonoBehaviour
{
    public float delay = 100f;
    public float startTime;

    public void Start()
    {
        this.startTime = Time.time;
    }

    public void Update()
    {
        if ((this.startTime + this.delay) < Time.time)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }
}

