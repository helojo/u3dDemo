using HutongGames.PlayMaker;
using System;

[ActionCategory("NGUI"), Tooltip("Saves the text in an NGUI Label to a variable")]
public class NguiGetInputText : FsmStateAction
{
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [Tooltip("NGUI Label to read"), RequiredField]
    public FsmOwnerDefault NguiLabel;
    [RequiredField, UIHint(UIHint.Variable), Tooltip("Variable to store the label's text")]
    public FsmString storeValue;

    private void DoReadLabel()
    {
        if ((this.NguiLabel != null) && (this.storeValue != null))
        {
            UILabel component = base.Fsm.GetOwnerDefaultTarget(this.NguiLabel).GetComponent<UILabel>();
            if (component != null)
            {
                this.storeValue.Value = component.text;
            }
        }
    }

    public override void OnEnter()
    {
        this.DoReadLabel();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoReadLabel();
    }

    public override void Reset()
    {
        this.NguiLabel = null;
        this.storeValue = null;
        this.everyFrame = false;
    }
}

