using System;
using UnityEngine;

public class GUITranspanetAnimation : GUIEntityAnimation
{
    private float alphaFrom;
    [HideInInspector]
    public float alphaMax = 1f;
    [HideInInspector]
    public float alphaMin;
    private float alphaTo = 1f;
    private UITweener.Method method;

    public override void Begin()
    {
        GUIEntity component = base.GetComponent<GUIEntity>();
        if (null != component)
        {
            base.nameAnim = "gui_animation_transparent_" + component.entity_id.ToString();
        }
        this.alphaFrom = this.alphaMin;
        this.alphaTo = this.alphaMax;
        this.method = UITweener.Method.EaseIn;
        UIPanel panel = base.GetComponent<UIPanel>();
        if (null != panel)
        {
            panel.alpha = this.alphaFrom;
        }
    }

    public override void Commit()
    {
        base.Active = true;
        TweenAlpha alpha = TweenAlpha.Begin(base.gameObject, base.duration / base.speed, this.alphaTo);
        alpha.method = this.method;
        alpha.SetOnFinished(new EventDelegate.Callback(this.OnComplete));
    }

    public override void Invert()
    {
        this.alphaFrom = this.alphaMax;
        this.alphaTo = this.alphaMax;
        this.method = UITweener.Method.EaseOut;
        UIPanel component = base.GetComponent<UIPanel>();
        if (null != component)
        {
            component.alpha = this.alphaFrom;
        }
    }

    public override void Reset()
    {
        this.Pause();
        UIPanel component = base.GetComponent<UIPanel>();
        if (null != component)
        {
            component.alpha = this.alphaMin;
        }
    }
}

