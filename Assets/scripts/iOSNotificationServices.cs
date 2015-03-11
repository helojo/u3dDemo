using System;
using UnityEngine;

public class iOSNotificationServices : MonoBehaviour
{
    private string ios_noification_url;
    private bool isInited;
    public static iOSNotificationServices mInstance;
    private bool tokenSent;

    private void Awake()
    {
        mInstance = this;
    }

    public void Init(string url)
    {
        this.isInited = true;
        this.ios_noification_url = url;
    }

    private void OnDestroy()
    {
        mInstance = null;
    }

    private void Start()
    {
    }
}

