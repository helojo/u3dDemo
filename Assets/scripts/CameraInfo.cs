using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraInfo
{
    public GameObject anim;
    public float defaultZoom;
    public List<Vector3> diffZoom = new List<Vector3>();
    public Vector3 dir;
    public Vector3 dirOffsetMax = Vector3.one;
    public Vector3 dirOffsetMin = -Vector3.one;
    public float fov = 45f;
    public Vector3 lookAtOffset;
    public float zoomMax;
    public float zoomMin;

    public void Copy(CameraInfo other)
    {
        this.dir = other.dir;
        this.defaultZoom = other.defaultZoom;
        this.lookAtOffset = other.lookAtOffset;
        this.fov = other.fov;
        this.dirOffsetMin = other.dirOffsetMin;
        this.dirOffsetMax = other.dirOffsetMax;
        this.anim = other.anim;
        this.zoomMin = other.zoomMin;
        this.zoomMax = other.zoomMax;
        this.diffZoom = other.diffZoom;
    }

    public float GetZoom()
    {
        float num = ((float) Screen.width) / ((float) Screen.height);
        foreach (Vector3 vector in this.diffZoom)
        {
            if (num <= ((vector.x / vector.y) + 0.01))
            {
                return (vector.z + this.defaultZoom);
            }
        }
        return this.defaultZoom;
    }
}

