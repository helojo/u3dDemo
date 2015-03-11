using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
    public Action action;
    public KeyCode keyCode;
    private bool mIgnoreUp;
    private bool mIsInput;
    public Modifier modifier;
    private bool mPress;

    private bool IsModifierActive()
    {
        if (this.modifier == Modifier.None)
        {
            return true;
        }
        if (this.modifier == Modifier.Alt)
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                return true;
            }
        }
        else if (this.modifier == Modifier.Control)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                return true;
            }
        }
        else if ((this.modifier == Modifier.Shift) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            return true;
        }
        return false;
    }

    private void OnSubmit()
    {
        if ((UICamera.currentKey == this.keyCode) && this.IsModifierActive())
        {
            this.mIgnoreUp = true;
        }
    }

    private void Start()
    {
        UIInput component = base.GetComponent<UIInput>();
        this.mIsInput = component != null;
        if (component != null)
        {
            EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
        }
    }

    private void Update()
    {
        if ((this.keyCode != KeyCode.None) && this.IsModifierActive())
        {
            if (this.action == Action.PressAndClick)
            {
                if (!UICamera.inputHasFocus)
                {
                    UICamera.currentTouch = UICamera.controller;
                    UICamera.currentScheme = UICamera.ControlScheme.Mouse;
                    UICamera.currentTouch.current = base.gameObject;
                    if (Input.GetKeyDown(this.keyCode))
                    {
                        this.mPress = true;
                        UICamera.Notify(base.gameObject, "OnPress", true);
                    }
                    if (Input.GetKeyUp(this.keyCode))
                    {
                        UICamera.Notify(base.gameObject, "OnPress", false);
                        if (this.mPress)
                        {
                            UICamera.Notify(base.gameObject, "OnClick", null);
                            this.mPress = false;
                        }
                    }
                    UICamera.currentTouch.current = null;
                }
            }
            else if ((this.action == Action.Select) && Input.GetKeyUp(this.keyCode))
            {
                if (this.mIsInput)
                {
                    if (!this.mIgnoreUp && !UICamera.inputHasFocus)
                    {
                        UICamera.selectedObject = base.gameObject;
                    }
                    this.mIgnoreUp = false;
                }
                else
                {
                    UICamera.selectedObject = base.gameObject;
                }
            }
        }
    }

    public enum Action
    {
        PressAndClick,
        Select
    }

    public enum Modifier
    {
        None,
        Shift,
        Control,
        Alt
    }
}

