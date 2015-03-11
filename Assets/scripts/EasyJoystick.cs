using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class EasyJoystick : MonoBehaviour
{
    private Vector2 anchorPosition = Vector2.zero;
    public DynamicArea area;
    public Color areaColor = Color.white;
    private Rect areaRect;
    public Texture areaTexture;
    public float clampXMax;
    public float clampXMin;
    public float clampYMax;
    public float clampYMin;
    private Rect deadRect;
    public Texture deadTexture;
    public float deadZone = 20f;
    [SerializeField]
    private bool dynamicJoystick;
    public bool enable = true;
    public bool enableInertia;
    public bool enableSmoothing;
    public bool enableXAutoStab;
    public bool enableXaxis = true;
    public bool enableXClamp;
    public bool enableYAutoStab;
    public bool enableYaxis = true;
    public bool enableYClamp;
    public int guiDepth;
    [SerializeField]
    public Vector2 inertia = new Vector2(100f, 100f);
    [SerializeField]
    private InteractionType interaction;
    public bool inverseXAxis;
    public bool inverseYAxis;
    public bool isActivated = true;
    public bool isUseGuiLayout = true;
    [SerializeField]
    private JoystickAnchor joyAnchor = JoystickAnchor.LowerLeft;
    private Vector2 joystickAxis;
    private Vector2 joystickCenter;
    private int joystickIndex = -1;
    [SerializeField]
    private Vector2 joystickPositionOffset = Vector2.zero;
    private Vector2 joystickTouch;
    private Vector2 joystickValue;
    public Broadcast messageMode;
    public GameObject receiverGameObject;
    public bool resetFingerExit;
    [SerializeField]
    private bool restrictArea;
    public bool selected;
    private bool sendEnd = true;
    public bool showAppearance;
    public bool showDeadZone = true;
    public bool showDebugRadius;
    public bool showInteraction;
    public bool showPosition = true;
    public bool showProperties = true;
    public bool showTouch = true;
    public bool showZone = true;
    [SerializeField]
    public Vector2 smoothing = new Vector2(2f, 2f);
    public Vector2 speed;
    [SerializeField]
    private float stabSpeedX = 20f;
    [SerializeField]
    private float stabSpeedY = 20f;
    private float startXLocalAngle;
    private float startYLocalAngle;
    [SerializeField]
    private float thresholdX = 0.01f;
    [SerializeField]
    private float thresholdY = 0.01f;
    public Color touchColor = Color.white;
    [SerializeField]
    private float touchSize = 30f;
    private float touchSizeCoef;
    public Texture touchTexture;
    public bool useBroadcast;
    public bool useFixedUpdate;
    private bool virtualJoystick = true;
    public bool visible = true;
    public AxisInfluenced xAI;
    public CharacterController xAxisCharacterController;
    public float xAxisGravity;
    [SerializeField]
    private Transform xAxisTransform;
    [SerializeField]
    private PropertiesInfluenced xTI;
    public AxisInfluenced yAI;
    public CharacterController yAxisCharacterController;
    public float yAxisGravity;
    [SerializeField]
    private Transform yAxisTransform;
    [SerializeField]
    private PropertiesInfluenced yTI;
    [SerializeField]
    private float zoneRadius = 100f;

    public static  event JoystickDoubleTapHandler On_JoystickDoubleTap;

    public static  event JoystickMoveHandler On_JoystickMove;

    public static  event JoystickMoveEndHandler On_JoystickMoveEnd;

    public static  event JoystickMoveStartHandler On_JoystickMoveStart;

    public static  event JoystickTapHandler On_JoystickTap;

    public static  event JoystickTouchStartHandler On_JoystickTouchStart;

    public static  event JoystickTouchUpHandler On_JoystickTouchUp;

    private float ComputeDeadZone()
    {
        float num2 = Mathf.Max(this.joystickTouch.magnitude, 0.1f);
        if (this.restrictArea)
        {
            return ((Mathf.Max((float) (num2 - this.deadZone), (float) 0f) / ((this.zoneRadius - this.touchSize) - this.deadZone)) / num2);
        }
        return ((Mathf.Max((float) (num2 - this.deadZone), (float) 0f) / (this.zoneRadius - this.deadZone)) / num2);
    }

    private void ComputeJoystickAnchor(JoystickAnchor anchor)
    {
        float touchSize = 0f;
        if (!this.restrictArea)
        {
            touchSize = this.touchSize;
        }
        switch (anchor)
        {
            case JoystickAnchor.None:
                this.anchorPosition = Vector2.zero;
                break;

            case JoystickAnchor.UpperLeft:
                this.anchorPosition = new Vector2(this.zoneRadius + touchSize, this.zoneRadius + touchSize);
                break;

            case JoystickAnchor.UpperCenter:
                this.anchorPosition = new Vector2(VirtualScreen.width / 2f, this.zoneRadius + touchSize);
                break;

            case JoystickAnchor.UpperRight:
                this.anchorPosition = new Vector2((VirtualScreen.width - this.zoneRadius) - touchSize, this.zoneRadius + touchSize);
                break;

            case JoystickAnchor.MiddleLeft:
                this.anchorPosition = new Vector2(this.zoneRadius + touchSize, VirtualScreen.height / 2f);
                break;

            case JoystickAnchor.MiddleCenter:
                this.anchorPosition = new Vector2(VirtualScreen.width / 2f, VirtualScreen.height / 2f);
                break;

            case JoystickAnchor.MiddleRight:
                this.anchorPosition = new Vector2((VirtualScreen.width - this.zoneRadius) - touchSize, VirtualScreen.height / 2f);
                break;

            case JoystickAnchor.LowerLeft:
                this.anchorPosition = new Vector2(this.zoneRadius + touchSize, (VirtualScreen.height - this.zoneRadius) - touchSize);
                break;

            case JoystickAnchor.LowerCenter:
                this.anchorPosition = new Vector2(VirtualScreen.width / 2f, (VirtualScreen.height - this.zoneRadius) - touchSize);
                break;

            case JoystickAnchor.LowerRight:
                this.anchorPosition = new Vector2((VirtualScreen.width - this.zoneRadius) - touchSize, (VirtualScreen.height - this.zoneRadius) - touchSize);
                break;
        }
        this.areaRect = new Rect((this.anchorPosition.x + this.joystickCenter.x) - this.zoneRadius, (this.anchorPosition.y + this.joystickCenter.y) - this.zoneRadius, this.zoneRadius * 2f, this.zoneRadius * 2f);
        this.deadRect = new Rect((this.anchorPosition.x + this.joystickCenter.x) - this.deadZone, (this.anchorPosition.y + this.joystickCenter.y) - this.deadZone, this.deadZone * 2f, this.deadZone * 2f);
    }

    private void CreateEvent(MessageName message)
    {
        MovingJoystick move = new MovingJoystick {
            joystickName = base.gameObject.name,
            joystickAxis = this.joystickAxis,
            joystickValue = this.joystickValue,
            fingerIndex = this.joystickIndex,
            joystick = this
        };
        if (!this.useBroadcast)
        {
            switch (message)
            {
                case MessageName.On_JoystickMoveStart:
                    if (On_JoystickMoveStart != null)
                    {
                        On_JoystickMoveStart(move);
                    }
                    break;

                case MessageName.On_JoystickTouchStart:
                    if (On_JoystickTouchStart != null)
                    {
                        On_JoystickTouchStart(move);
                    }
                    break;

                case MessageName.On_JoystickTouchUp:
                    if (On_JoystickTouchUp != null)
                    {
                        On_JoystickTouchUp(move);
                    }
                    break;

                case MessageName.On_JoystickMove:
                    if (On_JoystickMove != null)
                    {
                        On_JoystickMove(move);
                    }
                    break;

                case MessageName.On_JoystickMoveEnd:
                    if (On_JoystickMoveEnd != null)
                    {
                        On_JoystickMoveEnd(move);
                    }
                    break;

                case MessageName.On_JoystickTap:
                    if (On_JoystickTap != null)
                    {
                        On_JoystickTap(move);
                    }
                    break;

                case MessageName.On_JoystickDoubleTap:
                    if (On_JoystickDoubleTap != null)
                    {
                        On_JoystickDoubleTap(move);
                    }
                    break;
            }
        }
        else if (this.useBroadcast)
        {
            if (this.receiverGameObject != null)
            {
                switch (this.messageMode)
                {
                    case Broadcast.SendMessage:
                        this.receiverGameObject.SendMessage(message.ToString(), move, SendMessageOptions.DontRequireReceiver);
                        break;

                    case Broadcast.SendMessageUpwards:
                        this.receiverGameObject.SendMessageUpwards(message.ToString(), move, SendMessageOptions.DontRequireReceiver);
                        break;

                    case Broadcast.BroadcastMessage:
                        this.receiverGameObject.BroadcastMessage(message.ToString(), move, SendMessageOptions.DontRequireReceiver);
                        break;
                }
            }
            else
            {
                Debug.LogError("Joystick : " + base.gameObject.name + " : you must setup receiver gameobject");
            }
        }
    }

    private void DoActionDirect(Transform axisTransform, PropertiesInfluenced inlfuencedProperty, Vector3 axis, float sensibility, CharacterController charact)
    {
        switch (inlfuencedProperty)
        {
            case PropertiesInfluenced.Rotate:
                axisTransform.Rotate((Vector3) ((axis * sensibility) * Time.deltaTime), Space.World);
                break;

            case PropertiesInfluenced.RotateLocal:
                axisTransform.Rotate((Vector3) ((axis * sensibility) * Time.deltaTime), Space.Self);
                break;

            case PropertiesInfluenced.Translate:
                if (charact != null)
                {
                    Vector3 vector = new Vector3(axis.x, axis.y, axis.z) {
                        y = -(this.yAxisGravity + this.xAxisGravity)
                    };
                    charact.Move((Vector3) ((vector * sensibility) * Time.deltaTime));
                    break;
                }
                axisTransform.Translate((Vector3) ((axis * sensibility) * Time.deltaTime), Space.World);
                break;

            case PropertiesInfluenced.TranslateLocal:
                if (charact != null)
                {
                    Vector3 vector2 = (Vector3) (charact.transform.TransformDirection(axis) * sensibility);
                    vector2.y = -(this.yAxisGravity + this.xAxisGravity);
                    charact.Move((Vector3) (vector2 * Time.deltaTime));
                    break;
                }
                axisTransform.Translate((Vector3) ((axis * sensibility) * Time.deltaTime), Space.Self);
                break;

            case PropertiesInfluenced.Scale:
                axisTransform.localScale += (Vector3) ((axis * sensibility) * Time.deltaTime);
                break;
        }
    }

    private void DoAngleLimitation(Transform axisTransform, AxisInfluenced axisInfluenced, float clampMin, float clampMax, float startAngle)
    {
        float x = 0f;
        switch (axisInfluenced)
        {
            case AxisInfluenced.X:
                x = axisTransform.localRotation.eulerAngles.x;
                break;

            case AxisInfluenced.Y:
                x = axisTransform.localRotation.eulerAngles.y;
                break;

            case AxisInfluenced.Z:
                x = axisTransform.localRotation.eulerAngles.z;
                break;
        }
        if ((x <= 360f) && (x >= 180f))
        {
            x -= 360f;
        }
        x = Mathf.Clamp(x, -clampMax, clampMin);
        switch (axisInfluenced)
        {
            case AxisInfluenced.X:
                axisTransform.localEulerAngles = new Vector3(x, axisTransform.localEulerAngles.y, axisTransform.localEulerAngles.z);
                break;

            case AxisInfluenced.Y:
                axisTransform.localEulerAngles = new Vector3(axisTransform.localEulerAngles.x, x, axisTransform.localEulerAngles.z);
                break;

            case AxisInfluenced.Z:
                axisTransform.localEulerAngles = new Vector3(axisTransform.localEulerAngles.x, axisTransform.localEulerAngles.y, x);
                break;
        }
    }

    private void DoAutoStabilisation(Transform axisTransform, AxisInfluenced axisInfluenced, float threshold, float speed, float startAngle)
    {
        float x = 0f;
        switch (axisInfluenced)
        {
            case AxisInfluenced.X:
                x = axisTransform.localRotation.eulerAngles.x;
                break;

            case AxisInfluenced.Y:
                x = axisTransform.localRotation.eulerAngles.y;
                break;

            case AxisInfluenced.Z:
                x = axisTransform.localRotation.eulerAngles.z;
                break;
        }
        if ((x <= 360f) && (x >= 180f))
        {
            x -= 360f;
        }
        if ((x > (startAngle - threshold)) || (x < (startAngle + threshold)))
        {
            float num2 = 0f;
            Vector3 zero = Vector3.zero;
            if (x > (startAngle - threshold))
            {
                num2 = x + ((((speed / 100f) * Mathf.Abs((float) (x - startAngle))) * Time.deltaTime) * -1f);
            }
            if (x < (startAngle + threshold))
            {
                num2 = x + (((speed / 100f) * Mathf.Abs((float) (x - startAngle))) * Time.deltaTime);
            }
            switch (axisInfluenced)
            {
                case AxisInfluenced.X:
                    zero = new Vector3(num2, axisTransform.localRotation.eulerAngles.y, axisTransform.localRotation.eulerAngles.z);
                    break;

                case AxisInfluenced.Y:
                    zero = new Vector3(axisTransform.localRotation.eulerAngles.x, num2, axisTransform.localRotation.eulerAngles.z);
                    break;

                case AxisInfluenced.Z:
                    zero = new Vector3(axisTransform.localRotation.eulerAngles.x, axisTransform.localRotation.eulerAngles.y, num2);
                    break;
            }
            axisTransform.localRotation = Quaternion.Euler(zero);
        }
    }

    private void FixedUpdate()
    {
        if (this.useFixedUpdate && this.enable)
        {
            this.UpdateJoystick();
        }
    }

    private Vector3 GetInfluencedAxis(AxisInfluenced axisInfluenced)
    {
        Vector3 zero = Vector3.zero;
        switch (axisInfluenced)
        {
            case AxisInfluenced.X:
                return Vector3.right;

            case AxisInfluenced.Y:
                return Vector3.up;

            case AxisInfluenced.Z:
                return Vector3.forward;

            case AxisInfluenced.XYZ:
                return Vector3.one;
        }
        return zero;
    }

    private float GetStartAutoStabAngle(Transform axisTransform, AxisInfluenced axisInfluenced)
    {
        float x = 0f;
        if (axisTransform != null)
        {
            switch (axisInfluenced)
            {
                case AxisInfluenced.X:
                    x = axisTransform.localRotation.eulerAngles.x;
                    break;

                case AxisInfluenced.Y:
                    x = axisTransform.localRotation.eulerAngles.y;
                    break;

                case AxisInfluenced.Z:
                    x = axisTransform.localRotation.eulerAngles.z;
                    break;
            }
            if ((x <= 360f) && (x >= 180f))
            {
                x -= 360f;
            }
        }
        return x;
    }

    private void On_DoubleTap(Gesture gesture)
    {
        if (this.visible && (((!gesture.isHoverReservedArea && this.dynamicJoystick) || !this.dynamicJoystick) && (this.isActivated && (gesture.fingerIndex == this.joystickIndex))))
        {
            this.CreateEvent(MessageName.On_JoystickDoubleTap);
        }
    }

    public void On_Manual(Vector2 movement)
    {
        if (this.isActivated)
        {
            if (movement != Vector2.zero)
            {
                if (!this.virtualJoystick)
                {
                    this.virtualJoystick = true;
                    this.CreateEvent(MessageName.On_JoystickTouchStart);
                }
                this.joystickIndex = 0;
                this.joystickTouch.x = movement.x * (this.areaRect.width / 2f);
                this.joystickTouch.y = movement.y * (this.areaRect.height / 2f);
            }
            else if (this.virtualJoystick)
            {
                this.virtualJoystick = false;
                this.joystickIndex = -1;
                this.CreateEvent(MessageName.On_JoystickTouchUp);
            }
        }
    }

    private void On_SimpleTap(Gesture gesture)
    {
        if (this.visible && (((!gesture.isHoverReservedArea && this.dynamicJoystick) || !this.dynamicJoystick) && (this.isActivated && (gesture.fingerIndex == this.joystickIndex))))
        {
            this.CreateEvent(MessageName.On_JoystickTap);
        }
    }

    private void On_TouchDown(Gesture gesture)
    {
        if (this.visible && (((!gesture.isHoverReservedArea && this.dynamicJoystick) || !this.dynamicJoystick) && this.isActivated))
        {
            Vector2 vector = new Vector2((this.anchorPosition.x + this.joystickCenter.x) * VirtualScreen.xRatio, (VirtualScreen.height - (this.anchorPosition.y + this.joystickCenter.y)) * VirtualScreen.yRatio);
            if (gesture.fingerIndex == this.joystickIndex)
            {
                Vector2 vector2 = gesture.position - vector;
                if (((vector2.sqrMagnitude < ((this.zoneRadius * VirtualScreen.xRatio) * (this.zoneRadius * VirtualScreen.xRatio))) && this.resetFingerExit) || !this.resetFingerExit)
                {
                    this.joystickTouch = new Vector2(gesture.position.x, gesture.position.y) - vector;
                    this.joystickTouch = new Vector2(this.joystickTouch.x / VirtualScreen.xRatio, this.joystickTouch.y / VirtualScreen.yRatio);
                    if (!this.enableXaxis)
                    {
                        this.joystickTouch.x = 0f;
                    }
                    if (!this.enableYaxis)
                    {
                        this.joystickTouch.y = 0f;
                    }
                    Vector2 vector3 = (Vector2) (this.joystickTouch / (this.zoneRadius - this.touchSizeCoef));
                    if (vector3.sqrMagnitude > 1f)
                    {
                        this.joystickTouch.Normalize();
                        this.joystickTouch = (Vector2) (this.joystickTouch * (this.zoneRadius - this.touchSizeCoef));
                    }
                }
                else
                {
                    this.On_TouchUp(gesture);
                }
            }
        }
    }

    private void On_TouchStart(Gesture gesture)
    {
        if (this.visible && (((!gesture.isHoverReservedArea && this.dynamicJoystick) || !this.dynamicJoystick) && this.isActivated))
        {
            if (!this.dynamicJoystick)
            {
                Vector2 vector = new Vector2((this.anchorPosition.x + this.joystickCenter.x) * VirtualScreen.xRatio, ((VirtualScreen.height - this.anchorPosition.y) - this.joystickCenter.y) * VirtualScreen.yRatio);
                Vector2 vector2 = gesture.position - vector;
                if (vector2.sqrMagnitude < ((this.zoneRadius * VirtualScreen.xRatio) * (this.zoneRadius * VirtualScreen.xRatio)))
                {
                    this.joystickIndex = gesture.fingerIndex;
                    this.CreateEvent(MessageName.On_JoystickTouchStart);
                }
            }
            else if (!this.virtualJoystick)
            {
                switch (this.area)
                {
                    case DynamicArea.FullScreen:
                        this.virtualJoystick = true;
                        break;

                    case DynamicArea.Left:
                        if (gesture.position.x < (Screen.width / 2))
                        {
                            this.virtualJoystick = true;
                        }
                        break;

                    case DynamicArea.Right:
                        if (gesture.position.x > (Screen.width / 2))
                        {
                            this.virtualJoystick = true;
                        }
                        break;

                    case DynamicArea.Top:
                        if (gesture.position.y > (Screen.height / 2))
                        {
                            this.virtualJoystick = true;
                        }
                        break;

                    case DynamicArea.Bottom:
                        if (gesture.position.y < (Screen.height / 2))
                        {
                            this.virtualJoystick = true;
                        }
                        break;

                    case DynamicArea.TopLeft:
                        if ((gesture.position.y > (Screen.height / 2)) && (gesture.position.x < (Screen.width / 2)))
                        {
                            this.virtualJoystick = true;
                        }
                        break;

                    case DynamicArea.TopRight:
                        if ((gesture.position.y > (Screen.height / 2)) && (gesture.position.x > (Screen.width / 2)))
                        {
                            this.virtualJoystick = true;
                        }
                        break;

                    case DynamicArea.BottomLeft:
                        if ((gesture.position.y < (Screen.height / 2)) && (gesture.position.x < (Screen.width / 2)))
                        {
                            this.virtualJoystick = true;
                        }
                        break;

                    case DynamicArea.BottomRight:
                        if ((gesture.position.y < (Screen.height / 2)) && (gesture.position.x > (Screen.width / 2)))
                        {
                            this.virtualJoystick = true;
                        }
                        break;
                }
                if (this.virtualJoystick)
                {
                    this.joystickCenter = new Vector2(gesture.position.x / VirtualScreen.xRatio, VirtualScreen.height - (gesture.position.y / VirtualScreen.yRatio));
                    this.JoyAnchor = JoystickAnchor.None;
                    this.joystickIndex = gesture.fingerIndex;
                }
            }
        }
    }

    private void On_TouchUp(Gesture gesture)
    {
        if (this.visible && (gesture.fingerIndex == this.joystickIndex))
        {
            this.joystickIndex = -1;
            if (this.dynamicJoystick)
            {
                this.virtualJoystick = false;
            }
            this.CreateEvent(MessageName.On_JoystickTouchUp);
        }
    }

    private void OnDestroy()
    {
        EasyTouch.On_TouchStart -= new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.On_TouchUp);
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_SimpleTap -= new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
        EasyTouch.On_DoubleTap -= new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
        if (Application.isPlaying && (EasyTouch.instance != null))
        {
            EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
        }
    }

    private void OnDisable()
    {
        EasyTouch.On_TouchStart -= new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.On_TouchUp);
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_SimpleTap -= new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
        EasyTouch.On_DoubleTap -= new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
        if (Application.isPlaying && (EasyTouch.instance != null))
        {
            EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
        }
    }

    private void OnDrawGizmos()
    {
    }

    private void OnEnable()
    {
        EasyTouch.On_TouchStart += new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchUp += new EasyTouch.TouchUpHandler(this.On_TouchUp);
        EasyTouch.On_TouchDown += new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_SimpleTap += new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
        EasyTouch.On_DoubleTap += new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
    }

    private void OnGUI()
    {
        if (!this.enable || !this.visible)
        {
            if (Application.isPlaying)
            {
                EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
            }
        }
        else
        {
            GUI.depth = this.guiDepth;
            base.useGUILayout = this.isUseGuiLayout;
            if ((this.dynamicJoystick && Application.isEditor) && !Application.isPlaying)
            {
                switch (this.area)
                {
                    case DynamicArea.FullScreen:
                        this.ComputeJoystickAnchor(JoystickAnchor.MiddleCenter);
                        break;

                    case DynamicArea.Left:
                        this.ComputeJoystickAnchor(JoystickAnchor.MiddleLeft);
                        break;

                    case DynamicArea.Right:
                        this.ComputeJoystickAnchor(JoystickAnchor.MiddleRight);
                        break;

                    case DynamicArea.Top:
                        this.ComputeJoystickAnchor(JoystickAnchor.UpperCenter);
                        break;

                    case DynamicArea.Bottom:
                        this.ComputeJoystickAnchor(JoystickAnchor.LowerCenter);
                        break;

                    case DynamicArea.TopLeft:
                        this.ComputeJoystickAnchor(JoystickAnchor.UpperLeft);
                        break;

                    case DynamicArea.TopRight:
                        this.ComputeJoystickAnchor(JoystickAnchor.UpperRight);
                        break;

                    case DynamicArea.BottomLeft:
                        this.ComputeJoystickAnchor(JoystickAnchor.LowerLeft);
                        break;

                    case DynamicArea.BottomRight:
                        this.ComputeJoystickAnchor(JoystickAnchor.LowerRight);
                        break;
                }
            }
            if (Application.isEditor && !Application.isPlaying)
            {
                VirtualScreen.ComputeVirtualScreen();
                this.ComputeJoystickAnchor(this.joyAnchor);
            }
            VirtualScreen.SetGuiScaleMatrix();
            if ((((this.showZone && (this.areaTexture != null)) && !this.dynamicJoystick) || ((this.showZone && this.dynamicJoystick) && (this.virtualJoystick && (this.areaTexture != null)))) || ((this.dynamicJoystick && Application.isEditor) && !Application.isPlaying))
            {
                if (this.isActivated)
                {
                    GUI.color = this.areaColor;
                    if (Application.isPlaying && !this.dynamicJoystick)
                    {
                        EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
                        EasyTouch.instance.reservedVirtualAreas.Add(this.areaRect);
                    }
                }
                else
                {
                    GUI.color = new Color(this.areaColor.r, this.areaColor.g, this.areaColor.b, 0.2f);
                    if (Application.isPlaying && !this.dynamicJoystick)
                    {
                        EasyTouch.instance.reservedVirtualAreas.Remove(this.areaRect);
                    }
                }
                if ((this.showDebugRadius && Application.isEditor) && (this.selected && !Application.isPlaying))
                {
                    GUI.Box(this.areaRect, string.Empty);
                }
                GUI.DrawTexture(this.areaRect, this.areaTexture, ScaleMode.StretchToFill, true);
            }
            if ((((this.showTouch && (this.touchTexture != null)) && !this.dynamicJoystick) || ((this.showTouch && this.dynamicJoystick) && (this.virtualJoystick && (this.touchTexture != null)))) || ((this.dynamicJoystick && Application.isEditor) && !Application.isPlaying))
            {
                if (this.isActivated)
                {
                    GUI.color = this.touchColor;
                }
                else
                {
                    GUI.color = new Color(this.touchColor.r, this.touchColor.g, this.touchColor.b, 0.2f);
                }
                GUI.DrawTexture(new Rect((this.anchorPosition.x + this.joystickCenter.x) + (this.joystickTouch.x - this.touchSize), (this.anchorPosition.y + this.joystickCenter.y) - (this.joystickTouch.y + this.touchSize), this.touchSize * 2f, this.touchSize * 2f), this.touchTexture, ScaleMode.ScaleToFit, true);
            }
            if ((((this.showDeadZone && (this.deadTexture != null)) && !this.dynamicJoystick) || ((this.showDeadZone && this.dynamicJoystick) && (this.virtualJoystick && (this.deadTexture != null)))) || ((this.dynamicJoystick && Application.isEditor) && !Application.isPlaying))
            {
                GUI.DrawTexture(this.deadRect, this.deadTexture, ScaleMode.ScaleToFit, true);
            }
            GUI.color = Color.white;
        }
    }

    private void OnLevelWasLoaded()
    {
        this.joystickIndex = -1;
    }

    private void Start()
    {
        if (!this.dynamicJoystick)
        {
            this.joystickCenter = this.joystickPositionOffset;
            this.ComputeJoystickAnchor(this.joyAnchor);
            this.virtualJoystick = true;
        }
        else
        {
            this.virtualJoystick = false;
        }
        VirtualScreen.ComputeVirtualScreen();
        this.startXLocalAngle = this.GetStartAutoStabAngle(this.xAxisTransform, this.xAI);
        this.startYLocalAngle = this.GetStartAutoStabAngle(this.yAxisTransform, this.yAI);
        this.RestrictArea = this.restrictArea;
    }

    private void Update()
    {
        if (!this.useFixedUpdate && this.enable)
        {
            this.UpdateJoystick();
        }
    }

    private void UpdateDirect()
    {
        if (this.xAxisTransform != null)
        {
            Vector3 influencedAxis = this.GetInfluencedAxis(this.xAI);
            this.DoActionDirect(this.xAxisTransform, this.xTI, influencedAxis, this.joystickValue.x, this.xAxisCharacterController);
            if (this.enableXClamp && (this.xTI == PropertiesInfluenced.RotateLocal))
            {
                this.DoAngleLimitation(this.xAxisTransform, this.xAI, this.clampXMin, this.clampXMax, this.startXLocalAngle);
            }
        }
        if (this.YAxisTransform != null)
        {
            Vector3 axis = this.GetInfluencedAxis(this.yAI);
            this.DoActionDirect(this.yAxisTransform, this.yTI, axis, this.joystickValue.y, this.yAxisCharacterController);
            if (this.enableYClamp && (this.yTI == PropertiesInfluenced.RotateLocal))
            {
                this.DoAngleLimitation(this.yAxisTransform, this.yAI, this.clampYMin, this.clampYMax, this.startYLocalAngle);
            }
        }
    }

    private void UpdateGravity()
    {
        if (this.joystickAxis == Vector2.zero)
        {
            if ((this.xAxisCharacterController != null) && (this.xAxisGravity > 0f))
            {
                this.xAxisCharacterController.Move((Vector3) ((Vector3.down * this.xAxisGravity) * Time.deltaTime));
            }
            if ((this.yAxisCharacterController != null) && (this.yAxisGravity > 0f))
            {
                this.yAxisCharacterController.Move((Vector3) ((Vector3.down * this.yAxisGravity) * Time.deltaTime));
            }
        }
    }

    private void UpdateJoystick()
    {
        if (Application.isPlaying)
        {
            if (EasyTouch.GetTouchCount() == 0)
            {
                this.joystickIndex = -1;
                if (this.dynamicJoystick)
                {
                    this.virtualJoystick = false;
                }
            }
            if (this.isActivated)
            {
                if ((this.joystickIndex == -1) || ((this.joystickAxis == Vector2.zero) && (this.joystickIndex > -1)))
                {
                    if (this.enableXAutoStab)
                    {
                        this.DoAutoStabilisation(this.xAxisTransform, this.xAI, this.thresholdX, this.stabSpeedX, this.startXLocalAngle);
                    }
                    if (this.enableYAutoStab)
                    {
                        this.DoAutoStabilisation(this.yAxisTransform, this.yAI, this.thresholdY, this.stabSpeedY, this.startYLocalAngle);
                    }
                }
                if (!this.dynamicJoystick)
                {
                    this.joystickCenter = this.joystickPositionOffset;
                }
                if (this.joystickIndex == -1)
                {
                    if (!this.enableSmoothing)
                    {
                        this.joystickTouch = Vector2.zero;
                    }
                    else if (this.joystickTouch.sqrMagnitude > 0.0001)
                    {
                        this.joystickTouch = new Vector2(this.joystickTouch.x - ((this.joystickTouch.x * this.smoothing.x) * Time.deltaTime), this.joystickTouch.y - ((this.joystickTouch.y * this.smoothing.y) * Time.deltaTime));
                    }
                    else
                    {
                        this.joystickTouch = Vector2.zero;
                    }
                }
                Vector2 vector = new Vector2(this.joystickAxis.x, this.joystickAxis.y);
                float num = this.ComputeDeadZone();
                this.joystickAxis = new Vector2(this.joystickTouch.x * num, this.joystickTouch.y * num);
                if (this.inverseXAxis)
                {
                    this.joystickAxis.x *= -1f;
                }
                if (this.inverseYAxis)
                {
                    this.joystickAxis.y *= -1f;
                }
                Vector2 vector2 = new Vector2(this.speed.x * this.joystickAxis.x, this.speed.y * this.joystickAxis.y);
                if (this.enableInertia)
                {
                    Vector2 vector3 = vector2 - this.joystickValue;
                    vector3.x /= this.inertia.x;
                    vector3.y /= this.inertia.y;
                    this.joystickValue += vector3;
                }
                else
                {
                    this.joystickValue = vector2;
                }
                if (((vector == Vector2.zero) && (this.joystickAxis != Vector2.zero)) && ((this.interaction != InteractionType.Direct) && (this.interaction != InteractionType.Include)))
                {
                    this.CreateEvent(MessageName.On_JoystickMoveStart);
                }
                this.UpdateGravity();
                if (this.joystickAxis != Vector2.zero)
                {
                    this.sendEnd = false;
                    switch (this.interaction)
                    {
                        case InteractionType.Direct:
                            this.UpdateDirect();
                            break;

                        case InteractionType.EventNotification:
                            this.CreateEvent(MessageName.On_JoystickMove);
                            break;

                        case InteractionType.DirectAndEvent:
                            this.UpdateDirect();
                            this.CreateEvent(MessageName.On_JoystickMove);
                            break;
                    }
                }
                else if (!this.sendEnd)
                {
                    this.CreateEvent(MessageName.On_JoystickMoveEnd);
                    this.sendEnd = true;
                }
            }
        }
    }

    public bool DynamicJoystick
    {
        get
        {
            return this.dynamicJoystick;
        }
        set
        {
            if (!Application.isPlaying)
            {
                this.joystickIndex = -1;
                this.dynamicJoystick = value;
                if (this.dynamicJoystick)
                {
                    this.virtualJoystick = false;
                }
                else
                {
                    this.virtualJoystick = true;
                    this.joystickCenter = this.joystickPositionOffset;
                }
            }
        }
    }

    public Vector2 Inertia
    {
        get
        {
            return this.inertia;
        }
        set
        {
            this.inertia = value;
            if (this.inertia.x <= 0f)
            {
                this.inertia.x = 1f;
            }
            if (this.inertia.y <= 0f)
            {
                this.inertia.y = 1f;
            }
        }
    }

    public InteractionType Interaction
    {
        get
        {
            return this.interaction;
        }
        set
        {
            this.interaction = value;
            if ((this.interaction == InteractionType.Direct) || (this.interaction == InteractionType.Include))
            {
                this.useBroadcast = false;
            }
        }
    }

    public JoystickAnchor JoyAnchor
    {
        get
        {
            return this.joyAnchor;
        }
        set
        {
            this.joyAnchor = value;
            this.ComputeJoystickAnchor(this.joyAnchor);
        }
    }

    public Vector2 JoystickAxis
    {
        get
        {
            return this.joystickAxis;
        }
    }

    public Vector2 JoystickPositionOffset
    {
        get
        {
            return this.joystickPositionOffset;
        }
        set
        {
            this.joystickPositionOffset = value;
            this.joystickCenter = this.joystickPositionOffset;
            this.ComputeJoystickAnchor(this.joyAnchor);
        }
    }

    public Vector2 JoystickTouch
    {
        get
        {
            return new Vector2(this.joystickTouch.x / this.zoneRadius, this.joystickTouch.y / this.zoneRadius);
        }
        set
        {
            float x = Mathf.Clamp(value.x, -1f, 1f) * this.zoneRadius;
            float y = Mathf.Clamp(value.y, -1f, 1f) * this.zoneRadius;
            this.joystickTouch = new Vector2(x, y);
        }
    }

    public Vector2 JoystickValue
    {
        get
        {
            return this.joystickValue;
        }
    }

    public bool RestrictArea
    {
        get
        {
            return this.restrictArea;
        }
        set
        {
            this.restrictArea = value;
            if (this.restrictArea)
            {
                this.touchSizeCoef = this.touchSize;
            }
            else
            {
                this.touchSizeCoef = 0f;
            }
            this.ComputeJoystickAnchor(this.joyAnchor);
        }
    }

    public Vector2 Smoothing
    {
        get
        {
            return this.smoothing;
        }
        set
        {
            this.smoothing = value;
            if (this.smoothing.x < 0f)
            {
                this.smoothing.x = 0f;
            }
            if (this.smoothing.y < 0f)
            {
                this.smoothing.y = 0f;
            }
        }
    }

    public float StabSpeedX
    {
        get
        {
            return this.stabSpeedX;
        }
        set
        {
            if (value <= 0f)
            {
                this.stabSpeedX = value * -1f;
            }
            else
            {
                this.stabSpeedX = value;
            }
        }
    }

    public float StabSpeedY
    {
        get
        {
            return this.stabSpeedY;
        }
        set
        {
            if (value <= 0f)
            {
                this.stabSpeedY = value * -1f;
            }
            else
            {
                this.stabSpeedY = value;
            }
        }
    }

    public float ThresholdX
    {
        get
        {
            return this.thresholdX;
        }
        set
        {
            if (value <= 0f)
            {
                this.thresholdX = value * -1f;
            }
            else
            {
                this.thresholdX = value;
            }
        }
    }

    public float ThresholdY
    {
        get
        {
            return this.thresholdY;
        }
        set
        {
            if (value <= 0f)
            {
                this.thresholdY = value * -1f;
            }
            else
            {
                this.thresholdY = value;
            }
        }
    }

    public float TouchSize
    {
        get
        {
            return this.touchSize;
        }
        set
        {
            this.touchSize = value;
            if ((this.touchSize > (this.zoneRadius / 2f)) && this.restrictArea)
            {
                this.touchSize = this.zoneRadius / 2f;
            }
            this.ComputeJoystickAnchor(this.joyAnchor);
        }
    }

    public Transform XAxisTransform
    {
        get
        {
            return this.xAxisTransform;
        }
        set
        {
            this.xAxisTransform = value;
            if (this.xAxisTransform != null)
            {
                this.xAxisCharacterController = this.xAxisTransform.GetComponent<CharacterController>();
            }
            else
            {
                this.xAxisCharacterController = null;
                this.xAxisGravity = 0f;
            }
        }
    }

    public PropertiesInfluenced XTI
    {
        get
        {
            return this.xTI;
        }
        set
        {
            this.xTI = value;
            if (this.xTI != PropertiesInfluenced.RotateLocal)
            {
                this.enableXAutoStab = false;
                this.enableXClamp = false;
            }
        }
    }

    public Transform YAxisTransform
    {
        get
        {
            return this.yAxisTransform;
        }
        set
        {
            this.yAxisTransform = value;
            if (this.yAxisTransform != null)
            {
                this.yAxisCharacterController = this.yAxisTransform.GetComponent<CharacterController>();
            }
            else
            {
                this.yAxisCharacterController = null;
                this.yAxisGravity = 0f;
            }
        }
    }

    public PropertiesInfluenced YTI
    {
        get
        {
            return this.yTI;
        }
        set
        {
            this.yTI = value;
            if (this.yTI != PropertiesInfluenced.RotateLocal)
            {
                this.enableYAutoStab = false;
                this.enableYClamp = false;
            }
        }
    }

    public float ZoneRadius
    {
        get
        {
            return this.zoneRadius;
        }
        set
        {
            this.zoneRadius = value;
            this.ComputeJoystickAnchor(this.joyAnchor);
        }
    }

    public enum AxisInfluenced
    {
        X,
        Y,
        Z,
        XYZ
    }

    public enum Broadcast
    {
        SendMessage,
        SendMessageUpwards,
        BroadcastMessage
    }

    public enum DynamicArea
    {
        FullScreen,
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public enum InteractionType
    {
        Direct,
        Include,
        EventNotification,
        DirectAndEvent
    }

    public enum JoystickAnchor
    {
        None,
        UpperLeft,
        UpperCenter,
        UpperRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        LowerLeft,
        LowerCenter,
        LowerRight
    }

    public delegate void JoystickDoubleTapHandler(MovingJoystick move);

    public delegate void JoystickMoveEndHandler(MovingJoystick move);

    public delegate void JoystickMoveHandler(MovingJoystick move);

    public delegate void JoystickMoveStartHandler(MovingJoystick move);

    public delegate void JoystickTapHandler(MovingJoystick move);

    public delegate void JoystickTouchStartHandler(MovingJoystick move);

    public delegate void JoystickTouchUpHandler(MovingJoystick move);

    private enum MessageName
    {
        On_JoystickMoveStart,
        On_JoystickTouchStart,
        On_JoystickTouchUp,
        On_JoystickMove,
        On_JoystickMoveEnd,
        On_JoystickTap,
        On_JoystickDoubleTap
    }

    public enum PropertiesInfluenced
    {
        Rotate,
        RotateLocal,
        Translate,
        TranslateLocal,
        Scale
    }
}

