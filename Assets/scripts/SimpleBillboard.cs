using System;
using UnityEngine;

public class SimpleBillboard : MonoBehaviour
{
    private Transform cachedTrans;
    private Transform camerTrans;

    private void FixedUpdate()
    {
        if (this.cachedTrans.rotation != this.camerTrans.rotation)
        {
            this.cachedTrans.rotation = this.camerTrans.rotation;
        }
    }

    private void Start()
    {
        this.cachedTrans = base.transform;
        this.camerTrans = Camera.main.transform;
    }
}

