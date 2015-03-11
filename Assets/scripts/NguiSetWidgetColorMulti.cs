using HutongGames.PlayMaker;
using System;

[Tooltip("Sets the color value of multiple widgets to the same color"), ActionCategory("NGUI")]
public class NguiSetWidgetColorMulti : FsmStateAction
{
    [Tooltip("The new color to assign to the widgets"), RequiredField]
    public FsmColor color;
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [RequiredField, Tooltip("NGUI Widgets to update")]
    public FsmGameObject[] NguiWidgets;

    private void DoSetWidgetColor()
    {
        if (((this.NguiWidgets != null) && (this.NguiWidgets.Length != 0)) && (this.color != null))
        {
            int length = this.NguiWidgets.Length;
            for (int i = 0; i < length; i++)
            {
                UIWidget component = this.NguiWidgets[i].Value.GetComponent<UIWidget>();
                if (component != null)
                {
                    component.color = this.color.Value;
                }
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
        this.NguiWidgets = null;
        this.color = null;
        this.everyFrame = false;
    }
}

