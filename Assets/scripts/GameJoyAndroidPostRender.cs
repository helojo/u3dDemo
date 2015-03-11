using System;
using UnityEngine;

public class GameJoyAndroidPostRender : MonoBehaviour
{
    private bool hasPrintLog;

    private void OnPostRender()
    {
        int num = GameJoySDK.getGameJoyInstance().EndDraw();
        if (!this.hasPrintLog)
        {
            Debug.Log("GameJoy on post render | result=" + num);
            this.hasPrintLog = true;
        }
        if (num == 1)
        {
            Debug.Log("GameJoy on post render | result=" + num);
            int num2 = 0;
            Camera[] allCameras = Camera.allCameras;
            if (allCameras != null)
            {
                foreach (Camera camera in allCameras)
                {
                    if (camera != null)
                    {
                        camera.Render();
                        num2++;
                    }
                }
            }
            Debug.Log("GameJoy on post render camreas = " + num2);
        }
    }
}

