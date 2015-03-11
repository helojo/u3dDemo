using HutongGames.PlayMaker;
using System;

[Tooltip("Sets Multiple NGUI objects as Active or Inactive"), ActionCategory("NGUI")]
public class NguiSetActiveMulti : FsmStateAction
{
    [Tooltip("Active state to set these objects"), RequiredField]
    public FsmBool active;
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [RequiredField, Tooltip("List of NGUI objects to set")]
    public FsmGameObject[] NguiObjects;

    private void DoSetActiveState()
    {
        if (((this.NguiObjects != null) && (this.NguiObjects.Length != 0)) && (this.active != null))
        {
            foreach (FsmGameObject obj2 in this.NguiObjects)
            {
                NGUITools.SetActive(obj2.Value, this.active.Value);
            }
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
        this.NguiObjects = null;
        this.active = null;
        this.everyFrame = false;
    }
}

