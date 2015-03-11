using System;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public bool cameraFollow;
    public float distance;
    public float hight;
    private Camera m_Camera;
    private Vector3 roation = new Vector3(15f, 0f, 0f);
    public Transform target;
    private Vector3 targetV3 = Vector3.zero;

    public void Init()
    {
        this.m_Camera = base.camera;
        this.target = ParkourManager._instance.cCtrl.characterTr;
    }

    private void LateUpdate()
    {
        if (this.cameraFollow)
        {
            this.targetV3 = Vector3.Lerp(this.m_Camera.transform.position, this.target.position, 0.5f);
            this.m_Camera.transform.position = new Vector3(this.target.position.x, this.target.position.y + this.hight, this.targetV3.z - this.distance);
        }
    }
}

