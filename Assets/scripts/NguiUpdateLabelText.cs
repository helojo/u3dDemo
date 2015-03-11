using HutongGames.PlayMaker;
using System;

[ActionCategory("NGUI"), Tooltip("Updates the text in an NGUI Label")]
public class NguiUpdateLabelText : FsmStateAction
{
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    private string LastString = string.Empty;
    [UIHint(UIHint.Variable), RequiredField, Tooltip("New text for NGUI Label")]
    public FsmString newValue;
    [RequiredField, Tooltip("NGUI Label to update")]
    public FsmOwnerDefault NguiLabel;
    private UILabel NLabel;

    private void DoUpdateLabel()
    {
        if ((this.NguiLabel != null) && (this.newValue != null))
        {
            this.NLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabel).GetComponent<UILabel>();
            if ((this.NLabel != null) && !this.newValue.Value.Equals(this.LastString))
            {
                this.NLabel.text = this.newValue.Value;
                this.LastString = this.newValue.Value;
            }
        }
    }

    public override void OnEnter()
    {
        this.DoUpdateLabel();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoUpdateLabel();
    }

    public override void Reset()
    {
        this.NguiLabel = null;
        this.newValue = null;
        this.everyFrame = false;
    }
}

