using System;
using UnityEngine;

public class C_EasyButtonTemplate : MonoBehaviour
{
    private void On_ButtonDown(string buttonName)
    {
    }

    private void On_ButtonPress(string buttonName)
    {
    }

    private void On_ButtonUp(string buttonName)
    {
    }

    private void OnDestroy()
    {
        EasyButton.On_ButtonDown -= new EasyButton.ButtonDownHandler(this.On_ButtonDown);
        EasyButton.On_ButtonPress -= new EasyButton.ButtonPressHandler(this.On_ButtonPress);
        EasyButton.On_ButtonUp -= new EasyButton.ButtonUpHandler(this.On_ButtonUp);
    }

    private void OnDisable()
    {
        EasyButton.On_ButtonDown -= new EasyButton.ButtonDownHandler(this.On_ButtonDown);
        EasyButton.On_ButtonPress -= new EasyButton.ButtonPressHandler(this.On_ButtonPress);
        EasyButton.On_ButtonUp -= new EasyButton.ButtonUpHandler(this.On_ButtonUp);
    }

    private void OnEnable()
    {
        EasyButton.On_ButtonDown += new EasyButton.ButtonDownHandler(this.On_ButtonDown);
        EasyButton.On_ButtonPress += new EasyButton.ButtonPressHandler(this.On_ButtonPress);
        EasyButton.On_ButtonUp += new EasyButton.ButtonUpHandler(this.On_ButtonUp);
    }
}

