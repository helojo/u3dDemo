using System;
using UnityEngine;

public abstract class GUIAnimation : MonoBehaviour
{
    [HideInInspector]
    public System.Action actCompleted;
    private bool activeAnim;
    [HideInInspector]
    public float duration;
    protected string nameAnim = "unknown_gui_anim";
    [HideInInspector]
    public float speed = 1f;

    protected GUIAnimation()
    {
    }

    public abstract void Begin();
    public abstract void Commit();
    public abstract void Invert();
    protected void OnComplete()
    {
        this.Active = false;
        if (this.actCompleted != null)
        {
            this.actCompleted();
        }
        this.actCompleted = null;
    }

    private void OnDestory()
    {
        this.Pause();
        if (this.Active && (this.actCompleted != null))
        {
            this.actCompleted();
        }
    }

    public abstract void Pause();
    public abstract void Reset();

    protected bool Active
    {
        get
        {
            return this.activeAnim;
        }
        set
        {
            this.activeAnim = value;
            UIPanel component = base.GetComponent<UIPanel>();
            if (null != component)
            {
                component.widgetsAreStatic = value;
            }
        }
    }
}

