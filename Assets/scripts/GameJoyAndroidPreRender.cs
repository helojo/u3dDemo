using System;
using UnityEngine;

public class GameJoyAndroidPreRender : MonoBehaviour
{
    private bool hasPrintLog;

    private void OnPreRender()
    {
        int num = GameJoySDK.getGameJoyInstance().BeginDraw();
        if (!this.hasPrintLog)
        {
            Debug.Log("GameJoy on pre render | result = " + num);
            this.hasPrintLog = true;
        }
    }
}

