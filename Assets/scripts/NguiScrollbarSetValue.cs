using HutongGames.PlayMaker;
using System;

[Tooltip("Sets the value (position) of the scrollbar"), ActionCategory("NGUI")]
public class NguiScrollbarSetValue : FsmStateAction
{
    [UIHint(UIHint.Variable), RequiredField, Tooltip("New scrollbar value")]
    public FsmFloat newValue;
    [RequiredField, Tooltip("NGUI scrollbar")]
    public FsmOwnerDefault NguiScrollbar;

    private void DoSetScrollbar()
    {
        if ((this.NguiScrollbar != null) && (this.newValue != null))
        {
            base.Fsm.GetOwnerDefaultTarget(this.NguiScrollbar).GetComponent<UIScrollBar>().value = this.newValue.Value;
        }
    }

    public override void OnEnter()
    {
        this.DoSetScrollbar();
        base.Finish();
    }

    public override void OnUpdate()
    {
    }

    public override void Reset()
    {
        this.NguiScrollbar = null;
        this.newValue = null;
    }
}

