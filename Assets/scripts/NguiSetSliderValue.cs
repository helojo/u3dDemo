using HutongGames.PlayMaker;
using System;

[Tooltip("Sets the value of an NGUI progressbar or Slider"), ActionCategory("NGUI")]
public class NguiSetSliderValue : FsmStateAction
{
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [RequiredField, Tooltip("NGUI slider or progressbar to update")]
    public FsmOwnerDefault NguiSlider;
    [Tooltip("Save the value to a variable"), UIHint(UIHint.Variable)]
    public FsmFloat saveValue;
    [RequiredField, Tooltip("The new value to assign to the slider progressbar")]
    public FsmFloat value;

    private void DoSetSliderValue()
    {
        if ((this.NguiSlider != null) && (this.value != null))
        {
            UISlider component = base.Fsm.GetOwnerDefaultTarget(this.NguiSlider).GetComponent<UISlider>();
            if (component != null)
            {
                component.value = this.value.Value;
                if (this.saveValue != null)
                {
                    this.saveValue.Value = this.value.Value;
                }
            }
        }
    }

    public override void OnEnter()
    {
        this.DoSetSliderValue();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoSetSliderValue();
    }

    public override void Reset()
    {
        this.NguiSlider = null;
        this.value = null;
        this.saveValue = null;
        this.everyFrame = false;
    }
}

