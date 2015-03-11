using System;
using UnityEngine;

public class MoveToPositon : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 Totarget;

    public void Update()
    {
        if (Vector3.Distance(base.transform.position, this.Totarget) <= 0.01f)
        {
            UnityEngine.Object.Destroy(this);
        }
        base.transform.position = Vector3.Lerp(base.transform.position, this.Totarget, this.speed * Time.deltaTime);
    }
}

