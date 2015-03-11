using System;
using UnityEngine;

public class UILongPress : MonoBehaviour
{
    public Action<object> callBack;
    private float durtionTime;
    private bool hadPress;
    private bool isDownFlag;
    public object userState;

    private void OnPress(bool isDown)
    {
        if (isDown)
        {
            if (!this.isDownFlag)
            {
                this.isDownFlag = true;
                this.durtionTime = Time.time;
                this.hadPress = false;
            }
        }
        else
        {
            this.isDownFlag = false;
        }
    }

    private void Update()
    {
        if ((this.isDownFlag && !this.hadPress) && ((this.durtionTime + 1f) < Time.time))
        {
            if (this.callBack != null)
            {
                this.callBack(this.userState);
            }
            this.hadPress = true;
        }
    }
}

