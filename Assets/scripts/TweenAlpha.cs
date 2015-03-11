using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
    [Range(0f, 1f)]
    public float from = 1f;
    private UIRect mRect;
    [Range(0f, 1f)]
    public float to = 1f;

    public static TweenAlpha Begin(GameObject go, float duration, float alpha)
    {
        TweenAlpha alpha2 = UITweener.Begin<TweenAlpha>(go, duration);
        alpha2.from = alpha2.value;
        alpha2.to = alpha;
        if (duration <= 0f)
        {
            alpha2.Sample(1f, true);
            alpha2.enabled = false;
        }
        return alpha2;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.value = Mathf.Lerp(this.from, this.to, factor);
    }

    public override void SetEndToCurrentValue()
    {
        this.to = this.value;
    }

    public override void SetStartToCurrentValue()
    {
        this.from = this.value;
    }

    [Obsolete("Use 'value' instead")]
    public float alpha
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

    public UIRect cachedRect
    {
        get
        {
            if (this.mRect == null)
            {
                this.mRect = base.GetComponent<UIRect>();
                if (this.mRect == null)
                {
                    this.mRect = base.GetComponentInChildren<UIRect>();
                }
            }
            return this.mRect;
        }
    }

    public float value
    {
        get
        {
            return ((null != this.cachedRect) ? this.cachedRect.alpha : this.from);
        }
        set
        {
            if (null != this.cachedRect)
            {
                this.cachedRect.alpha = value;
            }
            foreach (UITexture texture in base.GetComponentsInChildren<UITexture>())
            {
                texture.alpha = value;
            }
        }
    }
}

