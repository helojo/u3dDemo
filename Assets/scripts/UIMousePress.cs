using System;
using UnityEngine;

public class UIMousePress : MonoBehaviour
{
    private bool IsPressed;
    public Action<bool, object> Press;
    public object userState;

    private void OnPress(bool isDown)
    {
        this.IsPressed = isDown;
        if (this.Press != null)
        {
            this.Press(this.IsPressed, this.userState);
        }
    }

    public void Update()
    {
    }
}

