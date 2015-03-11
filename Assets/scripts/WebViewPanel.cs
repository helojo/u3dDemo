using System;
using UnityEngine;

public class WebViewPanel : GUIEntity
{
    public Transform _BottomLeft;
    public Transform _BottomRight;
    public Transform _TopLeft;
    public Transform _TopRight;
    public GameObject _WebViewObject;

    public void Load(string url)
    {
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }
}

