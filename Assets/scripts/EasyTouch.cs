using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class EasyTouch : MonoBehaviour
{
    [CompilerGenerated]
    private static Predicate<ECamera> <>f__am$cache59;
    public bool autoSelect;
    private GestureType complexCurrentGesture = GestureType.None;
    public bool enable = true;
    public bool enable2D;
    public bool enable2FingersGesture = true;
    public bool enabledNGuiMode;
    public bool enablePinch = true;
    public bool enableRemote;
    public bool enableReservedArea = true;
    public bool enableTwist = true;
    private Finger[] fingers = new Finger[10];
    public bool FlexAnimationLocked;
    public bool HighPriorityLocked;
    private EasyTouchInput input;
    public static EasyTouch instance;
    public bool isExtension;
    private bool isStartHoverNGUI;
    private bool locked;
    public float longTapTime = 1f;
    public float minPinchLength;
    public float minTwistAngle = 1f;
    public List<Camera> nGUICameras = new List<Camera>();
    public LayerMask nGUILayers;
    private float oldFingerDistance;
    private GestureType oldGesture = GestureType.None;
    private GameObject oldPickObject2Finger;
    private Vector2 oldStartPosition2Finger;
    private int oldTouchCount;
    public LayerMask pickableLayers;
    public LayerMask pickableLayers2D;
    private GameObject pickObject2Finger;
    public GameObject receiverObject;
    public List<Rect> reservedAreas = new List<Rect>();
    public List<Rect> reservedGuiAreas = new List<Rect>();
    public List<Rect> reservedVirtualAreas = new List<Rect>();
    public Texture secondFingerTexture;
    public bool showGeneral = true;
    public bool showGesture = true;
    public bool showSecondFinger = true;
    public bool showSelect = true;
    public bool showTwoFinger = true;
    private Vector2 startPosition2Finger;
    private float startTimeAction;
    public float StationnaryTolerance = 25f;
    public KeyCode swipeKey = KeyCode.LeftControl;
    public float swipeTolerance = 0.85f;
    public List<ECamera> touchCameras = new List<ECamera>();
    public KeyCode twistKey = KeyCode.LeftAlt;
    private int twoFinger0;
    private int twoFinger1;
    private bool twoFingerDragStart;
    private bool twoFingerSwipeStart;
    public bool useBroadcastMessage = true;

    public static  event TouchCancelHandler On_Cancel;

    public static  event Cancel2FingersHandler On_Cancel2Fingers;

    public static  event DoubleTapHandler On_DoubleTap;

    public static  event DoubleTap2FingersHandler On_DoubleTap2Fingers;

    public static  event DragHandler On_Drag;

    public static  event Drag2FingersHandler On_Drag2Fingers;

    public static  event DragEndHandler On_DragEnd;

    public static  event DragEnd2FingersHandler On_DragEnd2Fingers;

    public static  event DragStartHandler On_DragStart;

    public static  event DragStart2FingersHandler On_DragStart2Fingers;

    public static  event EasyTouchIsReadyHandler On_EasyTouchIsReady;

    public static  event LongTapHandler On_LongTap;

    public static  event LongTap2FingersHandler On_LongTap2Fingers;

    public static  event LongTapEndHandler On_LongTapEnd;

    public static  event LongTapEnd2FingersHandler On_LongTapEnd2Fingers;

    public static  event LongTapStartHandler On_LongTapStart;

    public static  event LongTapStart2FingersHandler On_LongTapStart2Fingers;

    public static  event PinchEndHandler On_PinchEnd;

    public static  event PinchInHandler On_PinchIn;

    public static  event PinchOutHandler On_PinchOut;

    public static  event SimpleTapHandler On_SimpleTap;

    public static  event SimpleTap2FingersHandler On_SimpleTap2Fingers;

    public static  event SwipeHandler On_Swipe;

    public static  event Swipe2FingersHandler On_Swipe2Fingers;

    public static  event SwipeEndHandler On_SwipeEnd;

    public static  event SwipeEnd2FingersHandler On_SwipeEnd2Fingers;

    public static  event SwipeStartHandler On_SwipeStart;

    public static  event SwipeStart2FingersHandler On_SwipeStart2Fingers;

    public static  event TouchDownHandler On_TouchDown;

    public static  event TouchDown2FingersHandler On_TouchDown2Fingers;

    public static  event TouchStartHandler On_TouchStart;

    public static  event TouchStart2FingersHandler On_TouchStart2Fingers;

    public static  event TouchUpHandler On_TouchUp;

    public static  event TouchUp2FingersHandler On_TouchUp2Fingers;

    public static  event TwistHandler On_Twist;

    public static  event TwistEndHandler On_TwistEnd;

    public EasyTouch()
    {
        this.enable = true;
        this.useBroadcastMessage = false;
        this.enable2FingersGesture = true;
        this.enableTwist = true;
        this.enablePinch = true;
        this.autoSelect = false;
        this.StationnaryTolerance = 25f;
        this.longTapTime = 1f;
        this.swipeTolerance = 0.85f;
        this.minPinchLength = 0f;
        this.minTwistAngle = 1f;
    }

    public static void AddReservedArea(Rect rec)
    {
        if (instance != null)
        {
            instance.reservedAreas.Add(rec);
        }
    }

    public static void AddReservedGuiArea(Rect rec)
    {
        if (instance != null)
        {
            instance.reservedGuiAreas.Add(rec);
        }
    }

    private void CreateGesture(int touchIndex, EventName message, Finger finger, float actionTime, SwipeType swipe, float swipeLength, Vector2 swipeVector)
    {
        if ((message == EventName.On_TouchStart) || (message == EventName.On_TouchUp))
        {
            this.isStartHoverNGUI = this.IsTouchHoverNGui(touchIndex);
        }
        if (!this.isStartHoverNGUI)
        {
            Gesture gesture = new Gesture {
                fingerIndex = finger.fingerIndex,
                touchCount = finger.touchCount,
                startPosition = finger.startPosition,
                position = finger.position,
                deltaPosition = finger.deltaPosition,
                actionTime = actionTime,
                deltaTime = finger.deltaTime,
                swipe = swipe,
                swipeLength = swipeLength,
                swipeVector = swipeVector,
                deltaPinch = 0f,
                twistAngle = 0f,
                pickObject = finger.pickedObject,
                otherReceiver = this.receiverObject,
                isHoverReservedArea = this.IsTouchReservedArea(touchIndex),
                pickCamera = finger.pickedCamera,
                isGuiCamera = finger.isGuiCamera
            };
            if (this.useBroadcastMessage)
            {
                this.SendGesture(message, gesture);
            }
            if (!this.useBroadcastMessage || this.isExtension)
            {
                this.RaiseEvent(message, gesture);
            }
        }
    }

    private void CreateGesture2Finger(EventName message, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float actionTime, SwipeType swipe, float swipeLength, Vector2 swipeVector, float twist, float pinch, float twoDistance)
    {
        if (message == EventName.On_TouchStart2Fingers)
        {
            this.isStartHoverNGUI = this.IsTouchHoverNGui(this.twoFinger1) & this.IsTouchHoverNGui(this.twoFinger0);
        }
        if (!this.isStartHoverNGUI)
        {
            Gesture gesture = new Gesture {
                touchCount = 2,
                fingerIndex = -1,
                startPosition = startPosition,
                position = position,
                deltaPosition = deltaPosition,
                actionTime = actionTime
            };
            if (this.fingers[this.twoFinger0] != null)
            {
                gesture.deltaTime = this.fingers[this.twoFinger0].deltaTime;
            }
            else if (this.fingers[this.twoFinger1] != null)
            {
                gesture.deltaTime = this.fingers[this.twoFinger1].deltaTime;
            }
            else
            {
                gesture.deltaTime = 0f;
            }
            gesture.swipe = swipe;
            gesture.swipeLength = swipeLength;
            gesture.swipeVector = swipeVector;
            gesture.deltaPinch = pinch;
            gesture.twistAngle = twist;
            gesture.twoFingerDistance = twoDistance;
            if (this.fingers[this.twoFinger0] != null)
            {
                gesture.pickCamera = this.fingers[this.twoFinger0].pickedCamera;
                gesture.isGuiCamera = this.fingers[this.twoFinger0].isGuiCamera;
            }
            else if (this.fingers[this.twoFinger1] != null)
            {
                gesture.pickCamera = this.fingers[this.twoFinger1].pickedCamera;
                gesture.isGuiCamera = this.fingers[this.twoFinger1].isGuiCamera;
            }
            if (message != EventName.On_Cancel2Fingers)
            {
                gesture.pickObject = this.pickObject2Finger;
            }
            else
            {
                gesture.pickObject = this.oldPickObject2Finger;
            }
            gesture.otherReceiver = this.receiverObject;
            if (this.fingers[this.twoFinger0] != null)
            {
                gesture.isHoverReservedArea = this.IsTouchReservedArea(this.fingers[this.twoFinger0].fingerIndex);
            }
            if (this.fingers[this.twoFinger1] != null)
            {
                gesture.isHoverReservedArea = gesture.isHoverReservedArea || this.IsTouchReservedArea(this.fingers[this.twoFinger1].fingerIndex);
            }
            if (this.useBroadcastMessage)
            {
                this.SendGesture2Finger(message, gesture);
            }
            else
            {
                this.RaiseEvent(message, gesture);
            }
        }
    }

    private void CreateStateEnd2Fingers(GestureType gesture, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float time, bool realEnd, float fingerDistance)
    {
        switch (gesture)
        {
            case GestureType.Tap:
                if ((this.fingers[this.twoFinger0].tapCount >= 2) || (this.fingers[this.twoFinger1].tapCount >= 2))
                {
                    this.CreateGesture2Finger(EventName.On_DoubleTap2Fingers, startPosition, position, deltaPosition, time, SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
                    break;
                }
                this.CreateGesture2Finger(EventName.On_SimpleTap2Fingers, startPosition, position, deltaPosition, time, SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
                break;

            case GestureType.LongTap:
                this.CreateGesture2Finger(EventName.On_LongTapEnd2Fingers, startPosition, position, deltaPosition, time, SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
                break;

            case GestureType.Pinch:
                this.CreateGesture2Finger(EventName.On_PinchEnd, startPosition, position, deltaPosition, time, SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
                break;

            case GestureType.Twist:
                this.CreateGesture2Finger(EventName.On_TwistEnd, startPosition, position, deltaPosition, time, SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
                break;
        }
        if (realEnd)
        {
            if (this.twoFingerDragStart)
            {
                Vector2 vector = position - startPosition;
                this.CreateGesture2Finger(EventName.On_DragEnd2Fingers, startPosition, position, deltaPosition, time, this.GetSwipe(startPosition, position), vector.magnitude, position - startPosition, 0f, 0f, fingerDistance);
            }
            if (this.twoFingerSwipeStart)
            {
                Vector2 vector2 = position - startPosition;
                this.CreateGesture2Finger(EventName.On_SwipeEnd2Fingers, startPosition, position, deltaPosition, time, this.GetSwipe(startPosition, position), vector2.magnitude, position - startPosition, 0f, 0f, fingerDistance);
            }
            this.CreateGesture2Finger(EventName.On_TouchUp2Fingers, startPosition, position, deltaPosition, time, SwipeType.None, 0f, Vector2.zero, 0f, 0f, fingerDistance);
        }
    }

    private float DeltaAngle(Vector2 start, Vector2 end)
    {
        float y = (start.x * end.y) - (start.y * end.x);
        return Mathf.Atan2(y, Vector2.Dot(start, end));
    }

    private bool FingerInTolerance(Finger finger)
    {
        Vector2 vector = finger.position - finger.startPosition;
        return (vector.sqrMagnitude <= (this.StationnaryTolerance * this.StationnaryTolerance));
    }

    public static Camera GetCamera(int index = 0)
    {
        if (index < instance.touchCameras.Count)
        {
            return instance.touchCameras[index].camera;
        }
        return null;
    }

    public static GameObject GetCurrentPickedObject(int fingerIndex)
    {
        Finger finger = instance.GetFinger(fingerIndex);
        if (instance.GetPickeGameObject(ref finger, false))
        {
            return finger.pickedObject;
        }
        return null;
    }

    public static bool GetEnable2FingersGesture()
    {
        return instance.enable2FingersGesture;
    }

    public static bool GetEnableAutoSelect()
    {
        return instance.autoSelect;
    }

    public static bool GetEnabled()
    {
        return instance.enable;
    }

    public static bool GetEnablePinch()
    {
        return instance.enablePinch;
    }

    public static bool GetEnableTwist()
    {
        return instance.enableTwist;
    }

    private Finger GetFinger(int finderId)
    {
        int index = 0;
        Finger finger = null;
        while ((index < 10) && (finger == null))
        {
            if ((this.fingers[index] != null) && (this.fingers[index].fingerIndex == finderId))
            {
                finger = this.fingers[index];
            }
            index++;
        }
        return finger;
    }

    public static Vector2 GetFingerPosition(int fingerIndex)
    {
        if (instance.fingers[fingerIndex] != null)
        {
            return instance.GetFinger(fingerIndex).position;
        }
        return Vector2.zero;
    }

    public static bool GetIsReservedArea()
    {
        return ((instance != null) && instance.enableReservedArea);
    }

    public static float GetlongTapTime()
    {
        return instance.longTapTime;
    }

    public static float GetMinPinchLength()
    {
        return instance.minPinchLength;
    }

    public static float GetMinTwistAngle()
    {
        return instance.minTwistAngle;
    }

    public static GameObject GetOtherReceiverObject()
    {
        return instance.receiverObject;
    }

    public static LayerMask GetPickableLayer()
    {
        return instance.pickableLayers;
    }

    private bool GetPickeGameObject(ref Finger finger, bool twoFinger = false)
    {
        finger.isGuiCamera = false;
        finger.pickedCamera = null;
        finger.pickedObject = null;
        if (this.touchCameras.Count > 0)
        {
            for (int i = 0; i < this.touchCameras.Count; i++)
            {
                if ((this.touchCameras[i].camera != null) && this.touchCameras[i].camera.enabled)
                {
                    RaycastHit hit;
                    Vector2 zero = Vector2.zero;
                    if (!twoFinger)
                    {
                        zero = finger.startPosition;
                    }
                    else
                    {
                        zero = finger.complexStartPosition;
                    }
                    Ray ray = this.touchCameras[i].camera.ScreenPointToRay((Vector3) zero);
                    if (this.enable2D)
                    {
                        LayerMask mask = this.pickableLayers2D;
                        RaycastHit2D[] results = new RaycastHit2D[1];
                        if (Physics2D.GetRayIntersectionNonAlloc(ray, results, float.PositiveInfinity, (int) mask) > 0)
                        {
                            finger.pickedCamera = this.touchCameras[i].camera;
                            finger.isGuiCamera = this.touchCameras[i].guiCamera;
                            finger.pickedObject = results[0].collider.gameObject;
                            return true;
                        }
                    }
                    LayerMask pickableLayers = this.pickableLayers;
                    if (Physics.Raycast(ray, out hit, float.MaxValue, (int) pickableLayers))
                    {
                        finger.pickedCamera = this.touchCameras[i].camera;
                        finger.isGuiCamera = this.touchCameras[i].guiCamera;
                        finger.pickedObject = hit.collider.gameObject;
                        return true;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No camera is assigned to EasyTouch");
        }
        return false;
    }

    public static float GetStationnaryTolerance()
    {
        return instance.StationnaryTolerance;
    }

    private SwipeType GetSwipe(Vector2 start, Vector2 end)
    {
        Vector2 vector2 = end - start;
        Vector2 normalized = vector2.normalized;
        float introduced2 = Mathf.Abs(normalized.y);
        if (introduced2 > Mathf.Abs(normalized.x))
        {
            if (Vector2.Dot(normalized, Vector2.up) >= this.swipeTolerance)
            {
                return SwipeType.Up;
            }
            if (Vector2.Dot(normalized, -Vector2.up) >= this.swipeTolerance)
            {
                return SwipeType.Down;
            }
        }
        else
        {
            if (Vector2.Dot(normalized, Vector2.right) >= this.swipeTolerance)
            {
                return SwipeType.Right;
            }
            if (Vector2.Dot(normalized, -Vector2.right) >= this.swipeTolerance)
            {
                return SwipeType.Left;
            }
        }
        return SwipeType.Other;
    }

    public static float GetSwipeTolerance()
    {
        return instance.swipeTolerance;
    }

    public static int GetTouchCount()
    {
        if (instance != null)
        {
            return instance.input.TouchCount();
        }
        return 0;
    }

    private int GetTwoFinger(int index)
    {
        int num = index + 1;
        bool flag = false;
        while ((num < 10) && !flag)
        {
            if ((this.fingers[num] != null) && (num >= index))
            {
                flag = true;
            }
            num++;
        }
        num--;
        return num;
    }

    private void InitEasyTouch()
    {
        this.input = new EasyTouchInput();
        if (instance == null)
        {
            instance = this;
        }
    }

    public static bool IsRectUnderTouch(Rect rect, bool guiRect = false)
    {
        bool flag = false;
        for (int i = 0; i < 10; i++)
        {
            if (instance.fingers[i] != null)
            {
                if (guiRect)
                {
                    rect = new Rect(rect.x, (Screen.height - rect.y) - rect.height, rect.width, rect.height);
                }
                flag = rect.Contains(instance.fingers[i].position);
                if (flag)
                {
                    return flag;
                }
            }
        }
        return flag;
    }

    private bool IsTouchHoverNGui(int touchIndex)
    {
        bool flag = false;
        if (this.enabledNGuiMode)
        {
            LayerMask nGUILayers = this.nGUILayers;
            for (int i = 0; !flag && (i < this.nGUICameras.Count); i++)
            {
                RaycastHit hit;
                flag = Physics.Raycast(this.nGUICameras[i].ScreenPointToRay((Vector3) this.fingers[touchIndex].position), out hit, float.MaxValue, (int) nGUILayers);
            }
        }
        return flag;
    }

    private bool IsTouchReservedArea(int touchIndex)
    {
        bool flag = false;
        if (this.enableReservedArea && (this.fingers[touchIndex] != null))
        {
            int num = 0;
            Rect realRect = new Rect(0f, 0f, 0f, 0f);
            while (!flag && (num < this.reservedAreas.Count))
            {
                flag = this.reservedAreas[num].Contains(this.fingers[touchIndex].position);
                num++;
            }
            for (num = 0; !flag && (num < this.reservedGuiAreas.Count); num++)
            {
                Rect rect3 = this.reservedGuiAreas[num];
                Rect rect4 = this.reservedGuiAreas[num];
                Rect rect5 = this.reservedGuiAreas[num];
                Rect rect6 = this.reservedGuiAreas[num];
                Rect rect7 = this.reservedGuiAreas[num];
                realRect = new Rect(rect3.x, (Screen.height - rect4.y) - rect5.height, rect6.width, rect7.height);
                flag = realRect.Contains(this.fingers[touchIndex].position);
            }
            for (num = 0; !flag && (num < this.reservedVirtualAreas.Count); num++)
            {
                realRect = VirtualScreen.GetRealRect(this.reservedVirtualAreas[num]);
                flag = new Rect(realRect.x, (Screen.height - realRect.y) - realRect.height, realRect.width, realRect.height).Contains(this.fingers[touchIndex].position);
            }
        }
        return flag;
    }

    private void OnDrawGizmos()
    {
    }

    private void OneFinger(int fingerIndex)
    {
        float actionTime = 0f;
        if (this.fingers[fingerIndex] != null)
        {
            if (this.fingers[fingerIndex].gesture == GestureType.None)
            {
                this.startTimeAction = Time.realtimeSinceStartup;
                this.fingers[fingerIndex].gesture = GestureType.Acquisition;
                this.fingers[fingerIndex].startPosition = this.fingers[fingerIndex].position;
                if (this.autoSelect)
                {
                    this.GetPickeGameObject(ref this.fingers[fingerIndex], false);
                }
                this.CreateGesture(fingerIndex, EventName.On_TouchStart, this.fingers[fingerIndex], 0f, SwipeType.None, 0f, Vector2.zero);
            }
            actionTime = Time.realtimeSinceStartup - this.startTimeAction;
            if (this.fingers[fingerIndex].phase == TouchPhase.Canceled)
            {
                this.fingers[fingerIndex].gesture = GestureType.Cancel;
            }
            if ((this.fingers[fingerIndex].phase == TouchPhase.Ended) || (this.fingers[fingerIndex].phase == TouchPhase.Canceled))
            {
                bool flag = true;
                switch (this.fingers[fingerIndex].gesture)
                {
                    case GestureType.Drag:
                    {
                        Vector2 vector3 = this.fingers[fingerIndex].startPosition - this.fingers[fingerIndex].position;
                        this.CreateGesture(fingerIndex, EventName.On_DragEnd, this.fingers[fingerIndex], actionTime, this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), vector3.magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
                        break;
                    }
                    case GestureType.Swipe:
                    {
                        Vector2 vector4 = this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition;
                        this.CreateGesture(fingerIndex, EventName.On_SwipeEnd, this.fingers[fingerIndex], actionTime, this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), vector4.magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
                        break;
                    }
                    case GestureType.LongTap:
                        this.CreateGesture(fingerIndex, EventName.On_LongTapEnd, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                        break;

                    case GestureType.Cancel:
                        this.CreateGesture(fingerIndex, EventName.On_Cancel, this.fingers[fingerIndex], 0f, SwipeType.None, 0f, Vector2.zero);
                        break;

                    case GestureType.Acquisition:
                        if (!this.FingerInTolerance(this.fingers[fingerIndex]))
                        {
                            SwipeType swipe = this.GetSwipe(new Vector2(0f, 0f), this.fingers[fingerIndex].deltaPosition);
                            if (this.fingers[fingerIndex].pickedObject != null)
                            {
                                this.CreateGesture(fingerIndex, EventName.On_DragStart, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                                this.CreateGesture(fingerIndex, EventName.On_Drag, this.fingers[fingerIndex], actionTime, swipe, 0f, this.fingers[fingerIndex].deltaPosition);
                                Vector2 vector = this.fingers[fingerIndex].startPosition - this.fingers[fingerIndex].position;
                                this.CreateGesture(fingerIndex, EventName.On_DragEnd, this.fingers[fingerIndex], actionTime, this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), vector.magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
                            }
                            else
                            {
                                this.CreateGesture(fingerIndex, EventName.On_SwipeStart, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                                this.CreateGesture(fingerIndex, EventName.On_Swipe, this.fingers[fingerIndex], actionTime, swipe, 0f, this.fingers[fingerIndex].deltaPosition);
                                Vector2 vector2 = this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition;
                                this.CreateGesture(fingerIndex, EventName.On_SwipeEnd, this.fingers[fingerIndex], actionTime, this.GetSwipe(this.fingers[fingerIndex].startPosition, this.fingers[fingerIndex].position), vector2.magnitude, this.fingers[fingerIndex].position - this.fingers[fingerIndex].startPosition);
                            }
                            break;
                        }
                        if (this.fingers[fingerIndex].tapCount >= 2)
                        {
                            this.CreateGesture(fingerIndex, EventName.On_DoubleTap, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                            break;
                        }
                        this.CreateGesture(fingerIndex, EventName.On_SimpleTap, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                        break;
                }
                if (flag)
                {
                    this.CreateGesture(fingerIndex, EventName.On_TouchUp, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                    this.fingers[fingerIndex] = null;
                }
            }
            else
            {
                if (((this.fingers[fingerIndex].phase == TouchPhase.Stationary) && (actionTime >= this.longTapTime)) && (this.fingers[fingerIndex].gesture == GestureType.Acquisition))
                {
                    this.fingers[fingerIndex].gesture = GestureType.LongTap;
                    this.CreateGesture(fingerIndex, EventName.On_LongTapStart, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                }
                if (((this.fingers[fingerIndex].gesture == GestureType.Acquisition) || (this.fingers[fingerIndex].gesture == GestureType.LongTap)) && !this.FingerInTolerance(this.fingers[fingerIndex]))
                {
                    if (this.fingers[fingerIndex].gesture == GestureType.LongTap)
                    {
                        this.fingers[fingerIndex].gesture = GestureType.Cancel;
                        this.CreateGesture(fingerIndex, EventName.On_LongTapEnd, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                        this.fingers[fingerIndex].gesture = GestureType.None;
                    }
                    else if (this.fingers[fingerIndex].pickedObject != null)
                    {
                        this.fingers[fingerIndex].gesture = GestureType.Drag;
                        this.CreateGesture(fingerIndex, EventName.On_DragStart, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                    }
                    else
                    {
                        this.fingers[fingerIndex].gesture = GestureType.Swipe;
                        this.CreateGesture(fingerIndex, EventName.On_SwipeStart, this.fingers[fingerIndex], actionTime, SwipeType.None, 0f, Vector2.zero);
                    }
                }
                EventName none = EventName.None;
                switch (this.fingers[fingerIndex].gesture)
                {
                    case GestureType.Drag:
                        none = EventName.On_Drag;
                        break;

                    case GestureType.Swipe:
                        none = EventName.On_Swipe;
                        break;

                    case GestureType.LongTap:
                        none = EventName.On_LongTap;
                        break;
                }
                SwipeType type = SwipeType.None;
                if (none != EventName.None)
                {
                    type = this.GetSwipe(new Vector2(0f, 0f), this.fingers[fingerIndex].deltaPosition);
                    this.CreateGesture(fingerIndex, none, this.fingers[fingerIndex], actionTime, type, 0f, this.fingers[fingerIndex].deltaPosition);
                }
                this.CreateGesture(fingerIndex, EventName.On_TouchDown, this.fingers[fingerIndex], actionTime, type, 0f, this.fingers[fingerIndex].deltaPosition);
            }
        }
    }

    private void OnEnable()
    {
        if (Application.isPlaying && Application.isEditor)
        {
            this.InitEasyTouch();
        }
    }

    private void RaiseEvent(EventName evnt, Gesture gesture)
    {
        if ((!this.locked && !this.HighPriorityLocked) && !this.FlexAnimationLocked)
        {
            switch (evnt)
            {
                case EventName.On_Cancel:
                    if (On_Cancel != null)
                    {
                        On_Cancel(gesture);
                    }
                    break;

                case EventName.On_Cancel2Fingers:
                    if (On_Cancel2Fingers != null)
                    {
                        On_Cancel2Fingers(gesture);
                    }
                    break;

                case EventName.On_TouchStart:
                    if (On_TouchStart != null)
                    {
                        On_TouchStart(gesture);
                    }
                    break;

                case EventName.On_TouchDown:
                    if (On_TouchDown != null)
                    {
                        On_TouchDown(gesture);
                    }
                    break;

                case EventName.On_TouchUp:
                    if (On_TouchUp != null)
                    {
                        On_TouchUp(gesture);
                    }
                    break;

                case EventName.On_SimpleTap:
                    if (On_SimpleTap != null)
                    {
                        On_SimpleTap(gesture);
                    }
                    break;

                case EventName.On_DoubleTap:
                    if (On_DoubleTap != null)
                    {
                        On_DoubleTap(gesture);
                    }
                    break;

                case EventName.On_LongTapStart:
                    if (On_LongTapStart != null)
                    {
                        On_LongTapStart(gesture);
                    }
                    break;

                case EventName.On_LongTap:
                    if (On_LongTap != null)
                    {
                        On_LongTap(gesture);
                    }
                    break;

                case EventName.On_LongTapEnd:
                    if (On_LongTapEnd != null)
                    {
                        On_LongTapEnd(gesture);
                    }
                    break;

                case EventName.On_DragStart:
                    if (On_DragStart != null)
                    {
                        On_DragStart(gesture);
                    }
                    break;

                case EventName.On_Drag:
                    if (On_Drag != null)
                    {
                        On_Drag(gesture);
                    }
                    break;

                case EventName.On_DragEnd:
                    if (On_DragEnd != null)
                    {
                        On_DragEnd(gesture);
                    }
                    break;

                case EventName.On_SwipeStart:
                    if (On_SwipeStart != null)
                    {
                        On_SwipeStart(gesture);
                    }
                    break;

                case EventName.On_Swipe:
                    if (On_Swipe != null)
                    {
                        On_Swipe(gesture);
                    }
                    break;

                case EventName.On_SwipeEnd:
                    if (On_SwipeEnd != null)
                    {
                        On_SwipeEnd(gesture);
                    }
                    break;

                case EventName.On_TouchStart2Fingers:
                    if (On_TouchStart2Fingers != null)
                    {
                        On_TouchStart2Fingers(gesture);
                    }
                    break;

                case EventName.On_TouchDown2Fingers:
                    if (On_TouchDown2Fingers != null)
                    {
                        On_TouchDown2Fingers(gesture);
                    }
                    break;

                case EventName.On_TouchUp2Fingers:
                    if (On_TouchUp2Fingers != null)
                    {
                        On_TouchUp2Fingers(gesture);
                    }
                    break;

                case EventName.On_SimpleTap2Fingers:
                    if (On_SimpleTap2Fingers != null)
                    {
                        On_SimpleTap2Fingers(gesture);
                    }
                    break;

                case EventName.On_DoubleTap2Fingers:
                    if (On_DoubleTap2Fingers != null)
                    {
                        On_DoubleTap2Fingers(gesture);
                    }
                    break;

                case EventName.On_LongTapStart2Fingers:
                    if (On_LongTapStart2Fingers != null)
                    {
                        On_LongTapStart2Fingers(gesture);
                    }
                    break;

                case EventName.On_LongTap2Fingers:
                    if (On_LongTap2Fingers != null)
                    {
                        On_LongTap2Fingers(gesture);
                    }
                    break;

                case EventName.On_LongTapEnd2Fingers:
                    if (On_LongTapEnd2Fingers != null)
                    {
                        On_LongTapEnd2Fingers(gesture);
                    }
                    break;

                case EventName.On_Twist:
                    if (On_Twist != null)
                    {
                        On_Twist(gesture);
                    }
                    break;

                case EventName.On_TwistEnd:
                    if (On_TwistEnd != null)
                    {
                        On_TwistEnd(gesture);
                    }
                    break;

                case EventName.On_PinchIn:
                    if (On_PinchIn != null)
                    {
                        On_PinchIn(gesture);
                    }
                    break;

                case EventName.On_PinchOut:
                    if (On_PinchOut != null)
                    {
                        On_PinchOut(gesture);
                    }
                    break;

                case EventName.On_PinchEnd:
                    if (On_PinchEnd != null)
                    {
                        On_PinchEnd(gesture);
                    }
                    break;

                case EventName.On_DragStart2Fingers:
                    if (On_DragStart2Fingers != null)
                    {
                        On_DragStart2Fingers(gesture);
                    }
                    break;

                case EventName.On_Drag2Fingers:
                    if (On_Drag2Fingers != null)
                    {
                        On_Drag2Fingers(gesture);
                    }
                    break;

                case EventName.On_DragEnd2Fingers:
                    if (On_DragEnd2Fingers != null)
                    {
                        On_DragEnd2Fingers(gesture);
                    }
                    break;

                case EventName.On_SwipeStart2Fingers:
                    if (On_SwipeStart2Fingers != null)
                    {
                        On_SwipeStart2Fingers(gesture);
                    }
                    break;

                case EventName.On_Swipe2Fingers:
                    if (On_Swipe2Fingers != null)
                    {
                        On_Swipe2Fingers(gesture);
                    }
                    break;

                case EventName.On_SwipeEnd2Fingers:
                    if (On_SwipeEnd2Fingers != null)
                    {
                        On_SwipeEnd2Fingers(gesture);
                    }
                    break;
            }
        }
    }

    private void RaiseReadyEvent()
    {
        if (this.useBroadcastMessage)
        {
            if (this.receiverObject != null)
            {
                base.gameObject.SendMessage("On_EasyTouchIsReady", SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (On_EasyTouchIsReady != null)
        {
            On_EasyTouchIsReady();
        }
    }

    public static void RemoveReservedArea(Rect rec)
    {
        if (instance != null)
        {
            instance.reservedAreas.Remove(rec);
        }
    }

    public static void RemoveReservedGuiArea(Rect rec)
    {
        if (instance != null)
        {
            instance.reservedGuiAreas.Remove(rec);
        }
    }

    public static void ResetTouch(int fingerIndex)
    {
        if (instance != null)
        {
            instance.GetFinger(fingerIndex).gesture = GestureType.None;
        }
    }

    private void ResetTouches()
    {
        for (int i = 0; i < 10; i++)
        {
            this.fingers[i] = null;
        }
    }

    private void SendGesture(EventName message, Gesture gesture)
    {
        if (this.useBroadcastMessage)
        {
            if ((this.receiverObject != null) && (this.receiverObject != gesture.pickObject))
            {
                this.receiverObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
            }
            if (gesture.pickObject != null)
            {
                gesture.pickObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                base.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void SendGesture2Finger(EventName message, Gesture gesture)
    {
        if ((this.receiverObject != null) && (this.receiverObject != gesture.pickObject))
        {
            this.receiverObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
        }
        if (gesture.pickObject != null)
        {
            gesture.pickObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            base.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
        }
    }

    public static void SetCamera(Camera cam, bool guiCam = false)
    {
        instance.touchCameras.Add(new ECamera(cam, guiCam));
    }

    public static void SetEnable2FingersGesture(bool enable)
    {
        instance.enable2FingersGesture = enable;
    }

    public static void SetEnableAutoSelect(bool enable)
    {
        instance.autoSelect = enable;
    }

    public static void SetEnabled(bool enable)
    {
        instance.enable = enable;
        if (enable)
        {
            instance.ResetTouches();
        }
    }

    public static void SetEnablePinch(bool enable)
    {
        instance.enablePinch = enable;
    }

    public static void SetEnableTwist(bool enable)
    {
        instance.enableTwist = enable;
    }

    public static void SetIsReservedArea(bool enable)
    {
        instance.enableReservedArea = enable;
    }

    public static void SetlongTapTime(float time)
    {
        instance.longTapTime = time;
    }

    public static void SetMinPinchLength(float length)
    {
        instance.minPinchLength = length;
    }

    public static void SetMinTwistAngle(float angle)
    {
        instance.minTwistAngle = angle;
    }

    public static void SetOtherReceiverObject(GameObject receiver)
    {
        instance.receiverObject = receiver;
    }

    public static void SetPickableLayer(LayerMask mask)
    {
        if (instance != null)
        {
            instance.pickableLayers = mask;
        }
    }

    public static void SetStationnaryTolerance(float tolerance)
    {
        instance.StationnaryTolerance = tolerance;
    }

    public static void SetSwipeTolerance(float tolerance)
    {
        instance.swipeTolerance = tolerance;
    }

    private void Start()
    {
        if (<>f__am$cache59 == null)
        {
            <>f__am$cache59 = c => c.camera == Camera.main;
        }
        if (this.touchCameras.FindIndex(<>f__am$cache59) < 0)
        {
            this.touchCameras.Add(new ECamera(Camera.main, false));
        }
        this.InitEasyTouch();
        this.RaiseReadyEvent();
    }

    private float TwistAngle()
    {
        Vector2 end = this.fingers[this.twoFinger0].position - this.fingers[this.twoFinger1].position;
        Vector2 start = this.fingers[this.twoFinger0].oldPosition - this.fingers[this.twoFinger1].oldPosition;
        return (57.29578f * this.DeltaAngle(start, end));
    }

    private void TwoFinger()
    {
        float actionTime = 0f;
        bool flag = false;
        Vector2 zero = Vector2.zero;
        Vector2 deltaPosition = Vector2.zero;
        float twoDistance = 0f;
        if (this.complexCurrentGesture == GestureType.None)
        {
            this.twoFinger0 = this.GetTwoFinger(-1);
            this.twoFinger1 = this.GetTwoFinger(this.twoFinger0);
            this.startTimeAction = Time.realtimeSinceStartup;
            this.complexCurrentGesture = GestureType.Tap;
            this.fingers[this.twoFinger0].complexStartPosition = this.fingers[this.twoFinger0].position;
            this.fingers[this.twoFinger1].complexStartPosition = this.fingers[this.twoFinger1].position;
            this.fingers[this.twoFinger0].oldPosition = this.fingers[this.twoFinger0].position;
            this.fingers[this.twoFinger1].oldPosition = this.fingers[this.twoFinger1].position;
            this.oldFingerDistance = Mathf.Abs(Vector2.Distance(this.fingers[this.twoFinger0].position, this.fingers[this.twoFinger1].position));
            this.startPosition2Finger = new Vector2((this.fingers[this.twoFinger0].position.x + this.fingers[this.twoFinger1].position.x) / 2f, (this.fingers[this.twoFinger0].position.y + this.fingers[this.twoFinger1].position.y) / 2f);
            deltaPosition = Vector2.zero;
            if (this.autoSelect)
            {
                if (this.GetPickeGameObject(ref this.fingers[this.twoFinger0], true))
                {
                    this.GetPickeGameObject(ref this.fingers[this.twoFinger1], true);
                    if (this.fingers[this.twoFinger0].pickedObject != this.fingers[this.twoFinger1].pickedObject)
                    {
                        this.pickObject2Finger = null;
                        this.fingers[this.twoFinger0].pickedObject = null;
                        this.fingers[this.twoFinger1].pickedObject = null;
                        this.fingers[this.twoFinger0].isGuiCamera = false;
                        this.fingers[this.twoFinger1].isGuiCamera = false;
                        this.fingers[this.twoFinger0].pickedCamera = null;
                        this.fingers[this.twoFinger1].pickedCamera = null;
                    }
                    else
                    {
                        this.pickObject2Finger = this.fingers[this.twoFinger0].pickedObject;
                    }
                }
                else
                {
                    this.pickObject2Finger = null;
                }
            }
            this.CreateGesture2Finger(EventName.On_TouchStart2Fingers, this.startPosition2Finger, this.startPosition2Finger, deltaPosition, actionTime, SwipeType.None, 0f, Vector2.zero, 0f, 0f, this.oldFingerDistance);
        }
        actionTime = Time.realtimeSinceStartup - this.startTimeAction;
        zero = new Vector2((this.fingers[this.twoFinger0].position.x + this.fingers[this.twoFinger1].position.x) / 2f, (this.fingers[this.twoFinger0].position.y + this.fingers[this.twoFinger1].position.y) / 2f);
        deltaPosition = zero - this.oldStartPosition2Finger;
        twoDistance = Mathf.Abs(Vector2.Distance(this.fingers[this.twoFinger0].position, this.fingers[this.twoFinger1].position));
        if ((this.fingers[this.twoFinger0].phase == TouchPhase.Canceled) || (this.fingers[this.twoFinger1].phase == TouchPhase.Canceled))
        {
            this.complexCurrentGesture = GestureType.Cancel;
        }
        if (((this.fingers[this.twoFinger0].phase != TouchPhase.Ended) && (this.fingers[this.twoFinger1].phase != TouchPhase.Ended)) && (this.complexCurrentGesture != GestureType.Cancel))
        {
            if (((this.complexCurrentGesture == GestureType.Tap) && (actionTime >= this.longTapTime)) && (this.FingerInTolerance(this.fingers[this.twoFinger0]) && this.FingerInTolerance(this.fingers[this.twoFinger1])))
            {
                this.complexCurrentGesture = GestureType.LongTap;
                this.CreateGesture2Finger(EventName.On_LongTapStart2Fingers, this.startPosition2Finger, zero, deltaPosition, actionTime, SwipeType.None, 0f, Vector2.zero, 0f, 0f, twoDistance);
            }
            if (!this.FingerInTolerance(this.fingers[this.twoFinger0]) || !this.FingerInTolerance(this.fingers[this.twoFinger1]))
            {
                flag = true;
            }
            if (flag)
            {
                float num3 = Vector2.Dot(this.fingers[this.twoFinger0].deltaPosition.normalized, this.fingers[this.twoFinger1].deltaPosition.normalized);
                if (this.enablePinch && (twoDistance != this.oldFingerDistance))
                {
                    if (Mathf.Abs((float) (twoDistance - this.oldFingerDistance)) >= this.minPinchLength)
                    {
                        this.complexCurrentGesture = GestureType.Pinch;
                    }
                    if (this.complexCurrentGesture == GestureType.Pinch)
                    {
                        if (twoDistance < this.oldFingerDistance)
                        {
                            if (this.oldGesture != GestureType.Pinch)
                            {
                                this.CreateStateEnd2Fingers(this.oldGesture, this.startPosition2Finger, zero, deltaPosition, actionTime, false, twoDistance);
                                this.startTimeAction = Time.realtimeSinceStartup;
                            }
                            this.CreateGesture2Finger(EventName.On_PinchIn, this.startPosition2Finger, zero, deltaPosition, actionTime, this.GetSwipe(this.fingers[this.twoFinger0].complexStartPosition, this.fingers[this.twoFinger0].position), 0f, Vector2.zero, 0f, Mathf.Abs((float) (twoDistance - this.oldFingerDistance)), twoDistance);
                            this.complexCurrentGesture = GestureType.Pinch;
                        }
                        else if (twoDistance > this.oldFingerDistance)
                        {
                            if (this.oldGesture != GestureType.Pinch)
                            {
                                this.CreateStateEnd2Fingers(this.oldGesture, this.startPosition2Finger, zero, deltaPosition, actionTime, false, twoDistance);
                                this.startTimeAction = Time.realtimeSinceStartup;
                            }
                            this.CreateGesture2Finger(EventName.On_PinchOut, this.startPosition2Finger, zero, deltaPosition, actionTime, this.GetSwipe(this.fingers[this.twoFinger0].complexStartPosition, this.fingers[this.twoFinger0].position), 0f, Vector2.zero, 0f, Mathf.Abs((float) (twoDistance - this.oldFingerDistance)), twoDistance);
                            this.complexCurrentGesture = GestureType.Pinch;
                        }
                    }
                }
                if (this.enableTwist)
                {
                    if (Mathf.Abs(this.TwistAngle()) > this.minTwistAngle)
                    {
                        if (this.complexCurrentGesture != GestureType.Twist)
                        {
                            this.CreateStateEnd2Fingers(this.complexCurrentGesture, this.startPosition2Finger, zero, deltaPosition, actionTime, false, twoDistance);
                            this.startTimeAction = Time.realtimeSinceStartup;
                        }
                        this.complexCurrentGesture = GestureType.Twist;
                    }
                    if (this.complexCurrentGesture == GestureType.Twist)
                    {
                        this.CreateGesture2Finger(EventName.On_Twist, this.startPosition2Finger, zero, deltaPosition, actionTime, SwipeType.None, 0f, Vector2.zero, this.TwistAngle(), 0f, twoDistance);
                    }
                    this.fingers[this.twoFinger0].oldPosition = this.fingers[this.twoFinger0].position;
                    this.fingers[this.twoFinger1].oldPosition = this.fingers[this.twoFinger1].position;
                }
                if (num3 > 0f)
                {
                    if ((this.pickObject2Finger != null) && !this.twoFingerDragStart)
                    {
                        if (this.complexCurrentGesture != GestureType.Tap)
                        {
                            this.CreateStateEnd2Fingers(this.complexCurrentGesture, this.startPosition2Finger, zero, deltaPosition, actionTime, false, twoDistance);
                            this.startTimeAction = Time.realtimeSinceStartup;
                        }
                        this.CreateGesture2Finger(EventName.On_DragStart2Fingers, this.startPosition2Finger, zero, deltaPosition, actionTime, SwipeType.None, 0f, Vector2.zero, 0f, 0f, twoDistance);
                        this.twoFingerDragStart = true;
                    }
                    else if ((this.pickObject2Finger == null) && !this.twoFingerSwipeStart)
                    {
                        if (this.complexCurrentGesture != GestureType.Tap)
                        {
                            this.CreateStateEnd2Fingers(this.complexCurrentGesture, this.startPosition2Finger, zero, deltaPosition, actionTime, false, twoDistance);
                            this.startTimeAction = Time.realtimeSinceStartup;
                        }
                        this.CreateGesture2Finger(EventName.On_SwipeStart2Fingers, this.startPosition2Finger, zero, deltaPosition, actionTime, SwipeType.None, 0f, Vector2.zero, 0f, 0f, twoDistance);
                        this.twoFingerSwipeStart = true;
                    }
                }
                else if (num3 < 0f)
                {
                    this.twoFingerDragStart = false;
                    this.twoFingerSwipeStart = false;
                }
                if (this.twoFingerDragStart)
                {
                    this.CreateGesture2Finger(EventName.On_Drag2Fingers, this.startPosition2Finger, zero, deltaPosition, actionTime, this.GetSwipe(this.oldStartPosition2Finger, zero), 0f, deltaPosition, 0f, 0f, twoDistance);
                }
                if (this.twoFingerSwipeStart)
                {
                    this.CreateGesture2Finger(EventName.On_Swipe2Fingers, this.startPosition2Finger, zero, deltaPosition, actionTime, this.GetSwipe(this.oldStartPosition2Finger, zero), 0f, deltaPosition, 0f, 0f, twoDistance);
                }
            }
            else if (this.complexCurrentGesture == GestureType.LongTap)
            {
                this.CreateGesture2Finger(EventName.On_LongTap2Fingers, this.startPosition2Finger, zero, deltaPosition, actionTime, SwipeType.None, 0f, Vector2.zero, 0f, 0f, twoDistance);
            }
            this.CreateGesture2Finger(EventName.On_TouchDown2Fingers, this.startPosition2Finger, zero, deltaPosition, actionTime, this.GetSwipe(this.oldStartPosition2Finger, zero), 0f, deltaPosition, 0f, 0f, twoDistance);
            this.oldFingerDistance = twoDistance;
            this.oldStartPosition2Finger = zero;
            this.oldGesture = this.complexCurrentGesture;
        }
        else
        {
            this.CreateStateEnd2Fingers(this.complexCurrentGesture, this.startPosition2Finger, zero, deltaPosition, actionTime, true, twoDistance);
            this.complexCurrentGesture = GestureType.None;
            this.pickObject2Finger = null;
            this.twoFingerSwipeStart = false;
            this.twoFingerDragStart = false;
        }
    }

    private void Update()
    {
        if (this.enable && (instance == this))
        {
            int touchCount = this.input.TouchCount();
            if (((this.oldTouchCount == 2) && (touchCount != 2)) && (touchCount > 0))
            {
                this.CreateGesture2Finger(EventName.On_Cancel2Fingers, Vector2.zero, Vector2.zero, Vector2.zero, 0f, SwipeType.None, 0f, Vector2.zero, 0f, 0f, 0f);
            }
            this.UpdateTouches(true, touchCount);
            this.oldPickObject2Finger = this.pickObject2Finger;
            if (this.enable2FingersGesture)
            {
                if (touchCount == 2)
                {
                    this.TwoFinger();
                }
                else
                {
                    this.complexCurrentGesture = GestureType.None;
                    this.pickObject2Finger = null;
                    this.twoFingerSwipeStart = false;
                    this.twoFingerDragStart = false;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (this.fingers[i] != null)
                {
                    this.OneFinger(i);
                }
            }
            this.oldTouchCount = touchCount;
        }
    }

    private void UpdateTouches(bool realTouch, int touchCount)
    {
        Finger[] array = new Finger[10];
        this.fingers.CopyTo(array, 0);
        if (realTouch || this.enableRemote)
        {
            this.ResetTouches();
            for (int i = 0; i < touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                for (int j = 0; (j < 10) && (this.fingers[i] == null); j++)
                {
                    if ((array[j] != null) && (array[j].fingerIndex == touch.fingerId))
                    {
                        this.fingers[i] = array[j];
                    }
                }
                if (this.fingers[i] == null)
                {
                    this.fingers[i] = new Finger();
                    this.fingers[i].fingerIndex = touch.fingerId;
                    this.fingers[i].gesture = GestureType.None;
                    this.fingers[i].phase = TouchPhase.Began;
                }
                else
                {
                    this.fingers[i].phase = touch.phase;
                }
                this.fingers[i].position = touch.position;
                this.fingers[i].deltaPosition = touch.deltaPosition;
                this.fingers[i].tapCount = touch.tapCount;
                this.fingers[i].deltaTime = touch.deltaTime;
                this.fingers[i].touchCount = touchCount;
            }
        }
        else
        {
            for (int k = 0; k < touchCount; k++)
            {
                this.fingers[k] = this.input.GetMouseTouch(k, this.fingers[k]);
                this.fingers[k].touchCount = touchCount;
            }
        }
    }

    public bool Locked
    {
        get
        {
            return this.locked;
        }
        set
        {
            this.locked = value;
        }
    }

    public delegate void Cancel2FingersHandler(Gesture gesture);

    public delegate void DoubleTap2FingersHandler(Gesture gesture);

    public delegate void DoubleTapHandler(Gesture gesture);

    public delegate void Drag2FingersHandler(Gesture gesture);

    public delegate void DragEnd2FingersHandler(Gesture gesture);

    public delegate void DragEndHandler(Gesture gesture);

    public delegate void DragHandler(Gesture gesture);

    public delegate void DragStart2FingersHandler(Gesture gesture);

    public delegate void DragStartHandler(Gesture gesture);

    public delegate void EasyTouchIsReadyHandler();

    private enum EventName
    {
        None,
        On_Cancel,
        On_Cancel2Fingers,
        On_TouchStart,
        On_TouchDown,
        On_TouchUp,
        On_SimpleTap,
        On_DoubleTap,
        On_LongTapStart,
        On_LongTap,
        On_LongTapEnd,
        On_DragStart,
        On_Drag,
        On_DragEnd,
        On_SwipeStart,
        On_Swipe,
        On_SwipeEnd,
        On_TouchStart2Fingers,
        On_TouchDown2Fingers,
        On_TouchUp2Fingers,
        On_SimpleTap2Fingers,
        On_DoubleTap2Fingers,
        On_LongTapStart2Fingers,
        On_LongTap2Fingers,
        On_LongTapEnd2Fingers,
        On_Twist,
        On_TwistEnd,
        On_PinchIn,
        On_PinchOut,
        On_PinchEnd,
        On_DragStart2Fingers,
        On_Drag2Fingers,
        On_DragEnd2Fingers,
        On_SwipeStart2Fingers,
        On_Swipe2Fingers,
        On_SwipeEnd2Fingers,
        On_EasyTouchIsReady
    }

    public enum GestureType
    {
        Tap,
        Drag,
        Swipe,
        None,
        LongTap,
        Pinch,
        Twist,
        Cancel,
        Acquisition
    }

    public delegate void LongTap2FingersHandler(Gesture gesture);

    public delegate void LongTapEnd2FingersHandler(Gesture gesture);

    public delegate void LongTapEndHandler(Gesture gesture);

    public delegate void LongTapHandler(Gesture gesture);

    public delegate void LongTapStart2FingersHandler(Gesture gesture);

    public delegate void LongTapStartHandler(Gesture gesture);

    public delegate void PinchEndHandler(Gesture gesture);

    public delegate void PinchInHandler(Gesture gesture);

    public delegate void PinchOutHandler(Gesture gesture);

    public delegate void SimpleTap2FingersHandler(Gesture gesture);

    public delegate void SimpleTapHandler(Gesture gesture);

    public delegate void Swipe2FingersHandler(Gesture gesture);

    public delegate void SwipeEnd2FingersHandler(Gesture gesture);

    public delegate void SwipeEndHandler(Gesture gesture);

    public delegate void SwipeHandler(Gesture gesture);

    public delegate void SwipeStart2FingersHandler(Gesture gesture);

    public delegate void SwipeStartHandler(Gesture gesture);

    public enum SwipeType
    {
        None,
        Left,
        Right,
        Up,
        Down,
        Other
    }

    public delegate void TouchCancelHandler(Gesture gesture);

    public delegate void TouchDown2FingersHandler(Gesture gesture);

    public delegate void TouchDownHandler(Gesture gesture);

    public delegate void TouchStart2FingersHandler(Gesture gesture);

    public delegate void TouchStartHandler(Gesture gesture);

    public delegate void TouchUp2FingersHandler(Gesture gesture);

    public delegate void TouchUpHandler(Gesture gesture);

    public delegate void TwistEndHandler(Gesture gesture);

    public delegate void TwistHandler(Gesture gesture);
}

