using HutongGames.PlayMaker;
using System;

[ActionCategory("NGUI"), Tooltip("Adds a new item to a popup list or menu")]
public class NguiAddPopupOption : FsmStateAction
{
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [Tooltip("The new menu option to add"), RequiredField]
    public FsmString NewOption;
    [Tooltip("NGUI Popup to use"), RequiredField]
    public FsmOwnerDefault NguiPopup;

    private void DoAddOption()
    {
        if ((this.NguiPopup != null) && (this.NewOption != null))
        {
            UIPopupList component = base.Fsm.GetOwnerDefaultTarget(this.NguiPopup).GetComponent<UIPopupList>();
            if (component != null)
            {
                component.items.Add(this.NewOption.Value);
            }
        }
    }

    public override void OnEnter()
    {
        this.DoAddOption();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoAddOption();
    }

    public override void Reset()
    {
        this.NguiPopup = null;
        this.NewOption = null;
        this.everyFrame = false;
    }
}

