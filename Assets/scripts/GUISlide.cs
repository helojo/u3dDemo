using System;
using UnityEngine;

public class GUISlide : GUIEntityAnimation
{
    protected Vector3 targetPos = Vector3.zero;
    protected Vector3 targetScale = Vector3.one;

    public override void Begin()
    {
        GUIEntity component = base.GetComponent<GUIEntity>();
        if (null != component)
        {
            base.nameAnim = "gui_animation_slide_" + component.entity_id.ToString();
        }
        this.targetPos = base.transform.position;
        this.targetScale = Vector3.one;
        base.transform.localPosition = new Vector3(GUIMgr.Instance.Root.manualWidth * 1.5f, 130f, this.targetPos.z);
        base.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    public override void Commit()
    {
        base.Active = true;
        TweenPosition position = TweenPosition.Begin(base.gameObject, (base.duration * 2f) / base.speed, this.targetPos);
        position.method = UITweener.Method.EaseOutExpo;
        position.SetOnFinished((EventDelegate.Callback) null);
        TweenScale scale = TweenScale.Begin(base.gameObject, ((base.duration * 2f) / base.speed) * 1.2f, this.targetScale);
        scale.method = UITweener.Method.EaseOutExpo;
        scale.SetOnFinished((EventDelegate.Callback) (() => base.OnComplete()));
    }

    public override void Invert()
    {
        this.targetPos = new Vector3(GUIMgr.Instance.Root.manualWidth * 1.5f, 130f, base.transform.position.z);
        this.targetScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    public override void Reset()
    {
        this.Pause();
        base.transform.localPosition = new Vector3(GUIMgr.Instance.Root.manualWidth * 1.5f, 130f, this.targetPos.z);
        base.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }
}

