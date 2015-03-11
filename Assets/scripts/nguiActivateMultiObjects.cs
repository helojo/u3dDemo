using HutongGames.PlayMaker;
using System;

[ActionCategory("Ngui Actions"), Tooltip("Sets NGUI object to be activated or deactivated")]
public class nguiActivateMultiObjects : FsmStateAction
{
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [RequiredField, CompoundArray("How many", "Object", "setActive")]
    public FsmGameObject[] nguiObject;
    [Tooltip("Activate nGui GameObject. If False the game object will be Deactivated")]
    public FsmBool[] setActive;

    private void DoSetActive()
    {
        if ((this.nguiObject != null) && (this.nguiObject.Length >= 1))
        {
            for (int i = 0; i < this.nguiObject.Length; i++)
            {
                if (this.nguiObject[i].Value != null)
                {
                    NGUITools.SetActive(this.nguiObject[i].Value, this.setActive[i].Value);
                }
            }
        }
    }

    public override void OnEnter()
    {
        this.DoSetActive();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoSetActive();
    }

    public override void Reset()
    {
        this.nguiObject = null;
        this.setActive = null;
        this.everyFrame = false;
    }
}

