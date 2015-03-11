using HutongGames.PlayMaker;
using System;

[Tooltip("Displays the value from a slider or progressbar in an NGUI Label, as a percent"), ActionCategory("NGUI")]
public class NguiSliderValueToPercentLabel : FsmStateAction
{
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [RequiredField, Tooltip("NGUI label used to display the value")]
    public FsmGameObject NguiLabel;
    [RequiredField, Tooltip("NGUI slider or progressbar")]
    public FsmOwnerDefault NguiSlider;
    [Tooltip("Variable to store the slider's value"), UIHint(UIHint.Variable)]
    public FsmFloat storeValue;

    private void DoReadSlider()
    {
        if ((this.NguiLabel != null) && (this.NguiSlider != null))
        {
            UISlider component = base.Fsm.GetOwnerDefaultTarget(this.NguiSlider).GetComponent<UISlider>();
            UILabel label = this.NguiLabel.Value.GetComponent<UILabel>();
            if ((component != null) && (label != null))
            {
                label.text = string.Format("{0:P}", component.value);
                if (this.storeValue != null)
                {
                    this.storeValue.Value = component.value;
                }
            }
        }
    }

    public override void OnEnter()
    {
        this.DoReadSlider();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoReadSlider();
    }

    public override void Reset()
    {
        this.NguiSlider = null;
        this.NguiLabel = null;
        this.storeValue = null;
        this.everyFrame = false;
    }
}

