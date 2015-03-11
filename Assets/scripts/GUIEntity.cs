using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public abstract class GUIEntity : MonoBehaviour
{
    private PlayMakerFSM _fsm;
    private UIPanel _panel;
    public System.Action actClose;
    [HideInInspector]
    public int entity_id = -1;
    public bool FadeMask = true;
    public int groupID = -1;
    private bool isHidden;
    private bool isReady;
    public bool multiCamera;

    protected GUIEntity()
    {
    }

    public T Achieve<T>() where T: GUIEntity
    {
        return (T) this;
    }

    public T Animate<T>() where T: GUIAnimation
    {
        T component = base.gameObject.GetComponent<T>();
        if (null == component)
        {
            component = base.gameObject.AddComponent<T>();
        }
        component.Reset();
        return component;
    }

    public void AutoSetChildPanelDepth(UIPanel parentPnl, UIScrollView childListPanel)
    {
        UIPanel component = childListPanel.gameObject.GetComponent<UIPanel>();
        if (component != null)
        {
            component.depth = parentPnl.depth + 1;
        }
    }

    public void AutoSetOneUIAllPanelsDepth(UIPanel pnl, int depth)
    {
        this.ReCalculateDepth(pnl, depth);
    }

    public void BlendAnimations(System.Action actCompleted, bool invert = false, float speed = 1f)
    {
        <BlendAnimations>c__AnonStorey16A storeya = new <BlendAnimations>c__AnonStorey16A {
            actCompleted = actCompleted,
            <>f__this = this
        };
        this.Flexible(true);
        Component[] components = base.GetComponents<GUIAnimation>();
        float duration = 0f;
        GUIAnimation animation = null;
        foreach (Component component in components)
        {
            GUIAnimation animation2 = component as GUIAnimation;
            if (null != animation2)
            {
                animation2.speed = speed;
                if (animation2.duration >= duration)
                {
                    duration = animation2.duration;
                    animation = animation2;
                }
                if (invert)
                {
                    animation2.Invert();
                }
                else
                {
                    animation2.Begin();
                }
            }
        }
        if (null != animation)
        {
            animation.actCompleted = new System.Action(storeya.<>m__166);
        }
        else
        {
            this.Flexible(false);
            if (storeya.actCompleted != null)
            {
                storeya.actCompleted();
            }
        }
        foreach (Component component2 in components)
        {
            GUIAnimation animation3 = component2 as GUIAnimation;
            if (null != animation3)
            {
                animation3.Commit();
            }
        }
    }

    private int CalculateAbsoluteDepth(UIPanel pnl)
    {
        int depth = pnl.depth;
        foreach (UIPanel panel in pnl.GetComponentsInChildren<UIPanel>())
        {
            if (panel != pnl)
            {
                depth = Mathf.Max(depth, this.CalculateAbsoluteDepth(panel));
            }
        }
        return depth;
    }

    public void DeSerialization(GUIPersistence pers)
    {
        this.SendFsmEvent(GUI_FSM_EVENT.ON_GUI_DESERIALIZATION);
        pers.Seek2Begin();
        this.OnDeSerialization(pers);
    }

    public T FindChild<T>(string name) where T: Component
    {
        return base.transform.FindChild<T>(name);
    }

    public void FixedUpdate()
    {
    }

    public void Flexible(bool flx)
    {
        foreach (Component component in base.GetComponentsInChildren<UIAnchor>())
        {
            UIAnchor anchor = component as UIAnchor;
            if (null != anchor)
            {
                anchor.Flexible = flx;
            }
        }
    }

    public virtual void GUIStart()
    {
    }

    public void LateUpdate()
    {
        if (this.isReady)
        {
            this.OnLateUpdate();
        }
    }

    public virtual void OnDeSerialization(GUIPersistence pers)
    {
    }

    public virtual void OnDestroy()
    {
    }

    public virtual void OnInitialize()
    {
    }

    public virtual void OnLateUpdate()
    {
    }

    public virtual void OnPrepareAnimate()
    {
    }

    public virtual void OnRelease()
    {
    }

    public virtual void OnReset()
    {
    }

    public virtual void OnSerialization(GUIPersistence pers)
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnUpdateUIData()
    {
    }

    public void Ready()
    {
        this.isReady = true;
    }

    private int ReCalculateDepth(UIPanel pnl, int depth)
    {
        pnl.depth = depth;
        List<UIPanel> list = new List<UIPanel>(pnl.GetComponentsInChildren<UIPanel>(true));
        list.Sort(new CompareBrotherPanel());
        int b = depth;
        foreach (UIPanel panel in list)
        {
            if (panel != pnl)
            {
                b = Mathf.Max(this.ReCalculateDepth(panel, b + 1), b);
            }
        }
        return b;
    }

    public void ResetAnimations()
    {
        this.Flexible(true);
        foreach (Component component in base.GetComponents<GUINavigable>())
        {
            GUINavigable navigable = component as GUINavigable;
            if (null != navigable)
            {
                navigable.Reset();
            }
        }
        foreach (Component component2 in base.GetComponents<GUIAnimation>())
        {
            GUIAnimation animation = component2 as GUIAnimation;
            if (null != animation)
            {
                animation.Reset();
            }
        }
        this.Flexible(false);
    }

    public void ResetDepth()
    {
        this.Depth = this.Depth;
    }

    public void SendFsmEvent(GUI_FSM_EVENT evn)
    {
        this.FSM.SendEvent(FsmEventUnitility.Enum2FsmEventName(evn));
    }

    public void Serialization(GUIPersistence pers)
    {
        if (this.actClose != null)
        {
            this.actClose();
        }
        this.actClose = null;
        this.SendFsmEvent(GUI_FSM_EVENT.ON_GUI_SERIALIZATION);
        pers.Reset();
        this.OnSerialization(pers);
    }

    private void Start()
    {
        base.transform.parent = GUIMgr.Instance.ActivityCamera.transform;
        base.transform.localScale = Vector3.one;
        this.GUIStart();
    }

    public void Update()
    {
        if (this.isReady)
        {
            this.OnUpdate();
        }
    }

    public int AbsoluteDepth
    {
        get
        {
            return this.CalculateAbsoluteDepth(this.NGUIPanel);
        }
    }

    public float AnimationsDuration
    {
        get
        {
            float a = 0f;
            foreach (Component component in base.GetComponents<GUIAnimation>())
            {
                GUIAnimation animation = component as GUIAnimation;
                if (null != animation)
                {
                    a = Mathf.Max(a, animation.duration);
                }
            }
            return a;
        }
    }

    public int BasicRenderQueue
    {
        get
        {
            return this.NGUIPanel.startingRenderQueue;
        }
    }

    public int Depth
    {
        get
        {
            return this.NGUIPanel.depth;
        }
        set
        {
            this.ReCalculateDepth(this.NGUIPanel, value);
        }
    }

    public PlayMakerFSM FSM
    {
        get
        {
            if (null == this._fsm)
            {
                this._fsm = base.gameObject.GetComponent<PlayMakerFSM>();
                if (null == this._fsm)
                {
                    this._fsm = base.gameObject.AddComponent<PlayMakerFSM>();
                }
            }
            return this._fsm;
        }
    }

    public bool Hidden
    {
        get
        {
            return this.isHidden;
        }
        set
        {
            this.isHidden = value;
            NGUITools.SetActive(base.gameObject, !this.isHidden);
            UIPanel component = base.GetComponent<UIPanel>();
            if ((null != component) && !this.isHidden)
            {
                component.Refresh();
            }
        }
    }

    public UIPanel NGUIPanel
    {
        get
        {
            if (null == this._panel)
            {
                this._panel = base.gameObject.GetComponent<UIPanel>();
            }
            return this._panel;
        }
    }

    public int RelativeDepthOffset
    {
        get
        {
            return (this.AbsoluteDepth - this.Depth);
        }
    }

    public List<GUIEntity> SubEntities
    {
        get
        {
            return new List<GUIEntity>(base.GetComponentsInChildren<GUIEntity>(true)).FindAll(e => e != this);
        }
    }

    [CompilerGenerated]
    private sealed class <BlendAnimations>c__AnonStorey16A
    {
        internal GUIEntity <>f__this;
        internal System.Action actCompleted;

        internal void <>m__166()
        {
            this.<>f__this.Flexible(false);
            if (this.actCompleted != null)
            {
                this.actCompleted();
            }
        }
    }

    private class CompareBrotherPanel : IComparer<UIPanel>
    {
        public int Compare(UIPanel l, UIPanel r)
        {
            return (l.depth - r.depth);
        }
    }
}

