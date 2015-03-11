using System;
using UnityEngine;

public class UIMouseClick : MonoBehaviour
{
    public Action<object> click;
    private float timeLast;
    public object userState;

    private void OnClick()
    {
        if ((this.timeLast + 0.05f) <= Time.time)
        {
            this.timeLast = Time.time;
            if (this.click != null)
            {
                this.click(this.userState);
            }
        }
    }
}

