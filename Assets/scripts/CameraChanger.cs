using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraChanger : MonoBehaviour
{
    public List<Vector3> cameraOffsets = new List<Vector3>();
    private Vector3 curOffset;
    public float curZoom;
    private GameObject lookAt;
    private Vector3 offsetV;
    public float smoothTime = 1f;
    private Vector3 targetOffset;

    public void SetCamera(int index, bool isDirectSet)
    {
        if ((index >= 0) && (index < this.cameraOffsets.Count))
        {
            if (isDirectSet)
            {
                this.curOffset = this.targetOffset = this.cameraOffsets[index];
            }
            else
            {
                this.targetOffset = this.cameraOffsets[index];
            }
        }
    }

    public void SetLookAt(GameObject obj)
    {
        this.lookAt = obj;
    }

    private void Update()
    {
        if (this.curOffset != this.targetOffset)
        {
            this.curOffset.x = Mathf.SmoothDamp(this.curOffset.x, this.targetOffset.x, ref this.offsetV.x, this.smoothTime);
            this.curOffset.y = Mathf.SmoothDamp(this.curOffset.y, this.targetOffset.y, ref this.offsetV.y, this.smoothTime);
            this.curOffset.z = Mathf.SmoothDamp(this.curOffset.z, this.targetOffset.z, ref this.offsetV.z, this.smoothTime);
        }
        if (this.lookAt != null)
        {
            base.transform.position = this.lookAt.transform.position + this.curOffset;
            base.transform.LookAt(this.lookAt.transform.position);
        }
        else
        {
            base.transform.position = this.curOffset;
        }
    }

    public void Zoom(float delta)
    {
        this.curZoom += delta * 0.01f;
        this.curZoom = Mathf.Clamp01(this.curZoom);
        if (this.cameraOffsets.Count >= 2)
        {
            this.targetOffset = Vector3.Lerp(this.cameraOffsets[0], this.cameraOffsets[1], this.curZoom);
        }
    }
}

