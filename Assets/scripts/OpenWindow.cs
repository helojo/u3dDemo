using System;
using UnityEngine;

public class OpenWindow : MonoBehaviour
{
    public string windowName;

    private void OnClick()
    {
        GUIMgr.Instance.PushGUIEntity(this.windowName, null);
        GUIMgr.Instance.FloatTitleBar();
    }
}

