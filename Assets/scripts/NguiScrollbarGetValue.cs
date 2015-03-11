using HutongGames.PlayMaker;
using System;

[ActionCategory("NGUI"), Tooltip("Saves the value of a scrollbar to a variable")]
public class NguiScrollbarGetValue : FsmStateAction
{
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [Tooltip("NGUI scrollbar"), RequiredField]
    public FsmOwnerDefault NguiScrollbar;
    [Tooltip("Variable to store the scrollbar's value"), RequiredField, UIHint(UIHint.Variable)]
    public FsmFloat storeValue;

    private void DoReadScrollbar()
    {
        if ((this.NguiScrollbar != null) && (this.storeValue != null))
        {
            UIScrollBar component = base.Fsm.GetOwnerDefaultTarget(this.NguiScrollbar).GetComponent<UIScrollBar>();
            if (component != null)
            {
                this.storeValue.Value = component.value;
            }
        }
    }

    public override void OnEnter()
    {
        this.DoReadScrollbar();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoReadScrollbar();
    }

    public override void Reset()
    {
        this.NguiScrollbar = null;
        this.storeValue = null;
        this.everyFrame = false;
    }
}

