using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/NGUI Slider")]
public class UISlider : UIProgressBar
{
    [HideInInspector, SerializeField]
    private Direction direction = Direction.Upgraded;
    [HideInInspector, SerializeField]
    private Transform foreground;
    [HideInInspector, SerializeField]
    protected bool mInverted;
    [SerializeField, HideInInspector]
    private float rawValue = 1f;

    protected void OnDragBackground(GameObject go, Vector2 delta)
    {
        if (UICamera.currentScheme != UICamera.ControlScheme.Controller)
        {
            base.mCam = UICamera.currentCamera;
            base.value = base.ScreenToValue(UICamera.lastTouchPosition);
        }
    }

    protected void OnDragForeground(GameObject go, Vector2 delta)
    {
        if (UICamera.currentScheme != UICamera.ControlScheme.Controller)
        {
            base.mCam = UICamera.currentCamera;
            base.value = base.mOffset + base.ScreenToValue(UICamera.lastTouchPosition);
        }
    }

    protected void OnKey(KeyCode key)
    {
        if (base.enabled)
        {
            float num = (base.numberOfSteps <= 1f) ? 0.125f : (1f / ((float) (base.numberOfSteps - 1)));
            if ((base.fillDirection == UIProgressBar.FillDirection.LeftToRight) || (base.fillDirection == UIProgressBar.FillDirection.RightToLeft))
            {
                if (key == KeyCode.LeftArrow)
                {
                    base.value = base.mValue - num;
                }
                else if (key == KeyCode.RightArrow)
                {
                    base.value = base.mValue + num;
                }
            }
            else if (key == KeyCode.DownArrow)
            {
                base.value = base.mValue - num;
            }
            else if (key == KeyCode.UpArrow)
            {
                base.value = base.mValue + num;
            }
        }
    }

    protected void OnPressBackground(GameObject go, bool isPressed)
    {
        if (UICamera.currentScheme != UICamera.ControlScheme.Controller)
        {
            base.mCam = UICamera.currentCamera;
            base.value = base.ScreenToValue(UICamera.lastTouchPosition);
            if (!isPressed && (base.onDragFinished != null))
            {
                base.onDragFinished();
            }
        }
    }

    protected void OnPressForeground(GameObject go, bool isPressed)
    {
        if (UICamera.currentScheme != UICamera.ControlScheme.Controller)
        {
            if (isPressed)
            {
                base.mOffset = (base.mFG != null) ? (base.value - base.ScreenToValue(UICamera.lastTouchPosition)) : 0f;
            }
            else if (base.onDragFinished != null)
            {
                base.onDragFinished();
            }
        }
    }

    protected override void OnStart()
    {
        GameObject go = ((base.mBG == null) || ((base.mBG.collider == null) && (base.mBG.GetComponent<Collider2D>() == null))) ? base.gameObject : base.mBG.gameObject;
        UIEventListener listener = UIEventListener.Get(go);
        listener.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
        listener.onDrag = (UIEventListener.VectorDelegate) Delegate.Combine(listener.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
        if (((base.thumb != null) && ((base.thumb.collider != null) || (base.thumb.GetComponent<Collider2D>() != null))) && ((base.mFG == null) || (base.thumb != base.mFG.cachedTransform)))
        {
            UIEventListener listener2 = UIEventListener.Get(base.thumb.gameObject);
            listener2.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener2.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
            listener2.onDrag = (UIEventListener.VectorDelegate) Delegate.Combine(listener2.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
        }
    }

    protected override void Upgrade()
    {
        if (this.direction != Direction.Upgraded)
        {
            base.mValue = this.rawValue;
            if (this.foreground != null)
            {
                base.mFG = this.foreground.GetComponent<UIWidget>();
            }
            if (this.direction == Direction.Horizontal)
            {
                base.mFill = !this.mInverted ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft;
            }
            else
            {
                base.mFill = !this.mInverted ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom;
            }
            this.direction = Direction.Upgraded;
        }
    }

    [Obsolete("Use 'fillDirection' instead")]
    public bool inverted
    {
        get
        {
            return base.isInverted;
        }
        set
        {
        }
    }

    [Obsolete("Use 'value' instead")]
    public float sliderValue
    {
        get
        {
            return base.value;
        }
        set
        {
            base.value = value;
        }
    }

    private enum Direction
    {
        Horizontal,
        Vertical,
        Upgraded
    }
}

