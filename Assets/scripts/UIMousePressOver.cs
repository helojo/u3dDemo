using System;
using UnityEngine;

public class UIMousePressOver : MonoBehaviour
{
    private bool IsPressed;
    public Action<object> Press;
    public object userState;

    private void OnPress(bool isDown)
    {
        this.IsPressed = isDown;
    }

    public void Update()
    {
        if (this.IsPressed && (this.Press != null))
        {
            this.Press(this.userState);
        }
    }
}

