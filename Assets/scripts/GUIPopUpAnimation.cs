using System;
using UnityEngine;

public class GUIPopUpAnimation : GUIEntityAnimation
{
    protected iTween.EaseType easeType = iTween.EaseType.spring;
    protected Vector3 targetScale;
    protected float time;

    public override void Begin()
    {
        GUIEntity component = base.GetComponent<GUIEntity>();
        if (null != component)
        {
            base.nameAnim = "gui_animation_popup_" + component.entity_id.ToString();
        }
        this.targetScale = Vector3.one;
        base.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        this.easeType = iTween.EaseType.easeOutBack;
        this.time = base.duration / base.speed;
    }

    public override void Commit()
    {
        base.Active = true;
        object[] args = new object[] { "islocal", false, "name", base.nameAnim, "scale", this.targetScale, "time", this.time, "easetype", this.easeType, "oncompletetarget", base.gameObject, "oncomplete", "OnComplete" };
        iTween.ScaleTo(base.gameObject, iTween.Hash(args));
    }

    public override void Invert()
    {
        this.targetScale = new Vector3(0.001f, 0.001f, 0.001f);
        this.easeType = iTween.EaseType.easeInSine;
        this.time = base.duration / base.speed;
    }

    public override void Reset()
    {
        this.Pause();
        base.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }
}

