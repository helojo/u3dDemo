using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class EasyButton : MonoBehaviour
{
    [SerializeField]
    private Texture2D activeTexture;
    [SerializeField]
    private ButtonAnchor anchor = ButtonAnchor.LowerRight;
    public Color buttonActiveColor = Color.white;
    private int buttonFingerIndex = -1;
    public Color buttonNormalColor = Color.white;
    private Rect buttonRect;
    public ButtonState buttonState = ButtonState.None;
    private Color currentColor;
    private Texture2D currentTexture;
    public string downMethodName;
    public bool enable = true;
    private int frame;
    public int guiDepth;
    public InteractionType interaction;
    public bool isActivated = true;
    public bool isSwipeIn;
    public bool isSwipeOut;
    public bool isUseGuiLayout = true;
    public Broadcast messageMode;
    [SerializeField]
    private Texture2D normalTexture;
    [SerializeField]
    private Vector2 offset = Vector2.zero;
    public string pressMethodName;
    public GameObject receiverGameObject;
    [SerializeField]
    private Vector2 scale = Vector2.one;
    public bool selected;
    public bool showDebugArea = true;
    public bool showInspectorEvent;
    public bool showInspectorPosition = true;
    public bool showInspectorProperties = true;
    public bool showInspectorTexture;
    public string upMethodName;
    public bool useBroadcast;
    public bool useSpecificalMethod;

    public static  event ButtonDownHandler On_ButtonDown;

    public static  event ButtonPressHandler On_ButtonPress;

    public static  event ButtonUpHandler On_ButtonUp;

    private void ComputeButtonAnchor(ButtonAnchor anchor)
    {
        if (this.normalTexture != null)
        {
            Vector2 vector = new Vector2(this.normalTexture.width * this.scale.x, this.normalTexture.height * this.scale.y);
            Vector2 zero = Vector2.zero;
            switch (anchor)
            {
                case ButtonAnchor.UpperLeft:
                    zero = new Vector2(0f, 0f);
                    break;

                case ButtonAnchor.UpperCenter:
                    zero = new Vector2((VirtualScreen.width / 2f) - (vector.x / 2f), this.offset.y);
                    break;

                case ButtonAnchor.UpperRight:
                    zero = new Vector2(VirtualScreen.width - vector.x, 0f);
                    break;

                case ButtonAnchor.MiddleLeft:
                    zero = new Vector2(0f, (VirtualScreen.height / 2f) - (vector.y / 2f));
                    break;

                case ButtonAnchor.MiddleCenter:
                    zero = new Vector2((VirtualScreen.width / 2f) - (vector.x / 2f), (VirtualScreen.height / 2f) - (vector.y / 2f));
                    break;

                case ButtonAnchor.MiddleRight:
                    zero = new Vector2(VirtualScreen.width - vector.x, (VirtualScreen.height / 2f) - (vector.y / 2f));
                    break;

                case ButtonAnchor.LowerLeft:
                    zero = new Vector2(0f, VirtualScreen.height - vector.y);
                    break;

                case ButtonAnchor.LowerCenter:
                    zero = new Vector2((VirtualScreen.width / 2f) - (vector.x / 2f), VirtualScreen.height - vector.y);
                    break;

                case ButtonAnchor.LowerRight:
                    zero = new Vector2(VirtualScreen.width - vector.x, VirtualScreen.height - vector.y);
                    break;
            }
            this.buttonRect = new Rect(zero.x + this.offset.x, zero.y + this.offset.y, vector.x, vector.y);
        }
    }

    private void On_TouchDown(Gesture gesture)
    {
        if ((gesture.fingerIndex == this.buttonFingerIndex) || (this.isSwipeIn && (this.buttonState == ButtonState.None)))
        {
            if ((gesture.IsInRect(VirtualScreen.GetRealRect(this.buttonRect), true) && this.enable) && this.isActivated)
            {
                this.currentTexture = this.activeTexture;
                this.currentColor = this.buttonActiveColor;
                this.frame++;
                if (((this.buttonState == ButtonState.Down) || (this.buttonState == ButtonState.Press)) && (this.frame >= 2))
                {
                    this.RaiseEvent(MessageName.On_ButtonPress);
                    this.buttonState = ButtonState.Press;
                }
                if (this.buttonState == ButtonState.None)
                {
                    this.buttonFingerIndex = gesture.fingerIndex;
                    this.buttonState = ButtonState.Down;
                    this.frame = 0;
                    this.RaiseEvent(MessageName.On_ButtonDown);
                }
            }
            else if ((this.isSwipeIn || !this.isSwipeIn) && (!this.isSwipeOut && (this.buttonState == ButtonState.Press)))
            {
                this.buttonFingerIndex = -1;
                this.currentTexture = this.normalTexture;
                this.currentColor = this.buttonNormalColor;
                this.buttonState = ButtonState.None;
            }
            else if (this.isSwipeOut && (this.buttonState == ButtonState.Press))
            {
                this.RaiseEvent(MessageName.On_ButtonPress);
                this.buttonState = ButtonState.Press;
            }
        }
    }

    private void On_TouchStart(Gesture gesture)
    {
        if ((gesture.IsInRect(VirtualScreen.GetRealRect(this.buttonRect), true) && this.enable) && this.isActivated)
        {
            this.buttonFingerIndex = gesture.fingerIndex;
            this.currentTexture = this.activeTexture;
            this.currentColor = this.buttonActiveColor;
            this.buttonState = ButtonState.Down;
            this.frame = 0;
            this.RaiseEvent(MessageName.On_ButtonDown);
        }
    }

    private void On_TouchUp(Gesture gesture)
    {
        if (gesture.fingerIndex == this.buttonFingerIndex)
        {
            if ((gesture.IsInRect(VirtualScreen.GetRealRect(this.buttonRect), true) || (this.isSwipeOut && (this.buttonState == ButtonState.Press))) && (this.enable && this.isActivated))
            {
                this.RaiseEvent(MessageName.On_ButtonUp);
            }
            this.buttonState = ButtonState.Up;
            this.buttonFingerIndex = -1;
            this.currentTexture = this.normalTexture;
            this.currentColor = this.buttonNormalColor;
        }
    }

    private void OnDestroy()
    {
        EasyTouch.On_TouchStart -= new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.On_TouchUp);
        if (Application.isPlaying && (EasyTouch.instance != null))
        {
            EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
        }
    }

    private void OnDisable()
    {
        EasyTouch.On_TouchStart -= new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.On_TouchUp);
        if (Application.isPlaying && (EasyTouch.instance != null))
        {
            EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
        }
    }

    private void OnDrawGizmos()
    {
    }

    private void OnEnable()
    {
        EasyTouch.On_TouchStart += new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchDown += new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchUp += new EasyTouch.TouchUpHandler(this.On_TouchUp);
    }

    private void OnGUI()
    {
        if (this.enable)
        {
            GUI.depth = this.guiDepth;
            base.useGUILayout = this.isUseGuiLayout;
            VirtualScreen.ComputeVirtualScreen();
            VirtualScreen.SetGuiScaleMatrix();
            if ((this.normalTexture != null) && (this.activeTexture != null))
            {
                this.ComputeButtonAnchor(this.anchor);
                if (this.normalTexture != null)
                {
                    if (Application.isEditor && !Application.isPlaying)
                    {
                        this.currentTexture = this.normalTexture;
                    }
                    if ((this.showDebugArea && Application.isEditor) && (this.selected && !Application.isPlaying))
                    {
                        GUI.Box(this.buttonRect, string.Empty);
                    }
                    if (this.currentTexture != null)
                    {
                        if (this.isActivated)
                        {
                            GUI.color = this.currentColor;
                            if (Application.isPlaying)
                            {
                                EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
                                EasyTouch.instance.reservedVirtualAreas.Add(this.buttonRect);
                            }
                        }
                        else
                        {
                            GUI.color = new Color(this.currentColor.r, this.currentColor.g, this.currentColor.b, 0.2f);
                            if (Application.isPlaying)
                            {
                                EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
                            }
                        }
                        GUI.DrawTexture(this.buttonRect, this.currentTexture);
                        GUI.color = Color.white;
                    }
                }
            }
        }
        else if (Application.isPlaying)
        {
            EasyTouch.instance.reservedVirtualAreas.Remove(this.buttonRect);
        }
    }

    private void RaiseEvent(MessageName msg)
    {
        if (this.interaction == InteractionType.Event)
        {
            if (!this.useBroadcast)
            {
                switch (msg)
                {
                    case MessageName.On_ButtonDown:
                        if (On_ButtonDown != null)
                        {
                            On_ButtonDown(base.gameObject.name);
                        }
                        break;

                    case MessageName.On_ButtonPress:
                        if (On_ButtonPress != null)
                        {
                            On_ButtonPress(base.gameObject.name);
                        }
                        break;

                    case MessageName.On_ButtonUp:
                        if (On_ButtonUp != null)
                        {
                            On_ButtonUp(base.gameObject.name);
                        }
                        break;
                }
            }
            else
            {
                string methodName = msg.ToString();
                if (((msg == MessageName.On_ButtonDown) && (this.downMethodName != string.Empty)) && this.useSpecificalMethod)
                {
                    methodName = this.downMethodName;
                }
                if (((msg == MessageName.On_ButtonPress) && (this.pressMethodName != string.Empty)) && this.useSpecificalMethod)
                {
                    methodName = this.pressMethodName;
                }
                if (((msg == MessageName.On_ButtonUp) && (this.upMethodName != string.Empty)) && this.useSpecificalMethod)
                {
                    methodName = this.upMethodName;
                }
                if (this.receiverGameObject != null)
                {
                    switch (this.messageMode)
                    {
                        case Broadcast.SendMessage:
                            this.receiverGameObject.SendMessage(methodName, base.name, SendMessageOptions.DontRequireReceiver);
                            break;

                        case Broadcast.SendMessageUpwards:
                            this.receiverGameObject.SendMessageUpwards(methodName, base.name, SendMessageOptions.DontRequireReceiver);
                            break;

                        case Broadcast.BroadcastMessage:
                            this.receiverGameObject.BroadcastMessage(methodName, base.name, SendMessageOptions.DontRequireReceiver);
                            break;
                    }
                }
                else
                {
                    Debug.LogError("Button : " + base.gameObject.name + " : you must setup receiver gameobject");
                }
            }
        }
    }

    private void Start()
    {
        this.currentTexture = this.normalTexture;
        this.currentColor = this.buttonNormalColor;
        this.buttonState = ButtonState.None;
        VirtualScreen.ComputeVirtualScreen();
        this.ComputeButtonAnchor(this.anchor);
    }

    private void Update()
    {
        if (this.buttonState == ButtonState.Up)
        {
            this.buttonState = ButtonState.None;
        }
        if (EasyTouch.GetTouchCount() == 0)
        {
            this.buttonFingerIndex = -1;
            this.currentTexture = this.normalTexture;
            this.currentColor = this.buttonNormalColor;
            this.buttonState = ButtonState.None;
        }
    }

    public Texture2D ActiveTexture
    {
        get
        {
            return this.activeTexture;
        }
        set
        {
            this.activeTexture = value;
        }
    }

    public ButtonAnchor Anchor
    {
        get
        {
            return this.anchor;
        }
        set
        {
            this.anchor = value;
            this.ComputeButtonAnchor(this.anchor);
        }
    }

    public Texture2D NormalTexture
    {
        get
        {
            return this.normalTexture;
        }
        set
        {
            this.normalTexture = value;
            if (this.normalTexture != null)
            {
                this.ComputeButtonAnchor(this.anchor);
                this.currentTexture = this.normalTexture;
            }
        }
    }

    public Vector2 Offset
    {
        get
        {
            return this.offset;
        }
        set
        {
            this.offset = value;
            this.ComputeButtonAnchor(this.anchor);
        }
    }

    public Vector2 Scale
    {
        get
        {
            return this.scale;
        }
        set
        {
            this.scale = value;
            this.ComputeButtonAnchor(this.anchor);
        }
    }

    public enum Broadcast
    {
        SendMessage,
        SendMessageUpwards,
        BroadcastMessage
    }

    public enum ButtonAnchor
    {
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

    public delegate void ButtonDownHandler(string buttonName);

    public delegate void ButtonPressHandler(string buttonName);

    public enum ButtonState
    {
        Down,
        Press,
        Up,
        None
    }

    public delegate void ButtonUpHandler(string buttonName);

    public enum InteractionType
    {
        Event,
        Include
    }

    private enum MessageName
    {
        On_ButtonDown,
        On_ButtonPress,
        On_ButtonUp
    }
}

