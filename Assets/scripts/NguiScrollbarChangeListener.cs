using HutongGames.PlayMaker;
using System;

[Tooltip("Listens for the scrollbar to change"), ActionCategory("NGUI")]
public class NguiScrollbarChangeListener : FsmStateAction
{
    [Tooltip("Event to raise when scrollbar changes")]
    public FsmEvent ChangeEvent;
    private bool HasChanged;
    [Tooltip("NGUI scrollbar"), RequiredField]
    public FsmOwnerDefault NguiScrollbar;
    [UIHint(UIHint.Variable), Tooltip("Variable to store the scrollbar's value")]
    public FsmFloat storeValue;

    private void DoReadScrollbar()
    {
        if ((this.NguiScrollbar != null) && (this.ChangeEvent != null))
        {
            UIScrollBar component = base.Fsm.GetOwnerDefaultTarget(this.NguiScrollbar).GetComponent<UIScrollBar>();
            if (this.storeValue != null)
            {
                this.storeValue.Value = component.value;
            }
            base.Fsm.Event(this.ChangeEvent);
        }
    }

    private void DragFinished()
    {
        UIScrollBar component = base.Fsm.GetOwnerDefaultTarget(this.NguiScrollbar).GetComponent<UIScrollBar>();
        component.onDragFinished = (UIProgressBar.OnDragFinished) Delegate.Remove(component.onDragFinished, new UIProgressBar.OnDragFinished(this.DragFinished));
        this.HasChanged = true;
    }

    public override void OnEnter()
    {
        if (this.NguiScrollbar != null)
        {
            UIScrollBar component = base.Fsm.GetOwnerDefaultTarget(this.NguiScrollbar).GetComponent<UIScrollBar>();
            if (component != null)
            {
                component.onDragFinished = (UIProgressBar.OnDragFinished) Delegate.Combine(component.onDragFinished, new UIProgressBar.OnDragFinished(this.DragFinished));
            }
        }
    }

    public override void OnUpdate()
    {
        if (this.HasChanged)
        {
            this.DoReadScrollbar();
            base.Finish();
        }
    }

    public override void Reset()
    {
        this.NguiScrollbar = null;
        this.storeValue = null;
        this.HasChanged = false;
        this.ChangeEvent = null;
    }
}

