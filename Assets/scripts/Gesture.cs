using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Gesture
{
    public float actionTime;
    public float deltaPinch;
    public Vector2 deltaPosition;
    public float deltaTime;
    public int fingerIndex;
    public bool isGuiCamera;
    public bool isHoverReservedArea;
    public GameObject otherReceiver;
    public Camera pickCamera;
    public GameObject pickObject;
    public Vector2 position;
    public Vector2 startPosition;
    public EasyTouch.SwipeType swipe;
    public float swipeLength;
    public Vector2 swipeVector;
    public int touchCount;
    public float twistAngle;
    public float twoFingerDistance;

    public float GetSwipeOrDragAngle()
    {
        return (Mathf.Atan2(this.swipeVector.normalized.y, this.swipeVector.normalized.x) * 57.29578f);
    }

    public Vector3 GetTouchToWordlPoint(float z, bool worldZ = false)
    {
        if (!worldZ)
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(this.position.x, this.position.y, z));
        }
        return Camera.main.ScreenToWorldPoint(new Vector3(this.position.x, this.position.y, z - Camera.main.transform.position.z));
    }

    public bool IsInRect(Rect rect, bool guiRect = false)
    {
        if (guiRect)
        {
            rect = new Rect(rect.x, (Screen.height - rect.y) - rect.height, rect.width, rect.height);
        }
        return rect.Contains(this.position);
    }

    public Vector2 NormalizedPosition()
    {
        return new Vector2(((100f / ((float) Screen.width)) * this.position.x) / 100f, ((100f / ((float) Screen.height)) * this.position.y) / 100f);
    }
}

