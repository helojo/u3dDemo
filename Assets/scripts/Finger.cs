using System;
using UnityEngine;

public class Finger
{
    public Vector2 complexStartPosition;
    public Vector2 deltaPosition;
    public float deltaTime;
    public int fingerIndex;
    public EasyTouch.GestureType gesture;
    public bool isGuiCamera;
    public Vector2 oldPosition;
    public TouchPhase phase;
    public Camera pickedCamera;
    public GameObject pickedObject;
    public Vector2 position;
    public Vector2 startPosition;
    public int tapCount;
    public int touchCount;
}

