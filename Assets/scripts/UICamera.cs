using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Camera)), ExecuteInEditMode, AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)")]
public class UICamera : MonoBehaviour
{
    [CompilerGenerated]
    private static BetterList<DepthEntry>.CompareFunc <>f__am$cache3E;
    [CompilerGenerated]
    private static BetterList<DepthEntry>.CompareFunc <>f__am$cache3F;
    public bool allowMultiTouch = true;
    public KeyCode cancelKey0 = KeyCode.Escape;
    public KeyCode cancelKey1 = KeyCode.JoystickButton1;
    public static MouseOrTouch controller = new MouseOrTouch();
    public static UICamera current = null;
    public static Camera currentCamera = null;
    public static KeyCode currentKey = KeyCode.None;
    public static ControlScheme currentScheme = ControlScheme.Mouse;
    public static MouseOrTouch currentTouch = null;
    public static int currentTouchID = -1;
    public bool debug;
    public LayerMask eventReceiverMask = -1;
    public EventType eventType = EventType.UI_3D;
    public static GameObject fallThrough;
    public bool FlexAnimationLocked;
    public static GameObject genericEventHandler;
    private float globalclickTime;
    public bool HighPriorityLocked;
    public string horizontalAxisName = "Horizontal";
    public static GameObject hoveredObject;
    public static bool inputHasFocus = false;
    public static bool isDragging = false;
    public static RaycastHit lastHit;
    public static Vector2 lastTouchPosition = Vector2.zero;
    public static Vector3 lastWorldPosition = Vector3.zero;
    public static BetterList<UICamera> list = new BetterList<UICamera>();
    public bool Locked;
    private static Plane m2DPlane = new Plane(Vector3.back, 0f);
    private Camera mCam;
    private static GameObject mCurrentSelection = null;
    private static int mHeight = 0;
    private static DepthEntry mHit = new DepthEntry();
    private static BetterList<DepthEntry> mHits = new BetterList<DepthEntry>();
    private static GameObject mHover;
    private static MouseOrTouch[] mMouse = new MouseOrTouch[] { new MouseOrTouch(), new MouseOrTouch(), new MouseOrTouch() };
    private static float mNextEvent = 0f;
    private float mNextRaycast;
    private static ControlScheme mNextScheme = ControlScheme.Controller;
    private static GameObject mNextSelection = null;
    private static bool mNotifying = false;
    public float mouseClickThreshold = 10f;
    public float mouseDragThreshold = 4f;
    private GameObject mTooltip;
    private float mTooltipTime;
    private static Dictionary<int, MouseOrTouch> mTouches = new Dictionary<int, MouseOrTouch>();
    private static int mWidth = 0;
    public static OnCustomInput onCustomInput;
    public static OnScreenResize onScreenResize;
    public float rangeDistance = -1f;
    public string scrollAxisName = "Mouse ScrollWheel";
    public static bool showTooltips = true;
    public bool stickyTooltip = true;
    public KeyCode submitKey0 = KeyCode.Return;
    public KeyCode submitKey1 = KeyCode.JoystickButton0;
    public float tooltipDelay = 1f;
    public float touchClickThreshold = 40f;
    public float touchDragThreshold = 40f;
    public bool useController = true;
    public bool useKeyboard = true;
    public bool useMouse = true;
    public bool useTouch = true;
    public string verticalAxisName = "Vertical";

    private void Awake()
    {
        mWidth = Screen.width;
        mHeight = Screen.height;
        if (((Application.platform == RuntimePlatform.Android) || (Application.platform == RuntimePlatform.IPhonePlayer)) || ((Application.platform == RuntimePlatform.WP8Player) || (Application.platform == RuntimePlatform.BB10Player)))
        {
            this.useMouse = false;
            this.useTouch = true;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                this.useKeyboard = false;
                this.useController = false;
            }
        }
        else if ((Application.platform == RuntimePlatform.PS3) || (Application.platform == RuntimePlatform.XBOX360))
        {
            this.useMouse = false;
            this.useTouch = false;
            this.useKeyboard = false;
            this.useController = true;
        }
        mMouse[0].pos.x = Input.mousePosition.x;
        mMouse[0].pos.y = Input.mousePosition.y;
        for (int i = 1; i < 3; i++)
        {
            mMouse[i].pos = mMouse[0].pos;
            mMouse[i].lastPos = mMouse[0].pos;
        }
        lastTouchPosition = mMouse[0].pos;
    }

    [DebuggerHidden]
    private IEnumerator ChangeSelection()
    {
        return new <ChangeSelection>c__Iterator2 { <>f__this = this };
    }

    private static int CompareFunc(UICamera a, UICamera b)
    {
        if (a.cachedCamera.depth < b.cachedCamera.depth)
        {
            return 1;
        }
        if (a.cachedCamera.depth > b.cachedCamera.depth)
        {
            return -1;
        }
        return 0;
    }

    public static UICamera FindCameraForLayer(int layer)
    {
        int num = ((int) 1) << layer;
        for (int i = 0; i < list.size; i++)
        {
            UICamera camera = list.buffer[i];
            Camera cachedCamera = camera.cachedCamera;
            if ((cachedCamera != null) && ((cachedCamera.cullingMask & num) != 0))
            {
                return camera;
            }
        }
        return null;
    }

    private static int GetDirection(string axis)
    {
        float time = RealTime.time;
        if ((mNextEvent < time) && !string.IsNullOrEmpty(axis))
        {
            float num2 = Input.GetAxis(axis);
            if (num2 > 0.75f)
            {
                mNextEvent = time + 0.25f;
                return 1;
            }
            if (num2 < -0.75f)
            {
                mNextEvent = time + 0.25f;
                return -1;
            }
        }
        return 0;
    }

    private static int GetDirection(KeyCode up, KeyCode down)
    {
        if (Input.GetKeyDown(up))
        {
            return 1;
        }
        if (Input.GetKeyDown(down))
        {
            return -1;
        }
        return 0;
    }

    private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
    {
        if (Input.GetKeyDown(up0) || Input.GetKeyDown(up1))
        {
            return 1;
        }
        if (!Input.GetKeyDown(down0) && !Input.GetKeyDown(down1))
        {
            return 0;
        }
        return -1;
    }

    public static MouseOrTouch GetMouse(int button)
    {
        return mMouse[button];
    }

    public static MouseOrTouch GetTouch(int id)
    {
        MouseOrTouch touch = null;
        if (id < 0)
        {
            return GetMouse(-id - 1);
        }
        if (!mTouches.TryGetValue(id, out touch))
        {
            touch = new MouseOrTouch {
                touchBegan = true
            };
            mTouches.Add(id, touch);
        }
        return touch;
    }

    public static bool IsHighlighted(GameObject go)
    {
        if (currentScheme == ControlScheme.Mouse)
        {
            return (hoveredObject == go);
        }
        return ((currentScheme == ControlScheme.Controller) && (selectedObject == go));
    }

    public static bool IsPressed(GameObject go)
    {
        for (int i = 0; i < 3; i++)
        {
            if (mMouse[i].pressed == go)
            {
                return true;
            }
        }
        foreach (KeyValuePair<int, MouseOrTouch> pair in mTouches)
        {
            if (pair.Value.pressed == go)
            {
                return true;
            }
        }
        return (controller.pressed == go);
    }

    private static bool IsVisible(ref DepthEntry de)
    {
        for (UIPanel panel = NGUITools.FindInParents<UIPanel>(de.go); panel != null; panel = panel.parentPanel)
        {
            if (!panel.IsVisible(de.hit.point))
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsVisible(Vector3 worldPoint, GameObject go)
    {
        for (UIPanel panel = NGUITools.FindInParents<UIPanel>(go); panel != null; panel = panel.parentPanel)
        {
            if (!panel.IsVisible(worldPoint))
            {
                return false;
            }
        }
        return true;
    }

    private void LateUpdate()
    {
        if (this.handlesEvents)
        {
            int width = Screen.width;
            int height = Screen.height;
            if ((width != mWidth) || (height != mHeight))
            {
                mWidth = width;
                mHeight = height;
                UIRoot.Broadcast("UpdateAnchors");
                if (onScreenResize != null)
                {
                    onScreenResize();
                }
            }
        }
    }

    public static void Notify(GameObject go, string funcName, object obj)
    {
        if (!mNotifying)
        {
            mNotifying = true;
            if (NGUITools.GetActive(go))
            {
                go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
                if ((genericEventHandler != null) && (genericEventHandler != go))
                {
                    genericEventHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
                }
            }
            mNotifying = false;
        }
    }

    private void OnApplicationPause()
    {
        MouseOrTouch currentTouch = UICamera.currentTouch;
        if (this.useTouch)
        {
            BetterList<int> list = new BetterList<int>();
            foreach (KeyValuePair<int, MouseOrTouch> pair in mTouches)
            {
                if ((pair.Value != null) && (pair.Value.pressed != null))
                {
                    UICamera.currentTouch = pair.Value;
                    currentTouchID = pair.Key;
                    currentScheme = ControlScheme.Touch;
                    UICamera.currentTouch.clickNotification = ClickNotification.None;
                    this.ProcessTouch(false, true);
                    list.Add(currentTouchID);
                }
            }
            for (int i = 0; i < list.size; i++)
            {
                RemoveTouch(list[i]);
            }
        }
        if (this.useMouse)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mMouse[j].pressed != null)
                {
                    UICamera.currentTouch = mMouse[j];
                    currentTouchID = -1 - j;
                    currentKey = (KeyCode) (0x143 + j);
                    currentScheme = ControlScheme.Mouse;
                    UICamera.currentTouch.clickNotification = ClickNotification.None;
                    this.ProcessTouch(false, true);
                }
            }
        }
        if (this.useController && (controller.pressed != null))
        {
            UICamera.currentTouch = controller;
            currentTouchID = -100;
            currentScheme = ControlScheme.Controller;
            UICamera.currentTouch.last = UICamera.currentTouch.current;
            UICamera.currentTouch.current = mCurrentSelection;
            UICamera.currentTouch.clickNotification = ClickNotification.None;
            this.ProcessTouch(false, true);
            UICamera.currentTouch.last = null;
        }
        UICamera.currentTouch = currentTouch;
    }

    private void OnDisable()
    {
        list.Remove(this);
    }

    private void OnEnable()
    {
        list.Add(this);
        list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
    }

    private void ProcessFakeTouches()
    {
        bool mouseButtonDown = Input.GetMouseButtonDown(0);
        bool mouseButtonUp = Input.GetMouseButtonUp(0);
        bool mouseButton = Input.GetMouseButton(0);
        if ((mouseButtonDown || mouseButtonUp) || mouseButton)
        {
            currentTouchID = 1;
            currentTouch = mMouse[0];
            currentTouch.touchBegan = mouseButtonDown;
            Vector2 mousePosition = Input.mousePosition;
            currentTouch.delta = !mouseButtonDown ? (mousePosition - currentTouch.pos) : Vector2.zero;
            currentTouch.pos = mousePosition;
            if (!Raycast((Vector3) currentTouch.pos))
            {
                hoveredObject = fallThrough;
            }
            if (hoveredObject == null)
            {
                hoveredObject = genericEventHandler;
            }
            currentTouch.last = currentTouch.current;
            currentTouch.current = hoveredObject;
            lastTouchPosition = currentTouch.pos;
            if (mouseButtonDown)
            {
                currentTouch.pressedCam = currentCamera;
            }
            else if (currentTouch.pressed != null)
            {
                currentCamera = currentTouch.pressedCam;
            }
            this.ProcessTouch(mouseButtonDown, mouseButtonUp);
            if (mouseButtonUp)
            {
                RemoveTouch(currentTouchID);
            }
            currentTouch.last = null;
            currentTouch = null;
        }
    }

    public void ProcessMouse()
    {
        lastTouchPosition = Input.mousePosition;
        mMouse[0].delta = lastTouchPosition - mMouse[0].pos;
        mMouse[0].pos = lastTouchPosition;
        bool flag = mMouse[0].delta.sqrMagnitude > 0.001f;
        for (int i = 1; i < 3; i++)
        {
            mMouse[i].pos = mMouse[0].pos;
            mMouse[i].delta = mMouse[0].delta;
        }
        bool flag2 = false;
        bool flag3 = false;
        for (int j = 0; j < 3; j++)
        {
            if (Input.GetMouseButtonDown(j))
            {
                currentScheme = ControlScheme.Mouse;
                flag3 = true;
                flag2 = true;
            }
            else if (Input.GetMouseButton(j))
            {
                currentScheme = ControlScheme.Mouse;
                flag2 = true;
            }
        }
        if ((flag2 || flag) || (this.mNextRaycast < RealTime.time))
        {
            this.mNextRaycast = RealTime.time + 0.02f;
            if (!Raycast(Input.mousePosition))
            {
                hoveredObject = fallThrough;
            }
            if (hoveredObject == null)
            {
                hoveredObject = genericEventHandler;
            }
            for (int n = 0; n < 3; n++)
            {
                mMouse[n].current = hoveredObject;
            }
        }
        bool flag4 = mMouse[0].last != mMouse[0].current;
        if (flag4)
        {
            currentScheme = ControlScheme.Mouse;
        }
        if (flag2)
        {
            this.mTooltipTime = 0f;
        }
        else if (flag && (!this.stickyTooltip || flag4))
        {
            if (this.mTooltipTime != 0f)
            {
                this.mTooltipTime = RealTime.time + this.tooltipDelay;
            }
            else if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
        }
        if ((flag3 || !flag2) && ((mHover != null) && flag4))
        {
            currentScheme = ControlScheme.Mouse;
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            Notify(mHover, "OnHover", false);
            mHover = null;
        }
        for (int k = 0; k < 3; k++)
        {
            bool mouseButtonDown = Input.GetMouseButtonDown(k);
            bool mouseButtonUp = Input.GetMouseButtonUp(k);
            if (mouseButtonDown || mouseButtonUp)
            {
                currentScheme = ControlScheme.Mouse;
            }
            currentTouch = mMouse[k];
            currentTouchID = -1 - k;
            currentKey = (KeyCode) (0x143 + k);
            if (mouseButtonDown)
            {
                currentTouch.pressedCam = currentCamera;
            }
            else if (currentTouch.pressed != null)
            {
                currentCamera = currentTouch.pressedCam;
            }
            this.ProcessTouch(mouseButtonDown, mouseButtonUp);
            currentKey = KeyCode.None;
        }
        currentTouch = null;
        if (!flag2 && flag4)
        {
            currentScheme = ControlScheme.Mouse;
            this.mTooltipTime = RealTime.time + this.tooltipDelay;
            mHover = mMouse[0].current;
            Notify(mHover, "OnHover", true);
        }
        mMouse[0].last = mMouse[0].current;
        for (int m = 1; m < 3; m++)
        {
            mMouse[m].last = mMouse[0].last;
        }
    }

    public void ProcessOthers()
    {
        currentTouchID = -100;
        currentTouch = controller;
        bool pressed = false;
        bool unpressed = false;
        if ((this.submitKey0 != KeyCode.None) && Input.GetKeyDown(this.submitKey0))
        {
            currentKey = this.submitKey0;
            pressed = true;
        }
        if ((this.submitKey1 != KeyCode.None) && Input.GetKeyDown(this.submitKey1))
        {
            currentKey = this.submitKey1;
            pressed = true;
        }
        if ((this.submitKey0 != KeyCode.None) && Input.GetKeyUp(this.submitKey0))
        {
            currentKey = this.submitKey0;
            unpressed = true;
        }
        if ((this.submitKey1 != KeyCode.None) && Input.GetKeyUp(this.submitKey1))
        {
            currentKey = this.submitKey1;
            unpressed = true;
        }
        if (pressed || unpressed)
        {
            currentScheme = ControlScheme.Controller;
            currentTouch.last = currentTouch.current;
            currentTouch.current = mCurrentSelection;
            this.ProcessTouch(pressed, unpressed);
            currentTouch.last = null;
        }
        int num = 0;
        int num2 = 0;
        if (this.useKeyboard)
        {
            if (inputHasFocus)
            {
                num += GetDirection(KeyCode.UpArrow, KeyCode.DownArrow);
                num2 += GetDirection(KeyCode.RightArrow, KeyCode.LeftArrow);
            }
            else
            {
                num += GetDirection(KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow);
                num2 += GetDirection(KeyCode.D, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow);
            }
        }
        if (this.useController)
        {
            if (!string.IsNullOrEmpty(this.verticalAxisName))
            {
                num += GetDirection(this.verticalAxisName);
            }
            if (!string.IsNullOrEmpty(this.horizontalAxisName))
            {
                num2 += GetDirection(this.horizontalAxisName);
            }
        }
        if (num != 0)
        {
            currentScheme = ControlScheme.Controller;
            Notify(mCurrentSelection, "OnKey", (num <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow);
        }
        if (num2 != 0)
        {
            currentScheme = ControlScheme.Controller;
            Notify(mCurrentSelection, "OnKey", (num2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow);
        }
        if (this.useKeyboard && Input.GetKeyDown(KeyCode.Tab))
        {
            currentKey = KeyCode.Tab;
            currentScheme = ControlScheme.Controller;
            Notify(mCurrentSelection, "OnKey", KeyCode.Tab);
        }
        if ((this.cancelKey0 != KeyCode.None) && Input.GetKeyDown(this.cancelKey0))
        {
            currentKey = this.cancelKey0;
            currentScheme = ControlScheme.Controller;
            Notify(mCurrentSelection, "OnKey", KeyCode.Escape);
        }
        if ((this.cancelKey1 != KeyCode.None) && Input.GetKeyDown(this.cancelKey1))
        {
            currentKey = this.cancelKey1;
            currentScheme = ControlScheme.Controller;
            Notify(mCurrentSelection, "OnKey", KeyCode.Escape);
        }
        currentTouch = null;
        currentKey = KeyCode.None;
    }

    public void ProcessTouch(bool pressed, bool unpressed)
    {
        bool flag = currentScheme == ControlScheme.Mouse;
        float num = !flag ? this.touchDragThreshold : this.mouseDragThreshold;
        float num2 = !flag ? this.touchClickThreshold : this.mouseClickThreshold;
        num *= num;
        num2 *= num2;
        if (pressed)
        {
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            currentTouch.pressStarted = true;
            Notify(currentTouch.pressed, "OnPress", false);
            currentTouch.pressed = currentTouch.current;
            currentTouch.dragged = currentTouch.current;
            currentTouch.clickNotification = ClickNotification.BasedOnDelta;
            currentTouch.totalDelta = Vector2.zero;
            currentTouch.dragStarted = false;
            Notify(currentTouch.pressed, "OnPress", true);
            if (currentTouch.pressed != mCurrentSelection)
            {
                if (this.mTooltip != null)
                {
                    this.ShowTooltip(false);
                }
                currentScheme = ControlScheme.Touch;
                selectedObject = currentTouch.pressed;
            }
        }
        else if ((currentTouch.pressed != null) && ((currentTouch.delta.sqrMagnitude != 0f) || (currentTouch.current != currentTouch.last)))
        {
            currentTouch.totalDelta += currentTouch.delta;
            float sqrMagnitude = currentTouch.totalDelta.sqrMagnitude;
            bool flag2 = false;
            if (!currentTouch.dragStarted && (currentTouch.last != currentTouch.current))
            {
                currentTouch.dragStarted = true;
                currentTouch.delta = currentTouch.totalDelta;
                isDragging = true;
                Notify(currentTouch.dragged, "OnDragStart", null);
                Notify(currentTouch.last, "OnDragOver", currentTouch.dragged);
                isDragging = false;
            }
            else if (!currentTouch.dragStarted && (num < sqrMagnitude))
            {
                flag2 = true;
                currentTouch.dragStarted = true;
                currentTouch.delta = currentTouch.totalDelta;
            }
            if (currentTouch.dragStarted)
            {
                if (this.mTooltip != null)
                {
                    this.ShowTooltip(false);
                }
                isDragging = true;
                bool flag3 = currentTouch.clickNotification == ClickNotification.None;
                if (flag2)
                {
                    Notify(currentTouch.dragged, "OnDragStart", null);
                    Notify(currentTouch.current, "OnDragOver", currentTouch.dragged);
                }
                else if (currentTouch.last != currentTouch.current)
                {
                    Notify(currentTouch.last, "OnDragOut", currentTouch.dragged);
                    Notify(currentTouch.current, "OnDragOver", currentTouch.dragged);
                }
                Notify(currentTouch.dragged, "OnDrag", currentTouch.delta);
                currentTouch.last = currentTouch.current;
                isDragging = false;
                if (flag3)
                {
                    currentTouch.clickNotification = ClickNotification.None;
                }
                else if ((currentTouch.clickNotification == ClickNotification.BasedOnDelta) && (num2 < sqrMagnitude))
                {
                    currentTouch.clickNotification = ClickNotification.None;
                }
            }
        }
        if (unpressed)
        {
            currentTouch.pressStarted = false;
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            if (currentTouch.pressed != null)
            {
                if (currentTouch.dragStarted)
                {
                    Notify(currentTouch.last, "OnDragOut", currentTouch.dragged);
                    Notify(currentTouch.dragged, "OnDragEnd", null);
                }
                Notify(currentTouch.pressed, "OnPress", false);
                if (flag)
                {
                    Notify(currentTouch.current, "OnHover", true);
                }
                mHover = currentTouch.current;
                if ((currentTouch.dragged == currentTouch.current) || (((currentScheme != ControlScheme.Controller) && (currentTouch.clickNotification != ClickNotification.None)) && (currentTouch.totalDelta.sqrMagnitude < num)))
                {
                    if (currentTouch.pressed != mCurrentSelection)
                    {
                        mNextSelection = null;
                        mCurrentSelection = currentTouch.pressed;
                        Notify(currentTouch.pressed, "OnSelect", true);
                    }
                    else
                    {
                        mNextSelection = null;
                        mCurrentSelection = currentTouch.pressed;
                    }
                    if ((currentTouch.clickNotification != ClickNotification.None) && (currentTouch.pressed == currentTouch.current))
                    {
                        float time = RealTime.time;
                        Notify(currentTouch.pressed, "OnClick", null);
                        if ((currentTouch.clickTime + 0.35f) > time)
                        {
                            Notify(currentTouch.pressed, "OnDoubleClick", null);
                        }
                        currentTouch.clickTime = time;
                    }
                }
                else if (currentTouch.dragStarted)
                {
                    Notify(currentTouch.current, "OnDrop", currentTouch.dragged);
                }
            }
            currentTouch.dragStarted = false;
            currentTouch.pressed = null;
            currentTouch.dragged = null;
        }
    }

    public void ProcessTouches()
    {
        currentScheme = ControlScheme.Touch;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            currentTouchID = !this.allowMultiTouch ? 1 : touch.fingerId;
            currentTouch = GetTouch(currentTouchID);
            bool pressed = (touch.phase == TouchPhase.Began) || currentTouch.touchBegan;
            bool unpressed = (touch.phase == TouchPhase.Canceled) || (touch.phase == TouchPhase.Ended);
            currentTouch.touchBegan = false;
            currentTouch.delta = !pressed ? (touch.position - currentTouch.pos) : Vector2.zero;
            currentTouch.pos = touch.position;
            if (!Raycast((Vector3) currentTouch.pos))
            {
                hoveredObject = fallThrough;
            }
            if (hoveredObject == null)
            {
                hoveredObject = genericEventHandler;
            }
            currentTouch.last = currentTouch.current;
            currentTouch.current = hoveredObject;
            lastTouchPosition = currentTouch.pos;
            if (pressed)
            {
                currentTouch.pressedCam = currentCamera;
            }
            else if (currentTouch.pressed != null)
            {
                currentCamera = currentTouch.pressedCam;
            }
            if (touch.tapCount > 1)
            {
                currentTouch.clickTime = RealTime.time;
            }
            this.ProcessTouch(pressed, unpressed);
            if (unpressed)
            {
                RemoveTouch(currentTouchID);
            }
            currentTouch.last = null;
            currentTouch = null;
            if (!this.allowMultiTouch)
            {
                break;
            }
        }
        if ((Input.touchCount == 0) && this.useMouse)
        {
            this.ProcessMouse();
        }
    }

    public static bool Raycast(Vector3 inPos)
    {
        for (int i = 0; i < list.size; i++)
        {
            UICamera camera = list.buffer[i];
            if (!camera.enabled || !NGUITools.GetActive(camera.gameObject))
            {
                continue;
            }
            currentCamera = camera.cachedCamera;
            Vector3 vector = currentCamera.ScreenToViewportPoint(inPos);
            if ((float.IsNaN(vector.x) || float.IsNaN(vector.y)) || ((((vector.x < 0f) || (vector.x > 1f)) || (vector.y < 0f)) || (vector.y > 1f)))
            {
                continue;
            }
            Ray ray = currentCamera.ScreenPointToRay(inPos);
            int layerMask = currentCamera.cullingMask & camera.eventReceiverMask;
            float distance = (camera.rangeDistance <= 0f) ? (currentCamera.farClipPlane - currentCamera.nearClipPlane) : camera.rangeDistance;
            if (camera.eventType == EventType.World_3D)
            {
                if (Physics.Raycast(ray, out lastHit, distance, layerMask))
                {
                    hoveredObject = lastHit.collider.gameObject;
                    return true;
                }
                continue;
            }
            if (camera.eventType != EventType.UI_3D)
            {
                goto Label_045C;
            }
            RaycastHit[] hitArray = Physics.RaycastAll(ray, distance, layerMask);
            if (hitArray.Length > 1)
            {
                for (int j = 0; j < hitArray.Length; j++)
                {
                    GameObject obj2 = hitArray[j].collider.gameObject;
                    UIWidget widget = obj2.GetComponent<UIWidget>();
                    if (widget != null)
                    {
                        if (widget.isVisible && ((widget.hitCheck == null) || widget.hitCheck(hitArray[j].point)))
                        {
                            goto Label_0212;
                        }
                        continue;
                    }
                    UIRect rect = NGUITools.FindInParents<UIRect>(obj2);
                    if ((rect != null) && (rect.finalAlpha < 0.001f))
                    {
                        continue;
                    }
                Label_0212:
                    mHit.depth = NGUITools.CalculateRaycastDepth(obj2);
                    if (mHit.depth != 0x7fffffff)
                    {
                        mHit.hit = hitArray[j];
                        mHit.go = hitArray[j].collider.gameObject;
                        mHits.Add(mHit);
                    }
                }
                if (<>f__am$cache3E == null)
                {
                    <>f__am$cache3E = (r1, r2) => r2.depth.CompareTo(r1.depth);
                }
                mHits.Sort(<>f__am$cache3E);
                for (int k = 0; k < mHits.size; k++)
                {
                    if (IsVisible(ref mHits.buffer[k]))
                    {
                        DepthEntry entry = mHits[k];
                        lastHit = entry.hit;
                        DepthEntry entry2 = mHits[k];
                        hoveredObject = entry2.go;
                        lastWorldPosition = hitArray[k].point;
                        mHits.Clear();
                        return true;
                    }
                }
                mHits.Clear();
                continue;
            }
            if (hitArray.Length != 1)
            {
                continue;
            }
            GameObject gameObject = hitArray[0].collider.gameObject;
            UIWidget component = gameObject.GetComponent<UIWidget>();
            if (component != null)
            {
                if (component.isVisible && ((component.hitCheck == null) || component.hitCheck(hitArray[0].point)))
                {
                    goto Label_03F4;
                }
                continue;
            }
            UIRect rect2 = NGUITools.FindInParents<UIRect>(gameObject);
            if ((rect2 != null) && (rect2.finalAlpha < 0.001f))
            {
                continue;
            }
        Label_03F4:
            if (!IsVisible(hitArray[0].point, hitArray[0].collider.gameObject))
            {
                continue;
            }
            lastHit = hitArray[0];
            lastWorldPosition = hitArray[0].point;
            hoveredObject = lastHit.collider.gameObject;
            return true;
        Label_045C:
            if (camera.eventType == EventType.World_2D)
            {
                if (m2DPlane.Raycast(ray, out distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    Collider2D colliderd = Physics2D.OverlapPoint(point, layerMask);
                    if (colliderd != null)
                    {
                        lastWorldPosition = point;
                        hoveredObject = colliderd.gameObject;
                        return true;
                    }
                }
                continue;
            }
            if ((camera.eventType != EventType.UI_2D) || !m2DPlane.Raycast(ray, out distance))
            {
                continue;
            }
            lastWorldPosition = ray.GetPoint(distance);
            Collider2D[] colliderdArray = Physics2D.OverlapPointAll(lastWorldPosition, layerMask);
            if (colliderdArray.Length > 1)
            {
                for (int m = 0; m < colliderdArray.Length; m++)
                {
                    GameObject obj4 = colliderdArray[m].gameObject;
                    UIWidget widget3 = obj4.GetComponent<UIWidget>();
                    if (widget3 != null)
                    {
                        if (widget3.isVisible && ((widget3.hitCheck == null) || widget3.hitCheck(lastWorldPosition)))
                        {
                            goto Label_0597;
                        }
                        continue;
                    }
                    UIRect rect3 = NGUITools.FindInParents<UIRect>(obj4);
                    if ((rect3 != null) && (rect3.finalAlpha < 0.001f))
                    {
                        continue;
                    }
                Label_0597:
                    mHit.depth = NGUITools.CalculateRaycastDepth(obj4);
                    if (mHit.depth != 0x7fffffff)
                    {
                        mHit.go = obj4;
                        mHits.Add(mHit);
                    }
                }
                if (<>f__am$cache3F == null)
                {
                    <>f__am$cache3F = (r1, r2) => r2.depth.CompareTo(r1.depth);
                }
                mHits.Sort(<>f__am$cache3F);
                for (int n = 0; n < mHits.size; n++)
                {
                    if (IsVisible(ref mHits.buffer[n]))
                    {
                        DepthEntry entry3 = mHits[n];
                        hoveredObject = entry3.go;
                        mHits.Clear();
                        return true;
                    }
                }
                mHits.Clear();
                continue;
            }
            if (colliderdArray.Length != 1)
            {
                continue;
            }
            GameObject go = colliderdArray[0].gameObject;
            UIWidget widget4 = go.GetComponent<UIWidget>();
            if (widget4 != null)
            {
                if (widget4.isVisible && ((widget4.hitCheck == null) || widget4.hitCheck(lastWorldPosition)))
                {
                    goto Label_0712;
                }
                continue;
            }
            UIRect rect4 = NGUITools.FindInParents<UIRect>(go);
            if ((rect4 != null) && (rect4.finalAlpha < 0.001f))
            {
                continue;
            }
        Label_0712:
            if (IsVisible(lastWorldPosition, go))
            {
                hoveredObject = go;
                return true;
            }
        }
        return false;
    }

    public static void RemoveTouch(int id)
    {
        mTouches.Remove(id);
    }

    protected static void SetSelection(GameObject go, ControlScheme scheme)
    {
        if (mNextSelection != null)
        {
            mNextSelection = go;
        }
        else if (mCurrentSelection != go)
        {
            mNextSelection = go;
            mNextScheme = scheme;
            if (list.size > 0)
            {
                UICamera camera = (mNextSelection == null) ? list[0] : FindCameraForLayer(mNextSelection.layer);
                if (camera != null)
                {
                    camera.StartCoroutine(camera.ChangeSelection());
                }
            }
        }
    }

    public void ShowTooltip(bool val)
    {
        this.mTooltipTime = 0f;
        Notify(this.mTooltip, "OnTooltip", val);
        if (!val)
        {
            this.mTooltip = null;
        }
    }

    private void Start()
    {
        if ((this.eventType != EventType.World_3D) && (this.cachedCamera.transparencySortMode != TransparencySortMode.Orthographic))
        {
            this.cachedCamera.transparencySortMode = TransparencySortMode.Orthographic;
        }
        if (Application.isPlaying)
        {
            this.cachedCamera.eventMask = 0;
        }
        if (this.handlesEvents)
        {
            NGUIDebug.debugRaycast = this.debug;
        }
    }

    private void Update()
    {
        if (this.handlesEvents && (((!this.Locked && !this.HighPriorityLocked) && !this.FlexAnimationLocked) || GUIMgr.IsQuitMsgBoxShow))
        {
            current = this;
            if (this.useTouch)
            {
                this.ProcessTouches();
            }
            else if (this.useMouse)
            {
                this.ProcessMouse();
            }
            if (onCustomInput != null)
            {
                onCustomInput();
            }
            if (this.useMouse && (mCurrentSelection != null))
            {
                if ((this.cancelKey0 != KeyCode.None) && Input.GetKeyDown(this.cancelKey0))
                {
                    currentScheme = ControlScheme.Controller;
                    currentKey = this.cancelKey0;
                    selectedObject = null;
                }
                else if ((this.cancelKey1 != KeyCode.None) && Input.GetKeyDown(this.cancelKey1))
                {
                    currentScheme = ControlScheme.Controller;
                    currentKey = this.cancelKey1;
                    selectedObject = null;
                }
            }
            if (mCurrentSelection == null)
            {
                inputHasFocus = false;
            }
            if (mCurrentSelection != null)
            {
                this.ProcessOthers();
            }
            if (this.useMouse && (mHover != null))
            {
                float num = string.IsNullOrEmpty(this.scrollAxisName) ? 0f : Input.GetAxis(this.scrollAxisName);
                if (num != 0f)
                {
                    Notify(mHover, "OnScroll", num);
                }
                if ((showTooltips && (this.mTooltipTime != 0f)) && (((this.mTooltipTime < RealTime.time) || Input.GetKey(KeyCode.LeftShift)) || Input.GetKey(KeyCode.RightShift)))
                {
                    this.mTooltip = mHover;
                    this.ShowTooltip(true);
                }
            }
            current = null;
        }
    }

    public Camera cachedCamera
    {
        get
        {
            if (this.mCam == null)
            {
                this.mCam = base.camera;
            }
            return this.mCam;
        }
    }

    public static Ray currentRay
    {
        get
        {
            return (((currentCamera == null) || (currentTouch == null)) ? new Ray() : currentCamera.ScreenPointToRay((Vector3) currentTouch.pos));
        }
    }

    public static int dragCount
    {
        get
        {
            int num = 0;
            foreach (KeyValuePair<int, MouseOrTouch> pair in mTouches)
            {
                if (pair.Value.dragged != null)
                {
                    num++;
                }
            }
            for (int i = 0; i < mMouse.Length; i++)
            {
                if (mMouse[i].dragged != null)
                {
                    num++;
                }
            }
            if (controller.dragged != null)
            {
                num++;
            }
            return num;
        }
    }

    public static UICamera eventHandler
    {
        get
        {
            for (int i = 0; i < list.size; i++)
            {
                UICamera camera = list.buffer[i];
                if (((camera != null) && camera.enabled) && NGUITools.GetActive(camera.gameObject))
                {
                    return camera;
                }
            }
            return null;
        }
    }

    private bool handlesEvents
    {
        get
        {
            return (eventHandler == this);
        }
    }

    public static Camera mainCamera
    {
        get
        {
            UICamera eventHandler = UICamera.eventHandler;
            return ((eventHandler == null) ? null : eventHandler.cachedCamera);
        }
    }

    public static GameObject selectedObject
    {
        get
        {
            return mCurrentSelection;
        }
        set
        {
            SetSelection(value, currentScheme);
        }
    }

    [Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
    public bool stickyPress
    {
        get
        {
            return true;
        }
    }

    public static int touchCount
    {
        get
        {
            int num = 0;
            foreach (KeyValuePair<int, MouseOrTouch> pair in mTouches)
            {
                if (pair.Value.pressed != null)
                {
                    num++;
                }
            }
            for (int i = 0; i < mMouse.Length; i++)
            {
                if (mMouse[i].pressed != null)
                {
                    num++;
                }
            }
            if (controller.pressed != null)
            {
                num++;
            }
            return num;
        }
    }

    [CompilerGenerated]
    private sealed class <ChangeSelection>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal UICamera <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    return true;

                case 1:
                    UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", false);
                    UICamera.mCurrentSelection = UICamera.mNextSelection;
                    UICamera.mNextSelection = null;
                    if (UICamera.mCurrentSelection == null)
                    {
                        UICamera.inputHasFocus = false;
                        break;
                    }
                    UICamera.current = this.<>f__this;
                    UICamera.currentCamera = this.<>f__this.mCam;
                    UICamera.currentScheme = UICamera.mNextScheme;
                    UICamera.inputHasFocus = UICamera.mCurrentSelection.GetComponent<UIInput>() != null;
                    UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", true);
                    UICamera.current = null;
                    break;

                default:
                    goto Label_00D4;
            }
            this.$PC = -1;
        Label_00D4:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    public enum ClickNotification
    {
        None,
        Always,
        BasedOnDelta
    }

    public enum ControlScheme
    {
        Mouse,
        Touch,
        Controller
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct DepthEntry
    {
        public int depth;
        public RaycastHit hit;
        public GameObject go;
    }

    public enum EventType
    {
        World_3D,
        UI_3D,
        World_2D,
        UI_2D
    }

    public class MouseOrTouch
    {
        public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;
        public float clickTime;
        public GameObject current;
        public Vector2 delta;
        public GameObject dragged;
        public bool dragStarted;
        public GameObject last;
        public Vector2 lastPos;
        public Vector2 pos;
        public GameObject pressed;
        public Camera pressedCam;
        public bool pressStarted;
        public Vector2 totalDelta;
        public bool touchBegan = true;
    }

    public delegate void OnCustomInput();

    public delegate void OnScreenResize();
}

