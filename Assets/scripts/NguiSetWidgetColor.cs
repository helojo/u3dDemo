using HutongGames.PlayMaker;
using System;

[Tooltip("Sets the color value of a widget"), ActionCategory("NGUI")]
public class NguiSetWidgetColor : FsmStateAction
{
    [RequiredField, Tooltip("The new color to assign to the widget")]
    public FsmColor color;
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [Tooltip("NGUI Widget to update"), RequiredField]
    public FsmOwnerDefault NguiWidget;

    private void DoSetWidgetColor()
    {
        if ((this.NguiWidget != null) && (this.color != null))
        {
            UIWidget component = base.Fsm.GetOwnerDefaultTarget(this.NguiWidget).GetComponent<UIWidget>();
            if (component != null)
            {
                component.color = this.color.Value;
            }
        }
    }

    public override void OnEnter()
    {
        this.DoSetWidgetColor();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoSetWidgetColor();
    }

    public override void Reset()
    {
        this.NguiWidget = null;
        this.color = null;
        this.everyFrame = false;
    }
}

