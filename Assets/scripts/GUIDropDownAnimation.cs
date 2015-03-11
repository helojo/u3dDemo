using System;
using UnityEngine;

public class GUIDropDownAnimation : GUIEntityAnimation
{
    protected Vector3 targetPos = Vector3.zero;

    public override void Begin()
    {
        GUIEntity component = base.GetComponent<GUIEntity>();
        if (null != component)
        {
            base.nameAnim = "gui_animation_dropdown_" + component.entity_id.ToString();
        }
        this.targetPos = base.transform.position;
        base.transform.localPosition = new Vector3(base.transform.localPosition.x, GUIMgr.Instance.Root.manualHeight * 1.5f, 200f);
    }

    public override void Commit()
    {
        base.Active = true;
        object[] args = new object[] { "islocal", false, "name", base.nameAnim, "position", this.targetPos, "time", base.duration / base.speed, "easetype", iTween.EaseType.spring, "oncompletetarget", base.gameObject, "oncomplete", "OnComplete" };
        iTween.MoveTo(base.gameObject, iTween.Hash(args));
    }

    public override void Invert()
    {
        this.targetPos = new Vector3(base.transform.position.x, GUIMgr.Instance.Root.manualHeight * 1.5f, 200f);
    }

    public override void Reset()
    {
        this.Pause();
        base.transform.localPosition = new Vector3(base.transform.localPosition.x, GUIMgr.Instance.Root.manualHeight * 1.5f, 200f);
    }
}

