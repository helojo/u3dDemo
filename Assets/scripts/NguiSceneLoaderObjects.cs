using HutongGames.PlayMaker;
using System;

[Tooltip("Sets the list of NGUI objects used when loading a scene"), ActionCategory("NGUI")]
public class NguiSceneLoaderObjects : FsmStateAction
{
    [Tooltip("NGUI progressbar to display during scene load"), RequiredField]
    public FsmGameObject NguiProgressbar;
    [Tooltip("OPTIONAL - Label to display on the progressbar to show text of progress")]
    public FsmGameObject NguiProgressbarLabel;
    [RequiredField, Tooltip("NGUI window which holds the NGUI controls used during scene load")]
    public FsmGameObject NguiWindow;
    [Tooltip("The variable to hold the NGUI window"), UIHint(UIHint.Variable), RequiredField]
    public FsmGameObject PmNguiWindow;
    [RequiredField, Tooltip("The variable to hold the progressbar"), UIHint(UIHint.Variable)]
    public FsmGameObject PmProgressbar;
    [Tooltip("The variable to hold the progressbar label"), UIHint(UIHint.Variable)]
    public FsmGameObject PmProgressbarLabel;

    private void DoSetNGUI()
    {
        this.PmProgressbar.Value = this.NguiProgressbar.Value;
        this.PmNguiWindow.Value = this.NguiWindow.Value;
        if (this.NguiProgressbarLabel != null)
        {
            this.PmProgressbarLabel.Value = this.NguiProgressbarLabel.Value;
        }
    }

    public override void OnEnter()
    {
        this.DoSetNGUI();
        base.Finish();
    }

    public override void Reset()
    {
        this.NguiWindow = null;
        this.PmNguiWindow = null;
        this.NguiProgressbar = null;
        this.NguiProgressbarLabel = null;
        this.PmProgressbar = null;
        this.PmProgressbarLabel = null;
    }
}

