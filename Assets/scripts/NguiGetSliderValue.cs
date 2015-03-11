using HutongGames.PlayMaker;
using System;

[Tooltip("Gets the value of an NGUI progressbar or slider"), ActionCategory("NGUI")]
public class NguiGetSliderValue : FsmStateAction
{
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [Tooltip("NGUI slider or progressbar to update read"), RequiredField]
    public FsmOwnerDefault NguiSlider;
    [UIHint(UIHint.Variable), RequiredField, Tooltip("Save the value to a variable")]
    public FsmFloat saveValue;

    private void DoGetSliderValue()
    {
        if ((this.NguiSlider != null) && (this.saveValue != null))
        {
            UISlider component = base.Fsm.GetOwnerDefaultTarget(this.NguiSlider).GetComponent<UISlider>();
            if (component != null)
            {
                this.saveValue.Value = component.value;
            }
        }
    }

    public override void OnEnter()
    {
        this.DoGetSliderValue();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoGetSliderValue();
    }

    public override void Reset()
    {
        this.NguiSlider = null;
        this.saveValue = null;
        this.everyFrame = false;
    }
}

