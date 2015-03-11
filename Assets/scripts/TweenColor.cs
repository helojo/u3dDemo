using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
    public Color from = Color.white;
    private bool mCached;
    private Light mLight;
    private Material mMat;
    private UIWidget mWidget;
    public Color to = Color.white;

    public static TweenColor Begin(GameObject go, float duration, Color color)
    {
        TweenColor color2 = UITweener.Begin<TweenColor>(go, duration);
        color2.from = color2.value;
        color2.to = color;
        if (duration <= 0f)
        {
            color2.Sample(1f, true);
            color2.enabled = false;
        }
        return color2;
    }

    private void Cache()
    {
        this.mCached = true;
        this.mWidget = base.GetComponent<UIWidget>();
        Renderer renderer = base.renderer;
        if (renderer != null)
        {
            this.mMat = renderer.material;
        }
        this.mLight = base.light;
        if (((this.mWidget == null) && (this.mMat == null)) && (this.mLight == null))
        {
            this.mWidget = base.GetComponentInChildren<UIWidget>();
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = Color.Lerp(this.from, this.to, factor);
    }

    [ContextMenu("Assume value of 'To'")]
    private void SetCurrentValueToEnd()
    {
        this.value = this.to;
    }

    [ContextMenu("Assume value of 'From'")]
    private void SetCurrentValueToStart()
    {
        this.value = this.from;
    }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue()
    {
        this.to = this.value;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue()
    {
        this.from = this.value;
    }

    [Obsolete("Use 'value' instead")]
    public Color color
    {
        get
        {
            return this.value;
        }
        set
        {
            this.value = value;
        }
    }

    public Color value
    {
        get
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mWidget != null)
            {
                return this.mWidget.color;
            }
            if (this.mLight != null)
            {
                return this.mLight.color;
            }
            if (this.mMat != null)
            {
                return this.mMat.color;
            }
            return Color.black;
        }
        set
        {
            if (!this.mCached)
            {
                this.Cache();
            }
            if (this.mWidget != null)
            {
                this.mWidget.color = value;
            }
            if (this.mMat != null)
            {
                this.mMat.color = value;
            }
            if (this.mLight != null)
            {
                this.mLight.color = value;
                this.mLight.enabled = ((value.r + value.g) + value.b) > 0.01f;
            }
        }
    }
}

