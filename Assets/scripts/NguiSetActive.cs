using HutongGames.PlayMaker;
using System;

[Tooltip("Sets an NGUI object as Active or Inactive"), ActionCategory("NGUI")]
public class NguiSetActive : FsmStateAction
{
    [Tooltip("Active state to make this object"), RequiredField]
    public FsmBool active;
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [RequiredField, Tooltip("NGUI object")]
    public FsmOwnerDefault NguiObject;

    private void DoSetActiveState()
    {
        if ((this.NguiObject != null) && (this.active != null))
        {
            NGUITools.SetActive(base.Fsm.GetOwnerDefaultTarget(this.NguiObject), this.active.Value);
        }
    }

    public override void OnEnter()
    {
        this.DoSetActiveState();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoSetActiveState();
    }

    public override void Reset()
    {
        this.NguiObject = null;
        this.active = null;
        this.everyFrame = false;
    }
}

