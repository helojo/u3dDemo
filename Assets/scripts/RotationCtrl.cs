using System;
using UnityEngine;

public class RotationCtrl : MonoBehaviour
{
    public void Rot(GameObject obj)
    {
        obj.transform.localRotation = Quaternion.Euler(obj.transform.localRotation.eulerAngles.x, obj.transform.localRotation.eulerAngles.y + 180f, obj.transform.localRotation.eulerAngles.z);
    }
}

