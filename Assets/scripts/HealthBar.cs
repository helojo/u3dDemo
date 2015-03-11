using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject mountPoint;
    private Vector3 offset = new Vector3(0f, 0.2f, 0f);

    private void Start()
    {
    }

    private void Update()
    {
        if (this.mountPoint != null)
        {
            Vector3 position = this.mountPoint.transform.position + this.offset;
            position = Camera.main.WorldToViewportPoint(position);
            int activeHeight = UIRoot.list[0].activeHeight;
            position.x = ((position.x - 0.5f) * (((float) Screen.width) / ((float) Screen.height))) * activeHeight;
            position.y = (position.y - 0.5f) * activeHeight;
            base.transform.localPosition = position;
        }
    }
}

